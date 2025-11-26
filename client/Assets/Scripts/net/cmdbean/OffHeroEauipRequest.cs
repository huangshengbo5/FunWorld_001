public class OffHeroEauipRequest:Request
{
    public int id;
    public OffHeroEauipRequest()
    {
        this.serviceName = "/api/equip/unload";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public OffHeroEauipRequest(int id):this()
    {
        this.id = id;
    }
}