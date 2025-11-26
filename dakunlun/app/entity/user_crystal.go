package entity

type UserCrystalEntity struct {
	Model
	Uid       uint32 `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	CrystalID uint32 `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:水晶ID"`
	Level     uint16 `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:等级"`
}

// 设置表名，默认是结构体的名的复数形式
func (*UserCrystalEntity) TableName() string {
	return "game_user_crystal"
}

func NewUserCrystal(uid, crystalID uint32, level uint16) *UserCrystalEntity {
	return &UserCrystalEntity{
		Uid:       uid,
		CrystalID: crystalID,
		Level:     level,
	}
}
