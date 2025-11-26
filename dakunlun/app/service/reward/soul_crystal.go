package reward

import "dakunlun/app/constant"

type SoulCrystalReward struct {
	*BaseReward
}

func NewSoulCrystalReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *SoulCrystalReward {
	return &SoulCrystalReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *SoulCrystalReward) Send() (err error) {
	err = r.Ctx.UserService.IncrAssets(r.Ctx.UserEntity, constant.CostTypeSoulCrystal, 0, r.RealValue(nil))
	return
}
