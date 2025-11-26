package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//攻击力提高n%
// 效果值1:5000 50%
type Effect30002 struct {
	*EffectBase
}

func newEffect30002(Skill *Skill) *Effect30002 {
	return &Effect30002{
		normalEffect(Skill),
	}
}

func (e *Effect30002) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.AttackOnce += float64(e.EffectVal1) / constant.BaseMultiple
		return true
	}

	return false
}
