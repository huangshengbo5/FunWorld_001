package entity

type UserExploreEntity struct {
	Model
	Uid       uint32      `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:用户ID"`
	ExploreID uint32      `gorm:"uniqueIndex:udx_name;type:INT(10) UNSIGNED;not null;default:0;comment:探索ID"`
	StartTime int64       `gorm:"type:INT(10) UNSIGNED;not null;default:0;comment:结束时间"`
	HeroIDs   Uint32Slice `gorm:"type:blob"`
}

func (*UserExploreEntity) TableName() string {
	return "game_user_explore"
}

func NewUserExplore(uid, exploreID uint32) *UserExploreEntity {
	return &UserExploreEntity{
		Uid:       uid,
		ExploreID: exploreID,
		HeroIDs:   Uint32Slice{},
	}
}

func (e *UserExploreEntity) Mul(exploreData *ExploreData, userHeroEntitys []*UserHeroEntity) (r float64) {
	r = 1.0
	for _, v := range e.HeroIDs {
		for _, userHeroEntity := range userHeroEntitys {
			if userHeroEntity.ID == v {
				r += 0.025
				r += float64(userHeroEntity.Level+userHeroEntity.EvolveTimes) * 0.0001
				if exploreData.InRaces(userHeroEntity.Race) {
					r += 0.025
				}
				break
			}
		}
	}

	return
}
