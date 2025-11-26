package battle

import (
	"dakunlun/app/util"
)

//n%几率剩1hp
// 效果值1:5000 50%
type Effect30012 struct {
	*EffectBase
}

func newEffect30012(Skill *Skill) *Effect30012 {
	return &Effect30012{
		normalEffect(Skill),
	}
}

func (e *Effect30012) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		//再次释放奥义
		b.CurAttacker.ext.IncrHP(1)
		b.CurAttacker.attackContext.skillID = 22
		return true
	}

	return false
}
