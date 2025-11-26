package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
	"time"

	"github.com/vmihailenco/msgpack/v5"
)

type userHeroDao struct {
}

var UserHeroDao = new(userHeroDao)

func (dao *userHeroDao) KeyPrefix() string {
	return "hero"
}

func (dao *userHeroDao) Create(uid uint32, heroData *entity.HeroData, heroUpgradeData *entity.HeroUpgradeData) (userHeroEntity *entity.UserHeroEntity, err error) {
	userHeroEntity = entity.NewUserHero(uid, heroData, heroUpgradeData)
	err = util.GetDB().Create(userHeroEntity).Error
	return
}

func (dao *userHeroDao) FetchByID(id uint32) (userHeroEntity *entity.UserHeroEntity, err error) {
	userHeroEntity = &entity.UserHeroEntity{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 有数据走缓存
	if val != nil {
		err = msgpack.Unmarshal(val, userHeroEntity)
		return
	}

	err = util.GetDB().First(userHeroEntity, id).Error
	if err != nil {
		return
	}
	// 序列化对象
	var bytes []byte
	bytes, err = msgpack.Marshal(userHeroEntity)
	if err != nil {
		return
	}
	// 写入缓存
	saveToCache(key, bytes, time.Hour*24)

	return userHeroEntity, err
}

func (dao *userHeroDao) FetchByHeroID(uid, heroID uint32) (userHeroEntity *entity.UserHeroEntity, err error) {
	userHeroEntity = &entity.UserHeroEntity{}

	err = util.GetDB().Where("`uid` = ? AND `hero_id` = ?", uid, heroID).First(userHeroEntity).Error

	return userHeroEntity, err
}

func (dao *userHeroDao) FetchMultiByUid(uid uint32) (userHeroEntitys []*entity.UserHeroEntity, err error) {
	userHeroEntitys = make([]*entity.UserHeroEntity, 0)
	err = util.GetDB().Where("uid = ?", uid).Find(&userHeroEntitys).Error
	return
}

func (dao *userHeroDao) Update(userHeroEntity *entity.UserHeroEntity) (err error) {
	err = util.GetDB().Save(userHeroEntity).Error

	if err != nil {
		return
	}

	deleteCache(makeCacheKey(dao.KeyPrefix(), KeyID, userHeroEntity.ID))

	return
}
