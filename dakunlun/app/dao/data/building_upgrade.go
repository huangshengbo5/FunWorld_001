package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type buildingUpgradeDao struct {
}

var BuildingUpgradeDao = new(buildingUpgradeDao)

func (dao *buildingUpgradeDao) FetchBuildingIDAndLevel(buildingID uint32, level uint16) (buildingUpgradeData *entity.BuildingUpgradeData, err error) {
	buildingUpgradeData = &entity.BuildingUpgradeData{}
	err = util.GetDB().Where("`building_id` = ? and `level` = ?", buildingID, level).First(buildingUpgradeData).Error
	return
}
