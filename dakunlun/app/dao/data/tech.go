package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type techDao struct {
}

var TechDao = new(techDao)

func (dao *techDao) KeyPrefix() string {
	return "tech"
}

func (dao *techDao) FetchAll() (techDatas []*entity.TechData, err error) {
	techDatas = make([]*entity.TechData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		techDatas = val.([]*entity.TechData)
		return
	}

	err = util.GetDB().Find(&techDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, techDatas)

	return
}
