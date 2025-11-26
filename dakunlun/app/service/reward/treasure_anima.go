package reward

import "dakunlun/app/constant"

type TreasureAnimaReward struct {
	*BaseReward
}

func NewTreasureAnimaReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *TreasureAnimaReward {
	return &TreasureAnimaReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *TreasureAnimaReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeTreasureAnima, 0, r.RealValue(nil))
	return
}
