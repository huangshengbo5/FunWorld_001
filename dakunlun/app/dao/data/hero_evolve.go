package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type heroEvolveDao struct {
}

var HeroEvolveDao = new(heroEvolveDao)

const (
	KeyHE_1 = "hero_id:%v:evolve_times%v"
)

func (dao *heroEvolveDao) KeyPrefix() string {
	return "hero_evolve"
}

func (dao *heroEvolveDao) FetchHeroIDAndEvolveTimes(heroID uint32, evolveTimes uint16) (heroEvolveData *entity.
	HeroEvolveData, err error) {
	heroEvolveData = &entity.HeroEvolveData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyHE_1, heroID, evolveTimes)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		heroEvolveData = val.(*entity.HeroEvolveData)
		return
	}

	err = util.GetDB().Where("`hero_id` = ? and `evolve_times` = ?", heroID, evolveTimes).First(heroEvolveData).Error

	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, heroEvolveData)

	return
}
