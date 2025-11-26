package msg

type CrystalListRequest struct {
}

type CrystalListResponse struct {
	Crystals []*Crystal `json:"Crystals"`
}

type Crystal struct {
	ID        uint32 `json:"id"`
	CrystalID uint32 `json:"CrystalID"`
	Level     uint16 `json:"level"`
}

type CrystalUpgradeRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type CrystalUpgradeResponse struct {
	Level uint16 `json:"level"`
}
