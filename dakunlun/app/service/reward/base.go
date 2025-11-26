package reward

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"fmt"
	"math"

	"github.com/spf13/cast"
)

const (
	// 奖励类型 金币
	RewardTypeGold = iota + 1
	// 奖励类型 钻石
	RewardTypeDiamond
	// 奖励类型 魂石
	RewardTypeSoulCrystal
	// 奖励类型 宝物精华
	RewardTypeTreasureAnima
	// 奖励类型 狂妄之章
	RewardTypeSuperbiaStone
	// 奖励类型 憎恶之章
	RewardTypeInvidiaStone
	// 奖励类型 懈怠之章
	RewardTypeAcediaStone
	// 奖励类型 吞噬之章
	RewardTypeGulaStone
	// 奖励类型 无厌之章
	RewardTypeAvaritiaStone
	// 奖励类型 销魂之章
	RewardTypeLuxuriaStone
	// 奖励类型 肆虐之章
	RewardTypeIraStone
	// 奖励类型 本源之力
	RewardTypeBenYuan = 14
	// 奖励类型 潜能之力
	RewardTypeQianNeng = 15
	// 奖励类型 装备
	RewardTypeEquip = 16
	// 奖励类型 元素结晶
	RewardTypeElement = 17
	// 奖励类型 秘传书
	RewardTypeBook = 18
)

const (
	DefaultRewardFmt = "%d_%d_%d_%d"
)

func MakeRewardFmt( /*类型*/ mainType uint16 /*子类型*/, subType uint32 /*奖励值*/, value uint64, /*公式ID*/
	formulaID uint16) string {
	return fmt.Sprintf(DefaultRewardFmt, mainType, subType, value, formulaID)
}

type BaseReward struct {
	Ctx       *constant.RewardContext
	MainType  uint16
	SubType   uint32
	Val       uint64
	FormulaID uint16
	// 原公式
	RewardString string
}

func (b *BaseReward) Send() error {
	panic("implement me")
}

func (b *BaseReward) GetMainType() uint16 {
	return b.MainType
}

func (b *BaseReward) GetSubType() uint32 {
	return b.SubType
}

func (b *BaseReward) GetVal() uint64 {
	return b.Val
}

func (b *BaseReward) SetRealValue(formulaData interface{}) {
	b.Val = b.RealValue(formulaData)
	b.FormulaID = 0
}

func (b *BaseReward) RealValue(formulaData interface{}) uint64 {
	if b.FormulaID > 0 {
		return calculate(b.Ctx.UserEntity, b.FormulaID, formulaData, b.GetVal())
	} else {
		return b.Val
	}
}

func (b *BaseReward) GetFormulaID() uint16 {
	return b.FormulaID
}

func (b *BaseReward) NeedMerge() bool {
	return true
}

func (b *BaseReward) Merge(reward constant.IReward) {
	// 合并后因为取了真实value，所以要去除公式
	b.Val = b.RealValue(nil) + reward.RealValue(nil)
	b.FormulaID = 0
}

func NewBaseReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64,
	formulaID uint16) *BaseReward {
	return &BaseReward{
		Ctx:          ctx,
		MainType:     mainType,
		SubType:      subType,
		Val:          val,
		FormulaID:    formulaID,
		RewardString: MakeRewardFmt(mainType, subType, val, formulaID),
	}
}

func CreateReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64,
	formulaID uint16) (iReward constant.IReward) {
	switch mainType {
	// 奖励类型 金币
	case RewardTypeGold:
		iReward = NewGoldReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 钻石
	case RewardTypeDiamond:
		iReward = NewDiamondReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 魂石
	case RewardTypeSoulCrystal:
		iReward = NewSoulCrystalReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 宝物精华
	case RewardTypeTreasureAnima:
		iReward = NewTreasureAnimaReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 狂妄之章
	case RewardTypeSuperbiaStone:
		iReward = NewSuperbiaStoneReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 憎恶之章
	case RewardTypeInvidiaStone:
		iReward = NewInvidiaStoneReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 懈怠之章
	case RewardTypeAcediaStone:
		iReward = NewAcediaStoneReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 吞噬之章
	case RewardTypeGulaStone:
		iReward = NewGulaStoneReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 无厌之章
	case RewardTypeAvaritiaStone:
		iReward = NewAvaritiaStoneReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 销魂之章
	case RewardTypeLuxuriaStone:
		iReward = NewLuxuriaStoneReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 肆虐之章
	case RewardTypeIraStone:
		iReward = NewIraStoneReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 本能之力
	case RewardTypeBenYuan:
		iReward = NewBenYuanReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 潜能之力
	case RewardTypeQianNeng:
		iReward = NewQianNengReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 宝物
	case RewardTypeEquip:
		iReward = NewEquipReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 元素结晶
	case RewardTypeElement:
		iReward = NewElementReward(ctx, mainType, subType, val, formulaID)
	// 奖励类型 秘传书
	case RewardTypeBook:
		iReward = NewBookReward(ctx, mainType, subType, val, formulaID)
	}

	return
}

func calculate(userEntity *entity.UserEntity, formulaID uint16, formulaData interface{}, val uint64) (r uint64) {
	switch formulaID {
	case 1: //乘以比例
		var ratio float64 = 1
		if formulaData != nil {
			ratio = cast.ToFloat64(formulaData)
		}
		r = uint64(math.Ceil(ratio * float64(val)))
	}

	return
}
