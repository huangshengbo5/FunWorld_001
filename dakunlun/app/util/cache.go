package util

import (
	"github.com/bluele/gcache"
)

const goCacheSize = 2000

var dataCache gcache.Cache
var logicCache gcache.Cache

func MustInitCache() {
	dataCache = gcache.New(goCacheSize).LRU().Build()
	//logicCache = gcache.New(goCacheSize).LRU().Build()
}

// 配置缓存
func DataCache() gcache.Cache {
	return dataCache
}

// 业务缓存
func LogicCache() gcache.Cache {
	return logicCache
}
