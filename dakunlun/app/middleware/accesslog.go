package middleware

import (
	"bytes"
	"dakunlun/app/constant"
	"dakunlun/app/util"
	"fmt"
	"io/ioutil"
	"time"

	"github.com/spf13/cast"

	"encoding/json"

	"go.uber.org/zap"

	"github.com/gin-gonic/gin"
)

func AccessLog() gin.HandlerFunc {
	return func(c *gin.Context) {
		start := time.Now()

		// 从原有Request.Body读取
		bodyBytes, _ := ioutil.ReadAll(c.Request.Body)
		req := map[string]interface{}{}
		if len(bodyBytes) > 0 {
			// 新建缓冲区并替换原有Request.body
			c.Request.Body = ioutil.NopCloser(bytes.NewBuffer(bodyBytes))
			json.Unmarshal(bodyBytes, &req)
		}
		uid, _ := c.Get(constant.CtxConstUid)
		c.Next()
		rsp, _ := c.Get(constant.CtxConstRsp)
		util.GetLogger().Info("access_log",
			zap.String("uri", c.Request.URL.Path),
			zap.String("log_id", fmt.Sprintf("%v_%v", uid, start.UnixNano())),
			zap.String("uid", cast.ToString(uid)),
			zap.String("host", c.Request.Host),
			zap.String("client_ip", c.ClientIP()),
			zap.String("proto", c.Request.Proto),
			zap.String("referer", c.Request.Referer()),
			zap.String("method", c.Request.Method),
			zap.String("content_type", c.Request.Header.Get("Content-Type")),
			zap.String("user_agent", c.Request.UserAgent()),
			zap.Any("input", req),
			zap.String("cost", fmt.Sprintf("%dms", time.Since(start).Milliseconds())), // 毫秒),)
			zap.Int("status", c.Writer.Status()),
			zap.Int("size", c.Writer.Size()),
			zap.Any("output", rsp))
	}
}
