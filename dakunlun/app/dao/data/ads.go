package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type adsDao struct {
}

var AdsDao = new(adsDao)

func (dao *adsDao) KeyPrefix() string {
	return "ads"
}

func (dao *adsDao) FetchByID(id uint32) (adsData *entity.AdsData, err error) {
	adsData = &entity.AdsData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		adsData = val.(*entity.AdsData)
		return
	}

	err = util.GetDB().First(adsData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, adsData)

	return
}
