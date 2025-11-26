package msg

import "dakunlun/app/service/battle"

// 挑战关卡
type CampaignAttackRequest struct {
	CampaignID uint32 `json:"campaignID"`
}

type CampaignAttackResponse struct {
	Win       bool                 `json:"win"`
	Rewards   []*Reward            `json:"rewards"`
	Report    *battle.BattleReport `json:"report"`
	HeroID    uint32               `json:"heroID"`
	HeroLevel uint16               `json:"heroLevel"`
}

type CampaignReceiveRequest struct {
	Type  uint8  `json:"type"` //0 普通 1广告
	AdsID string `json:"adsID"`
}

type CampaignReceiveResponse struct {
	Rewards []*Reward `json:"rewards"`
}
