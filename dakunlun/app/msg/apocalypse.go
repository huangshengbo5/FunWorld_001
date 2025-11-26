package msg

import (
	"dakunlun/app/entity"
	"dakunlun/app/service/battle"
)

type ApocalypseShowRequest struct {
}

type ApocalypseShowResponse struct {
	ApocalypseID uint32        `json:"apocalypseID"` //boss表ID
	RemainNum    uint8         `json:"remainNum"`    //剩余次数
	Status       entity.Status `json:"status"`       //状态 0创建 1失败 2成功
}

type ApocalypseAttackRequest struct {
	ApocalypseID uint32 `json:"apocalypseID" binding:"required,gt=0"`
	Type         uint8  `json:"type"` //0 普通 1广告
	AdsID        string `json:"adsID"`
}

type Player struct {
	ID     uint32 `json:"id"`
	Avatar uint16 `json:"avatar"`
	Name   string `json:"name"`
	Level  uint16 `json:"level"`
}
type ApocalypseAttackResponse struct {
	Win     bool                   `json:"win"`
	Rewards []*Reward              `json:"rewards"`
	Players []*Player              `json:"players"` //队员列表
	Reports []*battle.BattleReport `json:"report"`  //战报列表
}

type ApocalypseReceiveRequest struct {
	ApocalypseID uint32 `json:"apocalypseID" binding:"required,gt=0"`
	Type         uint8  `json:"type"` //0 普通 1广告
	AdsID        string `json:"adsID"`
}

type ApocalypseReceiveResponse struct {
	Rewards []*Reward `json:"rewards"`
}
