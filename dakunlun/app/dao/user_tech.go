package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type userTechDao struct {
}

var UserTechDao = new(userTechDao)

func (dao *userTechDao) Create(uid, techID uint32, level uint16) (userTechEntity *entity.UserTechEntity, err error) {
	userTechEntity = entity.NewUserTech(uid, techID, level)
	err = util.GetDB().Create(userTechEntity).Error
	return
}

func (dao *userTechDao) FetchByID(id uint32) (userTechEntity *entity.UserTechEntity, err error) {
	userTechEntity = &entity.UserTechEntity{}
	err = util.GetDB().First(userTechEntity, id).Error
	return userTechEntity, err
}

func (dao *userTechDao) FetchMultiByUid(uid uint32) (userTechEntitys []*entity.UserTechEntity, err error) {
	userTechEntitys = make([]*entity.UserTechEntity, 0)
	err = util.GetDB().Where("uid = ?", uid).Find(&userTechEntitys).Error

	return
}

func (dao *userTechDao) Update(userTechEntity *entity.UserTechEntity) (err error) {
	err = util.GetDB().Save(userTechEntity).Error

	return
}
