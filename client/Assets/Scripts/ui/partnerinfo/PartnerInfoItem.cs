using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PartnerInfoItem_Data : GridItem_Data
{
    public Hero bean;
    public int index;
    public bool selected = false;
    public callbackObj callObj;
    public PartnerInfoItem item;
   

}
public class PartnerInfoItem : GridItem
{
    public GameObject selectkuang;
    public GameObject icongen;
    public UISprite herobgsp;
    public UISprite herolocatypesp;
    void Awake()
    {
        UIEventListener.Get(gameObject).onClick = onEvent_button;
        SoundEventer.add_But_ClickSound(gameObject);
    }

    private void onEvent_button(GameObject go)
    {
        if (data.callObj != null)
        {
            data.callObj(data);
        }
    }
    PartnerInfoItem_Data data;
    GameObject texframeobjs = null;
    TexPaintNode temppaintnode = null;
    public override void initItem(GridItem_Data data0)
    {
        data = (PartnerInfoItem_Data)data0;
        data.item = this;
        data.id = data.bean.id;
        data_heroBean dbbean = data_heroDef.getdatabyheroid(data.bean.heroID);
        if (herolocatypesp != null)
        {
            herolocatypesp.spriteName = "hero_location_0" + dbbean.hero_location;
        }
        string headstr = dbbean.face_square;
        herobgsp.spriteName = "icon_bg";
        if (data.bean.level < 1)
        {
            headstr = dbbean.face_square+"_sb";
            herobgsp.spriteName = "icon_bg0";
        }
        if (temppaintnode == null)
        {
            texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(icongen, "huobantou");
            temppaintnode.setdepth(400);
            temppaintnode.setShowRectLimit(200);
        }
            temppaintnode.playAction(headstr);

       

        selectkuang.SetActive(data.selected);
    }

    public void setSelect(bool v)
    {
        data.selected = v;
        selectkuang.SetActive(data.selected);
    }
}
