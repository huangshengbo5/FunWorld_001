package data

import (
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
)

type buildingService struct {
}

var BuildingService = new(buildingService)

func (srv *buildingService) GetBuildingByID(id uint32) (buildingEntity *entity.BuildingData, err error) {
	buildingEntity, err = data.BuildingDao.FetchByID(id)
	return
}

func (srv *buildingService) GetBuildingsByCondition(conditionType uint8, conditionVal uint32) (buildingDatas []*entity.BuildingData, err error) {
	buildingDatas, err = data.BuildingDao.FetchMultiByCondition(conditionType, conditionVal)
	return
}
