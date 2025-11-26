using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward 
{
    public int mainType;
    public int subType;
    public long val;

    public Reward()
    {
    }

    public Reward(int cost_type, int cost_sub_type, long cost_val)
    {
        this.mainType = cost_type;
        this.subType = cost_sub_type;
        this.val = cost_val;
    }

    public static Reward decode(string code)
    {

        string[] attrs = code.Split('_');

        if (code.Equals("") || attrs.Length < 2)
        {
            return null;
        }

        Reward bean = new Reward();
        try
        {
            bean.mainType = Convert.ToInt32(attrs[0]);

            int value2 = Convert.ToInt32(attrs[1]);
            bean.subType = value2;


            bean.val = Convert.ToInt32(attrs[2]);
        }
        catch (Exception e)
        {

        }

        return bean;
    }

    public static List<Reward> decodeList(string code)
    {
        List<Reward> rewards = new List<Reward>();
        if (code.Equals("") || code.Equals("-1") || code.Length < 2)
        {
            return rewards;
        }
        string[] rewardStr = code.Replace(";", " ").Replace("|", " ").Split(' ');
        for (int j = 0; j < rewardStr.Length; j++)
        {
            Reward bean = new Reward();
            string[] attrs = rewardStr[j].Split('_');
            bean.mainType = Convert.ToInt32(attrs[0]);

            int value2 = Convert.ToInt32(attrs[1]);
            bean.subType = value2;


            bean.val = Convert.ToInt32(attrs[2]);

            rewards.Add(bean);
        }
        return rewards;
    }
}
