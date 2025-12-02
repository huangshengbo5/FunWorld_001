package controller

import (
	"dakunlun/app/dao"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/service/data"
	"dakunlun/app/util"
	"errors"

	"github.com/gin-gonic/gin"
)

// @Summary 登录
// @Accept  json
// @Produce  json
// @Param name query string true "设备ID"
// @Param clear query bool true "是否清除账号"
// @Success 200 {object} msg.LoginResponse
// @Router /login/login/native [post]
func LoginNative(c *gin.Context) {
	req := &msg.LoginRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if req.Clear {
		err := service.PassportService.ClearPassport(req.Name)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	passportEntity, auth, isNew, err := service.PassportService.LoginNative(req.Name, req.Password)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(passportEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	needUpdate := false
	// 补发未领取的关卡奖励
	if userExtendEntity.CampaignOldID > 0 {
		campaignEntity, err := data.CampaignService.GetCampaignByID(userExtendEntity.CampaignOldID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		_, err = service.UserMailService.AddMail(userExtendEntity.ID, entity.MailIDCampain, nil, campaignEntity.Rewards)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		userExtendEntity.CampaignOldID = 0
		needUpdate = true
	}

	for _, v := range userExtendEntity.Tower {
		for _, t := range v {
			if t.Status.IsSuccessful() && len(t.Rewards) > 0 {
				_, err = service.UserMailService.AddMail(userExtendEntity.ID, entity.MailIDTower, nil, t.Rewards)
				if err != nil {
					service.RenderWithAppError(c, err)
					return
				}
				t.Status = entity.StatusDone
				needUpdate = true
			}
		}
	}

	if userExtendEntity.Apocalypse.Status.IsSuccessful() || userExtendEntity.Apocalypse.Status.IsFailed() {
		apocalypseData, err := service.UserService.GetApocalypseByID(userExtendEntity.Apocalypse.BossID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		userEntity, err := service.UserService.GetUserByID(userExtendEntity.ID)

		rwd, _ := service.RewardService.MakeRewards(userEntity, apocalypseData.Rewards, userExtendEntity.Apocalypse.Ratio)
		_, err = service.UserMailService.AddMail(userExtendEntity.ID, entity.MailIDBoss, nil, service.RewardService.RewardsToRewardStrs(rwd))

		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		userExtendEntity.Apocalypse.Status = entity.StatusCreated
		needUpdate = true
	}

	if needUpdate {
		err = service.UserService.UpdateUserExtend(userExtendEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	// 新手奖励
	if isNew {
		//util.GoPool().Submit(func() {
		//	for i := 0; i < 3; i++ {
		//		_, err := service.UserMailService.AddMail(userExtendEntity.ID, entity.MailIDTest, nil,
		//			entity.RewardStrings{"1_0_200000000_1", "2_0_200000000_1", "17_0_200000000_1"})
		//		if err != nil {
		//			util.GetLogger().Error(err.Error())
		//			return
		//		}
		//	}
		//})
	}

	rsp := &msg.LoginResponse{
		Uid:                    passportEntity.HidingUid(),
		Token:                  auth.AccessToken,
		RefreshToken:           auth.RefreshToken,
		TokenExpireTime:        auth.AccessTokenExpireIn,
		RefreshTokenExpireTime: auth.RefreshTokenExpireIn,
	}

	service.RenderSuccess(c, rsp)
}

func Regist(c *gin.Context) {
	req := &msg.RegistRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 校验身份证号
	ok, age, err := util.ValidateID(req.IDCard)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if !ok {
		service.RenderWithAppError(c, errors.New("身份证号不正确"))
		return
	}

	passportEntity, err := service.PassportService.GetPassportByName(req.Name)
	// if err != nil {
	// 	service.RenderWithAppError(c, errors.New("身份证号信息不正确"))
	// 	return
	// }
	// 411481199312263319

	if passportEntity != nil {
		service.RenderWithAppError(c, errors.New("用户名已存在"))
		return
	}

	if passportEntity == nil {
		//需要重新注册
		passportEntity, err = service.PassportService.CreatePassport(req.Name, req.Password)
		if err != nil {
			return
		}

		ageType := 0
		if age < 8 {
			ageType = 1
		} else if age >= 8 && age < 16 {
			ageType = 2
		} else if age >= 16 && age < 18 {
			ageType = 3
		} else {
			ageType = 4
		}

		//需要重新注册
		userEntity := entity.NewUser(passportEntity.ID, ageType)
		userEntity, err = dao.UserDao.Create(userEntity)
		if err != nil {
			return
		}

		_, err = service.UserService.CreateUserExtend(passportEntity.ID)
		if err != nil {
			return
		}

		//_, err = UserBuildingService.InitBuildings(userEntity)
		//if err != nil {
		//	return
		//}

		_, err = service.UserHeroService.InitMainHero(userEntity)
		if err != nil {
			return
		}
	}

	rsp := &msg.RegistResponse{Name: req.Name}

	service.RenderSuccess(c, rsp)
}
