package entity

const (
	InitEquipLevel = 1
	AttrNum        = 3
)

const (
	EquipQualityBlue = iota + 1
	EquipQualityPurple
	EquipQualityOrange
	EquipQualityRed
)

type EquipData struct {
	DataModel
	Icon    string
	SkillID uint32
	Name    string
	Rewards RewardStrings `gorm:"type:Varchar"`
	Weight  int
}

//设置表名，默认是结构体的名的复数形式
func (*EquipData) TableName() string {
	return "data_equip"
}
