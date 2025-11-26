package service

import (
	"dakunlun/app/constant"
	"dakunlun/app/dao"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	data2 "dakunlun/app/service/data"
	"dakunlun/app/util"
	"errors"

	"gorm.io/gorm"
)

type heroEquipService struct {
}

var HeroEquipService = new(heroEquipService)

// 添加equip
func (srv *heroEquipService) AddEquip(userEntity *entity.UserEntity, equipID uint32) (heroEquipEntity *entity.HeroEquipEntity, err error) {
	var equipData *entity.EquipData
	if equipID == 99999 {
		equipData, err = data2.EquipService.RandomEquip()
	} else {
		//获取装备配置
		equipData, err = data.EquipDao.FetchByID(equipID)
	}

	if err != nil {
		return
	}

	//获取装备升级配置
	var equipUpgradeData *entity.EquipUpgradeData
	equipUpgradeData, err = data.EquipUpgradeDao.FetchByLevel(entity.InitEquipLevel)
	if err != nil {
		return
	}

	// 获取属性池权重表
	var jackpotWeightMap map[int]int
	jackpotWeightMap, err = data2.EquipService.GetJackpotMap()
	if err != nil {
		return
	}

	var attrMap map[uint32]*entity.EquipAttrData
	var attrWeightMap map[uint32]map[int]int
	attrMap, attrWeightMap, err = data2.EquipService.GetAttrMap()
	if err != nil {
		return
	}

	// 获取属性池
	var attrDatas []*entity.EquipAttrData
	for i := 0; i < entity.AttrNum; i++ {
		// 随机属性池
		jackpotID := util.GetResultByWeightMap(jackpotWeightMap)
		// 随机属性值
		attrDatas = append(attrDatas, attrMap[uint32(util.GetResultByWeightMap(attrWeightMap[uint32(jackpotID)]))])
	}

	//生成装备
	var equipSkillData *entity.EquipSkillData
	equipSkillData, err = data.EquipSkillDao.FetchById(equipData.SkillID)
	heroEquipEntity, err = dao.HeroEquipDao.Create(userEntity.ID, equipData, equipUpgradeData, attrDatas, equipSkillData)
	if err != nil {
		return
	}

	//生成图鉴
	var heroEquipDocEntity *entity.HeroEquipDocEntity
	heroEquipDocEntity, err = srv.GetEquipDocByEquipID(heroEquipEntity.Uid, heroEquipEntity.EquipID)
	if err != nil {
		return
	}

	if heroEquipDocEntity == nil {
		_, err = dao.HeroEquipDocDao.Create(heroEquipEntity.Uid, heroEquipEntity.EquipID)
	}

	return
}

// 升级equip
func (srv *heroEquipService) Upgrade(userEntity *entity.UserEntity, id uint32) (heroEquipEntity *entity.HeroEquipEntity, err error) {
	heroEquipEntity, err = dao.HeroEquipDao.FetchByID(id)
	if err != nil {
		return
	}

	if heroEquipEntity.Uid != userEntity.ID {
		err = util.NewAppError(util.ErrorCodeHack, "错误的用户ID")
		return
	}

	heroEquipEntity.Level += 1

	//获取装备升级数据
	var equipUpgradeData *entity.EquipUpgradeData
	equipUpgradeData, err = data.EquipUpgradeDao.FetchByLevel(heroEquipEntity.Level)
	if err != nil {
		return
	}

	// 扣除资源
	err = UserService.DecrAssets(userEntity, equipUpgradeData.CostType, equipUpgradeData.CostSubType, equipUpgradeData.CostVal)
	if err != nil {
		return
	}

	err = UserService.UpdateUser(userEntity)
	if err != nil {
		return
	}

	//获取装备锻造数据
	var equipForgeData *entity.EquipForgeData
	if heroEquipEntity.ForgeID > 0 {
		equipForgeData, err = data.EquipForgeDao.FetchByID(heroEquipEntity.ForgeID)
		if err != nil {
			return
		}
	}

	//equip战力值更新
	heroEquipEntity.FightingCapacityPlus = srv.GetFightingCapacityPlus(heroEquipEntity, equipUpgradeData, equipForgeData)

	// 更新equip
	err = srv.UpdateEquip(heroEquipEntity)

	return
}

// 锻造equip
func (srv *heroEquipService) Forge(userEntity *entity.UserEntity, id uint32) (heroEquipEntity *entity.HeroEquipEntity, err error) {
	heroEquipEntity, err = dao.HeroEquipDao.FetchByID(id)
	if err != nil {
		return
	}

	if heroEquipEntity.Uid != userEntity.ID {
		err = util.NewAppError(util.ErrorCodeHack, "错误的用户ID")
		return
	}

	heroEquipEntity.ForgeID += 1

	//获取装备锻造数据
	var equipForgeData *entity.EquipForgeData
	equipForgeData, err = data.EquipForgeDao.FetchByID(heroEquipEntity.ForgeID)
	if err != nil {
		return
	}

	// 扣除资源
	err = UserService.DecrAssets(userEntity, equipForgeData.CostType, equipForgeData.CostSubType, equipForgeData.CostVal)
	if err != nil {
		return
	}

	err = UserService.UpdateUser(userEntity)
	if err != nil {
		return
	}

	//获取装备升级数据
	var equipUpgradeData *entity.EquipUpgradeData
	equipUpgradeData, err = data.EquipUpgradeDao.FetchByLevel(heroEquipEntity.Level)
	if err != nil {
		return
	}

	//equip战力值更新
	heroEquipEntity.FightingCapacityPlus = srv.GetFightingCapacityPlus(heroEquipEntity, equipUpgradeData, equipForgeData)

	// 更新equip
	err = srv.UpdateEquip(heroEquipEntity)

	return
}

func (srv *heroEquipService) GetFightingCapacityPlus(heroEquipEntity *entity.HeroEquipEntity,
	equipUpgradeData *entity.EquipUpgradeData, equipForgeData *entity.EquipForgeData) uint64 {

	var forge uint64
	if equipForgeData != nil {
		forge = equipForgeData.FightingCapacity
	}
	return heroEquipEntity.FightingCapacityBase*uint64(equipUpgradeData.Multiplier)/constant.BaseMultiple + forge

}

// 分页获取用户equip列表
func (srv *heroEquipService) GetEquipsByPage(uid uint32, page, perPage int) (heroEquipEntitys []*entity.
	HeroEquipEntity, pageInfo *constant.Paging, err error) {
	var total int64
	total, err = dao.HeroEquipDao.CountByUid(uid)

	if err != nil || total == 0 {
		return
	}

	pageInfo = &constant.Paging{
		Page:     page,
		PerPage:  perPage,
		TotalNum: int(total),
	}

	heroEquipEntitys, err = dao.HeroEquipDao.FetchMultiByUid(uid, "`pos` desc, `id` desc", (page-1)*perPage, perPage)

	if err != nil {
		return
	}

	pageInfo = &constant.Paging{
		Page:     page,
		PerPage:  perPage,
		TotalNum: int(total),
	}

	return
}

// 获取equip列表
func (srv *heroEquipService) GetEquipsByIds(ids []uint32) (heroEquipEntitys []*entity.HeroEquipEntity, err error) {
	heroEquipEntitys, err = dao.HeroEquipDao.FetchMultiByIds(ids)

	return
}

// 获取equip
func (srv *heroEquipService) GetEquipByID(id uint32) (heroEquipEntity *entity.HeroEquipEntity, err error) {
	heroEquipEntity, err = dao.HeroEquipDao.FetchByID(id)
	return
}

// 更新用户hero数据
func (srv *heroEquipService) UpdateEquip(heroEquipEntity *entity.HeroEquipEntity) (err error) {
	err = dao.HeroEquipDao.Update(heroEquipEntity)
	return
}

// 更新用户hero数据
func (srv *heroEquipService) DeleteEquip(heroEquipEntity *entity.HeroEquipEntity) (err error) {
	err = dao.HeroEquipDao.Delete(heroEquipEntity)
	return
}

// 获取equip-doc列表
func (srv *heroEquipService) GetEquipDocsByUid(uid uint32) (heroEquipDocEntitys []*entity.HeroEquipDocEntity, err error) {
	heroEquipDocEntitys, err = dao.HeroEquipDocDao.FetchMultiByUid(uid)
	return
}

// 根据id获取equip-doc数据
func (srv *heroEquipService) GetEquipDocsByID(id uint32) (heroEquipDocEntity *entity.HeroEquipDocEntity, err error) {
	heroEquipDocEntity, err = dao.HeroEquipDocDao.FetchByID(id)
	return
}

// 根据装备ID和用户ID获取equip-doc数据
func (srv *heroEquipService) GetEquipDocByEquipID(uid, equipID uint32) (heroEquipDocEntity *entity.HeroEquipDocEntity,
	err error) {
	heroEquipDocEntity, err = dao.HeroEquipDocDao.FetchByEquipID(uid, equipID)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, nil
		}
	}
	return
}

func (srv *heroEquipService) GetEquipSkillMap() (equipSkillMap map[uint32]*entity.EquipSkillData, err error) {
	equipSkillDatas, err := data.EquipSkillDao.FetchAll()
	if err != nil {
		return
	}
	equipSkillMap = make(map[uint32]*entity.EquipSkillData, 32)
	for _, equipSkillData := range equipSkillDatas {
		equipSkillMap[equipSkillData.ID] = equipSkillData
	}
	return
}

// 更新用户equip-doc数据
func (srv *heroEquipService) UpdateEquipDoc(heroEquipDocEntity *entity.HeroEquipDocEntity) (err error) {
	err = dao.HeroEquipDocDao.Update(heroEquipDocEntity)
	return
}
