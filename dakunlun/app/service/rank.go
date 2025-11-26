package service

import (
	"context"
	"dakunlun/app/util"
	"time"

	"github.com/go-redis/redis/v8"
	"golang.org/x/sync/singleflight"
)

type BaseRanker interface {
	Key() string
	Expire() time.Duration
	IsAsc() bool
	GetScore() float64
	GetMember() string
}

type RebuildFunc func() ([]BaseRanker, error)

type rankService struct {
	rebuildTask  []RebuildFunc
	singleFlight singleflight.Group
}

var RankService = &rankService{}

//获取成员名次
func (srv *rankService) GetRank(ranker BaseRanker) (int64, error) {
	return util.GetRedisClient().ZRank(context.Background(), ranker.Key(), ranker.GetMember()).Result()
}

//获取成员分数
func (srv *rankService) GetScore(ranker BaseRanker) (float64, error) {
	return util.GetRedisClient().ZScore(context.Background(), ranker.Key(), ranker.GetMember()).Result()
}

//添加成员到排行榜
func (srv *rankService) Add(ranker BaseRanker) error {
	return util.GetRedisClient().ZAdd(context.Background(), ranker.Key(), &redis.Z{
		Score:  ranker.GetScore(),
		Member: ranker.GetMember(),
	}).Err()
}

//批量添加成员到排行榜
func (srv *rankService) BatchAdd(rankers []BaseRanker) error {
	rankerMap := make(map[string][]*redis.Z)

	for _, ranker := range rankers {
		rankKey := ranker.Key()
		if _, exist := rankerMap[rankKey]; !exist {
			rankerMap[rankKey] = make([]*redis.Z, 0)
		}

		rankerMap[rankKey] = append(rankerMap[rankKey], &redis.Z{
			Score:  ranker.GetScore(),
			Member: ranker.GetMember(),
		})
	}

	for k, v := range rankerMap {
		err := util.GetRedisClient().ZAdd(context.Background(), k, v...).Err()
		if err != nil {
			return err
		}
	}
	return nil
}

//获取成员总数
func (srv *rankService) Len(ranker BaseRanker) (int64, error) {
	return util.GetRedisClient().ZCard(context.Background(), ranker.Key()).Result()
}

//获取排行榜start-end
func (srv *rankService) RangeByRank(ranker BaseRanker, start, end int64) ([]string, error) {
	if ranker.IsAsc() {
		return util.GetRedisClient().ZRange(context.Background(), ranker.Key(), start-1, end-1).Result()
	} else {
		return util.GetRedisClient().ZRevRange(context.Background(), ranker.Key(), start-1, end-1).Result()
	}
}

//检查是否存在
func (srv *rankService) Exist(ranker BaseRanker) (exist bool, err error) {
	var val int64
	val, err = util.GetRedisClient().Exists(context.Background(), ranker.Key()).Result()
	if err != nil {
		return false, err
	}

	if val == 1 {
		exist = true
	}

	return
}

// 检查并重建
func (srv *rankService) CheckRebuild(ranker BaseRanker, f func() ([]BaseRanker, error)) error {
	//v, err, shared := srv.singleFlight.Do(ranker.Key(), func() (interface{}, error) {
	//	exist, err := srv.Exist(ranker)
	//	if err != nil {
	//		return nil, err
	//	}
	//
	//	var rankers []BaseRanker
	//	if !exist {
	//
	//		rankers, err = f()
	//		if err != nil {
	//			return nil, err
	//		}
	//		err = srv.BatchAdd(rankers)
	//		if err != nil {
	//			return nil, err
	//		}
	//		err = util.GetRedisClient().Expire(context.Background(), ranker.Key(), ranker.Expire()).Err()
	//		if err != nil {
	//			return nil, err
	//		}
	//	} else {
	//		rankers = nil
	//	}
	//	return rankers, nil
	//})
	//
	//if shared {
	//	util.GetLogger().Info("rankService.CheckRebuild", zap.Bool("shared", shared))
	//}

	return nil
}
