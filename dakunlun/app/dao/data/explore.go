package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type exploreDao struct {
}

var ExploreDao = new(exploreDao)

func (dao *exploreDao) KeyPrefix() string {
	return "explore"
}

func (dao *exploreDao) FetchByID(id uint32) (exploreData *entity.ExploreData, err error) {
	exploreData = &entity.ExploreData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		exploreData = val.(*entity.ExploreData)
		return
	}

	err = util.GetDB().First(exploreData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, exploreData)

	return
}

func (dao *exploreDao) FetchAll() (exploreDatas []*entity.ExploreData, err error) {
	exploreDatas = make([]*entity.ExploreData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		exploreDatas = val.([]*entity.ExploreData)
		return
	}

	err = util.GetDB().Find(&exploreDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, exploreDatas)

	return
}
