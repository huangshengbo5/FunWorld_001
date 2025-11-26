package util

import (
	"dakunlun/configs"
	"os"
	"runtime"

	"go.uber.org/zap"
	"go.uber.org/zap/zapcore"
	"gopkg.in/natefinch/lumberjack.v2"
)

var logger *zap.Logger

func DirectorySeparator() string {
	if runtime.GOOS == "windows" {
		return "\\"
	} else {
		return "/"
	}
}

func MustInitLogger(cfg *configs.LogConfig) {
	workDir, err := os.Getwd()
	PanicIfErr(err)
	hook := lumberjack.Logger{
		Filename:   workDir + DirectorySeparator() + "logs" + DirectorySeparator() + cfg.LogFile, // 日志文件路径
		MaxSize:    cfg.LogMaxSize,                                                               // 每个日志文件保存的最大尺寸 单位：M
		MaxBackups: cfg.LogMaxBackups,                                                            // 日志文件最多保存多少个备份
		MaxAge:     cfg.LogMaxAge,                                                                // 文件最多保存多少天
		Compress:   cfg.LogCompress,                                                              // 是否压缩
		LocalTime:  true,                                                                         // 是否使用本地时间
	}

	encoderConfig := zapcore.EncoderConfig{
		TimeKey:        "time",
		LevelKey:       "level",
		NameKey:        "logger",
		CallerKey:      "linenum",
		MessageKey:     "msg",
		StacktraceKey:  "stacktrace",
		LineEnding:     zapcore.DefaultLineEnding,
		EncodeLevel:    zapcore.LowercaseLevelEncoder,  // 小写编码器
		EncodeTime:     zapcore.RFC3339TimeEncoder,     // 时间格式
		EncodeDuration: zapcore.SecondsDurationEncoder, //
		EncodeCaller:   zapcore.FullCallerEncoder,      // 全路径编码器
		EncodeName:     zapcore.FullNameEncoder,
	}

	// 设置日志级别
	atomicLevel := zap.NewAtomicLevel()
	atomicLevel.SetLevel(zap.InfoLevel)

	core := zapcore.NewCore(
		zapcore.NewJSONEncoder(encoderConfig),                                           // 编码器配置
		zapcore.NewMultiWriteSyncer(zapcore.AddSync(os.Stdout), zapcore.AddSync(&hook)), // 打印到控制台和文件
		atomicLevel, // 日志级别
	)

	// 开启开发模式，堆栈跟踪
	caller := zap.AddCaller()
	// 开启文件及行号
	development := zap.Development()
	// 设置初始化字段
	//filed := zap.Fields(zap.String("serviceName", "serviceName"))
	// 构造日志
	logger = zap.New(core, caller, development)
}

func GetLogger() *zap.Logger {
	return logger
}
