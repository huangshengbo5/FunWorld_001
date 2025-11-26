package entity

type AdsData struct {
	DataModel
	Val     uint8
	Rewards RewardStrings
}

//设置表名，默认是结构体的名的复数形式
func (*AdsData) TableName() string {
	return "data_ads"
}
