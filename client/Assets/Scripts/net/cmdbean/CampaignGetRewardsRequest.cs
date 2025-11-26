internal class CampaignGetRewardsRequest:Request
{
    private int type;
    private string adsID;
    public CampaignGetRewardsRequest()
    {
        this.serviceName = "/api/campaign/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public CampaignGetRewardsRequest(int type, string adid):this()
    {
        this.type = type;
        this.adsID = adid;
    }
}