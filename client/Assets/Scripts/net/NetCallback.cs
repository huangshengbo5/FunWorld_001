using System;

public delegate void nettyAction(Response response);

public class NetCallback
{

    public const long MESSAGE_TIMEOUT = 60 * 1000L;

    public int callbackId;
    public long time;
    public long timeout;

    //  public string protocalName = "";

    private nettyAction action = null;

    public const int SUCCESS = 0, FAILED = 1, TIMEOUT = 2;

    public NetCallback()
    {
        {
            time = currentTimeMillis();
            timeout = MESSAGE_TIMEOUT;

        }
    }
    private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static long currentTimeMillis()
    {
        return (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
    }
    public void setTimeout(long timeout)
    {
        this.timeout = timeout;
    }

    public void setAction(nettyAction action)
    {
        this.action = action;
    }

    public void callback(Response response)
    {
        if (action != null)
        {
            action(response);
        }
    }

    public static NetCallback create(nettyAction action)
    {
        NetCallback back = new NetCallback();
        back.setAction(action);

        return back;
    }

}
