using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class data_campaignBean
{
    public int id;
    public int type;
    public string name;
    public int fighting_capacity;
    public int prev_id;
    public int next_id;
    public int npc_id;
    public string rewards;
    public int spot;
    public int prebattledialogue_id;
    public int afterbattledialogue_id;
    public int hero_id;
    public int background_id;
}
public class data_campaignDef
{
    public static int[] types;
    public static List<data_campaignBean> datas = new List<data_campaignBean>();

    public static Dictionary<int, data_campaignBean> dataDic = new Dictionary<int, data_campaignBean>();
    public static void load()
    {
        string path = CTGlobal.defRoot + "data_campaign.bin";
   //     Debug.LogError("loaddata_campaign.bin");
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
                data_campaignBean row = new data_campaignBean();
                din.resetReadTimes();
                row.id = din.readInt();
                row.type = din.readInt();
                row.name = din.readUTF();
                row.fighting_capacity = din.readInt();
                row.prev_id = din.readInt();
                row.next_id = din.readInt();
                row.npc_id = din.readInt();
                row.rewards = din.readUTF();
                row.spot = din.readInt();
                row.prebattledialogue_id = din.readInt();
                row.afterbattledialogue_id = din.readInt();
                row.hero_id = din.readInt();
                row.background_id = din.readInt();
                DefTools.skipNewValue(din, types);
                datas.Add(row);
                dataDic.Add(row.id,row);
            }
            din.close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }

    }
}

