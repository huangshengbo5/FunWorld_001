package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type equipSkillDao struct {
}

var EquipSkillDao = new(equipSkillDao)

func (dao *equipSkillDao) KeyPrefix() string {
	return "equip_skill"
}

func (dao *equipSkillDao) FetchById(id uint32) (equipSkillData *entity.EquipSkillData,
	err error) {
	equipSkillData = &entity.EquipSkillData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		equipSkillData = val.(*entity.EquipSkillData)
		return
	}

	err = util.GetDB().First(equipSkillData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, equipSkillData)

	return
}

func (dao *equipSkillDao) FetchAll() (equipSkillDatas []*entity.EquipSkillData, err error) {
	tmp := make([]*entity.EquipSkillData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		equipSkillDatas = val.([]*entity.EquipSkillData)
		return
	}

	err = util.GetDB().Find(&tmp).Error
	if err != nil {
		return
	}

	for _, equipSkillData := range tmp {
		equipSkillDatas = append(equipSkillDatas, equipSkillData)
	}

	return
}
