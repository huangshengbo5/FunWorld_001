package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type npcDao struct {
}

var NpcDao = new(npcDao)

func (dao *npcDao) KeyPrefix() string {
	return "npc"
}

func (dao *npcDao) FetchByID(id uint32) (npcData *entity.NpcData, err error) {
	npcData = &entity.NpcData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		npcData = val.(*entity.NpcData)
		return
	}

	err = util.GetDB().First(npcData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, npcData)

	return
}
