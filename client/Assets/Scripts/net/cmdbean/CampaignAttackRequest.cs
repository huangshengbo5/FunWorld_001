using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CampaignAttackRequest : Request
{
    public int campaignID;
    public CampaignAttackRequest()
    {
        this.serviceName = "/api/campaign/attack";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public CampaignAttackRequest(int campaignID) : this()
    {
        this.campaignID = campaignID;
    }
}
