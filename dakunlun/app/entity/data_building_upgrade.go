package entity

type BuildingUpgradeData struct {
	DataModel
	BuildingID    uint32
	Level         uint16
	ConditionType uint8
	ConditionVal  uint32
	CostType      uint16
	CostSubType   uint32
	CostVal       uint64
	EffectID      uint32
	EffectVal     uint32
}

//设置表名，默认是结构体的名的复数形式
func (*BuildingUpgradeData) TableName() string {
	return "data_building_upgrade"
}
