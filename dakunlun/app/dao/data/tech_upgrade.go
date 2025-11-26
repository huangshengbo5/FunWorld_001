package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type techUpgradeDao struct {
}

var TechUpgradeDao = new(techUpgradeDao)

func (dao *techUpgradeDao) FetchTechIDAndLevel(techID uint32, level uint16) (techUpgradeData *entity.TechUpgradeData, err error) {
	techUpgradeData = &entity.TechUpgradeData{}
	err = util.GetDB().Where("`tech_id` = ? and `level` = ?", techID, level).First(techUpgradeData).Error
	return
}
