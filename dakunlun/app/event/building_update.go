package event

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

const BuildingUpdateEventName string = "BuildingUpdate"

type BuildingUpdateObject struct {
	UserEntity         *entity.UserEntity
	UserBuildingEntity *entity.UserBuildingEntity
}

func NewBuildingUpdateEvent(object interface{}) *util.Event {
	return &util.Event{
		Type:   BuildingUpdateEventName,
		Object: object,
	}
}
