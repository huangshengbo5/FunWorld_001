package util

import "fmt"

type CustomErrorCode = int

// go原生err
const ErrorCodeGo CustomErrorCode = 100
const ErrorConfigError CustomErrorCode = 101

// 本应该客户端拦截的请求 发到了服务端
const ErrorCodeHack CustomErrorCode = 1000
const ErrorCodePassportNameIsEmpty CustomErrorCode = 1001
const ErrorCodeGenUUID CustomErrorCode = 1002
const ErrorCodeTokenExpire CustomErrorCode = 1003
const ErrorCodeUserNotExist CustomErrorCode = 1004
const ErrorCodeLowLevel CustomErrorCode = 1005
const ErrorCodeLowCampaignNum CustomErrorCode = 1006
const ErrorCodeGoldNotEnough CustomErrorCode = 1007
const ErrorCodeDiamondNotEnough CustomErrorCode = 1008
const ErrorCodeHeroLevelNotEnough CustomErrorCode = 1009
const ErrorCodeHeroEquipNumFull CustomErrorCode = 1010
const ErrorCodeSoulCrystalNotEnough CustomErrorCode = 1011
const ErrorCodeSuperbiaStoneNotEnough CustomErrorCode = 1012
const ErrorCodeInvidiaStoneNotEnough CustomErrorCode = 1013
const ErrorCodeAcediaStoneNotEnough CustomErrorCode = 1014
const ErrorCodeGulaStoneNotEnough CustomErrorCode = 1015
const ErrorCodeAvaritiaStoneNotEnough CustomErrorCode = 1016
const ErrorCodeLuxuriaStoneNotEnough CustomErrorCode = 1017
const ErrorCodeIraStoneNotEnough CustomErrorCode = 1018
const ErrorCodeTreasureAnimaNotEnough CustomErrorCode = 1019
const ErrorCodeRewardStringWrong CustomErrorCode = 1020
const ErrorCodeFighterEmpty CustomErrorCode = 1021
const ErrorCodeTowerNotOpen CustomErrorCode = 1022
const ErrorCodeInFighting CustomErrorCode = 1023
const ErrorCodeRankInRebuild CustomErrorCode = 1024
const ErrorCodeNoAlchemyData CustomErrorCode = 1025
const ErrorCodeBenYuanNotEnough CustomErrorCode = 1026
const ErrorCodeQianNengNotEnough CustomErrorCode = 1027
const ErrorNameIsUsed CustomErrorCode = 1028
const ErrorCodeBusinessManNotOpen CustomErrorCode = 1029
const ErrorCodeElementNotEnough CustomErrorCode = 1030
const ErrorCodeAccountExpire CustomErrorCode = 1031
const ErrorCodeBookNotEnough CustomErrorCode = 1032
const ErrorCodeTokenIsInvalid CustomErrorCode = 1033
const ErrorCodePasswordIsInvalid CustomErrorCode = 1034
const ErrorCodeBuyError CustomErrorCode = 1035
const ErrorCodeTimeError1 CustomErrorCode = 1036
const ErrorCodeTimeError2 CustomErrorCode = 1037

// TODO 后期仍配置文件里
var (
	codeMap = map[CustomErrorCode]string{
		ErrorCodeGo:                     "服务器异常 [%v]",
		ErrorConfigError:                "服务器配置异常 [%v]",
		ErrorCodeHack:                   "客户端未检验 [%v]",
		ErrorCodePassportNameIsEmpty:    "账号为空",
		ErrorCodeGenUUID:                "生成用户错误",
		ErrorCodeTokenExpire:            "登录状态过期",
		ErrorCodeUserNotExist:           "用户不存在",
		ErrorCodeLowLevel:               "等级不足",
		ErrorCodeLowCampaignNum:         "不满足关卡条件",
		ErrorCodeGoldNotEnough:          "金币不足",
		ErrorCodeDiamondNotEnough:       "钻石不足",
		ErrorCodeHeroLevelNotEnough:     "英雄等级不足",
		ErrorCodeHeroEquipNumFull:       "装备达到上限",
		ErrorCodeSoulCrystalNotEnough:   "宝物精华数量不足",
		ErrorCodeSuperbiaStoneNotEnough: "炽焰之源数量不足",
		ErrorCodeInvidiaStoneNotEnough:  "跃水之源数量不足",
		ErrorCodeAcediaStoneNotEnough:   "飓风之源数量不足",
		ErrorCodeGulaStoneNotEnough:     "大地之源数量不足",
		ErrorCodeAvaritiaStoneNotEnough: "光明之源数量不足",
		ErrorCodeLuxuriaStoneNotEnough:  "黑暗之源数量不足",
		ErrorCodeIraStoneNotEnough:      "时空之源数量不足",
		ErrorCodeTreasureAnimaNotEnough: "特性宝珠数量不足",
		ErrorCodeRewardStringWrong:      "奖励异常",
		ErrorCodeFighterEmpty:           "玩家为空",
		ErrorCodeTowerNotOpen:           "建筑未开启",
		ErrorCodeInFighting:             "正在战斗中",
		ErrorCodeRankInRebuild:          "正在重新匹配",
		ErrorCodeNoAlchemyData:          "数据错误",
		ErrorCodeBenYuanNotEnough:       "本源之力数量不足",
		ErrorCodeQianNengNotEnough:      "潜能之力数量不足",
		ErrorNameIsUsed:                 "名字已存在",
		ErrorCodeBusinessManNotOpen:     "商人未开启",
		ErrorCodeElementNotEnough:       "资源数量不足",
		ErrorCodeAccountExpire:          "请重新登录",
		ErrorCodeBookNotEnough:          "技能书数量不足",
		ErrorCodeTokenIsInvalid:         "用户名错误",
		ErrorCodePasswordIsInvalid:      "密码错误",
		ErrorCodeBuyError:               "购买失败",
		ErrorCodeTimeError1:             "探索时间大于5秒才可进行领取",
		ErrorCodeTimeError2:             "法阵开启时间时间已达上限",
		2000:                            "不符合登录时间",
		2001:                            "奖励已领取",
	}
)

type AppError struct {
	ErrCode int
	ErrMsg  string
}

func (c *AppError) Error() string {
	return c.ErrMsg
}

func (c *AppError) Code() int {
	return c.ErrCode
}

func GetErrorMessage(errorCode CustomErrorCode, args ...interface{}) string {
	return fmt.Sprintf(codeMap[errorCode], args...)
}

func NewAppError(errorCode CustomErrorCode, args ...interface{}) *AppError {
	format := codeMap[errorCode]

	return &AppError{
		ErrCode: errorCode,
		ErrMsg:  fmt.Sprintf(format, args...),
	}
}
