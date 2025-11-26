public  class BuffGoldAddRequest:Request
{
    public string adsID;
    public BuffGoldAddRequest()
    {
        this.serviceName = "/api/ads/add/buff";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public BuffGoldAddRequest(string adid) : this()
    {
        this.adsID = adid;
    }

}