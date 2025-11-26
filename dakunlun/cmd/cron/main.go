package main

import (
	"dakunlun/app"
	"dakunlun/app/util"
	"dakunlun/configs"
	"flag"
	"fmt"
	"math/rand"
	"os"
	"os/signal"
	"time"

	"go.uber.org/zap"

	"github.com/timestee/goconf"
)

// @title dakunlun
// @version 1.0
// @description 接口文档
// @host 8.140.113.88:8080
func main() {
	// 异常处理
	defer func() {
		if reason := recover(); reason != nil {
			util.GetLogger().Error("main.recover", zap.Any("reason", reason))
		}
		util.ReleaseGoPool()
		//释放注册模块
		app.Free()
	}()
	// 读取配置文件
	config := flag.String("config", "dev", "dev/test/prod")
	flag.Parse()

	// 绑定配置文件
	goconf.MustResolve(configs.C, fmt.Sprintf("configs/%s.toml", *config))

	// 初始化logger
	util.MustInitLogger(configs.C.GetLogConfig())

	// 设置随机数seed
	rand.Seed(time.Now().UnixNano())

	// 注册数据库和redis
	app.RegisterBootStrap(app.NewDataModule(configs.C.GetMysqlConfig(), configs.C.GetRedisConfig()))
	// 注册事件
	app.RegisterBootStrap(app.NewEventModule())
	// 注册本地内存
	app.RegisterBootStrap(app.NewLocalCacheModule())
	// 注册lua脚本
	app.RegisterBootStrap(app.NewLockModule())
	// 注册cron
	app.RegisterBootStrap(app.NewCronModule())
	// 启动注册的模块
	app.Boot()

	quit := make(chan os.Signal)
	signal.Notify(quit, os.Interrupt)
	<-quit
	util.GetLogger().Info("Shutdown Cron After 10 sec...")
}
