package entity

type ArenaRewardData struct {
	DataModel
	RankTop    uint16
	RankBottom uint16
	Rewards    RewardStrings
}

//设置表名，默认是结构体的名的复数形式
func (*ArenaRewardData) TableName() string {
	return "data_arena_reward"
}
