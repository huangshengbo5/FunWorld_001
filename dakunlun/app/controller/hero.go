package controller

import (
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/util"

	"github.com/gin-gonic/gin"
)

// @Summary 伙伴列表
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.HeroListResponse
// @Router /api/hero/list [post]
func HeroList(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	// 获伙英雄列表
	userHeroEntitys, err := service.UserHeroService.GetHerosByUid(uid)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.HeroListResponse{}

	for _, userHeroEntity := range userHeroEntitys {
		//if userHeroEntity.IsPartner() {
		rsp.Heros = append(rsp.Heros, service.PackHero(userHeroEntity))
		//}
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 设置伙伴出战
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "伙伴id"
// @Success 200 {object} msg.HeroJoinResponse
// @Router /api/hero/join [post]
func HeroJoin(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	req := &msg.HeroJoinRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获取英雄
	userHeroEntity, err := service.UserHeroService.GetHeroByID(req.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity.SubHeroID = userHeroEntity.ID
	service.UserService.UpdateUser(userEntity)

	rsp := &msg.HeroJoinResponse{}

	service.RenderSuccess(c, rsp)
}

// @Summary 设置伙伴下阵
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.HeroUnloadResponse
// @Router /api/hero/unload [post]
func HeroUnload(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	if userEntity.SubHeroID == 0 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "no sub hero"))
		return
	}

	userEntity.SubHeroID = 0
	service.UserService.UpdateUser(userEntity)

	rsp := &msg.HeroUnloadResponse{}

	service.RenderSuccess(c, rsp)
}

// @Summary 伙伴升级
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "伙伴id"
// @Success 200 {object} msg.HeroUpgradeResponse
// @Router /api/hero/upgrade [post]
func HeroUpgrade(c *gin.Context) {
	req := &msg.HeroUpgradeRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userHeroEntity, err := service.UserHeroService.Upgrade(userEntity, req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.HeroUpgradeResponse{
		userHeroEntity.Level,
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 伙伴锻魂
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "伙伴id"
// @Success 200 {object} msg.HeroEvolveResponse
// @Router /api/hero/evolve [post]
func HeroEvolve(c *gin.Context) {
	req := &msg.HeroEvolveRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if userEntity.MainHeroID == req.ID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "main hero can not evolve"))
		return
	}

	userHeroEntity, err := service.UserHeroService.Evolve(userEntity, req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.HeroEvolveResponse{
		EvolveTimes: userHeroEntity.EvolveTimes,
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 看皮肤广告
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "伙伴id"
// @Param skinID query int true "皮肤ID"
// @Success 200 {object} msg.HeroSkinGetResponse
// @Router /api/hero/skin/get [post]
func HeroSkinGet(c *gin.Context) {
	req := &msg.HeroSkinGetRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	// 获取英雄
	userHeroEntity, err := service.UserHeroService.GetHeroByID(req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if uid != userHeroEntity.Uid {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, ""))
		return
	}

	// 获取英雄皮肤
	heroSkinData, err := service.UserHeroService.GetHeroSkin(req.SkinID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 皮肤英雄不匹配
	if heroSkinData.HeroID != userHeroEntity.ID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, ""))
		return
	}

	if _, exist := userHeroEntity.SkinMap[req.SkinID]; !exist {
		userHeroEntity.SkinMap[req.SkinID] = &entity.Skin{}
	}

	if heroSkinData.GetType == entity.TypeAds {
		userHeroEntity.SkinMap[req.SkinID].Val += 1
		if userHeroEntity.SkinMap[req.SkinID].Val >= heroSkinData.GetValue {
			userHeroEntity.SkinMap[req.SkinID].FightingCapacity = heroSkinData.FightingCapacity
			userHeroEntity.SkinMap[req.SkinID].Active = true
		}
	}

	err = service.UserHeroService.UpdateHero(userHeroEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.HeroSkinGetResponse{
		Skin: userHeroEntity.SkinMap[req.SkinID],
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 使用皮肤
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "伙伴id"
// @Param skinID query int true "皮肤IO"
// @Success 200 {object} msg.HeroSkinUseResponse
// @Router /api/hero/skin/use [post]
func HeroSkinUse(c *gin.Context) {
	req := &msg.HeroSkinUseRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	// 获取英雄
	userHeroEntity, err := service.UserHeroService.GetHeroByID(req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if uid != userHeroEntity.Uid {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, ""))
		return
	}

	if req.SkinID > 0 {
		if v, exist := userHeroEntity.SkinMap[req.SkinID]; !exist || !v.Active {
			service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "skin not exist"))
			return
		}
	}

	userHeroEntity.SkinID = req.SkinID

	err = service.UserHeroService.UpdateHero(userHeroEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.HeroSkinUseResponse{
		SkinID:           userHeroEntity.SkinID,
		FightingCapacity: userHeroEntity.GetFightingCapacity(),
	}

	service.RenderSuccess(c, rsp)
}
