package entity

type HeroEquipDocEntity struct {
	Model
	Uid        uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	EquipID    uint32 `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:英雄ID"`
	HasReceive uint8  `gorm:"type:TINYINT(3) UNSIGNED;not null;default:0;comment:英雄ID"`
}

// 设置表名，默认是结构体的名的复数形式
func (*HeroEquipDocEntity) TableName() string {
	return "game_hero_equip_doc"
}

func NewHeroEquipDoc(uid, equipID uint32) *HeroEquipDocEntity {
	return &HeroEquipDocEntity{
		Uid:        uid,
		EquipID:    equipID,
		HasReceive: IntFalse,
	}
}

func (e *HeroEquipDocEntity) HasReceived() bool {
	return e.HasReceive == IntTrue
}

func (e *HeroEquipDocEntity) MarkReceive() {
	e.HasReceive = IntTrue
}
