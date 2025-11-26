using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TansuoItem : MonoBehaviour
{
    public UILabel lab_title;
    public UILabel lab_increaseNum;
    public UILabel lab_totalNum;
    public UILabel lab_increaseTimes;
    public UILabel lab_leftTime;
    public UILabel lab_heroNum;

    public GameObject go_btnGet;
    public GameObject go_btnAddHero;
    public UILabel lab_btnAddHero;
    public explore data;

    public UISprite[] addPartnerSps = new UISprite[4];
    private void Awake()
    {
        UIEventListener.Get(go_btnGet).onClick = onGetBtnClick;
        UIEventListener.Get(go_btnAddHero).onClick = onAddHerobtnClick;
        SoundEventer.add_But_ClickSound(go_btnGet);
        SoundEventer.add_But_ClickSound(go_btnAddHero);
        GameGlobal.instance.SecondTimerCall += timercall;
    }

    private void OnDestroy()
    {
        GameGlobal.instance.SecondTimerCall -= timercall;
    }

    Action<explore> onAddpartnerCall;
    private void onAddHerobtnClick(GameObject go)
    {
        onAddpartnerCall?.Invoke(data);
    }

    double extra = 0;
    void setAddedPartners()
    {
        for (int i = 0; i < 4; i++)
        {
            string spName = "icon_bg0";
            GameObject texframeobjs = null;
            TexPaintNode temppaintnode = null;
            if (addPartnerSps[i].transform.childCount > 0)
            {
                texframeobjs = addPartnerSps[i].transform.GetChild(0).gameObject;
                temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            }
            if (i < data.herodbIDs.Length)
            {
                spName = "icon_bg";
                data_heroBean dbbean = data_heroDef.getdatabyheroid(data.herodbIDs[i]);
                string headstr = dbbean.face_square;
                if (texframeobjs == null)
                {
                    texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
                    texframeobjs.name = "icon";
                    temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
                    temppaintnode.create1(addPartnerSps[i].gameObject, "huobantou");
                    temppaintnode.setdepth(100);
                    temppaintnode.setShowRectLimit(addPartnerSps[i].width, addPartnerSps[i].height);
                }else
                {
                    texframeobjs.SetActive(true);
                }
                temppaintnode.playAction(headstr);
            }else
            {
                texframeobjs?.SetActive(false);
            }
            addPartnerSps[i].spriteName = spName;
        }
        if (data.heroIDs.Length > 0)
        {
            lab_btnAddHero.text = "更换英雄";
        }
        else
        {
            lab_btnAddHero.text = "派遣英雄";
        }
        lab_heroNum.text = data.herodbIDs.Length + "/4";
       
    }

    Action<explore> onGetRewardsCall;
    private void onGetBtnClick(GameObject go)
    {
        if (data.heroIDs.Length > 0)
        {
            onGetRewardsCall?.Invoke(data);
        }
        else
        {
            UIManager.showToast("需先派遣英雄！");
        }
    }

    public void InitData(explore _data, Action<explore> onGetRewardsCal, Action<explore> onAddpartnerCal)
    {
        setData(_data);
        if (onGetRewardsCal != null)
        {
            this.onGetRewardsCall = onGetRewardsCal;
        }
        if (onAddpartnerCal != null)
        {
            this.onAddpartnerCall = onAddpartnerCal;
        }
    }

    public void setData(explore _data)
    {
        data = _data;
        data.db = data_exploreDef.dicdatas[data.exploreID][0];
        Reward reward = Reward.decode(data.db.rewards);
        lab_title.text = data_resourcesDef.dicdatas[reward.mainType][0].resources_name;
        increasnum = reward.val;
        refreshUI();
    }

    public void refreshUI()
    {
        extra = data.mul;
        setTime();
        setBaseInfo();
        setAddedPartners();
    }

    int leftTime = 0;
    int pertime = 5;
    void timercall()
    {
        if (pertime > 0)
        {
            pertime -= 1;
            if (pertime <= 0)
            {
                pertime = 5;
                setTotalnum();
            }
        }
        if (leftTime > 0)
        {
            setLeftTime();
        }
    }
    void setLeftTime()
    {
        leftTime -= 1;
        if (leftTime > 0)
        {
            int h = leftTime / 3600;
            int m = (leftTime - h * 3600) / 60;
            int s = leftTime - h * 3600 - m * 60;
            lab_leftTime.text = h + ":" + m + ":" + s;
        }
        else
        {
            pertime = 0;
            lab_leftTime.text = "0:0:0";
        }
    }

    int intervaltime = 5;
    int passedtime = 0;
    void setTime()
    {
        int leftt = data.db.duration - ((int)(HttpManager.currentTimeMillis() / 1000) - data.startTime);
        if (leftt > 0)
        {
            leftTime = leftt;
            passedtime = data.db.duration - leftTime;
            pertime = intervaltime;
        }
        else
        {
            if (data.heroIDs.Length > 0)
            {
                passedtime = data.db.duration;
            }else
            {
                passedtime = 0;
            }
           
            leftTime = 0;
            pertime = 0;
        }
    }

    long increasnum = 0;
    int creatednul = 0;
    void setTotalnum()
    {
        creatednul += (int)(extra  * increasnum);
        lab_totalNum.text = creatednul.ToString();
    }
    void setBaseInfo()
    {
       
        creatednul = (int)(extra * increasnum * (passedtime/5));
        lab_increaseNum.text = increasnum + "/5秒";
        lab_increaseTimes.text = (int)(data.mul * 100) + "%";
        lab_totalNum.text = creatednul.ToString();
    }
}
