package data

import (
	"dakunlun/app/dao/data"
	"dakunlun/app/entity"
)

type campaignService struct {
}

var CampaignService = new(campaignService)

func (srv *campaignService) GetCampaignByID(id uint32) (campaignEntity *entity.CampaignData, err error) {
	campaignEntity, err = data.CampaignDao.FetchByID(id)
	return
}
