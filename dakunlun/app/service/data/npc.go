package data

import (
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
)

type npcService struct {
}

var NpcService = new(npcService)

func (srv *npcService) GetNpcByID(id uint32) (npcEntity *entity.NpcData, err error) {
	npcEntity, err = data.NpcDao.FetchByID(id)
	return
}
