package service

import (
	"context"
	"dakunlun/app/constant"
	"dakunlun/app/dao"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
	"dakunlun/app/msg"
	"dakunlun/app/util"
	"errors"
	"fmt"
	"strconv"
	"time"

	"gorm.io/gorm"

	"github.com/vmihailenco/msgpack/v5"

	"go.uber.org/zap"
)

const RecordNum = 2
const SignDay = 4       //竞技场报名日
const MatchStartDay = 5 //竞技场比赛开始日
const MatchEndDay = 7   //竞技场比赛结束日

type userArenaService struct {
}

var UserArenaService = new(userArenaService)

// 是否是报名日
func (srv *userArenaService) InSingUpDay() bool {
	return util.Carbon().Now().DayOfWeek() == SignDay
}

// 是否比赛日
func (srv *userArenaService) InMatchDay() bool {
	return util.Carbon().Now().DayOfWeek() >= MatchStartDay && util.Carbon().Now().DayOfWeek() <= MatchEndDay
}

// 获取用户竞技场数据
func (srv *userArenaService) GetUserArenaOrCreate(userEntity *entity.UserEntity) (userArenaEntity *entity.UserArenaEntity, err error) {
	userArenaEntity, err = dao.UserArenaDao.FetchByUid(userEntity.ID, 1)
	if err != nil {
		return
	}

	if userArenaEntity == nil {
		var mainHero *entity.UserHeroEntity
		mainHero, err = UserHeroService.GetHeroByID(userEntity.MainHeroID)
		if err != nil {
			return
		}
		userArenaEntity, err = dao.UserArenaDao.CreateByPlayer(userEntity, mainHero)
		if err != nil {
			return
		}
	}

	return
}

func (srv *userArenaService) GetUserArenaByID(id uint32) (userArenaEntity *entity.UserArenaEntity,
	err error) {
	userArenaEntity, err = dao.UserArenaDao.FetchByID(id)
	return
}

func (srv *userArenaService) GetUserArenaByUid(uid uint32) (userArenaEntity *entity.UserArenaEntity,
	err error) {
	userArenaEntity, err = dao.UserArenaDao.FetchByUid(uid, 1)
	return
}

func (srv *userArenaService) GetRankers(userArenaEntity *entity.UserArenaEntity, page,
	perPage int64) (userArenaEntitys []*entity.UserArenaEntity, pageInfo *constant.Paging, err error, inRebuild bool) {

	//var members []string
	//members, err = RankService.RangeByRank(userArenaEntity, page, perPage)
	//if err != nil {
	//	return
	//}
	//
	//if len(members) == 0 {
	//	util.GoPool().Submit(srv.RebuildFunc(userArenaEntity.GroupID, userArenaEntity.SignWeek))
	//	inRebuild = true
	//	return
	//}
	//
	//ids := make([]uint32, 0, len(members))
	//for _, m := range members {
	//	ids = append(ids, util.ToUint32(m, 0))
	//}
	//
	//var total int64
	//total, err = RankService.Len(userArenaEntity)
	//if err != nil {
	//	return
	//}

	//userArenaEntitys, err = dao.UserArenaDao.FetchByIDs(ids)
	//if err != nil {
	//	return
	//}
	//
	//pageInfo = &constant.Paging{
	//	Page:     int(page),
	//	PerPage:  int(perPage),
	//	TotalNum: int(total),
	//}

	userArenaEntitys, err = dao.UserArenaDao.FetchByGroupAndWeek(userArenaEntity.GroupID, userArenaEntity.SignWeek,
		true, int((page-1)*perPage), int(perPage))
	if err != nil {
		return
	}

	pageInfo = &constant.Paging{
		Page:     int(page),
		PerPage:  int(perPage),
		TotalNum: len(userArenaEntitys),
	}
	return
}

// 更新竞技场用户数据
func (srv *userArenaService) UpdateArena(userArenaEntity *entity.UserArenaEntity) (err error) {
	err = dao.UserArenaDao.Update(userArenaEntity)
	//RankService.Add(userArenaEntity)
	return
}

// 凌晨创建数据
func (srv *userArenaService) InitArena() {
	week := util.GetYearWeek()

	total, err := dao.UserArenaDao.CountByWeek(week)
	if err != nil {

	}

	//先补充NPC
	if total%entity.PerGroup > 0 {
		srv.AddNpc(week, int(entity.PerGroup-total%entity.PerGroup))
	}

	offset := 0
	limit := entity.PerGroup * 1
	var groupID uint16 = 1
	for i := 0; true; i++ {
		userArenaEntitys, err := dao.UserArenaDao.FetchAllByWeek(week, offset, limit, true)
		if err != nil {
			util.GetLogger().Error("InitArena", zap.Error(err))
		}

		//无数据则跳出
		if len(userArenaEntitys) == 0 {
			break
		}

		var rank uint16 = 1
		for _, userArenaEntity := range userArenaEntitys {
			userArenaEntity.GroupID = groupID
			userArenaEntity.Rank = rank
			//达到200重置
			if rank == entity.PerGroup {
				groupID++
				rank = 1
			} else {
				rank++
			}
		}

		//更新redis mysql
		//util.GoPool().Submit(func() {
		err = dao.UserArenaDao.UpdateMulti(userArenaEntitys)
		if err != nil {
			util.GetLogger().Error("InitArena.updateMysql", zap.Error(err))
		}
		//})

		//添加到排行榜
		//util.GoPool().Submit(func() {
		//	tmpRankers := make([]BaseRanker, 0, len(userArenaEntitys))
		//	for _, userArenaEntity := range userArenaEntitys {
		//		tmpRankers = append(tmpRankers, userArenaEntity)
		//	}
		//	err := RankService.BatchAdd(tmpRankers)
		//	if err != nil {
		//		util.GetLogger().Error("InitArena.updateMysql", zap.Error(err))
		//	}
		//})

		offset += limit
	}
}

func (srv *userArenaService) AddNpc(week int, num int) (err error) {
	for id := 80001; id < 80001+num; id++ {
		var npcData *entity.NpcData
		npcData, err = data.NpcDao.FetchByID(uint32(id))
		if err != nil {
			return
		}

		var npcArenaEntity *entity.UserArenaEntity
		npcArenaEntity, err = dao.UserArenaDao.FetchByUid(npcData.ID, 0)
		if err != nil {
			if !errors.Is(err, gorm.ErrRecordNotFound) {
				util.GetLogger().Error("addNpc", zap.Error(err))
			}
		}

		//创建新NPC
		if npcArenaEntity == nil {
			npcArenaEntity, err = dao.UserArenaDao.CreateByNpc(npcData, week)
			if err != nil {
				return
			}
		} else {
			npcArenaEntity.SignWeek = week
			err = srv.UpdateArena(npcArenaEntity)
			if err != nil {
				util.GetLogger().Error("addNpc", zap.Error(err))
			}
		}
	}

	return
}

// 排行榜结算
func (srv *userArenaService) RankCalculator() {
	week := util.GetPrevYearWeek()

	arenaRewardDatas, err := data.ArenaRewardDao.FetchAll()
	if err != nil {

	}

	//奖励map
	rankRewardMap := make(map[uint16]entity.RewardStrings, entity.PerGroup)
	for _, arenaRewardData := range arenaRewardDatas {
		for rank := arenaRewardData.RankTop; rank <= arenaRewardData.RankBottom; rank++ {
			rankRewardMap[rank] = arenaRewardData.Rewards
		}
	}

	offset := 0
	limit := entity.PerGroup * 1
	for i := 0; true; i++ {
		userArenaEntitys, err := dao.UserArenaDao.FetchAllByWeek(week, offset, limit, false)
		if err != nil {
			util.GetLogger().Error("InitArena", zap.Error(err))
		}

		//无数据则跳出
		if len(userArenaEntitys) == 0 {
			break
		}

		// 取奖励
		for _, userArenaEntity := range userArenaEntitys {
			if userArenaEntity.IsRealPerson() {
				_, err := UserMailService.AddMail(userArenaEntity.Uid, entity.MailIDArena,
					entity.Params{strconv.Itoa(int(userArenaEntity.Rank))}, rankRewardMap[userArenaEntity.Rank])
				if err != nil {
					util.GetLogger().Error("AddMail", zap.Error(err),
						zap.Any("ranker", userArenaEntity))
				} else {
					userArenaEntity.MarkSend()
					err := srv.UpdateArena(userArenaEntity)
					if err != nil {
						util.GetLogger().Error("UpdateArena", zap.Error(err),
							zap.Any("ranker", userArenaEntity))
					}
				}
			}
		}

		offset += limit
	}
}

func (srv *userArenaService) RebuildFunc(groupID uint16, signWeek int) func() {
	return func() {
		//bT := time.Now() // 开始时间
		//key := fmt.Sprintf("%s:%v:%v", "rebuild", signWeek, groupID)
		//lock := util.NewRedisLock(key, "", time.Minute)
		//if !lock.Lock() {
		//	return
		//}
		//
		//defer func() {
		//	if reason := recover(); reason != nil {
		//		util.GetLogger().Error("userArenaService.RebuildFunc", zap.Any("reason", reason))
		//	}
		//	util.GetRedisClient().Del(context.Background(), key).Err()
		//	util.GetLogger().Info("userArenaService.RebuildFunc", zap.Float64("costTime", time.Since(bT).Seconds()))
		//	lock.Unlock()
		//}()
		//
		//userArenaEntitys, err := dao.UserArenaDao.FetchByGroupAndWeek(groupID, signWeek)
		//if err != nil {
		//	util.GetLogger().Error("userArenaService.RebuildFunc.FetchByGroupAndWeek", zap.Error(err))
		//	return
		//}
		//
		//rankers := make([]BaseRanker, 0, len(userArenaEntitys))
		//for _, v := range userArenaEntitys {
		//	rankers = append(rankers, v)
		//}
		//
		//err = RankService.BatchAdd(rankers)
		//if err != nil {
		//	util.GetLogger().Error("userArenaService.RebuildFunc.BatchAdd", zap.Error(err))
		//	return
		//}
	}
}

func (srv *userArenaService) RecordKey(uid uint32) string {
	return fmt.Sprintf("arena_records:%v", uid)
}

// 更新竞技场用户数据
func (srv *userArenaService) AddRecord(attacker *entity.UserArenaEntity, defender *entity.UserArenaEntity,
	isWin bool, oldRank uint16) (err error) {
	record := &msg.ArenaRecord{
		Uid:     attacker.Uid,
		Name:    attacker.Name,
		Win:     isWin,
		OldRank: oldRank,
		NewRank: defender.Rank,
		Time:    time.Now().Unix(),
	}
	var byteData []byte
	byteData, err = msgpack.Marshal(record)
	if err != nil {
		return
	}
	ctx := context.TODO()
	cacheKey := srv.RecordKey(defender.Uid)
	var num int64
	num, err = util.GetRedisClient().LPush(ctx, cacheKey, byteData).Result()
	if err != nil {
		return
	}
	if num == 1 {
		err = util.GetRedisClient().Expire(ctx, cacheKey, time.Hour*72).Err()
		if err != nil {
			return
		}
	} else if num > RecordNum {
		err = util.GetRedisClient().RPop(ctx, cacheKey).Err()
		if err != nil {
			return
		}
	}

	return
}

func (srv *userArenaService) GetRecords(uid uint32) (records []*msg.ArenaRecord, err error) {
	var vals []string
	vals, err = util.GetRedisClient().LRange(context.TODO(), srv.RecordKey(uid), 0, RecordNum-1).Result()
	if err != nil {
		return
	}

	for _, v := range vals {
		record := &msg.ArenaRecord{}
		msgpack.Unmarshal([]byte(v), record)
		records = append(records, record)
	}

	return
}
