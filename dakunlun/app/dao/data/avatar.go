package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type avatarDao struct {
}

var AvatarDao = new(avatarDao)

func (dao *avatarDao) KeyPrefix() string {
	return "avatar"
}

func (dao *avatarDao) FetchByID(id uint32) (avatarData *entity.AvatarData, err error) {
	avatarData = &entity.AvatarData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		avatarData = val.(*entity.AvatarData)
		return
	}

	err = util.GetDB().First(avatarData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, avatarData)

	return
}

func (dao *avatarDao) FetchAll() (avatarDatas []*entity.AvatarData, err error) {
	avatarDatas = make([]*entity.AvatarData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		avatarDatas = val.([]*entity.AvatarData)
		return
	}

	err = util.GetDB().Find(&avatarDatas).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, avatarDatas)

	return
}
