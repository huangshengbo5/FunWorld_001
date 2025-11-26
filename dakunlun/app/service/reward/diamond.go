package reward

import "dakunlun/app/constant"

type DiamondReward struct {
	*BaseReward
}

func NewDiamondReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *DiamondReward {
	return &DiamondReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *DiamondReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeDiamond, 0, r.RealValue(nil))
	return
}
