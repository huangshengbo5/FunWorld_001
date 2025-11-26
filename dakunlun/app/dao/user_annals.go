package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type userAnnalsDao struct {
}

var UserAnnalsDao = new(userAnnalsDao)

func (dao *userAnnalsDao) Create(uid uint32) (userAnnalsEntity *entity.UserAnnalsEntity, err error) {
	userAnnalsEntity = entity.NewUserAnnals(uid)
	err = util.GetDB().Create(userAnnalsEntity).Error
	return
}

func (dao *userAnnalsDao) FetchByID(uid uint32) (userAnnalsEntity *entity.UserAnnalsEntity, err error) {
	userAnnalsEntity = &entity.UserAnnalsEntity{}
	err = util.GetDB().First(userAnnalsEntity, uid).Error
	return userAnnalsEntity, err
}

func (dao *userAnnalsDao) Update(userAnnalsEntity *entity.UserAnnalsEntity) (err error) {
	err = util.GetDB().Save(userAnnalsEntity).Error

	return
}
