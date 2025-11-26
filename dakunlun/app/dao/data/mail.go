package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type mailDao struct {
}

var MailDao = new(mailDao)

func (dao *mailDao) KeyPrefix() string {
	return "mail"
}

func (dao *mailDao) FetchByID(id uint32) (mailData *entity.MailData, err error) {
	mailData = &entity.MailData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		mailData = val.(*entity.MailData)
		return
	}

	err = util.GetDB().First(mailData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, mailData)

	return
}
