package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//血量每降低m%攻击力n%
// 效果值1:2400 24%
//  效果值2:500  5%
type Effect10012 struct {
	*EffectBase
}

func newEffect10012(Skill *Skill) *Effect10012 {
	return &Effect10012{
		normalEffect(Skill),
	}
}

func (e *Effect10012) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		mul := (b.CurAttacker.ext.TotalDecrHP() * constant.BaseMultiple / b.CurAttacker.ext.MaxHP) / int64(e.EffectVal1)

		b.CurAttacker.ext.AttackOnce += float64(mul) * (float64(e.EffectVal2) / constant.BaseMultiple)

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
