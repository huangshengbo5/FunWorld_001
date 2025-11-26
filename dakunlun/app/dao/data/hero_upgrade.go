package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type heroUpgradeDao struct {
}

var HeroUpgradeDao = new(heroUpgradeDao)

const (
	KeyHU_1 = "hero_id:%v:level%v"
)

func (dao *heroUpgradeDao) KeyPrefix() string {
	return "hero_upgrade"
}

func (dao *heroUpgradeDao) FetchHeroIDAndLevel(heroID uint32, level uint16) (heroUpgradeData *entity.HeroUpgradeData, err error) {
	heroUpgradeData = &entity.HeroUpgradeData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyHU_1, heroID, level)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		heroUpgradeData = val.(*entity.HeroUpgradeData)
		return
	}

	err = util.GetDB().Where("`hero_id` = ? and `level` = ?", heroID, level).First(heroUpgradeData).Error

	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, heroUpgradeData)
	return
}
