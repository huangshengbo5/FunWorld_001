package msg

import "dakunlun/app/entity"

type ExploreListRequest struct {
}

type ExploreListResponse struct {
	Heros    []*Hero    `json:"heros"`
	Explores []*Explore `json:"explores"`
}

type Explore struct {
	ID        uint32             `json:"id"`        //自增ID
	ExploreID uint32             `json:"exploreID"` //配置表ID
	StartTime int64              `json:"startTime"` //开始时间
	HeroIDs   entity.Uint32Slice `json:"heroIDs"`   //武将自增ID列表
	Mul       float64            `json:"mul"`       //倍数 1代表100%
}

type ExploreReceiveRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type ExploreReceiveResponse struct {
	Rewards []*Reward `json:"rewards"`
}

type ExploreSetRequest struct {
	ID      uint32   `json:"id" binding:"required,gt=0"`
	HeroIDs []uint32 `json:"heroIDs"`
}

type ExploreSetResponse struct {
	Rewards []*Reward `json:"rewards"`
	Explore *Explore  `json:"explore"`
}
