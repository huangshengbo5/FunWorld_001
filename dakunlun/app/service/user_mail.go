package service

import (
	"dakunlun/app/constant"
	"dakunlun/app/dao"
	"dakunlun/app/entity"
	"dakunlun/app/util"

	"go.uber.org/zap"
)

type userMailService struct {
}

var UserMailService = new(userMailService)

// 添加邮件
func (srv *userMailService) AddMail(uid uint32, mailID uint32, params entity.Params,
	rewards entity.RewardStrings) (mailEntity *entity.UserMailEntity, err error) {
	mailEntity, err = dao.UserMailDao.Create(uid, mailID, params, rewards)
	return
}

// 获取邮件
func (srv *userMailService) GetMailByID(id uint32) (userMailEntity *entity.UserMailEntity, err error) {
	userMailEntity, err = dao.UserMailDao.FetchByID(id)
	return
}

// 获取邮件列表
func (srv *userMailService) GetMailsByUid(uid uint32, page int, perPage int) (userMailEntitys []*entity.
	UserMailEntity, pageInfo *constant.Paging, err error) {
	var total int64
	total, err = dao.UserMailDao.CountByUid(uid)

	if err != nil {
		return
	}

	pageInfo = &constant.Paging{
		Page:     page,
		PerPage:  perPage,
		TotalNum: int(total),
	}

	if total > 0 {
		userMailEntitys, err = dao.UserMailDao.FetchMultiByUid(uid, (page-1)*perPage, perPage)
	}

	return
}

// 获取未领取的邮件列表
func (srv *userMailService) GetUnReceivedMails(uid uint32) (userMailEntitys []*entity.UserMailEntity, err error) {
	userMailEntitys, err = dao.UserMailDao.FetchMultiByUidAndUnReceived(uid)
	return
}

// 删除所有邮件
func (srv *userMailService) RemoveAllMails(uid uint32) (err error) {
	err = dao.UserMailDao.DeleteMultiByUid(uid)
	return
}

// 更新邮件
func (srv *userMailService) UpdateMail(userMailEntity *entity.UserMailEntity) (err error) {
	err = dao.UserMailDao.Update(userMailEntity)
	return
}

func (srv *userMailService) UpdateMultiMails(userMailEntitys []*entity.UserMailEntity) (err error) {
	for _, userMailEntity := range userMailEntitys {
		err = dao.UserMailDao.Update(userMailEntity)
		if err != nil {
			util.GetLogger().Error("UpdateMultiMails", zap.Error(err))
		}
	}

	return
}
