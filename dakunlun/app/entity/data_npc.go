package entity

type NpcData struct {
	DataModel
	ID               uint32
	Level            uint16
	Name             string
	Sex              uint8
	Race             uint8
	Resource         string
	Avatar           string
	AvatarID         uint16
	ZoomRatio        uint8
	AttackFreq       uint16
	FightingCapacity uint64
	Critical         uint32
	Tenacity         uint32
	Break            uint32
	Impregnable      uint32
	Hit              uint32
	Dodge            uint32
	Defuse           uint32
	Skill1ID         uint32
	Skill2ID         uint32
	Skill3ID         uint32
	Skill4ID         uint32
	Skill5ID         uint32
	Skill6ID         uint32
	AttackPlus       uint16
	DefendPlus       uint16
}

//设置表名，默认是结构体的名的复数形式
func (*NpcData) TableName() string {
	return "data_npc"
}
