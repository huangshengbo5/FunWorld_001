using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadItem : MonoBehaviour
{
    public data_avatarBean dbean;
    public UISprite icongen;
    public GameObject goSelectmark;
    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(this.gameObject).onClick = onClicked;
    }

    private void onClicked(GameObject go)
    {
        SetSelectState(true);
    }

    Action<HeadItem> act_select;
    public void initData(data_avatarBean data,Action<HeadItem> onSelectCall)
    {
        dbean = data;
        act_select = onSelectCall;
        setHeadIcon();
    }
    TexPaintNode temppaintnode;
    void setHeadIcon()
    {
        if (!temppaintnode)
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(icongen.gameObject, "huobantou");
            temppaintnode.setdepth(22);
            temppaintnode.setShowRectLimit(270, 270);
        }
        temppaintnode.playAction(dbean.name);
    }

    public void SetSelectState(bool isSelect)
    {
        goSelectmark.SetActive(isSelect);
        if (isSelect)
        {
            act_select?.Invoke(this);
        }
    }


}
