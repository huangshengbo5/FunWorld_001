package controller

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/service"
	"dakunlun/app/service/battle"
	"dakunlun/app/service/data"
	"dakunlun/app/util"
	"fmt"
	"net/http"
	"strings"

	"github.com/spf13/cast"

	"github.com/gin-gonic/gin"
)

// @Summary check server's healthy
// @Accept  json
// @Produce  json
// @Param message query string true "ping"
// @Success 200 {string} string	"pong"
// @Router /ping [get]
func Ping(c *gin.Context) {
	c.JSON(200, gin.H{
		"message": "pong",
	})
}

func FlushCache(c *gin.Context) {
	util.DataCache().Purge()
	c.JSON(200, gin.H{
		"message": "关我屁事",
	})
}

// @Summary 关卡挑战
// @Accept  json
// @Produce  json
// @Param GameUid header string true "用户ID"
// @Param GameToken header string true "令牌"
// @Param SeqId header int true "请求序号"
// @Param uid query int true "用户ID"
// @Param campaignID query int true "关卡ID"
// @Success 200 {object} msg.TestPveResponse
// @Router /pve [post]
func Pve(c *gin.Context) {
	req := &msg.TestPveRequest{}

	if c.Request.Method == http.MethodPost {
		if err := c.ShouldBind(req); err != nil {
			service.RenderWithAppError(c, err)
			return
		}
		fmt.Println("aaa")
	} else {
		req.Uid = cast.ToUint32(c.Query("uid"))
		req.CampaignID = cast.ToUint32(c.Query("campaignID"))
		fmt.Println("bbb")
	}

	userEntity, err := service.UserService.GetUserByID(req.Uid)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	//TODO 获取关卡数据 看关卡数据是否为空和前置关卡是否完成
	campaignEntity, err := data.CampaignService.GetCampaignByID(req.CampaignID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var attacker, defender constant.IFighter
	var attackerRuntime, defenderRuntime *battle.FighterRuntime
	attacker, attackerRuntime, err = service.BattleService.NewPlayer(userEntity, false)
	defender, defenderRuntime, err = service.BattleService.NewNpc(campaignEntity.NpcID)

	// 开始战斗req.CampaignID或者 campaignEntity.NextID
	battle := &battle.BattleInfo{
		Attacker:   attacker,
		Defender:   defender,
		AtkExt:     attackerRuntime,
		DefExt:     defenderRuntime,
		FightType:  constant.FightTypeCampaign,
		MaxTime:    constant.CampaignMaxTime,
		Background: campaignEntity.BackgroundID,
	}

	err = service.BattleService.Fight(battle)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	r := battle.Report.Show()

	rsp := &msg.TestPveResponse{
		Win:    (battle.Result == service.ResultAttackerWin),
		Report: battle.Report,
		Debug:  r,
	}

	service.RenderSuccess(c, rsp)
}

func Reward(c *gin.Context) {
	req := &msg.TestRewardRequest{}

	req.Name = c.Query("name")

	passportEntity, err := service.PassportService.GetPassportByName(req.Name)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	_, err = service.UserMailService.AddMail(passportEntity.ID, entity.MailIDTest, nil,
		entity.RewardStrings{"1_0_200000000_1", "2_0_200000000_1", "17_0_200000000_1"})

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	c.JSON(200, gin.H{
		"message": "关我屁事",
	})
}

func SendReward(c *gin.Context) {
	req := &msg.TestSendRewardRequest{}
	req.Name = c.Query("name")
	req.Rewards = c.Query("rewards")
	passportEntity, err := service.PassportService.GetPassportByName(req.Name)
	userEntity, err := service.UserService.GetUserByID(passportEntity.ID)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	var rewards = make(entity.RewardStrings, 0)
	rewards = strings.Split(req.Rewards, ";")

	//发放奖励
	_, err = service.RewardService.SendRewards(userEntity, rewards, service.SourceDefault, nil)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	c.JSON(200, gin.H{
		"message": "关我屁事",
	})
}



func SetResource(c *gin.Context) {
	name := c.Query("name")
	gem := c.Query("gem")
	gold := c.Query("gold")

	passportEntity, err := service.PassportService.GetPassportByName(name)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity, err := service.UserService.GetUserByID(passportEntity.ID)

	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	userEntity.Gold = cast.ToUint64(gold)
	userEntity.Diamond = cast.ToUint32(gem)

	err = service.UserService.UpdateUser(userEntity)
	if err != nil {
		service.RenderWithAppError(c, err)
		return
	}

	c.JSON(200, gin.H{
		"message": "关我屁事",
	})
}
