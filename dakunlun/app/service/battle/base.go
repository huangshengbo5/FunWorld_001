package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

//初始化的技能 TODO 可以干掉这个适配层
type Skill struct {
	SkillID           uint32               //技能ID
	TriggerType       constant.TriggerType //触发时机
	Probability       int                  //触发概率
	Target            constant.Target      //目标
	EffectID          uint32               //效果ID
	EffectProbability int                  //效果几率
	Continue          bool                 //是否继续
	MustHit           bool                 //是否必须命中
	EffectVal1        int                  //效果值1
	EffectVal2        int                  //效果值2
	SubSkills         []*Skill             //附加能效果
}

func HeroSkillToBattleSkill(skillLevelEntitys []*entity.SkillLevelData) *SkillInfo {
	var s *Skill
	for _, v := range skillLevelEntitys {
		if v.Seq == 1 {
			s = &Skill{
				SkillID:           v.SkillID,
				TriggerType:       v.TriggerType,
				Probability:       v.Probability,
				Target:            v.Target,
				EffectID:          v.EffectID,
				EffectProbability: v.EffectProbability,
				Continue:          v.CanSkip,
				MustHit:           v.MustHit,
				EffectVal1:        v.EffectVal1,
				EffectVal2:        v.EffectVal2,
				SubSkills:         make([]*Skill, 0),
			}
		} else {
			s.SubSkills = append(s.SubSkills, &Skill{
				SkillID:           v.SkillID,
				TriggerType:       v.TriggerType,
				Probability:       v.Probability,
				Target:            v.Target,
				EffectID:          v.EffectID,
				EffectProbability: v.EffectProbability,
				Continue:          v.CanSkip,
				MustHit:           v.MustHit,
				EffectVal1:        v.EffectVal1,
				EffectVal2:        v.EffectVal2,
			})
		}
	}

	return &SkillInfo{
		SkillID:          skillLevelEntitys[0].SkillID,
		Type:             constant.SkillNormal,
		TriggerCondition: constant.ConditionUndefined,
		Target:           skillLevelEntitys[0].Target,
		ConditionVal:     0,
		TriggerType:      skillLevelEntitys[0].TriggerType,
		Probability:      skillLevelEntitys[0].Probability,
		TriggerLimited:   0,
		Effect:           makeEffect(s),
	}
}

func EquipSkillToBattleSkill(equipSkillEntity *entity.EquipSkillData) *SkillInfo {
	probability := equipSkillEntity.Probability
	if equipSkillEntity.EffectID == 30001 || equipSkillEntity.EffectID == 30012 {
		probability = equipSkillEntity.Effect1Lower
	}
	s := &Skill{
		SkillID:           equipSkillEntity.ID,
		TriggerType:       equipSkillEntity.TriggerType,
		Probability:       probability,
		Target:            equipSkillEntity.Target,
		EffectID:          equipSkillEntity.EffectID,
		EffectProbability: constant.BaseMultiple,
		EffectVal1:        equipSkillEntity.Effect1Lower,
		EffectVal2:        equipSkillEntity.Effect2Lower,
	}

	return &SkillInfo{
		SkillID:          equipSkillEntity.ID,
		Type:             constant.SkillEquip,
		TriggerCondition: equipSkillEntity.TriggerCondition,
		ConditionVal:     equipSkillEntity.ConditionVal,
		TriggerType:      equipSkillEntity.TriggerType,
		Probability:      equipSkillEntity.Probability,
		TriggerLimited:   equipSkillEntity.TriggerLimited,
		Effect:           makeEffect(s),
	}
}

//战斗中用的技能信息
type SkillInfo struct {
	SkillID          uint32                    //技能ID
	Type             constant.SkillType        //技能类型 1技能 2宝物
	TriggerCondition constant.TriggerCondition //技能条件
	ConditionVal     int                       //技能条件值
	TriggerType      constant.TriggerType      //触发时机
	Probability      int                       //触发几率
	TriggerNum       int                       //触发次数
	TriggerLimited   int                       //触发次数上线
	Target           constant.Target           //攻击对象

	Effect IEffect
}

func (s *SkillInfo) UseSkill(b *BattleInfo) bool {
	if s.IsTrigger(b) {
		if s.TriggerLimited > 0 {
			s.TriggerNum++
		}
		var targetID uint32
		if s.Target == constant.TargetEnemy {
			targetID = b.CurDefender.figher.GetID()
		} else {
			targetID = b.CurAttacker.figher.GetID()
		}
		//记录出手
		b.Report.AddActionStart(b.CurAttacker.attackContext, b.CurrentTime, b.CurAttacker.figher.GetID(),
			targetID, s.Type, s.SkillID)
		s.Effect.execute(b)
		//记录出手结束
		b.Report.AddActionEnd()
		return true
	}
	return false
}

func (s *SkillInfo) IsTrigger(b *BattleInfo) bool {
	if s.TriggerLimited > 0 && s.TriggerNum >= s.TriggerLimited {
		return false
	}

	if util.JudgeProbability(s.Probability) {
		switch s.TriggerCondition {
		//正常
		case constant.ConditionUndefined:
			return true
		//1：释放奥义后
		//case constant.ConditionZXC:
		//2：敌人生命高于m%时
		case constant.ConditionHPHigh:
			return b.CurDefender.ext.HpRatio() >= s.ConditionVal
		//3：敌人生命低于m%时
		case constant.ConditionHPLow:
			return b.CurDefender.ext.HpRatio() <= s.ConditionVal
		//4：敌人有异常状态时（眩晕、中毒、破防、迟缓、虚弱状态都算是异常）
		case constant.ConditionDeBuff:
			for _, buff := range b.CurDefender.ext.Buffs {
				if buff.Type == constant.BuffDecrHP &&
					buff.Type == constant.BuffFreeze &&
					buff.Type == constant.BuffDecrAttack &&
					buff.Type == constant.BuffDecrAttackFreq &&
					buff.Type == constant.BuffDecrDefend {
					return true
				}

			}
		//5：敌人性别 1男性 2女性
		case constant.ConditionSex:
			return b.CurDefender.figher.GetSex() == uint8(s.ConditionVal)
		//6：敌人种族为1魔物 2精灵 3恶魔 4天使
		case constant.ConditionRace:
			return b.CurDefender.figher.GetRace() == uint8(s.ConditionVal)
		//7：自身战力比敌人高时
		case constant.ConditionFightingCapacity:
			return b.CurAttacker.figher.GetFightingCapacity() > b.CurDefender.figher.GetFightingCapacity()
		//8：单次受到的伤害超过自身生命30%时
		case constant.ConditionDamage:
			if b.defendContext == nil || b.defendContext.damages <= 0 {
				return false
			}
			return int(float64(b.defendContext.damages)/float64(b.CurAttacker.ext.MaxHP)*constant.BaseMultiple) >= s.
				ConditionVal
		//9：死亡时
		//case constant.ConditionDead:
		//	return false
		//10：自身生命低于45%
		case constant.ConditionSelfHPLow:
			return b.CurAttacker.ext.HpRatio() <= s.ConditionVal
		//11：自身生命大于40%时
		case constant.ConditionSelfHPHigh:
			return b.CurAttacker.ext.HpRatio() >= s.ConditionVal
		}
	}

	return false
}

//战斗中buff
type Buff struct {
	Type       constant.BuffType //Buff类型
	Val1       int               //buff值1
	Val2       int               //buff值2
	RemainTime int               //剩余时间
}

type HpSkill struct {
	Ratio int     //概率 万分比
	Val   float64 //减少伤害比例 万分比
}

type HpNumSkill struct {
	RemainNum int     //剩余次数
	Val       float64 //减少伤害比例 万分比
}

type DefendSkill struct {
	Ratio int     //hp降低的万分比
	Val   float64 //提升防御 万分比
}

type DodgeSkill struct {
	Ratio int     //上臂 万分比
	Val   float64 //提升闪避 万分比
}

type FighterRuntime struct {
	ID     uint32 //用户ID
	CDTime int    //冷却
	MaxHP  int64
	HP     int64 //血量

	Attack float64 //攻击力
	Defend float64 //防御力

	InitAttackPlus int //初始攻击力增加
	InitDefendPlus int //初始防御力增加

	AttackPlus           int     //攻击转换比例加成 万分比
	DefendPlus           int     //防御转换比例加成 万分比
	FightingCapacityPlus int     //战斗力增加比例 万分比
	CriticalPlus         float64 //暴击倍率

	Hit         float64 //命中 影响角色攻击的命中能力
	Dodge       float64 //闪避 影响角色闪避攻击的能力
	Critical    float64 //暴击 影响角色的暴击几率
	Tenacity    float64 //韧性 影响角色的被暴击几率
	Break       float64 //破甲 无视对方一定防御能力
	Impregnable float64 //铁壁 抵消破甲的效果
	Defuse      int     //化解 影响角色最终减伤

	//永久加成
	AttackRatio     float64 //攻击力加成
	DefendRatio     float64 //防御力加成
	DodgeRatio      float64 //闪避率加成
	CriticalRatio   float64 //暴击率加成
	HpProtected     float64 //减伤加成
	AttackFreqRatio float64 //攻击力加成

	DebuffRatio      float64      //buff持续时间降低
	HpProtectedSkill *HpSkill     //按几率减伤
	HpLimitedSkill   *HpNumSkill  //减伤指定次数
	DefendSkill      *DefendSkill //hp每降低万分之n 增加防御m
	DodgeSkill       *DodgeSkill  //hp每降低万分之n 增加闪避m

	//一次行动加成
	AttackOnce      float64 //攻击力加成
	DefendOnce      float64 //防御力加成
	DodgeOnce       float64 //闪避率加成
	CriticalOnce    float64 //暴击率加成
	HpProtectedOnce float64 //减伤加成
	AttackFreqOnce  float64 //攻击力加成

	IsFreeze     bool //冻结
	FreezeRemain int  //冻结剩余时间

	Buffs  map[constant.BuffType]*Buff           //buff
	Skills map[constant.TriggerType][]*SkillInfo //技能
}

func (f *FighterRuntime) GetDodge() float64 {
	v := f.Dodge
	if f.DodgeSkill != nil {
		mul := f.HpRatio() / f.DodgeSkill.Ratio
		if mul > 0 {
			v = v * (1 + f.DodgeSkill.Val)
		}
	}
	return v
}

func (f *FighterRuntime) GetDefend() float64 {
	v := f.Defend
	if f.DefendSkill != nil {
		mul := f.HpRatio() / f.DefendSkill.Ratio
		if mul > 0 {
			v = v * (1 + f.DefendSkill.Val)
		}
	}
	return v
}

func (f *FighterRuntime) TotalDecrHP() int64 {
	return f.MaxHP - f.HP
}

func (f *FighterRuntime) IncrHP(v int64) int64 {
	v = util.MinInt64(v, f.TotalDecrHP())
	f.HP += v
	return v
}

func (f *FighterRuntime) DecrHP(v int64) int64 {
	v = util.MinInt64(v, f.HP)
	f.HP -= v
	return v
}

func (f *FighterRuntime) IsDead() bool {
	return f.HP == 0
}

//剩余生命万分比
func (f *FighterRuntime) HpRatio() int {
	return int(float64(f.HP) / float64(f.MaxHP) * constant.BaseMultiple)
}

func (f *FighterRuntime) AddBuff(buffType constant.BuffType, remainTime int, val1, val2 int) {
	if _, exist := f.Buffs[buffType]; !exist {
		f.Buffs[buffType] = &Buff{
			Type: buffType,
		}
	}
	//减少持续时间的buff
	if f.DebuffRatio > 0 && constant.IsDeBuff(buffType) {
		remainTime = int(float64(remainTime) * (1 - f.DebuffRatio))
	}
	f.Buffs[buffType].RemainTime = remainTime
	f.Buffs[buffType].Val1 = val1
	f.Buffs[buffType].Val2 = val2
}

func (f *FighterRuntime) ClearBuff(buffType constant.BuffType) bool {
	if _, exist := f.Buffs[buffType]; !exist {
		delete(f.Buffs, buffType)
		return true
	} else {
		return false
	}
}

func (f *FighterRuntime) GetAttackRatio() (ratio float64) {
	ratio = f.AttackRatio + f.AttackOnce
	if v, exist := f.Buffs[constant.BuffDecrAttack]; exist {
		ratio -= float64(v.Val1) / constant.BaseMultiple
	}
	if v, exist := f.Buffs[constant.BuffIncrAttack]; exist {
		ratio += float64(v.Val1) / constant.BaseMultiple
	}
	return
}

func (f *FighterRuntime) GetDefendRatio() (ratio float64) {
	ratio = f.DefendRatio + f.DefendRatio
	if v, exist := f.Buffs[constant.BuffDecrDefend]; exist {
		ratio -= float64(v.Val1) / constant.BaseMultiple
	}
	if v, exist := f.Buffs[constant.BuffIncrDefend]; exist {
		ratio += float64(v.Val1) / constant.BaseMultiple
	}

	return
}

func (f *FighterRuntime) GetDodgeRatio() (ratio float64) {
	ratio = f.DodgeRatio + f.DodgeOnce
	return
}

func (f *FighterRuntime) GetHpProtectedRatio() (ratio float64) {
	ratio = f.HpProtected
	if v, exist := f.Buffs[constant.BuffHpProtected]; exist {
		ratio += float64(v.Val1) / constant.BaseMultiple
	}
	return
}

func (f *FighterRuntime) GetCriticalRatio() (ratio float64) {
	ratio = f.CriticalRatio
	return
}

type CurFigher struct {
	figher        constant.IFighter
	ext           *FighterRuntime
	attackContext *AttackContext
}

func (cf *CurFigher) resetAttack() {
	cf.ext.Attack = float64(cf.getFightingCapacity()) * float64(cf.figher.GetAttackPlus()+cf.ext.
		AttackPlus) / constant.BaseMultiple
}
func (cf *CurFigher) resetDefend() {
	cf.ext.Defend = float64(cf.getFightingCapacity()) * float64(cf.figher.GetDefendPlus()+cf.ext.
		DefendPlus) / constant.BaseMultiple
}

func (cf *CurFigher) getFightingCapacity() uint64 {
	return uint64(float64(cf.figher.GetFightingCapacity()) * float64(constant.BaseMultiple+cf.ext.
		FightingCapacityPlus) / constant.BaseMultiple)
}

type BattleInfo struct {
	Attacker      constant.IFighter //基础属性信息
	Defender      constant.IFighter
	AtkExt        *FighterRuntime //运行时会变化的信息
	DefExt        *FighterRuntime
	CurAttacker   *CurFigher //本次行动攻击方
	CurDefender   *CurFigher //本次行动防御方
	FightType     uint8
	MaxTime       int
	CurrentTime   int
	Background    uint16
	Result        uint8
	Report        *BattleReport
	attackContext *AttackContext
	defendContext *AttackContext
}

type AttackContext struct {
	isHit         bool   //攻击是否命中
	isCritical    bool   //攻击是否暴击
	damages       int64  //攻击伤害
	totalDamages  int64  //攻击累计伤害
	isReborn      bool   //攻击方复活
	attackTrigger bool   //攻击是否发动
	skillID       uint32 //血量修正（用于技能22）
	retriggerZXC  bool   //重放必杀技（用于技能1）

	//reIsHit         bool //反击是否命中
	//reIsCritical    bool //反击是否暴击
	//reDamages       int  //反击伤害
	//reTotalDamages  int  //反击累计伤害
	//reIsReborn      bool //防御方复活
	//reAttackTrigger bool //反击是否发动
}

func (b *BattleInfo) Init() {
	// 出手速度差值
	freqDiff := util.AbsInt(b.Attacker.GetAttackFreq() - b.Defender.GetAttackFreq())
	if b.Attacker.GetAttackFreq() <= b.Defender.GetAttackFreq() {
		b.AtkExt.CDTime, b.DefExt.CDTime = 0, freqDiff
	} else {
		b.AtkExt.CDTime, b.DefExt.CDTime = freqDiff, 0
	}

	// 血量初始化
	//b.AtkExt.HP = b.Attacker.GetFightingCapacity()
	//b.DefExt.HP = b.Defender.GetFightingCapacity()

	b.CurAttacker = &CurFigher{
		b.Attacker,
		b.AtkExt,
		&AttackContext{},
	}
	b.CurDefender = &CurFigher{
		b.Defender,
		b.DefExt,
		&AttackContext{},
	}

	b.CurAttacker.resetAttack()
	b.CurAttacker.resetDefend()
	b.CurDefender.resetAttack()
	b.CurDefender.resetDefend()

	b.Report = &BattleReport{
		Background: b.Background,
		Type:       b.FightType,
		MaxTime:    b.MaxTime,
		Attacker: ReportPlayer{
			ID:               b.Attacker.GetID(),
			Name:             b.Attacker.GetName(),
			Avatar:           b.Attacker.GetAvatar(),
			HeroID:           b.Attacker.GetHeroID(),
			Level:            b.Attacker.GetLevel(),
			HP:               b.AtkExt.HP,
			MaxHP:            b.AtkExt.MaxHP,
			FType:            b.Attacker.GetFType(),
			FightingCapacity: b.Attacker.GetFightingCapacity(),
		},
		Defender: ReportPlayer{
			ID:               b.Defender.GetID(),
			Name:             b.Defender.GetName(),
			Avatar:           b.Defender.GetAvatar(),
			HeroID:           b.Defender.GetHeroID(),
			Level:            b.Defender.GetLevel(),
			HP:               b.DefExt.HP,
			MaxHP:            b.DefExt.MaxHP,
			FType:            b.Defender.GetFType(),
			FightingCapacity: b.Defender.GetFightingCapacity(),
		},
	}

	//攻击方：战斗开始技能
	if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerBattleStart]; exist {
		for _, skill := range skills {
			skill.UseSkill(b)
		}
	}

	b.changeSide()
	//防御方：战斗开始技能
	if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerBattleStart]; exist {
		for _, skill := range skills {
			skill.UseSkill(b)
		}
	}
	b.changeSide()
}

// 用户行动前
func (b *BattleInfo) PreDo() {
	for _, v := range []*FighterRuntime{b.AtkExt, b.DefExt} {

		v.IsFreeze = false //重置状态
		for buffType, buff := range v.Buffs {
			switch buffType {
			case constant.BuffIncrHP:
				if buff.RemainTime%constant.OneSecond == 0 {
					//恢复最大血量的百分比
					b.Report.BalanceBuff(BuffStatusBalance, buff.Type, v.ID, buff.RemainTime, int(v.IncrHP(v.MaxHP*int64(buff.Val1)/constant.BaseMultiple)), 0, b.CurrentTime)
				}
			case constant.BuffDecrHP:
				if buff.RemainTime%constant.OneSecond == 0 {
					//降低当前血量百分比
					b.Report.BalanceBuff(BuffStatusBalance, buff.Type, v.ID, buff.RemainTime,
						int(v.DecrHP(v.HP*int64(buff.Val1)/constant.BaseMultiple)), 0, b.CurrentTime)
				}
			case constant.BuffFreeze:
				v.IsFreeze = true
				v.FreezeRemain = buff.RemainTime
			}
			//移除到期buff
			if buff.RemainTime == 0 {
				delete(v.Buffs, buffType)
				b.Report.BalanceBuff(BuffStatusClear, buffType, v.ID, 0, 0, 0, b.CurrentTime)
			}
		}
	}
}

func (b *BattleInfo) CheckFreeze() {
	for _, v := range []*FighterRuntime{b.AtkExt, b.DefExt} {
		v.IsFreeze = false //重置状态
		if buff, exist := v.Buffs[constant.BuffFreeze]; exist {
			v.IsFreeze = true
			v.FreezeRemain = buff.RemainTime
		}
	}
}

// 返回值 continueRound bool
func (b *BattleInfo) Do() (continueRound bool) {
	//b.attackContext = &AttackContext{}
	//b.defendContext = &AttackContext{}

	if b.DefExt.HP <= 0 || b.AtkExt.HP <= 0 {
		return
	}

	//判断是同时行动
	if b.AtkExt.CDTime == 0 && b.DefExt.CDTime == 0 {
		continueRound = true
	}

	needAttack := false
	//攻击方优先行动
	if b.AtkExt.CDTime == 0 {
		b.CurAttacker = &CurFigher{
			b.Attacker,
			b.AtkExt,
			&AttackContext{},
		}
		b.CurDefender = &CurFigher{
			b.Defender,
			b.DefExt,
			&AttackContext{},
		}
		needAttack = true
		//防御方行动
	} else if b.DefExt.CDTime == 0 {
		b.CurAttacker = &CurFigher{
			b.Defender,
			b.DefExt,
			&AttackContext{},
		}
		b.CurDefender = &CurFigher{
			b.Attacker,
			b.AtkExt,
			&AttackContext{},
		}
		needAttack = true
	}

	if needAttack {
		if b.CurAttacker.ext.IsFreeze {
			b.Report.BalanceBuff(BuffStatusBalance, constant.BuffFreeze, b.CurAttacker.figher.GetID(),
				b.CurAttacker.ext.FreezeRemain, 0, 0, b.CurrentTime)
		} else {
			//进行攻击
			b.doAttack()
		}

		// 重置CD时间
		b.CurAttacker.ext.CDTime = b.CurAttacker.figher.GetAttackFreq()
	}

	return
}

// 用户行动后
func (b *BattleInfo) AfterDo() (costTime int) {
	//计算耗时
	costTime = b.getCostTime()
	//更新CD时间
	b.AtkExt.CDTime -= costTime
	b.DefExt.CDTime -= costTime

	b.resetData(costTime)

	return
}

func (b *BattleInfo) doAttack() {
	//攻击方：攻击前
	if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerBeforeAttack]; exist {
		for _, skill := range skills {
			skill.UseSkill(b)
		}
	}

	b.changeSide()
	//防御方：被攻击前
	if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerBeforeBeAttack]; exist {
		for _, skill := range skills {
			skill.UseSkill(b)
		}
	}
	b.changeSide()

	// 生成攻击上下文
	b.CurAttacker.attackContext.isHit = b.isHit()
	b.CurAttacker.attackContext.isCritical = b.isCritical()

	//攻击方:攻击时技能
	if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerZXC]; exist {
		for _, skill := range skills {
			b.CurAttacker.attackContext.attackTrigger = skill.UseSkill(b)
			if b.CurAttacker.attackContext.attackTrigger {
				//攻击方-奥义释放后技能
				if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerAfterZXC]; exist {
					for _, skill := range skills {
						skill.UseSkill(b)
					}
				}
				break
			}
		}
	}

	//默认攻击技能（在攻击时技能未能触发时使用）
	if !b.CurAttacker.attackContext.attackTrigger {
		if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerDefault]; exist {
			for _, skill := range skills {
				b.CurAttacker.attackContext.attackTrigger = skill.UseSkill(b)
				if b.CurAttacker.attackContext.attackTrigger {
					break
				}
			}
		}
	}

	//防御方死亡
	if b.CurDefender.ext.IsDead() {
		b.changeSide()
		//防御方-复活技能
		if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerDeadth]; exist {
			for _, skill := range skills {
				skill.UseSkill(b)
			}
		}
		b.changeSide()
	} else {
		//攻击方-攻击后技能
		if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerAfterAttack]; exist {
			for _, skill := range skills {
				skill.UseSkill(b)
				//死亡要跳出
				if b.CurDefender.ext.IsDead() {
					break
				}
				//二次触发必杀
				if b.CurAttacker.attackContext.retriggerZXC {
					b.CurAttacker.ext.Skills[constant.TriggerZXC][0].UseSkill(b)
					b.CurAttacker.attackContext.retriggerZXC = false
				}
			}
		}

		if b.CurDefender.ext.IsDead() {
			b.changeSide()
			//防御方-复活技能
			if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerDeadth]; exist {
				for _, skill := range skills {
					skill.UseSkill(b)
				}
			}
			b.changeSide()
		} else {
			b.changeSide()
			// 生成攻击上下文
			b.CurAttacker.attackContext.isHit = b.isHit()
			b.CurAttacker.attackContext.isCritical = b.isCritical()

			//防御方-反击
			if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerCounterAttack]; exist {
				for _, skill := range skills {
					b.CurAttacker.attackContext.attackTrigger = skill.UseSkill(b)
				}
			}
			b.changeSide()

			// 攻击方死亡
			if b.CurAttacker.ext.IsDead() {
				//攻击方-复活技能
				if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerDeadth]; exist {
					for _, skill := range skills {
						skill.UseSkill(b)
					}
				}
			} else {
				//防御方-被攻击
				if b.CurAttacker.attackContext.isHit {
					b.changeSide()
					if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerBeAttacked]; exist {
						for _, skill := range skills {
							skill.UseSkill(b)
						}
					}
					b.changeSide()
				}
			}
		}
	}

}

// 基础攻击
func (b *BattleInfo) baseAttack(AttackSkillRatio float64, throughRatio float64) (damages int64, isDead bool) {
	attackRatio := b.CurAttacker.ext.GetAttackRatio()
	defendRatio := b.CurDefender.ext.GetDefendRatio()
	hpProtectedRatio := b.CurDefender.ext.GetHpProtectedRatio()

	criticalPlus := 1.0
	if b.CurAttacker.attackContext.isCritical {
		criticalPlus = b.CurAttacker.ext.CriticalPlus
	}

	var defuseRatio float64
	//	每1000点化解，获得0.2%伤害减免
	if b.CurDefender.ext.Defuse > 0 {
		tmp := (b.CurDefender.ext.Defuse / 1000) * 20
		defuseRatio = float64(tmp) / constant.BaseMultiple
	}

	// 概率减伤技能
	var hpSkillRatio float64 = 1
	if b.CurDefender.ext.HpProtectedSkill != nil {
		if util.JudgeProbability(b.CurDefender.ext.HpProtectedSkill.Ratio) {
			hpSkillRatio = b.CurDefender.ext.HpProtectedSkill.Val
		}
	}

	// 限制次数减伤技能
	if b.CurDefender.ext.HpLimitedSkill != nil {
		if b.CurDefender.ext.HpLimitedSkill.RemainNum > 0 {
			hpSkillRatio += b.CurDefender.ext.HpLimitedSkill.Val
			b.CurDefender.ext.HpLimitedSkill.RemainNum -= 1
		}
	}

	//最终攻击力=max([攻击方面板攻击力*（1+各类攻击力增幅）],1)
	finalAttack := util.MaxFloat(b.CurAttacker.ext.Attack*(1+attackRatio), 1)

	//最终防御力=max([（守方面板防御力*（1+各类防御力增幅）*贯穿系数],0)
	finalDefend := util.MaxFloat(b.CurDefender.ext.GetDefend()*(1+defendRatio)*throughRatio, 0)

	//最终伤害=max([（（最终攻击力-最终防御力）*技能系数*暴击倍率*最终伤害加成）*最终减伤加成],1)
	theoryDamages := int64(util.MaxFloat((finalAttack-finalDefend)*AttackSkillRatio*criticalPlus*util.MaxFloat(0,
		(1-hpProtectedRatio-defuseRatio))*hpSkillRatio, 1))
	//{[（攻方面板攻击力*（1+各种攻击力增幅）*技能系数）-（守方面板防御力*（1+各种防御力增幅）*min((守方铁壁/攻方破甲)，1)*贯穿系数）]*暴击倍率}*{max（（1-各类减伤加成）,0）}
	//theoryDamages := int((b.CurAttacker.ext.Attack*(1+attackRatio)*AttackSkillRatio - b.
	//	CurDefender.ext.GetDefend()*(1+defendRatio)*util.MinFloat((b.CurDefender.ext.Impregnable/b.
	//	CurAttacker.ext.Break), 1)*throughRatio) * criticalPlus * util.MaxFloat(0,
	//	(1-hpProtectedRatio-defuseRatio)) * hpSkillRatio)

	//理论伤害为负值 默认掉1HP
	//if theoryDamages <= 0 {
	//	theoryDamages = 1
	//}

	damages = b.CurDefender.ext.DecrHP(theoryDamages)

	//理论可死亡时
	if b.CurDefender.ext.IsDead() {
		//防御方-复活技能
		if skills, exist := b.CurAttacker.ext.Skills[constant.TriggerGoingToDeadth]; exist {
			for _, skill := range skills {
				if skill.IsTrigger(b) {
					skill.Effect.execute(b)
					//更新发动次数
					if skill.TriggerLimited > 0 {
						skill.TriggerNum += 1
					}
				}
			}
		}
	}

	b.CurAttacker.attackContext.damages = damages
	isDead = b.CurDefender.ext.IsDead()
	return
}

// 是否命中
func (b *BattleInfo) isHit() bool {
	dodgeRatio := b.CurDefender.ext.GetDodgeRatio()
	//命中=min（（（攻方命中*2）/（攻方命中+守方回避）），0.9））*（1-守方各类回避加成）
	hitProbability := util.MinFloat((b.CurAttacker.ext.Hit*2)/(b.CurAttacker.ext.Hit+b.CurDefender.ext.GetDodge()),
		0.9) * (1 - dodgeRatio)

	return util.JudgeProbability(int(hitProbability * constant.BaseMultiple))
}

// 是否暴击
func (b *BattleInfo) isCritical() bool {
	ratio := b.CurAttacker.ext.GetCriticalRatio()
	//min（（攻方暴击/（攻方暴击+守方坚韧）*（1+攻方各类暴击率加成）),1)
	critical := util.MinFloat((b.CurAttacker.ext.Critical)/(b.CurAttacker.ext.Critical+b.CurAttacker.ext.Tenacity)*(1+ratio), 1)
	return util.JudgeProbability(int(critical * constant.BaseMultiple))
}

// 获取扣除时间
func (b *BattleInfo) getCostTime() (costTime int) {
	costTime = util.MinInt(b.AtkExt.CDTime, b.DefExt.CDTime)

	//处理扣除时间
	for _, v := range []*FighterRuntime{b.AtkExt, b.DefExt} {

		for buffType, buff := range v.Buffs {
			compareTime := 0
			if buffType == constant.BuffDecrHP || buffType == constant.BuffIncrHP {
				compareTime = buff.RemainTime % constant.OneSecond
				if compareTime == 0 {
					compareTime = constant.OneSecond
				}
			} else {
				compareTime = buff.RemainTime
			}
			if costTime > compareTime {
				costTime = compareTime
			}
		}
	}

	return

}

// 刷新回合后数据
func (b *BattleInfo) resetData(costTime int) {
	//处理扣除时间
	for _, v := range []*FighterRuntime{b.AtkExt, b.DefExt} {
		//重置buff剩余时间
		for _, buff := range v.Buffs {
			buff.RemainTime -= costTime
		}

		//重置单次加成
		v.AttackOnce = 0
		v.DefendOnce = 0
		v.DodgeOnce = 0
		v.CriticalOnce = 0
		v.HpProtectedOnce = 0
		v.AttackFreqOnce = 0
		v.IsFreeze = false
	}

	return

}

// 交换攻击防御方
func (b *BattleInfo) changeSide() {
	b.CurAttacker, b.CurDefender = b.CurDefender, b.CurAttacker
}
