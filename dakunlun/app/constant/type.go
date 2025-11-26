package constant

import "dakunlun/app/entity"

// 分页信息
type Paging struct {
	Page     int `json:"page"`
	PerPage  int `json:"perPage"`
	TotalNum int `json:"totalNum"`
}

// 奖励上下文
type RewardContext struct {
	UserEntity   *entity.UserEntity
	UserService  IUserService
	EquipService IEquipService
}

// 奖励对象接口
type IReward interface {
	GetMainType() uint16
	GetSubType() uint32
	GetVal() uint64
	NeedMerge() bool
	RealValue(interface{}) uint64 // 用公式计算真实VALUE
	SetRealValue(interface{})     // 设置真实奖励值
	GetFormulaID() uint16
	Merge(IReward) // 合并奖励
	Send() error
}

// 奖励数组
type Rewards []IReward

type IUserService interface {
	IncrAssets(userEntity *entity.UserEntity, costType uint16, costSubType uint32, costVal uint64) (err error)
}

type IEquipService interface {
	AddEquip(userEntity *entity.UserEntity, equipID uint32) (heroEquipEntity *entity.HeroEquipEntity, err error)
}

// 战斗上下文
type BattleContext struct {
	UserEntity *entity.UserEntity
}

type IFighter interface {
	GetID() uint32
	GetName() string
	GetType() uint8
	GetFightingCapacity() uint64
	GetAttackFreq() int
	GetSex() uint8
	GetRace() uint8
	GetAttackPlus() int
	GetDefendPlus() int
	GetAvatar() uint16
	GetFType() uint8
	GetHeroID() uint32
	GetLevel() uint16
	GetSkills() entity.Skills
}
