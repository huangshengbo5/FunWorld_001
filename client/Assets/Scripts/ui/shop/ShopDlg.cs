using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDlg : DialogMonoBehaviour
{
    public GameObject closebtn;
    public GridAdapter grid;
    public GameObject itemgo;
    private void Awake()
    {
        setShowAnim(true);
        setClickOutZoneClose(false);
        UIEventListener.Get(closebtn).onClick = closeDialog;
        SoundEventer.add_But_ClickSound(closebtn);
    }

    private void Start()
    {
        this.initItemlist();
    }

    void initItemlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();

        for (int i = 0; i < data_storeDef.datas.Count; i++)
        {
            ShopItem_Data temp = new ShopItem_Data();
            temp.dbbean = data_storeDef.datas[i];
            temp.index = i;
            itemDatas.Add(temp);
        }

        grid.clear();
        grid.setListData(itemDatas, itemgo, GRID_RUNTYPE.GRID_TYPE_READLOAD);
    }
}
