using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoUI : DialogMonoBehaviour
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
        this.initBuildinglist();
    }

    void initBuildinglist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();

        for (int i = 0; i < GameGlobal.gamedata.buildlist.Count; i++)
        {
            data_buildingBean tempdbbean = data_buildingDef.getdatabybuildid(GameGlobal.gamedata.buildlist[i].buildingID);
            if (tempdbbean.type == 1)
            {
                continue;
            }
            else if (tempdbbean.condition_type == 1 && tempdbbean.condition_val > GameGlobal.gamedata.stageindex)
            {
                continue;
            }
            BuildingInfoUIItem_Data temp = new BuildingInfoUIItem_Data();
            temp.bean = GameGlobal.gamedata.buildlist[i];
            temp.index = i;
            temp.callObj = (object backObj) =>
            {
                //setselect(temp);
            };
            itemDatas.Add(temp);
        }
        grid.clear();
        grid.setListData(itemDatas, itemgo, GRID_RUNTYPE.GRID_TYPE_READLOAD);

        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow,(int)GuideAcitveWindow.JiuGuan);
        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow, (int)GuideAcitveWindow.Mofang);
    }
}
