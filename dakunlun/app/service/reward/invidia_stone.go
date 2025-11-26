package reward

import "dakunlun/app/constant"

type InvidiaStoneReward struct {
	*BaseReward
}

func NewInvidiaStoneReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *InvidiaStoneReward {
	return &InvidiaStoneReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *InvidiaStoneReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeInvidiaStone, 0, r.RealValue(nil))
	return
}
