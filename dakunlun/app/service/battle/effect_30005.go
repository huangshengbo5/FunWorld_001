package battle

import (
	"dakunlun/app/util"
)

//攻击力转换比率提高n%
// 效果值1:5000 50%
type Effect30005 struct {
	*EffectBase
}

func newEffect30005(Skill *Skill) *Effect30005 {
	return &Effect30005{
		normalEffect(Skill),
	}
}

func (e *Effect30005) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.AttackPlus += e.EffectVal1
		//重置攻击力
		b.CurAttacker.resetAttack()
		return true
	}

	return false
}
