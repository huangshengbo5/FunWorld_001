package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//攻击力在n-m%之间波动
// 效果值1:2000 20%
//  效果值2:5000 50%
type Effect30008 struct {
	*EffectBase
}

func newEffect30008(Skill *Skill) *Effect30008 {
	return &Effect30008{
		normalEffect(Skill),
	}
}

func (e *Effect30008) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.AttackOnce += float64(util.RandInt(0, e.EffectVal1+e.EffectVal2)-e.EffectVal1) / constant.BaseMultiple
		return true
	}

	return false
}
