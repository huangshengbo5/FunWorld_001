using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HistoryItem_Data : GridItem_Data
{
    public data_annalsBean dbean;
    public int index;
    public HistoryItem item;
}
public class HistoryItem : GridItem
{
    public HistoryItem_Data data;
    public UILabel lab_Name;
    public RewardItem[] rewardItems;
    public GameObject go_OverMark;
    public GameObject go_BtnGet;
    public GameObject go_unGet;
    public UILabel lab_btnget;
    public UILabel lab_overtime;

    public override void initItem(GridItem_Data data0)
    {
        data = (HistoryItem_Data)data0;
        data.item = this;
        refreshUI();
    }

    private void Start()
    {
        UIEventListener.Get(go_BtnGet).onClick = onGetBtnClick;
    }

    void onGetBtnClick(GameObject go)
    {
        HttpManager.instance.GetHistoryRewards(data.dbean.id, (callInt) =>
        {
            if (callInt == Callback.SUCCESS)
            {
                go_OverMark.SetActive(true);
                go_BtnGet.SetActive(false);
            }
        });
    }

    void setBtnState()
    {
        bool isComplete = checkHistoryItemFinished();
        bool isGotton = checkHistoryRewardGot();
        go_BtnGet.SetActive(isComplete && !isGotton);
        go_unGet.SetActive(!isComplete && !isGotton);
        go_OverMark.SetActive(isGotton);
    }

    bool checkHistoryRewardGot()
    {
        bool isGot = false;
        var overids = GameGlobal.gamedata.historyInfoData.annalsIDs;
        for (int i = 0; i < overids.Length; i++)
        {
            if (overids[i] == data.dbean.id)
            {
                isGot = true;
                break;
            }
        }
        return isGot;
    }

    bool checkHistoryItemFinished()
    {
        var historydata = GameGlobal.gamedata.historyInfoData;
        bool isFinish = false;
        if (data.dbean.type == 999)//创建角色
        {
            isFinish = true;
        }
        else if (data.dbean.type == 1)//通过制定管卡
        {
            isFinish = historydata.campaignNum >= data.dbean.value;
        }
        else if (data.dbean.type == 2)//获得指定伙伴
        {
            for (int i = 0; i < historydata.heroList.Length; i++)
            {
                if (historydata.heroList[i].heroID == data.dbean.value)
                {
                    isFinish = true;
                    break;
                }
            }
        }
        else if (data.dbean.type == 3)//总战力达到一定值
        {
            isFinish = historydata.fightingCapacity >= data.dbean.value;
        }
        else if (data.dbean.type == 4)//指定伙伴达到战力值
        {
            for (int i = 0; i < historydata.heroList.Length; i++)
            {
                if (historydata.heroList[i].heroID == data.dbean.sub_type && historydata.heroList[i].fightingCapacity >= data.dbean.value)
                {
                    isFinish = true;
                    break;
                }
            }
        }
        return isFinish;
    }

    public void refreshUI()
    {
        lab_Name.text = data.dbean.name;
        setRewards();
        setBtnState();
    }

    void setRewards()
    {
        var rewardstr = data.dbean.rewards.Split(';');
        for (int i = 0; i < 4; i++)
        {
            if (i<rewardstr.Length)
            {
                var reward0str = rewardstr[i].Split('_');
                Reward reward = new Reward(int.Parse(reward0str[0]), int.Parse(reward0str[1]), int.Parse(reward0str[2]));
                rewardItems[i].setReward(reward);
                rewardItems[i].gameObject.SetActive(true);
            }
            else
            {
                rewardItems[i].gameObject.SetActive(false);
            }
        }
    }


}
