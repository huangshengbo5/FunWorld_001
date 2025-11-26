package entity

const (
	// 主角
	HeroTypeMain = 1
	// 伙伴
	HeroTypePartner = 2
)

const (
	InitMainHeroID  = 1001
	InitHeroLevel   = 1
	InitEvolveTimes = 0
)

type HeroData struct {
	DataModel
	Type        uint8
	Name        string
	Resource    string
	Sex         uint8
	Race        uint8
	Face        string
	ZoomRatio   uint8
	AttackFreq  uint16
	Skill1ID    uint32
	Skill2ID    uint32
	Skill3ID    uint32
	HpRatio     uint16
	AttackRatio uint16
	DefendRatio uint16
	CampaignID  uint32
	Level       uint16
}

//设置表名，默认是结构体的名的复数形式
func (*HeroData) TableName() string {
	return "data_hero"
}
