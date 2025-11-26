package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//解除自身受到的10008效果
type Effect10015 struct {
	*EffectBase
}

func newEffect10015(Skill *Skill) *Effect10015 {
	return &Effect10015{
		normalEffect(Skill),
	}
}

func (e *Effect10015) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		rtn := b.CurAttacker.ext.ClearBuff(constant.BuffDecrAttack)
		if rtn {
			b.Report.BalanceBuff(BuffStatusClear, constant.BuffDecrAttack, b.CurAttacker.ext.ID, 0, 0, 0,
				b.CurrentTime)
		}

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
