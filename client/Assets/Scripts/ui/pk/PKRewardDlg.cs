using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKRewardDlg : DialogMonoBehaviour
{
    public static PKRewardDlg instance;
    public UILabel title;
    public GridAdapter grid;//格子
    public UISprite closeButton;//关闭按钮
    public GameObject itemObject;

    private List<ArenaRewardItem_Data> itemdatalist = new List<ArenaRewardItem_Data>();

    private void Awake()
    {
        instance = this;
        UIEventListener.Get(closeButton.gameObject).onClick = closeDialog;
        SoundEventer.add_But_ClickSound(closeButton.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        title.text = "排名奖励";
        List<data_arena_rewardBean> list = data_arena_rewardDef.datas;
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].rewards);
        }
        initlist();
    }

    public void initlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();
        itemdatalist.Clear();

        ArenaRewardItem_Data tempPlayerData = null;
        
        for (int i = 0; i < data_arena_rewardDef.datas.Count; i++)
        {

            ArenaRewardItem_Data temp = new ArenaRewardItem_Data();
            temp.reward = data_arena_rewardDef.datas[i];
            temp.index = i;
            temp.callObj = (object backObj) =>
            {
                //setselect(temp);
            };
            itemDatas.Add(temp);
            itemdatalist.Add(temp);
        }
        grid.clear();
        grid.setListData(itemDatas, itemObject, GRID_RUNTYPE.GRID_TYPE_READLOAD);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
