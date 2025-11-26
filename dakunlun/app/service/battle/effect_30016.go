package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//死亡恢复血量n%
// 效果值1:1000  10%
type Effect30016 struct {
	*EffectBase
}

func newEffect30016(Skill *Skill) *Effect30016 {
	return &Effect30016{
		normalEffect(Skill),
	}
}

func (e *Effect30016) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		//恢复血量百分比
		damages := b.CurAttacker.ext.IncrHP(b.CurAttacker.ext.MaxHP * int64(e.EffectVal1) / constant.BaseMultiple)
		//记录扣血结果
		b.Report.AddActionResult(b.CurAttacker.attackContext, b.CurAttacker.figher.GetID(),
			b.CurAttacker.figher.GetID(), -damages)

		return true
	}

	return false
}
