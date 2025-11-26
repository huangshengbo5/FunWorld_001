package reward

import "dakunlun/app/constant"

type BenYuanReward struct {
	*BaseReward
}

func NewBenYuanReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *BenYuanReward {
	return &BenYuanReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *BenYuanReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeBenYuan, 0, r.RealValue(nil))
	return
}
