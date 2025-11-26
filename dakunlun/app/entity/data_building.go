package entity

const (
	//可升级
	BTypeUpgrade = 1
	//不可升级
	BTypeNormal = 2
)

const (
	//议事厅
	BIDAssemblyHall = 1001
	BIDMill         = 1002
	BIDTavern       = 1003
	BIDMine         = 1004
	BIDMetallurgy   = 1005
	BIDInstitute    = 1006
	BIDTower        = 1011
)

type BuildingData struct {
	DataModel
	Type          uint8
	Name          string
	ConditionType uint8
	ConditionVal  uint32
}

//设置表名，默认是结构体的名的复数形式
func (*BuildingData) TableName() string {
	return "data_building"
}
