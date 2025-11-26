public class HistoryRewardsRequest :Request
{
    public int id;
    public HistoryRewardsRequest()
    {
        this.serviceName = "/api/lobby/annals/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public HistoryRewardsRequest(int id):this()
    {
        this.id = id;
    }
}