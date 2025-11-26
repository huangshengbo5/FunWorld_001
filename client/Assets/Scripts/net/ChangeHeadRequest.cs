internal class ChangeHeadRequest:Request
{
    public int id;
    public ChangeHeadRequest()
    {
        this.serviceName = "/api/lobby/change/avatar";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public ChangeHeadRequest(int headiconId) : this()
    {
        id = headiconId;
    }
}