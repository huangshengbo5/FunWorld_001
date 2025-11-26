using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoEx
{
    public int arenaRemainNum;//挑战次数
    public bool arenaissign;//是否报名
    public Campaign campaign;
    public OnlineReward onlineReward;
    public Alchemy alchemy;
    public ADs ads;
    public int businessMan;//1 十二点时间完成，2 下午6点时间完成 ，3 表示 1 、2 都完成
}

public class ADs
{
    public int num;
    public int[] ids;
}

public class Alchemy
{
    public int nextReceiveTime;
    public int payNum;
}