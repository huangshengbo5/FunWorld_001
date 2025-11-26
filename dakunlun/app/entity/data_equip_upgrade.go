package entity

type EquipUpgradeData struct {
	DataModel
	CostType    uint16
	CostSubType uint32
	CostVal     uint64
	Rewards     RewardStrings `gorm:"type:Varchar"`
	Multiplier  uint32
}

//设置表名，默认是结构体的名的复数形式
func (*EquipUpgradeData) TableName() string {
	return "data_equip_upgrade"
}
