package entity

type HeroUpgradeData struct {
	DataModel
	HeroID           uint32
	Level            uint16
	FightingCapacity uint64
	CostType         uint16
	CostSubType      uint32
	CostVal          uint32
	Skill1Level      uint16
	Skill2Level      uint16
	Skill3Level      uint16
}

//设置表名，默认是结构体的名的复数形式
func (*HeroUpgradeData) TableName() string {
	return "data_hero_upgrade"
}
