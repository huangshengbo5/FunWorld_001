using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInfoGoodItem : MonoBehaviour
{
    public GameObject addnode;
    public UISprite addnodesp;
    public UISprite itembg;
    public TexPaintNode icon;
    int pos;
    Equip equipdata;
    // Start is called before the first frame update
    void Awake()
    {
        itembg = this.transform.GetComponent<UISprite>();
        addnodesp = addnode.GetComponent<UISprite>();
        UIEventListener.Get(this.gameObject).onClick = onEquipNodeClick;
        SoundEventer.add_But_ClickSound(gameObject);
    }

    int isunOpenState = 0;
    public void SetData(Equip eq, int _pos)
    {
        equipdata = eq;
        pos = _pos;
        isunOpenState = 0;
        if (eq != null)
        {
            addnode.SetActive(false);
            itembg.spriteName = "kuang_build_select";
            if (icon == null)
            {
                GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
                texframeobjs.name = "icon";
                TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
                string path = "equip";
                temppaintnode.create1(this.gameObject, path);
                temppaintnode.setdepth(402);
                temppaintnode.setShowRectLimit(itembg.width, itembg.height);
                icon = temppaintnode;
            }
            data_equipBean resoursbean = data_equipDef.dicdatas[equipdata.equipID][0];
            string pic = resoursbean.icon;
            icon.playAction(pic);
            icon.gameObject.SetActive(true);
        }
        else
        {
            itembg.spriteName = "kuang_zhuangbei";
            addnode.SetActive(true);
            addnodesp.spriteName = "icon_jiahao";
            if (pos <= 2 && GameGlobal.gamedata.stageindex < 7)
            {
                addnodesp.spriteName = "function_locked";
                isunOpenState = 7;
            }
            else if (pos > 2 && pos <= 4 && GameGlobal.gamedata.stageindex < 25)
            {
                addnodesp.spriteName = "function_locked";
                isunOpenState = 25;
            }
            else if (pos > 4 && pos <= 6 && GameGlobal.gamedata.stageindex < 60)
            {
                addnodesp.spriteName = "function_locked";
                isunOpenState = 60;
            }
            if (icon != null)
            {
                icon.gameObject.SetActive(false);
            }
        }
    }

    private void onEquipNodeClick(GameObject go)
    {
        if (isunOpenState > 0)
        {
            var names = data_campaignDef.dataDic[isunOpenState].name;
            UIManager.showToast("通关 " + names + " 关卡解锁！");
            return;
        }
        if (GameGlobal.gamedata.equiplist.Count == 0)
        {
            UIManager.showToast("未拥有装备！");
        }
        else if (equipdata == null)
        {
            UIManager.showEquipSelect(pos);
        }
        else
        {
            UIManager.showTreasureInfoTip(equipdata, pos, 1);
        }
    }

}
