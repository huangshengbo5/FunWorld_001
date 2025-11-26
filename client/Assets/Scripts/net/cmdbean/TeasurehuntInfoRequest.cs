public class TeasurehuntInfoRequest : Request
{
    public int Type;
    public string adsID;
    public TeasurehuntInfoRequest()
    {
        this.serviceName = "/api/lobby/onlinereward/show";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public TeasurehuntInfoRequest(int getType,string adId) :this()
    {
        this.Type = getType;
        this.adsID = adId;
    }
}