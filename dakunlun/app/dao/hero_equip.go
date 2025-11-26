package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type heroEquipDao struct {
}

var HeroEquipDao = new(heroEquipDao)

func (dao *heroEquipDao) Create(uid uint32, equipData *entity.EquipData, equipUpgradeData *entity.EquipUpgradeData,
	attrDatas []*entity.EquipAttrData, equipSkillData *entity.EquipSkillData) (heroEquipEntity *entity.HeroEquipEntity,
	err error) {
	heroEquipEntity = entity.NewHeroEquip(uid, equipData, equipUpgradeData, attrDatas, equipSkillData)
	err = util.GetDB().Create(heroEquipEntity).Error
	return
}

func (dao *heroEquipDao) FetchByID(id uint32) (heroEquipEntity *entity.HeroEquipEntity, err error) {
	heroEquipEntity = &entity.HeroEquipEntity{}
	err = util.GetDB().First(heroEquipEntity, id).Error
	return
}

func (dao *heroEquipDao) FetchMultiByUid(uid uint32, order string, offset,
	limit int) (heroEquipEntitys []*entity.HeroEquipEntity, err error) {
	heroEquipEntitys = make([]*entity.HeroEquipEntity, 0)
	if order == "" {
		err = util.GetDB().Where("`uid` = ?", uid).Find(&heroEquipEntitys).Offset(offset).Limit(limit).Error
	} else {
		err = util.GetDB().Where("`uid` = ?", uid).Order(order).Offset(offset).Limit(limit).Find(&heroEquipEntitys).
			Error
	}
	return
}

func (dao *heroEquipDao) CountByUid(uid uint32) (count int64, err error) {
	err = util.GetDB().Model(entity.HeroEquipEntity{}).Where("uid = ?", uid).Count(&count).Error
	return
}

func (dao *heroEquipDao) FetchMultiByHeroID(uid uint32, heroID uint32) (heroEquipEntitys []*entity.HeroEquipEntity, err error) {
	heroEquipEntitys = make([]*entity.HeroEquipEntity, 0)
	err = util.GetDB().Where("`uid` = ? and `hero_id` = ?", uid, heroID).Find(&heroEquipEntitys).Error
	return
}

func (dao *heroEquipDao) FetchMultiByPos(uid uint32, heroID uint32, pos uint8) (heroEquipEntity *entity.HeroEquipEntity, err error) {
	heroEquipEntity = &entity.HeroEquipEntity{}
	err = util.GetDB().Where("`uid` = ? and `hero_id` = ? and `pos` =", uid, heroID, pos).First(heroEquipEntity).Error
	return
}

func (dao *heroEquipDao) FetchMultiByIds(ids []uint32) (heroEquipEntitys []*entity.HeroEquipEntity, err error) {
	heroEquipEntitys = make([]*entity.HeroEquipEntity, 0)
	err = util.GetDB().Where("`id` IN (?)", ids).Find(&heroEquipEntitys).Error
	return
}

func (dao *heroEquipDao) Update(heroEquipEntity *entity.HeroEquipEntity) (err error) {
	err = util.GetDB().Save(heroEquipEntity).Error

	return
}

func (dao *heroEquipDao) Delete(heroEquipEntity *entity.HeroEquipEntity) (err error) {
	err = util.GetDB().Delete(heroEquipEntity).Error

	return
}
