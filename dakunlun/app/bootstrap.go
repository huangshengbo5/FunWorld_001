package app

import (
	"dakunlun/app/event"
	"dakunlun/app/job"
	"dakunlun/app/listener"
	"dakunlun/app/util"
	"dakunlun/configs"
	"time"
)

var bootStrapPool []BootstrapModule

func RegisterBootStrap(boot BootstrapModule) {
	bootStrapPool = append(bootStrapPool, boot)
}

// 启动
func Boot() {
	for _, boot := range bootStrapPool {
		boot.OnInit()
	}
}

// 释放
func Free() {
	for _, boot := range bootStrapPool {
		boot.OnClose()
	}
}

type BootstrapModule interface {
	OnInit()
	OnClose()
}

type DataModule struct {
	mysqlConfig *configs.MysqlConfig
	redisConfig *configs.RedisConfig
}

// 数据模块
func NewDataModule(mysqlConfig *configs.MysqlConfig, redisConfig *configs.RedisConfig) *DataModule {
	return &DataModule{
		mysqlConfig: mysqlConfig,
		redisConfig: redisConfig,
	}
}

func (d *DataModule) OnInit() {
	// 初始化db
	util.MustInitMysql(&util.MysqlConf{
		Host:     d.mysqlConfig.MysqlHost,
		Port:     d.mysqlConfig.MysqlPort,
		DB:       d.mysqlConfig.MysqlDB,
		User:     d.mysqlConfig.MysqlUser,
		Password: d.mysqlConfig.MysqlPassword,
	})

	//err := util.GetDB().AutoMigrate(&entity.PassportEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserExtendEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserAnnalsEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserArenaEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserTechEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserHeroEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserMailEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserCrystalEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserExploreEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserCrystalEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.UserBuildingEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.HeroEquipDocEntity{})
	//util.PanicIfErr(err)
	//err = util.GetDB().AutoMigrate(&entity.HeroEquipEntity{})
	//util.PanicIfErr(err)
	// 初始化redis
	util.MustInitRedis(&util.RedisConf{
		Uri: d.redisConfig.RedisURI,
		Pwd: d.redisConfig.RedisPassword,
		DB:  d.redisConfig.RedisDB,
	})
}

func (d *DataModule) OnClose() {
	err := util.FreeMysqlClient()
	if err != nil {
		util.GetLogger().Error(err.Error())
	}

	err = util.FreeRedisClient()
	if err != nil {
		util.GetLogger().Error(err.Error())
	}
}

type EventModule struct {
}

// 事件模块
func NewEventModule() *EventModule {
	return &EventModule{}
}

func (e *EventModule) OnInit() {
	util.MustInitEventDispatcher()
	// 监听用户升级
	util.EventDispatcher().On(event.LevelUpEventName, util.NewEventListener(listener.BuildingOpenLisenter))
	// 监听建筑变化
	util.EventDispatcher().On(event.BuildingUpdateEventName, util.NewEventListener(listener.BuildingUpdateLisenter))
}

func (e *EventModule) OnClose() {

}

type LocalCacheModule struct {
}

// 本地内存
func NewLocalCacheModule() *LocalCacheModule {
	return &LocalCacheModule{}
}

func (l *LocalCacheModule) OnInit() {
	util.MustInitCache()
}

func (l *LocalCacheModule) OnClose() {

}

type LockModule struct {
}

// 本地内存
func NewLockModule() *LockModule {
	return &LockModule{}
}

func (l *LockModule) OnInit() {
	util.MustLockScript()
}

func (l *LockModule) OnClose() {

}

type CronModule struct {
	cron *util.CronManager
}

func NewCronModule() *CronModule {
	return &CronModule{
		cron: util.GetCronManager(),
	}
}

func (c *CronModule) OnInit() {
	c.cron.AddJob(util.CreateJob(job.NewArenaDivideGroupJob()))
	c.cron.AddJob(util.CreateJob(job.NewArenaSettlementJob()))
	c.cron.AddJob(util.CreateJob(job.NewExampleJob()))
	c.cron.Start()
}

func (c *CronModule) OnClose() {
	c.cron.Stop()
	time.Sleep(10 * time.Second)
}
