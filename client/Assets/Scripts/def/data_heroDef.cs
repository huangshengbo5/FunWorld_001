using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class data_heroBean
{
    public int id;
    public int type;
    public string name;
    public int sex;
    public int race;
    public string resource;
    public string face;
    public string face_square;
    public int zoom_ratio;
    public int attack_freq;
    public int skill1_id;
    public int skill2_id;
    public int skill3_id;
    public int hp_ratio;
    public int attack_ratio;
    public int defend_ratio;
    public int campaign_id;
    public int level;
    public int hero_location;

}
public class data_heroDef
{
    public static int[] types;
    public static List<data_heroBean> datas = new List<data_heroBean>();
    public static Dictionary<int, List<data_heroBean>> dicdatas = new Dictionary<int, List<data_heroBean>>();
    public static void load()
    {
        string path = CTGlobal.defRoot + "data_hero.bin";
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
                data_heroBean row = new data_heroBean();
                din.resetReadTimes();
                row.id = din.readInt();
                row.type = din.readInt();
                row.name = din.readUTF();
                row.sex = din.readInt();
                row.race = din.readInt();
                row.resource = din.readUTF();
                row.face = din.readUTF();
                row.face_square = din.readUTF();
                row.zoom_ratio = din.readInt();
                row.attack_freq = din.readInt();
                row.skill1_id = din.readInt();
                row.skill2_id = din.readInt();
                row.skill3_id = din.readInt();
                row.hp_ratio = din.readInt();
                row.attack_ratio = din.readInt();
                row.defend_ratio = din.readInt();
                row.campaign_id = din.readInt();
                row.level = din.readInt();
                row.hero_location = din.readInt();
                DefTools.skipNewValue(din, types);
                datas.Add(row);
                if (dicdatas.ContainsKey(row.type))
                {
                    List<data_heroBean> tempdatas = dicdatas[row.type];
                    tempdatas.Add(row);
                }
                else
                {
                    List<data_heroBean> tempdatas = new List<data_heroBean>();
                    tempdatas.Add(row);
                    dicdatas.Add(row.type, tempdatas);
                }
            }
            din.close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }

    }
    public static data_heroBean getdatabyheroid(int heroID)
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
