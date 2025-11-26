package listener

import (
	"dakunlun/app/event"
	"dakunlun/app/service"
	"dakunlun/app/util"
)

func BuildingUpdateLisenter(ev *util.Event) {
	if ev.Type != event.BuildingUpdateEventName {
		util.GetLogger().Panic("event type error")
		return
	}

	BuildingUpdateObject, ok := ev.Object.(*event.BuildingUpdateObject)

	if !ok {
		util.GetLogger().Panic("LevelUpListener object type error")
		return
	}

	err := service.UserBuildingService.Effect(BuildingUpdateObject.UserEntity, BuildingUpdateObject.UserBuildingEntity)
	if err != nil {
		util.GetLogger().Panic(err.Error())
		return
	}
}
