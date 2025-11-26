public class HistoryInfoResponse : Response
{
    public int[] annalsIDs;
    public int campaignNum;
    public int fightingCapacity;
    public HeroPowerData[] heroList;
}
public struct HeroPowerData
{
    public int fightingCapacity;
    public int heroID;
}