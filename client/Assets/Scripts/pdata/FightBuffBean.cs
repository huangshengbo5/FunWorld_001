using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightBuffBean 
{
    //中毒
    public const int BuffDecrHP = 1;
    //眩晕
    public const int BuffFreeze = 2;
    //降低攻击力
    public const int BuffDecrAttack = 3;
    //降低攻击频率
    public const int BuffDecrAttackFreq = 4;
    //降低防御力
    public const int BuffDecrDefend = 5;

    //回血
    public const int BuffIncrHP = 6;
    //增加攻击力
    public const int BuffIncrAttack = 7;
    //增加攻击频率
    public const int BuffIncrAttackFreq = 8;
    //减少自身伤害
    public const int BuffHpProtected = 9;
    //免疫指定技能
    public const int BuffImmune10007 = 10;

    public const int BuffImmune10008 = 11;

    public const int BuffImmune10009 = 12;
    public int buffid;
    public long time;
}
