package service

import (
	"dakunlun/app/constant"
	"dakunlun/app/dao"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/event"
	data2 "dakunlun/app/service/data"
	"dakunlun/app/util"
)

type userBuildingService struct {
}

var UserBuildingService = new(userBuildingService)

// 初始化建筑物
func (srv *userBuildingService) Check(userEntity *entity.UserEntity) (buildingEntitys []*entity.UserBuildingEntity, err error) {
	// 获取符合开放条件的建筑
	buildingDatas, err := data2.BuildingService.GetBuildingsByCondition(constant.OpenTypeCampaignNum, userEntity.CampaignNum)
	if err != nil {
		return
	}

	for _, buildingData := range buildingDatas {
		buildingEntity, err := srv.AddBuilding(userEntity, buildingData.ID, 1)
		if err != nil {
			return buildingEntitys, err
		}
		buildingEntitys = append(buildingEntitys, buildingEntity)
	}

	return
}

// 添加新建筑
func (srv *userBuildingService) AddBuilding(userEntity *entity.UserEntity, buildingID uint32, level uint16) (buildingEntity *entity.UserBuildingEntity, err error) {
	buildingEntity, err = dao.UserBuildingDao.Create(userEntity.ID, buildingID, level)
	if err == nil {
		// 抛出建筑变更事件
		util.EventDispatcher().Fire(event.NewBuildingUpdateEvent(&event.BuildingUpdateObject{
			UserEntity:         userEntity,
			UserBuildingEntity: buildingEntity,
		}))
	}

	return
}

// 建筑效果
func (srv *userBuildingService) Effect(userEntity *entity.UserEntity, userBuildingEntity *entity.UserBuildingEntity) (err error) {

	if userBuildingEntity.BuildingID < entity.BIDInstitute {
		var buildingUpgradeEntity *entity.BuildingUpgradeData
		buildingUpgradeEntity, err = data.BuildingUpgradeDao.FetchBuildingIDAndLevel(userBuildingEntity.BuildingID, userBuildingEntity.Level)
		if err != nil {
			return
		}

		switch buildingUpgradeEntity.EffectID {
		case constant.EffectIDGoldBase:
			//刷新金币
			_ = userEntity.CurGold()
			switch buildingUpgradeEntity.BuildingID {
			case entity.BIDMill:
				userEntity.BuildingEffect.GoldMill = buildingUpgradeEntity.EffectVal
			case entity.BIDTavern:
				userEntity.BuildingEffect.GoldTavern = buildingUpgradeEntity.EffectVal
			case entity.BIDMine:
				userEntity.BuildingEffect.GoldMine = buildingUpgradeEntity.EffectVal
			case entity.BIDMetallurgy:
				userEntity.BuildingEffect.GoldMetallurgy = buildingUpgradeEntity.EffectVal
			}
		case constant.EffectIDGoldRatio:
			//刷新金币
			_ = userEntity.CurGold()
			userEntity.BuildingEffect.GoldRatio = buildingUpgradeEntity.EffectVal
		}

		err = UserService.UpdateUser(userEntity)
	} else {
		//研究所初始化0级科技
		if userBuildingEntity.Level == 1 && userBuildingEntity.BuildingID == entity.BIDInstitute {
			var techDatas []*entity.TechData
			techDatas, err = data.TechDao.FetchAll()
			if err != nil {
				return
			}
			for _, techData := range techDatas {
				UserTechService.AddTech(userEntity, techData.ID, 0)
			}
		}

		if userBuildingEntity.Level == 1 && userBuildingEntity.BuildingID == entity.BIDTower {
			var exploreDatas []*entity.ExploreData
			exploreDatas, err = data.ExploreDao.FetchAll()
			if err != nil {
				return
			}
			for _, exploreData := range exploreDatas {
				UserExploreService.AddExplore(userEntity, exploreData.ID)
			}
		}
	}

	return
}

// 获取初始建筑ID列表
func (srv *userBuildingService) getInitIDs() []uint32 {
	return []uint32{
		entity.BIDAssemblyHall,
	}
}

// 获取用户建筑列表
func (srv *userBuildingService) GetBuildingsByUid(uid uint32) (userBuildingEntitys []*entity.UserBuildingEntity, err error) {
	userBuildingEntitys, err = dao.UserBuildingDao.FetchMultiByUid(uid)
	return
}

// 升级建筑
func (srv *userBuildingService) Upgrade(userEntity *entity.UserEntity, id uint32) (userBuildingEntity *entity.UserBuildingEntity, err error) {
	userBuildingEntity, err = dao.UserBuildingDao.FetchByID(id)
	if err != nil {
		return
	}
	userBuildingEntity.Level += 1

	if userEntity.ID != userBuildingEntity.Uid {
		err = util.NewAppError(util.ErrorCodeHack, "错误的用户ID")
		return
	}

	var buildingUpgradeData *entity.BuildingUpgradeData
	buildingUpgradeData, err = data.BuildingUpgradeDao.FetchBuildingIDAndLevel(userBuildingEntity.BuildingID, userBuildingEntity.Level)
	if err != nil {
		return
	}
	// 检查开放条件
	err = UserService.ConditionCheck(userEntity, buildingUpgradeData.ConditionType, buildingUpgradeData.ConditionVal)
	if err != nil {
		return
	}
	// 扣除资源
	err = UserService.DecrAssets(userEntity, buildingUpgradeData.CostType, buildingUpgradeData.CostSubType, buildingUpgradeData.CostVal)
	if err != nil {
		return
	}

	//扣除资源
	err = UserService.UpdateUser(userEntity)
	if err != nil {
		return
	}

	// 更新建筑登等级
	err = srv.UpdateBuilding(userBuildingEntity)

	if err != nil {
		return
	}

	// 抛出建筑变更事件
	util.EventDispatcher().Fire(event.NewBuildingUpdateEvent(&event.BuildingUpdateObject{
		UserEntity:         userEntity,
		UserBuildingEntity: userBuildingEntity,
	}))

	return
}

// 更新用户hero数据
func (srv *userBuildingService) UpdateBuilding(userBuildingEntity *entity.UserBuildingEntity) (err error) {
	err = dao.UserBuildingDao.Update(userBuildingEntity)
	return
}
