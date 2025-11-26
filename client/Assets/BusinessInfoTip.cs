using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessInfoTip : DialogMonoBehaviour
{
    public static BusinessInfoTip instance;
    public UIWidget bgwidget;
    public GameObject closebtn;
    public GameObject go_confirmbtn;
    public GameObject go_btnTrade;
    public GameObject go_noNode;
    public GameObject go_haveNode;
    public List<RewardItem> rewardItems;

    private void Awake()
    {
        instance = this;
        setShowAnim(false);
        setClickOutZoneClose(false);
        UIEventListener.Get(closebtn).onClick = closeDialog;
        UIEventListener.Get(go_confirmbtn).onClick = closeDialog;
        UIEventListener.Get(go_btnTrade).onClick = onADGetBtnClick;
        SoundEventer.add_But_ClickSound(closebtn);
        SoundEventer.add_But_ClickSound(go_confirmbtn);
        SoundEventer.add_But_ClickSound(go_btnTrade);
    }

    private void onADGetBtnClick(GameObject go)
    {
        //SDKManager.instance.ShowRewardsAD("businessman", (ca) =>
        //{
        //    if (ca == Callback.SUCCESS)
        //    {
                OnGetADSuccess();
        //    }
        //});
    }

    private void OnGetADSuccess()
    {
        string ADIdstr = GameGlobal.gamedata.curADServerId;
        HttpManager.instance.GetTradeGoods(0, ADIdstr, (callint) =>
         {
             if (callint == Callback.SUCCESS)
             {
                 GameGlobal.gamedata.userinfo.extend.businessMan += 1;
                 this.closeDialog(closebtn.gameObject);
             }
         });

    }

    private void Start()
    {
        this.initdata();
    }

    int istradTime = 0;
    private void initdata()
    {
        var businessStat = GameGlobal.gamedata.userinfo.extend.businessMan;
        Debug.LogError( " businessStat " + GameGlobal.gamedata.userinfo.extend.businessMan);

        istradTime = isTradeTime();
     //   istradTime = 4;
        Debug.LogError(" istradTime " + istradTime);
        setGetbtn(istradTime > businessStat);
    }

    int isTradeTime()
    {
        int istrade = 0;
        int curtime = (int)(HttpManager.currentTimeMillis() / 1000)+ 3600*8;
        int passed = curtime % 86400;
        Debug.LogError(" passed " + passed);
        if (43200 <= passed && passed < 50400) //12:00-- 14:00
        {
            istrade = 1;
        }
        else if (64800 <= passed && passed < 72000)//18:00-- 20:00
        {
            istrade = 2;
        }
        return istrade;
    }

    void setGetbtn(bool iscantrade)
    {
        if (iscantrade)
        {
            var getGold = GameGlobal.gamedata.userinfo.goldIncrement * 3600 * 6;
            rewardItems[0].setReward(new Reward() { mainType = 1, val = getGold });
            bgwidget.height = 1200;
            go_haveNode.SetActive(true);
            go_noNode.SetActive(false);

        }
        else
        {
            bgwidget.height = 960;
            go_noNode.SetActive(true);
            go_haveNode.SetActive(false);
        }
    }

}