package reward

import "dakunlun/app/constant"

type ElementReward struct {
	*BaseReward
}

func NewElementReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *ElementReward {
	return &ElementReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *ElementReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeElement, 0, r.RealValue(nil))
	return
}
