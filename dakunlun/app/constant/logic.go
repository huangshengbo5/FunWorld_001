package constant

const (
	// 开启条件 关卡数量
	OpenTypeCampaignNum = 1
	// 开启条件 用户等级
	OpenTypeLevel = 2
)

const (
	// 消耗类型 金币
	CostTypeGold = 1
	// 消耗类型 钻石
	CostTypeDiamond = 2
	// 消耗类型 魂石
	CostTypeSoulCrystal = 3
	// 消耗类型 宝物精华
	CostTypeTreasureAnima = 4
	// 消耗类型 炽焰之源
	CostTypeSuperbiaStone = 5
	// 消耗类型 跃水之源
	CostTypeInvidiaStone = 6
	// 消耗类型 飓风之源
	CostTypeAcediaStone = 7
	// 消耗类型 大地之源
	CostTypeGulaStone = 8
	// 消耗类型 光明之源
	CostTypeAvaritiaStone = 9
	// 消耗类型 黑暗之源
	CostTypeLuxuriaStone = 10
	// 消耗类型 时空之源
	CostTypeIraStone = 11
	// 消耗类型 本源之力
	CostTypeBenYuan = 14
	// 消耗类型 潜能之力
	CostTypeQianNeng = 15
	// 消耗类型 元素结晶
	CostTypeElement = 17
	// 消耗类型 秘传书
	CostTypeBook = 18
)

const (
	// 效果类型 金币产量
	EffectIDGoldBase = 1
	// 效果类型 金币产量加成百分比
	EffectIDGoldRatio = 2
	// 效果类型 主角费用
	EffectIDMainRatio = 3
	// 效果类型 伙伴费用
	EffectIDSubRatio = 7
	// 效果类型 主角战斗力
	EffectIDMainFightPlus = 4
	// 效果类型 伙伴战斗力
	EffectIDSubFightPlus = 8
	// 效果类型 攻击力
	EffectIDAttack = 5
	// 效果类型 防御力
	EffectIDDefend = 6
)

const (
	// 战士类型 测试
	FTypeTester = 0
	// 战士类型 用户
	FTypePlayer = 1
	// 战士类型 NPC
	FTypeNPC = 2
)

// 作用目标
type Target = uint8

const (
	TargetSelf Target = iota + 1
	TargetEnemy
)

//技能触发时机
type TriggerType = uint8

const (
	//未定义0
	TriggerUndefined TriggerType = iota
	//战斗开始前1
	TriggerBattleStart
	//自身攻击前2
	TriggerBeforeAttack
	//自身攻击时-大招3
	TriggerZXC
	//被攻击前4
	TriggerBeforeBeAttack
	//被攻击时-反击5
	TriggerCounterAttack
	//被攻击后6
	TriggerBeAttacked
	//自身攻击后7
	TriggerAfterAttack
	//死亡时8
	TriggerDeadth
	//攻击时如果默认使用技能-普通攻击9
	TriggerDefault
	//大招释放后10
	TriggerAfterZXC
	//要死亡时11
	TriggerGoingToDeadth
)

//  触发
type SkillType uint8

const (
	SkillNormal SkillType = iota + 1
	SkillEquip
)

//  触发
type TriggerCondition = uint8

const (
	ConditionUndefined TriggerCondition = iota
	//1：释放奥义后
	ConditionZXC
	//2：敌人生命高于m%时
	ConditionHPHigh
	//3：敌人生命低于m%时
	ConditionHPLow
	//4：敌人有异常状态时（眩晕、中毒、破防、迟缓、虚弱状态都算是异常）
	ConditionDeBuff
	//5：敌人性别 1男性 2女性
	ConditionSex
	//6：敌人种族为1魔物 2精灵 3恶魔 4天使
	ConditionRace
	//7：自身战力比敌人高时
	ConditionFightingCapacity
	//8：单次受到的伤害超过自身生命30%时
	ConditionDamage
	//9：死亡时
	ConditionDead
	//10：自身生命低于45%
	ConditionSelfHPLow
	//11：自身生命大于40%时
	ConditionSelfHPHigh
)

type BuffType uint8

const (
	//中毒 0
	BuffDecrHP BuffType = iota
	//眩晕 1
	BuffFreeze
	//降低攻击力 2
	BuffDecrAttack
	//降低攻击频率 3
	BuffDecrAttackFreq
	//降低防御力 4
	BuffDecrDefend

	//回血 5
	BuffIncrHP
	//增加攻击力 6
	BuffIncrAttack
	//增加攻击频率 7
	BuffIncrAttackFreq
	//增加防御力 8
	BuffIncrDefend
	//减少自身伤害 9
	BuffHpProtected
	//免疫指定技能 10-12
	BuffImmune10007
	BuffImmune10008
	BuffImmune10009
)

func IsDeBuff(v BuffType) bool {
	for _, buffType := range []BuffType{BuffDecrHP, BuffFreeze, BuffDecrAttack, BuffDecrAttackFreq, BuffDecrDefend} {
		if v == buffType {
			return true
		}
	}
	return false
}

func IsBuff(v BuffType) bool {
	return !IsDeBuff(v)
}

const (
	IsThrough = iota
	NoThrough
)

const BaseMultiple = 10000

const OneSecond = 1000

const (
	SexUndefined = iota
	SexMale
	SexFamale
)
const (
	RaceUndefined = iota
	//1魔物
	RaceDevil
	//2精灵
	RaceElf
	//3恶魔
	RaceDemon
	//4天使
	RaceAngel
)

const (
	CampaignMaxTime = 120000
)

const (
	FightTypeCampaign = iota + 1
	FightTypeApocalypse
	FightTypeTower
	FightTypeArena
)
