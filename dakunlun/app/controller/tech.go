package controller

import (
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/util"

	"github.com/gin-gonic/gin"
)

// @Summary 科技列表
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.TechListResponse
// @Router /api/tech/list [post]
func TechList(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	// 获取科技列表
	userTechEntitys, err := service.UserTechService.GetTechsByUid(uid)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.TechListResponse{}

	for _, userTechEntity := range userTechEntitys {
		rsp.Techs = append(rsp.Techs, service.PackTech(userTechEntity))
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 科技升级
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "科技自增ID"
// @Success 200 {object} msg.TechUpgradeResponse
// @Router /api/tech/upgrade [post]
func TechUpgrade(c *gin.Context) {
	req := &msg.TechUpgradeRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userTechEntity, err := service.UserTechService.Upgrade(userEntity, req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.TechUpgradeResponse{
		userTechEntity.Level,
	}

	service.RenderSuccess(c, rsp)
}
