package entity

import (
	"dakunlun/app/util"

	"encoding/json"
)

const SecStep = 5

type ExploreData struct {
	DataModel
	Rewards  RewardStrings
	Races    string
	Duration int64
}

// 设置表名，默认是结构体的名的复数形式
func (*ExploreData) TableName() string {
	return "data_explore"
}

func (e *ExploreData) InRaces(v uint8) bool {
	races := []uint8{}
	json.Unmarshal([]byte(e.Races), &races)
	return util.ContainsUint8(races, v)
}
