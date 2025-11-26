package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type equipForgeDao struct {
}

var EquipForgeDao = new(equipForgeDao)

func (dao *equipForgeDao) KeyPrefix() string {
	return "equip_forge"
}

func (dao *equipForgeDao) FetchByID(id uint16) (equipForgeData *entity.EquipForgeData, err error) {
	equipForgeData = &entity.EquipForgeData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		equipForgeData = val.(*entity.EquipForgeData)
		return
	}
	err = util.GetDB().First(equipForgeData, id).Error
	if err != nil {
		return
	}
	// 走DB则设置缓存
	saveToCache(key, equipForgeData)
	return
}
