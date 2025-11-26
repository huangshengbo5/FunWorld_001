package entity

type AccountData struct {
	DataModel
	Name       string
	Password   string
	Type       int
	Expire     bool
	ExpireDate string
	StartGold  uint64
}

// 设置表名，默认是结构体的名的复数形式
func (*AccountData) TableName() string {
	return "data_account"
}
