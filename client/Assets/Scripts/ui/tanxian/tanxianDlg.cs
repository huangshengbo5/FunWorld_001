using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tanxianDlg : DialogMonoBehaviour
{
    public static tanxianDlg instance;

    public UISprite close;

    public List<TansuoItem> itemlist = new List<TansuoItem>(3);

    public GameObject parterSelectNode;

    /// <summary>
    /// /selectNode
    /// </summary>


    public void Awake()
    {
        instance = this;
        UIEventListener.Get(close.gameObject).onClick = closeDialog;

        UIEventListener.Get(gobtn_closeSelect).onClick = (go) => { closeSelectTip(); };
        SoundEventer.add_But_ClickSound(gobtn_closeSelect);
        UIEventListener.Get(goSelecteConfirmbtn).onClick = onSelectHeroConfirmClick;
        SoundEventer.add_But_ClickSound(goSelecteConfirmbtn);
        parterSelectNode.SetActive(false);
    }

    GetTianxianInfoResponse curTianxianData;
    public void InitData(GetTianxianInfoResponse response)
    {
        curTianxianData = response;
        for (int i = 0; i < response.explores.Length; i++)
        {
            curTianxianData.explores[i].herodbIDs = initItemHeroData(curTianxianData.explores[i].heroIDs);
            if (curTianxianData.explores[i].heroIDs.Length <= 0)
            {
                curTianxianData.explores[i].startTime = 0;
            }
            itemlist[i].InitData(curTianxianData.explores[i], onGetRewardClick, onAddHerosClick);
        }
    }

    int[] initItemHeroData(int[] _heroIDs)
    {
        int[] herodbIDs = new int[_heroIDs.Length];
        for (int i = 0; i < _heroIDs.Length; i++)
        {
            for (int j = 0; j < curTianxianData.heros.Length; j++)
            {
                if (curTianxianData.heros[j].id == _heroIDs[i])
                {
                    herodbIDs[i] = curTianxianData.heros[j].heroID;
                }
            }
        }
        return herodbIDs;
    }


    void freshItem()
    {
        foreach (var item in itemlist)
        {
            if (item.data == curexplore)
            {
                item.refreshUI();
            }
        }
    }

    explore curexplore;
    void onAddHerosClick(explore _item)
    {
        curexplore = _item;
        openSelectPartnerNodeData();
    }

    public GameObject goSelecteConfirmbtn;
    public GameObject gobtn_closeSelect;
    public GridAdapter grid;
    public GameObject itemObject;
    public UILabel selectCount;
    public UILabel lab_curSelectCount;
    List<PartnerInfoItem_Data> selectedHeroItems = new List<PartnerInfoItem_Data>();
    List<Hero> herolist = new List<Hero>();
    List<int> otherHeroidlist = new List<int>();
    List<PartnerInfoItem_Data> itemdatalist = new List<PartnerInfoItem_Data>();
    List<GridItem_Data> gridItemDatas = new List<GridItem_Data>();

    void openSelectPartnerNodeData()
    {
        setlist();
        setSelectCount();
        parterSelectNode.SetActive(true);
    }

    void setSelectCount()
    {
        lab_curSelectCount.text = selectedHeroItems.Count + "/4";
    }


    public void setlist()
    {
        selectedHeroItems.Clear();
        herolist.Clear();
        otherHeroidlist.Clear();
        int[] curSelectedArr = new int[0];
        for (int i = 0; i < curTianxianData.explores.Length; i++)
        {
            if (curTianxianData.explores[i].id != curexplore.id)
            {
                for (int j = 0; j < curTianxianData.explores[i].heroIDs.Length; j++)
                {
                    otherHeroidlist.Add(curTianxianData.explores[i].heroIDs[j]);
                }
            }else
            {
                curSelectedArr = curTianxianData.explores[i].heroIDs;
            }
        }
        //for (int i = 0; i < GameGlobal.gamedata.partnerlist.Count; i++)
        //{
        //    herolist.Add(GameGlobal.gamedata.partnerlist[i]);
        for (int i = 0; i < curTianxianData.heros.Length; i++)
        {
            bool isCan = true;
            foreach (var item in otherHeroidlist)
            {
                if (curTianxianData.heros[i].id == item)
                {
                    isCan = false;
                }
            }
            if (curTianxianData.heros[i].heroID != 1001 && isCan)
            {
                herolist.Add(curTianxianData.heros[i]);
            }
        }
        itemdatalist.Clear();
        gridItemDatas.Clear();
        grid.clear();
        for (int i = 0; i < herolist.Count; i++)
        {
            PartnerInfoItem_Data temp = new PartnerInfoItem_Data();
            temp.bean = herolist[i];
            temp.index = i;
            temp.callObj = (object backObj) =>
            {
                setselect(temp);
            };
            gridItemDatas.Add(temp);
            itemdatalist.Add(temp);
            for (int j = 0; j < curSelectedArr.Length; j++)
            {
                if (curSelectedArr[j] == temp.bean.id)
                {
                    setselect(temp);
                }
            }
        }
        grid.setListData(gridItemDatas, itemObject, GRID_RUNTYPE.GRID_TYPE_READLOADPOOL);
        grid.refreshItemsData();
    }
    private void setselect(PartnerInfoItem_Data temp)
    {
        for (int i = 0; i < itemdatalist.Count; i++)
        {
            if (itemdatalist[i] == temp)
            {
                if (itemdatalist[i].selected)
                {
                    itemdatalist[i].selected = false;
                    itemdatalist[i].item?.setSelect(false);
                    selectedHeroItems.Remove(itemdatalist[i]);
                }
                else
                {
                    if (selectedHeroItems.Count >= 4)
                    {
                        UIManager.showToast("派遣数量已达上限！");
                    }
                    else
                    {
                        itemdatalist[i].selected = true;
                        itemdatalist[i].item?.setSelect(true);
                        selectedHeroItems.Add(itemdatalist[i]);
                    }
                }
            }
        }
        setSelectCount();
    }

    void closeSelectTip()
    {
        parterSelectNode.SetActive(false);
        selectedHeroItems.Clear();
    }
    void onSelectHeroConfirmClick(GameObject go)
    {

        List<int> heroIds = new List<int>();
        foreach (var item in selectedHeroItems)
        {
            heroIds.Add(item.id);
        }
        HttpManager.instance.SetTianxianHeros(curexplore.id, heroIds, (response) =>
        {
            //if (response.explore.heroIDs.Length <= 0)
            //{
            //    curexplore.heroIDs = heroIds.ToArray();
            //}else
            //{
            curexplore.heroIDs = response.explore.heroIDs;
            //}
            curexplore.herodbIDs = initItemHeroData(curexplore.heroIDs);
            curexplore.mul = response.explore.mul;
            if (curexplore.heroIDs.Length <= 0)
            {
                curexplore.startTime = 0;
            }
            else
            {
                curexplore.startTime = response.explore.startTime;
            }
            freshItem();
        });
        closeSelectTip();
    }

    void onGetRewardClick(explore _item)
    {
        curexplore = _item;

        HttpManager.instance.GetTianxianReward(curexplore.id, (ca) =>
        {
            _item.startTime = (int)(HttpManager.currentTimeMillis() / 1000);
            freshItem();
        });
    }
}
