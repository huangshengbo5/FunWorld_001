package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//m秒内攻击频率提升n%
// 效果值1:5000 持续时间毫秒
//  效果值2:2500  25%
type Effect10010 struct {
	*EffectBase
}

func newEffect10010(Skill *Skill) *Effect10010 {
	return &Effect10010{
		normalEffect(Skill),
	}
}

func (e *Effect10010) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		b.CurAttacker.ext.AddBuff(constant.BuffIncrAttackFreq, e.EffectVal1, e.EffectVal2, 0)
		//if _, exist := b.CurDefender.ext.Buffs[constant.BuffIncrAttackFreq]; !exist {
		//	b.CurDefender.ext.Buffs[constant.BuffIncrAttackFreq] = &Buff{
		//		Type: constant.BuffIncrAttackFreq,
		//	}
		//}
		//
		////增加时长
		//b.CurDefender.ext.Buffs[constant.BuffIncrAttackFreq].RemainTime += e.EffectVal1
		//b.CurAttacker.ext.AttackTrans += float64(e.EffectVal2) / constant.BaseMultiple
		//添加buff
		b.Report.CreateBuff(BuffStatusAdd, constant.BuffIncrAttackFreq, e.EffectVal1, b.CurAttacker.figher.GetID(),
			e.EffectVal2, 0)

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
