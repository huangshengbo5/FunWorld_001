package msg

import "dakunlun/app/entity"

type HeroListRequest struct {
}

type HeroListResponse struct {
	Heros []*Hero `json:"heros"`
}

type Hero struct {
	ID               uint32         `json:"id"`
	HeroID           uint32         `json:"heroID"`
	Type             uint8          `json:"type"`
	Name             string         `json:"name"`
	Level            uint16         `json:"level"`
	FightingCapacity uint64         `json:"fightingCapacity"`
	AttackFreq       uint16         `json:"attackFreq"`
	AttackTrans      uint16         `json:"attackTrans"`
	DefendTrans      uint16         `json:"defendTrans"`
	EvolveTimes      uint16         `json:"evolveTimes"`
	Sex              uint8          `json:"sex"`
	Race             uint8          `json:"race"`
	Skills           entity.Skills  `json:"skills"`
	SkinID           uint32         `json:"skinID"`
	Skins            entity.SkinMap `json:"skins"`
	ExploreID        uint32         `json:"exploreID"`
}

type HeroUpgradeRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type HeroUpgradeResponse struct {
	Level uint16 `json:"level"`
}

type HeroEvolveRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type HeroEvolveResponse struct {
	EvolveTimes uint16 `json:"evolveTimes"`
}

type HeroJoinRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type HeroJoinResponse struct {
}

type HeroUnloadResponse struct {
}

type HeroSkinGetRequest struct {
	ID     uint32 `json:"id" binding:"required,gt=0"`
	SkinID uint32 `json:"skinID" binding:"required,gt=0"`
}

type HeroSkinGetResponse struct {
	Skin *entity.Skin `json:"skin"`
}

type HeroSkinUseRequest struct {
	ID     uint32 `json:"id" binding:"required,gt=0"`
	SkinID uint32 `json:"skinID" binding:"required"`
}

type HeroSkinUseResponse struct {
	SkinID           uint32 `json:"skinID"`
	FightingCapacity uint64 `json:"fightingCapacity"`
}
