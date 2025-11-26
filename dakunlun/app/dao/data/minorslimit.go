package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type minorslimitDao struct {
}

var MinorslimitDao = new(minorslimitDao)

func (dao *minorslimitDao) KeyPrefix() string {
	return "ml"
}

func (dao *minorslimitDao) FetchAll() (minorslimitDatas []*entity.MinorslimitData, err error) {
	minorslimitDatas = make([]*entity.MinorslimitData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		minorslimitDatas = val.([]*entity.MinorslimitData)
		return
	}

	err = util.GetDB().Find(&minorslimitDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, minorslimitDatas)

	return
}
