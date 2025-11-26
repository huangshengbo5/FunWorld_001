using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TianqiActivityDlg : DialogMonoBehaviour
{
    public static TianqiActivityDlg instance;

    public UILabel tianqiTitle;//标题
    public UILabel bossName;//boss的名称
    public UILabel timelabel;//时间
    public UILabel remainNum;//剩余次数

    public RewardItem[] rewards = new RewardItem[4];

    public UISprite leftBut;//左边的按钮
    public UISprite rightBut;//右边的按钮
    public UISprite enterBtn;//战斗
    public UISprite backBtn;//退出
    public GameObject go_AdEnterbtn;

    public UILabel levelTitle;//困难等级
    public UISprite leftLevel;//选择左边
    public UISprite rightLevel;//选择右边
    public UISprite helpBut;//帮助
    public GameObject bossRoot;//怪物root

    public APaintNodeSpine renpaintnode;
    public TexPaintNode texpaintnode;

    private data_apocalypseBean _apocalypseBean;//

    private void Awake()
    {
        instance = this;
        UIEventListener.Get(leftBut.gameObject).onClick = OnLeftClick;
        UIEventListener.Get(rightBut.gameObject).onClick = OnRightClick;
        UIEventListener.Get(enterBtn.gameObject).onClick = OnBattleMonster;
        UIEventListener.Get(go_AdEnterbtn).onClick = onADenterClick;
        UIEventListener.Get(backBtn.gameObject).onClick = closeDialog;

        UIEventListener.Get(leftLevel.gameObject).onClick = OnLevelLeft;
        UIEventListener.Get(rightLevel.gameObject).onClick = OnLevelRight;
        SoundEventer.add_But_ClickSound(leftBut.gameObject);
        SoundEventer.add_But_ClickSound(rightBut.gameObject);
        SoundEventer.add_But_ClickSound(enterBtn.gameObject);
        SoundEventer.add_But_ClickSound(go_AdEnterbtn);
        SoundEventer.add_But_ClickSound(backBtn.gameObject);
        SoundEventer.add_But_ClickSound(leftLevel.gameObject);
        SoundEventer.add_But_ClickSound(rightLevel.gameObject);

        GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
        APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
        temppaintnode.create1(gameObject, "xuanWo", "xuanWo");
        temppaintnode.transform.localPosition = Vector3.zero;
        //temppaintnode.transform.localScale = Vector3.one * 0.15f;
        temppaintnode.setdepth(1);
        temppaintnode.playAction2Auto("xunHuan", true, null);

    }


    //向左按钮
    private void OnLeftClick(GameObject go)
    {

    }

    //向右按钮
    private void OnRightClick(GameObject go)
    {

    }

    //攻击boss
    private void OnBattleMonster(GameObject go)
    {
        if (GameGlobal.gamedata.remainNum > 0)
        {
            enterAttackMacth();
        }
        else
        {
            UIManager.showToast("剩余可攻击次数不足");
        }
    }

    void enterAttackMacth(int matchtype = 0)
    {
        string ADIdstr = "";
        if (matchtype == 1)
        {
            ADIdstr = GameGlobal.gamedata.curADServerId;
        }
        HttpManager.instance.TianqiAttack(GameGlobal.gamedata.apocalypseID, matchtype, ADIdstr, (code1) =>
         {
             if (code1 == Callback.SUCCESS)
             {
                 UIManager.showTianqiMatching(GameGlobal.gamedata.players);
             }
         });
    }

    void onADenterClick(GameObject go)
    {
        if (GameGlobal.gamedata.remainNum > 0)
        {
            //SDKManager.instance.ShowRewardsAD("worldbossRefresh", (callint) =>
            //{
            //    if (callint == Callback.SUCCESS)
            //    {
                    enterAttackMacth(1);
            //    }
            //});
        }
        else
        {
            UIManager.showToast("剩余可攻击次数不足");
        }
    }


    //困难向左
    private void OnLevelLeft(GameObject go)
    {

    }

    //难度向右
    private void OnLevelRight(GameObject go)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        tianqiTitle.text = "天启入侵";
        remainNum.text = string.Format("{0}次", GameGlobal.gamedata.remainNum);

        initBoss();
        showRewards();

        if (GameGlobal.gamedata.remainNum == 3 || GameGlobal.gamedata.remainNum <= 0)
        {
            enterBtn.gameObject.SetActive(true);
            go_AdEnterbtn.SetActive(false);
            enterBtn.spriteName = "btm_dalan";
            if (GameGlobal.gamedata.remainNum <= 0)
            {
                enterBtn.spriteName = "btn_hui";
            }
        }
        else
        {
            enterBtn.gameObject.SetActive(false);
            go_AdEnterbtn.SetActive(true);
        }
    }

    //初始化boss信息
    private void initBoss()
    {
        int id = GameGlobal.gamedata.apocalypseID;
        if (data_apocalypseDef.datasDic.ContainsKey(id))
        {
            var bean = data_apocalypseDef.datasDic[id];
            _apocalypseBean = bean;
            var dbbean = data_npcDef.getdatabynpcid(bean.npc_id);
            if (dbbean == null)
            {
                Debug.Log("无法找到对应的boss");
            }
            if (renpaintnode == null)
            {
                GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
                texframeobjs.name = "" + bean.npc_id;
                APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
                temppaintnode.create1(bossRoot, dbbean.resource, dbbean.resource);
                temppaintnode.transform.localPosition = Vector3.zero;
                temppaintnode.transform.localScale = Vector3.one * 0.5f;
                temppaintnode.setdepth(11);
                temppaintnode.setAutoAlpha(true);
                renpaintnode = temppaintnode;
            }
            else
            {
                renpaintnode.create1(bossRoot, dbbean.resource, dbbean.resource);
                renpaintnode.transform.localPosition = Vector3.zero;
                renpaintnode.transform.localScale = Vector3.one * 0.5f;
                renpaintnode.setdepth(11);
                renpaintnode.setAutoAlpha(true);
            }
            renpaintnode.playAction2Auto("idle", true);
        }
    }

    private void showRewards()
    {
        if (_apocalypseBean != null)
        {
            var npcData = data_npcDef.getdatabynpcid(_apocalypseBean.npc_id);
            bossName.text = npcData.name;
        }

        List<Reward> rblist = Reward.decodeList(_apocalypseBean.rewards);

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
