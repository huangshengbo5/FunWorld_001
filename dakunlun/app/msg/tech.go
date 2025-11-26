package msg

type TechListRequest struct {
}

type TechListResponse struct {
	Techs []*Tech `json:"techs"`
}

type Tech struct {
	ID     uint32 `json:"id"`
	TechID uint32 `json:"techID"`
	Level  uint16 `json:"level"`
}

type TechUpgradeRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type TechUpgradeResponse struct {
	Level uint16 `json:"level"`
}
