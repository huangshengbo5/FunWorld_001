package tester

import (
	"dakunlun/app/constant"
	"dakunlun/app/service/battle"
)

func NewTester04(id uint32) (r *battle.FighterRuntime) {
	skill10041 := &battle.SkillInfo{
		SkillID:     10041,
		TriggerType: constant.TriggerDefault,
		Probability: 10000, //100%技能发动
		Effect: &battle.Effect10002{
			&battle.EffectBase{
				Target:            constant.TargetEnemy,
				EffectID:          10002,
				EffectProbability: 10000, //100%生效
				EffectVal1:        15000, //150%普通
			},
		},
	}

	skill10042 := &battle.SkillInfo{
		SkillID:     10042,
		TriggerType: constant.TriggerAfterAttack,
		Probability: 2600, //26%技能发动
		Effect: &battle.Effect10002{
			&battle.EffectBase{
				Target:            constant.TargetSelf,
				EffectID:          10002,
				EffectProbability: 10000, //100%生效
				EffectVal1:        5000,  //5秒
				EffectVal2:        2500,  //25%
			},
		},
	}

	skill10043 := &battle.SkillInfo{
		SkillID:     10043,
		TriggerType: constant.TriggerZXC,
		Probability: 3000, //30%技能发动
		Effect: &battle.Effect10002{
			&battle.EffectBase{
				Target:            constant.TargetEnemy,
				EffectID:          10002,
				EffectProbability: 10000, //100%生效
				EffectVal1:        13600, //136%普通伤害
				SubEffects: []battle.IEffect{
					&battle.Effect10007{
						&battle.EffectBase{
							Target:            constant.TargetEnemy,
							EffectID:          10007,
							EffectProbability: 5000, //50%生效
							EffectVal1:        5000, //5秒
							EffectVal2:        2000, //2%
							Continue:          true,
						},
					}, &battle.Effect10008{
						&battle.EffectBase{
							Target:            constant.TargetEnemy,
							EffectID:          10008,
							EffectProbability: 5000, //50%生效
							EffectVal1:        5000, //5秒
							EffectVal2:        2000, //2%
							Continue:          true,
						},
					}, &battle.Effect10009{
						&battle.EffectBase{
							Target:            constant.TargetEnemy,
							EffectID:          10009,
							EffectProbability: 5000, //50%生效
							EffectVal1:        5000, //5秒
							EffectVal2:        2000, //2%
							Continue:          true,
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
			constant.TriggerDefault:     []*battle.SkillInfo{skill10041},
			constant.TriggerZXC:         []*battle.SkillInfo{skill10043},
			constant.TriggerAfterAttack: []*battle.SkillInfo{skill10042},
		},
	}

	return
}
