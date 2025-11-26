using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferAddInfoTip : DialogMonoBehaviour
{
    public static BufferAddInfoTip instance;
    public GameObject closebtn;
    public UILabel la_des;
    public UILabel la_ad;
    public UISprite sp_getbtn;
    public UILabel txtCost;
    private void Awake()
    {
        instance = this;
        setShowAnim(false);
        setClickOutZoneClose(false);
        UIEventListener.Get(closebtn).onClick = closeDialog;
        UIEventListener.Get(sp_getbtn.gameObject).onClick = onADGetBtnClick;
        SoundEventer.add_But_ClickSound(closebtn);
        SoundEventer.add_But_ClickSound(sp_getbtn.gameObject);
    }

    private void onADGetBtnClick(GameObject go)
    {
        if (!opflag)
        {
            OnGetADSuccess();
        }
        else
        {
            if (iscanadd)
            {
                //SDKManager.instance.ShowRewardsAD("buffer", (ca) =>
                //{
                //    if (ca == Callback.SUCCESS)
                //    {
                OnGetADSuccess();
                //    }
                //});



            }
            //else
            //{
            //    HttpManager.instance.getTeasurehuntReward(2, "", (callint) =>
            //    {
            //        if (callint == Callback.SUCCESS)
            //        {
            //            GameGlobal.gamedata.subCurrendy(Statics.TYPE_diamond, 20);

            //            OnGetADSuccess();
            //        }
            //    });
            //}
        }


    }

    private void OnGetADSuccess()
    {
        string ADIdstr = GameGlobal.gamedata.curADServerId;
        HttpManager.instance.GetBuffGoldAdd(ADIdstr, (callint) =>
       {
           if (callint == Callback.SUCCESS)
           {
               GameGlobal.gamedata.subCurrendy(Statics.TYPE_diamond, 20);
               UIManager.showToast("法阵开启成功！");
               CloseInfoTip();
           }
       });

    }

    private void Start()
    {
        this.initdata();
    }

    int leftTime = 0;
    private void initdata()
    {
        var endtime = GameGlobal.gamedata.userinfo.goldBuffEndIn;

        leftTime = (int)(endtime - (HttpManager.currentTimeMillis() / 1000));

       // Debug.LogError("endtime=" + endtime+" lefttime="+leftTime);
        GameGlobal.instance.SecondTimerCall += setTimeDes;
        la_ad.text = "开启";
        setTimeDes();
        long have = GameGlobal.gamedata.getCurrendy(2);
        txtCost.text = "20" + "/" + GameGlobal.gamedata.GetNumStr(have);
    }


    bool iscanadd = false;
    bool opflag = false;
    private void setTimeDes()
    {
        iscanadd = false;
        opflag = false;
        if (leftTime > 0)
        {
            opflag = true;
            var datetime = GameGlobal.gamedata.GetTimeHMS(leftTime);
            la_des.text = "法阵已经开启，家园建筑产出金币翻倍，剩余时间" + datetime.h + "小时" + datetime.m + "分" + datetime.s + "秒。";
            leftTime -= 1;
            if (leftTime <= 36000)
            {
                iscanadd = true;
               
            }
        }
        else
        {
            la_des.text = "当前法阵未启用，启用后可以使家园建筑产出金币翻倍。";
           // iscanadd = true;
        }

        setGetbtn();
    }

    void setGetbtn()
    {
        if (!opflag)
        {
            sp_getbtn.spriteName = "btm_dalan";
        }
        else
        {
            if (iscanadd)
            {
                sp_getbtn.spriteName = "btm_dalan";
                //if (leftTime > 0)
                //{
                //    la_ad.text = "已开启";
                //}
                //else
                //{
                //    la_ad.text = "开启";
                //}
            }
            else
            {

                sp_getbtn.spriteName = "btn_hui";
                //la_ad.text = "已开启";
            }
        }
       

       
    }
    public void CloseInfoTip()
    {
        GameGlobal.instance.SecondTimerCall -= setTimeDes;
        this.closeDialog(closebtn.gameObject);
    }

    private void OnDestroy()
    {
        GameGlobal.instance.SecondTimerCall -= setTimeDes;
    }

}
