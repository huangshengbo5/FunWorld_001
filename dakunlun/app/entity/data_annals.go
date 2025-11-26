package entity

const (
	AnnalsTypeNew      = 999 //新账号
	AnnalsTypeCampaign = 1   //关卡数量
	AnnalsTypeGetHero  = 2   //获得指定伙伴
	AnnalsTypeFC       = 3   //总战力
	AnnalsTypeHeroFC   = 4   //伙伴战力
)

type AnnalsData struct {
	DataModel
	Type    uint16
	SubType uint32
	Value   uint32
	Rewards RewardStrings
}

//设置表名，默认是结构体的名的复数形式
func (*AnnalsData) TableName() string {
	return "data_annals"
}
