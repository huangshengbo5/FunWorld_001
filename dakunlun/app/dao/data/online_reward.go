package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type onlineRewardDao struct {
}

var OnlineRewardDao = new(onlineRewardDao)

func (dao *onlineRewardDao) KeyPrefix() string {
	return "online_reward"
}

func (dao *onlineRewardDao) FetchAll() (equipJackpotDatas []*entity.OnlineRewardData, err error) {
	equipJackpotDatas = make([]*entity.OnlineRewardData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		equipJackpotDatas = val.([]*entity.OnlineRewardData)
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
