package reward

import "dakunlun/app/constant"

type AcediaStoneReward struct {
	*BaseReward
}

func NewAcediaStoneReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *AcediaStoneReward {
	return &AcediaStoneReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *AcediaStoneReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeAcediaStone, 0, r.RealValue(nil))
	return
}
