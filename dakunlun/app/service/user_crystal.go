package service

import (
	"dakunlun/app/dao"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type userCrystalService struct {
}

var UserCrystalService = new(userCrystalService)

// 添加水晶等级数据
func (srv *userCrystalService) AddCrystal(userEntity *entity.UserEntity, crystalID uint32) (crystalEntity *entity.UserCrystalEntity, err error) {
	crystalEntity, err = dao.UserCrystalDao.Create(userEntity.ID, crystalID, 0)
	return
}

// 获取用户水晶等级数据列表
func (srv *userCrystalService) GetCrystalsByUid(uid uint32) (userCrystalEntitys []*entity.UserCrystalEntity, err error) {
	userCrystalEntitys, err = dao.UserCrystalDao.FetchMultiByUid(uid)
	return
}

// 升级水晶
func (srv *userCrystalService) Upgrade(userEntity *entity.UserEntity, id uint32) (userCrystalEntity *entity.UserCrystalEntity, err error) {
	userCrystalEntity, err = dao.UserCrystalDao.FetchByID(id)
	if err != nil {
		return
	}

	if userEntity.ID != userCrystalEntity.Uid {
		err = util.NewAppError(util.ErrorCodeHack, "错误的用户ID")
		return
	}

	userCrystalEntity.Level += 1

	var crystalUpgradeData *entity.CrystalUpgradeData
	crystalUpgradeData, err = data.CrystalUpgradeDao.FetchCrystalIDAndLevel(userCrystalEntity.CrystalID, userCrystalEntity.Level)
	if err != nil {
		return
	}
	// 扣除资源
	err = UserService.DecrAssets(userEntity, crystalUpgradeData.CostType, crystalUpgradeData.CostSubType, crystalUpgradeData.CostVal)
	if err != nil {
		return
	}

	//效果记录
	switch crystalUpgradeData.EffectID {
	case entity.EffectIDHit:
		userEntity.Attr.HitCrystalPlus = crystalUpgradeData.EffectVal
	case entity.EffectIDDodge:
		userEntity.Attr.DodgeCrystalPlus = crystalUpgradeData.EffectVal
	case entity.EffectIDCritical:
		userEntity.Attr.CriticalCrystalPlus = crystalUpgradeData.EffectVal
	case entity.EffectIDTenacity:
		userEntity.Attr.TenacityCrystalPlus = crystalUpgradeData.EffectVal
	case entity.EffectIDBreak:
		userEntity.Attr.BreakCrystalPlus = crystalUpgradeData.EffectVal
	case entity.EffectIDImpregnable:
		userEntity.Attr.ImpregnableCrystalPlus = crystalUpgradeData.EffectVal
	case entity.EffectIDDefuse:
		userEntity.Attr.DefuseCrystalPlus = crystalUpgradeData.EffectVal
	}

	//扣除资源
	err = UserService.UpdateUser(userEntity)
	if err != nil {
		return
	}

	// 更新水晶等级
	err = srv.UpdateCrystal(userCrystalEntity)

	if err != nil {
		return
	}

	return
}

// 更新用户水晶数据
func (srv *userCrystalService) UpdateCrystal(userCrystalEntity *entity.UserCrystalEntity) (err error) {
	err = dao.UserCrystalDao.Update(userCrystalEntity)
	return
}
