using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightWinDlg : DialogMonoBehaviour
{
    public GameObject butgen, texgen;
    public GameObject go_btnRewardCommget;
    public GameObject go_btnRewardAdget;
    public GameObject go_btnRewardCommget0;
    public RewardItem[] rewards = new RewardItem[3];
    // Start is called before the first frame update
    void Start()
    {
        setClickOutZoneClose(false);
        setCloseCallback((int code) =>
        {
            if (FightControl.fbtype == 0)
            {
                GameGlobal.gamedata.stageindex++;
            }
            if (!isGetBtnClick)
            {
                OnCommGetBtnClick(null);
            }
            GameGlobal.gamedata.CheckIsSomeOpen();
            GameGlobal.enterMenuScene(true);
        });
        SoundPlayer.getInstance().PlayW("Victory", false);
        UIEventListener.Get(go_btnRewardCommget).onClick = OnCommGetBtnClick;
        UIEventListener.Get(go_btnRewardCommget0).onClick = OnCommGetBtnClick;
        UIEventListener.Get(go_btnRewardAdget).onClick = OnAdGetBtnClick;
        // setData();

        NGUITools.DestroyChildren(texgen.transform);
        GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
        APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
        temppaintnode.create1(texgen, "shengLi-ShiBai", "shengLi-ShiBai");
        temppaintnode.transform.localPosition = Vector3.zero;
        //temppaintnode.transform.localScale = Vector3.one * 0.15f;
        temppaintnode.setdepth(21);
        temppaintnode.playAction2Auto("shengLi1", false, () => { temppaintnode.playAction2Auto("shengLi4", true); });
    }

    bool isGetBtnClick = false;
    private void OnAdGetBtnClick(GameObject go)
    {
        isGetBtnClick = true;
        string adcode = "stageRewards";
        if (fightType == 1)
        {
            adcode = "worldbossRewards";
        }
        //SDKManager.instance.ShowRewardsAD(adcode, (callint) =>
        //{
        //    if (callint == Callback.SUCCESS)
        //    {
                getRewards(1);
        //    }
        //});
    }

    private void OnCommGetBtnClick(GameObject go)
    {
        isGetBtnClick = true;
        getRewards();
    }

    void getRewards(int getType = 0)
    {
        string ADIdstr = "";
        if (getType == 1)
        {
            ADIdstr = GameGlobal.gamedata.curADServerId;
        }
        if (fightType == 5) //天启(世界boss)
        {
            int id = GameGlobal.gamedata.apocalypseID;
            HttpManager.instance.GetWorldBossRwards(id, getType, ADIdstr, (callint) =>
            {
                if (callint == Callback.SUCCESS)
                {
                    onGetRwardBack(getType);
                }
            });
        }
        else if (fightType == 0 || fightType == 6) //关卡或引导
        {
            HttpManager.instance.GetFightRwards(getType, ADIdstr, (callint) =>
            {
                if (callint == Callback.SUCCESS)
                {
                    onGetRwardBack(getType);
                }
            });
        }
        else if (fightType == 1)//妖怪入侵
        {
            HttpManager.instance.sendMonsterReceive(FightControl.monster.index, (code1) =>
            {
                if (code1 == Callback.SUCCESS)
                {
                    onGetRwardBack(getType);
                }
            });
        }
        else if (fightType == 4)//竞技场//无广告                                                                                                   

        {
            onGetRwardBack(0);
        }
    }

    void onGetRwardBack(int getType)
    {
        HttpManager.instance.sendUserInfo(null);
        foreach (var item in rewardslist)
        {
            if (getType == 1)
            {
                item.val += item.val;
            }

        }
        this.closeDialog(this.gameObject);
        UIManager.showAwardDlg(rewardslist);
    }
    void OnDestroy()
    {
        SoundPlayer.getInstance().Stop();
    }

    private void showbut()
    {
        butgen.SetActive(true);
        setBtnsState();
    }

    void setBtnsState()
    {
        if (fightType == 0 || fightType == 6) //关卡/引导
        {
            int curindex = GameGlobal.gamedata.stageindex + 1;
            //if (curindex > 5 && curindex % 5 == 0)
            //{
            //    go_btnRewardCommget.SetActive(true);
            //    go_btnRewardAdget.SetActive(true);
            //    go_btnRewardCommget0.SetActive(false);
            //}
            //else
            {
                go_btnRewardCommget.SetActive(false);
                go_btnRewardAdget.SetActive(false);
                go_btnRewardCommget0.SetActive(true);
            }
        }
        else if (fightType == 4) //竞技场
        {
            go_btnRewardCommget.SetActive(false);
            go_btnRewardAdget.SetActive(false);
            go_btnRewardCommget0.SetActive(true);
        }
        else //1 妖怪入侵 5 世界boss
        {
            go_btnRewardCommget.SetActive(false);
            go_btnRewardAdget.SetActive(false);
            go_btnRewardCommget0.SetActive(true);
        }
    }
    List<Reward> rewardslist;
    int fightType = 0;
    public void setData(List<Reward> rblist, int figtype)
    {
        fightType = figtype;
        if (rblist != null)
        {
            rewardslist = rblist;
            for (int i = 0; i < rewards.Length; i++)
            {
                if (i < rblist.Count)
                {
                    rewards[i].gameObject.SetActive(true);
                    rewards[i].setData(rblist[i], null);
                    // GameGlobal.gamedata.addCurrendy(rblist[i].mainType, rblist[i].val);
                }
                else
                {
                    rewards[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                rewards[i].gameObject.SetActive(false);
            }
        }


        Invoke("showbut", 0.5f);
    }
}
