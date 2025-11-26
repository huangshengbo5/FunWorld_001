package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//n秒内防御提升m%
// 效果值1:10000  10秒
//  效果值2:1000  10%
type Effect30017 struct {
	*EffectBase
}

func newEffect30017(Skill *Skill) *Effect30017 {
	return &Effect30017{
		normalEffect(Skill),
	}
}

func (e *Effect30017) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.AddBuff(constant.BuffHpProtected, e.EffectVal1, e.EffectVal2, 0)
		b.Report.CreateBuff(BuffStatusAdd, constant.BuffHpProtected, e.EffectVal1, b.CurAttacker.figher.GetID(),
			e.EffectVal2, 0)

		return true
	}

	return false
}
