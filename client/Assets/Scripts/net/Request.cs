public class Request : Message
{
   // public int userId;
    public string serviceName = "";
    public string responseName = "";
    //public string GameUid;
    //public string GameToken;
    //public int SeqId;

    public Request()
    {
        msgId = msgCounter++;
        //GameUid = HttpManager.playerid+"";
        //GameToken = HttpManager.token;
        //SeqId = msgId;


    }

}
