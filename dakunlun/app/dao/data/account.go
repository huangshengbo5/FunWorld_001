package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type accountDao struct {
}

const KeyName = "id:%v"

var AccountDao = new(accountDao)

func (dao *accountDao) KeyPrefix() string {
	return "account"
}

func (dao *accountDao) FetchByName(name string) (accountData *entity.AccountData, err error) {
	accountData = &entity.AccountData{}
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyName, name)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		accountData = val.(*entity.AccountData)
		return
	}

	err = util.GetDB().Where("`name` = ?", name).First(accountData).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, accountData)

	return
}
