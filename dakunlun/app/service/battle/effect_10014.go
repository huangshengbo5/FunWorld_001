package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//解除自身受到的10007效果
type Effect10014 struct {
	*EffectBase
}

func newEffect10014(Skill *Skill) *Effect10014 {
	return &Effect10014{
		normalEffect(Skill),
	}
}

func (e *Effect10014) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		rtn := b.CurAttacker.ext.ClearBuff(constant.BuffDecrAttackFreq)
		if rtn {
			b.Report.BalanceBuff(BuffStatusClear, constant.BuffDecrAttackFreq, b.CurAttacker.ext.ID, 0, 0, 0,
				b.CurrentTime)
		}

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
