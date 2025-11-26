package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//消耗n%生命，增加m%攻击，持续10秒
// 效果值1:2000 20%
//  效果值2:5000 50%
type Effect30009 struct {
	*EffectBase
}

func newEffect30009(Skill *Skill) *Effect30009 {
	return &Effect30009{
		normalEffect(Skill),
	}
}

func (e *Effect30009) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		damages := b.CurAttacker.ext.DecrHP(b.CurAttacker.ext.MaxHP * int64(e.EffectVal1) / constant.BaseMultiple)
		//记录扣血结果
		b.Report.AddActionResult(b.CurAttacker.attackContext, b.CurAttacker.figher.GetID(), b.CurAttacker.figher.GetID(), damages)

		b.CurAttacker.ext.AddBuff(constant.BuffIncrAttack, 10000, e.EffectVal2, 0)
		b.Report.CreateBuff(BuffStatusAdd, constant.BuffIncrAttack, 10000, b.CurAttacker.figher.GetID(),
			e.EffectVal2, 0)
		return true
	}

	return false
}
