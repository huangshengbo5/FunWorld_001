using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuideConditionType 
{
    None = 0,
    FirstEnterWorld = 1,//第一次进入游戏
    LevelOver = 2,//关卡结束
    GuideOver = 3,//引导结束
    DialogueId = 4,//对话结束
}

public enum GuideActiveType
{
    None = 0,
    OpenWindow = 1,//打开界面
    UpBattle  = 2,//上阵
    LevelUp   = 3,//升级
    UnlockBuild = 4,//解锁建筑
    PlayDialogue = 5,//播放剧情
}

public enum GuideAcitveWindow
{
    None = 0,//
    Home = 1,//家园界面
    Level = 2,//关卡界面
    LevelAttack =3,//关卡挑战界面
    Mofang = 4,//磨坊建筑
    Partner = 5,//伙伴界面
    Hero    = 6,//角色界面
    JiuGuan = 7,//酒馆界面
    Yingdi = 8,//营地界面
    Baowu = 9,//宝物界面
    BaowuEquip = 10,//宝物装备界面
}
