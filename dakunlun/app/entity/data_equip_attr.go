package entity

import "dakunlun/app/util"

type EquipAttrData struct {
	DataModel
	JackpotID        uint32
	EffectID         uint32
	EffectValueLower uint32
	EffectValueUpper uint32
	Weight           int
}

//设置表名，默认是结构体的名的复数形式
func (*EquipAttrData) TableName() string {
	return "data_equip_attr"
}

func (e *EquipAttrData) RandomVal() uint32 {
	return util.RandUint32(e.EffectValueLower, e.EffectValueUpper)
}
