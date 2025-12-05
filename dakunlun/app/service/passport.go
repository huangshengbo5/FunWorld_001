package service

import (
	"dakunlun/app/dao"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/util"
	"errors"

	"gorm.io/gorm"
)

type passportService struct {
}

var PassportService = new(passportService)

func (srv *passportService) ClearPassport(name string) error {
	if name == "" {
		return util.NewAppError(util.ErrorCodePassportNameIsEmpty)
	}

	passportEntity, err := srv.GetPassportByName(name)
	if err != nil {
		return err
	}

	if passportEntity != nil {
		err = dao.PassportDao.Delete(passportEntity)
	}

	if err != nil {
		return err
	}

	return nil
}

func (srv *passportService) LoginNative(name string, password string) (passportEntity *entity.PassportEntity,
	auth *Auth, isNew bool, err error) {
	if name == "" {
		err = util.NewAppError(util.ErrorCodePassportNameIsEmpty)
		return
	}

	passportEntity, err = srv.GetPassportByName(name)
	if err != nil {
		return
	}

	accountData, err := data.AccountDao.FetchByName(name)

	// 有账号则验证账号身上密码，否则验证白名单密码
	if passportEntity != nil {
		// 验证用户身上密码
		if passportEntity.Password != password {
			if accountData != nil {
				// 白名单存在切不符合
				if accountData.Password != password {
					err = util.NewAppError(util.ErrorCodePasswordIsInvalid)
					return
				}
			} else {
				// 白名单不存在
				err = util.NewAppError(util.ErrorCodePasswordIsInvalid)
				return
			}
		}
	} else {
		if err != nil {
			err = util.NewAppError(util.ErrorCodePasswordIsInvalid)
			return
		}

		if accountData != nil {
			if accountData.Password != password {
				err = util.NewAppError(util.ErrorCodePasswordIsInvalid)
				return
			}
		}
	}

	if accountData.Expire && accountData.ExpireDate != "" {
		if util.Carbon().Now().Gt(util.Carbon().Parse(accountData.ExpireDate)) {
			err = util.NewAppError(util.ErrorCodeAccountExpire)
			return
		}
	}

	if passportEntity == nil {
		isNew = true
		//需要重新注册
		passportEntity, err = srv.CreatePassport(name, password)
		if err != nil {
			return
		}

		//需要重新注册
		var userEntity *entity.UserEntity
		userEntity, err = UserService.CreateUser(passportEntity.ID, accountData)
		if err != nil {
			return
		}

		_, err = UserService.CreateUserExtend(passportEntity.ID)
		if err != nil {
			return
		}

		//_, err = UserBuildingService.InitBuildings(userEntity)
		//if err != nil {
		//	return
		//}

		_, err = UserHeroService.InitMainHero(userEntity)
		if err != nil {
			return
		}
	}

	auth, err = TokenService.GenerateToken(passportEntity.ID)

	return
}

func (srv *passportService) GetPassportByName(name string) (passportEntity *entity.PassportEntity, err error) {
	passportEntity, err = dao.PassportDao.FetchByName(name)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, nil
		}
	}
	return
}

func (srv *passportService) CreatePassport(name string, password string) (passportEntity *entity.PassportEntity,
	err error) {
	passportEntity = entity.NewPassport(name, password)
	err = dao.PassportDao.Create(passportEntity)
	return
}
