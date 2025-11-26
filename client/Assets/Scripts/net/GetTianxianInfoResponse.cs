public class GetTianxianInfoResponse:Response
{
    public explore[] explores;
    public Hero[] heros;
}

    public class explore
{
    public int exploreID;
    public int[] heroIDs;
    public int id;
    public double mul;
    public int startTime;
    public int[] herodbIDs;
    public data_exploreBean db;
}