package entity

type MinorslimitData struct {
	DataModel
	StartTime string
	EndTime   string
}

// 设置表名，默认是结构体的名的复数形式
func (*MinorslimitData) TableName() string {
	return "data_minorslimit"
}
