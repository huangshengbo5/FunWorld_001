package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//n%+自身血量m%贯穿伤害
// 效果值1:1840 贯穿184%
//  效果值1:500 血量5%
type Effect10024 struct {
	*EffectBase
}

func newEffect10024(Skill *Skill) *Effect10024 {
	return &Effect10024{
		normalEffect(Skill),
	}
}

func (e *Effect10024) execute(b *BattleInfo) bool {
	//命中且生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		oldAttack := b.CurAttacker.ext.Attack
		//增加攻击力
		b.CurAttacker.ext.Attack += float64(b.CurAttacker.ext.HP * int64(e.EffectVal2) / constant.BaseMultiple)

		damages, isDead := int64(0), false
		if b.CurAttacker.attackContext.isHit {
			damages, isDead = b.baseAttack(float64(e.EffectVal1)/constant.BaseMultiple, constant.IsThrough)
		}

		//还原攻击力
		b.CurAttacker.ext.Attack = oldAttack
		//记录扣血结果
		b.Report.AddActionResult(b.CurAttacker.attackContext, b.CurAttacker.figher.GetID(), b.CurDefender.figher.GetID(), damages)
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
