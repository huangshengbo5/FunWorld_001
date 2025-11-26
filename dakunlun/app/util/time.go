package util

import "github.com/golang-module/carbon"

func GetYearWeek() int {
	return Carbon().Now().Year()*100 + Carbon().Now().WeekOfYear()
}

func GetPrevYearWeek() int {
	return Carbon().Now().Year()*100 + Carbon().Now().AddDays(-2).WeekOfYear()
}

func Carbon() carbon.Carbon {
	return carbon.SetTimezone(carbon.Shanghai)
}
