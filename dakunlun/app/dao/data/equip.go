package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type equipDao struct {
}

var EquipDao = new(equipDao)

func (dao *equipDao) KeyPrefix() string {
	return "equip"
}

func (dao *equipDao) FetchByID(id uint32) (equipData *entity.EquipData, err error) {
	equipData = &entity.EquipData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		equipData = val.(*entity.EquipData)
		return
	}

	err = util.GetDB().First(equipData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, equipData)

	return
}

func (dao *equipDao) FetchAll() (equipDatas []*entity.EquipData, err error) {
	equipDatas = make([]*entity.EquipData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		equipDatas = val.([]*entity.EquipData)
		return
	}

	err = util.GetDB().Find(&equipDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, equipDatas)

	return
}
