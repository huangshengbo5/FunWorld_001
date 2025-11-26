package entity

type CrystalUpgradeData struct {
	DataModel
	CrystalID   uint32
	Level       uint16
	EffectID    uint16
	EffectVal   uint32
	CostType    uint16
	CostSubType uint32
	CostVal     uint64
}

//设置表名，默认是结构体的名的复数形式
func (*CrystalUpgradeData) TableName() string {
	return "data_crystal_upgrade"
}
