public class HistoryInfoRequest : Request
{
    public HistoryInfoRequest()
    {
        this.serviceName = "/api/lobby/annals/show";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}