package msg

import (
	"dakunlun/app/constant"
)

type EquipListRequset struct {
	Page    int `json:"page" binding:"required,gt=0"`
	PerPage int `json:"perPage" binding:"required,gt=0"`
}

type EquipListResponse struct {
	PageInfo *constant.Paging `json:"pageInfo"`
	Equips   []*Equip         `json:"equips"`
}

type Equip struct {
	ID               uint32 `json:"id"`
	EquipID          uint32 `json:"equipID"`
	Level            uint16 `json:"level"`
	ForgeID          uint16 `json:"forgeID"`
	Pos              uint8  `json:"pos"`
	FightingCapacity uint64 `json:"fightingCapacity"`
	SkillID          uint32 `json:"skillID"`
	SkillEffect1     int    `json:"skillEffect1"`
	SkillEffect2     int    `json:"skillEffect2"`
	SkillEffect3     int    `json:"skillEffect3"`
	EffectID1        uint32 `json:"effectID1"`
	EffectVal1       uint32 `json:"effectVal1"`
	EffectID2        uint32 `json:"effectID2"`
	EffectVal2       uint32 `json:"effectVal2"`
	EffectID3        uint32 `json:"effectID3"`
	EffectVal3       uint32 `json:"effectVal3"`
}

type EquipUseRequest struct {
	ID  uint32 `json:"id"`
	Pos uint8  `json:"pos" binding:"gte=0,lte=6"` //0代表不指定位置随意找个空位
}

type EquipUseResponse struct {
}

type EquipUnloadRequest struct {
	ID uint32 `json:"id"`
}

type EquipUnloadResponse struct {
}

type EquipReceiveRequest struct {
	ID uint32 `json:"id"`
}

type EquipReceiveResponse struct {
	Rewards []*Reward
}

type EquipDecomposeRequest struct {
	IDs []uint32 `json:"ids"`
}

type EquipDecomposeResponse struct {
	Rewards []*Reward
}

type EquipDocListRequset struct {
}

type EquipDocListResponse struct {
	EquipDocs []*EquipDoc `json:"equipDocs"`
}
type EquipDoc struct {
	ID         uint32 `json:"id"`
	EquipID    uint32 `json:"equipID"`
	HasReceive bool   `json:"hasReceive"`
}

type EquipUpgradeRequest struct {
	ID uint32 `json:"id"`
}

type EquipUpgradeResponse struct {
	Equip *Equip `json:"equip"`
}

type EquipForgeRequest struct {
	ID uint32 `json:"id"`
}

type EquipForgeResponse struct {
	Equip *Equip `json:"equip"`
}
