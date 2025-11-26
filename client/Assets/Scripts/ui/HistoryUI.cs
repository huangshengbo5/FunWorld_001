using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryUI : DialogMonoBehaviour
{
    public GameObject closebtn;
    private void Awake()
    {
        setShowAnim(true);
        setClickOutZoneClose(true);
        UIEventListener.Get(closebtn).onClick = closeDialog;
    }
    public HistoryItem[] contentlist = new HistoryItem[2];
    List<data_annalsBean> dataList;
    public GridAdapter grid;
    public GameObject goItem;
    public int pageIndex = 0;
    private void Start()
    {
        dataList = data_annalsDef.datas;
        dataList.Sort((a, b) => a.id.CompareTo(b.id));
        initlist();
    }

    public void initlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();
        for (int i = 0; i < dataList.Count; i++)
        {
            HistoryItem_Data temp = new HistoryItem_Data();
            temp.dbean = dataList[i];
            temp.index = i;
            itemDatas.Add(temp);
        }
        grid.clear();
        grid.setListData(itemDatas, goItem, GRID_RUNTYPE.GRID_TYPE_READLOAD);
    }
}
