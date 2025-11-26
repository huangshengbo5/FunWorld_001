public class Response : Message
{
    public const int WAIT_RESULT = -1; // 等待返回结果
    public const int SUCCESS = 0; // 成功
    public const int FAILED = 404; // 失败

    public int code = WAIT_RESULT;//返回结果码
    public string msg;
    public int sysTime;

    public string exInfo = "";

    public Response()
    {
    }

    public Response(int code, int msgId)
    {
        this.code = code;
        this.msgId = msgId;
    }

    public void setExtraInfo(string info)
    {
        this.exInfo = info;
    }

}
