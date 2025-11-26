package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//生命值每损失n%，提升m%防御值
// 效果值1:1000  10%
//  效果值2:1000  10%
type Effect30014 struct {
	*EffectBase
}

func newEffect30014(Skill *Skill) *Effect30014 {
	return &Effect30014{
		normalEffect(Skill),
	}
}

func (e *Effect30014) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.DefendSkill = &DefendSkill{
			Ratio: e.EffectVal1,
			Val:   float64(e.EffectVal2) / constant.BaseMultiple,
		}

		return true
	}

	return false
}
