package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type skillLevelDao struct {
}

var SkillLevelDao = new(skillLevelDao)

const (
	KeySLevel_1 = "skill_id:%v:level:%v"
)

func (dao *skillLevelDao) KeyPrefix() string {
	return "skill_level"
}

func (dao *skillLevelDao) FetchSkillIDAndLevel(skillID uint32, level uint16) (skillLevelDatas []*entity.SkillLevelData, err error) {
	//tmp := make([]*entity.SkillLevelData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeySLevel_1, skillID, level)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		skillLevelDatas = val.([]*entity.SkillLevelData)
		return
	}

	err = util.GetDB().Where("`skill_id` = ? and `level` = ?", skillID, level).Order("seq").Find(&skillLevelDatas).Error
	if err != nil {
		return
	}

	//skillLevelDatas = tmp
	//for _, skillLevelData := range tmp {
	//	skillLevelDatas = append(skillLevelDatas, skillLevelData)
	//}

	return
}

func (dao *skillLevelDao) FetchAll() (skillLevelDatas []*entity.SkillLevelData, err error) {
	tmp := make([]*entity.SkillLevelData, 0)
	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyAll)
	val := loadFromCache(key)
	// 非空则取缓存数据
	if val != nil {
		skillLevelDatas = val.([]*entity.SkillLevelData)
		return
	}

	err = util.GetDB().Find(&tmp).Error
	if err != nil {
		return
	}

	for _, skillLevelData := range tmp {
		skillLevelDatas = append(skillLevelDatas, skillLevelData)
	}

	return
}
