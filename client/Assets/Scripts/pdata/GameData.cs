using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int uid;
    public UserInfo userinfo;
    public Hero mainhero;
    public Hero selectpartner;
    public List<Building> buildlist = new List<Building>();
    public List<Good> goodlist = new List<Good>();
    public List<Equip> equiplist = new List<Equip>();
    public List<Hero> partnerlist = new List<Hero>();
    public List<HeroStoneData> herostonelist = new List<HeroStoneData>();
    public List<Technical> technicallist = new List<Technical>();

    public List<FromTo> towerfromtolist = new List<FromTo>();
    public long towerConfigGetTime;
    public HistoryInfoResponse historyInfoData;


    //竞技场相关
    public ArenaPageInfo pageInfo;
    public List<ArenaRanker> rankers = new List<ArenaRanker>();

    internal Technical gettechnicall(int id)
    {
        for(int i = 0; i < technicallist.Count; i++)
        {
            if (technicallist[i].techID == id)
            {
                return technicallist[i];
            }
        }
        Technical temp = new Technical();
        temp.id = -1;
        temp.level = 0;
        temp.techID = id;
        technicallist.Add(temp);
        return temp;
    }

    public List<ArenaRecord> records = new List<ArenaRecord>();
    public ArenaSelf self;

    //邮件相关
    public ArenaPageInfo mailPageInfo;
    public List<MailInfo> mails = new List<MailInfo>();

    //天启入侵先关
    public int apocalypseID;//bossId
    public int remainNum;
    public int status;
    public List<Player> players = new List<Player>();//匹配到的队友
    public List<FightBean> tianqiReport = new List<FightBean>();//战报
    public List<Reward> tianqiRewards = new List<Reward>();//奖励
    public bool tianqiWin;//是否胜利
    public int tianqiReportIndex = 0;//当前第几个战报

    public int stageId = -1;
    public int stageindex = 0;
    public bool tongguan = false;

    public int guideStep = 0;//新手引导步骤
    public bool isWaitWorld = false;

    public Action act_userDataChanged;

    public GameData()
    {
        userinfo = new UserInfo();
        userinfo.name = "什么什么什么什么";
        userinfo.diamond = 88888888;
        userinfo.gold = 999999999;
        mainhero = new Hero();
        mainhero.heroID = Statics.HEROID;
        mainhero.name = "达瓦里氏";
        mainhero.level = 1;

        for (int i = 0; i < 7; i++)
        {
            Hero ptn = new Hero
            {
                heroID = Statics.PARTNERID + i
            };
            ptn.dbbean = data_heroDef.getdatabyheroid(ptn.heroID);
            ptn.level = 0;
            partnerlist.Add(ptn);
        }
        partnerlist.Sort((a, b) => { return a.dbbean.campaign_id.CompareTo(b.dbbean.campaign_id); });

        for (int i = 0; i < 5; i++)
        {
            Building bd = new Building
            {
                buildingID = Statics.BUILDID + i,
                level = i + 1
            };
            buildlist.Add(bd);
        }
        //for (int i = 0; i < 40; i += 2)
        //{
        //    Equip bd = new Equip
        //    {
        //        equipID = Statics.EQUIPID + i,
        //        level = 1,

        //    };
        //    if (i > 0 && i <= 6)
        //    {
        //        bd.pos = i;
        //    };
        //    equiplist.Add(bd);
        //}
        for (int i = 0; i < 10; i++)
        {
            Technical tc = new Technical
            {
                id = Statics.Technical + i,
                level = 1
            };
            technicallist.Add(tc);
        }

    }

    public Action UpdataUserPowerDataAct;

    public string curADServerId { get; internal set; }

    public Equip GetHeroEquipByPos(int pos)
    {
        Equip eq = null;
        for (int i = 0; i < equiplist.Count; i++)
        {
            if (equiplist[i].pos == pos)
            {
                eq = equiplist[i];
            }
        }
        return eq;
    }

    public void CheckIsSomeOpen()
    {
      //  Debug.LogError("stageindex=" + stageindex);
        var stage = stageindex;
        var herodatas = data_heroDef.datas;
        for (int i = 0; i < herodatas.Count; i++)
        {
            if (herodatas[i].type == 2 && herodatas[i].campaign_id == stage)
            {
                HttpManager.instance.GetHeroList(null);
                break;
            }
        }
        if (stage == 2)
        {
            HttpManager.instance.getHeroStoneInfo(null);
        }
        var buildingdatas = data_buildingDef.datas;
        for (int i = 0; i < buildingdatas.Count; i++)
        {
            if (buildingdatas[i].condition_val == stage)
            {
              //  Debug.LogError(buildingdatas[i].id);
                HttpManager.instance.GetBuildingList(null);
                //if (buildingdatas[i].id == 1006) //研究院开启
                //{
                //    HttpManager.instance.GetTechList(null);
                //}
                //if (buildingdatas[i].id == 1012) //探险家营地开启
                //{
                //    HttpManager.instance.GetEquipList(1, 100, null);
                //}
                HttpManager.instance.sendUserInfo(null);
                break;
            }
        }

        int techBuildingOpenLevel = data_buildingDef.getdatabybuildid(1006).condition_val;
        if (techBuildingOpenLevel == GameGlobal.gamedata.stageindex)//研究院开启
        {
            HttpManager.instance.GetTechList(null);
        }

        int campBuildingOpenLevel = data_buildingDef.getdatabybuildid(1012).condition_val;
        if (campBuildingOpenLevel == GameGlobal.gamedata.stageindex) //探险家营地开启
        {
            HttpManager.instance.GetEquipList(1, 100, null);
        }
    }

    public long getCurrendy(int itemType)
    {
        long vl = 0;
        if (itemType == Statics.TYPE_diamond)
        {
            vl = userinfo.diamond;
        }
        else if (itemType == Statics.TYPE_gold)
        {
            vl = userinfo.gold;
        }
        else if (itemType == Statics.TYPE_soulCrystal)
        {
            vl = userinfo.soulCrystal;
        }
        else
        {
            vl = getOtherResource(itemType);
        }
        return vl;
    }
    public void addCurrendy(int mainType, long val)
    {

        if (mainType == Statics.TYPE_gold)
        {
            addGold(val);
        }
        else if (mainType == Statics.TYPE_diamond)
        {
            addCrystals((int)val);
        }
        else if (mainType == Statics.TYPE_soulCrystal)
        {
            addsoulCrystal((int)val);
        }
        else
        {
            addOtherResource(mainType, (int)val);
        }

    }
    public void addsoulCrystal(int value)
    {
        userinfo.soulCrystal += value;
    }

    public void addCrystals(int value)
    {
        userinfo.diamond += value;
    }
    public void addGold(long value)
    {
        userinfo.gold += value;
    }

    public void SetActAutoAddGlod(bool isAuto)
    {
        if (isAuto)
        {
            GameGlobal.instance.SecondTimerCall += autoAddGlod;
        }
        else
        {
            GameGlobal.instance.SecondTimerCall -= autoAddGlod;
        }
    }

    public void AdjustBaseGold()
    {
        if (userinfo != null)
        {
            int deltaGold = 0;
            var deltaTime = 0;
            if (deltaGold > 0)
            {
                addGold(deltaGold);
            }
        }
    }

    void autoAddGlod()
    {
        if (userinfo != null)
        {
            var deltaGold = userinfo.goldIncrement;
            //Debug.LogError("==================================autoAddGlod========" + deltaGold);
            addGold(deltaGold);
        }
    }
    public void subCurrendy(int mainType, long val)
    {

        if (mainType == Statics.TYPE_gold)
        {
            subGold(val);
        }
        else if (mainType == Statics.TYPE_diamond)
        {
            subCrystals((int)val);
        }
        else if (mainType == Statics.TYPE_soulCrystal)
        {
            subsoulCrystal((int)val);
        }
        else
        {
            subOtherResource(mainType, (int)val);
        }
    }

    public void subOtherResource(int resourceType, int val)
    {
        switch (resourceType)
        {
            case Statics.TYPE_treasureAnima:
                userinfo.treasureAnima -= val;
                break;
            case Statics.TYPE_element:
                userinfo.element -= val;
                break;
            case Statics.TYPE_book:
                userinfo.book -= val;
                break;
            case Statics.TYPE_superbiaStone:
                userinfo.superbiaStone -= val;
                break;
            case Statics.TYPE_invidiaStone:
                userinfo.invidiaStone -= val;
                break;
            case Statics.TYPE_acediaStone:
                userinfo.acediaStone -= val;
                break;
            case Statics.TYPE_gulaStone:
                userinfo.gulaStone -= val;
                break;
            case Statics.TYPE_avaritiaStone:
                userinfo.avaritiaStone -= val;
                break;
            case Statics.TYPE_luxuriaStone:
                userinfo.luxuriaStone -= val;
                break;
            case Statics.TYPE_iraStone:
                userinfo.iraStone -= val;
                break;
            case Statics.TYPE_benYuan:
                userinfo.benYuan -= val;
                break;
            case Statics.TYPE_qianNeng:
                userinfo.qianNeng -= val;
                break;
            default:
                break;
        }
    }

    public void addOtherResource(int resourceType, int val)
    {
        switch (resourceType)
        {
            case Statics.TYPE_treasureAnima:
                userinfo.treasureAnima += val;
                break;
            case Statics.TYPE_element:
                userinfo.element += val;
                break;
            case Statics.TYPE_book:
                userinfo.book += val;
                break;
            case Statics.TYPE_superbiaStone:
                userinfo.superbiaStone += val;
                break;
            case Statics.TYPE_invidiaStone:
                userinfo.invidiaStone += val;
                break;
            case Statics.TYPE_acediaStone:
                userinfo.acediaStone += val;
                break;
            case Statics.TYPE_gulaStone:
                userinfo.gulaStone += val;
                break;
            case Statics.TYPE_avaritiaStone:
                userinfo.avaritiaStone += val;
                break;
            case Statics.TYPE_luxuriaStone:
                userinfo.luxuriaStone += val;
                break;
            case Statics.TYPE_iraStone:
                userinfo.iraStone += val;
                break;
            case Statics.TYPE_benYuan:
                userinfo.benYuan += val;
                break;
            case Statics.TYPE_qianNeng:
                userinfo.qianNeng += val;
                break;
            default:
                break;
        }
    }


    public int getOtherResource(int itemType)
    {
        int vl = 0;
        switch (itemType)
        {
            case Statics.TYPE_treasureAnima:
                vl = userinfo.treasureAnima;
                break;
            case Statics.TYPE_element:
                vl = userinfo.element;
                break;
            case Statics.TYPE_book:
                vl = userinfo.book;
                break;
            case Statics.TYPE_superbiaStone:
                vl = userinfo.superbiaStone;
                break;
            case Statics.TYPE_invidiaStone:
                vl = userinfo.invidiaStone;
                break;
            case Statics.TYPE_acediaStone:
                vl = userinfo.acediaStone;
                break;
            case Statics.TYPE_gulaStone:
                vl = userinfo.gulaStone;
                break;
            case Statics.TYPE_avaritiaStone:
                vl = userinfo.avaritiaStone;
                break;
            case Statics.TYPE_luxuriaStone:
                vl = userinfo.luxuriaStone;
                break;
            case Statics.TYPE_iraStone:
                vl = userinfo.iraStone;
                break;
            case Statics.TYPE_benYuan:
                vl = userinfo.benYuan;
                break;
            case Statics.TYPE_qianNeng:
                vl = userinfo.qianNeng;
                break;
            default:
                break;
        }
        return vl;
    }
    public void subsoulCrystal(int value)
    {
        userinfo.soulCrystal -= value;
    }

    public void subCrystals(int value)
    {
        userinfo.diamond -= value;
    }
    public void subGold(long value)
    {
        userinfo.gold -= value;
    }
    public Equip getEquip(int eid)
    {
        for (int i = 0; i < equiplist.Count; i++)
        {
            if (equiplist[i].equipID == eid)
            {
                return equiplist[i];
            }
        }
        return null;
    }

    public string GetNamebyCrystalId(int crystalId)
    {
        string name = "未知";
        switch (crystalId)
        {
            case 1:
                name = "炽焰";
                break;
            case 2:
                name = "水系";
                break;
            case 3:
                name = "飓风";
                break;
            case 4:
                name = "大地";
                break;
            case 5:
                name = "光明";
                break;
            case 6:
                name = "黑暗";
                break;
            case 7:
                name = "时空";
                break;
            default:
                break;
        }
        return name;
    }

    public string GetEffectDesByID(int effId)
    {
        string des = "未知";
        if (effId == 20001)
        {
            des = "重击";
        }
        else if (effId == 20002)
        {
            des = "韧性";
        }
        else if (effId == 20003)
        {
            des = "破甲";
        }
        else if (effId == 20004)
        {
            des = "铁壁";
        }
        else if (effId == 20005)
        {
            des = "命中";
        }
        else if (effId == 20006)
        {
            des = "闪避";
        }
        else if (effId == 20007)
        {
            des = "减伤";
        }
        return des;
    }

    public int GetJuexingLevel()
    {
        int level = 0;
        for (int i = 0; i < herostonelist.Count; i++)
        {
            level += herostonelist[i].level;
        }
        return level;
    }

    public data_alchemyBean GetLiandanDataByJuexinglevel(int level)
    {
        data_alchemyBean data = null;
        for (int i = 0; i < data_alchemyDef.datas.Count; i++)
        {
            if (data_alchemyDef.datas[i].attr_min <= level && data_alchemyDef.datas[i].attr_max >= level)
            {
                data = data_alchemyDef.datas[i];
            }
        }
        return data;
    }

    public Timecount GetTimeHMS(int seconds)
    {
        int h = seconds / 3600;
        int m = (seconds - h * 3600) / 60;
        int s = seconds - h * 3600 - m * 60;
        Timecount timecount = new Timecount
        {
            h = h,
            m = m,
            s = s,
        };
        return timecount;
    }

    public data_stageBean GetCurStagebean(int curcampID)
    {
        data_stageBean bean = null;
        for (int i = 0; i < data_stageDef.datas.Count; i++)
        {
            if (data_stageDef.datas[i].start_campaign_id <= curcampID && data_stageDef.datas[i].end_campaign_id >= curcampID)
            {
                bean = data_stageDef.datas[i];
            }
        }
        return bean;
    }
    public string GetNumStr(long num)
    {
        string result;
        if (num >= 100000)
        {
            var v = Math.Round(num / 10000f, 1);
            result = v + "万";
        }
        else
        {
            result = num.ToString();
        }
        return result;
    }
    public string GetNumStr22(long num)
    {
        string result;
        if (num >= 1000000000)
        {
            var v = Math.Round(num / 1000000000f, 1);
            result = v + "B";
        }
        else if (num >= 1000000)
        {
            var v = Math.Round(num / 1000000f, 1);
            result = v + "M";
        }
        else if (num >= 10000)
        {
            var v = Math.Round(num / 10000f, 1);
            result = v + "K";
        }
        else
        {
            result = num.ToString();
        }
        // return result;
        return num+"";
    }
}

public struct Timecount
{
    public int h;
    public int m;
    public int s;
}
