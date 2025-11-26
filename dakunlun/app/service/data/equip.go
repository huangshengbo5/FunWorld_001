package data

import (
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type equipService struct {
}

var EquipService = new(equipService)

//按权重随机equip
func (srv *equipService) RandomEquip() (equipData *entity.EquipData, err error) {
	var equipDatas []*entity.EquipData
	equipDatas, err = data.EquipDao.FetchAll()
	if err != nil {
		return
	}
	weightMap := make(map[int]int, len(equipDatas))
	for index, equipData := range equipDatas {
		weightMap[index] = equipData.Weight
	}

	equipData = equipDatas[util.GetResultByWeightMap(weightMap)]
	return
}

// jackpot权重表
func (srv *equipService) GetJackpotMap() (jackpotWeightMap map[int]int, err error) {
	var equipJackpotDatas []*entity.EquipJackpotData
	equipJackpotDatas, err = data.EquipJackpotDao.FetchAll()
	if err != nil {
		return
	}

	jackpotWeightMap = make(map[int]int, len(equipJackpotDatas))
	for _, equipJackpotData := range equipJackpotDatas {
		jackpotWeightMap[int(equipJackpotData.ID)] = equipJackpotData.Weight
	}

	return
}

// attr权重表
func (srv *equipService) GetAttrMap() (attrMap map[uint32]*entity.EquipAttrData, attrWeightMap map[uint32]map[int]int,
	err error) {
	var equipAttrDatas []*entity.EquipAttrData
	equipAttrDatas, err = data.EquipAttrDao.FetchAll()
	if err != nil {
		return
	}

	attrMap = make(map[uint32]*entity.EquipAttrData, len(equipAttrDatas))
	attrWeightMap = make(map[uint32]map[int]int)
	for _, equipAttrData := range equipAttrDatas {
		attrMap[equipAttrData.ID] = equipAttrData
		if _, exist := attrWeightMap[equipAttrData.JackpotID]; !exist {
			attrWeightMap[equipAttrData.JackpotID] = make(map[int]int, 7)
		}
		attrWeightMap[equipAttrData.JackpotID][int(equipAttrData.ID)] = equipAttrData.Weight
	}

	return
}

func (srv *equipService) GetUpgradeMap() (upgradeMap map[uint32]*entity.EquipUpgradeData, err error) {
	var equipUpgradeDatas []*entity.EquipUpgradeData
	equipUpgradeDatas, err = data.EquipUpgradeDao.FetchAll()
	if err != nil {
		return
	}

	upgradeMap = make(map[uint32]*entity.EquipUpgradeData)
	for _, equipUpgradeData := range equipUpgradeDatas {
		upgradeMap[equipUpgradeData.ID] = equipUpgradeData
	}

	return
}
