package entity

import (
	"database/sql/driver"
	"fmt"
	"strings"
	"time"

	"encoding/json"
)

const (
	TypeNormal uint8 = iota
	TypeAds
	TypeGem
)

const (
	CampaignMul     = 2
	OnlineRewardMul = 2
	BossMul         = 2
	TowerMul        = 2
	AlchemyMul      = 2
	BusinessManMul  = 2
)

const (
	BaseMultiple = 10000
)

type Status uint8

const (
	StatusCreated Status = iota
	StatusFailed
	StatusSuccessful
	StatusDone
)

func (s Status) IsCreated() bool {
	return s == StatusCreated
}

func (s Status) IsFailed() bool {
	return s == StatusFailed
}

func (s Status) IsSuccessful() bool {
	return s == StatusSuccessful
}

func (s Status) IsDone() bool {
	return s == StatusDone
}

// gorm.Model 的定义
type Model struct {
	ID        uint32 `gorm:"primaryKey;autoIncrement;type:INT(10) UNSIGNED;not null;comment:自增ID"`
	CreatedAt time.Time
	UpdatedAt time.Time
}

// gorm.Model 的定义
type DataModel struct {
	ID uint32 `gorm:"primaryKey"`
}

type RewardStrings []string

func (r *RewardStrings) Scan(value interface{}) error {
	rewardStrings, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal RewardStrings value:", value)
	}

	var result = make(RewardStrings, 0)
	toString := string(rewardStrings)
	if toString != "" {
		result = strings.Split(toString, ";")
	}

	*r = result
	return nil
}

func (r RewardStrings) Value() (driver.Value, error) {
	if len(r) == 0 {
		return "", nil
	}
	return strings.Join(r, ";"), nil
}

type Uint32Slice []uint32

func (u *Uint32Slice) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal CEffect value:", value)
	}
	result := Uint32Slice{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*u = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (u Uint32Slice) Value() (driver.Value, error) {
	return json.Marshal(u)
}
