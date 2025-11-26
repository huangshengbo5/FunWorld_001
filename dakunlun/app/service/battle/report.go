package battle

import (
	"dakunlun/app/constant"
	"fmt"
)

const (
	CmdActionStart = iota
	CmdResult
	CmdBuff
	CmdActionEnd
)

const (
	ResultDodge = iota
	ResultHit
)

const (
	BuffStatusBalance = iota
	BuffStatusClear
	BuffStatusAdd
)

type ReportPlayer struct {
	ID               uint32 `json:"id"`
	Name             string `json:"name"`
	Avatar           uint16 `json:"avatar"`
	HeroID           uint32 `json:"heroID"`
	Level            uint16 `json:"level"`
	HP               int64  `json:"hp"`
	MaxHP            int64  `json:"maxHP"` //总血量
	FType            uint8  `json:"fType"` //战斗类型
	FightingCapacity uint64 `json:"fightingCapacity"`
}

// 战报
type BattleReport struct {
	Type       uint8           `json:"type"`       //战斗类型
	Background uint16          `json:"background"` //背景
	Attacker   ReportPlayer    `json:"attacker"`   //攻击方信息
	Defender   ReportPlayer    `json:"defender"`   //防御方信息
	MaxTime    int             `json:"maxTime"`    //限时
	Actions    [][]interface{} `json:"actions"`    //指令
	Result     *Result         `json:"result"`     //结果
}
type Result struct {
	Result     uint8 `json:"result"`     //0攻击方胜利 1防守方胜利 2平局
	AttackerHP int64 `json:"attackerHP"` //攻击方剩余血量
	DefenderHP int64 `json:"defenderHP"` //防守方剩余血量
	CostTime   int   `json:"costTime"`   //消耗时间
}

func (report *BattleReport) AddActionStart(ctx *AttackContext, curTime int, attackerID uint32, defenderID uint32,
	skillType constant.SkillType, skillID uint32) {
	isHit := false
	isCritical := false
	if ctx != nil {
		isHit = ctx.isHit
		isCritical = ctx.isCritical
	}

	report.Actions = append(report.Actions, []interface{}{
		CmdActionStart,
		curTime,
		attackerID, //行动方
		defenderID, //目标方
		skillType,  //技能类型
		skillID,    //技能ID
		isHit,      //是否命中 bool
		isCritical, //是否暴击 bool
	})
}

func (report *BattleReport) addDodge(ctx *AttackContext, attackerID uint32, defenderID uint32) {
	report.Actions = append(report.Actions, []interface{}{
		CmdResult,
		!ctx.isHit,
		attackerID,
		defenderID,
		0, //伤害
	})

	//$this->actions[] = array(self::CMD_RESULT, array(self::getHeroKey($userHero)), array(2), array(0), array(0), array(0));
}

func (report *BattleReport) AddActionEnd() {
	report.Actions = append(report.Actions, []interface{}{
		CmdActionEnd,
	})
}

func (report *BattleReport) AddActionResult(ctx *AttackContext, attackerID uint32, defenderID uint32, damages int64) {
	datas := []interface{}{
		CmdResult,
		ctx.isHit,
		attackerID,
		defenderID,
		damages, //伤害
	}

	if ctx.skillID > 0 {
		datas = append(datas, ctx.skillID)
		ctx.skillID = 0
	}

	report.Actions = append(report.Actions, datas)
}

// 添加buff
func (report *BattleReport) CreateBuff(buffStatus int, buffID constant.BuffType, remainTime int, targetID uint32,
	val1, val2 int) {
	report.Actions = append(report.Actions, []interface{}{
		CmdBuff,
		buffStatus,
		targetID,
		buffID,
		remainTime,
		val1,
		val2,
	})
}

// 结算buff
func (report *BattleReport) BalanceBuff(buffStatus int, buffID constant.BuffType, targetID uint32, remainTime,
	val1, val2 int, curTime int) {
	report.Actions = append(report.Actions, []interface{}{
		CmdBuff,
		buffStatus,
		targetID,
		buffID,
		remainTime,
		val1,
		val2,
		curTime,
	})
}

// 设置战报结果
func (report *BattleReport) SetResult(battleInfo *BattleInfo) {
	report.Result = &Result{
		Result:     battleInfo.Result,
		AttackerHP: battleInfo.AtkExt.HP,
		DefenderHP: battleInfo.DefExt.HP,
		CostTime:   battleInfo.CurrentTime,
	}
}

func (report *BattleReport) Show() (r string) {
	r += fmt.Sprintf("\n攻击方:%v 血量:%v vs 防御方:%v 血量:%v\n", report.Attacker.ID, report.Attacker.HP, report.Defender.ID,
		report.Defender.HP)
	for _, v := range report.Actions {
		switch v[0] {
		case CmdActionStart:
			r += fmt.Sprintf("\n【行动开始】 时间点:%v 攻击方:%v 防御方:%v 技能类型:%v 技能ID:%v 是否命中:%v 是否暴击:%v \n", v[1:]...)
		case CmdActionEnd:
			r += fmt.Sprintf("【行动结束】 \n")
		case CmdResult:
			r += fmt.Sprintf("结果: 是否命中:%v 攻击方:%v 防御方:%v 伤害:%v \n", v[1:]...)
		case CmdBuff:
			var newSlice []interface{}
			newSlice = append(newSlice, v[2])
			newSlice = append(newSlice, report.buffTrans(v[3]))
			newSlice = append(newSlice, v[4:]...)

			switch v[1] {
			case BuffStatusAdd:
				r += fmt.Sprintf("添加buff: 目标:%v 类型:%v 剩余时间:%v 参数1:%v 参数2:%v \n", newSlice...)
			case BuffStatusBalance:
				r += fmt.Sprintf("\n【结算buff】: 目标:%v 类型:%v 剩余时间:%v 参数1:%v 参数2:%v 当前时间:%v\n", newSlice...)
			case BuffStatusClear:
				r += fmt.Sprintf("\n【消除buff】: 目标:%v 类型:%v 剩余时间:%v 参数1:%v 参数2:%v 当前时间:%v\n", newSlice...)
			}

		}
	}
	r += fmt.Sprintf("\n攻击方剩余血量:%v vs 防御方剩余血量:%v\n", report.Result.AttackerHP, report.Result.DefenderHP)
	return
}

func (report *BattleReport) buffTrans(v interface{}) (r interface{}) {
	switch v {
	case constant.BuffDecrHP:
		r = "中毒"
	case constant.BuffFreeze:
		r = "眩晕"
	case constant.BuffDecrAttack:
		r = "降低攻击"
	case constant.BuffDecrAttackFreq:
		r = "降低攻速"
	case constant.BuffDecrDefend:
		r = "降低防御"
	case constant.BuffIncrHP:
		r = "回HP"
	case constant.BuffIncrAttack:
		r = "增加攻击"
	case constant.BuffIncrAttackFreq:
		r = "增加攻速"
	case constant.BuffHpProtected:
		r = "减伤"
	case constant.BuffImmune10007:
		r = "免疫10007"
	case constant.BuffImmune10008:
		r = "免疫10008"
	case constant.BuffImmune10009:
		r = "免疫10009"
	}

	return
}
