package entity

type CastData struct {
	DataModel
	Grade            uint16
	Level            uint16
	CostType         uint16
	CostSubType      uint32
	CostVal          uint64
	FightingCapacity uint64
}

//设置表名，默认是结构体的名的复数形式
func (*CastData) TableName() string {
	return "data_cast"
}
