package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//每次攻击后提升n%攻击力
// 效果值1:1000 10%
type Effect30003 struct {
	*EffectBase
}

func newEffect30003(Skill *Skill) *Effect30003 {
	return &Effect30003{
		normalEffect(Skill),
	}
}

func (e *Effect30003) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.AttackRatio += float64(e.EffectVal1) / constant.BaseMultiple
		return true
	}

	return false
}
