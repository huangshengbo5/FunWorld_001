package reward

import "dakunlun/app/constant"

type IraStoneReward struct {
	*BaseReward
}

func NewIraStoneReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *IraStoneReward {
	return &IraStoneReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *IraStoneReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeIraStone, 0, r.RealValue(nil))
	return
}
