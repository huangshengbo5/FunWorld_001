using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class PlatformInterface
{
    public AndroidJavaObject jo = null;
    public void init()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");   //获取unity的Java类,只能调用静态方法，获取静态属性  
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");      //获取当前的Activity对象,能调用公开方法和公开属性  
#endif
    }

    private static PlatformInterface instance;
    public static PlatformInterface getInstance()
    {
        if (instance == null)
        {
            instance = new PlatformInterface();
        }
        return instance;
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void Internal_copyword(string copyBoardContent);
#endif
    public void copyword(string copyBoardContent)
    {
#if UNITY_EDITOR
        NGUITools.clipboard = copyBoardContent;
#elif UNITY_ANDROID
   //   jo.Call("copyword", copyBoardContent);
#elif UNITY_IOS
       Internal_copyword(copyBoardContent);
#endif
    }



    public string getDeviceId()
    {
        string data = "";


#if UNITY_EDITOR
        data = "pc_" + SystemInfo.deviceUniqueIdentifier;
#elif UNITY_ANDROID
    //    data = jo.Call<string>("getDeviceId");
#elif UNITY_IOS
        data = SystemInfo.deviceUniqueIdentifier;
#else
        string s = "";
        for(int i = 0; i < 9; i++)
        {
            s += CTTools.getRandom(9);
        }
        return "pc_"+s;
#endif

        return data;
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern string Internal_getDeviceInfo();
#endif
    public string getDeviceInfo()
    {
        string data = "";
#if UNITY_EDITOR
        data = "pc_" + SystemInfo.deviceModel;
#elif UNITY_ANDROID
    //    data += jo.Call<string>("getDeviceInfo");
#elif UNITY_IOS
        data = Internal_getDeviceInfo();
#else
        return "pc_"+ + CTTools.getRandom(1000000000);
#endif

        return data;
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern string Internal_getIMEI();
#endif
    public string getIMEI()
    {
        string data = "";
#if UNITY_EDITOR
        data = "pc_" + SystemInfo.graphicsDeviceID / 1000 + "" + CTTools.getRandom(1000);
#elif UNITY_ANDROID
     //   data += jo.Call<string>("getIMEI");
#elif UNITY_IOS
        return Internal_getIMEI();
#endif
        return data;
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern string Internal_getUserSource();
#endif

    public static int DefulatSource = 9000000;
    public static int userSource = -999;
    public int getUserSource()
    {

        if (userSource == -999)
        {
#if UNITY_EDITOR
            userSource = DefulatSource;
#elif UNITY_ANDROID
     //   userSource =  jo.Call<string>("getUserSource");
#elif UNITY_IOS
        userSource = Internal_getUserSource();
#else
        userSource =  DefulatSource;
#endif
        }
        return userSource;
    }


#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern int Internal_getVersionCode();
#endif

    public static int versionCode = -999;
    public int getVersionCode()
    {

        if (versionCode == -999)
        {

            versionCode = 11081;
#if UNITY_EDITOR

#elif UNITY_ANDROID
       //     versionCode =jo.Call<string>("getVersionCode");
#elif UNITY_IOS
        versionCode =  Internal_getVersionCode();
#endif
        }
        return versionCode;
    }
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern int Internal_getGISLocationBySystem();
#endif
    public double[] getGISLocationBySystem()
    {
#if UNITY_EDITOR
        return new double[] { 39.910958, 116.459498 };// 建外SOHO西区
#elif UNITY_ANDROID
         return new double[] { 39.910958, 116.459498 };// 建外SOHO西区
     //   return jo.Call<string>("getGISLocationBySystem");
#elif UNITY_IOS
        return Internal_getGISLocationBySystem();
#else
        return new double[] { 39.910958, 116.459498 };// 建外SOHO西区
#endif

    }
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern int Internal_getPhoneNumber();
#endif
    public string getPhoneNumber()
    {
#if UNITY_EDITOR
        string str = "";
        for (int i = 0; i < 11; i++)
        {
            str += CTTools.rd.Next(9);
        }

        return str;
#elif UNITY_ANDROID
        return "";
      //  return jo.Call<string>("getPhoneNumber");
#elif UNITY_IOS
        return Internal_getPhoneNumber();
#else
        string str = "";
        for (int i = 0; i < 11; i++)
        {
             str += CTTools.rd.Next(9);
        }

        return str;
#endif
    }

    public const int NETWORK_NULL = 0, NETWORK_WIFI = 1, NETWORK_MOBILE = 2;

    public int getNetworkType()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            return NETWORK_MOBILE;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return NETWORK_WIFI;
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return NETWORK_NULL;
        }
        return NETWORK_NULL;
    }

}
