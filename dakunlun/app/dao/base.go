package dao

import (
	"context"
	"dakunlun/app/util"
	"errors"
	"fmt"
	"time"

	"github.com/go-redis/redis/v8"

	"go.uber.org/zap"
)

const (
	KeyID = "id:%v"
)

func makeCacheKey(prefix string, format string, args ...interface{}) string {
	return "g:" + prefix + ":" + fmt.Sprintf(format, args)
}

func loadFromCache(key string) []byte {
	val, err := util.GetRedisClient().Get(context.Background(), key).Result()
	if err != nil {
		if errors.Is(err, redis.Nil) {
			return nil
		} else {
			util.GetLogger().Error("dao.loadFromCache", zap.Error(err))
		}
	}

	return []byte(val)
}

func saveToCache(key string, val []byte, expire time.Duration) (err error) {
	err = util.GetRedisClient().Set(context.Background(), key, val, expire).Err()
	if err != nil {
		util.GetLogger().Error("dao.saveToCache", zap.Error(err))
	}
	return
}

func deleteCache(keys ...string) (err error) {
	err = util.GetRedisClient().Del(context.Background(), keys...).Err()
	if err != nil {
		util.GetLogger().Error("dao.deleteCache", zap.Error(err))
	}
	return
}
