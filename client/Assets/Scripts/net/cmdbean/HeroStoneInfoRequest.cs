public class HeroStoneInfoRequest : Request
{
    public HeroStoneInfoRequest()
    {
        this.serviceName = "/api/crystal/list";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}