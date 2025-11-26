package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//永久攻击力提高n%
// 效果值1:5000 50%
type Effect30022 struct {
	*EffectBase
}

func newEffect30022(Skill *Skill) *Effect30022 {
	return &Effect30022{
		normalEffect(Skill),
	}
}

func (e *Effect30022) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.AttackRatio += float64(e.EffectVal1) / constant.BaseMultiple
		return true
	}

	return false
}
