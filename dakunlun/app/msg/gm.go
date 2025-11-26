package msg

type GmIncrGoldRequest struct {
	Gold uint64 `json:"gold" binding:"required,gt=0"`
}

type GmCommonResponse struct {
}

type GmAddEquipRequest struct {
	EquipID uint32 `json:"equipID" binding:"required,gt=0"`
}
