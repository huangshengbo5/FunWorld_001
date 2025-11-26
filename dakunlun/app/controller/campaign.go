package controller

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/service/battle"
	"dakunlun/app/service/data"
	"dakunlun/app/util"

	"github.com/gin-gonic/gin"
)

// @Summary 关卡挑战
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param campaignID query int true "关卡ID"
// @Success 200 {object} msg.CampaignAttackResponse
// @Router /api/campaign/attack [post]
func CampaignAttack(c *gin.Context) {
	req := &msg.CampaignAttackRequest{}

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

	//只能挑战新的关卡
	if req.CampaignID <= userExtendEntity.CampaignID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "不能挑战旧关卡"))
		return
	}

	//TODO 获取关卡数据 看关卡数据是否为空和前置关卡是否完成
	campaignEntity, err := data.CampaignService.GetCampaignByID(req.CampaignID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	//判断用户是否完成了当前关卡的前置关卡
	if campaignEntity.PrevID > 0 && campaignEntity.PrevID != userExtendEntity.CampaignID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "you can not attack this campaign"))
		return
	}

	// 上一关奖励未领取
	if userExtendEntity.CampaignOldID > 0 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "old campain reward has not received"))
		return
	}

	var attacker, defender constant.IFighter
	var attackerRuntime, defenderRuntime *battle.FighterRuntime
	attacker, attackerRuntime, err = service.BattleService.NewPlayer(userEntity, false)
	defender, defenderRuntime, err = service.BattleService.NewNpc(campaignEntity.NpcID)

	// 开始战斗req.CampaignID或者 campaignEntity.NextID
	battle := &battle.BattleInfo{
		Attacker:   attacker,
		Defender:   defender,
		AtkExt:     attackerRuntime,
		DefExt:     defenderRuntime,
		FightType:  constant.FightTypeCampaign,
		MaxTime:    constant.CampaignMaxTime,
		Background: campaignEntity.BackgroundID,
	}

	err = service.BattleService.Fight(battle)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//battle.Report.Show()

	isWin := (battle.Result == service.ResultAttackerWin)
	//var rewards []constant.IReward
	//battle.Report.Show()
	var heroID uint32
	var heroLevel uint16
	// 胜利需要记录通关记录和通关时间点并发奖
	if isWin {
		userEntity.CampaignNum += 1
		userExtendEntity.CampaignID = req.CampaignID
		userExtendEntity.CampaignTime = util.Carbon().Now().ToTimestamp()
		//发放奖励
		//rewards, err = service.RewardService.SendRewards(userEntity, campaignEntity.Rewards, service.SourceCampaign, nil)
		//if err != nil {
		//	service.RenderWithAppError(c, err)
		//	return
		//}
		userExtendEntity.CampaignOldID = req.CampaignID
		err = service.UserService.UpdateUser(userEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		err = service.UserService.UpdateUserExtend(userExtendEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		//解锁英雄
		if campaignEntity.HeroID > 0 {
			var userHeroEntity *entity.UserHeroEntity
			userHeroEntity, err = service.UserHeroService.AddHero(userEntity, campaignEntity.HeroID, 0)
			if err != nil {
				service.RenderWithAppError(c, err)
				return
			}
			heroID = userHeroEntity.HeroID
			heroLevel = userHeroEntity.Level
		}

		//解锁建筑
		_, err := service.UserBuildingService.Check(userEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		//解锁水晶
		if campaignEntity.ID == 2 {
			for _, id := range [7]uint32{1, 2, 3, 4, 5, 6, 7} {
				service.UserCrystalService.AddCrystal(userEntity, id)

			}

		}
	}

	rsp := &msg.CampaignAttackResponse{
		Win: isWin,
		//Rewards:   service.PackRewards(rewards),
		Report:    battle.Report,
		HeroID:    heroID,
		HeroLevel: heroLevel,
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 关卡领奖
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param type query int true "类型 0普通 1广告"
// @Param adsID query string true "广告ID"
// @Success 200 {object} msg.CampaignReceiveResponse
// @Router /api/campaign/receive [post]
func CampaignReceive(c *gin.Context) {
	req := &msg.CampaignReceiveRequest{}

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

	if userExtendEntity.CampaignOldID == 0 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "no reward"))
		return
	}

	campaignEntity, err := data.CampaignService.GetCampaignByID(userExtendEntity.CampaignOldID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var mul = 1
	if req.Type == entity.TypeAds && userExtendEntity.CampaignOldID%5 == 0 {
		mul = entity.CampaignMul
		err = service.AdsService.CheckAds(userEntity, req.AdsID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	//发放奖励
	rewards, err := service.RewardService.SendRewards(userEntity, campaignEntity.Rewards, service.SourceCampaign, mul)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity.CampaignOldID = 0
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

	rsp := &msg.CampaignReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}
