package service

import (
	"context"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/util"
	"fmt"
	"time"

	"github.com/spf13/cast"
)

const (
	AdsExpire = 600
	AdsMin    = 1
)

type adsService struct {
}

var AdsService = new(adsService)

func (srv *adsService) StartAds(userEntity *entity.UserEntity) (adsID string, expireTime int64, err error) {
	now := util.Carbon().Now().ToTimestamp()
	adsID = cast.ToString(now)

	key := fmt.Sprintf("%d:%s", userEntity.ID, adsID)

	expireTime = time.Now().Unix() + AdsExpire
	err = util.GetRedisClient().SetEX(context.Background(), key, now, time.Second*AdsExpire).Err()
	if err != nil {
		return
	}

	return
}

func (srv *adsService) CheckAds(userEntity *entity.UserEntity, adsID string) (err error) {
	key := fmt.Sprintf("%d:%s", userEntity.ID, adsID)

	var val string
	val, err = util.GetRedisClient().Get(context.Background(), key).Result()
	if err != nil {
		return
	}

	if util.Carbon().Now().ToTimestamp()-cast.ToInt64(val) < AdsMin {
		err = util.NewAppError(util.ErrorCodeHack, "ads not over")
	}

	return
}

func (srv *adsService) DropAds(userEntity *entity.UserEntity, adsID string) (err error) {
	key := fmt.Sprintf("%d:%s", userEntity.ID, adsID)
	err = util.GetRedisClient().Del(context.Background(), key).Err()
	if err != nil {
		return
	}
	var userExtendEntity *entity.UserExtendEntity
	userExtendEntity, err = UserService.GetUserExtendByID(userEntity.ID)

	if err != nil {
		return
	}
	userExtendEntity.Ads.AdsNum++
	err = UserService.UpdateUserExtend(userExtendEntity)
	return
}

func (srv *adsService) GetAdsReward(id uint32) (adsData *entity.AdsData, err error) {
	adsData, err = data.AdsDao.FetchByID(id)
	return
}
