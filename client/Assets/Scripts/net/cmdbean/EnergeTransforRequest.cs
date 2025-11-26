public class EnergeTransforRequest:Request
{
    public int type;
    public string adsID;
    public EnergeTransforRequest()
    {
        this.serviceName = "/api/lobby/alchemy/start";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public EnergeTransforRequest(int type,string adid):this()
    {
        this.type = type;
        this.adsID = adid;
    }
}