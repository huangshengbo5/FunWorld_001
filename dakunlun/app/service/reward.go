package service

import (
	"dakunlun/app/constant"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/service/reward"
	"dakunlun/app/util"
	"fmt"
	"strconv"
	"strings"
)

type rewardService struct {
}

var RewardService = new(rewardService)

type sourceType uint16

const (
	SourceDefault sourceType = iota + 1000
	//1001 邮件附件
	SourceMail
	//1002 成就奖励
	SourceAnnals
	//1003 炼药
	SourceAlchemy
	//1004 怪物入侵
	SourceTower
	//1005 装备图鉴
	SourceEquipDoc
	//1006 世界BOSS天启
	SourceApocalypse
	//1007 在线奖励
	SourceOnlineReward
	//1008 竞技场奖励
	SourceArena
	//1009 关卡
	SourceCampaign
	//1010 分解装备
	SourceDecompose
	//1011 商人
	SourceBusinessMan
	//1012激励奖励
	SourceAds
	//1013探索
	SourceExplore
	//1014购买
	SourceStore
)

// 根据奖励字符串数组发送奖励
func (srv *rewardService) SendRewards(userEntity *entity.UserEntity, rewardSlice entity.RewardStrings, source sourceType,
	formulaData interface{}) (rewards []constant.IReward, err error) {
	// 生成奖励数据
	rewards, err = srv.MakeRewards(userEntity, rewardSlice, formulaData)
	if err != nil {
		return
	}

	// 发奖
	for _, reward := range rewards {
		reward.Send()
		//userEntity.AppendToRewardPool(entity.NewRewardData(serve.MainType(), serve.SubType(), serve.Value(), source))
	}

	// 更新用户表
	err = UserService.UpdateUser(userEntity)

	return
}

// 根据奖励字符串数组 生成奖励对象数组
func (srv *rewardService) MakeRewards(userEntity *entity.UserEntity, rwdStrings []string,
	formulaData interface{}) (rewards []constant.IReward, err error) {
	if len(rwdStrings) == 0 {
		return
	}

	var (
		mainType, subType, val, formulaID uint64
		//可合并的奖励池子
		mergedPool = make(map[uint16]map[uint32]constant.IReward)
		//不可合并的奖励池子
		unMergedPool = make([]constant.IReward, 0)
	)

	for _, rwdString := range rwdStrings {
		tmp := strings.Split(rwdString, "_")
		if len(tmp) < 3 {
			err = util.NewAppError(util.ErrorCodeRewardStringWrong)
			return
		}

		mainType, err = strconv.ParseUint(tmp[0], 10, 16)
		if err != nil {
			return
		}
		subType, err = strconv.ParseUint(tmp[1], 10, 32)
		if err != nil {
			return
		}
		val, err = strconv.ParseUint(tmp[2], 10, 64)
		if err != nil {
			return
		}

		if len(tmp) == 4 {
			formulaID, err = strconv.ParseUint(tmp[3], 10, 32)
			if err != nil {
				return
			}
		}

		// 创建奖励对象
		rewardServe := reward.CreateReward(srv.NewRewardContext(userEntity), uint16(mainType), uint32(subType),
			val, uint16(formulaID))
		// 设置公式计算结果奖励
		rewardServe.SetRealValue(formulaData)

		// 合并同类
		if rewardServe.NeedMerge() {
			if v, exist := mergedPool[rewardServe.GetMainType()][rewardServe.GetSubType()]; exist {
				v.Merge(rewardServe)
			} else {
				if len(mergedPool[rewardServe.GetMainType()]) == 0 {
					mergedPool[rewardServe.GetMainType()] = make(map[uint32]constant.IReward)
				}
				mergedPool[rewardServe.GetMainType()][rewardServe.GetSubType()] = rewardServe
			}
		} else {
			unMergedPool = append(unMergedPool, rewardServe)
		}
	}

	for _, v := range mergedPool {
		for _, r := range v {
			rewards = append(rewards, r)
		}
	}

	rewards = append(rewards, unMergedPool...)

	return
}

// 根据id获取奖励字符串
func (srv *rewardService) GetEquipByID(id uint32) (equipData *entity.EquipData, err error) {
	equipData, err = data.EquipDao.FetchByID(id)
	return
}

func (srv *rewardService) NewRewardContext(userEntity *entity.UserEntity) *constant.RewardContext {
	return &constant.RewardContext{
		UserEntity:   userEntity,
		UserService:  UserService,
		EquipService: HeroEquipService,
	}
}

// 根据 奖励对象数组 生成 奖励字符串数组
func (srv *rewardService) RewardsToRewardStrs(rewards []constant.IReward) (rwdStrings []string) {
	for _, r := range rewards {
		rwdStrings = append(rwdStrings, fmt.Sprintf("%v_%v_%v_0", r.GetMainType(), r.GetSubType(), r.GetVal()))
	}

	return
}
