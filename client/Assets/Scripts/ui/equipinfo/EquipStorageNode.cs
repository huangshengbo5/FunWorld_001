using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipStorageNode : MonoBehaviour
{
    public GridAdapter grid;
    public GameObject itemObject;
    // Start is called before the first frame update
    //void Start()
    //{
    //   // initlist();
    //}

    public void initlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();


        for (int i = 0; i < data_equipDef.datas.Count; i++)
        {
            EquipInfoItem_Data temp = new EquipInfoItem_Data();
            temp.bean = GameGlobal.gamedata.getEquip(data_equipDef.datas[i].id);
            temp.dbbean = data_equipDef.datas[i];
            temp.index = i;
            temp.callObj = (object backObj) =>
            {
                setselect(temp);
            };
            itemDatas.Add(temp);

        }
        grid.clear();
        grid.setListData(itemDatas, itemObject, GRID_RUNTYPE.GRID_TYPE_READLOAD);
    }

    private void setselect(EquipInfoItem_Data temp)
    {
        if (temp.bean != null)
        {
            UIManager.showTreasureInfoTip(temp.bean);
        }
        else
        {
            Equip eq = new Equip
            {
                equipID = temp.dbbean.id,
                level = 1,
            };
            UIManager.showTreasureInfoTip(eq);
        }
    }
}
