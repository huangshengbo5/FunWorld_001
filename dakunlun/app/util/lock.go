package util

import (
	"context"
	"strings"
	"time"

	"github.com/go-redis/redis/v8"
	"github.com/google/uuid"
)

type LuaScript struct {
	src  string
	hash string
}

var unlockScript LuaScript

func MustLockScript() {
	src := `
if redis.call("get",KEYS[1]) == ARGV[1] then
    return redis.call("del",KEYS[1])
else
    return 0
end
`
	unlockScript = LuaScript{
		src:  src,
		hash: GetRedisClient().ScriptLoad(context.TODO(), src).Val(),
	}
}

type redisLock struct {
	redis  *redis.Client
	name   string
	owner  string
	expire time.Duration
}

func NewRedisLock(name, owner string, expire time.Duration) *redisLock {
	if owner == "" {
		owner = uuid.NewString()
	}
	return &redisLock{
		redis:  GetRedisClient(),
		name:   name,
		owner:  owner,
		expire: expire,
	}
}

func (lock *redisLock) Lock() bool {
	ok, err := lock.redis.SetNX(context.TODO(), lock.name, lock.owner, lock.expire).Result()
	if err != nil {
		panic(err)
	}
	return ok
}

func (lock *redisLock) Block(waitTime time.Duration) bool {
	startTime := time.Now()
	for !lock.Lock() {
		if time.Now().Sub(startTime) > waitTime {
			return false
		}
		time.Sleep(time.Millisecond * 300)
	}
	return true
}

func (lock *redisLock) Unlock() {
	ctx := context.TODO()
	r := lock.redis.EvalSha(ctx, unlockScript.hash, []string{lock.name}, lock.owner)
	if err := r.Err(); err != nil && strings.HasPrefix(err.Error(), "NOSCRIPT ") {
		r = lock.redis.Eval(ctx, unlockScript.src, []string{lock.name}, lock.owner)
	}
}
