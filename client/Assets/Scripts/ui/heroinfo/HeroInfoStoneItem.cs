using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInfoStoneItem : MonoBehaviour
{
    public GameObject suonode;
    public GameObject icongen;
    public GameObject selectnode;
    public UILabel namelabel, selectnamelabel;
    public TexPaintNode paintnode;
    public HeroStoneData stoneData;
    public Action<HeroInfoStoneItem> onItemClickCall;
    void Start()
    {
        UIEventListener.Get(this.gameObject).onClick = onItemClick;
    }
   
     void onItemClick(GameObject go)
    {
        //SetSelect(true);
        onItemClickCall?.Invoke(this);
    }

    public void SetSelect(bool isSelect)
    {
        selectnode.SetActive(isSelect);
    }
}
