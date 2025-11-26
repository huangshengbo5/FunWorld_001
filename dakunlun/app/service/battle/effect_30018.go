package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//每秒恢复自身1%的最大生命值，持续m秒
// 效果值1:10000  10秒
type Effect30018 struct {
	*EffectBase
}

func newEffect30018(Skill *Skill) *Effect30018 {
	return &Effect30018{
		normalEffect(Skill),
	}
}

func (e *Effect30018) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.AddBuff(constant.BuffIncrHP, e.EffectVal1, 100, 0)
		b.Report.CreateBuff(BuffStatusAdd, constant.BuffIncrHP, e.EffectVal1, b.CurAttacker.figher.GetID(),
			100, 0)

		return true
	}

	return false
}
