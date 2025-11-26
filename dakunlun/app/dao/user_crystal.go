package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type userCrystalDao struct {
}

var UserCrystalDao = new(userCrystalDao)

func (dao *userCrystalDao) Create(uid, crystalID uint32, level uint16) (userCrystalEntity *entity.UserCrystalEntity, err error) {
	userCrystalEntity = entity.NewUserCrystal(uid, crystalID, level)
	err = util.GetDB().Create(userCrystalEntity).Error
	return
}

func (dao *userCrystalDao) FetchByID(id uint32) (userCrystalEntity *entity.UserCrystalEntity, err error) {
	userCrystalEntity = &entity.UserCrystalEntity{}
	err = util.GetDB().First(userCrystalEntity, id).Error
	return userCrystalEntity, err
}

func (dao *userCrystalDao) FetchMultiByUid(uid uint32) (userCrystalEntitys []*entity.UserCrystalEntity, err error) {
	userCrystalEntitys = make([]*entity.UserCrystalEntity, 0)
	err = util.GetDB().Where("uid = ?", uid).Find(&userCrystalEntitys).Error

	return
}

func (dao *userCrystalDao) Update(userCrystalEntity *entity.UserCrystalEntity) (err error) {
	err = util.GetDB().Save(userCrystalEntity).Error

	return
}
