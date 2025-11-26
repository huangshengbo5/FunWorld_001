package service

import (
	"dakunlun/app/constant"
	"dakunlun/app/dao"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/util"
	"errors"
	"fmt"
	"math/rand"
	"strconv"
	"time"

	"github.com/spf13/cast"

	"go.uber.org/zap"

	"golang.org/x/sync/errgroup"

	"gorm.io/gorm"
)

type userService struct {
}

var UserService = new(userService)

func (srv *userService) CreateUser(id uint32, accountData *entity.AccountData) (userEntity *entity.UserEntity, err error) {
	userEntity = entity.NewUser(id, accountData.Type)
	userEntity.Gold = accountData.StartGold
	userEntity.Diamond = 0
	userEntity, err = dao.UserDao.Create(userEntity)
	return
}

func (srv *userService) GetUserByID(id uint32) (userEntity *entity.UserEntity, err error) {
	userEntity, err = dao.UserDao.FetchByID(id)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, nil
		}
	}

	if userEntity.Extra.AccountType != 4 {
		var minorslimitDatas []*entity.MinorslimitData
		minorslimitDatas, err = data.MinorslimitDao.FetchAll()
		if err != nil {
			return
		}
		now := util.Carbon().Now().ToTimestamp()
		for _, v := range minorslimitDatas {
			if now > util.Carbon().Parse(v.StartTime).ToTimestamp() && now < util.Carbon().Parse(v.EndTime).ToTimestamp() {
				return
			}
		}

		err = util.NewAppError(2000)
	}

	return
}

// 检测名字是否可用
func (srv *userService) CheckName(name string) (ok bool, err error) {
	_, err = dao.UserDao.FetchByName(name)
	if err != nil && errors.Is(err, gorm.ErrRecordNotFound) {
		ok = true
		err = nil
	}
	return
}

// 获取头像配置
func (srv *userService) GetAvatarByID(avatarID uint32) (avatarData *entity.AvatarData, err error) {
	avatarData, err = data.AvatarDao.FetchByID(avatarID)
	return
}

func (srv *userService) CreateUserExtend(id uint32) (userExtendEntity *entity.UserExtendEntity, err error) {
	userExtendEntity = entity.NewUserExtend(id)
	userExtendEntity, err = dao.UserExtendDao.Create(userExtendEntity)
	return
}

func (srv *userService) GetUserExtendByID(id uint32) (userExtendEntity *entity.UserExtendEntity, err error) {
	userExtendEntity, err = dao.UserExtendDao.FetchByID(id)

	day, _ := strconv.Atoi(util.Carbon().Now().Format("Ymd"))

	needUpdate := false
	if userExtendEntity.LastModifyDay == 0 || userExtendEntity.LastModifyDay != day {
		userExtendEntity.ResetPerDay(day, false)
		LobbyService.RefreshTower(userExtendEntity)
		needUpdate = true
	}

	if len(userExtendEntity.Tower) == 0 {
		LobbyService.RefreshTower(userExtendEntity)
		needUpdate = len(userExtendEntity.Tower) > 0
	}

	if needUpdate {
		err = srv.UpdateUserExtend(userExtendEntity)
	}

	return
}

func (srv *userService) DecrAssets(userEntity *entity.UserEntity, costType uint16, costSubType uint32,
	costVal uint64) (err error) {
	return srv.changeAssets(userEntity, costType, costSubType, costVal, true)
}

func (srv *userService) IncrAssets(userEntity *entity.UserEntity, costType uint16, costSubType uint32,
	costVal uint64) (err error) {
	return srv.changeAssets(userEntity, costType, costSubType, costVal, false)
}

func (srv *userService) changeAssets(userEntity *entity.UserEntity, costType uint16, costSubType uint32, costVal uint64, isDecr bool) (err error) {
	costVal32 := cast.ToUint32(costVal)
	switch costType {
	case constant.CostTypeGold:
		if isDecr {
			if userEntity.CurGold() < uint64(costVal) {
				return util.NewAppError(util.ErrorCodeGoldNotEnough)
			}
			userEntity.Gold -= costVal
		} else {
			userEntity.Gold += costVal
		}

	case constant.CostTypeDiamond:
		if isDecr {
			if userEntity.Diamond < costVal32 {
				return util.NewAppError(util.ErrorCodeDiamondNotEnough)
			}
			userEntity.Diamond -= costVal32
		} else {
			userEntity.Diamond += costVal32
		}
	case constant.CostTypeSoulCrystal:
		if isDecr {
			if userEntity.Resource.SoulCrystal < costVal32 {
				return util.NewAppError(util.ErrorCodeDiamondNotEnough)
			}
			userEntity.Resource.SoulCrystal -= costVal32
		} else {
			userEntity.Resource.SoulCrystal += costVal32
		}
	case constant.CostTypeTreasureAnima:
		if isDecr {
			if userEntity.Resource.TreasureAnima < costVal32 {
				return util.NewAppError(util.ErrorCodeTreasureAnimaNotEnough)
			}
			userEntity.Resource.TreasureAnima -= costVal32
		} else {
			userEntity.Resource.TreasureAnima += costVal32
		}
	case constant.CostTypeSuperbiaStone:
		if isDecr {
			if userEntity.Resource.SuperbiaStone < costVal32 {
				return util.NewAppError(util.ErrorCodeSuperbiaStoneNotEnough)
			}
			userEntity.Resource.SuperbiaStone -= costVal32
		} else {
			userEntity.Resource.SuperbiaStone += costVal32
		}
	case constant.CostTypeInvidiaStone:
		if isDecr {
			if userEntity.Resource.InvidiaStone < costVal32 {
				return util.NewAppError(util.ErrorCodeInvidiaStoneNotEnough)
			}
			userEntity.Resource.InvidiaStone -= costVal32
		} else {
			userEntity.Resource.InvidiaStone += costVal32
		}
	case constant.CostTypeAcediaStone:
		if isDecr {
			if userEntity.Resource.AcediaStone < costVal32 {
				return util.NewAppError(util.ErrorCodeAcediaStoneNotEnough)
			}
			userEntity.Resource.AcediaStone -= costVal32
		} else {
			userEntity.Resource.AcediaStone += costVal32
		}
	case constant.CostTypeGulaStone:
		if isDecr {
			if userEntity.Resource.GulaStone < costVal32 {
				return util.NewAppError(util.ErrorCodeGulaStoneNotEnough)
			}
			userEntity.Resource.GulaStone -= costVal32
		} else {
			userEntity.Resource.GulaStone += costVal32
		}
	case constant.CostTypeAvaritiaStone:
		if isDecr {
			if userEntity.Resource.AvaritiaStone < costVal32 {
				return util.NewAppError(util.ErrorCodeAvaritiaStoneNotEnough)
			}
			userEntity.Resource.AvaritiaStone -= costVal32
		} else {
			userEntity.Resource.AvaritiaStone += costVal32
		}
	case constant.CostTypeLuxuriaStone:
		if isDecr {
			if userEntity.Resource.LuxuriaStone < costVal32 {
				return util.NewAppError(util.ErrorCodeLuxuriaStoneNotEnough)
			}
			userEntity.Resource.LuxuriaStone -= costVal32
		} else {
			userEntity.Resource.LuxuriaStone += costVal32
		}
	case constant.CostTypeIraStone:
		if isDecr {
			if userEntity.Resource.IraStone < costVal32 {
				return util.NewAppError(util.ErrorCodeIraStoneNotEnough)
			}
			userEntity.Resource.IraStone -= costVal32
		} else {
			userEntity.Resource.IraStone += costVal32
		}
	case constant.CostTypeBenYuan:
		if isDecr {
			if userEntity.Resource.BenYuan < costVal32 {
				return util.NewAppError(util.ErrorCodeBenYuanNotEnough)
			}
			userEntity.Resource.BenYuan -= costVal32
		} else {
			userEntity.Resource.BenYuan += costVal32
		}
	case constant.CostTypeQianNeng:
		if isDecr {
			if userEntity.Resource.QianNeng < costVal32 {
				return util.NewAppError(util.ErrorCodeQianNengNotEnough)
			}
			userEntity.Resource.QianNeng -= costVal32
		} else {
			userEntity.Resource.QianNeng += costVal32
		}
	case constant.CostTypeElement:
		if isDecr {
			if userEntity.Resource.Element < costVal32 {
				return util.NewAppError(util.ErrorCodeElementNotEnough)
			}
			userEntity.Resource.Element -= costVal32
		} else {
			userEntity.Resource.Element += costVal32
		}
	case constant.CostTypeBook:
		if isDecr {
			if userEntity.Resource.Book < costVal32 {
				return util.NewAppError(util.ErrorCodeBookNotEnough)
			}
			userEntity.Resource.Book -= costVal32
		} else {
			userEntity.Resource.Book += costVal32
		}
	}

	return
}

// 检查开启条件
func (srv *userService) ConditionCheck(userEntity *entity.UserEntity, conditionType uint8, conditionVal uint32) (err error) {
	switch conditionType {
	case constant.OpenTypeLevel:
		if uint32(userEntity.Level) < conditionVal {
			err = util.NewAppError(util.ErrorCodeLowLevel)
		}
	case constant.OpenTypeCampaignNum:
		if userEntity.CampaignNum < conditionVal {
			err = util.NewAppError(util.ErrorCodeLowCampaignNum)
		}
	}
	return
}

func (srv *userService) UpdateEquips(userEntity *entity.UserEntity) (err error) {
	//使用中的装备ID列表
	ids := userEntity.EquipIDs()
	//清空装备加成
	userEntity.ClearEquipPlus()
	var fightingCapacityEquipPlus uint64
	if len(ids) > 0 {
		var heroEquipEntitys []*entity.HeroEquipEntity
		heroEquipEntitys, err = HeroEquipService.GetEquipsByIds(ids)
		if err != nil {
			return
		}

		//重新计算装备加成
		for _, v := range heroEquipEntitys {
			fightingCapacityEquipPlus += v.FightingCapacity()
			//装备加成更新
			userEntity.SetEquipPlus(v.EffectID1, v.EffectVal1)
			userEntity.SetEquipPlus(v.EffectID2, v.EffectVal2)
			userEntity.SetEquipPlus(v.EffectID3, v.EffectVal3)
		}
	}

	//装备加成更新
	userEntity.Attr.FightingCapacityEquipPlus = fightingCapacityEquipPlus

	return
}

func (srv *userService) UpdateUser(userEntity *entity.UserEntity) (err error) {
	err = dao.UserDao.Update(userEntity)
	return
}

func (srv *userService) UpdateUserExtend(userExtendEntity *entity.UserExtendEntity) (err error) {
	err = dao.UserExtendDao.Update(userExtendEntity)
	return
}

func (srv *userService) Shuffle(userEntitys []*entity.UserEntity) {
	r := rand.New(rand.NewSource(time.Now().Unix()))
	for len(userEntitys) > 0 {
		n := len(userEntitys)
		randIndex := r.Intn(n)
		userEntitys[n-1], userEntitys[randIndex] = userEntitys[randIndex], userEntitys[n-1]
		userEntitys = userEntitys[:n-1]
	}
}

// -------------------------- boos相关------------------------------------------

func (srv *userService) GenTodayApocalypse() (apocalypseData *entity.ApocalypseData, err error) {
	apocalypseData, err = data.ApocalypseDao.FetchByID(uint32(util.Carbon().Now().DayOfWeek()))
	return
}

func (srv *userService) GetApocalypseByID(apocalypseID uint32) (apocalypseData *entity.ApocalypseData, err error) {
	apocalypseData, err = data.ApocalypseDao.FetchByID(apocalypseID)
	return
}

func (srv *userService) GetAlchemyByAttr(attr uint32) (alchemyData *entity.AlchemyData, err error) {
	alchemyData, err = data.AlchemyDao.FetchByAttr(attr)
	return
}

func (srv *userService) MatchingForApocalypse(userEntity *entity.UserEntity) (userEntitys []*entity.UserEntity,
	err error) {
	var highUsers, lowUsers []*entity.UserEntity
	var g errgroup.Group
	g.Go(func() error {
		defer func() {
			if x := recover(); x != nil {
				util.GetLogger().Error("MatchingForApocalypse.highUsers", zap.Error(err))
			}
		}()
		highUsers, err = dao.UserDao.FetchMultiByLevelGT(userEntity.Level, 10)
		return err
	})
	g.Go(func() error {
		defer func() {
			if x := recover(); x != nil {
				util.GetLogger().Error("MatchingForApocalypse.lowUsers", zap.Error(err))
			}
		}()
		lowUsers, err = dao.UserDao.FetchMultiByLevelLTE(userEntity.Level, 5)
		return err
	})

	if err = g.Wait(); err != nil {
		util.GetLogger().Error("MatchingForApocalypse", zap.Error(err))
		return
	}

	userPools := append(highUsers, lowUsers...)
	srv.Shuffle(userPools)

	userEntitys = append(userEntitys, userEntity)
	for _, v := range userPools {
		if v.ID != userEntity.ID {
			userEntitys = append(userEntitys, v)
		}
		if len(userEntitys) == 3 {
			return
		}
	}

	return
}

// -------------------------- 成就相关------------------------------------------
func (srv *userService) GetOrCreateAnnals(uid uint32) (userAnnalsEntity *entity.UserAnnalsEntity, err error) {
	userAnnalsEntity, err = dao.UserAnnalsDao.FetchByID(uid)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			userAnnalsEntity, err = dao.UserAnnalsDao.Create(uid)
		}
	}

	return
}

func (srv *userService) GetAnnalsData(id uint32) (annalsData *entity.AnnalsData, err error) {
	annalsData, err = data.AnnalsDao.FetchByID(id)
	return
}

func (srv *userService) UpdateUserAnnals(userAnnalsEntity *entity.UserAnnalsEntity) (err error) {
	err = dao.UserAnnalsDao.Update(userAnnalsEntity)
	return
}

// 检查是否符合成就条件
func (srv *userService) CheckAnnalsCondition(userEntity *entity.UserEntity, annalsData *entity.AnnalsData) (ok bool, err error) {
	switch annalsData.Type {
	case entity.AnnalsTypeNew:
		ok = true
	case entity.AnnalsTypeCampaign:
		ok = userEntity.CampaignNum >= annalsData.Value
	case entity.AnnalsTypeGetHero:
		_, err = UserHeroService.GetHeroByHeroID(userEntity.ID, annalsData.Value)
		if err != nil {
			ok = false
		} else {
			ok = true
		}
	case entity.AnnalsTypeFC:
		var total uint64
		total, err = srv.GetTotalFightingCapacity(userEntity)
		if err != nil {
			ok = false
		} else {
			ok = total >= uint64(annalsData.Value)
		}
	case entity.AnnalsTypeHeroFC:
		var userHeroEntity *entity.UserHeroEntity
		userHeroEntity, err = UserHeroService.GetHeroByHeroID(userEntity.ID, annalsData.SubType)
		if err != nil {
			ok = false
		} else {
			ok = userHeroEntity.GetFightingCapacity() >= uint64(annalsData.Value)
		}
	default:
		ok = false
		err = util.NewAppError(util.ErrorConfigError, fmt.Sprintf("annals type : %d", annalsData.Type))
	}
	return
}

func (srv *userService) GetTotalFightingCapacity(userEntity *entity.UserEntity) (r uint64, err error) {
	heroEntitys, err := UserHeroService.GetHerosByUid(userEntity.ID)
	if err != nil {
		return
	}

	r = userEntity.TechEffect.MainHeroFightingCapacityPlus

	for _, heroEntity := range heroEntitys {
		r += heroEntity.GetFightingCapacity()
	}

	r += userEntity.CastEffect.FightingCapacityPlus

	return
}

// 根据CASTID获取铸造配置
func (srv *userService) GetCastByID(castID uint32) (castData *entity.CastData, err error) {
	castData, err = data.CastDao.FetchByID(castID)
	return
}
