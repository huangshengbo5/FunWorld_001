using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class data_equipBean
{
    public int id;
    public string icon;
    public int skill_id;
    public string name;
    public string rewards;
    public string skill_desc;
    public string prop1_desc;
    public string prop2_desc;
    public string prop3_desc;
    public string fighting_capacity;
    public int weight;

}
public class data_equipDef
{
    public static int[] types;
    public static List<data_equipBean> datas = new List<data_equipBean>();
    public static Dictionary<int, List<data_equipBean>> dicdatas = new Dictionary<int, List<data_equipBean>>();
    public static Dictionary<int, List<data_equipBean>> dicdatas2 = new Dictionary<int, List<data_equipBean>>();
    public static void load()
    {
        string path = CTGlobal.defRoot + "data_equip.bin";
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
                data_equipBean row = new data_equipBean();
                din.resetReadTimes();
                row.id = din.readInt();
                row.icon = din.readUTF();
                row.skill_id = din.readInt();
                row.name = din.readUTF();
                row.rewards = din.readUTF();
                row.skill_desc = din.readUTF();
                row.prop1_desc = din.readUTF();
                row.prop2_desc = din.readUTF();
                row.prop3_desc = din.readUTF();
                row.fighting_capacity = din.readUTF();
                row.weight = din.readInt();
                DefTools.skipNewValue(din, types);
                datas.Add(row);
                {
                    if (dicdatas.ContainsKey(row.id))
                    {
                        List<data_equipBean> tempdatas = dicdatas[row.id];
                        tempdatas.Add(row);
                    }
                    else
                    {
                        List<data_equipBean> tempdatas = new List<data_equipBean>();
                        tempdatas.Add(row);
                        dicdatas.Add(row.id, tempdatas);
                    }
                }
                {
                    if (dicdatas2.ContainsKey(row.skill_id))
                    {
                        List<data_equipBean> tempdatas = dicdatas[row.skill_id];
                        tempdatas.Add(row);
                    }
                    else
                    {
                        List<data_equipBean> tempdatas = new List<data_equipBean>();
                        tempdatas.Add(row);
                        dicdatas2.Add(row.skill_id, tempdatas);
                    }
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
