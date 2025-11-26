package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type buildingDao struct {
}

var BuildingDao = new(buildingDao)

const (
	KeyBuilding_1 = "condition_type:%v:condition_val%v"
)

func (dao *buildingDao) KeyPrefix() string {
	return "building"
}

func (dao *buildingDao) FetchByID(id uint32) (buildingData *entity.BuildingData, err error) {
	buildingData = &entity.BuildingData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		buildingData = val.(*entity.BuildingData)
		return
	}

	err = util.GetDB().First(buildingData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, buildingData)

	return
}

func (dao *buildingDao) FetchMultiByCondition(conditionType uint8, conditionVal uint32) (buildingDatas []*entity.BuildingData, err error) {
	buildingDatas = make([]*entity.BuildingData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyBuilding_1, conditionType, conditionVal)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		buildingDatas = val.([]*entity.BuildingData)
		return
	}

	err = util.GetDB().Where("`condition_type` = ? and `condition_val` = ?", conditionType, conditionVal).Find(&buildingDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, buildingDatas)

	return
}
