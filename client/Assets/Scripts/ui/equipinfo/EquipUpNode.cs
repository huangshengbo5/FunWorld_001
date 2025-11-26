using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUpNode : MonoBehaviour
{
    public GridAdapter grid;
    public GameObject itemObject;
    public GameObject selectkuang;
    public GameObject gonull;

    public UILabel lastnameandlvlabel, lastdesclabel, nextnameandlvlabel, nextdesclabel;
    public GameObject shengbut, maxbut, shengbuthui;
    public RewardItem costitem;


    UISprite shengbutsp;
    BoxCollider shengbutcox;
    void Start()
    {
        shengbutsp = shengbut.GetComponent<UISprite>();
        shengbutcox = shengbut.GetComponent<BoxCollider>();
        UIEventListener.Get(shengbut).onClick = onclicklevelup;
        SoundEventer.add_But_ClickSound(shengbut);
        // initlist();
    }
    private void onclicklevelup(GameObject go)
    {
        if (selectdata.bean.level >= Statics.EQUIPLEVELMAX)
        {
            //UIManager.showToast("升级成功");
            return;
        }

        HttpManager.instance.sendEquipLevelUp(selectdata.bean, nextleveldbbean.getlvcost(), (int code) =>
        {
            refreshui();
            //UIManager.showToast("升级成功");
        });
    }
    List<EquipInfoItem_Data> itemdatalist = new List<EquipInfoItem_Data>();
    public void initlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();
        itemdatalist.Clear();

        for (int i = 0; i < GameGlobal.gamedata.equiplist.Count; i++)
        {
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
            //UIManager.showToast("当前未获得宝物！");
        }

    }
    public EquipInfoItem_Data selectdata;
    data_equip_upgradeBean nextleveldbbean;

    private void refreshui()
    {
        if (selectdata != null)
        {
            gonull.SetActive(false);
            shengbuthui.SetActive(false);
            if (selectdata.bean.level >= Statics.EQUIPLEVELMAX)
            {
                shengbut.SetActive(false);
                maxbut.SetActive(true);
                costitem.setReward(new Reward(4, 0, 0));
                lastnameandlvlabel.text = selectdata.dbbean.name + "  等级：" + selectdata.bean.level;
                nextnameandlvlabel.text = "已到达最高级";

                lastdesclabel.text = selectdata.bean.fightingCapacity.ToString();
                nextdesclabel.text = "已到达最高级";
            }
            else
            {
                shengbut.SetActive(true);
                maxbut.SetActive(false);
                nextleveldbbean = data_equip_upgradeDef.dicdatas[selectdata.bean.level + 1][0];
                var cost = nextleveldbbean.getlvcost();
                long have = GameGlobal.gamedata.getCurrendy(cost.mainType);
                if (have < cost.val)
                {
                    shengbuthui.SetActive(true);
                    shengbut.SetActive(false);
                }else
                {
                    shengbuthui.SetActive(false);
                }
                costitem.setReward(cost);

                lastnameandlvlabel.text = selectdata.dbbean.name + "  等级：" + selectdata.bean.level;
                nextnameandlvlabel.text = selectdata.dbbean.name + "  等级：" + (selectdata.bean.level + 1);

                int lastvl = selectdata.bean.fightingCapacity;
                int nextvl = lastvl + lastvl * nextleveldbbean.multiplier / 1000;
                lastdesclabel.text = "战力 +" + lastvl;
                nextdesclabel.text = "战力 +" + nextvl;
            }
        }
        else
        {
            gonull.SetActive(true);
            shengbuthui.SetActive(true);
            shengbut.SetActive(false);
            maxbut.SetActive(false);
            lastnameandlvlabel.text = "? ? ?";
            nextnameandlvlabel.text = "? ? ?";
            lastdesclabel.text = "? ? ?";
            nextdesclabel.text = "? ? ?";

            costitem.setReward(new Reward(4, 0, 0));
        }

    }

    private void setselect(EquipInfoItem_Data temp)
    {
        if (temp != null)
        {
            selectdata = temp;
            selectkuang.SetActive(true);
            selectkuang.transform.localPosition = grid.transform.localPosition + temp.item.transform.localPosition;
        }
        else
        {
            selectkuang.SetActive(false);
        }
        refreshui();
    }
}
