package listener

import (
	"dakunlun/app/util"
)

func BuildingOpenLisenter(ev *util.Event) {
	//if ev.Type != event.LevelUpEventName {
	//	util.GetLogger().Panic("event type error")
	//	return
	//}
	//
	//userEntity, ok := ev.Object.(*entity.UserEntity)
	//
	//if !ok {
	//	util.GetLogger().Panic("LevelUpListener object type error")
	//	return
	//}
	//
	//// 获取符合开放条件的建筑
	//buildingDatas, err := data.BuildingService.GetBuildingsByCondition(constant.OpenTypeLevel, userEntity.Level)
	//if err != nil {
	//	util.GetLogger().Panic(err.Error())
	//	return
	//}
	//
	//// 创建新建筑
	//for _, buildingData := range buildingDatas {
	//	_, err := service.UserBuildingService.AddBuilding(userEntity, buildingData.ID, 1)
	//	if err != nil {
	//		util.GetLogger().Panic(err.Error())
	//		return
	//	}
	//}
}
