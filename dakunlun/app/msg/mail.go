package msg

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
)

type MailListRequset struct {
	Page    int `json:"page" binding:"required,gt=0"`
	PerPage int `json:"perPage" binding:"required,gt=0"`
}

type MailListResponse struct {
	PageInfo *constant.Paging `json:"pageInfo"`
	Mails    []*Mail          `json:"mails"`
}

type Mail struct {
	ID          uint32               `json:"id"`          //数据库ID
	MailID      uint32               `json:"mailID"`      //邮件模板ID
	Status      uint8                `json:"status"`      //状态 0未读 1已读 2已删除
	Args        entity.Params        `json:"args"`        //占位符参数
	HasReceived uint8                `json:"hasReceived"` //是否领取附件
	Attachment  entity.RewardStrings `json:"attachment"`  //附件（通用奖励格式）
	CreateTime  int64                `json:"createTime"`  //创建时间
}

type MailReadRequset struct {
	ID uint32 `json:"id"`
}

type MailReadResponse struct {
}

type MailReceiveRequset struct {
	ID uint32 `json:"id"`
}

type MailReceiveResponse struct {
	Rewards []*Reward `json:"rewards"`
}

type MailReceiveAllRequset struct {
}

type MailReceiveAllResponse struct {
	Rewards []*Reward `json:"rewards"`
}

type MailRemoveAllRequset struct {
}

type MailRemoveAllResponse struct {
}
