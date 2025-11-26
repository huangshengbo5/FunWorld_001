package entity

const (
	//任务类型 主线
	CampaignTypeMain uint8 = 1
)

type CampaignData struct {
	DataModel
	ID               uint32
	Type             uint8
	Resource         string
	Name             string
	FightingCapacity uint64        //建议战斗力
	X                int           //地图坐标
	Y                int           //地图坐标
	PrevID           uint32        //前置关卡ID
	NextID           uint32        //后置关卡ID
	NpcID            uint32        //NPC_ID
	Rewards          RewardStrings `gorm:"type:Varchar"`
	HeroID           uint32
	BackgroundID     uint16 //背景
}

//设置表名，默认是结构体的名的复数形式
func (*CampaignData) TableName() string {
	return "data_campaign"
}
