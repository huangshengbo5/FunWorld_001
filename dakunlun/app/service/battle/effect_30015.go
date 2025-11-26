package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//生命值每损失n%，提升m%闪避值
// 效果值1:1000  10%
//  效果值2:1000  10%
type Effect30015 struct {
	*EffectBase
}

func newEffect30015(Skill *Skill) *Effect30015 {
	return &Effect30015{
		normalEffect(Skill),
	}
}

func (e *Effect30015) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.DodgeSkill = &DodgeSkill{
			Ratio: e.EffectVal1,
			Val:   float64(e.EffectVal2) / constant.BaseMultiple,
		}

		return true
	}

	return false
}
