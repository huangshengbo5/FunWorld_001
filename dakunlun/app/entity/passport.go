package entity

import (
	"dakunlun/app/util"
	"time"
)

type PassportEntity struct {
	Model
	Name     string `gorm:"unique;type:VARCHAR(128);not null;comment:账号名"`
	Password string `gorm:"type:VARCHAR(128);not null;comment:账号名"`
	LoginAt  time.Time
}

// 设置表名，默认是结构体的名的复数形式
func (*PassportEntity) TableName() string {
	return "game_passport"
}

func NewPassport(name string, password string) *PassportEntity {
	return &PassportEntity{
		Name:     name,
		Password: password,
		LoginAt:  time.Now(),
	}
}

func (entity *PassportEntity) HidingUid() uint32 {
	return util.EncodeID(entity.ID)
}

func (entity *PassportEntity) SetPassword(v string) {
	entity.Name = v
}

func (entity *PassportEntity) GetPassword() string {
	return entity.Name
}
