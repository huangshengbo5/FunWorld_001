package battle

import (
	"dakunlun/app/constant"
	"dakunlun/app/util"
)

//对敌人造成一次30%的贯穿伤害
// 效果值1:3000 贯穿伤害系数
type Effect10004 struct {
	*EffectBase
}

func newEffect10004(Skill *Skill) *Effect10004 {
	return &Effect10004{
		normalEffect(Skill),
	}
}

func (e *Effect10004) execute(b *BattleInfo) bool {
	//命中且生效
	if util.JudgeProbability(e.EffectProbability) && e.checkHit(b.CurAttacker.attackContext.isHit) {
		//穿刺攻击
		damages, isDead := int64(0), false
		if b.CurAttacker.attackContext.isHit {
			damages, isDead = b.baseAttack(float64(e.EffectVal1)/constant.BaseMultiple, constant.IsThrough)
		}
		//记录扣血结果
		b.Report.AddActionResult(b.CurAttacker.attackContext, b.CurAttacker.figher.GetID(), b.CurDefender.figher.GetID(), damages)
		if isDead {
			return true
		}

		for _, sub := range e.SubEffects {
			sub.execute(b)
		}

		return true
	}

	return false
}
