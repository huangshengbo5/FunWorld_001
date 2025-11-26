package battle

import (
	"dakunlun/app/util"
)

//n%几率释放一次奥义
// 效果值1:5000 50%
type Effect30001 struct {
	*EffectBase
}

func newEffect30001(Skill *Skill) *Effect30001 {
	return &Effect30001{
		normalEffect(Skill),
	}
}

func (e *Effect30001) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		//再次释放奥义
		b.CurAttacker.attackContext.retriggerZXC = true
		return true
	}

	return false
}
