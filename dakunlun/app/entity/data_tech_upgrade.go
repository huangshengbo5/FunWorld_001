package entity

type TechUpgradeData struct {
	DataModel
	TechID      uint32
	Level       uint16
	TargetType  uint8
	TargetValue uint32
	EffectID    uint16
	EffectValue uint32
	CostType    uint16
	CostSubType uint32
	CostVal     uint64
}

//设置表名，默认是结构体的名的复数形式
func (*TechUpgradeData) TableName() string {
	return "data_tech_upgrade"
}
