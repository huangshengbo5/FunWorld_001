package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//下3次受到的伤害降低为{n}%
// 效果值1:5000 50%
type Effect30013 struct {
	*EffectBase
}

func newEffect30013(Skill *Skill) *Effect30013 {
	return &Effect30013{
		normalEffect(Skill),
	}
}

func (e *Effect30013) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.HpLimitedSkill = &HpNumSkill{
			RemainNum: 3,
			Val:       float64(e.EffectVal1) / constant.BaseMultiple,
		}

		return true
	}

	return false
}
