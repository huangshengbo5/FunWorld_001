package reward

import "dakunlun/app/constant"

type QianNengReward struct {
	*BaseReward
}

func NewQianNengReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *QianNengReward {
	return &QianNengReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *QianNengReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeQianNeng, 0, r.RealValue(nil))
	return
}
