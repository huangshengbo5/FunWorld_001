package entity

type BuildingEffectData struct {
	DataModel
	Type uint16
}

//设置表名，默认是结构体的名的复数形式
func (*BuildingEffectData) TableName() string {
	return "data_building_effect"
}
