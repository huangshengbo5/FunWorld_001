package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//每次被攻击后提升m%防御力
// 效果值1:1000 10%
type Effect30004 struct {
	*EffectBase
}

func newEffect30004(Skill *Skill) *Effect30004 {
	return &Effect30004{
		normalEffect(Skill),
	}
}

func (e *Effect30004) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.DefendRatio += float64(e.EffectVal1) / constant.BaseMultiple
		return true
	}

	return false
}
