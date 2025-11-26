package entity

import (
	"dakunlun/app/util"
	"fmt"
	"strconv"
	"time"
)

const PerGroup = 200
const RewardWin = "2_0_10"
const RewardLose = "2_0_5"

type IntBool = uint8

const (
	IntFalse IntBool = iota
	IntTrue
)

type UserArenaEntity struct {
	Model
	Uid              uint32        `gorm:"unique;type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	IsPlayer         uint8         `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:是否是用户"`
	Name             string        `gorm:"type:VARCHAR(64) ;not null;default:0;comment:用户ID"`
	Avatar           uint16        `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:头像"`
	Level            uint16        `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:低级"`
	FightingCapacity uint64        `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:战力"`
	SignWeek         int           `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:签到周"`
	GroupID          uint16        `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:分组"`
	Rank             uint16        `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:排名"`
	SendReward       IntBool       `gorm:"type:TINYINT(3) UNSIGNED;not null;default:0;comment:是否发奖"`
	Rewards          RewardStrings `gorm:"-"`
}

func (e *UserArenaEntity) Key() string {
	return fmt.Sprintf("arena:%v:%v", e.SignWeek, e.GroupID)
}

// 周日0点5分
func (e *UserArenaEntity) Expire() time.Duration {
	return time.Second*time.Duration(util.Carbon().Now().EndOfWeek(time.Monday).DiffInSecondsWithAbs(util.Carbon().Now())) + 5*time.
		Minute
}

func (e *UserArenaEntity) IsAsc() bool {
	return true
}

func (e *UserArenaEntity) GetScore() float64 {
	return float64(e.Rank)
}

func (e *UserArenaEntity) GetMember() string {
	return strconv.Itoa(int(e.ID))
}

// 设置表名，默认是结构体的名的复数形式
func (*UserArenaEntity) TableName() string {
	return "game_user_arena"
}

func NewUserArenaByPlayer(userEntity *UserEntity, mainHero *UserHeroEntity) *UserArenaEntity {
	return &UserArenaEntity{
		Uid:   userEntity.ID,
		Name:  userEntity.GetName(true),
		Level: userEntity.Level,
		FightingCapacity: userEntity.Attr.FightingCapacityEquipPlus + userEntity.TechEffect.
			MainHeroFightingCapacityPlus + mainHero.FightingCapacity + userEntity.CastEffect.FightingCapacityPlus,
		IsPlayer: 1,
		Avatar:   userEntity.Avatar,
		Rank:     0,
	}
}

func NewUserArenaByNpc(npcData *NpcData, week int) *UserArenaEntity {
	return &UserArenaEntity{
		Uid:              npcData.ID,
		Name:             npcData.Name,
		Level:            npcData.Level,
		FightingCapacity: npcData.FightingCapacity,
		IsPlayer:         0,
		Avatar:           0,
		Rank:             0,
		SignWeek:         week,
	}
}

func (e *UserArenaEntity) IsRealPerson() bool {
	return e.IsPlayer == 1
}

func (e *UserArenaEntity) IsNpc() bool {
	return e.IsPlayer == 0
}

func (e *UserArenaEntity) IsSign() bool {
	return e.SignWeek == util.GetYearWeek()
}

func (e *UserArenaEntity) SignUp() {
	e.SignWeek = util.GetYearWeek()
	e.GroupID = 0
	e.Rank = 0
	e.SendReward = IntFalse
}

func (e *UserArenaEntity) HasSend() bool {
	return e.SendReward == IntTrue
}

func (e *UserArenaEntity) MarkSend() {
	e.SendReward = IntTrue
}
