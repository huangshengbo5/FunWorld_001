package entity

type EquipForgeData struct {
	DataModel
	ForgeGrade       uint16
	ForgeLevel       uint16
	CostType         uint16
	CostSubType      uint32
	CostVal          uint64
	FightingCapacity uint64
}

//设置表名，默认是结构体的名的复数形式
func (*EquipForgeData) TableName() string {
	return "data_equip_forge"
}
