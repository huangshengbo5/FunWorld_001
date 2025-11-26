package util

import (
	"context"

	"github.com/go-redis/redis/v8"
)

var rdb *redis.Client

type RedisConf struct {
	Uri   string
	Pwd   string
	DB    int
	Addrs []string
}

func MustInitRedis(conf *RedisConf) {
	rdb = redis.NewClient(&redis.Options{
		Addr:     conf.Uri,
		Password: conf.Pwd, // no password set
		DB:       conf.DB,  // use default DB
	})

	pong, err := rdb.Ping(context.Background()).Result()
	PanicIfErr(err)
	GetLogger().Info(pong)
}

func GetRedisClient() *redis.Client {
	return rdb
}

func FreeRedisClient() (err error) {
	return rdb.Close()
}
