package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//{n}%的概率，只受到{m}%的伤害
// 效果值1:1000  10%概率
//  效果值2:1000  50%伤害比例
type Effect30019 struct {
	*EffectBase
}

func newEffect30019(Skill *Skill) *Effect30019 {
	return &Effect30019{
		normalEffect(Skill),
	}
}

func (e *Effect30019) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		b.CurAttacker.ext.HpProtectedSkill = &HpSkill{
			Ratio: e.EffectVal1,
			Val:   float64(e.EffectVal2) / constant.BaseMultiple,
		}

		return true
	}

	return false
}
