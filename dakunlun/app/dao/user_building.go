package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type userBuildingDao struct {
}

var UserBuildingDao = new(userBuildingDao)

func (dao *userBuildingDao) Create(uid, buildingID uint32, level uint16) (userBuildingEntity *entity.UserBuildingEntity, err error) {
	userBuildingEntity = entity.NewUserBuilding(uid, buildingID, level)
	err = util.GetDB().Create(userBuildingEntity).Error
	return
}

func (dao *userBuildingDao) FetchByID(id uint32) (userBuildingEntity *entity.UserBuildingEntity, err error) {
	userBuildingEntity = &entity.UserBuildingEntity{}
	err = util.GetDB().First(userBuildingEntity, id).Error
	return userBuildingEntity, err
}

func (dao *userBuildingDao) FetchMultiByUid(uid uint32) (userBuildingEntitys []*entity.UserBuildingEntity, err error) {
	userBuildingEntitys = make([]*entity.UserBuildingEntity, 0)
	err = util.GetDB().Where("uid = ?", uid).Find(&userBuildingEntitys).Error

	return
}

func (dao *userBuildingDao) Update(userBuildingEntity *entity.UserBuildingEntity) (err error) {
	err = util.GetDB().Save(userBuildingEntity).Error

	return
}
