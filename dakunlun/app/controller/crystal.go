package controller

import (
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/util"

	"github.com/gin-gonic/gin"
)

// @Summary 水晶列表
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.CrystalListResponse
// @Router /api/crystal/list [post]
func CrystalList(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	// 获取水晶列表
	userCrystalEntitys, err := service.UserCrystalService.GetCrystalsByUid(uid)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.CrystalListResponse{}

	for _, userCrystalEntity := range userCrystalEntitys {
		rsp.Crystals = append(rsp.Crystals, service.PackCrystal(userCrystalEntity))
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 水晶升级
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "水晶自增ID"
// @Success 200 {object} msg.CrystalUpgradeResponse
// @Router /api/crystal/upgrade [post]
func CrystalUpgrade(c *gin.Context) {
	req := &msg.CrystalUpgradeRequest{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userCrystalEntity, err := service.UserCrystalService.Upgrade(userEntity, req.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.CrystalUpgradeResponse{
		userCrystalEntity.Level,
	}

	service.RenderSuccess(c, rsp)
}
