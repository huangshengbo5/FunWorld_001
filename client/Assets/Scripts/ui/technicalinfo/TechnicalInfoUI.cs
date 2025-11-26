using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnicalInfoUI : DialogMonoBehaviour
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

        for (int i = 0; i < data_scienceDef.datas.Count; i++)
        {
            TechnicalInfoUIItem_Data temp = new TechnicalInfoUIItem_Data();
            temp.dbbean = data_scienceDef.datas[i];
            temp.bean = GameGlobal.gamedata.gettechnicall(data_scienceDef.datas[i].science_id);
            temp.index = i;
            itemDatas.Add(temp);
        }

        //for (int i = 0; i < GameGlobal.gamedata.technicallist.Count; i++)
        //{
        //    //data_scienceBean tempdbbean = data_scienceDef.getdatabyTechid(GameGlobal.gamedata.technicallist[i].id);

        //    TechnicalInfoUIItem_Data temp = new TechnicalInfoUIItem_Data();
        //    temp.bean = GameGlobal.gamedata.technicallist[i];
        //    temp.dbbean = data_scienceDef.dicdatas[GameGlobal.gamedata.technicallist[i].id][0];
        //    temp.index = i;
        //    itemDatas.Add(temp);
        //}
        grid.clear();
        grid.setListData(itemDatas, itemgo, GRID_RUNTYPE.GRID_TYPE_READLOAD);
    }
}
