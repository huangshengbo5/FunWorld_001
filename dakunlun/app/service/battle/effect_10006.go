package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//N秒内每秒损失自身当前生命值的m%
// 效果值1:5000 5秒
//  效果值1:1000 10%
type Effect10006 struct {
	*EffectBase
}

func newEffect10006(Skill *Skill) *Effect10006 {
	return &Effect10006{
		normalEffect(Skill),
	}
}

func (e *Effect10006) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		b.CurDefender.ext.AddBuff(constant.BuffDecrHP, int(e.EffectVal1), e.EffectVal2, 0)
		b.Report.CreateBuff(BuffStatusAdd, constant.BuffDecrHP, int(e.EffectVal1), b.CurDefender.figher.GetID(),
			e.EffectVal2, 0)

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
