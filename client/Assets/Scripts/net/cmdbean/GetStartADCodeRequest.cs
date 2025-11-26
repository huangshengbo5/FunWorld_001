public class GetStartADCodeRequest:Request
{
    public GetStartADCodeRequest()
    {
        this.serviceName = "/api/ads/start";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}