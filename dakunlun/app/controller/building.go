package controller

import (
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/util"

	"github.com/gin-gonic/gin"
)

// @Summary 建筑列表
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.BuildingListResponse
// @Router /api/building/list [post]
func BuildingList(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	// 获取建筑列表
	userBuildingEntitys, err := service.UserBuildingService.GetBuildingsByUid(uid)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.BuildingListResponse{}

	for _, userBuildingEntity := range userBuildingEntitys {
		rsp.Buildings = append(rsp.Buildings, service.PackBuilding(userBuildingEntity))
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 建筑升级
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "建筑自增ID"
// @Success 200 {object} msg.BuildingUpgradeResponse
// @Router /api/building/upgrade [post]
func BuildingUpgrade(c *gin.Context) {
	req := &msg.BuildingUpgradeRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userBuildingEntity, err := service.UserBuildingService.Upgrade(userEntity, req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.BuildingUpgradeResponse{
		userBuildingEntity.Level,
	}

	service.RenderSuccess(c, rsp)
}
