using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class FightObjBean
{

    public int maxhp;

    public int avatar;
    public int fType;
    public int fightingCapacity;
    public int heroID;
    public int hp;
    public int id;
    public int level;
    public string name;

}
public class FightBeanResult
{
    public int Result; //0平 1攻击方胜利 2防守方胜利
    public int AttackerHP; //剩余血量
    public int DefenderHP; //剩余血量
}
////指令--添加buff
//[2,2,123,101,0,100,0.1,0.2],  
//    //第一位指令类型  2 buff
//    //第二位buff状态  0 结算 1 清除  2创建
//    //第三位目标武将ID 
//    //第四位buffid   
//    //第五位剩余时间 单位毫秒
//    //第六位效果值1
//    //第七位效果值2
public class FightBeanAction
{
    public int type;//0行动1行动结果2 buff 3代表行动结束
    public long timestamp;
    public int fromid;
    public int toid;
    public int skilltype;
    public int skillid;
    public bool hitresult;
    public bool doubleresult;
    public int exskillmsg;

    public int hpcost;

    public int buffopttype;//0 结算 1 清除  2创建
    public int buffid;
    public long bufftime;
    public int buffeff0;
    public int buffeff1;

    public FightBeanAction resultaction;

    public List<FightBeanAction> buffactionlist = new List<FightBeanAction>();


    private object[] v;
    public FightBeanAction(object[] v)
    {
        this.v = v;
        type = (int)v[0];
        //        //指令--行动
        //[0,100,123,456,1,1001,0,1],  
        ////第一位代表指令类型 0开始行动  1行动结果   2buff 3行动结束
        ////第二位代表行动开始时间， 100代表距离开始时间多少毫秒
        ////第三位代表攻击方id
        ////第四位代表防御方id
        ////第五位代表技能类型（暂时无意义）
        ////第六位代表技能ID
        ////第七位代表是否命中  1命中  0否
        ////第八位代表是否暴击  1暴击  0否
        ///    //指令--结果-闪避
        //[1,0,123,456,0],  
        ////第一位指令类型 1行动结果
        ////第二位是否命中 0闪避 1命中
        ////第三位攻击方id
        ////第四位防御方ID
        ////第五位伤害值0
        ///    //事件-清除buff
        //[2,1,123,101,0,100,0.1,0.2],  
        ////第一位指令类型  2 buff
        ////第二位buff状态  0 结算 1 清除  2创建
        ////第三位目标武将ID 
        ////第四位buffid   
        ////第五位剩余时间 单位毫秒
        ////第六位效果值1
        ////第七位效果值2
        ////第八位 触发时间 单位毫秒
        ///[2,2,3,5,10000,100,0],
        if (type == 0)
        {
          
            timestamp = Convert.ToInt64(v[1]);
            fromid = (int)v[2];
            toid = (int)v[3];
            skilltype = (int)v[4];
            skillid = (int)v[5];
            hitresult = (bool)v[6];
            doubleresult = (bool)v[7];
            exskillmsg = 0;
            if (v.Length > 8)
            {
                exskillmsg = (int)v[8];
            }
           // Debug.LogError(v[6]+" "+hitresult);
        }else if (type == 1)
        {
            hitresult = (bool)v[1];
            fromid = (int)v[2];
            toid = (int)v[3];
            hpcost = (int)v[4];
        }else if (type == 2)
        {
            buffopttype = (int)v[1];
            toid = (int)v[2];
            buffid = (int)v[3];
            bufftime= Convert.ToInt64(v[4]);
            buffeff0 = (int)v[5];
            buffeff1 = (int)v[6];
            if(v.Length>7)
            timestamp = Convert.ToInt64(v[7]);
        }else if (type == 3)
        {

        }

    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for(int i = 0; i < v.Length; i++)
        {
            sb.Append(v[i] + ",");
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append("]");

        if (resultaction != null)
        {
            sb.Append(resultaction.ToString());
        }
        if (buffactionlist.Count > 0)
        {
            for(int i = 0; i < buffactionlist.Count; i++)
            {
                sb.Append(buffactionlist[i].ToString());
            }
        }

        return sb.ToString();
    }
}
public class FightBean 
{
    public int type; // 1 推图战斗
    public FightObjBean attacker;
    public FightObjBean defender;
    public int background=1; //场景ID之类的
    public int maxTime; //战斗最大时长单位毫秒
    public object[][] actions;
    public FightBeanResult result;

    public List<FightBeanAction> actionlist = new List<FightBeanAction>();
    public List<Reward> rewards;
    public bool win;
    public void format()
    {
        List<FightBeanAction> tempactionlist = new List<FightBeanAction>();
        for (int i = 0; i < actions.Length; i++)
        {
            FightBeanAction fba = new FightBeanAction(actions[i]);
            tempactionlist.Add(fba);
        }

        FightBeanAction lastaction = null;


        for (int i = 0; i < tempactionlist.Count; i++)
        {
            FightBeanAction tempaction = tempactionlist[i];
            if (lastaction == null)
            {
                lastaction = tempactionlist[i];
                actionlist.Add(lastaction);
            }
            if (lastaction.type==0)
            {
                if (tempaction.type == 1)
                {
                    lastaction.resultaction = tempaction;

                    if(tempaction.fromid== tempaction.toid)
                    {
                        tempaction.hitresult = true;
                    }

                    //if (lastaction.buffid == 19)
                    //{
                      //  lastaction.hitresult = true;
                      // tempaction.hitresult = true;
                    //}
                }else if (tempaction.type == 2)
                {
                    //if (lastaction.buffaction != null)
                    //{
                    //    Debug.LogError("sss");
                    //}
                //    Debug.LogError(actionlist.Count);
                    lastaction.buffactionlist.Add(tempaction);
                    lastaction.hitresult = true;
                }
                else if (tempaction.type == 3)
                {
                    if (lastaction.buffactionlist.Count==0 && lastaction.resultaction == null)
                    {
                        actionlist.Remove(lastaction);
                    }
                    lastaction = null;

                }
                
            }else if (lastaction.type == 2)
            {
                lastaction = null;
            }
        }
        Debug.LogError("youxiao=" + actionlist.Count);

        for (int i = 0; i < actionlist.Count; i++)
        {
           // Debug.LogError("tst=" + actionlist[i].timestamp+" "+ actionlist[i].type);
        }

    }
}
