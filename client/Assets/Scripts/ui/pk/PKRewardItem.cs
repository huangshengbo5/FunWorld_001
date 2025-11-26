using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArenaRewardItem_Data : GridItem_Data
{
    public data_arena_rewardBean reward;
    public int index;
    public bool selected = false;
    public callbackObj callObj;
    public PKRewardItem item;
}

public class PKRewardItem : GridItem
{
    public UILabel rankNum;//排名的名次
    public UISprite timeBg;//排名背景
    public UISprite timeBg1;//第一名显示
    public UISprite timeBg2;//第二名显示
    public UISprite timeBg3;//第三名显示

    public RewardItem[] rewards = new RewardItem[4];

    void Awake()
    {
        UIEventListener.Get(gameObject).onClick = onEvent_button;
        SoundEventer.add_But_ClickSound(gameObject);
    }


    private void onEvent_button(GameObject go)
    {
        //if (data.callObj != null)
        //{
        //    data.callObj(data);
        //}
    }
    ArenaRewardItem_Data data;
    public override void initItem(GridItem_Data data0)
    {
        data = (ArenaRewardItem_Data)data0;
        data.item = this;
        timeBg.gameObject.SetActive(false);
        timeBg1.gameObject.SetActive(false);
        timeBg2.gameObject.SetActive(false);
        timeBg3.gameObject.SetActive(false);
        if (data.reward.rank_top == 1)
        {
            timeBg1.gameObject.SetActive(true);
        }
        else if (data.reward.rank_top == 2)
        {
            timeBg2.gameObject.SetActive(true);
        }
        else if (data.reward.rank_top == 3)
        {
            timeBg3.gameObject.SetActive(true);
        }
        else
        {
            timeBg.gameObject.SetActive(true);
            rankNum.text = string.Format("{0}-{1}",data.reward.rank_top,data.reward.rank_bottom);
        }

        List<Reward> rblist = Reward.decodeList(data.reward.rewards);

        for (int i = 0; i < rewards.Length; i++)
        {
            if (i < rblist.Count)
            {
                rewards[i].gameObject.SetActive(true);
                rewards[i].setData(rblist[i], null);
            }
            else
            {
                rewards[i].gameObject.SetActive(false);
            }
        }
    }

    public void setSelect(bool v)
    {
        data.selected = v;
    }
}
