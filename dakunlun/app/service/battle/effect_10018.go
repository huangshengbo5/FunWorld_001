package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//m秒内免疫10007效果
// 效果值1 1000  单位毫秒
type Effect10018 struct {
	*EffectBase
}

func newEffect10018(Skill *Skill) *Effect10018 {
	return &Effect10018{
		normalEffect(Skill),
	}
}

func (e *Effect10018) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		b.CurAttacker.ext.AddBuff(constant.BuffImmune10008, e.EffectVal1, 0, 0)
		b.Report.CreateBuff(BuffStatusAdd, constant.BuffImmune10008, e.EffectVal1, b.CurDefender.figher.GetID(),
			0, 0)

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
