package job

import (
	"dakunlun/app/util"
	"fmt"
)

// 竞技场分组
type ExampleJob struct {
	util.BaseJob
}

func NewExampleJob() *ExampleJob {
	return &ExampleJob{}
}

func (job *ExampleJob) GetName() string {
	return "ExampleJob"
}

func (job *ExampleJob) GetSpec() string {
	return "5 0 * * *"
}

func (job *ExampleJob) GetFunc() func() {
	return func() {
		fmt.Println("hello example")
	}
}
