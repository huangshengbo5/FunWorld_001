package data

import (
	"dakunlun/app/entity"
	"dakunlun/app/util"
)

type campaignDao struct {
}

var CampaignDao = new(campaignDao)

func (dao *campaignDao) KeyPrefix() string {
	return "campaign"
}

func (dao *campaignDao) FetchByID(id uint32) (campaignData *entity.CampaignData, err error) {
	campaignData = &entity.CampaignData{}

	// 生成缓存key
	key := makeCacheKey(dao.KeyPrefix(), KeyID, id)
	val := loadFromCache(key)

	// 非空则取缓存数据
	if val != nil {
		campaignData = val.(*entity.CampaignData)
		return
	}

	err = util.GetDB().First(campaignData, id).Error
	if err != nil {
		return
	}

	// 走DB则设置缓存
	saveToCache(key, campaignData)

	return
}
