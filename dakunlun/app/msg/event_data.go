package msg

const (
	EventTypeUserInfo = iota + 1
	EventTypeUserExtend
)

type EventData struct {
	Type       uint8
	EventParam interface{}
}
