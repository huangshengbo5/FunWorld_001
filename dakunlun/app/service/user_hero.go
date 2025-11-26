package service

import (
	"dakunlun/app/dao"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/event"
	"dakunlun/app/util"
)

type userHeroService struct {
}

var UserHeroService = new(userHeroService)

// 初始化主将
func (srv *userHeroService) InitMainHero(userEntity *entity.UserEntity) (userHeroEntity *entity.UserHeroEntity, err error) {
	userHeroEntity, err = srv.AddHero(userEntity, entity.InitMainHeroID, entity.InitHeroLevel)
	if err != nil {
		return
	}
	//设置主将id
	userEntity.MainHeroID = userHeroEntity.ID
	err = UserService.UpdateUser(userEntity)
	return
}

// 添加英雄
func (srv *userHeroService) AddHero(userEntity *entity.UserEntity, heroID uint32, level uint16) (userHeroEntity *entity.UserHeroEntity, err error) {

	var heroData *entity.HeroData
	heroData, err = data.HeroDao.FetchByID(heroID)
	if err != nil {
		return
	}

	var heroUpgradeData *entity.HeroUpgradeData
	if level == 0 {
		level = heroData.Level
	}
	heroUpgradeData, err = data.HeroUpgradeDao.FetchHeroIDAndLevel(heroID, level)
	if err != nil {
		return
	}

	userHeroEntity, err = dao.UserHeroDao.Create(userEntity.ID, heroData, heroUpgradeData)

	return
}

// 升级HERO
func (srv *userHeroService) Upgrade(userEntity *entity.UserEntity, id uint32) (userHeroEntity *entity.UserHeroEntity, err error) {
	userHeroEntity, err = dao.UserHeroDao.FetchByID(id)
	if err != nil {
		return
	}

	if userEntity.ID != userHeroEntity.Uid {
		err = util.NewAppError(util.ErrorCodeHack, "错误的用户ID")
		return
	}

	userHeroEntity.Level += 1

	//获取锻魂附加的战力值
	var fightingCapacityPlus uint64
	if userHeroEntity.EvolveTimes > 0 {
		var heroEvolveData *entity.HeroEvolveData
		heroEvolveData, err = data.HeroEvolveDao.FetchHeroIDAndEvolveTimes(userHeroEntity.HeroID, userHeroEntity.EvolveTimes)
		if err != nil {
			return
		}
		fightingCapacityPlus = heroEvolveData.FightingCapacity
	}

	//获取升级数据
	var heroUpgradeData *entity.HeroUpgradeData
	heroUpgradeData, err = data.HeroUpgradeDao.FetchHeroIDAndLevel(userHeroEntity.HeroID, userHeroEntity.Level)
	if err != nil {
		return
	}

	//科技折扣
	var discount float64 = 1
	if userHeroEntity.IsMainHero() {
		discount -= float64(userEntity.TechEffect.MainHeroUpgradeRatio) / entity.BaseMultiple
	} else {
		discount -= float64(userEntity.TechEffect.SubHeroUpgradeRatio) / entity.BaseMultiple
	}

	costVal := uint64(float64(heroUpgradeData.CostVal) * discount)
	// 扣除资源
	err = UserService.DecrAssets(userEntity, heroUpgradeData.CostType, heroUpgradeData.CostSubType, costVal)
	if err != nil {
		return
	}

	// 主将的话同步user.level
	if userHeroEntity.IsMainHero() {
		userEntity.Level = userHeroEntity.Level
	}

	//扣除资源
	err = UserService.UpdateUser(userEntity)
	if err != nil {
		return
	}

	//同步英雄技能和战斗力值
	userHeroEntity.FightingCapacity = heroUpgradeData.FightingCapacity + fightingCapacityPlus
	userHeroEntity.Skills[entity.SkillOneIndex].Level = heroUpgradeData.Skill1Level
	userHeroEntity.Skills[entity.SkillTwoIndex].Level = heroUpgradeData.Skill2Level
	userHeroEntity.Skills[entity.SkillThreeIndex].Level = heroUpgradeData.Skill3Level

	// 更新英雄
	err = srv.UpdateHero(userHeroEntity)

	if userHeroEntity.IsMainHero() {
		// 抛出建筑变更事件
		util.EventDispatcher().Fire(event.NewLevelUpEvent(userEntity))
	}

	return
}

// 锻魂HERO
func (srv *userHeroService) Evolve(userEntity *entity.UserEntity, id uint32) (userHeroEntity *entity.UserHeroEntity, err error) {
	userHeroEntity, err = dao.UserHeroDao.FetchByID(id)
	if err != nil {
		return
	}

	if userEntity.ID != userHeroEntity.Uid {
		err = util.NewAppError(util.ErrorCodeHack, "错误的用户ID")
		return
	}

	userHeroEntity.EvolveTimes += 1

	//获取锻魂数据
	var heroEvolveData *entity.HeroEvolveData
	heroEvolveData, err = data.HeroEvolveDao.FetchHeroIDAndEvolveTimes(userHeroEntity.HeroID, userHeroEntity.EvolveTimes)
	if err != nil {
		return
	}

	//检查HERO等级
	if heroEvolveData.Level > userHeroEntity.Level {
		err = util.NewAppError(util.ErrorCodeHeroLevelNotEnough)
		return
	}

	//获取升级数据
	var heroUpgradeData *entity.HeroUpgradeData
	heroUpgradeData, err = data.HeroUpgradeDao.FetchHeroIDAndLevel(userHeroEntity.HeroID, userHeroEntity.Level)
	if err != nil {
		return
	}

	// 扣除资源
	err = UserService.DecrAssets(userEntity, heroEvolveData.CostType, heroEvolveData.CostSubType, heroEvolveData.CostVal)
	if err != nil {
		return
	}

	//扣除资源
	err = UserService.UpdateUser(userEntity)
	if err != nil {
		return
	}

	//同步英雄战斗力值
	userHeroEntity.FightingCapacity = heroUpgradeData.FightingCapacity + heroEvolveData.FightingCapacity
	//同步英雄攻防转换比例
	userHeroEntity.AttackRatio = heroEvolveData.AttackRatio
	userHeroEntity.DefendRatio = heroEvolveData.DefendRatio
	// 更新英雄
	err = srv.UpdateHero(userHeroEntity)

	return
}

// 获取英雄列表
func (srv *userHeroService) GetHerosByUid(uid uint32) (userHeroEntitys []*entity.UserHeroEntity, err error) {
	userHeroEntitys, err = dao.UserHeroDao.FetchMultiByUid(uid)
	return
}

// 根据ID获取英雄
func (srv *userHeroService) GetHeroByID(id uint32) (userHeroEntity *entity.UserHeroEntity, err error) {
	userHeroEntity, err = dao.UserHeroDao.FetchByID(id)
	return
}

func (srv *userHeroService) GetHeroByHeroID(uid, heroID uint32) (userHeroEntity *entity.UserHeroEntity,
	err error) {
	userHeroEntity, err = dao.UserHeroDao.FetchByHeroID(uid, heroID)
	return
}

// 根据ID获取英雄皮肤
func (srv *userHeroService) GetHeroSkin(id uint32) (heroDataEntity *entity.HeroSkinData, err error) {
	heroDataEntity, err = data.HeroSkinDao.FetchByID(id)
	return
}

// 更新用户hero数据
func (srv *userHeroService) UpdateHero(userHeroEntity *entity.UserHeroEntity) (err error) {
	err = dao.UserHeroDao.Update(userHeroEntity)
	return
}
