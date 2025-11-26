package controller

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/service/data"
	"dakunlun/app/util"

	"github.com/gin-gonic/gin"
)

// @Summary 装备列表
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param page query int true "页号"
// @Param perPage query int true "页号"
// @Success 200 {object} msg.EquipListResponse
// @Router /api/equip/list [post]
func EquipList(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	req := &msg.EquipListRequset{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获伙equip列表
	heroEquipEntitys, pageInfo, err := service.HeroEquipService.GetEquipsByPage(uid, req.Page, req.PerPage)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.EquipListResponse{
		PageInfo: pageInfo,
		Equips:   make([]*msg.Equip, 0, len(heroEquipEntitys)),
	}

	for _, heroEquipEntity := range heroEquipEntitys {
		rsp.Equips = append(rsp.Equips, service.PackEquip(heroEquipEntity))
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 装备使用
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "ID"
// @Param pos query int true "位置1-6"
// @Success 200 {object} msg.EquipUseResponse
// @Router /api/equip/use [post]
func EquipUse(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)
	if err != nil {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	req := &msg.EquipUseRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	pos := req.Pos
	// 未指定则取空位
	if pos == 0 {
		//下一个空位
		for p, v := range userEntity.Equips {
			if v.ID == 0 {
				pos = p
				break
			}
		}

		//无空位
		if pos == 0 {
			service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHeroEquipNumFull))
			return
		}
	}

	// 获取新装备
	heroEquipEntity, err := service.HeroEquipService.GetEquipByID(req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 是否是本人装备
	if heroEquipEntity.Uid != userEntity.ID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "错误的用户"))
		return
	}

	for _, v := range userEntity.Equips {
		if v.EquipID == heroEquipEntity.EquipID {
			service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "already have same equip"))
			return
		}
	}

	// 查询该位置有没有装备
	if v, _ := userEntity.Equips[pos]; v.ID > 0 {
		oldHeroEquipEntity, err := service.HeroEquipService.GetEquipByID(v.ID)

		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}

		oldHeroEquipEntity.Unload()
		err = service.HeroEquipService.UpdateEquip(oldHeroEquipEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
		userEntity.Equips[pos] = entity.Equip{}
	}

	// 穿新装备
	heroEquipEntity.Use(pos)
	userEntity.Equips[pos] = entity.Equip{
		heroEquipEntity.ID,
		heroEquipEntity.EquipID,
		heroEquipEntity.SkillID,
		heroEquipEntity.SkillEffect1,
		heroEquipEntity.SkillEffect2,
		heroEquipEntity.SkillEffect3,
	}

	err = service.HeroEquipService.UpdateEquip(heroEquipEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 更新用户属性
	service.UserService.UpdateEquips(userEntity)
	err = service.UserService.UpdateUser(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.EquipUseResponse{}

	service.RenderSuccess(c, rsp)
}

// @Summary 装备卸载
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "ID"
// @Success 200 {object} msg.EquipUnloadResponse
// @Router /api/equip/unload [post]
func EquipUnload(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	req := &msg.EquipUnloadRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获取装备数据
	heroEquipEntity, err := service.HeroEquipService.GetEquipByID(req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 是否是本人装备
	if heroEquipEntity.Uid != userEntity.ID {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "错误的用户ID"))
		return
	}

	pos := heroEquipEntity.Pos
	// 卸载装备
	heroEquipEntity.Unload()
	err = service.HeroEquipService.UpdateEquip(heroEquipEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	userEntity.Equips[pos] = entity.Equip{}
	// 更新用户属性
	service.UserService.UpdateEquips(userEntity)
	err = service.UserService.UpdateUser(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	rsp := &msg.EquipUnloadResponse{}

	service.RenderSuccess(c, rsp)
}

// @Summary 装备图鉴
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.EquipUseResponse
// @Router /api/equip/doc [post]
func EquipDoc(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	//req := &msg.EquipDocListRequset{}
	//
	//if err := c.ShouldBind(req); err != nil {
	//	service.RenderWithAppError(c, err)
	//	return
	//}

	// 获伙equip-doc列表
	heroEquipDocEntitys, err := service.HeroEquipService.GetEquipDocsByUid(uid)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.EquipDocListResponse{}

	for _, heroEquipDocEntity := range heroEquipDocEntitys {
		rsp.EquipDocs = append(rsp.EquipDocs, service.PackEquipDoc(heroEquipDocEntity))
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 图鉴奖励领取
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "自增ID"
// @Success 200 {object} msg.EquipReceiveResponse
// @Router /api/equip/receive [post]
func EquipReceive(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)
	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}
	// 自动验证参数
	req := &msg.EquipReceiveRequest{}
	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	// 查询是否已经领过奖了
	HeroEquipDocEntity, err := service.HeroEquipService.GetEquipDocsByID(req.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	if uid != HeroEquipDocEntity.Uid {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "错误的用户ID"))
		return
	}

	if HeroEquipDocEntity.HasReceived() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "already received"))
		return
	}
	// 获得领奖reward是字符串
	EquipEntity, err := service.RewardService.GetEquipByID(HeroEquipDocEntity.EquipID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	// 获得领奖所需的用户信息
	userEntity, err := service.GetUserEntityInContext(c)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	//开始领奖
	rewards, err := service.RewardService.SendRewards(userEntity, EquipEntity.Rewards, service.SourceEquipDoc, nil)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}
	// 更新状态为已领奖
	HeroEquipDocEntity.MarkReceive()
	err = service.HeroEquipService.UpdateEquipDoc(HeroEquipDocEntity)
	// 返回数据
	rsp := &msg.EquipReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}
	service.RenderSuccess(c, rsp)
}

// @Summary 装备分解
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param ids query []int true "id列表"
// @Success 200 {object} msg.EquipDecomposeResponse
// @Router /api/equip/decompose [post]
func EquipDecompose(c *gin.Context) {
	req := &msg.EquipDecomposeRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	//获取列表
	heroEquipEntitys, err := service.HeroEquipService.GetEquipsByIds(req.IDs)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	equipUpgradeMap, err := data.EquipService.GetUpgradeMap()
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var rewards []constant.IReward
	//检查是否有使用中的
	for _, heroEquipEntity := range heroEquipEntitys {
		if heroEquipEntity.Pos > 0 {
			service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "can not decompose equip in use"))
			return
		}
		if heroEquipEntity.Uid != userEntity.ID {
			service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "错误的用户ID"))
			return
		}
	}

	//发奖
	for _, heroEquipEntity := range heroEquipEntitys {
		rwds, err := service.RewardService.SendRewards(userEntity, equipUpgradeMap[uint32(heroEquipEntity.Level)].
			Rewards, service.SourceDecompose, nil)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
		rewards = append(rewards, rwds...)

		err = service.HeroEquipService.DeleteEquip(heroEquipEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	rsp := &msg.EquipDecomposeResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 装备升级
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "id"
// @Success 200 {object} msg.EquipUpgradeResponse
// @Router /api/equip/upgrade [post]
func EquipUpgrade(c *gin.Context) {
	req := &msg.EquipUpgradeRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//升级
	heroEquipEntity, err := service.HeroEquipService.Upgrade(userEntity, req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 使用中的装备要更新用户信息
	if heroEquipEntity.Pos > 0 {
		service.UserService.UpdateEquips(userEntity)
		err = service.UserService.UpdateUser(userEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	rsp := &msg.EquipUpgradeResponse{
		Equip: service.PackEquip(heroEquipEntity),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 装备锻造
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "id"
// @Success 200 {object} msg.EquipForgeResponse
// @Router /api/equip/forge [post]
func EquipForge(c *gin.Context) {
	req := &msg.EquipForgeRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//锻造
	heroEquipEntity, err := service.HeroEquipService.Forge(userEntity, req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 使用中的装备要更新用户信息
	if heroEquipEntity.Pos > 0 {
		service.UserService.UpdateEquips(userEntity)
		err = service.UserService.UpdateUser(userEntity)
		if err != nil {
			service.RenderWithAppError(c, err)
			return
		}
	}

	rsp := &msg.EquipForgeResponse{
		Equip: service.PackEquip(heroEquipEntity),
	}

	service.RenderSuccess(c, rsp)
}
