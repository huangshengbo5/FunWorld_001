using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIcon : MonoBehaviour
{
    public UISprite jindutiao;
    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(gameObject).onClick = onclick;
    }

    private void onclick(GameObject go)
    {
        // UIManager.showToast(data_buff_comparisonDef.dicdatas[buff.buffid][0].buff_des);
        FightControl.instance.fui.settip(this, buff.buffid);
    }

    FightBuff buff;
    public void setdata(FightBuff buff)
    {
        this.buff = buff;
        {
            GameObject texframeobjs0 = ResManager.getGameObject("allpre", "vtexpaintnode");
            TexPaintNode temppaintnode = texframeobjs0.GetComponent<TexPaintNode>();
            string path = "buff";
            temppaintnode.create1(gameObject, path);
            temppaintnode.setdepth(250);
            temppaintnode.setShowRectLimit(50);
            temppaintnode.playAction(data_buff_comparisonDef.dicdatas[buff.buffid][0].buff_pic);
            temppaintnode.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        buff.time -= (int)(Time.deltaTime * 1000);
        jindutiao.fillAmount =1- buff.time*1.0f / buff.totaltime;
    }
}
