using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuildingInfoItem_Data : GridItem_Data
{
    public Building bean;
    public int index;
    public bool selected = false;
    public callbackObj callObj;
    public BuildingInfoItem item;

}
public class BuildingInfoItem : GridItem
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

    //void Start()
    //{

    //}

    BuildingInfoItem_Data data;
    public override void initItem(GridItem_Data data0)
    {
        data = (BuildingInfoItem_Data)data0;
        data.item = this;

        data_buildingBean dbbean = data_buildingDef.getdatabybuildid(data.bean.buildingID);

        Debug.LogError(data.bean.buildingID);
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(icongen, "jianzhutou");
            temppaintnode.setdepth(411);
            temppaintnode.playAction(0,dbbean.id - Statics.BUILDID);
        }
        selectkuang.SetActive(data.selected);
    }

    public void setSelect(bool v)
    {
        data.selected = v;
        selectkuang.SetActive(data.selected);
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
