package msg

import (
	"dakunlun/app/constant"
	"dakunlun/app/service/battle"
)

type ArenaSignUpRequest struct {
}

type ArenaSignUpResponse struct {
	ArenaIsSign bool `json:"arenaIsSign"`
}

type ArenaListRequest struct {
	Page    int64 `json:"page" binding:"required,gt=0"`
	PerPage int64 `json:"perPage" binding:"required,gt=0"`
}

type Ranker struct {
	ID               uint32 `json:"id"`               //rank表主键
	Uid              uint32 `json:"uid"`              //用户id
	IsPlayer         bool   `json:"isPlayer"`         //是否是玩家
	Name             string `json:"name"`             //用户名
	Avatar           uint16 `json:"avatar"`           //头像
	Level            uint16 `json:"level"`            //等级
	FightingCapacity uint64 `json:"fightingCapacity"` //战力
	Rank             uint16 `json:"rank"`             //名次
}

type ArenaListResponse struct {
	PageInfo *constant.Paging `json:"pageInfo"` //分页信息
	Rankers  []*Ranker        `json:"rankers"`  //排行榜列表
	Self     *Ranker          `json:"self"`     //本人排名信息
	Records  []*ArenaRecord   `json:"records"`  //挑战记录
}

type ArenaAttackRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type ArenaAttackResponse struct {
	Win     bool                 `json:"win"`     //是否胜利
	Rewards []*Reward            `json:"rewards"` //奖励
	Report  *battle.BattleReport `json:"report"`  //战报
}

type ArenaRecord struct {
	Uid     uint32 `json:"uid"`     //用户ID
	Name    string `json:"name"`    //用户名
	Win     bool   `json:"win"`     //是否战胜本人
	OldRank uint16 `json:"oldRank"` //旧名次
	NewRank uint16 `json:"newRank"` //新名次
	Time    int64  `json:"time"`    //时间戳
}
