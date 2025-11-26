using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergeTransNode : MonoBehaviour
{
    public GameObject go_btnTrans;
    public GameObject go_adTrans;
    public GameObject go_btnClear;
    public RewardItem rewardItem;
    public UILabel lab_title;
    public UILabel lab_CD;
    public UILabel lab_cost;
    public GameObject go_costsp;
    public UISprite go_btnTransSp;
    public UISprite go_btnClearSp;
    public BoxCollider go_btnTranscox;
    public BoxCollider go_btnClearcox;
    void Start()
    {
        go_btnClearSp = go_btnClear.GetComponent<UISprite>();
        go_btnClearcox = go_btnClear.GetComponent<BoxCollider>();
        go_btnTransSp = go_btnTrans.GetComponent<UISprite>();
        go_btnTranscox = go_btnTrans.GetComponent<BoxCollider>();
        UIEventListener.Get(go_btnClear).onClick = onbtnClearclick;
        UIEventListener.Get(go_btnTrans).onClick = onbtnTransclick;
        UIEventListener.Get(go_adTrans).onClick = onADTransClik;
    }

    private void onADTransClik(GameObject go)
    {
        //SDKManager.instance.ShowRewardsAD("energeTrans", (callint) =>
        //{
        //    if (callint == Callback.SUCCESS)
        //    {
                EnergeTrans(1);
        //    }
        //});
    }

    Reward transCost;
    Reward transGet;
    void setUI()
    {
        int level = GameGlobal.gamedata.GetJuexingLevel();
        lab_title.text = "觉醒  等级 " + level;
        var data = GameGlobal.gamedata.GetLiandanDataByJuexinglevel(level);
        transGet = Reward.decode(data.rewards);
        rewardItem.setReward(transGet);
        transCost = new Reward(data.cost_type, data.cost_sub_type, data.cost_val);
        setBtnState();
    }

    bool isCanTrans = true;
    void setBtnState()
    {
        var alchemy = GameGlobal.gamedata.userinfo.extend.alchemy;
        var timel = alchemy.nextReceiveTime - HttpManager.currentTimeMillis() / 1000;
        if (timel > 0)
        {
            leftTime = (int)timel;
            go_btnClear.SetActive(true);
            go_btnTrans.SetActive(false);
            go_adTrans.SetActive(false);
            long havenum = GameGlobal.gamedata.getCurrendy(2);
            long CDcost = (long)getClearCDCost(alchemy.payNum + 1);
            lab_cost.text = GameGlobal.gamedata.GetNumStr(CDcost) + "/" + GameGlobal.gamedata.GetNumStr(havenum);
            setCostpic(2);//消耗钻石
            isCanTrans = havenum >= CDcost;
            if (isCanTrans)
            {
                go_btnTranscox.enabled = true;
                go_btnTransSp.spriteName = "btm_dalan";
            }
            else
            {
                go_btnTranscox.enabled = false;
                go_btnTransSp.spriteName = "btn_hui";
            }
        }
        else
        {
            leftTime = 0;
            lab_CD.text = "";
            go_btnClear.SetActive(false);
            go_btnTrans.SetActive(true);
            go_adTrans.SetActive(false);
           long havenum = GameGlobal.gamedata.getCurrendy(transCost.mainType);
            lab_cost.text = GameGlobal.gamedata.GetNumStr(transCost.val) + "/" + GameGlobal.gamedata.GetNumStr(havenum);
            setCostpic(transCost.mainType);//消耗金币
            isCanTrans = havenum >= transCost.val;
            if (isCanTrans)
            {
                go_btnClearcox.enabled = true;
                go_btnClearSp.spriteName = "btm_dalan";
            }
            else
            {
                go_btnClearcox.enabled = false;
                go_btnClearSp.spriteName = "btn_hui";
            }
        }
    }

    TexPaintNode paintnode;
    void setCostpic(int costType)
    {
        string path = "item";
        string pic = "";
        data_resourcesBean resoursbean = data_resourcesDef.dicdatas[costType][0];
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
        //paintnode.transform.localScale = new Vector3(0.8f, 0.8f, 0);
    }

    Decimal getClearCDCost(int cleartimes) // 第1次消耗 20钻 以后每次消耗比前一次多一倍 
    {
        Decimal cost = 20;
        for (int i = 0; i < cleartimes - 1; i++)
        {
            cost += cost;
        }
        return cost;
    }

    private void onbtnTransclick(GameObject go)
    {
        if (isCanTrans)
        {
            EnergeTrans();
        }
    }

    void EnergeTrans(int type = 0)
    {
        string ADIdstr = "";
        if (type == 1)
        {
            ADIdstr = GameGlobal.gamedata.curADServerId;
        }
        HttpManager.instance.EnergeTransfor(type, ADIdstr, (callint) =>
         {
             if (callint == Callback.SUCCESS)
             {
                 //UIManager.showToast("转化成功！");
                 long v = transGet.val;
                 if (type == 1)
                 {
                     v += transGet.val;
                 }
                 UIManager.showAwardDlg(new List<Reward>() { new Reward() { mainType = transGet.mainType, val = v, } });
                 setBtnState();
             }
         });//0 正常 1 广告
    }

    private void onbtnClearclick(GameObject go)
    {
        if (isCanTrans)
        {
            HttpManager.instance.ClearEnergeTansCD((callint) =>
            {
                if (callint == Callback.SUCCESS)
                {
                    //UIManager.showToast("CD清除成功！");
                    setBtnState();
                }
            });
        }
    }

    int leftTime = 0;
    private void setTimeDes()
    {
        if (leftTime > 0)
        {
            var datetime = GameGlobal.gamedata.GetTimeHMS(leftTime);
            lab_CD.text = datetime.h + ":" + datetime.m + ":" + datetime.s;
            leftTime -= 1;
        }
        else if (GameGlobal.gamedata.userinfo.extend.alchemy.nextReceiveTime > 0)
        {
            GameGlobal.gamedata.userinfo.extend.alchemy.nextReceiveTime = 0;
            setBtnState();
        }
    }

    void OnEnable()
    {
        GameGlobal.instance.SecondTimerCall += setTimeDes;
        setUI();
    }

    void OnDisable()
    {
        GameGlobal.instance.SecondTimerCall -= setTimeDes;
    }
}
