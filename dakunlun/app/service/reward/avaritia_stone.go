package reward

import "dakunlun/app/constant"

type AvaritiaStoneReward struct {
	*BaseReward
}

func NewAvaritiaStoneReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *AvaritiaStoneReward {
	return &AvaritiaStoneReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *AvaritiaStoneReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeAvaritiaStone, 0, r.RealValue(nil))
	return
}
