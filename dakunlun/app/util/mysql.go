package util

import (
	"database/sql"
	"fmt"
	"time"

	"gorm.io/driver/mysql"
	"gorm.io/gorm"
)

var gormDB *gorm.DB
var sqlDB *sql.DB

type MysqlConf struct {
	Host     string
	Port     int
	DB       string
	User     string
	Password string
}

func MustInitMysql(conf *MysqlConf) {
	var err error

	gormDB, err = gorm.Open(mysql.Open(fmt.Sprintf("%s:%s@tcp(%s:%v)/%s?charset=utf8mb4&parseTime=True&loc=Local",
		conf.User, conf.Password, conf.Host, conf.Port, conf.DB)), &gorm.Config{
		SkipDefaultTransaction: true,
		PrepareStmt:            true,
	})

	PanicIfErr(err)

	sqlDB, err = gormDB.DB()

	PanicIfErr(err)

	// SetMaxIdleConns 设置空闲连接池中连接的最大数量
	sqlDB.SetMaxIdleConns(10)

	// SetMaxOpenConns 设置打开数据库连接的最大数量
	sqlDB.SetMaxOpenConns(100)

	// SetConnMaxLifetime 设置了连接可复用的最大时间
	sqlDB.SetConnMaxLifetime(time.Hour)

}

func GetDB() *gorm.DB {
	return gormDB
}

func FreeMysqlClient() (err error) {
	err = sqlDB.Close()
	gormDB, sqlDB = nil, nil
	return
}
