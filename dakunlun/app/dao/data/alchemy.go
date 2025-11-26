package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type alchemyDao struct {
}

var AlchemyDao = new(alchemyDao)

func (dao *alchemyDao) KeyPrefix() string {
	return "alchemy"
}

func (dao *alchemyDao) FetchByAttr(attr uint32) (alchemyData *entity.AlchemyData, err error) {
	var alchemyDatas []*entity.AlchemyData
	alchemyDatas, err = dao.FetchAll()
	if err != nil {
		return
	}

	for _, tmpData := range alchemyDatas {
		if tmpData.AttrMin <= attr && tmpData.AttrMax >= attr {
			alchemyData = tmpData
			return
		}
	}

	return nil, util.NewAppError(util.ErrorCodeNoAlchemyData)
}

func (dao *alchemyDao) FetchAll() (alchemyDatas []*entity.AlchemyData, err error) {
	alchemyDatas = make([]*entity.AlchemyData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		alchemyDatas = val.([]*entity.AlchemyData)
		return
	}

	err = util.GetDB().Find(&alchemyDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, alchemyDatas)

	return
}
