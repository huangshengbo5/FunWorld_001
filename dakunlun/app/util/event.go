package util

import (
	"fmt"

	"go.uber.org/zap"
)

// 创建事件
func NewEvent(eventType string, object interface{}) Event {
	e := Event{Type: eventType, Object: object}
	return e
}

// 事件类型基类
type Event struct {
	//事件触发实例
	Target IEventDispatcher
	//事件类型
	Type string
	//事件携带数据源
	Object interface{}
}

// 克隆事件
func (ec *Event) Clone() *Event {
	e := new(Event)
	e.Type = ec.Type
	e.Target = ec.Target
	e.Object = ec.Object
	return e
}

func (ec *Event) String() string {
	return fmt.Sprintf("Event Type:%s object:%+v", ec.Type, ec.Object)
}

// 监听器
type eventListener struct {
	Handler EventHandler
}

// 监听器函数
type EventHandler func(event *Event)

// 创建监听器
func NewEventListener(h EventHandler) *eventListener {
	l := new(eventListener)
	l.Handler = h
	return l
}

// 事件调度接口
type IEventDispatcher interface {
	//事件监听
	On(eventType string, listener *eventListener)
	//移除事件监听
	Remove(eventType string, listener *eventListener) bool
	//是否包含事件
	Contains(eventType string) bool
	//事件派发
	Fire(events ...*Event)
}

// 事件调度器基类
type eventDispatcher struct {
	//savers []*EventSaver
	savers map[string]*eventSaver
}

// 事件调度器中存放的单元
type eventSaver struct {
	Type      string
	Listeners []*eventListener
}

var dispatcher *eventDispatcher

// 创建事件派发器
func MustInitEventDispatcher() {
	dispatcher = &eventDispatcher{
		savers: make(map[string]*eventSaver),
	}
}

// 获取事件派发器
func EventDispatcher() *eventDispatcher {
	if dispatcher == nil {
		GetLogger().Error("EventDispatcher", zap.String("reason", "event dispatcher not init"))
	}
	return dispatcher
}

// 事件调度器添加事件
func (ed *eventDispatcher) On(eventType string, listener *eventListener) {
	if _, exists := ed.savers[eventType]; !exists {
		saver := &eventSaver{Type: eventType, Listeners: []*eventListener{listener}}
		ed.savers[eventType] = saver
	} else {
		ed.savers[eventType].Listeners = append(ed.savers[eventType].Listeners, listener)
	}
}

// 事件调度器移除某个监听
func (ed *eventDispatcher) Remove(eventType string, listener *eventListener) bool {
	if _, exists := ed.savers[eventType]; exists {
		delete(ed.savers, eventType)
	}
	return false
}

// 事件调度器是否包含某个类型的监听
func (ed *eventDispatcher) Contains(eventType string) bool {
	_, exists := ed.savers[eventType]
	return exists
}

// 事件调度器派发事件
func (ed *eventDispatcher) Fire(events ...*Event) {
	if len(events) == 0 {
		return
	}

	for _, ev := range events {
		if saver, exists := ed.savers[ev.Type]; exists {
			for _, listener := range saver.Listeners {
				ev.Target = ed
				listener.Handler(ev)
			}
		}
	}
}
