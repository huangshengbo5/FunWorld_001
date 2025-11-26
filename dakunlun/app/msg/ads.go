package msg

type AdsStartRequest struct {
}

type AdsStartResponse struct {
	AdsID      string `json:"adsID"`      //广告ID
	ExpireTime int64  `json:"expireTime"` //过期时间
}

type AdsReceiveRequest struct {
	ID uint32 `json:"id"` // ID
}

type AdsReceiveResponse struct {
	Rewards []*Reward `json:"rewards"`
}

type AddBuffRequest struct {
	AdsID string `json:"adsID"`
}

type AddBuffResponse struct {
	Gold          uint64 `json:"gold"`          //金币
	GoldIncrement uint64 `json:"goldIncrement"` //金币每秒增速
	GoldFlushIn   int64  `json:"goldFlushIn"`   //上次金币结算时间
	GoldBuffEndIn int64  `json:"goldBuffEndIn"` //双倍金币到期时间
}
