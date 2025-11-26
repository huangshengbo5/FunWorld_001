package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//消耗当前生命值n%，临时攻击提升m%，行动结束后归0
// 效果值1:1000 10%
//  效果值2:3000 30%
type Effect30023 struct {
	*EffectBase
}

func newEffect30023(Skill *Skill) *Effect30023 {
	return &Effect30023{
		normalEffect(Skill),
	}
}

func (e *Effect30023) execute(b *BattleInfo) bool {
	//生效
	if util.JudgeProbability(e.EffectProbability) {
		damages := b.CurAttacker.ext.DecrHP((b.CurAttacker.ext.HP * int64(e.EffectVal1) / constant.BaseMultiple))
		//记录扣血结果
		b.Report.AddActionResult(b.CurAttacker.attackContext, b.CurAttacker.figher.GetID(), b.CurAttacker.figher.GetID(), damages)
		//增加攻击力加成
		b.CurAttacker.ext.AttackOnce += float64(e.EffectVal2)
		return true
	}

	return false
}
