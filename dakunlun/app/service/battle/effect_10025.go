package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//让敌人5秒内减伤15%
// 效果值1:5000 持续时间单位毫秒
//  效果值2:1500 减伤15%
type Effect10025 struct {
	*EffectBase
}

func newEffect10025(Skill *Skill) *Effect10025 {
	return &Effect10025{
		normalEffect(Skill),
	}
}

func (e *Effect10025) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		b.CurAttacker.ext.AddBuff(constant.BuffHpProtected, e.EffectVal1, e.EffectVal2, 0)

		//添加buff
		b.Report.CreateBuff(BuffStatusAdd, constant.BuffHpProtected, e.EffectVal1, b.CurAttacker.figher.GetID(),
			e.EffectVal2, 0)

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
