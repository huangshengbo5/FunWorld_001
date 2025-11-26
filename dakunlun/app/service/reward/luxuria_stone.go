package reward

import "dakunlun/app/constant"

type LuxuriaStoneReward struct {
	*BaseReward
}

func NewLuxuriaStoneReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *LuxuriaStoneReward {
	return &LuxuriaStoneReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *LuxuriaStoneReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeLuxuriaStone, 0, r.RealValue(nil))
	return
}
