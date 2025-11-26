using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardItem : MonoBehaviour
{
    BoxCollider box;
    void Awake()
    {
        box = this.gameObject.GetComponent<BoxCollider>();
        if (box == null)
        {
            box = this.gameObject.AddComponent<BoxCollider>();
        }
        var widg = this.GetComponent<UIWidget>();
        box.size = new Vector3(widg.localSize.x, widg.localSize.y, 0);
        UIEventListener.Get(gameObject).onClick = onEvent_button;
        SoundEventer.add_But_ClickSound(gameObject);
    }

    private void onEvent_button(GameObject go)
    {
        if (callObj != null)
        {
            callObj(reward);
        }
    }
    public void setData(Reward rewardBean, callbackObj callObj)
    {
        this.callObj = callObj;
        setReward(rewardBean);
    }
    callbackObj callObj;
    public Reward reward;
    public UILabel countLabel;
    public UISprite icongen;
    TexPaintNode paintnode;
    public int type = 0;
    public void setReward(Reward bean)
    {
        this.reward = bean;

      
        string path = "item";
        string pic = "";
        data_resourcesBean resoursbean = data_resourcesDef.dicdatas[bean.mainType][0];
        if (type == 1)
        {
            countLabel.text = "x" + bean.val;
        }
        else if (type == 0)
        {
            countLabel.text = bean.val + "";
        }
        else if (type == 2)
        {
           long havenum =GameGlobal.gamedata.getCurrendy(bean.mainType);
            countLabel.text = GameGlobal.gamedata.GetNumStr(bean.val) + "/" + GameGlobal.gamedata.GetNumStr(havenum);
            countLabel.text = GameGlobal.gamedata.GetNumStr(bean.val) + "/" + GameGlobal.gamedata.GetNumStr(havenum);
        }
        pic = resoursbean.resources_picture;
        if (paintnode == null)
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(icongen.gameObject, path);
            temppaintnode.setdepth(402);
            temppaintnode.setShowRectLimit(icongen.width,icongen.height);
            paintnode = temppaintnode;
        }
        paintnode.playAction(pic);
        //paintnode.transform.localScale = new Vector3(0.8f, 0.8f, 0);
    }

    public void settextcolor(string v)
    {
        countLabel.text = v + reward.val + "";
    }
}
