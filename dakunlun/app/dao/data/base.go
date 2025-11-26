package data

import (
	"dakunlun/app/util"
	"errors"
	"fmt"
	"time"

	"go.uber.org/zap"

	"github.com/bluele/gcache"
)

const (
	KeyID  = "id:%v"
	KeyAll = "all"
)

func makeCacheKey(prefix string, format string, args ...interface{}) string {
	return "d:" + prefix + ":" + fmt.Sprintf(format, args...)
}

func loadFromCache(key string) interface{} {
	val, err := util.DataCache().Get(key)
	if err != nil {
		if errors.Is(err, gcache.KeyNotFoundError) {
			val = nil
			err = nil
		} else {
			util.GetLogger().Error("data.loadFromCache", zap.Error(err))
		}
	}

	return val
}

func saveToCache(key string, val interface{}) (err error) {
	err = util.DataCache().SetWithExpire(key, val, time.Hour*72) //缓存时间1周
	if err != nil {
		util.GetLogger().Error("data.saveToCache", zap.Error(err))
	}
	return
}
