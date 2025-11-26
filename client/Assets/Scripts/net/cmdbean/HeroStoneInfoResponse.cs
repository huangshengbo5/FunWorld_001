public class HeroStoneInfoResponse : Response
{
    public HeroStoneData[] Crystals;
}
public class HeroStoneData
{
    public int  id;
    public int CrystalID;
    public int level;
}