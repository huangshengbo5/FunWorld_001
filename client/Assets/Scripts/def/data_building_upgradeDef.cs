using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class data_building_upgradeBean
{
    public int id;
    public int building_id;
    public int level;
    public int condition_type;
    public int condition_val;
    public int cost_type;
    public int cost_sub_type;
    public int cost_val;
    public int effect_id;
    public int effect_val;
    public string desc;

    public Reward upcost;
    public Reward getlvcost()
    {
        if (upcost == null)
        {
            upcost = new Reward(cost_type, cost_sub_type, cost_val);
        }
        return upcost;
    }
}
public class data_building_upgradeDef
{
    public static int[] types;
    public static List<data_building_upgradeBean> datas = new List<data_building_upgradeBean>();
    public static Dictionary<int, List<data_building_upgradeBean>> dicdatas = new Dictionary<int, List<data_building_upgradeBean>>();
    public static void load()
    {
        string path = CTGlobal.defRoot + "data_building_upgrade.bin";
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
                data_building_upgradeBean row = new data_building_upgradeBean();
                din.resetReadTimes();
                row.id = din.readInt();
                row.building_id = din.readInt();
                row.level = din.readInt();
                row.condition_type = din.readInt();
                row.condition_val = din.readInt();
                row.cost_type = din.readInt();
                row.cost_sub_type = din.readInt();
                row.cost_val = din.readInt();
                row.effect_id = din.readInt();
                row.effect_val = din.readInt();
                row.desc = din.readUTF();
                DefTools.skipNewValue(din, types);
                datas.Add(row);
                if (dicdatas.ContainsKey(row.building_id))
                {
                    List<data_building_upgradeBean> tempdatas = dicdatas[row.building_id];
                    tempdatas.Add(row);
                }
                else
                {
                    List<data_building_upgradeBean> tempdatas = new List<data_building_upgradeBean>();
                    tempdatas.Add(row);
                    dicdatas.Add(row.building_id, tempdatas);
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
