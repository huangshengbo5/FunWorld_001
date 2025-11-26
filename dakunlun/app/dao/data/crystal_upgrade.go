package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type crystalUpgradeDao struct {
}

var CrystalUpgradeDao = new(crystalUpgradeDao)

func (dao *crystalUpgradeDao) FetchCrystalIDAndLevel(crystalID uint32, level uint16) (crystalUpgradeData *entity.CrystalUpgradeData, err error) {
	crystalUpgradeData = &entity.CrystalUpgradeData{}
	err = util.GetDB().Where("`crystal_id` = ? and `level` = ?", crystalID, level).First(crystalUpgradeData).Error
	return
}
