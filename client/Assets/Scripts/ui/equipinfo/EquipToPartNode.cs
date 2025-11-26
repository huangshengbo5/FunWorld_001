using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToPartNode : MonoBehaviour
{
    public GridAdapter grid;
    public GameObject itemObject;
    public GameObject selectkuang;
    public GameObject gonull;

    public GameObject icongen;
    public RewardItem rewardItem;
    TexPaintNode paintnode;
    public UILabel nameandnumlabel;
    public GameObject fenjiebut;
    public GameObject fenjiebuthui;

    void Start()
    {
        UIEventListener.Get(fenjiebut).onClick = onclickPart;
        SoundEventer.add_But_ClickSound(fenjiebut);
        //  initlist();
    }
    private void onclickPart(GameObject go)
    {
        if (selectdatalist.Count > 0)
        {
            HttpManager.instance.sendEquipToPart(selectdatalist, (int code) =>
            {
                initlist();
                //UIManager.showToast("分解成功！");
            });
        }else
        {
            UIManager.showToast("没有可分解的宝物！");
        }
       
    }

    List<EquipInfoItem_Data> itemdatalist = new List<EquipInfoItem_Data>();
   
    public List<EquipInfoItem_Data> selectdatalist = new List<EquipInfoItem_Data>();
    public void initlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();
        itemdatalist.Clear();
        selectdatalist.Clear();
        for (int i = 0; i < GameGlobal.gamedata.equiplist.Count; i++)
        {
            var data = GameGlobal.gamedata.equiplist[i];
            if (data.pos > 0)
            {
                continue;
            }
            EquipInfoItem_Data temp = new EquipInfoItem_Data();
            temp.bean = GameGlobal.gamedata.equiplist[i];
            for (int j = 0; j < data_equipDef.datas.Count; j++)
            {
                if (data_equipDef.datas[j].id == temp.bean.equipID)
                {
                    temp.dbbean = data_equipDef.datas[j];
                }
            }
            temp.index = i;
            temp.callObj = (object backObj) =>
            {
                setselect(temp);
            };
            itemDatas.Add(temp);
            itemdatalist.Add(temp);
        }
        grid.clear();
        grid.setListData(itemDatas, itemObject, GRID_RUNTYPE.GRID_TYPE_READLOAD);

        if (itemdatalist.Count > 0)
        {
            setselect(itemdatalist[0]);
        }
        else
        {
            setselect(null);
            //UIManager.showToast("没有可分解的宝物！");
        }
    }



    private void refreshui()
    {
        Reward partrb = new Reward(4,0,0);
        if (selectdatalist.Count > 0)
        {
            gonull.SetActive(false);
            fenjiebuthui.SetActive(false);
            fenjiebut.SetActive(true);
            var data0 = data_equip_upgradeDef.dicdatas[selectdatalist[0].bean.level][0];
            partrb = Reward.decode(data0.rewards);
            if (selectdatalist.Count > 1)
            {
                for (int i = 1; i < selectdatalist.Count; i++)
                {
                    Reward partrbtemp = Reward.decode(data_equip_upgradeDef.dicdatas[selectdatalist[i].bean.level][0].rewards);
                    partrb.val += partrbtemp.val;

                }
            }
           
        }
        else
        {
            gonull.SetActive(true);
            fenjiebuthui.SetActive(true);
            fenjiebut.SetActive(false);
        }
        rewardItem.setReward(partrb);
    }

    private void setselect(EquipInfoItem_Data temp)
    {
        if (temp != null)
        {
            bool isHave = false;
            for (int i = 0; i < selectdatalist.Count; i++)
            {
                if (selectdatalist[i].bean.id == temp.bean.id)
                {
                    isHave = true;
                    break;
                }
            }

            if (isHave)
            {
                if (selectdatalist.Count > 1)
                {
                    selectdatalist.Remove(temp);
                    temp.item.setSelect(false);
                }
            }
            else
            {
                selectdatalist.Add(temp);
                temp.item.setSelect(true);
            }
        }
        refreshui();
    }
}
