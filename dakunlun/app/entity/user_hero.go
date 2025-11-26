package entity

import (
	"database/sql/driver"
	"fmt"

	"encoding/json"
)

const (
	SkillOneIndex = iota + 1
	SkillTwoIndex
	SkillThreeIndex
)

type UserHeroEntity struct {
	Model
	Uid              uint32  `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	HeroID           uint32  `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	Type             uint8   `gorm:"type:TINYINT(3) UNSIGNED;not null;default:0;comment:用户ID"`
	Name             string  `gorm:"type:VARCHAR(64);not null;default:0;comment:用户ID"`
	Level            uint16  `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:用户ID"`
	EvolveTimes      uint16  `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:用户ID"`
	FightingCapacity uint64  `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	AttackFreq       uint16  `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:用户ID"`
	HpTrans          uint16  `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:用户ID"`
	AttackTrans      uint16  `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:用户ID"`
	DefendTrans      uint16  `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:用户ID"`
	AttackRatio      uint16  `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:用户ID"`
	DefendRatio      uint16  `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:用户ID"`
	Sex              uint8   `gorm:"type:TINYINT(3) UNSIGNED;not null;default:0;comment:用户ID"`
	Race             uint8   `gorm:"type:TINYINT(3) UNSIGNED;not null;default:0;comment:用户ID"`
	Skills           Skills  `gorm:"type:tinyblob"`
	SkinID           uint32  `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	SkinMap          SkinMap `gorm:"type:tinyblob"`
	ExploreID        uint32  `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
}
type Skills map[uint8]*Skill

const SkilLIDStart = 10000

type Skill struct {
	ID    uint32
	Level uint16
}

func (r *Skills) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Skills value:", value)
	}
	result := Skills{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r Skills) Value() (driver.Value, error) {
	return json.Marshal(r)
}

type SkinMap map[uint32]*Skin

type Skin struct {
	Val              uint8
	FightingCapacity uint64
	Active           bool
}

func (r *SkinMap) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal SkinMap value:", value)
	}
	result := SkinMap{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r SkinMap) Value() (driver.Value, error) {
	return json.Marshal(r)
}

// 设置表名，默认是结构体的名的复数形式
func (*UserHeroEntity) TableName() string {
	return "game_user_hero"
}

func NewUserHero(uid uint32, heroData *HeroData, heroUpgradeData *HeroUpgradeData) *UserHeroEntity {
	skills := make(map[uint8]*Skill, 3)
	skills[SkillOneIndex] = &Skill{
		ID:    heroData.Skill1ID,
		Level: heroUpgradeData.Skill1Level,
	}
	skills[SkillTwoIndex] = &Skill{
		ID:    heroData.Skill2ID,
		Level: heroUpgradeData.Skill2Level,
	}
	skills[SkillThreeIndex] = &Skill{
		ID:    heroData.Skill3ID,
		Level: heroUpgradeData.Skill3Level,
	}
	return &UserHeroEntity{
		Uid:              uid,
		HeroID:           heroData.ID,
		Type:             heroData.Type,
		Name:             heroData.Name,
		Level:            heroUpgradeData.Level,
		EvolveTimes:      InitEvolveTimes,
		FightingCapacity: heroUpgradeData.FightingCapacity,
		AttackFreq:       heroData.AttackFreq,
		HpTrans:          heroData.HpRatio,
		AttackTrans:      heroData.AttackRatio,
		DefendTrans:      heroData.DefendRatio,
		AttackRatio:      0,
		DefendRatio:      0,
		Skills:           skills,
		Sex:              heroData.Sex,
		Race:             heroData.Race,
		ExploreID:        0,
	}
}

func (e *UserHeroEntity) IsPartner() bool {
	return e.Type == HeroTypePartner
}

func (e *UserHeroEntity) IsMainHero() bool {
	return e.Type == HeroTypeMain
}

func (e *UserHeroEntity) GetAttackTrans() uint16 {
	return e.AttackTrans + e.AttackRatio
}

func (e *UserHeroEntity) GetDefendTrans() uint16 {
	return e.DefendTrans + e.DefendRatio
}

func (e *UserHeroEntity) GetHpTrans() uint16 {
	return e.HpTrans
}

func (e *UserHeroEntity) GetFightingCapacity() uint64 {
	v := e.FightingCapacity
	if e.SkinID > 0 {
		v += e.SkinMap[e.SkinID].FightingCapacity
	}
	return v
}
