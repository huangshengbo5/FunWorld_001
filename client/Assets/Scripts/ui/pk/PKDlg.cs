using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKDlg : DialogMonoBehaviour
{
    public static PKDlg instance;

    public UILabel pkTitle;//标题
    public UILabel pkTimes;//剩余时间

    public UILabel myRank;//标题
    public UILabel rankNum;//排名数量
    public UILabel changeNum;//挑战次数
    public UILabel msgLabel;//第一条
    public UILabel msgLabel2;//第二条

    public UISprite signUpButton;//报名
    public UISprite rewardButton;//领取按钮
    public UISprite rankReward;//排名奖励按钮
    public UISprite closeButton;//关闭按钮
    public GridAdapter grid;//格子
    public GameObject itemObject;

    private List<ArenaInfoItem_Data> itemdatalist = new List<ArenaInfoItem_Data>();

    public ArenaInfoItem_Data selectherodata;

    private void Awake()
    {
        instance = this;
        UIEventListener.Get(rewardButton.gameObject).onClick = _OnRewardClick;
        UIEventListener.Get(rankReward.gameObject).onClick = _OnRankReward;
        UIEventListener.Get(closeButton.gameObject).onClick = closeDialog;
        SoundEventer.add_But_ClickSound(rewardButton.gameObject);
        SoundEventer.add_But_ClickSound(rankReward.gameObject);
        SoundEventer.add_But_ClickSound(closeButton.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

        pkTitle.text = "竞技场剩余时间：";
        myRank.text = "我的排名";
        msgLabel.text = "";
        msgLabel2.text = "";
        initlist();
        ShowRank();
    }

    public void initlist()
    {
        List<GridItem_Data> itemDatas = new List<GridItem_Data>();
        itemdatalist.Clear();

        ArenaInfoItem_Data tempPlayerData = null;
        for (int i = 0; i < GameGlobal.gamedata.rankers.Count; i++)
        {

            ArenaInfoItem_Data temp = new ArenaInfoItem_Data();
            temp.ranker = GameGlobal.gamedata.rankers[i];
            temp.index = i;
            temp.callObj = (object backObj) =>
            {
                //setselect(temp);
            };
            itemDatas.Add(temp);
            itemdatalist.Add(temp);
            if (temp.ranker.isPlayer)
            {
                tempPlayerData = temp;
            }
        }
        grid.clear();
        grid.setListData(itemDatas, itemObject, GRID_RUNTYPE.GRID_TYPE_READLOAD);

        if (tempPlayerData != null)
        {
            //需要下滑到自己当前的地方。
            setselect(tempPlayerData);
        }
    }
    private void setselect(ArenaInfoItem_Data data)
    {
        correctScrollItem_GridItem(grid, data);
    }

    private void ShowRank()
    {
        if (GameGlobal.gamedata.self != null)
        {
            rankNum.text = GameGlobal.gamedata.self.rank.ToString();
        }
        changeNum.text = string.Format("剩余挑战次数{0}/10", GameGlobal.gamedata.userinfo.extend.arenaRemainNum); 
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

    private void _OnRewardClick(GameObject go)
    {
        //领奖按钮点击
    }

    private void _OnRankReward(GameObject go)
    {
        //奖励界面
        UIManager.showRewardInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
