package entity

import (
	"database/sql/driver"
	"fmt"

	"encoding/json"
)

type AnnalsList []uint32

func (r *AnnalsList) Scan(value interface{}) error {
	bytes, ok := value.([]byte)
	if !ok {
		return fmt.Errorf("Failed to unmarshal AnnalsList value:", value)
	}
	result := AnnalsList{}
	err := json.Unmarshal(bytes, &result)
	if err != nil {
		return err
	}
	*r = result
	return err
}

// 实现 driver.Valuer 接口，Value 返回 json value
func (r AnnalsList) Value() (driver.Value, error) {
	return json.Marshal(r)
}

type UserAnnalsEntity struct {
	Model
	DoneList AnnalsList `gorm:"type:blob"`
}

// 设置表名，默认是结构体的名的复数形式
func (*UserAnnalsEntity) TableName() string {
	return "game_user_annals"
}

func NewUserAnnals(uid uint32) *UserAnnalsEntity {
	return &UserAnnalsEntity{
		Model: Model{
			ID: uid,
		},
		DoneList: AnnalsList{},
	}
}

func (e *UserAnnalsEntity) AddAnnals(id uint32) bool {
	for _, v := range e.DoneList {
		if v == id {
			return false
		}
	}

	e.DoneList = append(e.DoneList, id)

	return true
}
