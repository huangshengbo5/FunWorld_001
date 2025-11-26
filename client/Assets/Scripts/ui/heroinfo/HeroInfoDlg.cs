using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInfoDlg : DialogMonoBehaviour
{
    public static HeroInfoDlg instance;
    public data_heroBean dbbean;
    public GameObject heroImgGo;
    public GameObject closebut;
    public UILabel nameandlvlabel, zhanlilabel;
    public GameObject rennode, infonode, stonenode;
    public HeroInfoGoodItem[] gooditems = new HeroInfoGoodItem[6];
    public UISprite tianfuicongen;
    public UILabel tianfunameandlvlabel, tianfudesclabel;
    public HeroInfoSkillItem[] skillitems = new HeroInfoSkillItem[3];
    public GameObject infoshengbut, infomaxbut;
    public UILabel infoshengbutlab, infomaxbutlab;
    public GameObject infoshengcostlabel;
    public RewardItem infocostitem;

    public HeroInfoStoneItem[] stoneitems = new HeroInfoStoneItem[7];
    public UILabel lastnameandlvlabel, lastdesclabel, nextnameandlvlabel, nextdesclabel;
    public GameObject stoneshengbut, stonemaxbut;
    public RewardItemBian stonecostitem;

    public APaintNodeSpine renpaintnode;
    public TexPaintNode texpaintnode;

    public GameObject go_btnEnergeTrans;
    public GameObject go_back2;
    public GameObject go_EnergeTransnode;

    public TablesNode tableNode;
    UISprite infoshengbutSp;
    void Awake()
    {
        instance = this;
        dbbean = data_heroDef.getdatabyheroid(GameGlobal.gamedata.mainhero.heroID);
        GameGlobal.gamedata.mainhero.name = dbbean.name;
        setgooditems();
        initSkillitems();
        setShowAnim(true);
        setClickOutZoneClose(false);
        infoshengbutSp = infoshengbut.GetComponent<UISprite>();
        UIEventListener.Get(closebut).onClick = closeDialog;

        UIEventListener.Get(infoshengbut).onClick = onclicklevelup;
        UIEventListener.Get(heroImgGo).onClick = onheroimgeclick;

        SoundEventer.add_But_ClickSound(closebut);
        SoundEventer.add_But_ClickSound(infoshengbut);
        SoundEventer.add_But_ClickSound(heroImgGo);


        infonode.SetActive(true);
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
            texframeobjs.name = "tiao";
            APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
            temppaintnode.create1(rennode, dbbean.resource, dbbean.resource);
            temppaintnode.transform.localPosition = Vector3.zero;
            temppaintnode.transform.localScale = Vector3.one * 0.5f;
            temppaintnode.setdepth(11);
            temppaintnode.playAction2Auto("idle", true);
            temppaintnode.setAutoAlpha(true);

            renpaintnode = temppaintnode;
        }

        GameGlobal.gamedata.UpdataUserPowerDataAct += setgooditems;

        tableNode.InitTables(new string[] { "属性", "觉醒", "魔能转换" }, onTableClick, onTableUnClick, checkTabelCanOpen);
    }

    bool checkTabelCanOpen(int tabelIndex)
    {
        bool isOpen = false;
        var lastcamp = GameGlobal.gamedata.stageindex;
        if (tabelIndex == 1 && lastcamp >= 15)
        {
            isOpen = true;
        }
        else if (tabelIndex == 2 && lastcamp >= 70)
        {
            isOpen = true;
        }
        else if (tabelIndex == 0)
        {
            isOpen = true;
        }
        if (!isOpen)
        {
            if (tabelIndex == 1)
            {
                UIManager.showToast("通过 如影随形 关卡后解锁!");
            }
            else if (tabelIndex == 2)
            {
                UIManager.showToast("通过 见识恐怖吧 关卡后解锁!");
            }
        }
        return isOpen;
    }

    public GameObject[] tableNodList;
    void onTableClick(int tableIndex)
    {
        tableNodList[tableIndex].SetActive(true);
    }
    void onTableUnClick(int tableIndex)
    {
        tableNodList[tableIndex].SetActive(false);
    }

    void setgooditems()
    {
        for (int i = 0; i < 6; i++)
        {
            int pos = i + 1;
            var gooditem = gooditems[i];
            var eq = GameGlobal.gamedata.GetHeroEquipByPos(pos);
            gooditem.SetData(eq, pos);
        }
        setZhanli();
    }

    private void initSkillitems()
    {
        SkillItemData itemdata1 = new SkillItemData
        {
            skillIndex = 1,
            skillid = dbbean.skill1_id,
            skilllevel = 1,
            bean = data_skillDef.dicdatas[dbbean.skill1_id],
        };
        skillitems[0].initItem(itemdata1);
        SkillItemData itemdata2 = new SkillItemData
        {
            skillIndex = 2,
            skillid = dbbean.skill2_id,
            skilllevel = 1,
            bean = data_skillDef.dicdatas[dbbean.skill2_id],
        };
        skillitems[1].initItem(itemdata2);
        SkillItemData itemdata3 = new SkillItemData
        {
            skillIndex = 3,
            skillid = dbbean.skill3_id,
            skilllevel = 1,
            bean = data_skillDef.dicdatas[dbbean.skill3_id],
        };
        skillitems[2].initItem(itemdata3);
    }

    void refreshSkillItemdatalevel()
    {
        var herolevelbean = data_hero_upgradeDef.dicdatas[GameGlobal.gamedata.mainhero.heroID][GameGlobal.gamedata.mainhero.level];
        skillitems[0].skilldata.skilllevel = herolevelbean.skill1_level;
        skillitems[1].skilldata.skilllevel = herolevelbean.skill2_level;
        skillitems[2].skilldata.skilllevel = herolevelbean.skill3_level;
        skillitems[0].skilldata.nextneedlevel = data_hero_upgradeDef.getNextSkillNeedHeroLevel(GameGlobal.gamedata.mainhero.heroID, 1, herolevelbean.skill1_level);
        skillitems[1].skilldata.nextneedlevel = data_hero_upgradeDef.getNextSkillNeedHeroLevel(GameGlobal.gamedata.mainhero.heroID, 2, herolevelbean.skill1_level);
        skillitems[2].skilldata.nextneedlevel = data_hero_upgradeDef.getNextSkillNeedHeroLevel(GameGlobal.gamedata.mainhero.heroID, 3, herolevelbean.skill1_level);
    }

    void setTalentInfo()
    {
        var bean = data_skillDef.dicdatas[99999];
        tianfuicongen.spriteName = bean.skill_picture;
        tianfunameandlvlabel.text = bean.skill_name + " " + "";
        var leveldata = data_skill_levelDef.dicdatas[99999];
        tianfudesclabel.text = leveldata[0].describe;
    }

    private void onheroimgeclick(GameObject go)
    {
        var rand = new System.Random();
        var randget = rand.Next(0, 3);
        string animname = "attack";
        if (randget == 1)
        {
            animname = "skill";
        }
        else if (randget == 2)
        {
            animname = "win";
        }
        renpaintnode.playAction2Auto(animname, false, () => { renpaintnode.playAction2Auto("idle", true); });
        string sname = dbbean.id + "_talk0" + rand.Next(1, 4);
        SoundPlayer.getInstance().PlayW(sname, false);
    }

    private void onclicktostone(GameObject go)
    {
        if (GameGlobal.gamedata.stageindex < 2)
        {
            UIManager.showToast("尚未解锁！");
            return;
        }
        stonenode.SetActive(true);
        infonode.SetActive(false);
    }

    private void onclicklevelup(GameObject go)
    {
        if (GameGlobal.gamedata.mainhero.level >= Statics.PARTNERLEVELMAX)
        {
            UIManager.showToast("等级已满");
            return;
        }

        if (iscanup)
        {
            HttpManager.instance.sendHeroLevelUp(GameGlobal.gamedata.mainhero, nextlevelbean.getlvcost(), (int code) =>
            {
                refreshSkillItemdatalevel();
                refreshui();
                xgpaintnode.gameObject.SetActive(true);
                xgpaintnode.playActionAuto(0, true, () =>
                {
                    xgpaintnode.gameObject.SetActive(false);
                });
            });
        }
    }

    data_hero_upgradeBean nextlevelbean;
    bool iscanup = true;
    private void refreshui()
    {
        if (GameGlobal.gamedata.mainhero.level >= Statics.PARTNERLEVELMAX)
        {
            infoshengbut.SetActive(false);
            infomaxbut.SetActive(true);
        }
        else
        {
            infoshengbut.SetActive(true);
            infomaxbut.SetActive(false);
            nextlevelbean = data_hero_upgradeDef.dicdatas[GameGlobal.gamedata.mainhero.heroID][GameGlobal.gamedata.mainhero.level];
            var costitem = nextlevelbean.getlvcost();
            infocostitem.setReward(costitem);
            long havenum = GameGlobal.gamedata.getCurrendy(costitem.mainType);
            iscanup = havenum >= costitem.val;

            if (iscanup)
            {
                infoshengbutSp.spriteName = "btm_shengji";
            }
            else
            {
                infoshengbutSp.spriteName = "btn_hui";
            }
        }


        nameandlvlabel.text = GameGlobal.gamedata.mainhero.name + "  等级" + GameGlobal.gamedata.mainhero.level;

        setZhanli();
    }

    void setZhanli()
    {
        int zhanli = DataManager.GetHeroZhanli(GameGlobal.gamedata.mainhero);
        zhanli += DataManager.GetUsedEquipZhanli();
        zhanlilabel.text = zhanli.ToString();
    }

    APaintNodeSpine xgpaintnode;
    void Start()
    {
        GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
        xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
        xgpaintnode.create1(gameObject, "shengJi-GuangShu", "shengJi-GuangShu", 0.5f);
        //xgpaintnode.playActionAuto(actionid, true);
        // xgpaintnode.playAction2(actionname, true);
        xgpaintnode.setdepth(20);
        xgpaintnode.transform.localScale = Vector3.one;
        xgpaintnode.transform.localPosition = new Vector3(0, 0, 0);
        xgpaintnode.gameObject.SetActive(false);

        setTalentInfo();
        refreshSkillItemdatalevel();
        refreshui();

        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow, (int)GuideAcitveWindow.Hero);
    }

    void OnDestroy()
    {
        GameGlobal.gamedata.UpdataUserPowerDataAct -= setgooditems;
        Resources.UnloadUnusedAssets();
    }
}
