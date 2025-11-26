package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type equipJackpotDao struct {
}

var EquipJackpotDao = new(equipJackpotDao)

func (dao *equipJackpotDao) KeyPrefix() string {
	return "equip_jackpot"
}

func (dao *equipJackpotDao) FetchAll() (equipJackpotDatas []*entity.EquipJackpotData, err error) {
	equipJackpotDatas = make([]*entity.EquipJackpotData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		equipJackpotDatas = val.([]*entity.EquipJackpotData)
		return
	}

	err = util.GetDB().Find(&equipJackpotDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, equipJackpotDatas)

	return
}
