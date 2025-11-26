using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailMsgDlg : DialogMonoBehaviour
{
    public static MailMsgDlg instance;

    public UILabel titleLable;//标题
    public UISprite closeButton;//关闭按钮
    public UILabel timeLable;//邮件时间
    public UILabel descLable;//描述内容
    public UISprite butredButton;//领取按钮
    public UILabel rewardLabel;//已领取显示
    public UILabel levellab;
    public UISprite regon;

    public RewardItem[] rewards = new RewardItem[4];

    private MailInfo _mailInfo;

    public void SetData(MailInfo mailInfo)
    {
        _mailInfo = mailInfo;
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(closeButton.gameObject).onClick = closeDialog;
        UIEventListener.Get(butredButton.gameObject).onClick = onButredButton;
        SoundEventer.add_But_ClickSound(closeButton.gameObject);
        SoundEventer.add_But_ClickSound(butredButton.gameObject);
        _ShowMailContent();
    }

    private void _ShowMailContent()
    {
        if (_mailInfo == null)
        {
            return;
        }
        System.DateTime time = System.DateTime.UtcNow.AddTicks(_mailInfo.createTime);
        timeLable.text = time.ToString("yyyy/MM/dd hh:mm:ss");
        if (data_mailDef.datasDic.ContainsKey(_mailInfo.mailID))
        {
            titleLable.text = data_mailDef.datasDic[_mailInfo.mailID].title;
            if (_mailInfo.hasReceived == 1)
            {
                rewardLabel.gameObject.SetActive(true);
                butredButton.gameObject.SetActive(false);
            }
            else
            {
                rewardLabel.gameObject.SetActive(false);
                butredButton.gameObject.SetActive(true);
            }
            if (_mailInfo.args != null)
            {
                descLable.text = string.Format(data_mailDef.datasDic[_mailInfo.mailID].content, _mailInfo.args);
            }
            else
            {
                descLable.text = data_mailDef.datasDic[_mailInfo.mailID].content;
            }

            List<Reward> rblist = new List<Reward>();
            for (int n = 0; n < _mailInfo.attachment.Length; n++)
            {
                List<Reward> rewards = Reward.decodeList(_mailInfo.attachment[n]);
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

    private void onButredButton(GameObject go)
    {
        if (_mailInfo.hasReceived == 0)
        {
            HttpManager.instance.MailReceive(_mailInfo.id, (code1) =>
            {
                if (code1 == Callback.SUCCESS)
                {
                    _ShowMailContent();
                }

            });
        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
