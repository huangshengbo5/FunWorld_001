package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type equipUpgradeDao struct {
}

var EquipUpgradeDao = new(equipUpgradeDao)

func (dao *equipUpgradeDao) KeyPrefix() string {
	return "equip_upgrade"
}

func (dao *equipUpgradeDao) FetchByLevel(level uint16) (equipUpgradeData *entity.EquipUpgradeData, err error) {
	equipUpgradeData = &entity.EquipUpgradeData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, level)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		equipUpgradeData = val.(*entity.EquipUpgradeData)
		return
	}
	err = util.GetDB().First(equipUpgradeData, level).Error
	if err != nil {
		return
	}
	// 走DB则设置缓存
	saveToCache(key, equipUpgradeData)
	return
}

func (dao *equipUpgradeDao) FetchAll() (equipUpgradeDatas []*entity.EquipUpgradeData, err error) {
	tmp := make([]*entity.EquipUpgradeData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		equipUpgradeDatas = val.([]*entity.EquipUpgradeData)
		return
	}

	err = util.GetDB().Find(&tmp).Error
	if err != nil {
		return
	}
	for _, equipUpgradeData := range tmp {
		equipUpgradeDatas = append(equipUpgradeDatas, equipUpgradeData)
	}

	return
}
