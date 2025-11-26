public class WorldBoosGetRewardsRequest:Request
{
    public int apocalypseID;
    public int type;
    public string adsID;
    public WorldBoosGetRewardsRequest()
    {
        this.serviceName = "/api/apocalypse/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public WorldBoosGetRewardsRequest(int id,int type, string adid):this()
    {
        this.type = type;
        this.adsID = adid;
        this.apocalypseID = id;
    }
}