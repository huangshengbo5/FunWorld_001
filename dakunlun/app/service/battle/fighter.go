package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
)

type BaseFighter struct {
	ctx              *constant.BattleContext
	id               uint32 //玩家id
	avatar           uint16 //玩家头像
	name             string //玩家名称
	heroID           uint32 //英雄ID
	fType            uint8
	fightingCapacity uint64
	attackFreq       int
	sex              uint8
	race             uint8
	attackPlus       int
	defendPlus       int
	level            uint16
	skills           entity.Skills `gorm:"type:tinyblob"`
}

func (b *BaseFighter) GetID() uint32 {
	return b.id
}

func (b *BaseFighter) GetName() string {
	return b.name
}

func (b *BaseFighter) GetType() uint8 {
	return b.fType
}

func (b *BaseFighter) GetFightingCapacity() uint64 {
	return b.fightingCapacity
}

func (b *BaseFighter) GetAttackFreq() int {
	return b.attackFreq
}
func (b *BaseFighter) GetSex() uint8 {
	return b.sex
}
func (b *BaseFighter) GetRace() uint8 {
	return b.race
}

func (b *BaseFighter) GetAttackPlus() int {
	return b.attackPlus
}

func (b *BaseFighter) GetDefendPlus() int {
	return b.defendPlus
}

func (b *BaseFighter) GetAvatar() uint16 {
	return b.avatar
}

func (b *BaseFighter) GetFType() uint8 {
	return b.fType
}
func (b *BaseFighter) GetHeroID() uint32 {
	return b.heroID
}

func (b *BaseFighter) GetLevel() uint16 {
	return b.level
}

func (b *BaseFighter) GetSkills() entity.Skills {
	return b.skills
}
func NewBaseFighter(ctx *constant.BattleContext, id uint32, avatar uint16, name string, heroID uint32, fType uint8,
	fightingCapacity uint64, attackFreq int, sex uint8, race uint8, attackPlus int, defendPlus int, level uint16,
	skills entity.Skills) *BaseFighter {
	return &BaseFighter{
		ctx:              ctx,
		id:               id,
		avatar:           avatar,
		name:             name,
		heroID:           heroID,
		fType:            fType,
		fightingCapacity: fightingCapacity,
		attackFreq:       attackFreq,
		sex:              sex,
		race:             race,
		attackPlus:       attackPlus,
		defendPlus:       defendPlus,
		level:            level,
		skills:           skills,
	}
}

func NewTestFighter(ctx *constant.BattleContext, fType uint8, id uint32, name string,
	fightingCapacity uint64, attackFreq int, sex uint8, race uint8) *BaseFighter {
	return &BaseFighter{
		ctx:              ctx,
		fType:            constant.FTypeTester,
		id:               id,
		name:             name,
		fightingCapacity: fightingCapacity,
		attackFreq:       attackFreq,
		sex:              sex,
		race:             race,
	}
}
