package entity

type HeroSkinData struct {
	DataModel
	HeroID           uint32
	GetType          uint8
	GetValue         uint8
	FightingCapacity uint64
}

//设置表名，默认是结构体的名的复数形式
func (*HeroSkinData) TableName() string {
	return "data_hero_skin"
}
