internal class ChangeNameRequest:Request
{
    public string name;
    public ChangeNameRequest()
    {
        this.serviceName = "/api/lobby/change/name";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public ChangeNameRequest(string newname) : this()
    {
        name = newname;
    }
}