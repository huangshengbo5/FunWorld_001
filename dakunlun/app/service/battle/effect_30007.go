package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//异常状态持续时间降低n%
// 效果值1:5000 50%
type Effect30007 struct {
	*EffectBase
}

func newEffect30007(Skill *Skill) *Effect30007 {
	return &Effect30007{
		normalEffect(Skill),
	}
}

func (e *Effect30007) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.DebuffRatio += float64(e.EffectVal1) / constant.BaseMultiple
	}

	return false
}
