using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNameTip : DialogMonoBehaviour
{
    public static ChangeNameTip instance;
    public UILabel labcost;
    public UIInput inpName;
    public GameObject closebut;
    public GameObject btnConfirm;
    void Awake()
    {
        instance = this;
        setClickOutZoneClose(false);
        SoundEventer.add_But_ClickSound(closebut);
        UIEventListener.Get(closebut).onClick = closeDialog;

        SoundEventer.add_But_ClickSound(btnConfirm);
        UIEventListener.Get(btnConfirm).onClick = onbtnConfirmClick;

    }

    private void Start()
    {
         string userName = GameGlobal.gamedata.userinfo.name;
        if (string.IsNullOrEmpty(userName))
        {
            inpName.value  = "游客"+HttpManager.playerid;
        }
        else
        {
            inpName.value = userName;
        }
        labcost.text = "0";
    }

    private void onbtnConfirmClick(GameObject go)
    {
        string changeName = inpName.value;

        if (SensitiveWords.containIllegalWord(changeName))
        {
            UIManager.showToast("包含敏感字，请重新输入。");
            return;
        }


        HttpManager.instance.changeName(changeName, (ca) => { GameGlobal.gamedata.act_userDataChanged?.Invoke(); });
        closeDialog(closebut);
    }
}
