package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type userExploreDao struct {
}

var UserExploreDao = new(userExploreDao)

func (dao *userExploreDao) Create(uid, exploreID uint32) (userExploreEntity *entity.UserExploreEntity, err error) {
	userExploreEntity = entity.NewUserExplore(uid, exploreID)
	err = util.GetDB().Create(userExploreEntity).Error
	return
}

func (dao *userExploreDao) FetchByID(id uint32) (userExploreEntity *entity.UserExploreEntity, err error) {
	userExploreEntity = &entity.UserExploreEntity{}
	err = util.GetDB().First(userExploreEntity, id).Error
	return userExploreEntity, err
}

func (dao *userExploreDao) FetchMultiByUid(uid uint32) (userExploreEntitys []*entity.UserExploreEntity, err error) {
	userExploreEntitys = make([]*entity.UserExploreEntity, 0)
	err = util.GetDB().Where("uid = ?", uid).Find(&userExploreEntitys).Error

	return
}

func (dao *userExploreDao) Update(userExploreEntity *entity.UserExploreEntity) (err error) {
	err = util.GetDB().Save(userExploreEntity).Error

	return
}
