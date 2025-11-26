package entity

type TowerData struct {
	DataModel
	NpcID               uint32
	CampaignIDLower     uint32
	CampaignIDUpper     uint32
	Seq                 uint8
	Weight              int
	Rewards1            RewardStrings `gorm:"type:Varchar"`
	Rewards1Probability int
	Rewards2            RewardStrings `gorm:"type:Varchar"`
	Rewards2Probability int
	Rewards3            RewardStrings `gorm:"type:Varchar"`
	Rewards3Probability int
	Rewards4            RewardStrings `gorm:"type:Varchar"`
	Rewards4Probability int
}

//设置表名，默认是结构体的名的复数形式
func (*TowerData) TableName() string {
	return "data_tower"
}
