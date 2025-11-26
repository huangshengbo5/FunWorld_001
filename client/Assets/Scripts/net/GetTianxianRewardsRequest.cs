public class GetTianxianRewardsRequest:Request
{
    public int id;
    public GetTianxianRewardsRequest()
    {
        this.serviceName = "/api/explore/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public GetTianxianRewardsRequest(int curId) : this()
    {
        this.id = curId;
    }
}