package middleware

import (
	"context"
	"dakunlun/app/constant"
	"dakunlun/app/service"
	"dakunlun/app/util"
	"fmt"
	"strconv"
	"time"

	"github.com/gin-gonic/gin"
	"github.com/go-redis/redis"
)

type GetSessionIdFromContext func(ctx *gin.Context) (sessionId string)

type SeqConf struct {
	SessionIDProvider GetSessionIdFromContext
	ExpireTime        time.Duration
}

// 用于检查seqid
func SeqCheck(conf *SeqConf) gin.HandlerFunc {
	return func(c *gin.Context) {
		//获取seqID
		seqID, err := strconv.Atoi(c.GetHeader(constant.HeaderSeqID))

		if err != nil {
			service.AbortWithError(c, util.ErrorCodeHack, err.Error())
			return
		}

		lastSeqIDstr, err := util.GetRedisClient().Get(context.Background(), conf.SessionIDProvider(c)).Result()
		if err != nil {
			if redis.Nil == err {
				lastSeqIDstr = "-1"
			} else {
				service.AbortWithError(c, util.ErrorCodeHack, "get lastSeqID failed")
				return
			}
		}

		lastSeqID, _ := strconv.Atoi(lastSeqIDstr)

		if seqID <= lastSeqID {
			service.AbortWithError(c, util.ErrorCodeHack, "seqid check failed")
			return
		}

		util.GetRedisClient().Set(context.Background(), conf.SessionIDProvider(c), seqID, conf.ExpireTime).Err()
		if err != nil {
			util.GetLogger().Error(err.Error())
			seqID = lastSeqID
		}

		c.Set(constant.CtxConstSeqID, seqID)

		c.Next()
	}

}

func GetSessionID(ctx *gin.Context) (sessionId string) {
	uid, _ := service.GetUidInContext(ctx)
	token, _ := service.GetTokenInContext(ctx)
	return fmt.Sprintf("%d_%s", uid, token)
}
