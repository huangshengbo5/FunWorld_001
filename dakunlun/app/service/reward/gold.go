package reward

import "dakunlun/app/constant"

type GoldReward struct {
	*BaseReward
}

func NewGoldReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *GoldReward {
	return &GoldReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *GoldReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeGold, 0, r.RealValue(nil))
	return
}
