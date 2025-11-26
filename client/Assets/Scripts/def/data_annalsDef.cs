using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class data_annalsBean
{
    public int id;
    public int type;
    public int sub_type;
    public int value;
    public string name;
    public string describe;
    public string rewards;
}
public class data_annalsDef
{
    public static int[] types;
    public static List<data_annalsBean> datas = new List<data_annalsBean>();
    public static Dictionary<int, List<data_annalsBean>> dicdatas = new Dictionary<int, List<data_annalsBean>>();
    public static void load()
    {
        string path = CTGlobal.defRoot + "data_annals.bin";
        JavaReader jr = DefTools.getSdCardResourcedef(path);
        load(jr);
    }
    public static void load(JavaReader din)
    {
        datas.Clear();
        try
        {
            int len = din.readInt();
            types = new int[len];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = din.readByte();
            }
            int dataLen = din.readInt();
            for (int i = 0; i < dataLen; i++)
            {
                data_annalsBean row = new data_annalsBean();
                din.resetReadTimes();
                row.id = din.readInt();
                row.type = din.readInt();
                row.sub_type = din.readInt();
                row.value = din.readInt();
                row.name = din.readUTF();
                row.describe = din.readUTF();
                row.rewards = din.readUTF();
                DefTools.skipNewValue(din, types);
                datas.Add(row);
                if (dicdatas.ContainsKey(row.id))
                {
                    List<data_annalsBean> tempdatas = dicdatas[row.id];
                    tempdatas.Add(row);
                }
                else
                {
                    List<data_annalsBean> tempdatas = new List<data_annalsBean>();
                    tempdatas.Add(row);
                    dicdatas.Add(row.id, tempdatas);
                }
            }
            din.close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }

    }
}
