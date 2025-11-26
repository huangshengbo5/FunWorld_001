package entity

import "dakunlun/app/util"

const (
	PosEmpty uint8 = iota
	PosOne
	PosTwo
	PosThree
	PosFour
	PosFive
	PosSix
)

const (
	EquipEffectOne = iota
	EquipEffectTwo
	EquipEffectThree
)

type HeroEquipEntity struct {
	Model
	Uid                  uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	EquipID              uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	Name                 string `gorm:"type:VARCHAR(32);not null;default:'';comment:用户名"`
	Level                uint16 `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:英雄ID"`
	ForgeID              uint16 `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:英雄ID"`
	Pos                  uint8  `gorm:"type:TINYINT(3) UNSIGNED;not null;default:0;comment:英雄ID"`
	FightingCapacityBase uint64 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	FightingCapacityPlus uint64 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	SkillID              uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	SkillEffect1         int    `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	SkillEffect2         int    `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	SkillEffect3         int    `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	EffectID1            uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	EffectVal1           uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	EffectID2            uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	EffectVal2           uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	EffectID3            uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	EffectVal3           uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
}

// 设置表名，默认是结构体的名的复数形式
func (*HeroEquipEntity) TableName() string {
	return "game_hero_equip"
}

func NewHeroEquip(uid uint32, equipData *EquipData, equipUpgradeData *EquipUpgradeData,
	attrDatas []*EquipAttrData, equipSkillData *EquipSkillData) *HeroEquipEntity {
	r := &HeroEquipEntity{
		Uid:          uid,
		EquipID:      equipData.ID,
		Name:         equipData.Name,
		Level:        uint16(equipUpgradeData.ID),
		ForgeID:      0,
		Pos:          PosEmpty,
		SkillID:      equipData.SkillID,
		SkillEffect1: util.RandInt(equipSkillData.Effect1Lower, equipSkillData.Effect1Upper),
		SkillEffect2: util.RandInt(equipSkillData.Effect2Lower, equipSkillData.Effect2Upper),
		SkillEffect3: util.RandInt(equipSkillData.Effect3Lower, equipSkillData.Effect3Upper),
	}

	//随机二级属性
	for i := 0; i < AttrNum; i++ {
		switch i {
		case EquipEffectOne:
			r.EffectID1 = attrDatas[i].EffectID
			r.EffectVal1 = attrDatas[i].RandomVal()
		case EquipEffectTwo:
			r.EffectID2 = attrDatas[i].EffectID
			r.EffectVal2 = attrDatas[i].RandomVal()
		case EquipEffectThree:
			r.EffectID3 = attrDatas[i].EffectID
			r.EffectVal3 = attrDatas[i].RandomVal()
		}
	}

	//基础战力
	r.FightingCapacityBase = uint64(r.EffectVal1+r.EffectVal2+r.EffectVal3) / 2

	return r
}

// 战斗力值
func (e *HeroEquipEntity) FightingCapacity() uint64 {
	return e.FightingCapacityBase + e.FightingCapacityPlus
}

// 脱装备
func (e *HeroEquipEntity) Unload() {
	e.Pos = 0
}

// 穿装备
func (e *HeroEquipEntity) Use(pos uint8) {
	e.Pos = pos
}
