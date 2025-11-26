package entity

type UserBuildingEntity struct {
	Model
	Uid        uint32 `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	BuildingID uint32 `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:建筑ID"`
	Level      uint16 `gorm:"type:SMALLINT(6) UNSIGNED;not null;default:0;comment:等级"`
}

// 设置表名，默认是结构体的名的复数形式
func (*UserBuildingEntity) TableName() string {
	return "game_user_building"
}

func NewUserBuilding(uid, buildingID uint32, level uint16) *UserBuildingEntity {
	return &UserBuildingEntity{
		Uid:        uid,
		BuildingID: buildingID,
		Level:      level,
	}
}
