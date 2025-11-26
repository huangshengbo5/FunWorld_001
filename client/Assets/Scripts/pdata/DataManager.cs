using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public static int GetHeroZhanli(Hero hero)
    {
        int index = hero.level - 1 >= 0 ? hero.level - 1 : 0;
        data_hero_upgradeBean levelbean = data_hero_upgradeDef.dicdatas[hero.heroID][index];
        return levelbean.fighting_capacity;
    }

    public static int GetUsedEquipZhanli()
    {
        int vl = 0;
        for (int i = 0; i < GameGlobal.gamedata.equiplist.Count; i++)
        {
            if (GameGlobal.gamedata.equiplist[i].pos > 0)
            {
                vl += GameGlobal.gamedata.equiplist[i].fightingCapacity;
            }
        }
        return vl;
    }

    public static int GetUserPartnersZhanli()
    {
        int vl = 0;
        for (int i = 0; i < GameGlobal.gamedata.partnerlist.Count; i++)
        {
            if (GameGlobal.gamedata.partnerlist[i].level > 0)
            {
                vl += GetHeroZhanli(GameGlobal.gamedata.partnerlist[i]);
            }
        }
        return vl;
    }
}
