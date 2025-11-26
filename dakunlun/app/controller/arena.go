package controller

import (
	"context"
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/service/battle"
	"dakunlun/app/util"
	"fmt"
	"time"

	"go.uber.org/zap"

	"github.com/gin-gonic/gin"
)

// @Summary 报名参赛
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.ArenaSignUpResponse
// @Router /api/arena/signup [post]
func ArenaSignUp(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//是否符合报名时间
	if !service.UserArenaService.InSingUpDay() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "not open"))
		return
	}

	userArenaEntity, err := service.UserArenaService.GetUserArenaOrCreate(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//禁止重复报名
	if userArenaEntity.IsSign() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "already signup"))
		return
	}

	//报名
	userArenaEntity.SignUp()
	service.UserArenaService.UpdateArena(userArenaEntity)

	//更新用户扩展信息
	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	userExtendEntity.ArenaSignWeek = userArenaEntity.SignWeek
	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	rsp := &msg.ArenaSignUpResponse{
		ArenaIsSign: userExtendEntity.ArenaIsSign(),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 竞技场列表
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.ArenaListResponse
// @Router /api/arena/list [post]
func ArenaList(c *gin.Context) {
	req := &msg.ArenaListRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userArenaEntity, err := service.UserArenaService.GetUserArenaOrCreate(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//未参赛
	if !userArenaEntity.IsSign() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "not signup"))
		return
	}

	//获取排行榜列表
	userArenaEntitys, pageInfo, err, inRebuild := service.UserArenaService.GetRankers(userArenaEntity, req.Page,
		req.PerPage)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if inRebuild {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeRankInRebuild))
		return
	}

	records, _ := service.UserArenaService.GetRecords(userArenaEntity.Uid)

	rsp := &msg.ArenaListResponse{
		PageInfo: pageInfo,
		Self:     service.PackRanker(userArenaEntity),
		Rankers:  make([]*msg.Ranker, 0, len(userArenaEntitys)),
		Records:  records,
	}
	for _, v := range userArenaEntitys {
		rsp.Rankers = append(rsp.Rankers, service.PackRanker(v))
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 竞技场列表
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "ID"
// @Success 200 {object} msg.ArenaAttackResponse
// @Router /api/arena/attack [post]
func ArenaAttack(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	req := &msg.ArenaAttackRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity, err := service.UserService.GetUserExtendByID(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if userExtendEntity.ArenaRemainNum < 1 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "剩余次数不足"))
		return
	}

	//读取攻击方
	attackArenaEntity, err := service.UserArenaService.GetUserArenaByUid(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	if attackArenaEntity == nil {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "防守方不存在"))
		return
	}

	//读取防御方
	defendArenaEntity, err := service.UserArenaService.GetUserArenaByID(req.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if defendArenaEntity == nil {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "defender not exist"))
		return
	}

	//未参赛
	if !attackArenaEntity.IsSign() || !defendArenaEntity.IsSign() || attackArenaEntity.GroupID != defendArenaEntity.
		GroupID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "nosign or not in same group"))
		return
	}

	var attacker, defender constant.IFighter
	var attackerRuntime, defenderRuntime *battle.FighterRuntime
	attacker, attackerRuntime, err = service.BattleService.NewPlayer(userEntity, defendArenaEntity.IsRealPerson())

	if defendArenaEntity.IsRealPerson() {
		defenderUserEntity, err := service.UserService.GetUserByID(defendArenaEntity.Uid)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
		defender, defenderRuntime, err = service.BattleService.NewPlayer(defenderUserEntity, defendArenaEntity.IsRealPerson())
	} else {
		defender, defenderRuntime, err = service.BattleService.NewNpc(defendArenaEntity.Uid)
	}

	// 开始战斗req.CampaignID或者 campaignEntity.NextID
	battle := &battle.BattleInfo{
		Attacker:   attacker,
		Defender:   defender,
		AtkExt:     attackerRuntime,
		DefExt:     defenderRuntime,
		FightType:  constant.FightTypeArena,
		MaxTime:    constant.CampaignMaxTime,
		Background: 1,
	}

	//用户状态锁
	prefix := "match"
	ttl := 10 * time.Second
	attackerKey := fmt.Sprintf("%s:%v", prefix, attackArenaEntity.ID)
	defenderKey := fmt.Sprintf("%s:%v", prefix, defendArenaEntity.ID)
	result1, err1 := util.GetRedisClient().SetNX(context.Background(), attackerKey, "1", ttl).Result()
	result2, err2 := util.GetRedisClient().SetNX(context.Background(), defenderKey, "1", ttl).Result()

	//释放锁
	defer func() {
		util.GoPool().Submit(func() {
			err := util.GetRedisClient().Del(context.Background(), attackerKey, defenderKey).Err()
			if err != nil {
				util.GetLogger().Error(fmt.Sprintf("del %v and %v : %v", attackerKey, defenderKey, err.Error()))
			}
		})
	}()

	if err1 != nil || err2 != nil || !result1 || !result2 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeInFighting))
		return
	}

	//设置缓存时间
	defer func() {
		util.GoPool().Submit(func() {
			err = util.GetRedisClient().Expire(context.Background(), attackerKey, ttl).Err()
			if err != nil {
				util.GetLogger().Error(fmt.Sprintf("set expire %v : %v", attackerKey, err.Error()))
			}
			err = util.GetRedisClient().Expire(context.Background(), defenderKey, ttl).Err()
			if err != nil {
				util.GetLogger().Error(fmt.Sprintf("set expire %v : %v", defenderKey, err.Error()))
			}
		})
	}()

	err = service.BattleService.Fight(battle)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//battle.Report.Show()

	isWin := (battle.Result == service.ResultAttackerWin)
	rewardStrings := []string{}
	oldRank := defendArenaEntity.Rank
	if isWin && attackArenaEntity.Rank > defendArenaEntity.Rank {
		//交换排名
		attackArenaEntity.Rank, defendArenaEntity.Rank = defendArenaEntity.Rank, attackArenaEntity.Rank
		err = service.UserArenaService.UpdateArena(attackArenaEntity)
		err = service.UserArenaService.UpdateArena(defendArenaEntity)
		rewardStrings = []string{entity.RewardWin}
	} else {
		rewardStrings = []string{entity.RewardLose}
	}

	//记录日志
	util.GoPool().Submit(func() {
		if defendArenaEntity.IsRealPerson() {
			err := service.UserArenaService.AddRecord(attackArenaEntity, defendArenaEntity, isWin, oldRank)
			if err != nil {
				util.GetLogger().Error("AddRecord", zap.Error(err))
			}
		}
	})

	rewards, err := service.RewardService.SendRewards(userEntity, rewardStrings, service.SourceArena, nil)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExtendEntity.ArenaRemainNum -= 1
	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	//报名
	//service.UserArenaService.GetRankers(userArenaEntity)

	rsp := &msg.ArenaAttackResponse{
		Win:     isWin,
		Rewards: service.PackRewards(rewards),
		Report:  battle.Report,
	}

	service.RenderSuccess(c, rsp)
}
