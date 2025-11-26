package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type apocalypseDao struct {
}

var ApocalypseDao = new(apocalypseDao)

func (dao *apocalypseDao) KeyPrefix() string {
	return "apocalypse"
}

func (dao *apocalypseDao) FetchByID(id uint32) (apocalypseData *entity.ApocalypseData, err error) {
	apocalypseData = &entity.ApocalypseData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		apocalypseData = val.(*entity.ApocalypseData)
		return
	}

	err = util.GetDB().First(apocalypseData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, apocalypseData)

	return
}
