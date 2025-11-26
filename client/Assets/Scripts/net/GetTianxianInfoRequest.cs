public class GetTianxianInfoRequest : Request
{
    public GetTianxianInfoRequest()
    {
        this.serviceName = "/api/explore/list";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}