package entity

const (
	MailIDArena = iota + 1
	MailIDCampain
	MailIDTest
	MailIDBoss
	MailIDTower
)

type MailData struct {
	DataModel
	Type    uint8
	Title   string
	Pic     string
	Content string
}

//设置表名，默认是结构体的名的复数形式
func (*MailData) TableName() string {
	return "data_mail"
}
