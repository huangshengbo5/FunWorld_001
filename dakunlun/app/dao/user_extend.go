package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
	"time"

	"github.com/vmihailenco/msgpack/v5"
)

type userExtendDao struct {
}

var UserExtendDao = new(userExtendDao)

func (dao *userExtendDao) KeyPrefix() string {
	return "user_extend"
}

func (dao *userExtendDao) Create(userExtendEntity *entity.UserExtendEntity) (*entity.UserExtendEntity, error) {
	err := util.GetDB().Create(userExtendEntity).Error
	return userExtendEntity, err
}

func (dao *userExtendDao) FetchByID(id uint32) (userExtendEntity *entity.UserExtendEntity, err error) {
	userExtendEntity = &entity.UserExtendEntity{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 有数据走缓存
	if val != nil {
		err = msgpack.Unmarshal(val, userExtendEntity)
	}

	// 没数据走数据库
	err = util.GetDB().First(userExtendEntity, id).Error

	if err != nil {
		return
	}

	// 序列化对象
	var bytes []byte
	bytes, err = msgpack.Marshal(userExtendEntity)
	if err != nil {
		return
	}
	// 写入缓存
	saveToCache(key, bytes, time.Hour*24)

	return userExtendEntity, err
}

func (dao *userExtendDao) Update(userExtendEntity *entity.UserExtendEntity) (err error) {
	err = util.GetDB().Save(userExtendEntity).Error

	if err != nil {
		return
	}

	// 生成缓存key
	deleteCache(makeCacheKey(dao.KeyPrefix(), KeyID, userExtendEntity.ID))

	return
}
