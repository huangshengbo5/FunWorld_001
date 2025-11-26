package dao

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
	"errors"
	"time"

	"gorm.io/gorm"

	"github.com/vmihailenco/msgpack/v5"
)

type userArenaDao struct {
}

var UserArenaDao = new(userArenaDao)

const (
	KeyUA_1 = "uid:%v:isPlayer:%v"
)

func (dao *userArenaDao) KeyPrefix() string {
	return "arena"
}

func (dao *userArenaDao) CreateByPlayer(userEntity *entity.UserEntity, mainHero *entity.UserHeroEntity) (userArenaEntity *entity.UserArenaEntity, err error) {
	userArenaEntity = entity.NewUserArenaByPlayer(userEntity, mainHero)
	err = util.GetDB().Create(userArenaEntity).Error
	return
}

func (dao *userArenaDao) CreateByNpc(npcData *entity.NpcData, week int) (userArenaEntity *entity.UserArenaEntity,
	err error) {
	userArenaEntity = entity.NewUserArenaByNpc(npcData, week)
	err = util.GetDB().Create(userArenaEntity).Error
	return
}

func (dao *userArenaDao) FetchByUid(uid uint32, isPlayer uint8) (userArenaEntity *entity.UserArenaEntity, err error) {
	userArenaEntity = &entity.UserArenaEntity{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyUA_1, uid, isPlayer)
	val := loadFromCache(key)

	// 有数据走缓存
	if val != nil {
		err = msgpack.Unmarshal(val, userArenaEntity)
		return
	}

	err = util.GetDB().Where("`uid` = ? and `is_player` = ?", uid, isPlayer).First(userArenaEntity).Error
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, nil
		}
	}

	// 序列化对象
	var bytes []byte
	bytes, err = msgpack.Marshal(userArenaEntity)
	if err != nil {
		return
	}
	// 写入缓存
	saveToCache(key, bytes, time.Hour*24)

	return userArenaEntity, err
}

func (dao *userArenaDao) FetchByID(id uint32) (userArenaEntity *entity.UserArenaEntity, err error) {
	userArenaEntity = &entity.UserArenaEntity{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 有数据走缓存
	if val != nil {
		err = msgpack.Unmarshal(val, userArenaEntity)
		return
	}

	err = util.GetDB().First(userArenaEntity, id).Error
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, nil
		}
	}

	// 序列化对象
	var bytes []byte
	bytes, err = msgpack.Marshal(userArenaEntity)
	if err != nil {
		return
	}
	// 写入缓存
	saveToCache(key, bytes, time.Hour*24)

	return userArenaEntity, err
}

func (dao *userArenaDao) FetchByGroupAndWeek(groupID uint16, signWeek int,
	orderByRank bool, offset, limit int) (userArenaEntitys []*entity.UserArenaEntity,
	err error) {
	userArenaEntitys = make([]*entity.UserArenaEntity, 0)

	if orderByRank {
		err = util.GetDB().Where("`sign_week` = ? and `group_id` = ?", signWeek,
			groupID).Order("`rank` ASC").Offset(offset).Limit(limit).Find(&userArenaEntitys).Error
	} else {
		err = util.GetDB().Where("`sign_week` = ? and `group_id` = ?", signWeek, groupID).Find(&userArenaEntitys).Error
	}

	if err != nil {
		return
	}

	return
}

func (dao *userArenaDao) FetchByIDs(ids []uint32) (userArenaEntitys []*entity.UserArenaEntity,
	err error) {
	userArenaEntitys = make([]*entity.UserArenaEntity, 0)
	err = util.GetDB().Where("`id` IN (?)", ids).Find(&userArenaEntitys).Error
	if err != nil {
		return
	}

	return
}

// 获取数据总量
func (dao *userArenaDao) CountByWeek(week int) (count int64, err error) {
	err = util.GetDB().Model(entity.UserArenaEntity{}).Where("sign_week = ?", week).Count(&count).Error
	return
}

// 获取指定周数据
func (dao *userArenaDao) FetchAllByWeek(week, offset, limit int, inOrder bool) (userArenaEntitys []*entity.
	UserArenaEntity,
	err error) {
	// 每次批量处理 200 条
	if inOrder {
		err = util.GetDB().Where("`sign_week` = ?", week).Order("`fighting_capacity` DESC").Offset(offset).
			Limit(limit).Find(&userArenaEntitys).Error
	} else {
		err = util.GetDB().Where("`sign_week` = ?", week).Order("`id` ASC").Offset(offset).Limit(limit).Find(
			&userArenaEntitys).Error
	}

	return
}

func (dao *userArenaDao) Update(userArenaEntity *entity.UserArenaEntity) (err error) {
	err = util.GetDB().Save(userArenaEntity).Error

	if err != nil {
		return
	}

	deleteCache(makeCacheKey(dao.KeyPrefix(), KeyUA_1, userArenaEntity.Uid, userArenaEntity.IsPlayer), makeCacheKey(dao.KeyPrefix(), KeyID, userArenaEntity.ID))

	return
}

func (dao *userArenaDao) UpdateMulti(userArenaEntitys []*entity.UserArenaEntity) (err error) {
	keys := make([]string, 0, 2*len(userArenaEntitys))

	for _, userArenaEntity := range userArenaEntitys {
		keys = append(keys, makeCacheKey(dao.KeyPrefix(), KeyUA_1, userArenaEntity.Uid, userArenaEntity.IsPlayer))
		keys = append(keys, makeCacheKey(dao.KeyPrefix(), KeyID, userArenaEntity.ID))
	}

	err = util.GetDB().Save(userArenaEntitys).Error

	if err != nil {
		return
	}

	deleteCache(keys...)

	return
}
