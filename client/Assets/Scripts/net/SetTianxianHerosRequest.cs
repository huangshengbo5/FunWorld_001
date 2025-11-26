internal class SetTianxianHerosRequest:Request
{
    public int id;
    public int[] heroIDs;
    public SetTianxianHerosRequest()
    {
        this.serviceName = "/api/explore/set";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public SetTianxianHerosRequest(int curId, int[] vs):this()
    {
        this.id = curId;
        this.heroIDs = vs;
    }
}