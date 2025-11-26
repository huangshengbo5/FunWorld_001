package reward

import "dakunlun/app/constant"

type BookReward struct {
	*BaseReward
}

func NewBookReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *BookReward {
	return &BookReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *BookReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeBook, 0, r.RealValue(nil))
	return
}
