package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type annalsDao struct {
}

var AnnalsDao = new(annalsDao)

func (dao *annalsDao) KeyPrefix() string {
	return "annals"
}

func (dao *annalsDao) FetchByID(id uint32) (annalsData *entity.AnnalsData, err error) {
	annalsData = &entity.AnnalsData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		annalsData = val.(*entity.AnnalsData)
		return
	}

	err = util.GetDB().First(annalsData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, annalsData)

	return
}
