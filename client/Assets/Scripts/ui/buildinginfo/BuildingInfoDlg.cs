using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoDlg : DialogMonoBehaviour
{
    public static BuildingInfoDlg instance;
    public GameObject closebut;
    public UILabel nameandlvlabel;
    public GameObject rennode;
    public UILabel lastnameandlvlabel, lastdesclabel, nextnameandlvlabel, nextdesclabel;
  //  public GameObject shangbut, xiabut;
    public GameObject leftbut, rightbut;
    public GameObject shengbut,maxbut;
    public RewardItemBian costitem;

    public GridAdapter grid;
    public GameObject itemObject;
 //   public GameObject selectkuang;

    public APaintNodeSpine renpaintnode;
    public TexPaintNode texpaintnode;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        setShowAnim(true);
        setClickOutZoneClose(false);
        UIEventListener.Get(closebut).onClick = closeDialog;



        UIEventListener.Get(leftbut).onClick = onclickleft;
        UIEventListener.Get(rightbut).onClick = onclickright;


        UIEventListener.Get(shengbut).onClick = onclicklevelup;

        SoundEventer.add_But_ClickSound(closebut);
        SoundEventer.add_But_ClickSound(leftbut);
        SoundEventer.add_But_ClickSound(rightbut);
        SoundEventer.add_But_ClickSound(shengbut);

    }



    private void onclicklevelup(GameObject go)
    {
        if (selectherodata.bean.level >= Statics.BUILDLEVELMAX)
        {
            //UIManager.showToast("等级已满！");
            return;
        }

        HttpManager.instance.sendBuildingLevelUp(selectherodata.bean, nextlevelbean.getlvcost(), (int code) =>
        {
            refreshui();
            //UIManager.showToast("升级成功");
        });
    }
    private void onclickright(GameObject go)
    {
        BuildingInfoItem_Data next = itemdatalist[(selectherodata.index + 1 + itemdatalist.Count) % itemdatalist.Count];
        setselect(next);
        correctScrollItem_GridItem(grid, next);
    }

    private void onclickleft(GameObject go)
    {
        BuildingInfoItem_Data next = itemdatalist[(selectherodata.index - 1 + itemdatalist.Count) % itemdatalist.Count];
        setselect(next);
        correctScrollItem_GridItem(grid, next);
    }

    public static void correctScrollItem_GridItem(GridAdapter grid, GridItem_Data showData)
    {

        UIScrollView ScrollView = grid.scrollView;
        UIPanel scrollPanel = grid.scrollPanel;
        SpringPanel spring = ScrollView.GetComponent<SpringPanel>();


        int showIndex = 0;
        for (int i = 0; i < grid.getDataList().Count; i++)
        {
            if (grid.getDataList()[i] == showData)
            {
                showIndex = i;
                break;
            }
        }

        showIndex = showIndex / grid.maxColumeLimit;

        if (ScrollView.canMoveVertically)
        {

            //小数                        //大数
            Vector2 moveRange = new Vector2(grid.panelInitPosition.y, grid.cellHeight * grid.getDataList().Count + grid.panelInitPosition.y - scrollPanel.GetViewSize().y);


            float goalTargetY = (grid.cellHeight * showIndex - scrollPanel.GetViewSize().y / 2);
            if (goalTargetY < moveRange.x)
            {
                goalTargetY = moveRange.x;
            }
            else if (goalTargetY > moveRange.y)
            {
                goalTargetY = moveRange.y;
            }

            //动画
            if (spring == null)
            {
                spring = ScrollView.gameObject.AddComponent<SpringPanel>();
            }
            spring.target = new Vector3(scrollPanel.transform.localPosition.x, goalTargetY, scrollPanel.transform.localPosition.z);
            spring.enabled = true;

        }
        else if (ScrollView.canMoveHorizontally)
        {


            Vector2 moveRange = new Vector2(grid.panelInitPosition.x, -grid.cellWidth * grid.getDataList().Count + grid.panelInitPosition.x + scrollPanel.GetViewSize().x);


            float goalTargetX = (-grid.cellWidth * showIndex + scrollPanel.GetViewSize().x / 2);
            if (goalTargetX > moveRange.x)
            {
                goalTargetX = moveRange.x;
            }
            else if (goalTargetX < moveRange.y)
            {
                goalTargetX = moveRange.y;
            }

            //动画
            if (spring == null)
            {
                spring = ScrollView.gameObject.AddComponent<SpringPanel>();
            }
            spring.target = new Vector3(goalTargetX, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);
            spring.enabled = true;

        }

        spring.Start();
        spring.AdvanceTowardsPosition(true);
        spring.enabled = false;
        grid.Update();
    }

    void Start()
    {
        initlist();
    }
    public static int initindex = 0;
    List<BuildingInfoItem_Data> itemdatalist = new List<BuildingInfoItem_Data>();
    public void initlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();
        itemdatalist.Clear();

        for (int i = 0; i < GameGlobal.gamedata.buildlist.Count; i++)
        {
            data_buildingBean tempdbbean = data_buildingDef.getdatabybuildid(GameGlobal.gamedata.buildlist[i].buildingID);
            if (tempdbbean.type == 1 )
            {
                continue;
            }
            else if (tempdbbean.condition_type == 1 && tempdbbean.condition_val < GameGlobal.gamedata.stageindex)
            {
                continue;
            }
            BuildingInfoItem_Data temp = new BuildingInfoItem_Data();
            temp.bean = GameGlobal.gamedata.buildlist[i];
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


        setselect(itemdatalist[initindex]);

    }
    public BuildingInfoItem_Data selectherodata;
    private void setselect(BuildingInfoItem_Data temp)
    {
        for (int i = 0; i < itemdatalist.Count; i++)
        {
            if (itemdatalist[i] == temp)
            {
                itemdatalist[i].selected = true;
                if (itemdatalist[i].item != null)
                {
                    itemdatalist[i].item.setSelect(true);
                }
                selectherodata = itemdatalist[i];
                setDetail(selectherodata);
            }
            else
            {
                itemdatalist[i].selected = false;
                if (itemdatalist[i].item != null)
                {
                    itemdatalist[i].item.setSelect(false);
                }
            }
        }
    }

    private void setDetail(BuildingInfoItem_Data selectherodata)
    {
        if (texpaintnode == null)
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "ren";
            TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(rennode, "jianzhu");
            temppaintnode.transform.localPosition = new Vector3(0, 300, 0);
            temppaintnode.setdepth(11);
            temppaintnode.setShowRectLimit(800);
            temppaintnode.playAction(0,0);
            texpaintnode = temppaintnode;
        }

        texpaintnode.playAction(0,selectherodata.bean.buildingID - Statics.BUILDID);

        dbbean = data_buildingDef.getdatabybuildid(selectherodata.bean.buildingID);

        refreshui();

    }
    data_building_upgradeBean nextlevelbean,nowlevelbean;
    data_buildingBean dbbean;
    private void refreshui()
    {
        if (selectherodata.bean.level >= Statics.BUILDLEVELMAX)
        {
            shengbut.SetActive(false);
            maxbut.SetActive(true);
            nowlevelbean = data_building_upgradeDef.dicdatas[selectherodata.bean.buildingID][selectherodata.bean.level - 1];

            lastnameandlvlabel.text = dbbean.name + "  LV" + selectherodata.bean.level;
            nextnameandlvlabel.text = "MAX";

            lastdesclabel.text = nowlevelbean.effect_id + "+" + nowlevelbean.effect_val;
            nextdesclabel.text = "MAX";
        }
        else
        {
            shengbut.SetActive(true);
            maxbut.SetActive(false);
            nowlevelbean = data_building_upgradeDef.dicdatas[selectherodata.bean.buildingID][selectherodata.bean.level-1];
            nextlevelbean = data_building_upgradeDef.dicdatas[selectherodata.bean.buildingID][selectherodata.bean.level];
            costitem.setReward(nextlevelbean.getlvcost());

            lastnameandlvlabel.text= dbbean.name + "  LV" + selectherodata.bean.level;
            nextnameandlvlabel.text = dbbean.name + "  LV" + (selectherodata.bean.level+1);

            lastdesclabel.text = nowlevelbean.effect_id + " " + nowlevelbean.effect_val;
            nextdesclabel.text = nextlevelbean.effect_id + "+" + nextlevelbean.effect_val;
        }


        nameandlvlabel.text = dbbean.name + "  LV" + selectherodata.bean.level;


    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}