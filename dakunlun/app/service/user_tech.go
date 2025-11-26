package service

import (
	"dakunlun/app/constant"
	"dakunlun/app/dao"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type userTechService struct {
}

var UserTechService = new(userTechService)

// 添加科技
func (srv *userTechService) AddTech(userEntity *entity.UserEntity, techID uint32, level uint16) (techEntity *entity.UserTechEntity, err error) {
	techEntity, err = dao.UserTechDao.Create(userEntity.ID, techID, 0)
	return
}

// 获取用户科技列表
func (srv *userTechService) GetTechsByUid(uid uint32) (userTechEntitys []*entity.UserTechEntity, err error) {
	userTechEntitys, err = dao.UserTechDao.FetchMultiByUid(uid)
	return
}

// 升级科技
func (srv *userTechService) Upgrade(userEntity *entity.UserEntity, id uint32) (userTechEntity *entity.UserTechEntity, err error) {
	userTechEntity, err = dao.UserTechDao.FetchByID(id)
	if err != nil {
		return
	}

	if userEntity.ID != userTechEntity.Uid {
		err = util.NewAppError(util.ErrorCodeHack, "错误的用户ID")
		return
	}

	userTechEntity.Level += 1

	var techUpgradeData *entity.TechUpgradeData
	techUpgradeData, err = data.TechUpgradeDao.FetchTechIDAndLevel(userTechEntity.TechID, userTechEntity.Level)
	if err != nil {
		return
	}
	// 扣除资源
	err = UserService.DecrAssets(userEntity, techUpgradeData.CostType, techUpgradeData.CostSubType, techUpgradeData.CostVal)
	if err != nil {
		return
	}

	//效果记录
	switch techUpgradeData.EffectID {
	case constant.EffectIDGoldRatio:
		switch techUpgradeData.EffectValue {
		case entity.BIDMill:
			userEntity.TechEffect.MillRatio = techUpgradeData.EffectValue
		case entity.BIDTavern:
			userEntity.TechEffect.TavernRatio = techUpgradeData.EffectValue
		case entity.BIDMine:
			userEntity.TechEffect.MineRatio = techUpgradeData.EffectValue
		case entity.BIDMetallurgy:
			userEntity.TechEffect.MetallurgyRatio = techUpgradeData.EffectValue
		}
	case constant.EffectIDMainRatio:
		userEntity.TechEffect.MainHeroUpgradeRatio = techUpgradeData.EffectValue
	case constant.EffectIDSubRatio:
		userEntity.TechEffect.SubHeroUpgradeRatio = techUpgradeData.EffectValue
	case constant.EffectIDMainFightPlus:
		userEntity.TechEffect.MainHeroFightingCapacityPlus = uint64(techUpgradeData.EffectValue)
	case constant.EffectIDSubFightPlus:
		userEntity.TechEffect.SubHeroFightingCapacityPlus = uint64(techUpgradeData.EffectValue)
	case constant.EffectIDAttack:
		userEntity.TechEffect.AttackTrans = techUpgradeData.EffectValue
	case constant.EffectIDDefend:
		userEntity.TechEffect.DefendTrans = techUpgradeData.EffectValue
	}

	//扣除资源
	err = UserService.UpdateUser(userEntity)
	if err != nil {
		return
	}

	// 更新科技等级
	err = srv.UpdateTech(userTechEntity)

	if err != nil {
		return
	}

	return
}

// 更新用户科技数据
func (srv *userTechService) UpdateTech(userTechEntity *entity.UserTechEntity) (err error) {
	err = dao.UserTechDao.Update(userTechEntity)
	return
}
