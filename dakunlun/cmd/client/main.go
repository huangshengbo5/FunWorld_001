package main

import (
	"dakunlun/app/constant"
	"dakunlun/app/service"
	"dakunlun/app/service/battle"
	"dakunlun/cmd/client/tester"
	"fmt"

	"encoding/json"
)

func main() {
	p1 := battle.NewTestFighter(nil, 0, 111111, "sb崔亮", 10000000, 1500, constant.SexMale, constant.RaceDevil)
	p2 := battle.NewTestFighter(nil, 0, 222222, "加班哥", 10000000, 2000, constant.SexMale, constant.RaceElf)

	b := &battle.BattleInfo{
		Attacker:    p1,
		Defender:    p2,
		AtkExt:      tester.NewTester01(p1.GetID()),
		DefExt:      tester.NewTester04(p2.GetID()),
		CurAttacker: nil,
		CurDefender: nil,
		FightType:   1,
		MaxTime:     120000,
		CurrentTime: 0,
		Background:  1,
		Report: &battle.BattleReport{
			Type: 1,
			Attacker: battle.ReportPlayer{
				ID:    p1.GetID(),
				Name:  p1.GetName(),
				Level: 50,
				HP:    979875,
			},
			Defender: battle.ReportPlayer{
				ID:    p2.GetID(),
				Name:  p2.GetName(),
				Level: 100,
				HP:    979875,
			},
			Background: 1,
		},
	}

	service.BattleService.Fight(b)

	b.Report.Show()
	v, _ := json.Marshal(b.Report)
	fmt.Println(string(v))
}
