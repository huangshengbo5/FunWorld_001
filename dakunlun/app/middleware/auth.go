package middleware

import (
	"dakunlun/app/constant"
	"dakunlun/app/service"
	"dakunlun/app/util"
	"fmt"
	"strconv"
	"time"

	"github.com/gin-gonic/gin"
)

func Auth() gin.HandlerFunc {
	return func(c *gin.Context) {

		debug := c.Query("debug")
		if debug == "1" {
			uidString := c.Query("uid")
			uidInt, _ := strconv.Atoi(uidString)
			c.Header(constant.HeaderSeqID, fmt.Sprintf("%v", time.Now().Unix()))
			c.Set(constant.CtxConstUid, uint32(uidInt))
			c.Set(constant.CtxConstToken, "")
			c.Next()
		} else {
			token := c.GetHeader(constant.HeaderGameToken)
			if token == "" {
				service.AbortWithError(c, util.ErrorCodeHack, "用户令牌为空")
				return
			}

			uidInt, err := strconv.Atoi(c.GetHeader(constant.HeaderGameUid))
			if err != nil {
				service.AbortWithError(c, util.ErrorCodeHack, err.Error())
				return
			}
			//反解用户ID
			uidUint32 := util.DecodeID(uint32(uidInt))

			ok, err := service.TokenService.AuthToken(uidUint32, token, service.TokenTypeAceessToken)

			if err != nil {
				service.AbortWithError(c, util.ErrorCodeHack, err.Error())
				return
			}

			if !ok {
				service.AbortWithError(c, util.ErrorCodeTokenIsInvalid, "")
				return
			}

			c.Set(constant.CtxConstUid, uidUint32)
			c.Set(constant.CtxConstToken, token)
			c.Next()
		}

	}
}
