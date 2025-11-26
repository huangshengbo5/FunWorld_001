package entity

type AlchemyData struct {
	DataModel
	AttrMin     uint32
	AttrMax     uint32
	CdTimes     int64
	CostType    uint16
	CostSubType uint32
	CostVal     uint64
	Rewards     RewardStrings `gorm:"type:Varchar"`
}

//设置表名，默认是结构体的名的复数形式
func (*AlchemyData) TableName() string {
	return "data_alchemy"
}
