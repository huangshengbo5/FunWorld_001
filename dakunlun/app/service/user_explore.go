package service

import (
	"dakunlun/app/dao"
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
)

type userExploreService struct {
}

var UserExploreService = new(userExploreService)

// 添加探索
func (srv *userExploreService) AddExplore(userEntity *entity.UserEntity, exploreID uint32) (exploreEntity *entity.UserExploreEntity, err error) {
	exploreEntity, err = dao.UserExploreDao.Create(userEntity.ID, exploreID)
	return
}

// 获取用户探索
func (srv *userExploreService) GetExploresByUid(uid uint32) (userExploreEntitys []*entity.UserExploreEntity, err error) {
	userExploreEntitys, err = dao.UserExploreDao.FetchMultiByUid(uid)
	return
}

// 获取探索配置数据
func (srv *userExploreService) GetExploreData(id uint32) (exploreData *entity.ExploreData, err error) {
	exploreData, err = data.ExploreDao.FetchByID(id)
	return
}

// 获取用户探索
func (srv *userExploreService) GetExploreByID(id uint32) (userExploreEntity *entity.UserExploreEntity, err error) {
	userExploreEntity, err = dao.UserExploreDao.FetchByID(id)
	return
}

// 更新用户探索数据
func (srv *userExploreService) UpdateExplore(userExploreEntity *entity.UserExploreEntity) (err error) {
	err = dao.UserExploreDao.Update(userExploreEntity)
	return
}
