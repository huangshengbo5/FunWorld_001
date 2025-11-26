package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//m秒内降低防御力n%
// 效果值1:5000 持续时间毫秒
//  效果值2:500  5%
type Effect10009 struct {
	*EffectBase
}

func newEffect10009(Skill *Skill) *Effect10009 {
	return &Effect10009{
		normalEffect(Skill),
	}
}

func (e *Effect10009) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		if _, exist := b.CurDefender.ext.Buffs[constant.BuffImmune10009]; !exist {
			b.CurDefender.ext.AddBuff(constant.BuffDecrDefend, e.EffectVal1, e.EffectVal2, 0)
			//添加buff
			b.Report.CreateBuff(BuffStatusAdd, constant.BuffDecrDefend, e.EffectVal1, b.CurDefender.figher.GetID(),
				e.EffectVal2, 0)
		}

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
