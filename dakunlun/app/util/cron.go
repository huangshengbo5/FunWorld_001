package util

import (
	"context"
	"fmt"
	"sync"

	"go.uber.org/zap"

	"github.com/robfig/cron/v3"
)

var cronOnce sync.Once

var cm *CronManager

type CronManager struct {
	c *cron.Cron
}

type IJob interface {
	GetSpec() string
	GetName() string
	GetFunc() func()
}

func CreateJob(job IJob) *BaseJob {
	return &BaseJob{
		name: job.GetName(),
		spec: job.GetSpec(),
		run:  job.GetFunc(),
	}
}

type BaseJob struct {
	// 任务名
	name string
	// 执行周期
	spec string
	// 执行内容
	run func()
}

func (base *BaseJob) Run() {
	GetLogger().Info("job.run", zap.String("job", fmt.Sprintf("%s job start", base.name)))
	base.run()
	GetLogger().Info("job.run", zap.String("job", fmt.Sprintf("%s job end", base.name)))
}

func (base *BaseJob) Spec() string {
	return base.spec
}

func GetCronManager() *CronManager {
	cronOnce.Do(func() {
		cm = &CronManager{
			c: cron.New(),
		}
	})
	return cm
}

func (c *CronManager) Start() {
	c.c.Start()
}

func (c *CronManager) AddJob(j *BaseJob) {
	c.c.AddJob(j.Spec(), j)
}

func (c *CronManager) Stop() context.Context {
	return c.c.Stop()
}
