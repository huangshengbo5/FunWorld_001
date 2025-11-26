using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipInfoItem_Data : GridItem_Data
{
    public Equip bean;
    public data_equipBean dbbean;
    public int index;
    public bool selected = false;
    public callbackObj callObj;
    public EquipInfoItem item;

}
public class EquipInfoItem : GridItem
{
    public GameObject selectkuang;
    public GameObject icongen;
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
    
    EquipInfoItem_Data data;
    public override void initItem(GridItem_Data data0)
    {
        data = (EquipInfoItem_Data)data0;
        data.item = this;

        //Debug.LogError(data.bean.buildingID);
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(icongen, "equip");
            temppaintnode.setdepth(411);
            // temppaintnode.playAction(0, data.dbbean.id - Statics.EQUIPID);
            temppaintnode.setShowRectLimit(150);
            temppaintnode.playAction(data.dbbean.icon);

            if (data.bean == null)
            {
                temppaintnode.sethui();
            }
            else
            {
                temppaintnode.SetNormal();
            }
        }


        selectkuang.SetActive(false);
    }

    public void setSelect(bool v)
    {
        data.selected = v;
        selectkuang.SetActive(data.selected);
    }
}
