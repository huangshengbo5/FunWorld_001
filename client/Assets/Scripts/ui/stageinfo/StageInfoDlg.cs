using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoDlg : DialogMonoBehaviour
{
    public static StageInfoDlg instance;
    public GameObject closebut;
    public GameObject touxianggen;
    TexPaintNode paintnode;
    public UILabel ttlabel, zhanlilabel,contentlab;
    public RewardItem[] rewards = new RewardItem[3];
    public GameObject gobut;
    void Awake()
    {
        instance = this;
        setShowAnim(true);
        setClickOutZoneClose(false);
        UIEventListener.Get(closebut).onClick = closeDialog0;

        UIEventListener.Get(gobut).onClick = onclickgo;
        SoundEventer.add_But_ClickSound(closebut);
        SoundEventer.add_But_ClickSound(gobut);

    }

    private void closeDialog0(GameObject go)
    {
        closeDialog(null);
        WorldLayer.instance.ppback();
    }


    private void onclickgo(GameObject go)
    {
        //{
        //    GameGlobal.gamedata.stageindex++;
        //    WorldLayer.instance.refresh();
        //    closeDialog(null);
        //}

        //判断是否有剧情
        if (dbbean !=null && dbbean.prebattledialogue_id > 0)
        {
            //有剧情
            UIManager.showDialogue(dbbean.prebattledialogue_id, _EnterBattle);
        }
        else
        {
            //没有直接进战斗
            _EnterBattle();
        }

        closeDialog(null);
    }

    private void _EnterBattle()
    {
        if (fighttype == 0)
        {
            GameGlobal.gamedata.stageId = dbbean.id;
            HttpManager.instance.sendFight(dbbean.id, (code1) =>
            {
                if (code1 == Callback.SUCCESS)
                {

                    GameGlobal.enterFightScene();

                }

            });
        }
        else
        {
            HttpManager.instance.sendMonsterFight(monster, (code1) =>
            {

                if (code1 == Callback.SUCCESS)
                {
                    GameGlobal.fromMonsterFight = true;
                    GameGlobal.enterFightScene();
                }

            });
        }
    }

    data_campaignBean dbbean;
    public int fighttype;
    Monster monster = null;
    public void setData(data_campaignBean selectdbbean, int type, Monster monster = null)
    {
       
        fighttype = type;
        this.monster = monster;

        if (fighttype == 0)
        {
            dbbean = selectdbbean;
            ttlabel.text = dbbean.name;
            zhanlilabel.text = dbbean.fighting_capacity + "";
            contentlab.text = "关卡奖励";
            data_npcBean npcbean = data_npcDef.getdatabynpcid(dbbean.npc_id);

            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(touxianggen, "huobantou");
            temppaintnode.setdepth(411);
            temppaintnode.setShowRectLimit(290);
            temppaintnode.playAction(npcbean.avatar);
            paintnode = temppaintnode;
            List<Reward> rblist = Reward.decodeList(dbbean.rewards);
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
        else
        {
            data_npcBean npcbean = data_npcDef.getdatabynpcid(data_towerDef.getdatabyid(monster.towerID).npc_id);
            ttlabel.text = npcbean.name;
            zhanlilabel.text = npcbean.fighting_capacity + "";
            contentlab.text = "获胜奖励";
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(touxianggen, "huobantou");
            temppaintnode.setdepth(411);
            temppaintnode.setShowRectLimit(290);
            temppaintnode.playAction(npcbean.avatar);
            paintnode = temppaintnode;
            string tempstring = data_towerDef.getdatabyid(monster.towerID).getrewardstr();
            List<Reward> rblist = Reward.decodeList(tempstring);

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
        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow,(int)GuideAcitveWindow.LevelAttack);
    }

}
