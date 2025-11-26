package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//解除自身受到的10009效果
type Effect10016 struct {
	*EffectBase
}

func newEffect10016(Skill *Skill) *Effect10016 {
	return &Effect10016{
		normalEffect(Skill),
	}
}

func (e *Effect10016) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		rtn := b.CurAttacker.ext.ClearBuff(constant.BuffDecrDefend)
		if rtn {
			b.Report.BalanceBuff(BuffStatusClear, constant.BuffDecrDefend, b.CurAttacker.ext.ID, 0, 0, 0,
				b.CurrentTime)
		}

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
