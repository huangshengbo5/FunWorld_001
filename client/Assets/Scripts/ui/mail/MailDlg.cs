using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailDlg : DialogMonoBehaviour
{
    public static MailDlg instance;

    public UILabel titleLable;//标题
    public UISprite closeButton;//关闭按钮
    public UISprite allGetButton;//一键领取
    public UISprite alldelButton;//一键领取
    public UILabel kongtitle;//空的时候显示
    public GridAdapter grid;//格子
    public GameObject itemObject;

    private List<MailInfoItem_Data> itemdatalist = new List<MailInfoItem_Data>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(closeButton.gameObject).onClick = closeDialog;
        UIEventListener.Get(allGetButton.gameObject).onClick = onAllGet;
        UIEventListener.Get(alldelButton.gameObject).onClick = onAlldel;
        SoundEventer.add_But_ClickSound(closeButton.gameObject);
        SoundEventer.add_But_ClickSound(allGetButton.gameObject);

        initlist();
    }

    private void onAllGet(GameObject go)
    {
        HttpManager.instance.MailReceiveAll((code1) =>
        {
            if (code1 == Callback.SUCCESS)
            {
                HttpManager.instance.MailList((code2) =>
                {
                    if (code2 == Callback.SUCCESS)
                    {
                        initlist();
                    }

                });
            }

        });
    }
    private void onAlldel(GameObject go)
    {
        HttpManager.instance.MailRemoveAll((code1) =>
        {
            if (code1 == Callback.SUCCESS)
            {
                HttpManager.instance.MailList((code2) =>
                {
                    if (code2 == Callback.SUCCESS)
                    {
                        initlist();
                    }

                });
            }

        });
    }

    public void initlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();
        itemdatalist.Clear();
        if (GameGlobal.gamedata.mails.Count > 0)
        {
            kongtitle.gameObject.SetActive(false);
        }
        else
        {
            kongtitle.gameObject.SetActive(true);
        }

        bool isCanGetAll = false;
        for (int i = 0; i < GameGlobal.gamedata.mails.Count; i++)
        {
            MailInfoItem_Data temp = new MailInfoItem_Data();
            temp.mailData = GameGlobal.gamedata.mails[i];
            temp.index = i;
            temp.callObj = (object backObj) =>
            {
                setselect(temp);
            };
            itemDatas.Add(temp);
            itemdatalist.Add(temp);
            if (temp.mailData.hasReceived == 0)
            {
                isCanGetAll = true;
            }
        }
        grid.clear();
        grid.setListData(itemDatas, itemObject, GRID_RUNTYPE.GRID_TYPE_READLOAD);
        if (isCanGetAll)
        {
            allGetButton.gameObject.SetActive(true);
            alldelButton.gameObject.SetActive(false);
        }
        else
        {
            allGetButton.gameObject.SetActive(false);
            alldelButton.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setselect(MailInfoItem_Data temp)
    {
        //显示信息界面
        UIManager.showMailMsgInfo(temp.mailData);
        if (temp.mailData.status == 0)
        {
            HttpManager.instance.MailRead(temp.mailData.id, (code1) =>
            {
                if (code1 == Callback.SUCCESS)
                {
                    initlist();
                }

            });
        }
    }
}
