using System.Collections.Generic;
using UnityEngine;

public class TreasurehuntOKTip : DialogMonoBehaviour
{
    public GameObject closebtn;
    public UIGrid grid;
    public GameObject btnADGet;
    public GameObject btnCommGet;
    RewardItem[] rewardItemlist;
    private void Awake()
    {
        setShowAnim(true);
        setClickOutZoneClose(true);
        UIEventListener.Get(closebtn).onClick = closeDialog;
        UIEventListener.Get(btnADGet).onClick = onbtnADGetClick;
        UIEventListener.Get(btnCommGet).onClick = onbtnCommGetClick;
        SoundEventer.add_But_ClickSound(closebtn);
        SoundEventer.add_But_ClickSound(btnADGet);
        SoundEventer.add_But_ClickSound(btnCommGet);
        int count = grid.transform.childCount;
        rewardItemlist = new RewardItem[count];
        for (int i = 0; i < count; i++)
        {
            rewardItemlist[i] = grid.transform.GetChild(i).GetComponent<RewardItem>();
            rewardItemlist[i].gameObject.SetActive(false);
        }
    }

    private void onbtnCommGetClick(GameObject go)
    {
        getReward(0);
    }
    public void OnGetADSuccess()
    {
        getReward(2);
    }
    private void onbtnADGetClick(GameObject go)
    {
        if (GameGlobal.gamedata.userinfo.extend.onlineReward.remainAdsNum > 0)
        {
            //SDKManager.instance.ShowRewardsAD("treasureHunter", (ca) => {
            //    if (ca == Callback.SUCCESS)
            //    {
                    OnGetADSuccess();
            //    }
            //});
            //GameGlobal.gamedata.userinfo.extend.onlineReward.remainAdsNum -= 1;
            //OnGetADSuccess();
        }
        else
        {
            UIManager.showToast("今日广告次数用完！");
        }
    }

    void getReward(int getType)
    {
        string ADIdstr = "";
        if (getType == 1)
        {
            ADIdstr = GameGlobal.gamedata.curADServerId;
        }
        HttpManager.instance.getTeasurehuntReward(getType, ADIdstr,(callint) =>
        {
            if (callint == Callback.SUCCESS)
            {
                HttpManager.instance.sendUserInfo(null);
                closeDialog(closebtn);
                if (isEquipdrop)
                {
                    HttpManager.instance.GetEquipList(1, 100, null);
                }
                //UIManager.showToast("奖励领取成功！");
                if (getType == 1)
                {
                    foreach (var item in rewardslist)
                    {
                        item.val += item.val;
                    }
                }
                UIManager.showAwardDlg(rewardslist);
            }
        });
    }
    //Dictionary<int, string> dic_rwarddata;
    private void Start()
    {
        //dic_rwarddata = new Dictionary<int, string>
        //{
        //    {1,"1_0_5000"},
        //    {2,"2_0_100"},
        //    {3,"16_99999_1"},
        //    {4,"4_0_2000"},
        //};
        setRewardList();
        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow, (int)GuideAcitveWindow.Yingdi);
    }

    bool isEquipdrop = false;
    List<Reward> rewardslist;
    private void setRewardList()
    {
        var rewards = GameGlobal.gamedata.userinfo.extend.onlineReward.rewardIDs;
        rewardslist = new List<Reward>();
        for (int i = 0; i < rewards.Count; i++)
        {
            int rewdId = GameGlobal.gamedata.userinfo.extend.onlineReward.rewardIDs[i];
            if (rewdId == 3)
            {
                isEquipdrop = true;
            }
            
            var data = data_online_rewardDef.dicdatas[rewdId][0].rewards;// dic_rwarddata[rewdId];
            var rwardstr = data.Split('_');
            Reward reward = new Reward {
                mainType = int.Parse(rwardstr[0]),
                subType = int.Parse(rwardstr[1]),
                val = int.Parse(rwardstr[2]),
            };
            rewardslist.Add(reward);
            rewardItemlist[i].setData(reward, onClickRewardItem);
            rewardItemlist[i].gameObject.SetActive(true);
        }
    }

    void onClickRewardItem(object rewardObj)
    {
        Reward rward = (Reward)rewardObj;
        UIManager.showItemInfoTip(rward);
    }
}