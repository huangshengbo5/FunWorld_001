package entity

import (
	"dakunlun/app/util"
	"database/sql/driver"
	"fmt"
	"github.com/golang-module/carbon"
	"strconv"
	"time"

	"encoding/json"
)

const (
	OnlineRewardAdsNum = 0
	//OnlineRewardInterval = 14400
	OnlineRewardInterval = 120
	OnlineRewardDiamond  = 50
	AlchemyDiamond       = 20
)

const (
	TowerOne = iota + 1
	TowerTwo
	TowerThree
	TowerFour
	TowerFive
	TowerSix
	TowerSeven
	TowerEight
	BusinessOne
	BusinessTwo
)

const (
	BusinessOneFlag = 1 << iota
	BusinessTwoFlag
)

const RefreshTowerNum = 10
const ArenaRemainNum = 10

type TimeSetting struct {
	startHour int
	startMin  int
	startSec  int
	endHour   int
	endMin    int
	endSec    int
}

func (t *TimeSetting) StartTime() carbon.Carbon {
	return util.Carbon().CreateFromTime(t.startHour, t.startMin, t.startSec)
}

func (t *TimeSetting) EndTime() carbon.Carbon {
	return util.Carbon().CreateFromTime(t.endHour, t.endMin, t.endSec)
}

var TimeSettings = map[uint8]*TimeSetting{
	TowerOne:    {startHour: 0, startMin: 0, startSec: 0, endHour: 2, endMin: 59, endSec: 59},
	TowerTwo:    {startHour: 3, startMin: 0, startSec: 0, endHour: 5, endMin: 59, endSec: 59},
	TowerThree:  {startHour: 6, startMin: 0, startSec: 0, endHour: 8, endMin: 59, endSec: 59},
	TowerFour:   {startHour: 9, startMin: 0, startSec: 0, endHour: 11, endMin: 59, endSec: 59},
	TowerFive:   {startHour: 12, startMin: 0, startSec: 0, endHour: 14, endMin: 59, endSec: 59},
	TowerSix:    {startHour: 15, startMin: 0, startSec: 0, endHour: 17, endMin: 59, endSec: 59},
	TowerSeven:  {startHour: 18, startMin: 0, startSec: 0, endHour: 20, endMin: 59, endSec: 59},
	TowerEight:  {startHour: 21, startMin: 0, startSec: 0, endHour: 23, endMin: 59, endSec: 59},
	BusinessOne: {startHour: 12, startMin: 0, startSec: 0, endHour: 13, endMin: 59, endSec: 59},
	BusinessTwo: {startHour: 18, startMin: 0, startSec: 0, endHour: 19, endMin: 59, endSec: 59},
}

type UserExtendEntity struct {
	Model
	CampaignID     uint32       `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	CampaignTime   int64        `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	CampaignOldID  uint32       `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	OnlineReward   OnlineReward `gorm:"type:tinyblob"` //在线奖励数据
	Apocalypse     Apocalypse   `gorm:"type:tinyblob"` //天启数据
	Tower          Tower        `gorm:"type:blob"`     //怪物入侵数据
	ArenaRemainNum uint8        `gorm:"type:TINYINT(3) UNSIGNED;not null;default:0;comment:用户ID"`
	ArenaSignWeek  int          `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	Alchemy        Alchemy      `gorm:"type:blob"` //怪物入侵数据
	BusinessMan    int          `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	Ads            Ads          `gorm:"type:blob"` //怪物入侵数据
	LastModifyDay  int          `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
}

type OnlineReward struct {
	GetFirst        bool //首次奖励已领取
	NextReceiveTime int64
	RemainAdsNum    int
	PayNum          int
	RewardIDs       []uint32
}

func (r *OnlineReward) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal OnlineReward value:", value)
	}
	result := OnlineReward{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r OnlineReward) Value() (driver.Value, error) {
	return json.Marshal(r)
}

type Ads struct {
	AdsNum uint8
	IDs    []uint32
}

func (r *Ads) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Ads value:", value)
	}
	result := Ads{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r Ads) Value() (driver.Value, error) {
	return json.Marshal(r)
}

type Alchemy struct {
	NextReceiveTime int64
	PayNum          int
}

func (r *Alchemy) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Resource value:", value)
	}
	result := Alchemy{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r Alchemy) Value() (driver.Value, error) {
	return json.Marshal(r)
}

type Apocalypse struct {
	BossID    uint32
	RemainNum uint8
	Status    Status
	Ratio     float64
}

func (r *Apocalypse) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Apocalypse value:", value)
	}
	result := Apocalypse{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r Apocalypse) Value() (driver.Value, error) {
	return json.Marshal(r)
}

type Tower map[uint8][]*Monster

type Monster struct {
	TowerID uint32
	Status  Status
	Rewards RewardStrings
}

func (r *Tower) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Tower value:", value)
	}
	result := Tower{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r Tower) Value() (driver.Value, error) {
	return json.Marshal(r)
}

//type Arena struct {
//	uint32
//	Status Status
//}

//
//func (r *Arena) Scan(value interface{}) error {
//	bytes, ok := value.([]byte)
//	if !ok {
//		return fmt.Errorf("Failed to unmarshal Resource value:", value)
//	}
//	result := Arena{}
//	err := json.Unmarshal(bytes, &result)
//	if err != nil {
//		return err
//	}
//	*r = result
//	return err
//}
//
//// 实现 driver.Valuer 接口，Value 返回 json value
//func (r Arena) Value() (driver.Value, error) {
//	return json.Marshal(r)
//}

// 设置表名，默认是结构体的名的复数形式
func (*UserExtendEntity) TableName() string {
	return "game_user_extend"
}

func NewUserExtend(id uint32) *UserExtendEntity {
	r := &UserExtendEntity{
		Model: Model{
			ID: id,
		},
		CampaignID:     0,
		CampaignTime:   0,
		CampaignOldID:  0,
		OnlineReward:   OnlineReward{},
		Apocalypse:     Apocalypse{},
		Tower:          Tower{},
		ArenaRemainNum: ArenaRemainNum,
		ArenaSignWeek:  0,
		Alchemy:        Alchemy{},
		BusinessMan:    0,
		Ads:            Ads{},
	}

	day, _ := strconv.Atoi(util.Carbon().Now().Format("Ymd"))

	r.ResetPerDay(day, true)

	return r
}

func (e *UserExtendEntity) ResetPerDay(day int, isFirst bool) {
	now := time.Now().Unix()
	//是否首次
	if isFirst {
		e.OnlineReward.NextReceiveTime = now + OnlineRewardInterval
	}
	e.LastModifyDay = day
	e.OnlineReward.RemainAdsNum = OnlineRewardAdsNum
	e.OnlineReward.PayNum = 0
	e.Apocalypse = Apocalypse{}
	e.Tower = make(map[uint8][]*Monster, 6)
	e.ArenaRemainNum = ArenaRemainNum
	e.Alchemy.PayNum = 0
	e.BusinessMan = 0
	e.Ads = Ads{}
}

func (e *UserExtendEntity) NeedDiamondOnlineReward() uint32 {
	return OnlineRewardDiamond
}

func (e *UserExtendEntity) ResetOnlineReward() {
	e.OnlineReward.RewardIDs = nil
	e.OnlineReward.NextReceiveTime = time.Now().Unix() + OnlineRewardInterval
}

func (e *UserExtendEntity) WhichTower() uint8 {
	now := util.Carbon().Now()
	//for towerLevel, setting := range TimeSettings {
	for i := uint8(TowerEight); i >= TowerOne; i-- {
		if now.Gte(TimeSettings[i].StartTime()) && now.Lte(TimeSettings[i].EndTime()) {
			return i
		}
	}
	return 0
}

func (e *UserExtendEntity) WhichBusinessMan() uint8 {
	now := util.Carbon().Now()
	//for towerLevel, setting := range TimeSettings {
	for i := uint8(BusinessTwo); i >= BusinessOne; i-- {
		if now.Gte(TimeSettings[i].StartTime()) && now.Lte(TimeSettings[i].EndTime()) {
			return i
		}
	}
	return 0
}

func (e *UserExtendEntity) NeedDiamondAlchemy() uint32 {
	return AlchemyDiamond * 1
}

func (e *UserExtendEntity) AlchemyInCD() bool {
	return time.Now().Unix() < e.Alchemy.NextReceiveTime
}

func (e *UserExtendEntity) ArenaIsSign() bool {
	return e.ArenaSignWeek == util.GetYearWeek()
}
