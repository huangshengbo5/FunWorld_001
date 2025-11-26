package service

import (
	"dakunlun/app/constant"
	datadao "dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/service/battle"
	"dakunlun/app/service/data"
)

type battleService struct {
}

var BattleService = new(battleService)

const (
	ResultAttackerWin = iota
	ResultDefenderWin
	ResultDraw
)

// TODO 战斗
func (srv *battleService) Fight(battleInfo *battle.BattleInfo) (err error) {

	battleInfo.Init()

	var (
		costTime      int  //标识
		continueRound bool //回合结束标识
	)

	for {

		continueRound = true
		//行动前 主要是buff结算
		battleInfo.PreDo()

		for continueRound {
			//行动
			continueRound = battleInfo.Do()
			//判断血量
			if battleInfo.DefExt.HP <= 0 {
				battleInfo.Result = ResultAttackerWin
				goto BATTLEOVER
			}
			if battleInfo.AtkExt.HP <= 0 {
				battleInfo.Result = ResultDefenderWin
				goto BATTLEOVER
			}

			battleInfo.CheckFreeze()
		}

		//耗时
		costTime = battleInfo.AfterDo()
		//记录时间变化
		battleInfo.CurrentTime += costTime

		//判断时长
		if battleInfo.CurrentTime >= battleInfo.MaxTime {
			battleInfo.Result = ResultDefenderWin
			goto BATTLEOVER
		}
	}

BATTLEOVER:
	{
		battleInfo.Report.SetResult(battleInfo)
	}

	return
}

func (srv *battleService) NewPlayer(userEntity *entity.UserEntity, isPvp bool) (iFighter constant.IFighter,
	runtime *battle.FighterRuntime, err error) {
	var userHeroEntitys []*entity.UserHeroEntity
	userHeroEntitys, err = UserHeroService.GetHerosByUid(userEntity.ID)

	//总战力
	var totalFightingCapacity = userEntity.Attr.FightingCapacityEquipPlus + userEntity.CastEffect.FightingCapacityPlus
	// 参战英雄
	var fightHero *entity.UserHeroEntity
	for _, userHeroEntity := range userHeroEntitys {
		// 科技
		if userHeroEntity.IsMainHero() {
			totalFightingCapacity += userHeroEntity.GetFightingCapacity() + userEntity.TechEffect.MainHeroFightingCapacityPlus
		} else {
			totalFightingCapacity += userHeroEntity.GetFightingCapacity() + userEntity.TechEffect.SubHeroFightingCapacityPlus
		}

		if userEntity.SubHeroID > 0 {
			if userEntity.SubHeroID == userHeroEntity.ID {
				fightHero = userHeroEntity
			}
		} else {
			if userEntity.MainHeroID == userHeroEntity.ID {
				fightHero = userHeroEntity
			}
		}
	}

	iFighter = battle.NewBaseFighter(
		nil,
		userEntity.ID,
		userEntity.Avatar,
		userEntity.GetName(true),
		fightHero.HeroID,
		constant.FTypePlayer,
		totalFightingCapacity,
		int(fightHero.AttackFreq),
		fightHero.Sex,
		fightHero.Race,
		int(fightHero.GetAttackTrans())+int(userEntity.TechEffect.AttackTrans),
		int(fightHero.GetDefendTrans())+int(userEntity.TechEffect.DefendTrans),
		userEntity.Level,
		fightHero.Skills)

	var skillUpgradeDatas []*entity.SkillLevelData
	skillMap := make(map[constant.TriggerType][]*battle.SkillInfo, 4) //技能
	for _, skill := range fightHero.Skills {
		skillUpgradeDatas, err = datadao.SkillLevelDao.FetchSkillIDAndLevel(skill.ID, skill.Level)
		if err != nil {
			return
		}
		skillMap[skillUpgradeDatas[0].TriggerType] = append(skillMap[skillUpgradeDatas[0].TriggerType], battle.HeroSkillToBattleSkill(skillUpgradeDatas))
	}
	equipSkillMap, err := HeroEquipService.GetEquipSkillMap()
	if err != nil {
		return
	}
	for _, equip := range userEntity.Equips {
		if equip.ID > 0 && equip.SkillID > 0 {
			skillMap[equipSkillMap[equip.SkillID].TriggerType] = append(skillMap[equipSkillMap[equip.SkillID].TriggerType], battle.EquipSkillToBattleSkill(equipSkillMap[equip.SkillID]))
		}
	}

	//血量乘以血量转化率
	hp := totalFightingCapacity * uint64(fightHero.GetHpTrans()) / constant.BaseMultiple

	runtime = &battle.FighterRuntime{
		ID:           iFighter.GetID(),
		MaxHP:        int64(hp),
		HP:           int64(hp),
		CriticalPlus: 2,                                    //暴击倍率
		Hit:          float64(userEntity.GetHit()),         //命中 影响角色攻击的命中能力
		Dodge:        float64(userEntity.GetDodge()),       //闪避 影响角色闪避攻击的能力
		Critical:     float64(userEntity.GetCritical()),    //暴击 影响角色的暴击几率
		Tenacity:     float64(userEntity.GetTenacity()),    //韧性 影响角色的被暴击几率
		Break:        float64(userEntity.GetBreak()),       //破甲 无视对方一定防御能力
		Impregnable:  float64(userEntity.GetImpregnable()), //铁壁 抵消破甲的效果
		Defuse:       int(userEntity.GetDefuse()),          //化解 影响角色最终减伤
		Buffs:        make(map[constant.BuffType]*battle.Buff),
		Skills:       skillMap,
	}

	return
}

func (srv *battleService) NewNpc(id uint32) (iFighter constant.IFighter, runtime *battle.FighterRuntime, err error) {
	var npcEntity *entity.NpcData
	npcEntity, err = data.NpcService.GetNpcByID(id)

	if err != nil {
		return
	}

	iFighter = battle.NewBaseFighter(
		nil, //TODO
		npcEntity.ID,
		npcEntity.AvatarID,
		npcEntity.Name,
		npcEntity.ID,
		constant.FTypeNPC,
		npcEntity.FightingCapacity,
		int(npcEntity.AttackFreq),
		npcEntity.Sex,
		npcEntity.Race,
		int(npcEntity.AttackPlus),
		int(npcEntity.DefendPlus),
		npcEntity.Level,
		entity.Skills{
			1: {ID: npcEntity.Skill1ID, Level: 1},
			2: {ID: npcEntity.Skill2ID, Level: 1},
			3: {ID: npcEntity.Skill3ID, Level: 1},
			4: {ID: npcEntity.Skill4ID, Level: 1},
			5: {ID: npcEntity.Skill5ID, Level: 1},
			6: {ID: npcEntity.Skill6ID, Level: 1},
		})

	var skillUpgradeDatas []*entity.SkillLevelData
	var equipSkillData *entity.EquipSkillData
	skillMap := make(map[constant.TriggerType][]*battle.SkillInfo, 4) //技能
	for _, skill := range iFighter.GetSkills() {
		if skill.ID == 0 {
			continue
		}
		if skill.ID >= entity.SkilLIDStart {
			skillUpgradeDatas, err = datadao.SkillLevelDao.FetchSkillIDAndLevel(skill.ID, skill.Level)
			if err != nil {
				return
			}
			skillMap[skillUpgradeDatas[0].TriggerType] = append(skillMap[skillUpgradeDatas[0].TriggerType], battle.HeroSkillToBattleSkill(skillUpgradeDatas))
		} else {
			equipSkillData, err = datadao.EquipSkillDao.FetchById(skill.ID)
			if err != nil {
				return
			}
			skillMap[equipSkillData.TriggerType] = append(skillMap[equipSkillData.TriggerType], battle.EquipSkillToBattleSkill(equipSkillData))
		}

	}

	runtime = &battle.FighterRuntime{
		ID:           iFighter.GetID(),
		MaxHP:        int64(iFighter.GetFightingCapacity()),
		HP:           int64(iFighter.GetFightingCapacity()),
		CriticalPlus: 2,                              //暴击倍率
		Hit:          float64(npcEntity.Hit),         //命中 影响角色攻击的命中能力
		Dodge:        float64(npcEntity.Dodge),       //闪避 影响角色闪避攻击的能力
		Critical:     float64(npcEntity.Critical),    //暴击 影响角色的暴击几率
		Tenacity:     float64(npcEntity.Tenacity),    //韧性 影响角色的被暴击几率
		Break:        float64(npcEntity.Break),       //破甲 无视对方一定防御能力
		Impregnable:  float64(npcEntity.Impregnable), //铁壁 抵消破甲的效果
		Defuse:       int(npcEntity.Defuse),          //化解 影响角色最终减伤
		Buffs:        make(map[constant.BuffType]*battle.Buff),
		Skills:       skillMap,
	}

	return
}
