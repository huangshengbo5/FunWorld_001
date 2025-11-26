package entity

import (
	"database/sql/driver"
	"fmt"

	"encoding/json"
)

const (
	MailStatusUnRead = iota
	MailStatusRead
	MailStatusDelete
)

type UserMailEntity struct {
	Model
	Uid         uint32        `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	MailID      uint32        `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	Params      Params        `gorm:"type:tinyblob"`
	Status      uint8         `gorm:"type:TINYINT(3) UNSIGNED;not null;default:0;comment:用户ID"`
	HasReceived IntBool       `gorm:"type:TINYINT(3) UNSIGNED;not null;default:0;comment:用户ID"`
	Attachment  RewardStrings `gorm:"type:tinyblob"`
}

type Params []string

func (r *Params) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal Skills value:", value)
	}
	result := Params{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r Params) Value() (driver.Value, error) {
	return json.Marshal(r)
}

// 设置表名，默认是结构体的名的复数形式
func (*UserMailEntity) TableName() string {
	return "game_user_mail"
}

func NewUserMail(uid uint32, mailID uint32, params Params, rewards RewardStrings) *UserMailEntity {
	return &UserMailEntity{
		Uid:         uid,
		MailID:      mailID,
		Params:      params,
		Status:      MailStatusUnRead,
		HasReceived: IntFalse,
		Attachment:  rewards,
	}
}

func (e *UserMailEntity) IsUnRead() bool {
	return e.Status == MailStatusUnRead
}

func (e *UserMailEntity) IsRead() bool {
	return e.Status == MailStatusRead
}

func (e *UserMailEntity) MarkRead() {
	e.Status = MailStatusRead
}

func (e *UserMailEntity) IsReceived() bool {
	return e.HasReceived == IntTrue
}

func (e *UserMailEntity) MarkReceived() {
	e.HasReceived = IntTrue
}
