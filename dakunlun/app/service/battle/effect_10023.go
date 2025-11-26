package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//吸血攻击m%
// 效果值1:500 m%
type Effect10023 struct {
	*EffectBase
}

func newEffect10023(Skill *Skill) *Effect10023 {
	return &Effect10023{
		normalEffect(Skill),
	}
}

func (e *Effect10023) execute(b *BattleInfo) bool {
	//命中且生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		damages := b.CurAttacker.ext.IncrHP(b.CurAttacker.attackContext.damages * int64(e.EffectVal1) / constant.
			BaseMultiple)
		//记录扣血结果
		b.Report.AddActionResult(b.CurAttacker.attackContext, b.CurAttacker.figher.GetID(), b.CurAttacker.figher.GetID(), -damages)
		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
