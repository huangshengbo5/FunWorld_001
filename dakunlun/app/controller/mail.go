package controller

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/util"

	"go.uber.org/zap"
	"golang.org/x/sync/errgroup"

	"github.com/gin-gonic/gin"
)

// @Summary 邮件列表
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param page query int true "页号"
// @Param perPage query int true "页号"
// @Success 200 {object} msg.MailListResponse
// @Router /api/mail/list [post]
func MailList(c *gin.Context) {
	uid, ok := service.GetUidInContext(c)

	if !ok {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "需要登录"))
		return
	}

	req := &msg.MailListRequset{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获伙mail列表
	userMailEntitys, pageInfo, err := service.UserMailService.GetMailsByUid(uid, req.Page, req.PerPage)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.MailListResponse{
		PageInfo: pageInfo,
		Mails:    make([]*msg.Mail, 0, len(userMailEntitys)),
	}

	for _, userMailEntity := range userMailEntitys {
		rsp.Mails = append(rsp.Mails, service.PackMail(userMailEntity))
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 标记已读
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "唯一ID"
// @Success 200 {object} msg.MailReadResponse
// @Router /api/mail/read [post]
func MailRead(c *gin.Context) {
	req := &msg.MailReadRequset{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获取mail
	userMailEntity, err := service.UserMailService.GetMailByID(req.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 验证邮件
	if userEntity.ID != userMailEntity.Uid {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "not your mail"))
		return
	}

	//验证状态
	if !userMailEntity.IsUnRead() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "not unread mail"))
		return
	}

	//标记已读
	userMailEntity.MarkRead()
	err = service.UserMailService.UpdateMail(userMailEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.MailReadResponse{}

	service.RenderSuccess(c, rsp)
}

// @Summary 领取附件
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param id query int true "唯一ID"
// @Success 200 {object} msg.MailReceiveResponse
// @Router /api/mail/receive [post]
func MailReceive(c *gin.Context) {
	req := &msg.MailReceiveRequset{}

	if err := c.ShouldBind(req); err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获取mail
	userMailEntity, err := service.UserMailService.GetMailByID(req.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 验证邮件
	if userEntity.ID != userMailEntity.Uid {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "not your mail"))
		return
	}

	// 是否有附件
	if len(userMailEntity.Attachment) == 0 {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "no attachment"))
		return
	}

	// 验证状态
	if userMailEntity.IsReceived() {
		service.RenderWithAppError(c, util.NewAppError(util.ErrorCodeHack, "has received"))
		return
	}

	rewards, err := service.RewardService.SendRewards(userEntity, userMailEntity.Attachment, service.SourceMail, nil)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 标记领取
	userMailEntity.MarkReceived()
	err = service.UserMailService.UpdateMail(userMailEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.MailReceiveResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 邮件批量领取
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.MailReceiveAllResponse
// @Router /api/mail/receiveall [post]
func MailReceiveAll(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 获取mail
	userMailEntitys, err := service.UserMailService.GetUnReceivedMails(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var rewardSlice entity.RewardStrings
	var needChange []*entity.UserMailEntity
	for _, userMailEntity := range userMailEntitys {
		// 是否有附件
		if len(userMailEntity.Attachment) > 0 {
			userMailEntity.MarkRead()
			userMailEntity.MarkReceived()
			rewardSlice = append(rewardSlice, userMailEntity.Attachment...)
			needChange = append(needChange, userMailEntity)
		}
	}

	var rewards []constant.IReward
	//批量发奖并改数据
	if len(rewardSlice) > 0 {
		//修改邮件状态
		var g errgroup.Group
		g.Go(func() error {
			defer func() {
				if x := recover(); x != nil {
					util.GetLogger().Error("mail.UpdateMultiMails", zap.Error(err))
				}
			}()
			err = service.UserMailService.UpdateMultiMails(needChange)
			return err
		})

		//发奖
		g.Go(func() error {
			defer func() {
				if x := recover(); x != nil {
					util.GetLogger().Error("mail.SendRewards", zap.Error(err))
				}
			}()
			rewards, err = service.RewardService.SendRewards(userEntity, rewardSlice, service.SourceMail, nil)
			return err
		})

		if err = g.Wait(); err != nil {
			util.GetLogger().Error("mail.Wait", zap.Error(err))
			service.RenderWithAppError(c, err)
			return
		}
	}

	rsp := &msg.MailReceiveAllResponse{
		Rewards: service.PackRewards(rewards),
	}

	service.RenderSuccess(c, rsp)
}

// @Summary 邮件批量删除
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Success 200 {object} msg.MailRemoveAllResponse
// @Router /api/mail/removeall [post]
func MailRemoveAll(c *gin.Context) {
	userEntity, err := service.GetUserEntityInContext(c)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	// 删除全部邮件
	err = service.UserMailService.RemoveAllMails(userEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	rsp := &msg.MailRemoveAllResponse{}

	service.RenderSuccess(c, rsp)
}
