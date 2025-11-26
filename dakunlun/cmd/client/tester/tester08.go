package tester

import (
	"dakunlun/app/constant"
	"dakunlun/app/service/battle"
)

func NewTester08(id uint32) (r *battle.FighterRuntime) {
	skill10081 := &battle.SkillInfo{
		SkillID:     10081,
		TriggerType: constant.TriggerDefault,
		Probability: 10000, //100%技能发动
		Effect: &battle.Effect10004{
			&battle.EffectBase{
				Target:            constant.TargetEnemy,
				EffectID:          10004,
				EffectProbability: 10000, //100%生效
				EffectVal1:        15000, //150%穿刺
			},
		},
	}

	skill10082 := &battle.SkillInfo{
		SkillID:     10082,
		TriggerType: constant.TriggerAfterAttack,
		Probability: 1500, //15%技能发动
		Effect: &battle.Effect10023{
			&battle.EffectBase{
				Target:            constant.TargetEnemy,
				EffectID:          10023,
				EffectProbability: 10000, //100%生效
				EffectVal1:        5000,  //50%
			},
		},
	}

	skill10083 := &battle.SkillInfo{
		SkillID:     10083,
		TriggerType: constant.TriggerZXC,
		Probability: 3000, //30%技能发动
		Effect: &battle.Effect10024{
			&battle.EffectBase{
				Target:            constant.TargetEnemy,
				EffectID:          10024,
				EffectProbability: 10000, //100%生效
				EffectVal1:        18400, //184%
				EffectVal2:        500,   //5%
			},
		},
	}

	r = &battle.FighterRuntime{
		ID:           id,
		CDTime:       0,
		MaxHP:        979875,
		HP:           979875,
		Attack:       70350,
		Defend:       20100, //防御力
		CriticalPlus: 2,     //暴击倍率
		Hit:          30000, //命中 影响角色攻击的命中能力
		Dodge:        30000, //闪避 影响角色闪避攻击的能力
		Critical:     30000, //暴击 影响角色的暴击几率
		Tenacity:     30000, //韧性 影响角色的被暴击几率
		Break:        30000, //破甲 无视对方一定防御能力
		Impregnable:  30000, //铁壁 抵消破甲的效果
		Defuse:       30000, //化解 影响角色最终减伤
		Buffs:        make(map[constant.BuffType]*battle.Buff),
		Skills: map[constant.TriggerType][]*battle.SkillInfo{
			constant.TriggerDefault:     []*battle.SkillInfo{skill10081},
			constant.TriggerZXC:         []*battle.SkillInfo{skill10083},
			constant.TriggerAfterAttack: []*battle.SkillInfo{skill10082},
		},
	}

	return
}
