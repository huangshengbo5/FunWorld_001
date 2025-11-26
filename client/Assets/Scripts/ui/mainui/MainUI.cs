using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI instance;

    public GameObject[] btmbuts = new GameObject[5];
    public GameObject[] buttips = new GameObject[5];

    public GameObject mailBut;
    public GameObject settingbtn;
    public GameObject hideInWorldRoot;

    public GameObject hometop, worldtop;

    public GameObject worldtitl1node;
    public UILabel worldtt0label, worldtt1label, worldtt2label;

    public GameObject headgen;
    public UILabel namelabel, zllabel, levellab;
    public UILabel zuanlabel, jinbilabel;
    public GameObject zuanbut, jinbibut;


    public GameObject tansuobut, shopbut;
    public UILabel tansuotimelabel, shoplabel;

    public UITexture headIcon;

    public UICamera mapcamera;
    void Awake()
    {
        instance = this;
        mapcamera = transform.parent.GetComponentInChildren<UICamera>();
        UIViewGroup.setmapcamera(mapcamera);
        UIEventListener.Get(headIcon.gameObject).onClick = OnHeadClick;

        UIEventListener.Get(zuanbut.gameObject).onClick = onclickqian;
        UIEventListener.Get(jinbibut.gameObject).onClick = onclickqian;
    }

    private void onclickqian(GameObject go)
    {
        UIManager.showShop();
    }

    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(worldtitl1node).onClick = onClickTest;

        for (int i = 0; i < btmbuts.Length; i++)
        {
            UIEventListener.Get(btmbuts[i]).onClick = onClickBtm;
            SoundEventer.add_But_ClickSound(btmbuts[i]);
        }
        if (GameGlobal.fromfightflag && !GameGlobal.fromArenaFight && !GameGlobal.fromTianqiFight && !GameGlobal.fromMonsterFight)
        {
            if (data_campaignDef.dataDic.ContainsKey(GameGlobal.gamedata.stageId))
            {
                var stageData = data_campaignDef.dataDic[GameGlobal.gamedata.stageId];
                if (stageData.afterbattledialogue_id > 0)
                {
                    UIManager.showDialogue(stageData.afterbattledialogue_id, null);
                }
                else
                {
                    //
                    GuideManager.Instance.CheckGuideActive(2, GameGlobal.gamedata.stageId);
                }
            }
            setworld();
        }
        else
        {
            if (GameGlobal.fromTianqiFight)
            {
                HttpManager.instance.TianqiShow((code1) =>
                {
                    if (code1 == Callback.SUCCESS)
                    {
                        UIManager.showTianqiActivity();
                    }

                });
            }
            if (GameGlobal.fromArenaFight)
            {
                HttpManager.instance.ArenaList((code1) =>
                {
                    if (code1 == Callback.SUCCESS)
                    {
                        UIManager.showPkInfo();
                    }

                });
            }
            if (GameGlobal.fromMonsterFight)
            {
                setworld();
            }
            else
            {
                sethome();
            }
        }
        GameGlobal.fromArenaFight = false;
        GameGlobal.fromTianqiFight = false;
        GameGlobal.fromMonsterFight = false;


        SoundEventer.add_But_ClickSound(mailBut);
        UIEventListener.Get(settingbtn).onClick = (go) =>
        {
            UIManager.showsettingUI();
        };
        SoundEventer.add_But_ClickSound(settingbtn);

        //在第一次新手引导需要播放剧情
        if (GameGlobal.gamedata.guideStep < 8)
        {
            UIManager.showDialogue(10005, _OnStepCallback6);
        }
        else if (GameGlobal.gamedata.guideStep >= 9 && GameGlobal.gamedata.guideStep < 16 && GameGlobal.gamedata.isWaitWorld)
        {
            int step = GameGlobal.gamedata.guideStep + 1;
            //已经过了一个引导，并且没全过，那么播放第一个
            if (data_guidesDef.guideList.ContainsKey(step))
            {
                var list = data_guidesDef.guideList[step];
                if (list != null && list.Count > 0)
                {
                    data_guidesBean bean = list[0];
                    if (bean.activetype == 1 && bean.activeparam == 2)
                    {
                        GameGlobal.gamedata.isWaitWorld = true;
                        setworld();
                    }
                    else
                    {
                        UIManager.showGuideStep(bean.id);
                    }
                }
            }
        }
        GameGlobal.gamedata.act_userDataChanged += setBaseInfo;
        GameGlobal.gamedata.UpdataUserPowerDataAct += setUserPower;
        GameGlobal.instance.SecondTimerCall += setGlodText;
        setBaseInfo();
        RefreshHelp();
    }

    void setBaseInfo()
    {
        string userName = GameGlobal.gamedata.userinfo.name;
        if (string.IsNullOrEmpty(userName))
        {
            namelabel.text = "游客" + HttpManager.playerid;
        }
        else
        {
            namelabel.text = userName;
        }
        setUserPower();
        setHeadIcon();
        setGlodText();
        refreshcount();
    }

    private void _OnStepCallback6()
    {
        UIManager.showDialogue(10008, _OnStepCallback7);
    }

    private void _OnStepCallback7()
    {
        HttpManager.instance.sendGuideStep(8, (code1) =>
        {
            if (code1 == Callback.SUCCESS)
            {
                Debug.Log("成功同步引导步骤");

                //弹出新手引导
                UIManager.showGuideStep(1);
            }
        });
    }

    private void OnDestroy()
    {
        GameGlobal.gamedata.UpdataUserPowerDataAct -= setUserPower;
        GameGlobal.instance.SecondTimerCall -= setGlodText;
        GameGlobal.gamedata.act_userDataChanged -= setBaseInfo;
    }

    TexPaintNode temppaintnode;
    void setHeadIcon()
    {
        if (!temppaintnode)
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(headgen, "huobantou");
            temppaintnode.setdepth(22);
            temppaintnode.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        int headid = GameGlobal.gamedata.userinfo.avatar;
        string headIconName = data_avatarDef.dicdatas[headid][0].name;
        temppaintnode.playAction(headIconName);
    }

    void setUserPower()
    {
        int vl = 0;
        vl += DataManager.GetHeroZhanli(GameGlobal.gamedata.mainhero);
        vl += DataManager.GetUserPartnersZhanli();
        vl += DataManager.GetUsedEquipZhanli();
        zllabel.text = vl.ToString();
        levellab.text = "等级:" + GameGlobal.gamedata.mainhero.level;
    }

    public UIInput input_GMaddGold;
    public void OnGMAddGoldInput()
    {
        long vl = long.Parse(input_GMaddGold.value);
        if (vl > 400000000)
        {
            vl = 400000000;
        }
        HttpManager.instance.GMIncreatGold((int)vl);
    }


    public void setworldmsg(data_campaignBean selectdbbean)
    {

        worldtt1label.text = selectdbbean.name;
        var stagebean = GameGlobal.gamedata.GetCurStagebean(selectdbbean.id);
        if (stagebean != null)
        {

            worldtt0label.text = stagebean.name;
        }
        else
        {
            worldtt0label.text = "无章节数据！campID=" + selectdbbean.id;
        }
        if (GameGlobal.gamedata.tongguan)
        {
            worldtt0label.text = "恭喜领主";
            worldtt2label.text = "已通过全部主线关卡";
            worldtt1label.gameObject.SetActive(false);
        }
    }



    //private void onClickMailBut(GameObject go)
    //{
    //   
    //}

    private void onClickBtm(GameObject go)
    {
        for (int i = 0; i < btmbuts.Length; i++)
        {
            if (go == btmbuts[i])
            {
                if (i == 0)
                {
                    UIManager.showHeroInfo();
                }
                else if (i == 1)
                {
                    UIManager.showPartnerInfo();
                }
                else if (i == 2)
                {
                    HttpManager.instance.GetHistoryInfo((code1) =>
                    {
                        if (code1 == Callback.SUCCESS)
                        {
                            UIManager.showHistoryUI();
                        }

                    });

                }
                else if (i == 3)
                {
                    UIManager.showEquipInfo();
                }
                else if (i == 4)
                {
                    setworld();
                }
                else if (i == 5)
                {
                    sethome();
                }
                break;
            }
        }
    }

    private void setworld()
    {
        hideInWorldRoot.SetActive(false);
        btmbuts[4].SetActive(false);
        btmbuts[5].SetActive(true);
        worldtop.SetActive(true);
        MapManager.instance.setworld();
    }

    private void sethome()
    {
        hideInWorldRoot.SetActive(true);
        btmbuts[4].SetActive(true);
        btmbuts[5].SetActive(false);
        worldtop.SetActive(false);
        MapManager.instance.sethome();
    }

    private void onClickTest(GameObject go)
    {
        // Debug.Log("hello");
        //  UIManager.showToast("helloworld");
        MapManager.instance.world.movePMapTo(WorldLayer.instance.nowpoint.index);
    }
    private void refreshcount()
    {
        for (int i = 0; i < tipcountcount.Length; i++)
        {
            tipcountcount[i] = getnewcount(i);
        }
        setbtnRedTip();
    }

    private int getnewcount(int listType)
    {
        int count = 0;

        return count;
    }

    public static int[] tipcountcount = { 0, 0, 0, 0, 0, 0 };

    void setGlodText()
    {
        zuanlabel.text = GameGlobal.gamedata.GetNumStr(GameGlobal.gamedata.userinfo.diamond);
        jinbilabel.text = GameGlobal.gamedata.GetNumStr( GameGlobal.gamedata.userinfo.gold);
    }

    void setbtnRedTip()
    {
        for (int i = 0; i < buttips.Length; i++)
        {
            UTools.setActive(buttips[i], tipcountcount[i] == 1);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //播放一个新手引导
            UIManager.showGuideStep(50);
        }
    }

    public void RefreshHelp()
    {
        bool isActive = false;
        for (int i = 0; i < help_dialogueDef.datas.Count; i++)
        {

            if (GameGlobal.gamedata.stageindex >= help_dialogueDef.datas[i].campaign_id_lower
               && GameGlobal.gamedata.stageindex <= help_dialogueDef.datas[i].campaign_id_upper)
            {
                isActive = true;
            }
        }
        if (isActive)
        {
            headIcon.gameObject.SetActive(true);
        }
        else
        {
            headIcon.gameObject.SetActive(false);
        }
    }

    public void OnHeadClick(GameObject go)
    {
        int dialogueId = 0;
        for (int i = 0; i < help_dialogueDef.datas.Count; i++)
        {
            if (GameGlobal.gamedata.stageindex >= help_dialogueDef.datas[i].campaign_id_lower
               && GameGlobal.gamedata.stageindex <= help_dialogueDef.datas[i].campaign_id_upper)
            {
                var dia_weight = help_dialogueDef.datas[i].list;
                List<int> weight = new List<int>();
                for (int n = 0; n < dia_weight.Count; n++)
                {
                    weight.Add(dia_weight[n].weight);
                }
                int index = Random(weight);
                if (index > -1)
                {
                    dialogueId = dia_weight[index].dialogueId;
                }
                break;
            }
        }
        UIManager.showDialogue(dialogueId, null);
    }

    public static int Random(List<int> weightList)
    {
        if (weightList == null || weightList.Count == 0)
        {
            return -1;
        }

        int result = -1;
        int total = 0;
        for (int i = 0; i < weightList.Count; ++i)
        {
            if (weightList[i] < 0)
            {
                weightList[i] = 0;
            }
            total += weightList[i];
        }
        int random = UnityEngine.Random.Range(0, total);
        total = 0;
        for (int i = 0; i < weightList.Count; i++)
        {
            total += weightList[i];
            if (random < total)
            {
                result = i;
                break;
            }
        }
        return result;
    }
}
