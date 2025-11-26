package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//在接下来的{m}秒伤害提升{n}%
// 效果值1:10000  10秒
//  效果值2:1000  10%
type Effect30021 struct {
	*EffectBase
}

func newEffect30021(Skill *Skill) *Effect30021 {
	return &Effect30021{
		normalEffect(Skill),
	}
}

func (e *Effect30021) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.AddBuff(constant.BuffIncrAttack, e.EffectVal1, e.EffectVal2, 0)
		b.Report.CreateBuff(BuffStatusAdd, constant.BuffIncrAttack, e.EffectVal1, b.CurAttacker.figher.GetID(),
			e.EffectVal2, 0)

		return true
	}

	return false
}
