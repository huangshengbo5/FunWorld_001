package controller

import (
	"dakunlun/app/constant"
	"dakunlun/app/msg"
	"dakunlun/app/service"

	"github.com/gin-gonic/gin"
)

// @Summary 增加金币
// @Accept  json
// @Produce  json
// @Param gold query int true "金币数量"
// @Success 200 {object} msg.GmCommonResponse
// @Router /api/gm/incrgold [post]
func GmIncrGold(c *gin.Context) {
	req := &msg.GmIncrGoldRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	err = service.UserService.IncrAssets(userEntity, constant.CostTypeGold, 0, req.Gold)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	err = service.UserService.UpdateUser(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.GmCommonResponse{}

	service.RenderSuccess(c, rsp)
}

// @Summary 增加装备
// @Accept  json
// @Produce  json
// @Param equipID query int true "装备ID"
// @Success 200 {object} msg.GmCommonResponse
// @Router /api/gm/addequip [post]
func GmAddEquip(c *gin.Context) {
	req := &msg.GmAddEquipRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	_, err = service.HeroEquipService.AddEquip(userEntity, req.EquipID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.GmCommonResponse{}

	service.RenderSuccess(c, rsp)
}
