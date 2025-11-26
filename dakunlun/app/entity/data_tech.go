package entity

type TechData struct {
	DataModel
}

//设置表名，默认是结构体的名的复数形式
func (*TechData) TableName() string {
	return "data_tech"
}
