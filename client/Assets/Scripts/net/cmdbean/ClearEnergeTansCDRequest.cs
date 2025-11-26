public class ClearEnergeTansCDRequest:Request
{
    public ClearEnergeTansCDRequest()
    {
        this.serviceName = "/api/lobby/alchemy/clear";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}