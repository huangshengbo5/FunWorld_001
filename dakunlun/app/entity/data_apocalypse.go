package entity

type ApocalypseData struct {
	DataModel
	FightingCapacity uint64
	NpcID            uint32
	Rewards          RewardStrings `gorm:"type:Varchar"`
	Limited          uint8
}

//设置表名，默认是结构体的名的复数形式
func (*ApocalypseData) TableName() string {
	return "data_apocalypse"
}
