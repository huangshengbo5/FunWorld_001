package entity

type EquipSkillData struct {
	DataModel
	TriggerType      uint8
	TriggerCondition uint8
	ConditionVal     int
	Target           uint8
	Seq              uint8
	Probability      int
	EffectID         uint32
	TriggerLimited   int
	Effect1Lower     int
	Effect1Upper     int
	Effect2Lower     int
	Effect2Upper     int
	Effect3Lower     int
	Effect3Upper     int
	Desc             string
}

//设置表名，默认是结构体的名的复数形式
func (*EquipSkillData) TableName() string {
	return "data_equip_skill"
}
