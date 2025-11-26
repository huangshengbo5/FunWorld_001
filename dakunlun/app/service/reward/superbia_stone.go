package reward

import "dakunlun/app/constant"

type SuperbiaStoneReward struct {
	*BaseReward
}

func NewSuperbiaStoneReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *SuperbiaStoneReward {
	return &SuperbiaStoneReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *SuperbiaStoneReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeSuperbiaStone, 0, r.RealValue(nil))
	return
}
