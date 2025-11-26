package battle

import (
	"dakunlun/app/util"
)

//防御力转换比率提高n%
// 效果值1:5000 50%
type Effect30006 struct {
	*EffectBase
}

func newEffect30006(Skill *Skill) *Effect30006 {
	return &Effect30006{
		normalEffect(Skill),
	}
}

func (e *Effect30006) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.DefendPlus += e.EffectVal1
		//重置防御力
		b.CurAttacker.resetDefend()
		return true
	}

	return false
}
