using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerInfoDlg : DialogMonoBehaviour
{
    public static PartnerInfoDlg instance;
    public GameObject closebut;
    public UILabel nameandlvlabel, zhanlilabel;
    public GameObject rennode;
    public HeroInfoSkillItem[] skillitems = new HeroInfoSkillItem[3];
    public GameObject shangbut, xiabut;
    public GameObject leftbut, rightbut;
    public GameObject shengbut, maxbut;
    UISprite shengbutsp;
    BoxCollider shengbutcox;
    public RewardItem costitem;
    public evoleveNode evoleveNode;
    public GameObject go_CommNode;

    public GridAdapter grid;
    public GameObject itemObject;
    public GameObject selectkuang;

    public APaintNodeSpine renpaintnode;
    public TexPaintNode texpaintnode;

    public TablesNode tableNode;

    public GameObject goOptionroot;
    public UILabel labLeveNotice;
    void Awake()
    {
        instance = this;
        setShowAnim(true);
        setClickOutZoneClose(false);
        UIEventListener.Get(closebut).onClick = closeDialog;
        UIEventListener.Get(rennode).onClick = onheroimgeclick;
        UIEventListener.Get(leftbut).onClick = onclickleft;
        UIEventListener.Get(rightbut).onClick = onclickright;
        UIEventListener.Get(shengbut).onClick = onclicklevelup;
        UIEventListener.Get(shangbut).onClick = onclickshang;
        UIEventListener.Get(xiabut).onClick = onclickxia;

        SoundEventer.add_But_ClickSound(closebut);
        SoundEventer.add_But_ClickSound(rennode);
        SoundEventer.add_But_ClickSound(leftbut);
        SoundEventer.add_But_ClickSound(rightbut);
        SoundEventer.add_But_ClickSound(shengbut);
        SoundEventer.add_But_ClickSound(shengbut);
        SoundEventer.add_But_ClickSound(xiabut);
        shengbutsp = shengbut.GetComponent<UISprite>();
        shengbutcox = shengbut.GetComponent<BoxCollider>();
        evoleveNode.initData(() => { go_CommNode.SetActive(true); });
        tableNode.InitTables(new string[] { "属性", "赋能" }, onTableClick, onTableUnClick, checkTable);
    }

    int curtabelIndex = 0;
    public bool checkTable(int tableindex)
    {
        bool isOpen = true;
        var lastcamp = GameGlobal.gamedata.stageindex;
        if (tableindex == 1)
        {
            if (lastcamp < 70 )
            {
                UIManager.showToast("通过 见识恐怖吧 关卡后解锁!");
                isOpen = false;
            }
            else if (selectherodata != null && selectherodata.bean.level <= 0)
            {
                UIManager.showToast("伙伴未拥有!");
                isOpen = false;
            }
        }
        if (isOpen)
        {
            curtabelIndex = tableindex;
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
       // SoundPlayer.getInstance().PlayW(sname, false);
    }

    private void onclickshang(GameObject go)
    {
        HttpManager.instance.sendPartnerShang(selectherodata.bean, (int code) =>
        {
            SoundPlayer.getInstance().PlayW(selectherodata.bean.heroID + "_star01", false);
            refreshui();
            UIManager.showToast("上阵成功");
        });
    }

    private void onclickxia(GameObject go)
    {
        HttpManager.instance.sendPartnerXia(selectherodata.bean, (int code) =>
        {
            refreshui();
            UIManager.showToast("休息成功");
        });
    }

    private void onclicklevelup(GameObject go)
    {

        if (GameGlobal.gamedata.getCurrendy(nextlevelbean.getlvcost().mainType) < nextlevelbean.getlvcost().val)
        {
            UIManager.showToast("资源不足");
            return;
        }

        if (selectherodata.bean.level >= Statics.PARTNERLEVELMAX)
        {
            //UIManager.showToast("等级已满");
            return;
        }

        HttpManager.instance.sendHeroLevelUp(selectherodata.bean, nextlevelbean.getlvcost(), (int code) =>
        {
            refreshui();
            //UIManager.showToast("升级成功");
            xgpaintnode.gameObject.SetActive(true);
            xgpaintnode.playActionAuto(0, true, () =>
            {
                xgpaintnode.gameObject.SetActive(false);
            });


        });
    }
    private void onclickright(GameObject go)
    {
        PartnerInfoItem_Data next = itemdatalist[(selectherodata.index + 1 + itemdatalist.Count) % itemdatalist.Count];
        //if (next.bean.level > 0)
        //{
            setselect(next);
            correctScrollItem_GridItem(grid, next);
        //}
        //else
        //{
        //    UIManager.showToast("未拥有！");
        //}
    }

    private void onclickleft(GameObject go)
    {
        PartnerInfoItem_Data next = itemdatalist[(selectherodata.index - 1 + itemdatalist.Count) % itemdatalist.Count];
        //if (next.bean.level > 0)
        //{
        setselect(next);
        correctScrollItem_GridItem(grid, next);
        //}
        //else
        //{
        //    UIManager.showToast("未拥有！");
        //}
    }

    public static void correctScrollItem_GridItem(GridAdapter grid, GridItem_Data showData)
    {

        UIScrollView ScrollView = grid.scrollView;
        UIPanel scrollPanel = grid.scrollPanel;
        SpringPanel spring = ScrollView.GetComponent<SpringPanel>();


        int showIndex = 0;
        for (int i = 0; i < grid.getDataList().Count; i++)
        {
            if (grid.getDataList()[i] == showData)
            {
                showIndex = i;
                break;
            }
        }

        showIndex = showIndex / grid.maxColumeLimit;

        if (ScrollView.canMoveVertically)
        {

            //小数                        //大数
            Vector2 moveRange = new Vector2(grid.panelInitPosition.y, grid.cellHeight * grid.getDataList().Count + grid.panelInitPosition.y - scrollPanel.GetViewSize().y);


            float goalTargetY = (grid.cellHeight * showIndex - scrollPanel.GetViewSize().y / 2);
            if (goalTargetY < moveRange.x)
            {
                goalTargetY = moveRange.x;
            }
            else if (goalTargetY > moveRange.y)
            {
                goalTargetY = moveRange.y;
            }

            //动画
            if (spring == null)
            {
                spring = ScrollView.gameObject.AddComponent<SpringPanel>();
            }
            spring.target = new Vector3(scrollPanel.transform.localPosition.x, goalTargetY, scrollPanel.transform.localPosition.z);
            spring.enabled = true;

        }
        else if (ScrollView.canMoveHorizontally)
        {
            Vector2 moveRange = new Vector2(grid.panelInitPosition.x, -grid.cellWidth * grid.getDataList().Count + grid.panelInitPosition.x + scrollPanel.GetViewSize().x);
            float goalTargetX = (-grid.cellWidth * showIndex + scrollPanel.GetViewSize().x / 2);
            if (goalTargetX > moveRange.x)
            {
                goalTargetX = moveRange.x;
            }
            else if (goalTargetX < moveRange.y)
            {
                goalTargetX = moveRange.y;
            }

            //动画
            if (spring == null)
            {
                spring = ScrollView.gameObject.AddComponent<SpringPanel>();
            }
            spring.target = new Vector3(goalTargetX, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);
            spring.enabled = true;
        }

        spring.Start();
        spring.AdvanceTowardsPosition(true);
        spring.enabled = false;
        grid.Update();
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

        initlist();
    }
    List<PartnerInfoItem_Data> itemdatalist = new List<PartnerInfoItem_Data>();
    public void initlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();
        itemdatalist.Clear();

        for (int i = 0; i < GameGlobal.gamedata.partnerlist.Count; i++)
        {

            PartnerInfoItem_Data temp = new PartnerInfoItem_Data();
            temp.bean = GameGlobal.gamedata.partnerlist[i];
            temp.index = i;

            temp.callObj = (object backObj) =>
            {
                if (curtabelIndex == 1 && temp.bean.level <= 0)
                {
                    UIManager.showToast("未拥有！");
                  
                }
                else
                {
                    setselect(temp);
                }
            };
            itemDatas.Add(temp);
            itemdatalist.Add(temp);
        }
        grid.clear();
        grid.setListData(itemDatas, itemObject, GRID_RUNTYPE.GRID_TYPE_READLOAD);


        setselect(itemdatalist[0]);

        //GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow, (int)GuideAcitveWindow.Partner);
        StartCoroutine(startGuideNextframe());
    }
    
    IEnumerator startGuideNextframe()
    {
        yield return null;
        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow, (int)GuideAcitveWindow.Partner);
    }
    public PartnerInfoItem_Data selectherodata;
    private void setselect(PartnerInfoItem_Data temp)
    {
        for (int i = 0; i < itemdatalist.Count; i++)
        {
            if (itemdatalist[i] == temp)
            {
                itemdatalist[i].selected = true;
                if (itemdatalist[i].item != null)
                {
                    itemdatalist[i].item.setSelect(true);
                }
                selectherodata = itemdatalist[i];
                setDetail(selectherodata);
            }
            else
            {
                itemdatalist[i].selected = false;
                if (itemdatalist[i].item != null)
                {
                    itemdatalist[i].item.setSelect(false);
                }
            }
        }
        setSkillItems();
        if (temp.bean.level > 0)
        {
            evoleveNode.setData(selectherodata);
        }
    }

    void setSkillItems()
    {
        var herolevelbean = data_hero_upgradeDef.dicdatas[selectherodata.bean.heroID][selectherodata.bean.level];
        SkillItemData itemdata1 = new SkillItemData
        {
            skillIndex = 1,
            skillid = dbbean.skill1_id,
            skilllevel = herolevelbean.skill1_level,
            nextneedlevel = data_hero_upgradeDef.getNextSkillNeedHeroLevel(selectherodata.bean.heroID, 1, herolevelbean.skill1_level),
            bean = data_skillDef.dicdatas[dbbean.skill1_id],
        };
        skillitems[0].initItem(itemdata1);
        SkillItemData itemdata2 = new SkillItemData
        {
            skillIndex = 2,
            skillid = dbbean.skill2_id,
            skilllevel = herolevelbean.skill2_level,
            nextneedlevel = data_hero_upgradeDef.getNextSkillNeedHeroLevel(selectherodata.bean.heroID, 2, herolevelbean.skill2_level),
            bean = data_skillDef.dicdatas[dbbean.skill2_id],
        };
        skillitems[1].initItem(itemdata2);
        SkillItemData itemdata3 = new SkillItemData
        {
            skillIndex = 3,
            skillid = dbbean.skill3_id,
            skilllevel = herolevelbean.skill3_level,
            nextneedlevel = data_hero_upgradeDef.getNextSkillNeedHeroLevel(selectherodata.bean.heroID, 3, herolevelbean.skill3_level),
            bean = data_skillDef.dicdatas[dbbean.skill3_id],
        };
        skillitems[2].initItem(itemdata3);
    }

    private void setDetail(PartnerInfoItem_Data selectherodata)
    {
        dbbean = data_heroDef.getdatabyheroid(selectherodata.bean.heroID);

        if (renpaintnode == null)
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
            texframeobjs.name = "" + selectherodata.bean.heroID;
            APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
            temppaintnode.create1(rennode, dbbean.resource, dbbean.resource);
            temppaintnode.transform.localPosition = Vector3.zero;
            temppaintnode.transform.localScale = Vector3.one * 0.5f;
            temppaintnode.setdepth(11);
            temppaintnode.setAutoAlpha(true);
            renpaintnode = temppaintnode;
        }
        else
        {
            renpaintnode.create1(rennode, dbbean.resource, dbbean.resource);
            renpaintnode.transform.localPosition = Vector3.zero;
            renpaintnode.transform.localScale = Vector3.one * 0.5f;
            renpaintnode.setdepth(11);
            renpaintnode.setAutoAlpha(true);
        }

        renpaintnode.playAction2Auto("idle", true);

        if (selectherodata.bean.level > 0)
        {
            labLeveNotice.text = "";
            goOptionroot.SetActive(true);
        }
        else
        {
            string name = data_campaignDef.dataDic[selectherodata.bean.dbbean.campaign_id].name;
            labLeveNotice.text = "通过 "+ name +" 关卡后加入!";
            goOptionroot.SetActive(false);
        }
        refreshui();
    }
    data_hero_upgradeBean nextlevelbean;
    data_heroBean dbbean;
    bool iscan = true;
    private void refreshui()
    {
        if (selectherodata.bean.level >= Statics.PARTNERLEVELMAX)
        {
            shengbut.SetActive(false);
            maxbut.SetActive(true);
        }
        else
        {
            shengbut.SetActive(true);
            maxbut.SetActive(false);
            nextlevelbean = data_hero_upgradeDef.dicdatas[selectherodata.bean.heroID][selectherodata.bean.level];
            var cost = nextlevelbean.getlvcost();
            costitem.setReward(cost);
            long havenum = GameGlobal.gamedata.getCurrendy(cost.mainType);
            iscan = havenum >= cost.val;
            if (iscan)
            {
                shengbutsp.spriteName = "btm_shengji";
                shengbutcox.enabled = true;
            }else
            {
                shengbutsp.spriteName = "btn_hui";
                shengbutcox.enabled = false;
            }
           
        }

        nameandlvlabel.text = dbbean.name + "  等级" + selectherodata.bean.level;
        zhanlilabel.text = DataManager.GetHeroZhanli(selectherodata.bean) + "";

        if (GameGlobal.gamedata.selectpartner == selectherodata.bean)
        {
            shangbut.SetActive(false);
            xiabut.SetActive(true);
        }
        else
        {
            shangbut.SetActive(true);
            xiabut.SetActive(false);
        }

    }
}
