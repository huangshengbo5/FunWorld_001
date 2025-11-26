package reward

import "dakunlun/app/constant"

type GulaStoneReward struct {
	*BaseReward
}

func NewGulaStoneReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *GulaStoneReward {
	return &GulaStoneReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *GulaStoneReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeGulaStone, 0, r.RealValue(nil))
	return
}
