package controller

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/service/battle"
	"dakunlun/app/service/reward"
	"dakunlun/app/util"
	"errors"
	"fmt"
	"time"

	"github.com/go-redis/redis/v8"

	"github.com/gin-gonic/gin"
	"github.com/spf13/cast"
)

// @Summary 新手引导步骤号设置
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param step query int true "步骤号 0-255"
// @Success 200 {object} msg.GuideStepResponse
// @Router /api/lobby/guide/step [post]
func GuideStep(c *gin.Context) {
	req := &msg.GuideStepRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	//设置步骤号
	userEntity.Ftue = req.Step

	err = service.UserService.UpdateUser(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.GuideStepResponse{
		Step: userEntity.Ftue,
	}

	service.RenderSuccess(c, rsp)
}

// @Summary
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.GuideBattleResponse
// @Router /api/lobby/guide/battle [post]
func GuideBattle(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var attacker, defender constant.IFighter
	var attackerRuntime, defenderRuntime *battle.FighterRuntime
	attacker, attackerRuntime, err = service.BattleService.NewPlayer(userEntity, false)
	defender, defenderRuntime, err = service.BattleService.NewNpc(99999)

	// 开始战斗req.CampaignID或者 campaignEntity.NextID
	battle := &battle.BattleInfo{
		Attacker:   attacker,
		Defender:   defender,
		AtkExt:     attackerRuntime,
		DefExt:     defenderRuntime,
		FightType:  constant.FightTypeCampaign,
		MaxTime:    constant.CampaignMaxTime,
		Background: 2,
	}

	err = service.BattleService.Fight(battle)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//battle.Report.Show()

	rsp := &msg.GuideBattleResponse{
		Win:    (battle.Result == service.ResultAttackerWin),
		Report: battle.Report,
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 修改头像
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "头像表ID"
// @Success 200 {object} msg.ChangeAvatarResponse
// @Router /api/lobby/change/avatar [post]
func ChangeAvatar(c *gin.Context) {
	req := &msg.ChangeAvatarRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if userEntity.Avatar == uint16(req.ID) {
		//service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "select same avatar"))
		//return
	}

	avatarData, err := service.UserService.GetAvatarByID(req.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if !avatarData.CanUse {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "wrong avatar id"))
		return
	}

	userEntity.Avatar = uint16(avatarData.ID)
	err = service.UserService.UpdateUser(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.ChangeAvatarResponse{}

	service.RenderSuccess(c, rsp)
}

// @Summary 修改名字
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param name query string true "名字"
// @Success 200 {object} msg.ChangeNameResponse
// @Router /api/lobby/change/name [post]
func ChangeName(c *gin.Context) {
	req := &msg.ChangeNameRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	ok, err := service.UserService.CheckName(req.Name)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorNameIsUsed))
		return
	}

	// 改过名字的话 需要扣1000钻石
	if userEntity.Name != "" {
		// 扣除
		err = service.UserService.DecrAssets(userEntity, constant.CostTypeDiamond, 0, entity.ChangeNameCost)
		if err != nil {
			return
		}
	}

	userEntity.Name = req.Name

	err = service.UserService.UpdateUser(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.ChangeNameResponse{}

	service.RenderSuccess(c, rsp)
}

// @Summary 用户信息
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.UserInfoResponse
// @Router /api/lobby/user [post]
func UserInfo(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.UserInfoResponse{
		User:   service.PackUserResponse(userEntity),
		Extend: service.PackUserExtendResponse(userExtendEntity),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 查看在线奖励
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param adsID query string true "广告ID"
// @Param type query int true "方式 0正常 1广告 2钻石"
// @Success 200 {object} msg.OnlineRewardShowResponse
// @Router /api/lobby/onlinereward/show [post]
func OnlineRewardShow(c *gin.Context) {
	req := &msg.OnlineRewardShowRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//有奖励未领取
	if len(userExtendEntity.OnlineReward.RewardIDs) > 0 {
		rsp := &msg.OnlineRewardShowResponse{
			RewardIDs: userExtendEntity.OnlineReward.RewardIDs,
		}
		service.RenderSuccess(c, rsp)
		return
	}

	now := time.Now().Unix()
	switch req.Type {
	case entity.TypeNormal: //正常
		//cd时间未到
		if userExtendEntity.OnlineReward.NextReceiveTime > now {
			service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "time is not at"))
			return
		}
	case entity.TypeAds: //广告
		//剩余广告次数为0
		if userExtendEntity.OnlineReward.RemainAdsNum-1 < 0 {
			service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "ads num reach the limited"))
			return
		}

		err = service.AdsService.CheckAds(userEntity, req.AdsID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		userExtendEntity.OnlineReward.RemainAdsNum -= 1
	case entity.TypeGem: //钻石
		//消耗钻石
		err = service.UserService.DecrAssets(userEntity, constant.CostTypeDiamond, 0,
			uint64(userExtendEntity.NeedDiamondOnlineReward()))
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
		userExtendEntity.OnlineReward.PayNum += 1
		err = service.UserService.UpdateUser(userEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	//生成奖励
	if userExtendEntity.OnlineReward.GetFirst {
		userExtendEntity.OnlineReward.RewardIDs, err = service.LobbyService.RandomOnlineReward()
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	} else {
		userExtendEntity.OnlineReward.RewardIDs = []uint32{1, 2, 3, 4}
		userExtendEntity.OnlineReward.GetFirst = true
	}
	//清空cd
	userExtendEntity.OnlineReward.NextReceiveTime = 0

	//更新用户扩展信息
	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	err = service.AdsService.DropAds(userEntity, req.AdsID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.OnlineRewardShowResponse{
		RewardIDs: userExtendEntity.OnlineReward.RewardIDs,
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 领取在线奖励
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param type query int true "方式 0正常 1广告 2钻石"
// @Param AdsID query string false "广告ID"
// @Success 200 {object} msg.OnlineRewardReceiveResponse
// @Router /api/lobby/onlinereward/receive [post]
func OnlineRewardReceive(c *gin.Context) {
	req := &msg.OnlineRewardReceiveRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if len(userExtendEntity.OnlineReward.RewardIDs) == 0 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "no rewards"))
		return
	}

	var mul float64 = 1
	if req.Type == entity.TypeAds {
		mul = entity.OnlineRewardMul
		//查看广告
		err = service.AdsService.CheckAds(userEntity, req.AdsID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	//获取奖励
	rewardStrs, err := service.LobbyService.GetOnlineReward(userExtendEntity.OnlineReward.RewardIDs)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//发放奖励
	rewards, err := service.RewardService.SendRewards(userEntity, rewardStrs, service.SourceOnlineReward, mul)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity.ResetOnlineReward()
	//更新用户扩展信息
	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if req.Type == entity.TypeAds {
		// 删除广告记录
		err = service.AdsService.DropAds(userEntity, req.AdsID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	rsp := &msg.OnlineRewardReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 怪物入侵大地图
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.MonsterShowResponse
// @Router /api/lobby/monster/show [post]
func MonsterShow(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(uid)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if userExtendEntity.CampaignID < 15 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "need attack campain 15"))
		return
	}

	towerLevel := userExtendEntity.WhichTower()
	if towerLevel == 0 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeTowerNotOpen))
		return
	}

	cacheKey := fmt.Sprintf("tower:%d:%d", userExtendEntity.ID, towerLevel)
	v, err := util.GetRedisClient().Get(c, cacheKey).Result()
	if err != nil {
		if errors.Is(err, redis.Nil) {
			err := util.GetRedisClient().Set(c, cacheKey, userExtendEntity.CampaignID, time.Hour*3).Err()
			if err != nil {
				service.RenderWithAppError(c, err)
				return
			}
		} else {
			service.RenderWithAppError(c, err)
			return
		}
	}

	var campaignID uint32
	if v != "" {
		campaignID = cast.ToUint32(v)
	} else {
		campaignID = userExtendEntity.CampaignID
	}

	rsp := &msg.MonsterShowResponse{
		CampaignID: campaignID,
	}

	for _, v := range userExtendEntity.Tower[towerLevel] {
		rsp.Monsters = append(rsp.Monsters, &msg.Monster{
			TowerID: v.TowerID,
			Status:  v.Status,
		})
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 怪物入侵挑战
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param index query int true "列表index"
// @Success 200 {object} msg.MonsterAttackResponse
// @Router /api/lobby/monster/attack [post]
func MonsterAttack(c *gin.Context) {
	req := &msg.MonsterAttackRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	towerLevel := userExtendEntity.WhichTower()
	if towerLevel == 0 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeTowerNotOpen))
		return
	}

	if userExtendEntity.Tower[towerLevel][req.Index].Status.IsSuccessful() || userExtendEntity.Tower[towerLevel][req.Index].Status.IsDone() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "状态错误"))
		return
	}

	towerData, err := service.LobbyService.GetTowerByID(userExtendEntity.Tower[towerLevel][req.Index].TowerID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var attacker, defender constant.IFighter
	var attackerRuntime, defenderRuntime *battle.FighterRuntime
	attacker, attackerRuntime, err = service.BattleService.NewPlayer(userEntity, false)
	defender, defenderRuntime, err = service.BattleService.NewNpc(towerData.NpcID)

	// 开始战斗req.CampaignID或者 campaignEntity.NextID
	battle := &battle.BattleInfo{
		Attacker:   attacker,
		Defender:   defender,
		AtkExt:     attackerRuntime,
		DefExt:     defenderRuntime,
		FightType:  constant.FightTypeTower,
		MaxTime:    constant.CampaignMaxTime,
		Background: 1,
	}

	err = service.BattleService.Fight(battle)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	isWin := (battle.Result == service.ResultAttackerWin)
	var rewardStrs entity.RewardStrings
	if isWin {
		//标记胜利
		userExtendEntity.Tower[towerLevel][req.Index].Status = entity.StatusSuccessful
		err = service.UserService.UpdateUserExtend(userExtendEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		for _, v := range []struct {
			probability int
			rewards     entity.RewardStrings
		}{
			{
				towerData.Rewards1Probability,
				towerData.Rewards1,
			},
			{
				towerData.Rewards2Probability,
				towerData.Rewards2,
			},
			{
				towerData.Rewards3Probability,
				towerData.Rewards3,
			},
			{
				towerData.Rewards4Probability,
				towerData.Rewards4,
			},
		} {
			if util.JudgeProbability(v.probability) {
				rewardStrs = append(rewardStrs, v.rewards...)
			}
		}

		userExtendEntity.Tower[towerLevel][req.Index].Rewards = rewardStrs
	}

	rwd, _ := service.RewardService.MakeRewards(userEntity, rewardStrs, nil)
	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	rsp := &msg.MonsterAttackResponse{
		Win:     isWin,
		Report:  battle.Report,
		Rewards: service.PackRewards(rwd),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 领取挑战奖励
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param index query int true "列表index"
// @Param type query int true "类型 0普通 1广告"
// @Param adsID query string true "广告ID"
// @Success 200 {object} msg.MonsterReceiveResponse
// @Router /api/lobby/monster/receive [post]
func MonsterReceive(c *gin.Context) {
	req := &msg.MonsterReceiveRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	towerLevel := userExtendEntity.WhichTower()
	if towerLevel == 0 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeTowerNotOpen))
		return
	}

	if !userExtendEntity.Tower[towerLevel][req.Index].Status.IsSuccessful() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "状态错误"))
		return
	}

	//towerData, err := service.LobbyService.GetTowerByID(userExtendEntity.Tower[towerLevel][req.Index].TowerID)
	//if err != nil {
	//	service.RenderWithAppError(c, err)
	//	return
	//}

	//发放奖励
	//var rewardStrs entity.RewardStrings
	//for _, v := range []struct {
	//	probability int
	//	rewards     entity.RewardStrings
	//}{
	//	{
	//		towerData.Rewards1Probability,
	//		towerData.Rewards1,
	//	},
	//	{
	//		towerData.Rewards2Probability,
	//		towerData.Rewards2,
	//	},
	//	{
	//		towerData.Rewards2Probability,
	//		towerData.Rewards2,
	//	},
	//	{
	//		towerData.Rewards2Probability,
	//		towerData.Rewards2,
	//	},
	//} {
	//	if util.JudgeProbability(v.probability) {
	//		rewardStrs = append(rewardStrs, v.rewards...)
	//	}
	//}

	var mul float64 = 1
	if req.Type == entity.TypeAds {
		mul = entity.TowerMul
		err = service.AdsService.CheckAds(userEntity, req.AdsID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	rewards, err := service.RewardService.SendRewards(userEntity, userExtendEntity.Tower[towerLevel][req.Index].Rewards,
		service.SourceTower, mul)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	err = service.AdsService.DropAds(userEntity, req.AdsID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//标记领奖
	userExtendEntity.Tower[towerLevel][req.Index].Status = entity.StatusDone
	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.MonsterReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 开始炼药
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param type query int true "方式 0正常 1广告"
// @Param adsID query string true "广告ID"
// @Success 200 {object} msg.AlchemyStartResponse
// @Router /api/lobby/alchemy/start [post]
func AlchemyStart(c *gin.Context) {
	req := &msg.AlchemyStartRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if userExtendEntity.AlchemyInCD() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "冷却中"))
		return
	}

	userCrystalEntitys, err := service.UserCrystalService.GetCrystalsByUid(userExtendEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var totalLevel uint16
	for _, userCrystalEntity := range userCrystalEntitys {
		totalLevel += userCrystalEntity.Level
	}

	alchemyData, err := service.UserService.GetAlchemyByAttr(uint32(totalLevel))
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	err = service.UserService.DecrAssets(userEntity, alchemyData.CostType, alchemyData.CostSubType, alchemyData.CostVal)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//更新CD
	userExtendEntity.Alchemy.NextReceiveTime = time.Now().Unix() + alchemyData.CdTimes*60

	var mul float64 = 1
	if req.Type == entity.TypeAds {
		mul = entity.AlchemyMul
		err = service.AdsService.CheckAds(userEntity, req.AdsID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	rewards, err := service.RewardService.SendRewards(userEntity, alchemyData.Rewards, service.SourceAlchemy, mul)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.AlchemyStartResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 炼药CD清除
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.AlchemyClearRequest
// @Router /api/lobby/alchemy/clear [post]
func AlchemyClear(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if !userExtendEntity.AlchemyInCD() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "冷却中"))
		return
	}

	//消耗钻石
	err = service.UserService.DecrAssets(userEntity, constant.CostTypeDiamond, 0,
		uint64(userExtendEntity.NeedDiamondAlchemy()))
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	//更新CD
	userExtendEntity.Alchemy.NextReceiveTime = 0
	userExtendEntity.Alchemy.PayNum += 1

	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.AlchemyClearResponse{}

	service.RenderSuccess(c, rsp)
}

// @Summary 系统配置，每日0点刷一次，每次登录刷一次
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.SystemConfigResponse
// @Router /api/lobby/system/config [post]
func SystemConfig(c *gin.Context) {
	rsp := msg.SystemConfigResponse{
		TowerSettings:       make(map[string]msg.FromTo, 8),
		BusinessManSettings: make(map[string]msg.FromTo, 2),
	}

	for i := uint8(entity.TowerEight); i >= entity.TowerOne; i-- {
		rsp.TowerSettings[fmt.Sprintf("additionalProp%d", i)] = msg.FromTo{
			From: entity.TimeSettings[i].StartTime().ToTimestamp(),
			To:   entity.TimeSettings[i].EndTime().ToTimestamp(),
		}
	}

	rsp.BusinessManSettings["additionalProp1"] = msg.FromTo{
		From: entity.TimeSettings[entity.BusinessOne].StartTime().ToTimestamp(),
		To:   entity.TimeSettings[entity.BusinessOne].EndTime().ToTimestamp(),
	}

	rsp.BusinessManSettings["additionalProp2"] = msg.FromTo{
		From: entity.TimeSettings[entity.BusinessTwo].StartTime().ToTimestamp(),
		To:   entity.TimeSettings[entity.BusinessTwo].EndTime().ToTimestamp(),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 成就首页
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.AnnalsShowResponse
// @Router /api/lobby/annals/show [post]
func AnnalsShow(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	heroEntitys, err := service.UserHeroService.GetHerosByUid(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userAnnalsEntity, err := service.UserService.GetOrCreateAnnals(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获取总战力
	v, _ := service.UserService.GetTotalFightingCapacity(userEntity)
	rsp := &msg.AnnalsShowResponse{
		CampaignNum:      userEntity.CampaignNum,
		FightingCapacity: v,
		AnnalsIDs:        userAnnalsEntity.DoneList,
	}

	for _, heroEntity := range heroEntitys {
		if !heroEntity.IsMainHero() {
			rsp.HeroList = append(rsp.HeroList, &msg.HeroAnnals{
				HeroID:           heroEntity.HeroID,
				FightingCapacity: heroEntity.GetFightingCapacity(),
			})
		}
		rsp.FightingCapacity += heroEntity.GetFightingCapacity()
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 成就奖励领取
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "成就ID"
// @Success 200 {object} msg.AnnalsReceiveResponse
// @Router /api/lobby/annals/receive [post]
func AnnalsReceive(c *gin.Context) {
	req := &msg.AnnalsReceiveRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userAnnalsEntity, err := service.UserService.GetOrCreateAnnals(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if !userAnnalsEntity.AddAnnals(req.ID) {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "already received"))
		return
	}

	annalsData, err := service.UserService.GetAnnalsData(req.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	ok, err := service.UserService.CheckAnnalsCondition(userEntity, annalsData)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, ""))
		return
	}

	//更新annals
	err = service.UserService.UpdateUserAnnals(userAnnalsEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//发放奖励
	rewards, err := service.RewardService.SendRewards(userEntity, annalsData.Rewards, service.SourceAnnals, nil)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.AnnalsReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 神秘商人奖励
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param type query int true "类型 0普通 1广告"
// @Param adsID query string true "广告ID"
// @Success 200 {object} msg.BusinessManReceiveResponse
// @Router /api/lobby/businessman/receive [post]
func BusinessManReceive(c *gin.Context) {
	req := &msg.BusinessManReceiveRequest{}
	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	businessManID := userExtendEntity.WhichBusinessMan()
	if !util.ContainsUint8([]uint8{entity.BusinessOne, entity.BusinessTwo}, businessManID) {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeBusinessManNotOpen))
		return
	}

	var businessFlag int
	if businessManID == entity.BusinessOne {
		businessFlag = entity.BusinessOneFlag
	} else {
		businessFlag = entity.BusinessTwoFlag
	}

	if (userExtendEntity.BusinessMan & businessFlag) == businessFlag {
		service.RenderWithAppError(c, util.NewAppError(2001))
		return
	}

	userExtendEntity.BusinessMan |= businessFlag

	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var mul = 1
	if req.Type == entity.TypeAds {
		mul = entity.BusinessManMul
		err = service.AdsService.CheckAds(userEntity, req.AdsID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
		err = service.AdsService.DropAds(userEntity, req.AdsID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	var rewardStrs []string
	rewardStrs = append(rewardStrs, fmt.Sprintf("%v_%v_%v_0", reward.RewardTypeGold, 0,
		int(userEntity.GoldIncrement())*mul*7200))

	//发放奖励
	rewards, err := service.RewardService.SendRewards(userEntity, rewardStrs, service.SourceBusinessMan, nil)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.BusinessManReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 铸造
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.CastResponse
// @Router /api/lobby/cast [post]
func Cast(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//获取铸造数据
	userEntity.CastEffect.CastID += 1
	castData, err := service.UserService.GetCastByID(userEntity.CastEffect.CastID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if castData == nil {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "no cast data"))
		return
	}

	//扣除资源
	err = service.UserService.DecrAssets(userEntity, castData.CostType, castData.CostSubType, castData.CostVal)
	if err != nil {
		return
	}

	//更新战力
	userEntity.CastEffect.FightingCapacityPlus = castData.FightingCapacity
	//更新用户数据
	err = service.UserService.UpdateUser(userEntity)
	if err != nil {
		return
	}

	rsp := &msg.CastResponse{
		ID: userEntity.CastEffect.CastID,
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 铸造
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "商品ID"
// @Success 200 {object} msg.BuyResponse
// @Router /api/lobby/buy [post]
func Buy(c *gin.Context) {
	req := &msg.BuyRequest{}
	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rewards, err := service.LobbyService.Buy(userEntity, req.ID)
	if err != nil {
		switch err.(type) {
		case *util.AppError:
			rsp := &msg.BuyResponse{
				Error: true,
			}
			service.RenderSuccess(c, rsp)
			return
		}

		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.BuyResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}
