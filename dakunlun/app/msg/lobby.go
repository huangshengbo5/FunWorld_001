package msg

import (
	"dakunlun/app/entity"
	"dakunlun/app/service/battle"
)

type UserInfoRequest struct {
}

type User struct {
	Name           string `json:"name"`           //用户名
	Avatar         uint16 `json:"avatar"`         //头像
	Level          uint16 `json:"level"`          //等级
	GuideStep      uint8  `json:"guideStep"`      //新手引导步骤
	Gold           uint64 `json:"gold"`           //金币
	GoldIncrement  uint64 `json:"goldIncrement"`  //金币每秒增速
	GoldFlushIn    int64  `json:"goldFlushIn"`    //上次金币结算时间
	GoldBuffEndIn  int64  `json:"goldBuffEndIn"`  //双倍金币到期时间
	Diamond        uint32 `json:"diamond"`        //钻石
	SoulCrystal    uint32 `json:"soulCrystal"`    //魂石
	TreasureAnima  uint32 `json:"treasureAnima"`  //宝物精华
	SuperbiaStone  uint32 `json:"superbiaStone"`  //炽焰之源
	InvidiaStone   uint32 `json:"invidiaStone"`   //跃水之源
	AcediaStone    uint32 `json:"acediaStone"`    //飓风之源
	GulaStone      uint32 `json:"gulaStone"`      //大地之源
	AvaritiaStone  uint32 `json:"avaritiaStone"`  //光明之源
	LuxuriaStone   uint32 `json:"luxuriaStone"`   //黑暗之源
	IraStone       uint32 `json:"iraStone"`       //时空之源
	BenYuan        uint32 `json:"benYuan"`        //本源之力
	QianNeng       uint32 `json:"qianNeng"`       //潜能之力
	Element        uint32 `json:"element"`        //元素结晶
	Book           uint32 `json:"book"`           //秘传书
	CampaignNum    uint32 `json:"campaignNum"`    //关卡数
	MainHeroID     uint32 `json:"mainHeroID"`     //主将ID
	SubHeroID      uint32 `json:"subHeroID"`      //伙伴ID
	CastID         uint32 `json:"castID"`         //锻造ID
	Type           int    `json:"type"`           //用户账号类型1-4 对应4个年龄段
	SingleLimit    uint64 `json:"singleLimit"`    //单笔限额
	MonthLimit     uint64 `json:"monthLimit"`     //月限额
	BuyRefreshTime int64  `json:"buyRefreshTime"` //购买总额刷新时间
	BuyTotal       uint64 `json:"buyTotal"`       //当月购买总额
}

type UserExtend struct {
	OnlineReward   OnlineReward `json:"onlineReward"`   //在线奖励
	Campaign       Campaign     `json:"campaign"`       //关卡
	ArenaRemainNum uint8        `json:"arenaRemainNum"` //竞技场剩余挑战次数
	ArenaIsSign    bool         `json:"arenaIsSign"`    //竞技场报名状态
	Alchemy        Alchemy      `json:"alchemy"`        //炼药状态
	Ads            Ads          `json:"ads"`            //广告状态
	BusinessMan    int          `json:"businessMan"`    //商人状态
}

type Ads struct {
	Num uint8    `json:"num"` //当日累计看广告次数
	IDs []uint32 `json:"ids"` //当日领取过得广告奖励ID列表
}

type OnlineReward struct {
	NextReceiveTime int64    `json:"nextReceiveTime"` //下次领奖时间
	RemainAdsNum    int      `json:"remainAdsNum"`    //剩余广告次数
	PayNum          int      `json:"payNum"`          //支付次数
	RewardIDs       []uint32 `json:"rewardIDs"`       //奖励ID列表
}

type Campaign struct {
	LastCampainID   uint32 `json:"lastCampainID"`   //最后通过的关卡ID
	LastCampainTime int64  `json:"lastCampainTime"` //最后通关时间
	PreCampainID    uint32 `json:"preCampainID"`    //未领取奖励的关卡
}

type Alchemy struct {
	NextReceiveTime int64 `json:"nextReceiveTime"` //下次CD
	PayNum          int   `json:"payNum"`          //付费次数
}

type UserInfoResponse struct {
	User   *User       `json:"user"`
	Extend *UserExtend `json:"extend"`
}

type OnlineRewardShowRequest struct {
	Type  uint8  `json:"type"` //0 普通 1广告 2钻石
	AdsID string `json:"adsID"`
}

type OnlineRewardShowResponse struct {
	RewardIDs []uint32 `json:"rewardIDs"`
}

type OnlineRewardReceiveRequest struct {
	Type  uint8  `json:"type"` //0 普通 1广告
	AdsID string `json:"adsID"`
}

type OnlineRewardReceiveResponse struct {
	Rewards []*Reward
}

type MonsterShowRequest struct {
}

type MonsterShowResponse struct {
	CampaignID uint32     `json:"campaignID"`
	Monsters   []*Monster `json:"monsters"`
}

type Monster struct {
	TowerID uint32        `json:"towerID"`
	Status  entity.Status `json:"status"` // 0 创建 1战斗失败 2战斗胜利 3已领奖
}

type MonsterAttackRequest struct {
	Index uint8 `json:"index"`
}

type MonsterAttackResponse struct {
	Win     bool                 `json:"win"`
	Rewards []*Reward            `json:"rewards"`
	Report  *battle.BattleReport `json:"report"`
}

type MonsterReceiveRequest struct {
	Index uint8  `json:"index"`
	Type  uint8  `json:"type"` //0 普通 1广告
	AdsID string `json:"adsID"`
}

type MonsterReceiveResponse struct {
	Rewards []*Reward `json:"rewards"`
}

type AlchemyStartRequest struct {
	Type  uint8  `json:"type"` //0 普通 1广告
	AdsID string `json:"adsID"`
}
type AlchemyStartResponse struct {
	Rewards []*Reward `json:"rewards"`
}

type AlchemyClearRequest struct {
}
type AlchemyClearResponse struct {
}

type FromTo struct {
	From int64 `json:"from"`
	To   int64 `json:"to"`
}
type SystemConfigRequest struct {
}
type SystemConfigResponse struct {
	TowerSettings       map[string]FromTo `json:"towerSettings"`
	BusinessManSettings map[string]FromTo `json:"businessManSettings"`
}

type AnnalsShowRequest struct {
}

type HeroAnnals struct {
	HeroID           uint32 `json:"heroID"`
	FightingCapacity uint64 `json:"fightingCapacity"`
}

type AnnalsShowResponse struct {
	CampaignNum      uint32        `json:"campaignNum"`      //关卡数
	FightingCapacity uint64        `json:"fightingCapacity"` //总战力
	HeroList         []*HeroAnnals `json:"heroList"`         //英雄列表
	AnnalsIDs        []uint32      `json:"annalsIDs"`        //已经完成的成就ID列表
}

type AnnalsReceiveRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type AnnalsReceiveResponse struct {
	Rewards []*Reward `json:"rewards"`
}

type ChangeNameRequest struct {
	Name string `json:"name" binding:"required,min=2,max=30"`
}

type ChangeNameResponse struct {
}

type ChangeAvatarRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type ChangeAvatarResponse struct {
}

type GuideStepRequest struct {
	Step uint8 `json:"step" binding:"required,gt=0"`
}

type GuideStepResponse struct {
	Step uint8 `json:"step"`
}

type GuideBattleRequest struct {
}

type GuideBattleResponse struct {
	Win    bool                 `json:"win"`
	Report *battle.BattleReport `json:"report"`
}

type BusinessManReceiveRequest struct {
	Type  uint8  `json:"type"` //0 普通 1广告
	AdsID string `json:"adsID"`
}

type BusinessManReceiveResponse struct {
	Rewards []*Reward `json:"rewards"`
}

type CastRequest struct {
}

type CastResponse struct {
	ID uint32 `json:"id"` //铸造表ID
}

type BuyRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type BuyResponse struct {
	Error   bool      `json:"error"`
	Rewards []*Reward `json:"rewards"`
}
