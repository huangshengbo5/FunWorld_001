using System;

public delegate void callbackInt(int code);
public delegate void callbackObj(object obj);
public delegate void callbackIntObj(int code, Object obj);

public class Callback
{

    public bool free;

    public static readonly int SUCCESS = 0, FAILED = 1;

    public static readonly int BUTTON0 = 10, BUTTON1 = 11, BUTTON2 = 12;

    public static readonly int SHOW = 20, HIDE = 21, DISMISS = 22;

    public void callback(callbackInt cb)
    {
        free = true;
    }

    public void callback(callbackObj cb)
    {
        free = true;
    }

    public void callback(callbackIntObj cb)
    {
        free = true;
    }

}
