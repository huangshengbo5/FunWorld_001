package controller

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/util"
	"time"

	"go.uber.org/zap"

	"github.com/gin-gonic/gin"
)

// @Summary 探索列表
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.ExploreListResponse
// @Router /api/explore/list [post]
func ExploreList(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	// 获取探索列表
	userExploreEntitys, err := service.UserExploreService.GetExploresByUid(uid)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获伙英雄列表
	userHeroEntitys, err := service.UserHeroService.GetHerosByUid(uid)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.ExploreListResponse{}

	for _, userHeroEntity := range userHeroEntitys {
		rsp.Heros = append(rsp.Heros, service.PackHero(userHeroEntity))
	}

	for _, userExploreEntity := range userExploreEntitys {
		exploreData, _ := service.UserExploreService.GetExploreData(userExploreEntity.ExploreID)
		rsp.Explores = append(rsp.Explores, service.PackExplore(userExploreEntity, exploreData, userHeroEntitys))
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 探索领奖
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "自增ID"
// @Success 200 {object} msg.ExploreReceiveResponse
// @Router /api/explore/receive [post]
func ExploreReceive(c *gin.Context) {
	req := &msg.ExploreReceiveRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExploreEntity, err := service.UserExploreService.GetExploreByID(req.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//非本人
	if userExploreEntity.Uid != userEntity.ID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "not your explore"))
		return
	}

	// 检查是否符合领奖状态
	now := time.Now().Unix()
	if len(userExploreEntity.HeroIDs) == 0 || now-userExploreEntity.StartTime < entity.SecStep {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeTimeError1))
		return
	}

	exploreData, err := service.UserExploreService.GetExploreData(userExploreEntity.ExploreID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获伙英雄列表
	userHeroEntitys, err := service.UserHeroService.GetHerosByUid(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//获取倍数
	mul := userExploreEntity.Mul(exploreData, userHeroEntitys)
	mul *= float64(util.MinInt64(userExploreEntity.StartTime+exploreData.Duration,
		now)-userExploreEntity.StartTime) / entity.SecStep

	//发放奖励
	rewards, err := service.RewardService.SendRewards(userEntity, exploreData.Rewards, service.SourceExplore, mul)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExploreEntity.StartTime = now
	err = service.UserExploreService.UpdateExplore(userExploreEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.ExploreReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 探索设置
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "探索自增ID"
// @Param heroIDs query []int true "英雄自增ID列表"
// @Success 200 {object} msg.ExploreSetResponse
// @Router /api/explore/set [post]
func ExploreSet(c *gin.Context) {
	req := &msg.ExploreSetRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userExploreEntity, err := service.UserExploreService.GetExploreByID(req.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//非本人
	if userExploreEntity.Uid != userEntity.ID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "not your explore"))
		return
	}

	exploreData, err := service.UserExploreService.GetExploreData(userExploreEntity.ExploreID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获伙英雄列表
	userHeroEntitys, err := service.UserHeroService.GetHerosByUid(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	now := time.Now().Unix()
	var rewards []constant.IReward
	if userExploreEntity.StartTime == 0 {
		userExploreEntity.StartTime = now
	}

	//是否需要清算旧的
	if len(userExploreEntity.HeroIDs) > 0 {
		if now-userExploreEntity.StartTime >= entity.SecStep {
			//获取倍数
			mul := userExploreEntity.Mul(exploreData, userHeroEntitys)

			mul *= float64(util.MinInt64(userExploreEntity.StartTime+exploreData.Duration,
				now)-userExploreEntity.StartTime) / entity.SecStep
			//发放奖励
			rewards, err = service.RewardService.SendRewards(userEntity, exploreData.Rewards, service.SourceExplore, mul)
			if err != nil {
				service.RenderWithAppError(c, err)
				return
			}
			//结算的话重置时间
			userExploreEntity.StartTime = now
		}
	}

	//是否是清空操作
	if len(req.HeroIDs) == 0 && len(userExploreEntity.HeroIDs) > 0 {
		for _, userHeroEntity := range userHeroEntitys {
			for _, heroID := range userExploreEntity.HeroIDs {
				if userHeroEntity.ID == heroID {
					userHeroEntity.ExploreID = 0
					err = service.UserHeroService.UpdateHero(userHeroEntity)
					if err != nil {
						util.GetLogger().Error("explore.set1", zap.Error(err))
					}
				}
			}
		}
		//清空伙伴 清空时间和伙伴列表
		userExploreEntity.StartTime = 0
		userExploreEntity.HeroIDs = entity.Uint32Slice{}
	} else {
		userExploreEntity.HeroIDs = entity.Uint32Slice{}
		for _, heroID := range req.HeroIDs {
			for _, userHeroEntity := range userHeroEntitys {
				if userHeroEntity.ID == heroID && userHeroEntity.IsPartner() {
					if userHeroEntity.ExploreID == 0 || userHeroEntity.ExploreID == userExploreEntity.ExploreID {
						userExploreEntity.HeroIDs = append(userExploreEntity.HeroIDs, heroID)
						userHeroEntity.ExploreID = userExploreEntity.ExploreID
						err = service.UserHeroService.UpdateHero(userHeroEntity)
						if err != nil {
							util.GetLogger().Error("explore.set2", zap.Error(err))
						}
					}
				}
			}
		}
	}

	err = service.UserExploreService.UpdateExplore(userExploreEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.ExploreSetResponse{
		Rewards: service.PackRewards(rewards),
		Explore: service.PackExplore(userExploreEntity, exploreData, userHeroEntitys),
	}

	service.RenderSuccess(c, rsp)
}
