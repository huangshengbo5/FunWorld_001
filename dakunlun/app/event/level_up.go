package event

import "dakunlun/app/util"

const LevelUpEventName string = "LevelUp"

func NewLevelUpEvent(object interface{}) *util.Event {
	return &util.Event{
		Type:   LevelUpEventName,
		Object: object,
	}
}
