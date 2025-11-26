package controller

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/service/battle"
	"dakunlun/app/util"
	"github.com/gin-gonic/gin"
)

// @Summary 天启界面
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.ApocalypseShowResponse
// @Router /api/apocalypse/show [post]
func ApocalypseShow(c *gin.Context) {
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

	//生成新BOSS
	if userExtendEntity.Apocalypse.BossID == 0 {
		apocalypseData, err := service.UserService.GenTodayApocalypse()
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
		userExtendEntity.Apocalypse.BossID = apocalypseData.ID
		userExtendEntity.Apocalypse.RemainNum = apocalypseData.Limited
		userExtendEntity.Apocalypse.Status = entity.StatusCreated
		err = service.UserService.UpdateUserExtend(userExtendEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}
	// 获取BOSS列表
	rsp := &msg.ApocalypseShowResponse{
		ApocalypseID: userExtendEntity.Apocalypse.BossID,
		RemainNum:    userExtendEntity.Apocalypse.RemainNum,
		Status:       userExtendEntity.Apocalypse.Status,
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 挑战天启BOSS
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param apocalypseID query int true "天启表ID"
// @Param type query int true "类型 0普通 1广告"
// @Param adsID query string true "广告ID"
// @Success 200 {object} msg.ApocalypseAttackResponse
// @Router /api/apocalypse/attack [post]
func ApocalypseAttack(c *gin.Context) {
	req := &msg.ApocalypseAttackRequest{}

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

	if userExtendEntity.Apocalypse.BossID == 0 || userExtendEntity.Apocalypse.BossID != req.ApocalypseID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "no boss"))
		return
	}

	if userExtendEntity.Apocalypse.RemainNum < 1 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "剩余次数不足"))
		return
	}

	if !userExtendEntity.Apocalypse.Status.IsCreated() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "状态错误"))
		return
	}

	apocalypseData, err := service.UserService.GetApocalypseByID(req.ApocalypseID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if userExtendEntity.Apocalypse.RemainNum != apocalypseData.Limited {
		//if req.Type == entity.TypeAds {
		//	err = service.AdsService.CheckAds(userEntity, req.AdsID)
		//	if err != nil {
		//		service.RenderWithAppError(c, err)
		//		return
		//	}
		//	service.AdsService.DropAds(userEntity, req.AdsID)
		//} else {
		//	service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "剩余次数不足"))
		//	return
		//}
	}

	//匹配队友
	userEntitys, err := service.UserService.MatchingForApocalypse(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.ApocalypseAttackResponse{}
	var attacker, defender constant.IFighter
	var attackerRuntime, defenderRuntime *battle.FighterRuntime

	isWin := false
	var remainHP int64
	var bossHP int64
	for _, v := range userEntitys {
		rsp.Players = append(rsp.Players, service.PackPlayer(v))

		if !isWin {
			attacker, attackerRuntime, err = service.BattleService.NewPlayer(v, false)
			defender, defenderRuntime, err = service.BattleService.NewNpc(apocalypseData.NpcID)
			if bossHP == 0 {
				bossHP = defenderRuntime.MaxHP
			}
			if remainHP > 0 {
				defenderRuntime.HP = remainHP
				defenderRuntime.MaxHP = remainHP
			}
			// 开始战斗req.CampaignID或者 campaignEntity.NextID
			battle := &battle.BattleInfo{
				Attacker:   attacker,
				Defender:   defender,
				AtkExt:     attackerRuntime,
				DefExt:     defenderRuntime,
				FightType:  constant.FightTypeApocalypse,
				MaxTime:    constant.CampaignMaxTime,
				Background: 1,
			}

			err = service.BattleService.Fight(battle)
			if err != nil {
				service.RenderWithAppError(c, err)
				return
			}

			//battle.Report.Show()
			isWin = (battle.Result == service.ResultAttackerWin)
			remainHP = defenderRuntime.HP

			rsp.Reports = append(rsp.Reports, battle.Report)
		}
	}

	if isWin {
		//标记胜利
		userExtendEntity.Apocalypse.Status = entity.StatusSuccessful
		userExtendEntity.Apocalypse.Ratio = 1
	} else {
		//标记胜利
		userExtendEntity.Apocalypse.Status = entity.StatusFailed
		userExtendEntity.Apocalypse.Ratio = util.MaxFloat(float64(remainHP)/float64(bossHP), 0.1)
	}

	rwd, _ := service.RewardService.MakeRewards(userEntity, apocalypseData.Rewards, userExtendEntity.Apocalypse.Ratio)
	rsp.Rewards = service.PackRewards(rwd)
	//减少boss战斗次数
	userExtendEntity.Apocalypse.RemainNum -= 1

	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 领取挑战奖励
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param apocalypseID query int true "天启表ID"
// @Param type query int true "类型 0普通 1广告"
// @Param adsID query string true "广告ID"
// @Success 200 {object} msg.ApocalypseReceiveResponse
// @Router /api/apocalypse/receive [post]
func ApocalypseReceive(c *gin.Context) {
	req := &msg.ApocalypseReceiveRequest{}

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

	if userExtendEntity.Apocalypse.BossID == 0 || userExtendEntity.Apocalypse.BossID != req.ApocalypseID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "no boss"))
		return
	}

	if userExtendEntity.Apocalypse.Status.IsCreated() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "状态错误"))
		return
	}

	apocalypseData, err := service.UserService.GetApocalypseByID(req.ApocalypseID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var mul = userExtendEntity.Apocalypse.Ratio
	if req.Type == entity.TypeAds {
		mul *= entity.BossMul
		err = service.AdsService.CheckAds(userEntity, req.AdsID)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	//发放奖励
	rewards, err := service.RewardService.SendRewards(userEntity, apocalypseData.Rewards, service.SourceApocalypse, mul)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//标记领奖
	userExtendEntity.Apocalypse.Status = entity.StatusCreated
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

	rsp := &msg.ApocalypseReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}
