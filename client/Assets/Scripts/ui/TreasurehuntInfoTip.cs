using System;
using UnityEngine;
public class TreasurehuntInfoTip : DialogMonoBehaviour
{
    public static TreasurehuntInfoTip instance;
    public GameObject closebtn;
    public UILabel txtDes;
    public UILabel txtCost;
    public GameObject btnADGet;
    public GameObject goADGettxt;
    public GameObject goCostGettxt;
    UISprite btnADGetsp;
    BoxCollider btnADGetcox;
    private void Awake()
    {
        instance = this;
        setShowAnim(false);
        setClickOutZoneClose(false);
        UIEventListener.Get(closebtn).onClick = closeDialog;
        UIEventListener.Get(btnADGet).onClick = onADGetBtnClick;
        SoundEventer.add_But_ClickSound(closebtn);
        SoundEventer.add_But_ClickSound(btnADGet);
        btnADGetsp = btnADGet.GetComponent<UISprite>();
        btnADGetcox = btnADGet.GetComponent<BoxCollider>();
    }


    private void Start()
    {
        this.initdata();
    }

    int leftTime = 0;
    private void initdata()
    {
        GameGlobal.gamedata.userinfo.extend.onlineReward.payNum = 50;
        var onlineRewardData = GameGlobal.gamedata.userinfo.extend.onlineReward;
        leftTime = onlineRewardData.nextReceiveTime - (int)(HttpManager.currentTimeMillis() / 1000);
        GameGlobal.instance.SecondTimerCall += setTimeDes;
        setTimeDes();
        long have = GameGlobal.gamedata.getCurrendy(2);
       txtCost.text = onlineRewardData.payNum+"/" + GameGlobal.gamedata.GetNumStr(have);
        goADGettxt.SetActive(onlineRewardData.payNum <= 0);
        goCostGettxt.SetActive(onlineRewardData.payNum > 0);
        btnADGetsp.spriteName = "btm_dalan";
        btnADGetcox.enabled = true;

        if (onlineRewardData.payNum > 0 && have >= onlineRewardData.payNum)
        {
           // btnADGetsp.spriteName = "btn_hui";
           // btnADGetcox.enabled = false;
        }
    }

    private void setTimeDes()
    {
        if (leftTime > 0)
        {
            var datetime = GameGlobal.gamedata.GetTimeHMS(leftTime);
            txtDes.text = "寻宝还需" + datetime.h + "小时" + datetime.m + "分" + datetime.s + "秒完成，当前可以立刻完成寻宝，是否立刻完成？";
            leftTime -= 1;
        }
        else
        {
            OnHuntInTime();
        }
    }

    public void OnGetADSuccess()
    {
        OnHuntInTime(0);

    }

    private void OnDestroy()
    {
        GameGlobal.instance.SecondTimerCall -= setTimeDes;
    }

    private void onADGetBtnClick(GameObject go)
    {
        if (GameGlobal.gamedata.userinfo.extend.onlineReward.remainAdsNum > 0)
        {
            //SDKManager.instance.ShowRewardsAD("treasureHunter", (ca) =>
            //{
            //    if (ca == Callback.SUCCESS)
            //    {
                    OnGetADSuccess();
            //    }
            //});
        }
        else
        {
            OnHuntInTime(2);
        }
    }

    public void CloseInfoTip()
    {
        GameGlobal.instance.SecondTimerCall -= setTimeDes;
        this.closeDialog(closebtn.gameObject);
    }

    public void OnHuntInTime(int getType = 0)
    {
        GameGlobal.gamedata.userinfo.extend.onlineReward.remainAdsNum = -1;

        string ADIdstr = "";
        if (getType == 1)
        {
            ADIdstr = GameGlobal.gamedata.curADServerId;
        }
        GameGlobal.gamedata.userinfo.extend.onlineReward.nextReceiveTime = (int)HttpManager.currentTimeMillis() / 1000;
        HttpManager.instance.getTeasurehuntInfo(getType, ADIdstr, (callint) =>
         {
             if (callint == Callback.SUCCESS)
             {
                 if (getType == 1 && GameGlobal.gamedata.userinfo.extend.onlineReward.remainAdsNum > 0)
                 {
                     GameGlobal.gamedata.userinfo.extend.onlineReward.remainAdsNum -= 1;
                 }
                 else
                 {
                     var onlineRewardData = GameGlobal.gamedata.userinfo.extend.onlineReward;
                     GameGlobal.gamedata.subCrystals(onlineRewardData.payNum);
                 }
                 UIManager.showTreasurehuntOKTip();
             }
         });
        CloseInfoTip();
    }
}