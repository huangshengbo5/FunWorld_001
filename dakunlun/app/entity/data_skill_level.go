package entity

type SkillLevelData struct {
	DataModel
	SkillID           uint32
	Level             uint16
	Describe          string
	TriggerType       uint8
	Seq               uint8
	MustHit           bool
	CanSkip           bool
	Probability       int
	Target            uint8
	EffectID          uint32
	EffectProbability int
	EffectVal1        int
	EffectVal2        int
}

//设置表名，默认是结构体的名的复数形式
func (*SkillLevelData) TableName() string {
	return "data_skill_level"
}
