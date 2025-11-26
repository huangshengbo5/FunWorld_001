package msg

import "dakunlun/app/service/battle"

// 挑战关卡
type TestPveRequest struct {
	Uid        uint32 `json:"uid"`
	CampaignID uint32 `json:"campaignID"`
}

type TestPveResponse struct {
	Win    bool                 `json:"win"`
	Report *battle.BattleReport `json:"report"`
	Debug  string               `json:"debug"`
}

// 挑战关卡
type TestRewardRequest struct {
	Name string `json:"name"`
}

// 挑战关卡
type TestSendRewardRequest struct {
	Name    string `json:"name"`
	Rewards string `json:"rewards"`
}

// 挑战关卡
type SetResourceReq struct {
	Gold string `json:"gold"`
	Gem  string `json:"gem"`
}

// 挑战关卡
type SetResourceRsp struct {
	Gold string `json:"gold"`
	Gem  string `json:"gem"`
}
