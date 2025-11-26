package controller

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/util"

	"github.com/gin-gonic/gin"
)

// @Summary 广告开始
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.AdsStartResponse
// @Router /api/ads/start [post]
func AdsStart(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	adsID, expireTime, err := service.AdsService.StartAds(userEntity)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	rsp := &msg.AdsStartResponse{
		AdsID:      adsID,
		ExpireTime: expireTime,
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 广告领奖
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param ID query int true "ID"
// @Success 200 {object} msg.AdsReceiveResponse
// @Router /api/ads/receive [post]
func AdsReceive(c *gin.Context) {
	req := &msg.AdsReceiveRequest{}

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

	adsData, err := service.AdsService.GetAdsReward(req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//广告次数不够
	if userExtendEntity.Ads.AdsNum < adsData.Val {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "次数不足"))
		return
	}

	//不能重复领取
	if util.ContainsUint32(userExtendEntity.Ads.IDs, adsData.ID) {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "重复领取"))
		return
	}

	//发放奖励
	rewards, err := service.RewardService.SendRewards(userEntity, adsData.Rewards, service.SourceAds, nil)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//保存用户状态
	userExtendEntity.Ads.IDs = append(userExtendEntity.Ads.IDs, adsData.ID)
	err = service.UserService.UpdateUserExtend(userExtendEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.AdsReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 图腾
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param adsID query string true "广告ID"
// @Success 200 {object} msg.AddBuffRequest
// @Router /api/ads/add/buff [post]
func AddBuff(c *gin.Context) {
	req := &msg.AddBuffRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//err = service.AdsService.CheckAds(userEntity, req.AdsID)
	//if err != nil {
	//	service.RenderWithAppError(c, err)
	//	return
	//}
	//service.AdsService.DropAds(userEntity, req.AdsID)
	err = service.UserService.DecrAssets(userEntity, constant.CostTypeDiamond, 0, 20)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	err = userEntity.IncrGoldBuff(entity.GoldBuffIncr)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	err = service.UserService.UpdateUser(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.AddBuffResponse{
		Gold:          userEntity.CurGold(),
		GoldFlushIn:   userEntity.GoldFlushIn,
		GoldIncrement: userEntity.GoldIncrement(),
		GoldBuffEndIn: userEntity.GoldBuffEndTime,
	}

	service.RenderSuccess(c, rsp)
}
