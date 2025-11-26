using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHeadIconTip : DialogMonoBehaviour
{
    public static ChangeHeadIconTip instance;
    public GameObject closebut;
    HeadItem curitem ;
    public UIGrid gridList;
    public HeadItem[] headItems ;
    void Awake()
    {
        instance = this;
        setClickOutZoneClose(false);
        SoundEventer.add_But_ClickSound(closebut);
        UIEventListener.Get(closebut).onClick = closeDialog;
    }

    private void Start()
    {
        headItems = new HeadItem[gridList.transform.childCount];
        for (int i = 0; i < gridList.transform.childCount; i++)
        {
            headItems[i] = gridList.transform.GetChild(i).GetComponent<HeadItem>();
        }
        var datas = data_avatarDef.datas;
        for (int i = 0; i < datas.Count; i++)
        {
            headItems[i].initData(datas[i],onSelcetCall);
            if (GameGlobal.gamedata.userinfo.avatar == datas[i].id)
            {
                curitem = headItems[i];
                curitem.SetSelectState(true);
            }
        }
    }

    private void onSelcetCall(HeadItem selctItem)
    {
        if (curitem.dbean.id != selctItem.dbean.id)
        {
            curitem.SetSelectState(false);
            curitem = selctItem;
        }
    }

    public override void closeDilaogEvent()
    {
        base.closeDilaogEvent();
        HttpManager.instance.changeHeadIcon(curitem.dbean.id,(ca)=> { GameGlobal.gamedata.act_userDataChanged?.Invoke(); });
    }
}
