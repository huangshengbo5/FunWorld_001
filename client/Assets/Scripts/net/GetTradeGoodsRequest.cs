internal class GetTradeGoodsRequest : Request
{
    public int type;
    public string adsID;
    public GetTradeGoodsRequest()
    {
        this.serviceName = "/api/lobby/businessman/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public GetTradeGoodsRequest(int getType, string aDIdstr):this()
    {
        this.type = getType;
        this.adsID = aDIdstr;
    }
}