package util

import (
	"fmt"
	"math"
	"math/rand"
	"sort"
	"strconv"
	"time"
)

func Round(input interface{}) (int, error) {

	var inputToFloat64 float64
	switch input := input.(type) {
	case int:
		inputToFloat64 = float64(input)
	case int8:
		inputToFloat64 = float64(input)
	case int16:
		inputToFloat64 = float64(input)
	case int32:
		inputToFloat64 = float64(input)
	case int64:
		inputToFloat64 = float64(input)
	case uint8:
		inputToFloat64 = float64(input)
	case uint16:
		inputToFloat64 = float64(input)
	case uint32:
		inputToFloat64 = float64(input)
	case uint64:
		inputToFloat64 = float64(input)
	case float32:
		inputToFloat64 = float64(input)
	case float64:
		inputToFloat64 = float64(input)
	default:
		return 0, fmt.Errorf("wrong input : %v", input)
	}

	return int(math.Floor(inputToFloat64 + 0.5)), nil
}

func AbsInt(v int) int {
	if v < 0 {
		return -v
	}
	return v
}

// 计算概率
func JudgeProbability(ratio int) bool {
	randomNum := RandN(1, 10001)
	return randomNum <= ratio
}

// [min,max)
func RandN(min, max int) int {
	if min == max {
		return min
	}

	if min > max {
		min, max = max, min
	}

	n := rand.Intn(max - min)

	return min + n
}

// [min,max]
func RandUint32(min, max uint32) uint32 {
	return rand.Uint32()%(max+1-min) + min
}

// [min,max]
func RandInt(min, max int) int {
	return rand.Int()%(max+1-min) + min
}

type weightItem struct {
	Weight int
	Index  int
}

type weightMapSlice []weightItem

func (p weightMapSlice) Len() int           { return len(p) }
func (p weightMapSlice) Less(i, j int) bool { return p[i].Weight < p[j].Weight }
func (p weightMapSlice) Swap(i, j int)      { p[i], p[j] = p[j], p[i] }

// 根据权重map返回结果
func GetResultByWeightMap(weightMap map[int]int) (result int) {

	var totalWeight = 0
	weightSlice := make(weightMapSlice, 0, len(weightMap))
	for k, v := range weightMap {
		weightSlice = append(weightSlice, weightItem{
			Weight: v,
			Index:  k,
		})
		totalWeight += v
	}
	sort.Sort(weightSlice)
	x := RandN(0, totalWeight)

	var sum int = 0

	for _, item := range weightSlice {
		sum += item.Weight
		if x < sum {
			return item.Index
		}
	}

	return
}

func MaxInt64(v1 int64, v2 int64) int64 {
	if v1 > v2 {
		return v1
	}
	return v2
}

func MinInt64(v1 int64, v2 int64) int64 {
	if v1 < v2 {
		return v1
	}
	return v2
}

func MinInt(v1 int, v2 int) int {
	if v1 < v2 {
		return v1
	}
	return v2
}

func MinFloat(v1 float64, v2 float64) float64 {
	if v1 < v2 {
		return v1
	}
	return v2
}

func MaxFloat(v1 float64, v2 float64) float64 {
	if v1 > v2 {
		return v1
	}
	return v2
}

// 整数向上向下取整，其余低位码为0
// 入参：digit是需要取整数字，num是要保留的整数位
// 返回值：向上取整、向下取整
func IntRound(digit int, num int) (int, int) {
	lenDigit := len(strconv.Itoa(digit))
	if lenDigit <= num {
		return digit, digit
	}

	// 差值位取模
	mod := int(math.Pow10(lenDigit - num))
	tmp := digit / mod

	return tmp * mod, (tmp + 1) * mod
}

func Shuffle(slice []int) {
	r := rand.New(rand.NewSource(time.Now().Unix()))
	for len(slice) > 0 {
		n := len(slice)
		randIndex := r.Intn(n)
		slice[n-1], slice[randIndex] = slice[randIndex], slice[n-1]
		slice = slice[:n-1]
	}
}

// 整数向下取整
// 入参：digit是需要取整数字，num是要精确位数
// 比如：digit=123456789，mum=4，取整到万位是123450000
func Uint64RoundDown(digit uint64, num uint64) uint64 {
	if digit <= num {
		return digit
	}
	return digit - digit%num
}

// 整数向上取整
// 入参：digit是需要取整数字，num是要精确位数
// 比如：digit=123456789，mum=4，取整到万位是123450000
func Uint64RoundUp(digit uint64, num uint64) uint64 {
	if digit <= num {
		return digit
	}

	mod := digit % num
	if mod == 0 {
		return digit
	}

	return digit - digit%num + num
}

// 四舍五入取整
// 入参：digit是需要取整数字，num是要精确位数
// 比如：digit=123456789，mum=4，取整到万位是123450000
func Uint64Round(digit uint64, num uint64) uint64 {
	if digit <= num {
		return digit
	}

	mod := digit % num
	if mod == 0 {
		return digit
	}

	var result uint64
	// 五入
	if mod >= num/2 {
		result = digit - mod + num
	} else {
		// 四舍
		result = digit - mod
	}

	return result
}
