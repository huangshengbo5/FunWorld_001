package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type heroDao struct {
}

var HeroDao = new(heroDao)

func (dao *heroDao) KeyPrefix() string {
	return "hero"
}

func (dao *heroDao) FetchByID(id uint32) (heroData *entity.HeroData, err error) {
	heroData = &entity.HeroData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		heroData = val.(*entity.HeroData)
		return
	}

	err = util.GetDB().First(heroData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, heroData)

	return
}
