package util

import (
	"crypto/hmac"
	"crypto/md5"
	"crypto/sha256"
	"encoding/base64"
	"encoding/hex"
	"fmt"
	"hash/crc32"
	"io"
	"regexp"
	"strconv"
	"strings"
	"time"

	"github.com/google/uuid"
)

func GenerateToken() string {
	return uuid.NewString()
}

/**
 * 对整数id进行可逆混淆
 */
func EncodeID(id uint32) uint32 {
	sid := (id & 0xff000000)
	sid += (id & 0x0000ff00) << 8
	sid += (id & 0x00ff0000) >> 8
	sid += (id & 0x0000000f) << 4
	sid += (id & 0x000000f0) >> 4
	sid ^= 11184810
	return sid
}

/**
 * 对通过EncodeID混淆的id进行还原
 */
func DecodeID(sid uint32) uint32 {
	sid ^= 11184810
	id := (sid & 0xff000000)
	id += (sid & 0x00ff0000) >> 8
	id += (sid & 0x0000ff00) << 8
	id += (sid & 0x000000f0) >> 4
	id += (sid & 0x0000000f) << 4
	return id
}

// 把header里string类型的混淆过的ID decode为真实的uint32类型ID
func DecodePlayerID(playerID string) (uint32PlayerID uint32, err error) {
	var uint64PlayerID uint64
	uint64PlayerID, err = strconv.ParseUint(playerID, 10, 32)

	if err != nil {
		return
	}

	uint32PlayerID = DecodeID(uint32(uint64PlayerID))

	return
}

func HashCode(s string) uint32 {
	v := crc32.ChecksumIEEE([]byte(s))
	return v
}

func SignWithSha256AndBase64(key []byte, in []byte) (sign string, err error) {
	mac := hmac.New(sha256.New, key)
	var n int
	n, err = mac.Write(in)
	if err != nil {
		return
	}

	if n > 0 {
		data := hex.EncodeToString(mac.Sum(nil))
		if len(data) > 0 {
			sign = base64.StdEncoding.EncodeToString([]byte(data))
		}
	}

	return
}

func MD5(in string) string {
	h := md5.New()
	n, err := io.WriteString(h, in)
	if err != nil {
		return ""
	}
	if n <= 0 {
		return ""
	}
	out := h.Sum(nil)
	return hex.EncodeToString(out)
	//return fmt.Sprintf("%X", out)
}

func ContainsUint8(list []uint8, val uint8) bool {
	for _, x := range list {
		if x == val {
			return true
		}
	}

	return false
}

func ContainsUint32(list []uint32, val uint32) bool {
	for _, x := range list {
		if x == val {
			return true
		}
	}

	return false
}

func ContainsInt(list []int, val int) bool {
	for _, x := range list {
		if x == val {
			return true
		}
	}

	return false
}

// 计算身份证号码的校验码
func calcCheckDigit(id string) string {
	var sum int
	weights := []int{7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2}
	for i, v := range id[:17] {
		num, _ := strconv.Atoi(string(v))
		sum += num * weights[i]
	}
	mod := sum % 11
	checkDigit := "10X98765432"[mod]
	return string(checkDigit)
}

// 验证身份证号码
func ValidateID(id string) (bool, int, error) {
	if len(id) != 18 {
		return false, 0, fmt.Errorf("身份证号码长度不正确。")
	}

	// 检查是否全为数字且最后一位可能是X或x
	if !regexp.MustCompile(`^[1-9]\d{5}(18|19|20)\d{2}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])\d{3}[\dXx]$`).MatchString(id) {
		return false, 0, fmt.Errorf("身份证号码格式不正确。")
	}

	// 校验码验证
	if calcCheckDigit(id[:17]) != strings.ToUpper(string(id[17])) {
		return false, 0, fmt.Errorf("请输入正确的身份证。")
	}

	// 假设地区码有效（实际应检查地区码表）
	// 这里仅检查长度和是否全为数字
	if !regexp.MustCompile(`^[1-9]\d{5}`).MatchString(id[:6]) {
		return false, 0, fmt.Errorf("地区码不合法。")
	}

	// 验证出生年月日是否合法
	birthYear, err := strconv.Atoi(id[6:10])
	if err != nil || birthYear < 1900 || birthYear > time.Now().Year() {
		return false, 0, fmt.Errorf("出生年份不合法。")
	}
	birthMonth, err := strconv.Atoi(id[10:12])
	if err != nil || birthMonth < 1 || birthMonth > 12 {
		return false, 0, fmt.Errorf("出生月份不合法。")
	}
	birthDay, err := strconv.Atoi(id[12:14])
	if err != nil || (birthMonth == 2 && birthDay > 29) ||
		(birthMonth%2 == 0 && birthMonth != 2 && birthDay > 30) ||
		(birthMonth%2 != 0 && birthDay > 31) {
		return false, 0, fmt.Errorf("出生日期不合法。")
	}

	// 计算年龄
	birthDate, err := time.Parse("20060102", id[6:14])
	if err != nil {
		return false, 0, fmt.Errorf("无法解析出生日期。")
	}
	age := time.Now().Year() - birthDate.Year()
	if time.Now().Before(birthDate.AddDate(age, 0, 0)) {
		age--
	}

	return true, age, nil
}
