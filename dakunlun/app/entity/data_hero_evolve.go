package entity

type HeroEvolveData struct {
	DataModel
	HeroID           uint32
	EvolveTimes      uint16 //锻魂次数
	Level            uint16 //所需等级
	CostType         uint16
	CostSubType      uint32
	CostVal          uint64
	FightingCapacity uint64
	AttackRatio      uint16 //攻击转化加成
	DefendRatio      uint16 //防御转化加成
}

//设置表名，默认是结构体的名的复数形式
func (*HeroEvolveData) TableName() string {
	return "data_hero_evolve"
}
