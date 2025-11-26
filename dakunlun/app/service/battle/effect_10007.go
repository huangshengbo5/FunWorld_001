package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//m秒内攻击攻击击频率降低n%
// 效果值1:5000 持续时间毫秒
//  效果值2:500  5%
type Effect10007 struct {
	*EffectBase
}

func newEffect10007(Skill *Skill) *Effect10007 {
	return &Effect10007{
		normalEffect(Skill),
	}
}

func (e *Effect10007) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		if _, exist := b.CurDefender.ext.Buffs[constant.BuffImmune10007]; !exist {
			b.CurDefender.ext.AddBuff(constant.BuffDecrAttackFreq, e.EffectVal1, e.EffectVal2, 0)
			//b.CurDefender.ext.AttackFreqRatio -= float64(e.EffectVal2) / constant.BaseMultiple
			//添加buff
			b.Report.CreateBuff(BuffStatusAdd, constant.BuffDecrAttackFreq, e.EffectVal1, b.CurDefender.figher.GetID(),
				e.EffectVal2, 0)
		}

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
