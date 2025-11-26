package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type heroEquipDocDao struct {
}

var HeroEquipDocDao = new(heroEquipDocDao)

func (dao *heroEquipDocDao) Create(uid, equipID uint32) (heroEquipDocEntity *entity.HeroEquipDocEntity,
	err error) {
	heroEquipDocEntity = entity.NewHeroEquipDoc(uid, equipID)
	err = util.GetDB().Create(heroEquipDocEntity).Error
	return
}

func (dao *heroEquipDocDao) FetchByID(id uint32) (heroEquipDocEntity *entity.HeroEquipDocEntity, err error) {
	heroEquipDocEntity = &entity.HeroEquipDocEntity{}
	err = util.GetDB().First(heroEquipDocEntity, id).Error
	return
}

func (dao *heroEquipDocDao) FetchByEquipID(uid, equipID uint32) (heroEquipDocEntity *entity.HeroEquipDocEntity, err error) {
	heroEquipDocEntity = &entity.HeroEquipDocEntity{}
	err = util.GetDB().Where("`uid` = ? and `equip_id` = ?", uid, equipID).First(heroEquipDocEntity).Error
	return
}

func (dao *heroEquipDocDao) FetchMultiByUid(uid uint32) (heroEquipDocEntitys []*entity.HeroEquipDocEntity, err error) {
	heroEquipDocEntitys = make([]*entity.HeroEquipDocEntity, 0)
	err = util.GetDB().Where("`uid` = ?", uid).Find(&heroEquipDocEntitys).Error
	if err != nil {
		return
	}

	return
}

func (dao *heroEquipDocDao) Update(heroEquipDocEntity *entity.HeroEquipDocEntity) (err error) {
	err = util.GetDB().Save(heroEquipDocEntity).Error

	return
}
