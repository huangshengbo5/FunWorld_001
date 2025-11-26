package entity

type OnlineRewardData struct {
	DataModel
	Rewards     RewardStrings `gorm:"type:Varchar"`
	Probability int
}

//设置表名，默认是结构体的名的复数形式
func (*OnlineRewardData) TableName() string {
	return "data_online_reward"
}
