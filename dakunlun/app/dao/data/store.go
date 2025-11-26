package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type storeDao struct {
}

var StoreDao = new(storeDao)

func (dao *storeDao) KeyPrefix() string {
	return "store"
}

func (dao *storeDao) FetchByID(id uint32) (storeData *entity.StoreData, err error) {
	storeData = &entity.StoreData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyName, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		storeData = val.(*entity.StoreData)
		return
	}

	err = util.GetDB().Where("`id` = ?", id).First(storeData).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, storeData)

	return
}
