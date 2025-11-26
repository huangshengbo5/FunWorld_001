package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
	"time"

	"github.com/vmihailenco/msgpack/v5"
)

type passportDao struct {
}

var PassportDao = new(passportDao)

const (
	KeyName_1 = "name:%v"
)

func (dao *passportDao) KeyPrefix() string {
	return "passport"
}

func (dao *passportDao) FetchByName(name string) (passportEntity *entity.PassportEntity, err error) {
	passportEntity = &entity.PassportEntity{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyName_1, name)
	val := loadFromCache(key)
	// 有数据走缓存
	if val != nil {
		err = msgpack.Unmarshal(val, passportEntity)
		return
	}

	err = util.GetDB().Where("name = ?", name).First(passportEntity).Error

	if err != nil {
		return
	}

	// 序列化对象
	var bytes []byte
	bytes, err = msgpack.Marshal(passportEntity)
	if err != nil {
		return
	}
	// 写入缓存
	saveToCache(key, bytes, time.Hour*24)

	return
}

func (dao *passportDao) Create(passportEntity *entity.PassportEntity) (err error) {
	err = util.GetDB().Create(passportEntity).Error
	return
}

func (dao *passportDao) Delete(passportEntity *entity.PassportEntity) (err error) {
	err = util.GetDB().Delete(passportEntity).Error
	// 生成缓存key
	deleteCache(makeCacheKey(dao.KeyPrefix(), KeyName_1, passportEntity.Name))
	return
}
