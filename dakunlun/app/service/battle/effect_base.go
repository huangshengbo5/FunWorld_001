package battle

import (
	"dakunlun/app/constant"
	"fmt"
)

type IEffect interface {
	execute(b *BattleInfo) bool
}

type EffectBase struct {
	Target            constant.Target //目标
	EffectID          uint32          //效果ID
	EffectProbability int             //效果几率
	EffectVal1        int             //效果值1
	EffectVal2        int             //效果值2
	Continue          bool            //是否继续
	MustHit           bool            //是否必须命中
	SubEffects        []IEffect       //附加能效果
}

func makeEffect(Skill *Skill) IEffect {
	switch Skill.EffectID {
	case 10001:
		return newEffect10001(Skill)
	case 10002:
		return newEffect10002(Skill)
	case 10003:
		return newEffect10003(Skill)
	case 10004:
		return newEffect10004(Skill)
	case 10005:
		return newEffect10005(Skill)
	case 10006:
		return newEffect10006(Skill)
	case 10007:
		return newEffect10007(Skill)
	case 10008:
		return newEffect10008(Skill)
	case 10009:
		return newEffect10009(Skill)
	case 10010:
		return newEffect10010(Skill)
	case 10012:
		return newEffect10012(Skill)
	case 10014:
		return newEffect10014(Skill)
	case 10015:
		return newEffect10015(Skill)
	case 10016:
		return newEffect10016(Skill)
	case 10017:
		return newEffect10017(Skill)
	case 10018:
		return newEffect10018(Skill)
	case 10019:
		return newEffect10019(Skill)
	//case 10020:
	//	return newEffect10020(Skill)
	//case 10021:
	//	return newEffect10021(Skill)
	//case 10022:
	//	return newEffect10022(Skill)
	case 10023:
		return newEffect10023(Skill)
	case 10024:
		return newEffect10024(Skill)
	case 10025:
		return newEffect10025(Skill)
	case 30001:
		return newEffect30001(Skill)
	case 30002:
		return newEffect30002(Skill)
	case 30003:
		return newEffect30003(Skill)
	case 30004:
		return newEffect30004(Skill)
	case 30005:
		return newEffect30005(Skill)
	case 30006:
		return newEffect30006(Skill)
	case 30007:
		return newEffect30007(Skill)
	case 30008:
		return newEffect30008(Skill)
	case 30009:
		return newEffect30009(Skill)
	case 30012:
		return newEffect30012(Skill)
	case 30013:
		return newEffect30013(Skill)
	case 30014:
		return newEffect30014(Skill)
	case 30015:
		return newEffect30015(Skill)
	case 30016:
		return newEffect30016(Skill)
	case 30017:
		return newEffect30017(Skill)
	case 30018:
		return newEffect30018(Skill)
	case 30019:
		return newEffect30019(Skill)
	case 30020:
		return newEffect30020(Skill)
	case 30021:
		return newEffect30021(Skill)
	case 30022:
		return newEffect30022(Skill)
	case 30023:
		return newEffect30023(Skill)
	}

	panic(fmt.Sprintf("effect id [%v] is not definded", Skill.EffectID))
}

func normalEffect(Skill *Skill) *EffectBase {
	e := &EffectBase{
		Target:            Skill.Target,
		EffectID:          Skill.EffectID,
		EffectProbability: Skill.EffectProbability,
		EffectVal1:        Skill.EffectVal1,
		EffectVal2:        Skill.EffectVal2,
		Continue:          Skill.Continue,
		MustHit:           Skill.MustHit,
		SubEffects:        nil,
	}
	for _, subSkill := range Skill.SubSkills {
		e.SubEffects = append(e.SubEffects, makeEffect(subSkill))
	}

	return e
}

func (e *EffectBase) checkHit(isHit bool) bool {
	if !e.MustHit {
		return true
	}

	return isHit
}
