package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//m秒内攻击攻击降低n%
// 效果值1:5000 持续时间毫秒
//  效果值2:500  5%
type Effect10008 struct {
	*EffectBase
}

func newEffect10008(Skill *Skill) *Effect10008 {
	return &Effect10008{
		normalEffect(Skill),
	}
}

func (e *Effect10008) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		if _, exist := b.CurDefender.ext.Buffs[constant.BuffImmune10008]; !exist {
			b.CurDefender.ext.AddBuff(constant.BuffDecrAttack, e.EffectVal1, e.EffectVal2, 0)
			//添加buff
			b.Report.CreateBuff(BuffStatusAdd, constant.BuffDecrAttack, e.EffectVal1, b.CurDefender.figher.GetID(),
				e.EffectVal2, 0)
		}

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
