package util

import (
	"crypto/md5"
	"encoding/hex"
)

func Md5Encrypt(v string) string {
	// 将密码转换为字节数组
	bytes := []byte(v)

	// 计算MD5哈希值
	hash := md5.Sum(bytes)

	// 将哈希值转换为16进制字符串
	hashString := hex.EncodeToString(hash[:])

	return hashString
}
