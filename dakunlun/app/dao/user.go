package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
	"time"

	"github.com/vmihailenco/msgpack/v5"
)

type userDao struct {
}

var UserDao = new(userDao)

func (dao *userDao) KeyPrefix() string {
	return "user"
}

func (dao *userDao) Create(userEntity *entity.UserEntity) (*entity.UserEntity, error) {
	err := util.GetDB().Create(userEntity).Error
	return userEntity, err
}

func (dao *userDao) FetchByID(id uint32) (userEntity *entity.UserEntity, err error) {
	userEntity = &entity.UserEntity{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 有数据走缓存
	if val != nil {
		err = msgpack.Unmarshal(val, userEntity)
		return
	}

	// 没数据走数据库
	err = util.GetDB().First(userEntity, id).Error

	if err != nil {
		return
	}

	// 序列化对象
	var bytes []byte
	bytes, err = msgpack.Marshal(userEntity)
	if err != nil {
		return
	}
	// 写入缓存
	saveToCache(key, bytes, time.Hour*24)

	return userEntity, err
}

func (dao *userDao) FetchByName(name string) (userEntity *entity.UserEntity, err error) {
	userEntity = &entity.UserEntity{}

	// 没数据走数据库
	err = util.GetDB().Where("`name` = ?", name).First(userEntity).Error

	if err != nil {
		return
	}

	return userEntity, err
}

func (dao *userDao) FetchMultiByLevelGT(level uint16, num int) (userEntitys []*entity.UserEntity, err error) {
	err = util.GetDB().Where("`level` > ?", level).Order("`level` ASC").Limit(num).Find(&userEntitys).Error

	if err != nil {
		return
	}

	return
}

func (dao *userDao) FetchMultiByLevelLTE(level uint16, num int) (userEntitys []*entity.UserEntity, err error) {
	err = util.GetDB().Where("`level` <= ?", level).Order("`level` DESC").Limit(num).Find(&userEntitys).Error

	if err != nil {
		return
	}

	return
}

func (dao *userDao) Update(userEntity *entity.UserEntity) (err error) {
	err = util.GetDB().Save(userEntity).Error

	if err != nil {
		return
	}

	// 生成缓存key
	deleteCache(makeCacheKey(dao.KeyPrefix(), KeyID, userEntity.ID))

	return
}
