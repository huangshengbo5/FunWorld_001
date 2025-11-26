package util

import (
	"time"

	"github.com/edwingeng/wuid/mysql/wuid"
)

type IUUIDGenerator interface {
	Gen() int64
}

type IStringGenerator interface {
	Gen() int64
}

var wuidPool map[string]*wuid.WUID

var tags = []struct {
	string
	int8
}{{"uid", 1}}

func MustInitUUIDClient() {
	wuidPool := make(map[string]*wuid.WUID, len(tags))
	for _, tag := range tags {
		g := wuid.NewWUID(tag.string, nil, wuid.WithSection(tag.int8))
		err := g.LoadH28FromMysql(nil, "wuid")
		PanicIfErr(err)
		wuidPool[tag.string] = g
	}

}

type LocalUUID struct {
}

func (LocalUUID) Gen() int64 {
	return time.Now().UnixNano()
}

//var wuid
type WuidWithMysql struct {
}

func (WuidWithMysql) Gen() int64 {
	return time.Now().UnixNano()
}

//
//func testv1() {
//	id, err := uuid.NewUUID()
//	if err != nil {
//		fmt.Printf("%v\n", err)
//		return
//	}
//	fmt.Printf("%s %s\n", id, id.Version().String())
//}
//
//func testv4() {
//	id := uuid.New()
//	fmt.Printf("%s %s\n", id, id.Version().String())
//
//	————————————————
//	版权声明：本文为CSDN博主「pengpengzhou」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
//	原文链接：https://blog.csdn.net/pengpengzhou/article/details/105269410
