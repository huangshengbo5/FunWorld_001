using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaInfoItem_Data : GridItem_Data
{
    public ArenaRanker ranker;
    public int index;
    public bool selected = false;
    public callbackObj callObj;
    public PKItem item;
}


public class PKItem : GridItem
{
    public UISprite rankSpr1;//1
    public UISprite rankSpr2;//2
    public UISprite rankSpr3;//3
    public UISprite rankBg;//背景
    public UILabel rankLabel;//排名显示
    public UILabel nameLabel;//名字
    public UILabel zhanliLabel;//战力
    public UISprite changeButton;//挑战按钮

    void Awake()
    {
        UIEventListener.Get(changeButton.gameObject).onClick = onChangeClick;
        UIEventListener.Get(gameObject).onClick = onEvent_button;
        SoundEventer.add_But_ClickSound(gameObject);
        SoundEventer.add_But_ClickSound(changeButton.gameObject);

    }

    private void onChangeClick(GameObject go)
    {
        HttpManager.instance.ArenaAttack(data.ranker.id,
        (code1) =>
        {
            if (code1 == Callback.SUCCESS)
            {
                GameGlobal.fromArenaFight = true;
                GameGlobal.enterFightScene();
            }

        });
    }

    private void onEvent_button(GameObject go)
    {
        if (data.callObj != null)
        {
            data.callObj(data);
        }
    }
    ArenaInfoItem_Data data;
    public override void initItem(GridItem_Data data0)
    {
        data = (ArenaInfoItem_Data)data0;
        data.item = this;
        //data.ranker.avatar;//头像
        nameLabel.text = data.ranker.name;

        rankSpr1.gameObject.SetActive(false);
        rankSpr2.gameObject.SetActive(false);
        rankSpr3.gameObject.SetActive(false);
        rankBg.gameObject.SetActive(false);

        switch (data.ranker.rank)
        {
            case 1:
                rankSpr1.gameObject.SetActive(true);
                break;
            case 2:
                rankSpr2.gameObject.SetActive(true);
                break;
            case 3:
                rankSpr3.gameObject.SetActive(true);
                break;
            default:
                rankBg.gameObject.SetActive(true);
                rankLabel.text = data.ranker.rank.ToString();
                break;
        }
        zhanliLabel.text = data.ranker.fightingCapacity.ToString();
        if (data.ranker.id == GameGlobal.gamedata.self.id)
        {
            //如果是玩家自己
            changeButton.gameObject.SetActive(false);
        }
        else 
        {
            changeButton.gameObject.SetActive(true);
        }
    }

    public void setSelect(bool v)
    {
        data.selected = v;

    }
}
