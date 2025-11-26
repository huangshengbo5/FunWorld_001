package service

import (
	"dakunlun/app/constant"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/util"
	"fmt"
	"net/http"
	"strconv"
	"time"

	"github.com/gin-gonic/gin"
)

// 输出正常response
func RenderSuccess(c *gin.Context, data interface{}) {
	renderJson(c, 0, "", data)
}

// 输出异常
func RenderError(c *gin.Context, code int, message string, args ...interface{}) {
	renderJson(c, code, fmt.Sprintf(message, args...), nil)
}

// 使用异常输出
func RenderWithAppError(c *gin.Context, err error) {
	switch e := err.(type) {
	case *util.AppError:
		renderJson(c, e.Code(), e.ErrMsg, nil)
	case error:
		renderJson(c, util.ErrorCodeGo, e.Error(), nil)
	}
}

// 中间件用
func AbortWithError(c *gin.Context, code int, message string) {
	if seqID, exist := GetSeqIDInContext(c); exist {
		c.Header(constant.HeaderSeqID, strconv.Itoa(seqID))
	}

	c.AbortWithStatusJSON(http.StatusOK, packJsonData(c, code, message, nil))
}

func renderJson(c *gin.Context, code int, message string, data interface{}) {
	if seqID, exist := GetSeqIDInContext(c); exist {
		c.Header(constant.HeaderSeqID, strconv.Itoa(seqID))
	}
	rsp := packJsonData(c, code, message, data)
	c.JSON(http.StatusOK, rsp)
	c.Set(constant.CtxConstRsp, rsp)
}

func packJsonData(c *gin.Context, code int, message string, data interface{}) gin.H {
	return gin.H{
		"code":    code,
		"msg":     message,
		"data":    data,
		"sysTime": time.Now().Unix(),
	}
}

func GetUidInContext(c *gin.Context) (uint32, bool) {
	uid, exist := c.Get(constant.CtxConstUid)
	if exist {
		return uid.(uint32), true
	}

	return 0, false
}

func GetUserEntityInContext(c *gin.Context) (*entity.UserEntity, error) {
	uid, exist := GetUidInContext(c)
	if !exist {
		return nil, util.NewAppError(util.ErrorCodeUserNotExist)
	}

	return UserService.GetUserByID(uid)
}

func GetTokenInContext(c *gin.Context) (string, bool) {
	token, exist := c.Get(constant.CtxConstToken)
	if exist {
		return token.(string), true
	}

	return "", false
}

func GetSeqIDInContext(c *gin.Context) (int, bool) {
	seqID, exist := c.Get(constant.CtxConstSeqID)
	if exist {
		return seqID.(int), true
	}

	return 0, false
}

func PackUserResponse(userEntity *entity.UserEntity) *msg.User {
	userEntity.Extra.Flush()
	return &msg.User{
		Name:           userEntity.GetName(false),
		Avatar:         userEntity.Avatar,
		Level:          userEntity.Level,
		GuideStep:      userEntity.Ftue,
		Gold:           userEntity.CurGold(),
		GoldFlushIn:    userEntity.GoldFlushIn,
		GoldIncrement:  userEntity.GoldIncrement(),
		GoldBuffEndIn:  userEntity.GoldBuffEndTime,
		CampaignNum:    userEntity.CampaignNum,
		Diamond:        userEntity.Diamond,
		MainHeroID:     userEntity.MainHeroID,
		SubHeroID:      userEntity.SubHeroID,
		SoulCrystal:    userEntity.Resource.SoulCrystal,
		TreasureAnima:  userEntity.Resource.TreasureAnima,
		SuperbiaStone:  userEntity.Resource.SuperbiaStone,
		InvidiaStone:   userEntity.Resource.InvidiaStone,
		AcediaStone:    userEntity.Resource.AcediaStone,
		GulaStone:      userEntity.Resource.GulaStone,
		AvaritiaStone:  userEntity.Resource.AvaritiaStone,
		LuxuriaStone:   userEntity.Resource.LuxuriaStone,
		IraStone:       userEntity.Resource.IraStone,
		BenYuan:        userEntity.Resource.BenYuan,
		QianNeng:       userEntity.Resource.QianNeng,
		Element:        userEntity.Resource.Element,
		Book:           userEntity.Resource.Book,
		CastID:         userEntity.CastEffect.CastID,
		Type:           userEntity.Extra.AccountType,
		SingleLimit:    userEntity.Extra.SingleLimit,
		MonthLimit:     userEntity.Extra.MonthLimit,
		BuyRefreshTime: userEntity.Extra.RefreshTime,
		BuyTotal:       userEntity.Extra.Total,
	}
}

func PackPlayer(userEntity *entity.UserEntity) *msg.Player {
	return &msg.Player{
		ID:     userEntity.HidingUid(),
		Name:   userEntity.GetName(false),
		Avatar: userEntity.Avatar,
		Level:  userEntity.Level,
	}
}

func PackUserExtendResponse(userExtendEntity *entity.UserExtendEntity) *msg.UserExtend {
	return &msg.UserExtend{
		OnlineReward: msg.OnlineReward{
			NextReceiveTime: userExtendEntity.OnlineReward.NextReceiveTime,
			RemainAdsNum:    userExtendEntity.OnlineReward.RemainAdsNum,
			PayNum:          userExtendEntity.OnlineReward.PayNum,
			RewardIDs:       userExtendEntity.OnlineReward.RewardIDs,
		},
		Campaign: msg.Campaign{
			LastCampainID:   userExtendEntity.CampaignID,
			LastCampainTime: userExtendEntity.CampaignTime,
			PreCampainID:    userExtendEntity.CampaignOldID,
		},
		ArenaRemainNum: userExtendEntity.ArenaRemainNum,
		ArenaIsSign:    userExtendEntity.ArenaIsSign(),
		Alchemy: msg.Alchemy{
			NextReceiveTime: userExtendEntity.Alchemy.NextReceiveTime,
			PayNum:          userExtendEntity.Alchemy.PayNum,
		},
		Ads: msg.Ads{
			Num: userExtendEntity.Ads.AdsNum,
			IDs: userExtendEntity.Ads.IDs,
		},
		BusinessMan: userExtendEntity.BusinessMan,
	}
}

func PackBuilding(userBuildingEntity *entity.UserBuildingEntity) *msg.Building {
	return &msg.Building{
		ID:         userBuildingEntity.ID,
		BuildingID: userBuildingEntity.BuildingID,
		Level:      userBuildingEntity.Level,
	}
}

func PackTech(userTechEntity *entity.UserTechEntity) *msg.Tech {
	return &msg.Tech{
		ID:     userTechEntity.ID,
		TechID: userTechEntity.TechID,
		Level:  userTechEntity.Level,
	}
}

func PackExplore(userExploreEntity *entity.UserExploreEntity, exploreData *entity.ExploreData,
	userHeroEntitys []*entity.UserHeroEntity) *msg.Explore {
	return &msg.Explore{
		ID:        userExploreEntity.ID,
		ExploreID: userExploreEntity.ExploreID,
		StartTime: userExploreEntity.StartTime,
		HeroIDs:   userExploreEntity.HeroIDs,
		Mul:       userExploreEntity.Mul(exploreData, userHeroEntitys),
	}
}

func PackCrystal(userCrystalEntity *entity.UserCrystalEntity) *msg.Crystal {
	return &msg.Crystal{
		ID:        userCrystalEntity.ID,
		CrystalID: userCrystalEntity.CrystalID,
		Level:     userCrystalEntity.Level,
	}
}

func PackHero(userHeroEntity *entity.UserHeroEntity) *msg.Hero {
	return &msg.Hero{
		ID:               userHeroEntity.ID,
		HeroID:           userHeroEntity.HeroID,
		Type:             userHeroEntity.Type,
		Name:             userHeroEntity.Name,
		Level:            userHeroEntity.Level,
		FightingCapacity: userHeroEntity.GetFightingCapacity(),
		AttackFreq:       userHeroEntity.AttackFreq,
		AttackTrans:      userHeroEntity.GetAttackTrans(),
		DefendTrans:      userHeroEntity.GetDefendTrans(),
		EvolveTimes:      userHeroEntity.EvolveTimes,
		Sex:              userHeroEntity.Sex,
		Race:             userHeroEntity.Race,
		Skills:           userHeroEntity.Skills,
		SkinID:           userHeroEntity.SkinID,
		Skins:            userHeroEntity.SkinMap,
		ExploreID:        userHeroEntity.ExploreID,
	}
}

func PackEquip(heroEquipEntity *entity.HeroEquipEntity) *msg.Equip {
	return &msg.Equip{
		ID:               heroEquipEntity.ID,
		EquipID:          heroEquipEntity.EquipID,
		Level:            heroEquipEntity.Level,
		ForgeID:          heroEquipEntity.ForgeID,
		Pos:              heroEquipEntity.Pos,
		FightingCapacity: heroEquipEntity.FightingCapacity(),
		SkillID:          heroEquipEntity.SkillID,
		SkillEffect1:     heroEquipEntity.SkillEffect1,
		SkillEffect2:     heroEquipEntity.SkillEffect2,
		SkillEffect3:     heroEquipEntity.SkillEffect3,
		EffectID1:        heroEquipEntity.EffectID1,
		EffectVal1:       heroEquipEntity.EffectVal1,
		EffectID2:        heroEquipEntity.EffectID2,
		EffectVal2:       heroEquipEntity.EffectVal2,
		EffectID3:        heroEquipEntity.EffectID3,
		EffectVal3:       heroEquipEntity.EffectVal3,
	}
}

func PackMail(userMailEntity *entity.UserMailEntity) *msg.Mail {
	return &msg.Mail{
		ID:          userMailEntity.ID,
		MailID:      userMailEntity.MailID,
		Status:      userMailEntity.Status,
		Args:        userMailEntity.Params,
		HasReceived: userMailEntity.HasReceived,
		Attachment:  userMailEntity.Attachment,
		CreateTime:  userMailEntity.CreatedAt.Unix(),
	}
}

func PackEquipDoc(heroEquipDocEntity *entity.HeroEquipDocEntity) *msg.EquipDoc {
	return &msg.EquipDoc{
		ID:         heroEquipDocEntity.ID,
		EquipID:    heroEquipDocEntity.EquipID,
		HasReceive: heroEquipDocEntity.HasReceived(),
	}
}

func PackRewards(rewards []constant.IReward) (rewardsMsg []*msg.Reward) {
	for _, reward := range rewards {
		rewardsMsg = append(rewardsMsg, &msg.Reward{
			MainType: reward.GetMainType(),
			SubType:  reward.GetSubType(),
			Val:      reward.GetVal(),
		})
	}
	return
}

func PackRanker(userArenaEntity *entity.UserArenaEntity) *msg.Ranker {
	return &msg.Ranker{
		ID:               userArenaEntity.ID,
		Uid:              userArenaEntity.Uid,
		IsPlayer:         userArenaEntity.IsRealPerson(),
		Name:             userArenaEntity.Name,
		Avatar:           userArenaEntity.Avatar,
		Level:            userArenaEntity.Level,
		FightingCapacity: userArenaEntity.FightingCapacity,
		Rank:             userArenaEntity.Rank,
	}
}
