package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type towerDao struct {
}

var TowerDao = new(towerDao)

func (dao *towerDao) KeyPrefix() string {
	return "tower"
}

func (dao *towerDao) FetchByID(id uint32) (towerData *entity.TowerData, err error) {
	towerData = &entity.TowerData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		towerData = val.(*entity.TowerData)
		return
	}

	err = util.GetDB().First(towerData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, towerData)

	return
}

func (dao *towerDao) FetchAll() (towerDatas []*entity.TowerData, err error) {
	towerDatas = make([]*entity.TowerData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		towerDatas = val.([]*entity.TowerData)
		return
	}

	err = util.GetDB().Find(&towerDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, towerDatas)

	return
}
