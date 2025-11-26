using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class evoleveNode : MonoBehaviour
{
    public GameObject go_btnUp;
  
    public GameObject go_maxmark;
    public RewardItem costItem;
    public UISlider progressslider;
    public UILabel lab_progress;
    public UILabel lab_Title;

    Action act_closeCall;
    Hero hero;
    data_hero_evolveBean hero_EvolveBean;
    UISprite go_btnUpsp;
    BoxCollider go_btnUpspcox;
    // Start is called before the first frame update
    void Start()
    {
        go_btnUpsp = go_btnUp.GetComponent<UISprite>();
        go_btnUpspcox = go_btnUp.GetComponent<BoxCollider>();
        UIEventListener.Get(go_btnUp).onClick = onLevelUpbtnClik;
    }

    private void onLevelUpbtnClik(GameObject go)
    {
        if (iscanup)
        {
            HttpManager.instance.sendHeroevolveUp(hero, new Reward(), (callInt) =>
        {
            if (callInt == Callback.SUCCESS)
            {
                //UIManager.showToast("赋能成功！");
                setUI();
            }
        });
        }
    }

    public void initData(Action nodeClosecall)
    {
        act_closeCall = nodeClosecall;
    }

    public void setData(PartnerInfoItem_Data partnerData)
    {
        hero = partnerData.bean;
        setUI();
    }

    bool iscanup = true;
    void setUI()
    {
        hero_EvolveBean = data_hero_evolveDef.dicdatas[hero.heroID][hero.evolveTimes];
        int curvl = (hero_EvolveBean.soul_level - 1) * 10;
        lab_Title.text = "赋能 " + (hero_EvolveBean.soul_grade - 1) + " 阶";

        int totalvl = getCurtotalvl(hero.heroID, hero_EvolveBean.soul_grade);
        lab_progress.text = GameGlobal.gamedata.GetNumStr(curvl) + "/" + GameGlobal.gamedata.GetNumStr(totalvl);
        progressslider.value = (float)curvl / totalvl;
        costItem.setReward(new Reward(hero_EvolveBean.cost_type, hero_EvolveBean.cost_sub_type, hero_EvolveBean.cost_val));
        iscanup = totalvl >= curvl;
        if (go_btnUpsp == null)
        {
            return;
        }
        if (iscanup)
        {
            go_btnUpsp.spriteName = "btm_shengji";
            go_btnUpspcox.enabled = true;
        }
        else
        {
            go_btnUpspcox.enabled = false;
            go_btnUpsp.spriteName = "btn_hui";
        }
    }

    int getCurtotalvl(int heroId, int soul_grade)
    {
        int vl = 0;
        var datas = data_hero_evolveDef.dicdatas[heroId];
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].soul_grade == soul_grade)
            {
                vl += 10;
            }
        }
        return vl;
    }
}
