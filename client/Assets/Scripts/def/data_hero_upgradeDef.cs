using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class data_hero_upgradeBean
{
    public int id;
    public int hero_id;
    public int level;
    public int break_level;
    public int fighting_capacity;
    public int cost_type;
    public int cost_sub_type;
    public int cost_val;
    public int skill1_level;
    public int skill2_level;
    public int skill3_level;
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
public class data_hero_upgradeDef
{
    public static int[] types;
    public static List<data_hero_upgradeBean> datas = new List<data_hero_upgradeBean>();
    public static Dictionary<int, List<data_hero_upgradeBean>> dicdatas = new Dictionary<int, List<data_hero_upgradeBean>>();
    public static void load()
    {
        string path = CTGlobal.defRoot + "data_hero_upgrade.bin";
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
                data_hero_upgradeBean row = new data_hero_upgradeBean();
                din.resetReadTimes();
                row.id = din.readInt();
                row.hero_id = din.readInt();
                row.level = din.readInt();
                row.break_level = din.readInt();
                row.fighting_capacity = din.readInt();
                row.cost_type = din.readInt();
                row.cost_sub_type = din.readInt();
                row.cost_val = din.readInt();
                row.skill1_level = din.readInt();
                row.skill2_level = din.readInt();
                row.skill3_level = din.readInt();
                DefTools.skipNewValue(din, types);
                datas.Add(row);
                if (dicdatas.ContainsKey(row.hero_id))
                {
                    List<data_hero_upgradeBean> tempdatas = dicdatas[row.hero_id];
                    tempdatas.Add(row);
                }
                else
                {
                    List<data_hero_upgradeBean> tempdatas = new List<data_hero_upgradeBean>();
                    tempdatas.Add(row);
                    dicdatas.Add(row.hero_id, tempdatas);
                }
            }
            din.close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }

    }

    public static int getNextSkillNeedHeroLevel(int heroId,int skillIndex, int Currskilllevel)
    {
        int needHeroLevel = -1;
        int nextSkillLevel = Currskilllevel + 1;
        var herodatas = dicdatas[heroId];
        foreach (var item in herodatas)
        {
            if (skillIndex == 1)
            {
                if (item.skill1_level == nextSkillLevel)
                {
                    needHeroLevel = item.level;
                    break;
                }
            }
            else if (skillIndex == 2)
            {
                if (item.skill2_level == nextSkillLevel)
                {
                    needHeroLevel = item.level;
                    break;
                }
            }
            else if (skillIndex == 3)
            {
                if (item.skill3_level == nextSkillLevel)
                {
                    needHeroLevel = item.level;
                    break;
                }
            }

        }

        return needHeroLevel;
    }
}
