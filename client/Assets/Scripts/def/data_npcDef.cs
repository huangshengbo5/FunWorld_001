using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class data_npcBean
{
    public int id;
    public int level;
    public string name;
    public int sex;
    public int race;
    public string resource;
    public string avatar;
    public int zoom_ratio;
    public int attack_freq;
    public int fighting_capacity;
    public int critical;
    public int tenacity;
    public int breakwhat;
    public int impregnable;
    public int hit;
    public int dodge;
    public int defuse;
    public int skill1_id;
    public int skill2_id;
    public int skill3_id;
    public int skill4_id;
    public int skill5_id;
    public int skill6_id;
    public int attack_plus;
    public int defend_plus;
    public int avatar_face_id;

}
public class data_npcDef
{
    public static int[] types;
    public static List<data_npcBean> datas = new List<data_npcBean>();
    public static Dictionary<int, List<data_npcBean>> dicdatas = new Dictionary<int, List<data_npcBean>>();
    public static void load()
    {
        string path = CTGlobal.defRoot + "data_npc.bin";
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
                data_npcBean row = new data_npcBean();
                din.resetReadTimes();
                row.id = din.readInt();
                row.level = din.readInt();
                row.name = din.readUTF();
                row.sex = din.readInt();
                row.race = din.readInt();
                row.resource = din.readUTF();
                row.avatar = din.readUTF();
                row.zoom_ratio = din.readInt();
                row.attack_freq = din.readInt();
                row.fighting_capacity = din.readInt();
                row.critical = din.readInt();
                row.tenacity = din.readInt();
                row.breakwhat = din.readInt();
                row.impregnable = din.readInt();
                row.hit = din.readInt();
                row.dodge = din.readInt();
                row.defuse = din.readInt();
                row.skill1_id = din.readInt();
                row.skill2_id = din.readInt();
                row.skill3_id = din.readInt();
                row.skill4_id = din.readInt();
                row.skill5_id = din.readInt();
                row.skill6_id = din.readInt();
                row.attack_plus = din.readInt();
                row.defend_plus = din.readInt();
                row.avatar_face_id = din.readInt();
                DefTools.skipNewValue(din, types);
                datas.Add(row);
                if (dicdatas.ContainsKey(row.level))
                {
                    List<data_npcBean> tempdatas = dicdatas[row.level];
                    tempdatas.Add(row);
                }
                else
                {
                    List<data_npcBean> tempdatas = new List<data_npcBean>();
                    tempdatas.Add(row);
                    dicdatas.Add(row.level, tempdatas);
                }
            }
            din.close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }

    }
    public static data_npcBean getdatabynpcid(int heroID)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].id == heroID)
            {
                return datas[i];
            }
        }
        return null;
    }
}
