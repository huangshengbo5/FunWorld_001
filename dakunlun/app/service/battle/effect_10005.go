package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//让敌人陷入眩晕1秒
// 效果值1:1000 眩晕时间单位毫秒
type Effect10005 struct {
	*EffectBase
}

func newEffect10005(Skill *Skill) *Effect10005 {
	return &Effect10005{
		normalEffect(Skill),
	}
}

func (e *Effect10005) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		b.CurDefender.ext.AddBuff(constant.BuffFreeze, e.EffectVal1, 0, 0)
		//if _, exist := b.CurDefender.ext.Buffs[constant.BuffFreeze]; !exist {
		//	b.CurDefender.ext.Buffs[constant.BuffFreeze] = &Buff{
		//		Type: constant.BuffFreeze,
		//	}
		//}
		//
		////增加时长
		//b.CurDefender.ext.Buffs[constant.BuffFreeze].RemainTime += e.EffectVal1

		//添加buff
		b.Report.CreateBuff(BuffStatusAdd, constant.BuffFreeze, e.EffectVal1, b.CurDefender.figher.GetID(),
			e.EffectVal2, 0)

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
