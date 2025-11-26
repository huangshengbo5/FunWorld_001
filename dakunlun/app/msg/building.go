package msg

type BuildingListRequest struct {
}

type BuildingListResponse struct {
	Buildings []*Building `json:"buildings"`
}

type Building struct {
	ID         uint32 `json:"id"`
	BuildingID uint32 `json:"buildingID"`
	Level      uint16 `json:"level"`
}

type BuildingUpgradeRequest struct {
	ID uint32 `json:"id" binding:"required,gt=0"`
}

type BuildingUpgradeResponse struct {
	Level uint16 `json:"level"`
}
