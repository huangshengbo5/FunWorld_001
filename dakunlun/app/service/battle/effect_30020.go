package battle

import (
	"dakunlun/app/util"
)

//战斗力永久提高n%
// 效果值1:5000 50%
type Effect30020 struct {
	*EffectBase
}

func newEffect30020(Skill *Skill) *Effect30020 {
	return &Effect30020{
		normalEffect(Skill),
	}
}

func (e *Effect30020) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		oldFightingCapacity := b.CurAttacker.figher.GetFightingCapacity()
		b.CurAttacker.ext.FightingCapacityPlus += e.EffectVal1
		//计算增量
		incr := int64(b.CurAttacker.getFightingCapacity() - oldFightingCapacity)
		//重置血量/攻击/防御
		b.CurAttacker.ext.MaxHP += incr
		b.CurAttacker.ext.IncrHP(incr)
		b.CurAttacker.resetAttack()
		b.CurAttacker.resetDefend()
		return true
	}

	return false
}
