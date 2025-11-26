package tester

import (
	"dakunlun/app/constant"
	"dakunlun/app/service/battle"
)

func NewTester03(id uint32) (r *battle.FighterRuntime) {
	skill10031 := &battle.SkillInfo{
		SkillID:     10031,
		TriggerType: constant.TriggerDefault,
		Probability: 10000, //100%技能发动
		Effect: &battle.Effect10002{
			&battle.EffectBase{
				Target:            constant.TargetEnemy,
				EffectID:          10002,
				EffectProbability: 10000, //100%生效
				EffectVal1:        13000, //130%普通
			},
		},
	}

	skill10032 := &battle.SkillInfo{
		SkillID:     10032,
		TriggerType: constant.TriggerCounterAttack,
		Probability: 2000, //20%技能发动
		Effect: &battle.Effect10002{
			&battle.EffectBase{
				Target:            constant.TargetEnemy,
				EffectID:          10002,
				EffectProbability: 10000, //100%生效
				EffectVal1:        14500, //145%普通
			},
		},
	}

	skill10033 := &battle.SkillInfo{
		SkillID:     10033,
		TriggerType: constant.TriggerZXC,
		Probability: 3000, //30%技能发动
		Effect: &battle.Effect10002{
			&battle.EffectBase{
				Target:            constant.TargetEnemy,
				EffectID:          10002,
				EffectProbability: 10000, //100%生效
				EffectVal1:        13300, //133%普通伤害
				SubEffects: []battle.IEffect{&battle.Effect10025{
					&battle.EffectBase{
						Target:            constant.TargetSelf,
						EffectID:          10025,
						EffectProbability: 10000, //100%生效
						EffectVal1:        5000,  //5秒
					},
				}},
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
			constant.TriggerDefault:       []*battle.SkillInfo{skill10031},
			constant.TriggerZXC:           []*battle.SkillInfo{skill10033},
			constant.TriggerCounterAttack: []*battle.SkillInfo{skill10032},
		},
	}

	return
}
