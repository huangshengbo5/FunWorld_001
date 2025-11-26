package entity

type AvatarData struct {
	DataModel
	CanUse bool
}

//设置表名，默认是结构体的名的复数形式
func (*AvatarData) TableName() string {
	return "data_avatar"
}
