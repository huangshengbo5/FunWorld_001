using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingUI : DialogMonoBehaviour
{
    public static settingUI instance;
    public UISprite regon;
    public UILabel labName;
    public UILabel labID;
    public GameObject closebut,qiebut,tuibut;
    public GameObject btnChaneHeadIcon;
    public GameObject btnChangeName;
    public UIToggle toggle0, toggle1, toggle2;
    void Awake()
    {
        instance = this;
        setShowAnim(true);
        setClickOutZoneClose(false);
        SoundEventer.add_But_ClickSound(closebut);
        UIEventListener.Get(closebut).onClick = closeDialog;

        SoundEventer.add_But_ClickSound(btnChaneHeadIcon);
        UIEventListener.Get(btnChaneHeadIcon).onClick = onChangeHeadClick;
        SoundEventer.add_But_ClickSound(btnChangeName);
        UIEventListener.Get(btnChangeName).onClick = onChangeNameClick;

        GameGlobal.gamedata.act_userDataChanged += setData;


        UIEventListener.Get(qiebut).onClick = onClickqie;
        UIEventListener.Get(tuibut).onClick = onClicktui;


        EventDelegate.Add(toggle0.onChange, Toggle0);
        EventDelegate.Add(toggle1.onChange, Toggle1);
        EventDelegate.Add(toggle2.onChange, Toggle2);

    }

    private void onClicktui(GameObject go)
    {
        //string str = "是否退出游戏？";
        //UIManager.showTip(str, "确定", "取消", (int code) =>
        //{
        //    if (code == Tip.TIP_CONFIRM)
        //    {
                Application.Quit();
        //    }
      
        //}, null, "提示信息");
     
    }

    private void onClickqie(GameObject go)
    {
      

        //string str = "是否退出账号？";
        //UIManager.showTip(str, "确定", "取消", (int code) =>
        //{
        //    if (code == Tip.TIP_CONFIRM)
        //    {
                GameGlobal.enterCoverScene(false);
        //    }

        //}, null, "提示信息");
    }

    private void Toggle2()
    {
        GameGlobal.fireworksOn = UIToggle.current.value;
    }

    private void Toggle1()
    {
        bool val = UIToggle.current.value;

        GameGlobal.Snd_Switch = val;

        SoundPlayer.setPlayering(GameGlobal.Snd_Switch);
    }

    private void Toggle0()
    {
        bool val = UIToggle.current.value;

        GameGlobal.Bgm_Switch = val;


        MusicPlayer.setPlayering(GameGlobal.Bgm_Switch);

        if (val)
        {
            if (!MapManager.instance.world.gameObject.activeSelf)
            {
                MusicPlayer.getInstance().PlayW("City", true);
            }
            else
            {
                MusicPlayer.getInstance().PlayW("Map", true);
            }

        }
    }

    private void onChangeNameClick(GameObject go)
    {
        UIManager.showChangeNameTip();
    }

    private void onChangeHeadClick(GameObject go)
    {
        UIManager.showChangeHeadIconTip();
    }

  

    private void OnDestroy()
    {
        GameGlobal.gamedata.act_userDataChanged -= setData;
    }

    private void Start()
    {
        setData();
        toggle0.value = GameGlobal.Bgm_Switch;
        toggle1.value = GameGlobal.Snd_Switch;
        toggle2.value = GameGlobal.fireworksOn;
    }
    public void setData()
    {
        string userName = GameGlobal.gamedata.userinfo.name;
        if (string.IsNullOrEmpty(userName))
        {
            labName.text = "游客"+HttpManager.playerid;
        }
        else
        {
            labName.text = userName;
        }
        labID.text = GameGlobal.gamedata.userinfo.level.ToString();
        setHeadIcon();
    }

    TexPaintNode temppaintnode;
    void setHeadIcon()
    {
        if (!temppaintnode)
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(regon.gameObject, "huobantou");
            temppaintnode.setdepth(22);
            temppaintnode.setShowRectLimit(270, 270);
        }

        int headid = GameGlobal.gamedata.userinfo.avatar;
        string headIconName = data_avatarDef.dicdatas[headid][0].name;
        temppaintnode.playAction(headIconName);
    }
}
