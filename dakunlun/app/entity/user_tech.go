package entity

type UserTechEntity struct {
	Model
	Uid    uint32 `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	TechID uint32 `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	Level  uint16 `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:用户ID"`
}

// 设置表名，默认是结构体的名的复数形式
func (*UserTechEntity) TableName() string {
	return "game_user_tech"
}

func NewUserTech(uid, techID uint32, level uint16) *UserTechEntity {
	return &UserTechEntity{
		Uid:    uid,
		TechID: techID,
		Level:  level,
	}
}
