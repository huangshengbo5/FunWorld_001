package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type castDao struct {
}

var CastDao = new(castDao)

func (dao *castDao) KeyPrefix() string {
	return "cast"
}

func (dao *castDao) FetchByID(id uint32) (castData *entity.CastData, err error) {
	castData = &entity.CastData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		castData = val.(*entity.CastData)
		return
	}

	err = util.GetDB().First(castData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, castData)

	return
}
