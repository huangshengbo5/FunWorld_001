package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type userMailDao struct {
}

var UserMailDao = new(userMailDao)

func (dao *userMailDao) KeyPrefix() string {
	return "mail"
}

func (dao *userMailDao) Create(uid uint32, mailID uint32, params entity.Params, rewards entity.RewardStrings) (
	userMailEntity *entity.UserMailEntity, err error) {
	userMailEntity = entity.NewUserMail(uid, mailID, params, rewards)
	err = util.GetDB().Create(userMailEntity).Error
	return
}

func (dao *userMailDao) FetchByID(id uint32) (userMailEntity *entity.UserMailEntity, err error) {
	userMailEntity = &entity.UserMailEntity{}
	err = util.GetDB().First(userMailEntity, id).Error
	return
}

func (dao *userMailDao) FetchMultiByUid(uid uint32, offset int, limit int) (userMailEntitys []*entity.UserMailEntity,
	err error) {
	userMailEntitys = make([]*entity.UserMailEntity, 0)
	err = util.GetDB().Where("`uid` = ?", uid).Order("`id` DESC").Offset(offset).Limit(limit).Find(&userMailEntitys).Error
	return
}

func (dao *userMailDao) FetchMultiByUidAndUnReceived(uid uint32) (userMailEntitys []*entity.UserMailEntity,
	err error) {
	userMailEntitys = make([]*entity.UserMailEntity, 0)
	err = util.GetDB().Where("`uid` = ? AND `has_received` = ?", uid, entity.IntFalse).Find(&userMailEntitys).
		Error
	return
}

func (dao *userMailDao) DeleteMultiByUid(uid uint32) (err error) {
	err = util.GetDB().Where("`uid` = ? AND `has_received` = ?", uid, entity.IntFalse).Delete(&entity.UserMailEntity{}).
		Error
	return
}

func (dao *userMailDao) CountByUid(uid uint32) (count int64,
	err error) {
	err = util.GetDB().Model(entity.UserMailEntity{}).Where("uid = ?", uid).Count(&count).Error
	return
}

func (dao *userMailDao) Update(userMailEntity *entity.UserMailEntity) (err error) {
	err = util.GetDB().Save(userMailEntity).Error
	return
}
