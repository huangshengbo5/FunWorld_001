package entity

type StoreData struct {
	DataModel
	Name    string
	Info    string
	Icon    string
	Type    uint8
	Price   uint64
	Rewards RewardStrings `gorm:"type:Varchar"`
}

// 设置表名，默认是结构体的名的复数形式
func (*StoreData) TableName() string {
	return "data_store"
}
