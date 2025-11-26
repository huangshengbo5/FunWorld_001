using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tianfunode : MonoBehaviour
{
    public UILabel title;
    public UILabel lab_curName;
    public UILabel lab_curDes;
    public UILabel lab_nextName;
    public UILabel lab_nextDes;

    public GameObject go_Upcost;
    public GameObject go_costsp;
    public UILabel lab_cost;
    public RewardItem costItem;

    public GameObject go_btnUp;
    public GameObject go_MaxMark;

    public HeroInfoStoneItem[] stonList;
    HeroInfoStoneItem curSelectItem;
    data_crystal_upgradeBean curbean;
    data_crystal_upgradeBean nextbean;
    UISprite go_btnUpSp;
    BoxCollider go_btnUpcox;

    void Start()
    {
        UIEventListener.Get(go_btnUp).onClick = OnUpBtnClick;
        go_btnUpSp = go_btnUp.GetComponent<UISprite>();
        go_btnUpcox = go_btnUp.GetComponent<BoxCollider>();
        initStoneListData();
    }

    void initStoneListData()
    {
        if (GameGlobal.gamedata.herostonelist.Count <= 0)
        {
            UIManager.showToast("资源不足!");
            return;
        }
        for (int i = 0; i < stonList.Length; i++)
        {
            stonList[i].stoneData = GameGlobal.gamedata.herostonelist[i];

            stonList[i].onItemClickCall = setCurSelectItem;
        }
        setCurSelectItem(stonList[0]);
    }

    void setCurSelectItem(HeroInfoStoneItem selectitem)
    {
        if (curSelectItem)
        {
            if (curSelectItem.stoneData.id != selectitem.stoneData.id)
            {
                curSelectItem.SetSelect(false);
            }
        }
        curSelectItem = selectitem;
        curSelectItem.SetSelect(true);
        nextbean = null;
        var datas = data_crystal_upgradeDef.dicdatas[curSelectItem.stoneData.CrystalID];
        int curLevel = curSelectItem.stoneData.level;
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].level == curLevel)
            {
                curbean = datas[i];
            }
            else if (datas[i].level == curLevel + 1)
            {
                if (curLevel == 0)
                {
                    curbean = datas[i];
                }
                nextbean = datas[i];
            }
        }

        setCurSelectUI();
    }

    void OnUpBtnClick(GameObject go)
    {
        if (curSelectItem)
        {
            int id = curSelectItem.stoneData.id;
            Reward cost = new Reward(nextbean.cost_type, nextbean.cost_sub_type, nextbean.cost_val);
            HttpManager.instance.sendHeroStoneLevelUp(curSelectItem.stoneData, cost, (callInt) =>
            {
                if (callInt == Callback.SUCCESS)
                {
                    if (id == curSelectItem.stoneData.id)
                    {
                        setCurSelectItem(curSelectItem);
                    }
                }
            });
        }
    }

    bool isCanUp = true;
    static string[] titlename = new string[]
    {
        "",
           "炽焰",
   "跃水",
   "飓风",
   "大地",
   "光明",
   "黑暗",
   "时空"
    };
    void setCurSelectUI()
    {
        title.text = GameGlobal.gamedata.GetNamebyCrystalId(curbean.crystal_id);
        lab_curName.text = GameGlobal.gamedata.GetNamebyCrystalId(curbean.crystal_id) + " 等级：" + curSelectItem.stoneData.level;
        if (curSelectItem.stoneData.level == 0)
        {
            lab_curDes.text = GameGlobal.gamedata.GetEffectDesByID(curbean.effect_id) + " 0" ;
        }
        else
        {

            lab_curDes.text = GameGlobal.gamedata.GetEffectDesByID(curbean.effect_id) + " +" + curbean.effect_val;
        }

        if (nextbean != null)
        {
            go_MaxMark.SetActive(false);
            go_btnUp.SetActive(true);
            go_Upcost.SetActive(true);
            setCostpic();
            long havenum = GameGlobal.gamedata.getCurrendy(nextbean.cost_type);
            lab_cost.text = GameGlobal.gamedata.GetNumStr(nextbean.cost_val) + "/" + GameGlobal.gamedata.GetNumStr(havenum);
            lab_nextName.text = GameGlobal.gamedata.GetNamebyCrystalId(nextbean.crystal_id) + " 等级：" + (curSelectItem.stoneData.level + 1);
            lab_nextDes.text = GameGlobal.gamedata.GetEffectDesByID(nextbean.effect_id) + " +" + nextbean.effect_val;
            isCanUp = havenum >= nextbean.cost_val;
            if (isCanUp)
            {
                go_btnUpcox.enabled = true;
                go_btnUpSp.spriteName = "btm_shengji";
            }
            else
            {
                go_btnUpcox.enabled = false;
                go_btnUpSp.spriteName = "btn_hui";
            }
        }
        else
        {
            go_MaxMark.SetActive(true);
            go_btnUp.SetActive(false);
            go_Upcost.SetActive(false);
            lab_nextName.text = lab_curName.text;
            lab_nextDes.text = lab_curDes.text;
        }
    }
    TexPaintNode paintnode;
    void setCostpic()
    {
        string path = "item";
        string pic = "";
        data_resourcesBean resoursbean = data_resourcesDef.dicdatas[nextbean.cost_type][0];
        pic = resoursbean.resources_picture;
        if (paintnode == null)
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(go_costsp, path);
            temppaintnode.setdepth(402);
            temppaintnode.setShowRectLimit(60, 60);
            paintnode = temppaintnode;
        }
        paintnode.playAction(pic);
    }
}
