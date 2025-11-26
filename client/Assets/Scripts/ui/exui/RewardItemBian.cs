using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardItemBian : MonoBehaviour
{
    public void setData(Reward rewardBean, callbackObj callObj)
    {
        this.callObj = callObj;
        setReward(rewardBean);
    }
    callbackObj callObj;
    public Reward bean;
    public UILabel countLabel;
    public UISprite icongen;
    public int type = 0;
    public void setReward(Reward bean)
    {
        this.bean = bean;

        if (type == 1)
        {
            countLabel.text = "x" + bean.val;
        }
        else
        {
            countLabel.text = bean.val + "";
        }

        if (bean.mainType == Statics.TYPE_diamond)
        {
            icongen.spriteName = "icon_zuan";
        }
        else if (bean.mainType == Statics.TYPE_gold)
        {
            icongen.spriteName = "icon_jinbi";
        }
        


    }

    public void settextcolor(string v)
    {
        countLabel.text = v + bean.val + "";
    }
    //public void sethui()
    //{
    //    if (icon != null)
    //    {
    //        icon.setShader(Shader.Find("Unlit/Transparent Colored hui"));
    //    }
    //    else
    //    {
    //        icongen.spriteName = icongen.spriteName + "_hui";
    //    }

    //}
}
