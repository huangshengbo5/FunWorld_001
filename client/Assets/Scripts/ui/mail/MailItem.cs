using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailInfoItem_Data : GridItem_Data
{
    public MailInfo mailData;
    public int index;
    public bool selected = false;
    public callbackObj callObj;
    public MailItem item;
}

public class MailItem : GridItem
{
    public UILabel nameLabel;//名称
    public UILabel timeLabel;//时间
    public UISprite lingleLabel;//已领取
    public UISprite butling;//领取

    public RewardItem[] rewards = new RewardItem[3];


    MailInfoItem_Data data;

    void Awake()
    {
        UIEventListener.Get(gameObject).onClick = onEvent_button;
        UIEventListener.Get(butling.gameObject).onClick = onRewardButton;
        SoundEventer.add_But_ClickSound(gameObject);
        SoundEventer.add_But_ClickSound(butling.gameObject);
    }

    private void onRewardButton(GameObject go)
    {
        if (data.mailData.hasReceived == 0)
        {
            HttpManager.instance.MailReceive(data.mailData.id, (code1) =>
            {
                if (code1 == Callback.SUCCESS)
                {
                    initItem(data);
                }

            });
        }
    }

    private void onEvent_button(GameObject go)
    {
        if (data.callObj != null)
        {
            data.callObj(data);
        }
    }

    public override void initItem(GridItem_Data data0)
    {
        data = (MailInfoItem_Data)data0;
        data.item = this;
        System.DateTime time = System.DateTime.UtcNow.AddTicks(data.mailData.createTime);
        timeLabel.text = time.ToString("yyyy/MM/dd hh:mm:ss");
        //data.ranker.avatar;//头像
        if (data_mailDef.datasDic.ContainsKey(data.mailData.mailID))
        {
            nameLabel.text = data_mailDef.datasDic[data.mailData.mailID].title;
            if (data.mailData.hasReceived == 1)
            {
                lingleLabel.gameObject.SetActive(true);
                butling.gameObject.SetActive(false);
            }
            else
            {
                lingleLabel.gameObject.SetActive(false);
                butling.gameObject.SetActive(true);
            }

            List<Reward> rblist = new List<Reward>();
            for (int n = 0; n < data.mailData.attachment.Length; n++)
            {
                List<Reward> rewards = Reward.decodeList(data.mailData.attachment[n]);
                rblist.AddRange(rewards);
            }

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
    }

    public void setSelect(bool v)
    {
        data.selected = v;

    }
}
