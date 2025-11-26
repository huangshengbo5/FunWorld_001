package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type equipAttrDao struct {
}

var EquipAttrDao = new(equipAttrDao)

func (dao *equipAttrDao) KeyPrefix() string {
	return "equip_attr"
}

func (dao *equipAttrDao) FetchAll() (equipAttrDatas []*entity.EquipAttrData, err error) {
	equipAttrDatas = make([]*entity.EquipAttrData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		equipAttrDatas = val.([]*entity.EquipAttrData)
		return
	}

	err = util.GetDB().Find(&equipAttrDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, equipAttrDatas)

	return
}
