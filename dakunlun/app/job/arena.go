package job

import (
	"dakunlun/app/service"
	"dakunlun/app/util"
)

// 竞技场分组
type ArenaDivideGroupJob struct {
	util.BaseJob
}

func NewArenaDivideGroupJob() *ArenaDivideGroupJob {
	return &ArenaDivideGroupJob{}
}

func (job *ArenaDivideGroupJob) GetName() string {
	return "ArenaDivideGroupJob"
}

func (job *ArenaDivideGroupJob) GetSpec() string {
	return "5 0 * * 4" //每周4  00:05分分组
}

func (job *ArenaDivideGroupJob) GetFunc() func() {
	return func() {
		service.UserArenaService.InitArena()
	}
}

// 竞技场结算
type ArenaSettlementJob struct {
	util.BaseJob
}

func NewArenaSettlementJob() *ArenaSettlementJob {
	return &ArenaSettlementJob{}
}

func (job *ArenaSettlementJob) GetName() string {
	return "ArenaSettlementJob"
}

func (job *ArenaSettlementJob) GetSpec() string {
	return "5 0 * * 1" //每周1  00:05分分组
}

func (job *ArenaSettlementJob) GetFunc() func() {
	return func() {
		service.UserArenaService.RankCalculator()
	}
}
