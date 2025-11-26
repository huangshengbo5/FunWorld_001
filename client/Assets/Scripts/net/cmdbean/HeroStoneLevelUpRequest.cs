public class HeroStoneLevelUpRequest : Request
{
    public int id;
    public HeroStoneLevelUpRequest()
    {
        this.serviceName = "/api/crystal/upgrade";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public HeroStoneLevelUpRequest(int id):this()
    {
        this.id = id;
    }
}