public class TeasurehuntRewardGetRequest:Request
{
    public int Type;
    public string adsID;
    public TeasurehuntRewardGetRequest()
    {
        this.serviceName = "/api/lobby/onlinereward/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public TeasurehuntRewardGetRequest(int getType,string adId) : this()
    {
        this.Type = getType;
        this.adsID = adId;
    }
}