using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;


public class DefTools
{
    public static readonly int BYTE = 0, SHORT = 1, INT = 2, STRING = 3, FLOAT = 4, LONG = 5, BOOLEAN = 6, REWARD_BEAN = 7, REWARD_BEANS = 8;

    public static void skipNewValue(JavaReader din, int[] types)
    {
        int readTimes = din.readTimes;
     //   Debug.LogError(readTimes);
        if (readTimes == types.Length) return;

        if(readTimes < types.Length)
        {
            for(int i = readTimes; i < types.Length; i++)
            {
                int type = types[i];

                if(type == BYTE)
                {
                    din.readByte();
                }
                else if (type == SHORT)
                {
                    din.readShort();
                }
                else if (type == INT)
                {
                    din.readInt();
                }
                else if (type == STRING)
                {
                    din.readUTF();
                }
                else if (type == FLOAT)
                {
                    din.readFloat();
                }
                else if (type == LONG)
                {
                    din.readLong();
                }
                else if (type == BOOLEAN)
                {
                    din.readBoolean();
                }
                else if (type == REWARD_BEAN)
                {
                    din.readUTF();
                }
                else if (type == REWARD_BEANS)
                {
                    din.readUTF();
                }
            }
        }else
        {
            Debug.LogError("读取表的列 变少了？");
        }

    }

    public static JavaReader getSdCardResourcedef(string filePath)
    {
        JavaReader jr = null;

        //不适用streamasset资源 先从sd卡加载
        if (!CTGlobal.useStreamingAssets)
        {
            jr = getFileDataInputStream(CTGlobal.mainResourcePath + filePath);
        }

        if (jr == null)
        {
            jr = getStreamingAssetsData(filePath);
        }

        if (jr == null)
        {
            Debug.Log("找不到该文件=======" + filePath);
        }


        return jr;
    }

    public static List<int> getsplitlistint(string yoke)
    {
        string[] splitstr = yoke.Split('|');
        List<int> templist = new List<int>();
        for(int i = 0; i < splitstr.Length; i++)
        {
            templist.Add(int.Parse(splitstr[i]));
        }
        return templist;
    }
    public static List<float> getsplitlistfloat(string yoke)
    {
        string[] splitstr = yoke.Split('|');
        List<float> templist = new List<float>();
        for (int i = 0; i < splitstr.Length; i++)
        {
            templist.Add(float.Parse(splitstr[i]));
        }
        return templist;
    }
    //public static List<RewardBean> getsplitlistreward(string yoke)
    //{
    //    string[] splitstr = yoke.Split('|');
    //    List<RewardBean> templist = new List<RewardBean>();
    //    for (int i = 0; i < splitstr.Length; i++)
    //    {
    //        templist.Add(new RewardBean(splitstr[i],3));
    //    }
    //    return templist;
    //}
    public static JavaReader getFileDataInputStream(string path)
    {
        FileHandler file = new FileHandler(path);
        if (file.exist() == false)
        {
            return null;
        }

        MemoryStream ms = new MemoryStream(file.getData());

        JavaReader jr = new JavaReader();
        jr.setBinaryReader(new BinaryReader(ms));
        return jr;
    }
    public static JavaReader getStreamingAssetsData(string filePath)
    {

        string url = UnityEngine.Application.streamingAssetsPath + "/" + filePath;

        byte[] data = null;

        try
        {
#if UNITY_EDITOR
            WWW www = new WWW("file://" + url);
            while (!www.isDone) { }
            data = www.bytes;
#elif UNITY_ANDROID

            Debug.Log("尝试读取android assets---  "+url);

            WWW www = new WWW(url);
            while (!www.isDone) { }
            data = www.bytes;
            //使用android自带的方法
            //data = AndroidPlatform.getInstance().getBytes(filePath);
#elif UNITY_IPHONE
            WWW www = new WWW("file://"+url);
            while (!www.isDone) { }
            data = www.bytes;
#else
            data = File.ReadAllBytes(url);
#endif
        }
        catch (Exception e)
        {
            Debug.LogWarning("未读取到文件===============" + url + "\n" + e.StackTrace);
            return null;
        }

        if (data.Length < 1)
        {
            Debug.LogWarning("未读取到文件===============" + url);
            return null;
        }

        JavaReader jr = new JavaReader();
        jr.setBinaryReader(new BinaryReader(new MemoryStream(data)));
        return jr;
    }

}
