package entity

type EquipJackpotData struct {
	DataModel
	Weight int
}

//设置表名，默认是结构体的名的复数形式
func (*EquipJackpotData) TableName() string {
	return "data_equip_jackpot"
}
