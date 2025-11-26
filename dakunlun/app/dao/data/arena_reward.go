package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type arenaRewardDao struct {
}

var ArenaRewardDao = new(arenaRewardDao)

func (dao *arenaRewardDao) KeyPrefix() string {
	return "arena_reward"
}

func (dao *arenaRewardDao) FetchAll() (arenaRewardDatas []*entity.ArenaRewardData, err error) {
	tmp := make([]*entity.ArenaRewardData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		arenaRewardDatas = val.([]*entity.ArenaRewardData)
		return
	}

	err = util.GetDB().Order("`id` ASC").Find(&tmp).Error
	if err != nil {
		return
	}

	for _, arenaRewardData := range tmp {
		arenaRewardDatas = append(arenaRewardDatas, arenaRewardData)
	}

	return
}
