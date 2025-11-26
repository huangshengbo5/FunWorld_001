package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//按百分比提高基础伤害
// 效果值1: 10200   102%
type Effect10002 struct {
	*EffectBase
}

func newEffect10002(Skill *Skill) *Effect10002 {
	return &Effect10002{
		normalEffect(Skill),
	}
}

func (e *Effect10002) execute(b *BattleInfo) bool {
	//命中且生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		//普通攻击
		damages, isDead := int64(0), false
		if b.CurAttacker.attackContext.isHit {
			damages, isDead = b.baseAttack(float64(e.EffectVal1)/constant.BaseMultiple, constant.NoThrough)
		}

		//记录扣血结果
		b.Report.AddActionResult(b.CurAttacker.attackContext, b.CurAttacker.figher.GetID(),
			b.CurDefender.figher.GetID(), damages)

		if isDead {
			return true
		}

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
