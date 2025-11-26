using System.Collections.Generic;
using UnityEngine;
public class TreasureselectUI : DialogMonoBehaviour
{
    public GridAdapter grid;
    public GameObject itemObject;
    public GameObject selectkuang;
    public int selectPos;

    public GameObject closebtn;
    private void Awake()
    {
        setShowAnim(true);
        setClickOutZoneClose(true);
        UIEventListener.Get(closebtn).onClick = closeDialog;
        SoundEventer.add_But_ClickSound(closebtn);

        setlist();
    }

    private void Start()
    {
        GameGlobal.gamedata.UpdataUserPowerDataAct += setlist;

        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow, (int)GuideAcitveWindow.Baowu);
    }

    List<EquipInfoItem_Data> itemdatalist = new List<EquipInfoItem_Data>();
    public void setlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();
        itemdatalist.Clear();

        for (int i = 0; i < GameGlobal.gamedata.equiplist.Count; i++)
        {
            var data = GameGlobal.gamedata.equiplist[i];
            if (data.pos > 0 )
            {
                continue;
            }
            EquipInfoItem_Data temp = new EquipInfoItem_Data();
            temp.bean = data;
            for (int j = 0; j < data_equipDef.datas.Count; j++)
            {
                if (data_equipDef.datas[j].id == temp.bean.equipID)
                {
                    temp.dbbean = data_equipDef.datas[j];
                    temp.index = i;
                    temp.callObj = (object backObj) =>
                    {
                        setselect(temp);
                    };
                    itemDatas.Add(temp);
                    itemdatalist.Add(temp);
                }
            }
        }
        grid.clear();
        grid.setListData(itemDatas, itemObject, GRID_RUNTYPE.GRID_TYPE_NONE);
    }

    public EquipInfoItem_Data selectdata;
    private void setselect(EquipInfoItem_Data temp)
    {
        selectdata = temp;
        selectkuang.SetActive(true);
        selectkuang.transform.localPosition = grid.transform.localPosition + temp.item.transform.localPosition +new Vector3(0,0,0);
        UIManager.showTreasureInfoTip(selectdata.bean, selectPos,2,()=> { closeDialog(closebtn); });
    }

    private void OnDestroy()
    {
        GameGlobal.gamedata.UpdataUserPowerDataAct -= setlist;
    }
}