package service

import (
	"dakunlun/app/constant"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/util"
	"dakunlun/app/util/deepcopy"
)

type lobbyService struct {
}

var LobbyService = new(lobbyService)

// 生成在线奖励
func (srv *lobbyService) RandomOnlineReward() (ids []uint32, err error) {
	var equipJackpotDatas []*entity.OnlineRewardData
	equipJackpotDatas, err = data.OnlineRewardDao.FetchAll()
	for _, v := range equipJackpotDatas {
		if util.JudgeProbability(v.Probability) {
			ids = append(ids, v.ID)
		}
	}
	return
}

func (srv *lobbyService) GetOnlineReward(ids []uint32) (rewardStrs entity.RewardStrings, err error) {
	var equipJackpotDatas []*entity.OnlineRewardData
	equipJackpotDatas, err = data.OnlineRewardDao.FetchAll()
	for _, v := range equipJackpotDatas {
		if util.ContainsUint32(ids, v.ID) {
			for _, val := range v.Rewards {
				rewardStrs = append(rewardStrs, val)
			}
		}
	}
	return
}

// 怪物入侵刷新
func (srv *lobbyService) RefreshTower(userExtendEntity *entity.UserExtendEntity) (err error) {
	var towerDatas []*entity.TowerData
	towerDatas, err = data.TowerDao.FetchAll()
	towerWeightMap := make(map[uint8]map[int]int)
	for _, towerData := range towerDatas {
		if towerData.CampaignIDLower <= userExtendEntity.CampaignID && towerData.CampaignIDUpper >= userExtendEntity.
			CampaignID {
			if _, exist := towerWeightMap[towerData.Seq]; !exist {
				towerWeightMap[towerData.Seq] = make(map[int]int, 2)
			}
			towerWeightMap[towerData.Seq][int(towerData.ID)] = towerData.Weight
		}
	}

	total := len(towerWeightMap)
	for _, v := range []uint8{entity.TowerOne, entity.TowerTwo, entity.TowerThree, entity.TowerFour,
		entity.TowerFive, entity.TowerSix, entity.TowerSeven, entity.TowerEight} {
		userExtendEntity.Tower[v] = make([]*entity.Monster, 0, entity.RefreshTowerNum)
		tmpWeightMap := deepcopy.Copy(towerWeightMap).(map[uint8]map[int]int)
		for i := 0; i < total; i++ {
			towerID := util.GetResultByWeightMap(tmpWeightMap[uint8(i+1)])
			delete(tmpWeightMap, uint8(i+1))
			userExtendEntity.Tower[v] = append(userExtendEntity.Tower[v], &entity.Monster{
				TowerID: uint32(towerID),
				Status:  entity.StatusCreated,
			})
		}
	}

	return
}

func (srv *lobbyService) GetTowerByID(towerID uint32) (towerData *entity.TowerData, err error) {
	towerData, err = data.TowerDao.FetchByID(towerID)
	return
}

func (srv *lobbyService) Buy(userEntity *entity.UserEntity,
	storeID uint32) (rewards []constant.IReward, err error) {
	storeData, err := data.StoreDao.FetchByID(storeID)
	if err != nil {
		return
	}

	if userEntity.Extra.AccountType == 1 {
		err = util.NewAppError(util.ErrorCodeBuyError, "未满8周岁不能使用支付服务")
		return
	}

	userEntity.Extra.Flush()

	if storeData.Type == 1 {
		// 单笔购买上限
		if userEntity.Extra.SingleLimit != 0 {
			if storeData.Price > userEntity.Extra.SingleLimit {
				err = util.NewAppError(util.ErrorCodeBuyError, "超过单笔支付上限")
			}
		}
		// 月购买上限
		if userEntity.Extra.MonthLimit != 0 {
			if userEntity.Extra.Total+storeData.Price > userEntity.Extra.MonthLimit {
				err = util.NewAppError(util.ErrorCodeBuyError, "超过每月支付上限")
			}
		}

		if err != nil {
			return
		}

		userEntity.Extra.Total += storeData.Price
		UserService.UpdateUser(userEntity)
	} else {
		err = UserService.DecrAssets(userEntity, constant.CostTypeDiamond, 0, storeData.Price)
	}

	if err != nil {
		return
	}

	rewards, err = RewardService.SendRewards(userEntity, storeData.Rewards, SourceStore, nil)
	if err != nil {
		return
	}

	return
}
