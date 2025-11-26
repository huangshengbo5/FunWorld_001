package main

import (
	"context"
	"fmt"

	"github.com/go-redis/redis/v8"
)

var RedisClient *redis.Client

// 初始化 redis
func NewRedisClient() (*redis.Client, error) {
	RedisClient = redis.NewClient(&redis.Options{
		Addr:     "192.168.142.128:6379", // 要连接的redis IP:port
		Password: "",                     // redis 密码
		DB:       0,                      // 要连接的redis 库
	})
	// 检测心跳
	pong, err := RedisClient.Ping(context.Background()).Result()
	if err != nil {
		fmt.Println("connect redis failed")
		return nil, err
	}
	fmt.Printf("redis ping result: %s\n", pong)
	return RedisClient, nil
}

type Player struct {
	ID   int
	Name string
}

func main() {
	NewRedisClient()
	val1, err := RedisClient.LRange(context.TODO(), "xxxxxxxx", 0, 1).Result()
	fmt.Println(val1, err)
}
