package reward

import "dakunlun/app/constant"

type EquipReward struct {
	*BaseReward
}

func NewEquipReward(ctx *constant.RewardContext, mainType uint16, subType uint32, val uint64, formulaID uint16) *EquipReward {
	return &EquipReward{
		NewBaseReward(ctx, mainType, subType, val, formulaID),
	}
}

func (r *EquipReward) Send() (err error) {
	for i := uint64(0); i < r.RealValue(nil); i++ {
		_, err = r.Ctx.EquipService.AddEquip(r.Ctx.UserEntity, r.GetSubType())
	}
	return
}

func (b *EquipReward) NeedMerge() bool {
	return false
}
