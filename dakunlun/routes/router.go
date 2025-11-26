package routes

import (
	"dakunlun/app/constant"
	"dakunlun/app/controller"
	"dakunlun/app/middleware"
	"dakunlun/configs"

	_ "dakunlun/docs"

	swaggerFiles "github.com/swaggo/files"
	ginSwagger "github.com/swaggo/gin-swagger"

	"github.com/gin-gonic/gin"
)

func InitRoutes() *gin.Engine {

	r := gin.New()
	r.Use(gin.Logger())
	r.Use(gin.Recovery())
	//zap.ReplaceGlobals(util.GetLogger())
	//添加一个ginzap中间件，它：
	//记录所有请求，如组合的访问和错误日志。
	//记录到stdout。
	//RFC3339，UTC时间格式。
	//r.Use(ginzap.Ginzap(util.GetLogger(), time.RFC3339, true))

	//将所有死机记录到错误日志中
	//stack表示是否输出堆栈信息。
	//r.Use(ginzap.RecoveryWithZap(util.GetLogger(), true))
	// 使用跨域中间件
	r.Use(middleware.Cors(middleware.WithAllowHeaders([]string{constant.HeaderGameToken, constant.HeaderGameUid,
		constant.HeaderSeqID})))

	// 请求保续中间件
	//seqCheckMid := middleware.SeqCheck(&middleware.SeqConf{
	//	SessionIDProvider: middleware.GetSessionID,
	//	ExpireTime:        configs.C.ReqcheckTtl,
	//})

	// 鉴权中间件
	authMid := middleware.Auth()
	accessLogMid := middleware.AccessLog()

	r.Static("/public", "./public") // 静态文件服务
	//r.LoadHTMLGlob("views/**/*")    // 载入html模板目录

	// web路由
	r.GET("/ping", controller.Ping)        // 服务器健康检查
	r.GET("/flush", controller.FlushCache) // GOPOOL FLUSH
	r.POST("/pve", controller.Pve)         // 测试用户VS关卡
	r.GET("/pve", controller.Pve)          // 测试用户VS关卡
	r.GET("/reward", controller.Reward)    // 测试用户VS关卡
	r.GET("/send", controller.SendReward)  // 测试用户VS关卡
	r.GET("/set", controller.SetResource)

	loginGroup := r.Group("/login").Use()
	{
		loginGroup.POST("/login/native", controller.LoginNative) //原生登录
		loginGroup.POST("/login/regist", controller.Regist)      //原生登录
	}

	apiGroup := r.Group("/api").Use(authMid, accessLogMid)
	{
		apiGroup.POST("/building/list", controller.BuildingList)           // 建筑列表
		apiGroup.POST("/building/upgrade", controller.BuildingUpgrade)     // 建筑升级
		apiGroup.POST("/tech/list", controller.TechList)                   // 科技列表
		apiGroup.POST("/tech/upgrade", controller.TechUpgrade)             // 科技升级
		apiGroup.POST("/crystal/list", controller.CrystalList)             // 水晶列表
		apiGroup.POST("/crystal/upgrade", controller.CrystalUpgrade)       // 水晶升级
		apiGroup.POST("/apocalypse/show", controller.ApocalypseShow)       // 天启界面
		apiGroup.POST("/apocalypse/attack", controller.ApocalypseAttack)   // 挑战天启
		apiGroup.POST("/apocalypse/receive", controller.ApocalypseReceive) // 领取天启奖励

		apiGroup.POST("/lobby/user", controller.UserInfo)
		apiGroup.POST("/lobby/change/avatar", controller.ChangeAvatar)               //修改头像
		apiGroup.POST("/lobby/change/name", controller.ChangeName)                   //修改用户名
		apiGroup.POST("/lobby/guide/step", controller.GuideStep)                     //新手引导步骤号设置
		apiGroup.POST("/lobby/guide/battle", controller.GuideBattle)                 //新手引导战斗
		apiGroup.POST("/lobby/onlinereward/show", controller.OnlineRewardShow)       // 在线奖励展示
		apiGroup.POST("/lobby/onlinereward/receive", controller.OnlineRewardReceive) // 在线奖励领取
		apiGroup.POST("/lobby/monster/show", controller.MonsterShow)                 // 怪物入侵界面
		apiGroup.POST("/lobby/monster/attack", controller.MonsterAttack)             // 怪物入侵挑战
		apiGroup.POST("/lobby/monster/receive", controller.MonsterReceive)           // 领取怪物入侵奖励
		apiGroup.POST("/lobby/alchemy/start", controller.AlchemyStart)               // 开始制药
		apiGroup.POST("/lobby/alchemy/clear", controller.AlchemyClear)               // 清除制药cd
		apiGroup.POST("/lobby/system/config", controller.SystemConfig)               // 系统配置
		apiGroup.POST("/lobby/annals/show", controller.AnnalsShow)                   // 成就页面
		apiGroup.POST("/lobby/annals/receive", controller.AnnalsReceive)             // 成就领奖
		apiGroup.POST("/lobby/businessman/receive", controller.BusinessManReceive)   // 商人领奖
		apiGroup.POST("/lobby/cast", controller.Cast)
		apiGroup.POST("/lobby/buy", controller.Buy) // 商城购买

		apiGroup.POST("/campaign/attack", controller.CampaignAttack)   // 挑战关卡
		apiGroup.POST("/campaign/receive", controller.CampaignReceive) // 领取关卡奖励

		apiGroup.POST("/hero/list", controller.HeroList)        // 伙伴列表
		apiGroup.POST("/hero/join", controller.HeroJoin)        // 伙伴出战
		apiGroup.POST("/hero/unload", controller.HeroUnload)    // 伙伴休息
		apiGroup.POST("/hero/upgrade", controller.HeroUpgrade)  // 主角、伙伴升级
		apiGroup.POST("/hero/evolve", controller.HeroEvolve)    // 主角、伙伴锻魂
		apiGroup.POST("/hero/skin/get", controller.HeroSkinGet) // 看皮肤广告
		apiGroup.POST("/hero/skin/use", controller.HeroSkinUse) // 使用皮肤

		apiGroup.POST("/equip/list", controller.EquipList)           // 装备列表
		apiGroup.POST("/equip/doc", controller.EquipDoc)             // 装备图鉴
		apiGroup.POST("/equip/receive", controller.EquipReceive)     // 领取图鉴奖励
		apiGroup.POST("/equip/use", controller.EquipUse)             // 装备装配
		apiGroup.POST("/equip/unload", controller.EquipUnload)       // 装备装配
		apiGroup.POST("/equip/decompose", controller.EquipDecompose) // 装备分解
		apiGroup.POST("/equip/upgrade", controller.EquipUpgrade)     // 装备升级
		apiGroup.POST("/equip/forge", controller.EquipForge)         // 装备锻造

		apiGroup.POST("/arena/signup", controller.ArenaSignUp) // 装备升级
		apiGroup.POST("/arena/list", controller.ArenaList)     // 装备升级
		apiGroup.POST("/arena/attack", controller.ArenaAttack) // 装备升级

		apiGroup.POST("/mail/list", controller.MailList)             // 邮件列表
		apiGroup.POST("/mail/read", controller.MailRead)             // 邮件阅读
		apiGroup.POST("/mail/receive", controller.MailReceive)       // 邮件领取附件
		apiGroup.POST("/mail/receiveall", controller.MailReceiveAll) // 邮件全部领取附件
		apiGroup.POST("/mail/removeall", controller.MailRemoveAll)   // 邮件全部删除

		apiGroup.POST("/ads/start", controller.AdsStart)     // 广告开始
		apiGroup.POST("/ads/receive", controller.AdsReceive) // 广告领奖
		apiGroup.POST("/ads/add/buff", controller.AddBuff)   // 图腾

		apiGroup.POST("/gm/incrgold", controller.GmIncrGold) // 增加金币
		apiGroup.POST("/gm/addequip", controller.GmAddEquip) // 增加装备

		apiGroup.POST("/explore/receive", controller.ExploreReceive) // 探索奖励
		apiGroup.POST("/explore/list", controller.ExploreList)       // 探索列表
		apiGroup.POST("/explore/set", controller.ExploreSet)         // 探索更换
	}
	url := ginSwagger.URL(configs.C.SwaggerJson)
	r.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler, url))

	return r
}
