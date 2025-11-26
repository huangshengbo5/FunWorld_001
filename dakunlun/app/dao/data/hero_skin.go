package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type heroSkinDao struct {
}

var HeroSkinDao = new(heroSkinDao)

func (dao *heroSkinDao) KeyPrefix() string {
	return "hero_skin"
}

func (dao *heroSkinDao) FetchByID(id uint32) (heroSkinData *entity.HeroSkinData, err error) {
	heroSkinData = &entity.HeroSkinData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		heroSkinData = val.(*entity.HeroSkinData)
		return
	}

	err = util.GetDB().First(heroSkinData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, heroSkinData)

	return
}
