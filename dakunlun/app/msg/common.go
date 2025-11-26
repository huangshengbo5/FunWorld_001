package msg

type Reward struct {
	MainType uint16 `json:"mainType"`
	SubType  uint32 `json:"subType"`
	Val      uint64 `json:"val"`
}
