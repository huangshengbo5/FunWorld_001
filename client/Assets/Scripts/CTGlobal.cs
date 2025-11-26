using System;
using System.Net;
using UnityEngine;

public class CTGlobal
{
    public const int FPS = 32;
    public const int INTEVAL = 1000 / FPS;

    public static int FRAME_PER_MM = 30;
    public static float SPRITE_RES_SCALE = 1f;
    public static float AVATAR_RES_SCALE = 1f;
    public static int scrWidth = 1080;
    public static int scrHeight = 2340;
    public static int realWidth;
    public static int realHeight;
    public static bool ispad = false;
   // public static bool useugui = false;
  //  public static bool usesprmod = false;
  //  public static bool jiazuo = true;
    public static bool paneldepth = true;
    public static bool renhetuflag = false;
    //   public static bool paneldepthinrole = false;
   // public static bool killpanel = true;

    public static bool addpanel = true;

    public static bool banbanpanel = true;

    public static bool paotootherpanel = true;

    public static bool nametootherpanel = true;


    public static int timeZone = 8;// 时区

    public CTGlobal()
    {
    }
    //是否强制使用内部资源
    public static bool useStreamingAssets = true;
    //主存档目录
    public static string mainArchivePath = "";

    //资源读取的跟目录
    public static string mainResourcePath = "D:/Android/data/catstudio/nekostory/assets/";
    public static string bgRoot = "bg/";
    public static string defRoot = "def/";
    public static string effectRoot = "Effect/";
    public static string exproRoot = "Expro/";
    public static string live2dRoot = "live2d/";
    public static string luaRoot = "Lua/";
    public static string luaresRoot = "luares/";
    public static string myluaRoot = "mylua/";
    public static string roleRoot = "roledraw/";
    public static string roomRoot = "room/";
    public static string spriteRoot = "sprite/";
    public static string spriteHRoot = "spriteH/";
    public static string audioRoot = "Audio/";

    public static void initRoot()
    {
        mainArchivePath = Application.persistentDataPath + "//project3/";


#if UNITY_EDITOR
        mainResourcePath = System.Environment.CurrentDirectory + "/data/catstudio/nekostory/assets/";

        /* //如果是自己的电脑
         Debug.Log("本机名称=======" + Dns.GetHostName());
         if(Dns.GetHostName().Equals("wangjinqiang"))
         {
#if (isDeveloping)
             mainResourcePath = System.Environment.CurrentDirectory + "/data/catstudio/nekostory/assets/";
#else
             mainResourcePath = System.Environment.CurrentDirectory+ "/data/catstudio/nekostory/assets/";
#endif
         }*/



#elif UNITY_STANDALONE_WIN
        mainResourcePath = System.Environment.CurrentDirectory + "/data/catstudio/nekostory/assets/";
#elif UNITY_ANDROID
      //  string sdCardRoot = JniHandler.getInstance().getSdCardRoot();
     //   mainResourcePath = sdCardRoot +"/Android/data/catstudio/nekostory/assets/";
#elif UNITY_IOS
        mainResourcePath = Application.persistentDataPath+"/Assets/";
#else
        mainResourcePath = Application.persistentDataPath+"/Assets/";
#endif
    }
}

