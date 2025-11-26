using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TianqiMatchingDlg : DialogMonoBehaviour
{
    public static TianqiMatchingDlg instance;

    public UILabel titleName;//标题
    public UILabel playerNum;//角色数量
    public UISprite close;//关闭按钮

    public PlayerItem[] players;

    private float time;
    private bool isReadPlayer1 = false;//第一个人是否准备好
    private bool isReadPlayer2 = false;//第二个人是否准备好
    private bool isReadPlayer3 = false;//第三个人是否准备好

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        time = 0f;
        isReadPlayer1 = false;
        isReadPlayer2 = false;
        isReadPlayer2 = false;
        titleName.text = "匹配中";
        playerNum.text = "1/3";
        close.gameObject.SetActive(false);
        UIEventListener.Get(close.gameObject).onClick = closeDialog;
        SoundEventer.add_But_ClickSound(close.gameObject);
        initPlayerItem();
    }

    public void initPalyers(List<Player> playersdata)
    {
        for (int i = 0; i < playersdata.Count; i++)
        {
            players[i].setdata(playersdata[i]);
        }
    }

    private void initPlayerItem()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (i > 0)
            {
                players[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (!isReadPlayer1 && time > 1f)
        {
            isReadPlayer1 = true;
            playerNum.text = "1/3";
            //显示1号
            if (players.Length > 1)
            {
                players[1].gameObject.SetActive(true);
            }
        }
        if (!isReadPlayer2 && time > 2f)
        {
            isReadPlayer2 = true;
            playerNum.text = "2/3";
            //显示2号
            if (players.Length > 2)
            {
                players[2].gameObject.SetActive(true);
            }
        }
        if (!isReadPlayer3 && time > 3f)
        {
            isReadPlayer3 = true;
            playerNum.text = "3/3";
            //显示2号
            if (players.Length > 3)
            {
                players[3].gameObject.SetActive(true);
            }
        }

        if (time > 3.5f)
        {
            closeDialog(null);//关闭并且跳转战斗
            GameGlobal.fromTianqiFight = true;
            GameGlobal.gamedata.tianqiReportIndex = 0;
            GameGlobal.enterFightScene();
        }
    }
}
