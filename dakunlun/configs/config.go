package configs

import (
	"time"

	"github.com/timestee/goconf"
)

type RedisConfig struct {
	RedisURI          string
	RedisPassword     string
	RedisDB           int
	RedisClusterAddrs []string
}

type MysqlConfig struct {
	MysqlHost     string
	MysqlPort     int
	MysqlDB       string
	MysqlAgent    int
	MysqlUser     string
	MysqlPassword string
}

type LogConfig struct {
	LogFile       string
	LogMaxSize    int
	LogMaxBackups int
	LogMaxAge     int
	LogCompress   bool
}

type Config struct {
	goconf.AutoOptions
	HttpPort    int    `default:"8080"`
	Env         string `default:"dev"`
	Platform    string `default:"native"`
	ReqcheckTtl time.Duration
	SwaggerJson string `default:"http://127.0.0.1:8080/swagger/doc.json"`

	RedisConfig
	MysqlConfig
	LogConfig
}

var C = &Config{}

func (c *Config) GetRedisConfig() *RedisConfig {
	return &c.RedisConfig
}

func (c *Config) GetMysqlConfig() *MysqlConfig {
	return &c.MysqlConfig
}

func (c *Config) GetLogConfig() *LogConfig {
	return &c.LogConfig
}
