using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public enum GRID_RUNTYPE {

    GRID_TYPE_NONE,
    GRID_TYPE_READLOAD, //实时加载
    GRID_TYPE_READHIDE, //实时加载 隐藏不用的
    GRID_TYPE_READLOADPOOL, //实时加载 隐藏不用的添加到池中 重复利用
}

public class GridAdapter : MonoBehaviour{

    private GRID_RUNTYPE runType = GRID_RUNTYPE.GRID_TYPE_NONE;

    //滚动条
    public LoopScrollBar loopScrollBar = null;

    //依赖的空间
    public UIScrollView scrollView;
    public UIPanel scrollPanel;

    //内容项
    public List<GridItem_Data> dataList = new List<GridItem_Data>();
    private GridItem[] itemList = new GridItem[0];

    //最初的位置信息
    public Vector3 panelInitPosition = new Vector3();
    private Vector2 panelInitOffset = new Vector2();

    //初始化列表后的位置信息
    private Vector3 panelFinishPosition = new Vector3();
    private Vector2 panelFinishOffset = new Vector2();

    //中心点
    private Vector2 centerPosition = new Vector2();

    private bool isHor = false;
    private bool isVor = false;

    private float itemGap = 0;      //间隔
    private float itemGapSecond = 0;    //第二间隔

    private float buffRange = 0;    //缓冲填充补充部分
    private float showRange = 0;    //整体显示范围
    
    //Item
    private GameObject itemPrefab;


    public bool isShowNULL = true;
    public GameObject gobj_null;


    //是否使用 逐步加载
    private bool useReadLoad = false;
    //是否使用池
    private bool useItemPool = false;
    //是否隐藏 不需要的item
    private bool useHideMode = false;


    //列表的间距之类的信息
    public float cellWidth;
    public float cellHeight;
    public int maxColumeLimit = 1;
    //public UIGrid.Arrangement arrangement;

    public bool isLoadFinish = false;

    void Awake()
    {
        
        initWidget();
    }


    private bool isInit = false;
    public void initWidget()
    {
        if (isInit) return;
        isInit = true;

        scrollView = this.transform.parent.GetComponent<UIScrollView>();
        scrollPanel = scrollView.GetComponent<UIPanel>();


        UIGrid grid = GetComponent<UIGrid>();
        if (grid != null)
        {
            cellWidth = grid.cellWidth;
            cellHeight = grid.cellHeight;
            maxColumeLimit = grid.maxPerLine;

            Destroy(grid);
        }
        maxColumeLimit = maxColumeLimit <= 0 ? 1 : maxColumeLimit;


        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            isVor = true;
            itemGap = cellHeight;
            itemGapSecond = cellWidth;
            buffRange = 1600;// itemGap * 2;
            showRange = scrollPanel.GetViewSize().y;
        }
        else
        {
            isHor = true;
            itemGap = cellWidth;
            itemGapSecond = cellHeight;
            buffRange = 1600;// itemGap * 2;
            showRange = scrollPanel.GetViewSize().x;
        }


        initProperty();

        if (gobj_null == null && isShowNULL)
        {
            GameObject obj = ResManager.getGameObject("allpre", "ui/exui/GridNull");
            gobj_null = UTools.AddChild(scrollPanel.gameObject, obj);
            UTools.setActive(gobj_null, true);
        }


        //去掉scrollview的center坐标
        scrollPanel.transform.localPosition += new Vector3(scrollPanel.baseClipRegion.x, scrollPanel.baseClipRegion.y, 0);
        scrollPanel.baseClipRegion = new Vector4(0, 0, scrollPanel.baseClipRegion.z, scrollPanel.baseClipRegion.w);

        //scrollview 中心localPosition点
        centerPosition = new Vector2(scrollPanel.clipOffset.x, scrollPanel.clipOffset.y);
        panelInitPosition = new Vector3(scrollPanel.transform.localPosition.x, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);
        panelInitOffset = new Vector2(scrollPanel.clipOffset.x, scrollPanel.clipOffset.y);




        scrollPanel.clipSoftness = new Vector2(0, 0);
    }


    void OnEnable()
    {

    }

    void OnDisable()
    {
        
    }


    //滚动系数之类的
    private void initProperty()
    {
        scrollView.disableDragIfFits = true;
        scrollView.dragEffect = UIScrollView.DragEffect.CATSTUDIO;
        //catstudio方式不行也要此参数
        //scrollView.momentumAmount = 80;
    }
    

    public void setCell(int cellWidth, int cellHeight)
    {
        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            isVor = true;
            itemGap = cellHeight;
            itemGapSecond = cellWidth;
        }
        else
        {
            isHor = true;
            itemGap = cellWidth;
            itemGapSecond = cellHeight;
        }
    }
/*
    public void setListData(List<GridItem_Data> dataList, GameObject item, GRID_RUNTYPE runType)
    {
        StartCoroutine(setListData2(dataList, item, runType));
    }*/

    /// <summary>
    /// 设置待现实数据列表
    /// </summary>
    /// <param name="dataList">显示的数据列表</param>
    /// <param name="item">基础item</param>
    /// <param name="useReadLoad">是否及时加载</param>
    /// <param name="useItemPool">是否使用池 非及时加载自动关闭此选项</param>
    /// <param name="useHideMode">是否隐藏两侧Item 以提高效率</param>
    public void setListData(List<GridItem_Data> dataList, GameObject item, GRID_RUNTYPE m_runType, bool isUseHide = true)
    {
        initWidget();

        this.dataList = dataList;
        itemList = new GridItem[dataList.Count];
        this.runType = m_runType;

        this.itemPrefab = item;
        if(itemPrefab != null)
            UTools.setActive(itemPrefab, false);
        
        if(runType == GRID_RUNTYPE.GRID_TYPE_NONE)
        {
            this.useReadLoad = false;
            this.useItemPool = false;
        }else if(runType == GRID_RUNTYPE.GRID_TYPE_READLOAD)
        {
            this.useReadLoad = true;
            this.useItemPool = false;
            //this.useHideMode = true;
        }
        else if (runType == GRID_RUNTYPE.GRID_TYPE_READHIDE)
        {
            this.useReadLoad = true;
            this.useItemPool = false;
            //this.useHideMode = true;
        }
        else if (runType == GRID_RUNTYPE.GRID_TYPE_READLOADPOOL)
        {
            this.useReadLoad = true;
            this.useItemPool = true;
            //this.useHideMode = true;
        }
        this.useHideMode = isUseHide;

        
        if(itemPrefab != null)
        {
            //初始化 grid的位置  本gameobject的位置
            if (scrollView.movement == UIScrollView.Movement.Vertical)
            {
                float sourceHeight = 0;
                BoxCollider collider = itemPrefab.GetComponent<BoxCollider>();
                if (collider == null)
                {
                    collider = itemPrefab.AddComponent<BoxCollider>();
                    Debug.LogError("没有添加item BoxCollider");
                }
                sourceHeight = collider.size.y;
                collider.size = new Vector3(collider.size.x, cellHeight, 0);

                this.transform.localPosition = new Vector3(centerPosition.x, centerPosition.y + scrollPanel.GetViewSize().y / 2 - cellHeight / 2, 0);
                Debug.LogError(this.transform.localPosition + " " + centerPosition + " " + scrollPanel.GetViewSize());
            }
            else
            {
                float sourceWidth = 0;
                BoxCollider collider = itemPrefab.GetComponent<BoxCollider>();
                if (collider == null)
                {
                    collider = itemPrefab.AddComponent<BoxCollider>();
                    Debug.LogError("没有添加item BoxCollider");
                }
                sourceWidth = collider.size.x;
                collider.size = new Vector3(cellWidth, collider.size.y, 0);

                this.transform.localPosition = new Vector3(centerPosition.x - scrollPanel.GetViewSize().x / 2 + cellWidth / 2, centerPosition.y);
            }
        }

      //  Debug.LogError("---" + scrollPanel.GetComponent<UIScrollView>().bounds);

        isLoadFinish = true;
        initShowItem();



        refreshNullFlag();


        if(isKeepPosition)
        {
            isKeepPosition = false;

            scrollPanel.transform.localPosition = new Vector3(keepPosition.x, keepPosition.y, keepPosition.z);
            scrollPanel.clipOffset = new Vector2(keepClipOffset.x, keepClipOffset.y);

            FairyRunnable.postRunnable(() =>
            {
                scrollView.RestrictCatStudioWithinBounds();
            });

        }

        //yield return 0;
    }

    //增加数据
    public void addListData(List<GridItem_Data> newList)
    {
        int startIndex = this.dataList.Count;
        foreach(GridItem_Data temp in newList)
        {
            this.dataList.Add(temp);
        }
        
        GridItem[] tempList = new GridItem[dataList.Count];
        for(int i = 0; i < itemList.Length; i++)
        {
            tempList[i] = itemList[i];
        }
        itemList = tempList;

        //不实时读取  直接加载出来
        if (!useReadLoad)
        {
            for(int i = 0; i < newList.Count; i++)
            {
                addItem(newList[i], startIndex+i);
            }
        }
        
        repositionGrid();
        refreshState();

        refreshNullFlag();

    }


    //增加数据
    public void InsertListData(GridItem_Data temp, int startIndex)
    {
        
        this.dataList.Insert(startIndex, temp);
        
        GridItem[] tempList = new GridItem[dataList.Count];
        int addFlag = 0;
        for (int i = 0; i < itemList.Length; i++)
        {
            if(i == startIndex)
            {
                addFlag = 1;
            }
            tempList[i+ addFlag] = itemList[i];
        }
        itemList = tempList;

        //不实时读取  直接加载出来
        if (!useReadLoad)
        {
            addItem(temp, startIndex);
        }

        repositionGrid();
        refreshState();
        refreshNullFlag();
    }

    //刷新所有的数据
    public void refreshItemsData()
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            if(itemList[i]!=null)
            itemList[i].refreshData();
        }
    }

    //空空如也标记符
    public void refreshNullFlag()
    {
        if (gobj_null == null) return;
        if (dataList.Count < 1)
        {
            UTools.setActive(gobj_null.gameObject, true);

            //408 233 自动适配  空空如也的尺寸
            float scale = 0;


            float scaleX = (float)scrollPanel.GetViewSize().x / 408;
            float scaleY = (float)scrollPanel.GetViewSize().y / 233;

            scale = Mathf.Min(scaleX, scaleY);
            
            if (scale > 1) scale = 1;
            gobj_null.transform.localScale = new Vector3(scale, scale, scale);


            Vector2 test = new Vector2(scrollPanel.clipOffset.x, scrollPanel.clipOffset.y);
            //中心位置
            gobj_null.transform.localPosition = new Vector3(test.x, test.y);
        }
        else
        {
            UTools.setActive(gobj_null, false);
        }
    }

    #region 数组 和 数据的维护
    private void RemoveItemListNode(int index)
    {
        if(index >= itemList.Length)
        {
            Debug.Log("什么情况   移除的index  > items.Length");
            return;
        }

        GridItem[] tempList = new GridItem[itemList.Length -1];
        int tempIndex = 0;
        for(int i = 0; i < itemList.Length; i++)
        {
            if(i == index)
            {
                MonoBehaviour.DestroyImmediate(itemList[i].gameObject);
            }
            else
            {
                tempList[tempIndex] = itemList[i];
                tempIndex++;
            }
        }
        itemList = tempList;


        refreshNullFlag();
    }

    //插入数据
    public void InsertItemListNode(int index, GridItem_Data data)
    {
        //添加到尾部
        if(index >= dataList.Count)
        {
            List<GridItem_Data> newDatas = new List<GridItem_Data>();
            newDatas.Add(data);
            addListData(newDatas);
        }else
        {
            dataList.Insert(index, data);

            GridItem[] newItemList = new GridItem[itemList.Length + 1];

            int orderIndex = 0;
            for (int i = 0; i < newItemList.Length; i++)
            {
                if (i == index)
                {
                    orderIndex = 1;
                    continue;
                }
                else
                {
                    newItemList[i] = itemList[i - orderIndex];
                }
            }
            itemList = newItemList;

            addItem(data, index);
            
            //排序
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i] == null) break;
                itemList[i].transform.SetSiblingIndex(i);
            }
        }

        repositionGrid();
    }


    #endregion



    #region Item的创建与使用

    private void initShowItem()
    {
        int curColumnCount = 0;

        float fullHeight = 0;
        for(int i = 0; i < dataList.Count; i++)
        {
            GridItem gridItem = addItem(dataList[i],i);

            //如果使用池 及时加载 隐藏item模式
            if(useReadLoad)
            {
                if (maxColumeLimit > 1)
                {
                    curColumnCount += 1;
                    if (curColumnCount >= maxColumeLimit)
                    {
                        curColumnCount = 0;
                        fullHeight += itemGap;
                    }
                }
                else
                {
                    fullHeight += itemGap;
                }

                if (fullHeight >= showRange + buffRange)
                {
                    break;
                }
            }
        }
        /*   //更新池对象
           refreshPoolObject();*/

        //没数据直接不刷新
        if (dataList.Count < 1) return;

        repositionGrid();
        resetPosition();

/*

        scrollView.ResetPosition();*/
    }

    private GridItem addItem(GridItem_Data data, int index)
    {
        //Logger.Log("s=====" + Tool.currentTimeMillis());

        GameObject obj = null;

        //如果使用了池
        if (useItemPool)
        {
            obj = popItemInPool();
            if(obj == null)
            {
                if (itemPrefab != null)
                    obj = NGUITools.AddChild(gameObject, itemPrefab);
                else
                    obj = NGUITools.AddChild(gameObject, data.obj_prefab);
            }
        }
        else
        {
            if (itemPrefab != null)
                obj = NGUITools.AddChild(gameObject, itemPrefab);
            else
                obj = NGUITools.AddChild(gameObject, data.obj_prefab);
        }
        obj.name = "gridItem_" + index;
        //Logger.Log("0=====" + Tool.currentTimeMillis());
        UTools.setActive(obj, true);


        //Logger.Log("1=====" + Tool.currentTimeMillis());
        GridItem item = obj.GetComponent<GridItem>();
        item.initWidget();

        //Logger.Log("2=====" + Tool.currentTimeMillis());
        item.setGridItem_Data(data);
        item._initItem();

        //Logger.Log("3=====" + Tool.currentTimeMillis());
        data.data_Object = item.gameObject;

        BoxCollider box = item.GetComponent<BoxCollider>();
        if(index != 0 && box != null)
        {
            if(isVor)
            {
                box.size = new Vector3(box.size.x, itemGap, box.size.z);
            }
            else
            {
                box.size = new Vector3(itemGap, box.size.y, box.size.z);
            }
        }

        //Logger.Log("4=====" + Tool.currentTimeMillis());
        itemList[index] = (item);     //增加到对应的对象池中
       // Logger.Log("f=====" + Tool.currentTimeMillis());
        return item;
    }

    public void removeItem(GridItem item)
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            if(itemList[i] != null && itemList[i].Equals(item))
            {
                RemoveItemListNode(i);
                dataList.RemoveAt(i);
                repositionGrid();
                
                return;
            }
        }
    }

    #region 对象池    最大池数量 约为X2的显示量   比如在最底层 执行跳到顶层时
/*
    private GridItem firstItem;
    private GridItem endItem;
*/

    private List<GridItem> itemPool = new List<GridItem>();

    private void pushItemToPool(GridItem noUseItem, int index, bool removeFormItemList=true)
    {
        UTools.setActive(noUseItem.gameObject, false);
        itemPool.Add(noUseItem);

        if(removeFormItemList)
        {
            //itemList.Remove(index);
            //RemoveItemListNode(index);
            itemList[index] = null;
        }
    }

    private GameObject popItemInPool()
    {
        if(itemPool.Count > 0)
        {
            GridItem temp = itemPool[0];
            itemPool.RemoveAt(0);
            return temp.gameObject;
        }
        return null;
    }

    /*

        //刷新池对象
        private void refreshPoolObject()
        {
            if (useItemPool)
            {
                if (itemList.Length > 0)
                {
                    firstItem = itemList[0];
                    endItem = itemList[itemList.Length - 1];
                }
            }
        }
    */

    #endregion


    #endregion



    #region 刷新 和 位置 部分

    public IEnumerator respositionInitNow()
    {
        yield return new WaitForEndOfFrame();
        repositionGrid();
        resetPosition();// resetPosition();
    }
    //手动 更改位置   使用池的时候使用
    public void repositionGrid()
    {

        float startX = 0;
        float startY = 0;// itemGap/2;

        if(isVor)
        {
            if(maxColumeLimit > 1)
            {
                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] == null) continue;

                    int row = i%maxColumeLimit;
                    int line = (i + 1) / maxColumeLimit; ;
                    
                    startX = ((-(maxColumeLimit-1) * 0.5f)  + row) * itemGapSecond;
                    startY = -itemGap * ((i)/maxColumeLimit);
                    itemList[i].transform.localPosition = new Vector3(startX, startY);
                }
            }
            else
            {
                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] == null) continue;
                    startY = -itemGap * (i);
                    itemList[i].transform.localPosition = new Vector3(startX, startY);
                }
            }
        }else
        {
            if (maxColumeLimit > 1)
            {

                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] == null) continue;
                    
                    int line = i % maxColumeLimit;
                    int row = (i) / maxColumeLimit;
                    
                    startX = itemGap * row;
                    startY = (((maxColumeLimit - 1) * 0.5f) - line) * itemGapSecond;
                    itemList[i].transform.localPosition = new Vector3(startX, startY);
                }
            }
            else
            {
                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] == null) continue;
                    startX = itemGap * (i);
                    itemList[i].transform.localPosition = new Vector3(startX, startY);
                }
            }
        }

        //scrollView.MoveRelative(new Vector3(-1, 0, 0));
        //这句要不要屏蔽掉呢？
        scrollView.RestrictCatStudioWithinBounds();
        // scrollView.RestrictWithinBounds(true);//.UpdatePosition();
    }


    //置顶
    public void resetPosition()
    {

        scrollView.ResetPosition();
        
        panelFinishOffset = new Vector2(scrollPanel.clipOffset.x, scrollPanel.clipOffset.y);
        panelFinishPosition = new Vector3(scrollPanel.transform.localPosition.x, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);


        //centerPosition = new Vector2(scrollPanel.clipOffset.x, scrollPanel.clipOffset.y);
        refreshNullFlag();
    }

    public void toStart()
    {
        //如果现有项 大于 
        if (dataList.Count * itemGap < showRange) return;

        for(int i = 0; i < itemList.Length; i++)
        {
            if(itemList[i] != null)
            {
                UTools.setActive(itemList[i].gameObject, true);
            }
        }

        resetPosition();
        refreshState();
    }

    //判断是否处在 最下端
    public bool isEndPosition()
    {
        //如果现有项 大于 
        float elmTotleSize = dataList.Count * itemGap;
        if (maxColumeLimit > 1)
        {
            elmTotleSize = (dataList.Count / maxColumeLimit) * itemGap;
            if (dataList.Count % maxColumeLimit != 0)
            {
                elmTotleSize += itemGap;
            }
            if (elmTotleSize < showRange) return true;
        }
        else if (dataList.Count * itemGap < showRange)
        {
            return true;
        }


        float offset = (elmTotleSize - showRange);

        if (isVor)
        {
            if(Mathf.Abs(scrollPanel.clipOffset.y - (panelFinishOffset.y - offset)) < 15 &&  Mathf.Abs(scrollPanel.transform.localPosition.y - (panelFinishPosition.y + offset)) < 15)
            {
                return true;
            }
        }
        else
        {
            if (Mathf.Abs(scrollPanel.clipOffset.x - (panelFinishOffset.x + offset)) < 15 && Mathf.Abs(scrollPanel.transform.localPosition.x - (panelFinishPosition.x - offset)) < 15)
            {
                return true;
            }
        }

        return false;
    }

    public void toEnd()
    {
        //如果现有项 大于 
        float elmTotleSize = dataList.Count * itemGap;
        if (maxColumeLimit > 1)
        {
            elmTotleSize = (dataList.Count / maxColumeLimit) * itemGap;
            if (dataList.Count % maxColumeLimit != 0)
            {
                elmTotleSize += itemGap;
            }
            if (elmTotleSize < showRange) return;
        }
        else if (dataList.Count * itemGap < showRange) {
            return;
        }

        clearSpine();

        float offset = (elmTotleSize - showRange) + 10;

        if (isVor)
        {
            scrollPanel.clipOffset = new Vector3(scrollPanel.clipOffset.x, panelFinishOffset.y - offset);
            scrollPanel.transform.localPosition = new Vector3(scrollPanel.transform.localPosition.x, panelFinishPosition.y + offset,scrollPanel.transform.localPosition.z);
        }else
        {
            scrollPanel.clipOffset = new Vector3(panelFinishOffset.x + offset, scrollPanel.clipOffset.y);
            scrollPanel.transform.localPosition = new Vector3(panelFinishPosition.x - offset, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);
        }
        refreshState();
    }

    public void toIndex(int index)
    {
        //如果现有项 大于 
        float elmTotleSize = (index+1) * itemGap;
        if (maxColumeLimit > 1)
        {
            elmTotleSize = ((index + 1) / maxColumeLimit) * itemGap;
            if ((index + 1) % maxColumeLimit != 0)
            {
                elmTotleSize += itemGap;
            }
            if (elmTotleSize < showRange) return;
        }
        else if ((index + 1) * itemGap < showRange)
        {
            return;
        }

        clearSpine();

        float offset = (elmTotleSize - showRange) + 10;

        if (isVor)
        {
            scrollPanel.clipOffset = new Vector3(scrollPanel.clipOffset.x, panelFinishOffset.y - offset);
            scrollPanel.transform.localPosition = new Vector3(scrollPanel.transform.localPosition.x, panelFinishPosition.y + offset, scrollPanel.transform.localPosition.z);
        }
        else
        {
            scrollPanel.clipOffset = new Vector3(panelFinishOffset.x + offset, scrollPanel.clipOffset.y);
            scrollPanel.transform.localPosition = new Vector3(panelFinishPosition.x - offset, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);
        }
        refreshState();
    }


    public int getListHeight()
    {
        int line = dataList.Count / maxColumeLimit;
        if (dataList.Count % maxColumeLimit > 0) line += 1;
        if (isVor)
        {
            return (int)(line * cellHeight);
        }
        else
        {
            return (int)(line * cellWidth);
        }
    }

    private void clearSpine()
    {
        //消除动画
        SpringPanel spring = scrollPanel.GetComponent<SpringPanel>();
        if (spring != null)
        {
            spring.enabled = false;
        }
    }

    #endregion


    #region update部分

    public void Update()
    {
        if (loopScrollBar != null)
        {
            if (isVor)
            {
                loopScrollBar.updataBar(scrollView, cellHeight, itemList.Length, scrollPanel.clipOffset.y, scrollPanel.GetViewSize().y);
            }
            else
            {
                loopScrollBar.updataBar(scrollView, cellWidth, itemList.Length, scrollPanel.clipOffset.x, scrollPanel.GetViewSize().x);
            }
        }



        refreshState();
    }


    List<GridItem> useItemList = new List<GridItem>();

    //刷新状态
    private void refreshState()
    {

        //偏移量
        float clipOffset = isVor ? (panelFinishOffset.y - scrollPanel.clipOffset.y) : (scrollPanel.clipOffset.x - panelFinishOffset.x);


        //是否需要排列一下
        bool isAddItem = false;

        if (useItemPool)
        {
            useItemList.Clear();
        }



        if (isVor)
        {
            float minY = clipOffset - buffRange / 2;
            float maxY = clipOffset + showRange + buffRange / 2;

            int minCell = (int)(minY / itemGap / transform.localScale.y);
            int maxCell = (int)(maxY / itemGap / transform.localScale.y);

            if (maxColumeLimit > 1)
            {
                minCell = minCell * maxColumeLimit;
                maxCell = maxCell * maxColumeLimit;
            }
            //提前放入池中
            if(useItemPool)
            {
                for (int i = 0; i < minCell; i++)
                {
                    if (itemList[i] != null)
                    {
                        pushItemToPool(itemList[i], i);
                    }
                }
                for (int i = maxCell + 1; i < dataList.Count; i++)
                {
                    if (itemList[i] != null)
                    {
                        pushItemToPool(itemList[i], i);
                    }
                }
            }

            for (int i = minCell; i <= maxCell; i++)
            {
                if (i < 0) continue;
                if (i >= dataList.Count) break;

                if (itemList[i] == null)
                {
                    addItem(dataList[i], i);
                    isAddItem = true;
                }
                if (useItemPool)
                {
                    useItemList.Add(itemList[i]);
                }
            }


            if (useHideMode)
            {
                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] == null) continue;

                    if(i >= minCell && i <= maxCell)
                    {
                        UTools.setActive(itemList[i].gameObject, true);
                    }else
                    {
                        UTools.setActive(itemList[i].gameObject, false);
                    }
                }
            }

            if (usescollbar&& itemList.Length>0)
            {
                if (itemList[itemList.Length - 1] == null)
                {
                    addItem(dataList[itemList.Length - 1], itemList.Length - 1);
                    isAddItem = true;
                }
            }

        }
        else if (isHor)
        {
            float minX = clipOffset - buffRange / 2;
            float maxX = clipOffset + showRange + buffRange / 2;

            int minCell = (int)(minX / itemGap / transform.localScale.x);
            int maxCell = (int)(maxX / itemGap / transform.localScale.x);

            if (maxColumeLimit > 1)
            {
                minCell = minCell * maxColumeLimit;
                maxCell = maxCell * maxColumeLimit;
            }

            //提前放入池中
            if (useItemPool)
            {
                for (int i = 0; i < minCell; i++)
                {
                    if (itemList[i] != null)
                    {
                        pushItemToPool(itemList[i], i);
                    }
                }
                for (int i = maxCell + 1; i < dataList.Count; i++)
                {
                    if (itemList[i] != null)
                    {
                        pushItemToPool(itemList[i], i);
                    }
                }
            }

            for (int i = minCell; i <= maxCell; i++)
            {
                if (i < 0) continue;
                if (i >= dataList.Count) break;

                if (itemList[i] == null)
                {
                    addItem(dataList[i], i);
                    isAddItem = true;
                }
                if (useItemPool)
                {
                    useItemList.Add(itemList[i]);
                }
            }

            if (useHideMode)
            {
                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] == null) continue;

                    if (i >= minCell && i <= maxCell)
                    {
                        UTools.setActive(itemList[i].gameObject, true);
                    }
                    else
                    {
                        UTools.setActive(itemList[i].gameObject, false);
                    }
                }
            }
        }

        if (isAddItem)
        {
            repositionGrid();
        }

    }

    #endregion


    public void restrictOffset()
    {
        StartCoroutine(restrictOffsetIE());
    }

    private IEnumerator restrictOffsetIE()
    {
        yield return 0;
        scrollView.RestrictCatStudioWithinBounds();
    }


    public GridItem[] getTransList()
    {
        return itemList;
    }

    public List<GridItem_Data> getDataList()
    {
        return dataList;
    }

    public float getGridSize()
    {
        float totleSize = 0;
        for(int i = 0; i < dataList.Count; i++)
        {
            if(dataList[i] != null)
            {
                if (isVor)
                {
                    totleSize += cellHeight;
                }
                else
                {
                    totleSize += cellWidth;
                }
            }
        }
        return totleSize;
    }

    public bool isFullCell()
    {
        if(isVor)
        {
            if (getGridSize() >= scrollPanel.GetViewSize().y)
            {
                return true;
            }
        }
        else
        {
            if (getGridSize() >= scrollPanel.GetViewSize().x)
            {
                return true;
            }
        }
        
        return false;
    }

    public GameObject getScrollPanel()
    {
        initWidget();
        return scrollPanel.gameObject;
    }


    public void clear()
    {
        clear(false);
    }


    private Vector3 keepPosition = new Vector3();
    private Vector2 keepClipOffset = new Vector2();
    private bool isKeepPosition;
    public bool usescollbar=false;

    public void clear(bool isKeepPosition)
    {
        if(scrollPanel != null && scrollPanel.transform != null)
        {
            keepPosition = new Vector3(scrollPanel.transform.localPosition.x, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);
            keepClipOffset = new Vector2(scrollPanel.clipOffset.x, scrollPanel.clipOffset.y);
        }

        initWidget();

        dataList.Clear();

        UTools.clearChildImmediate(gameObject, true);

        for(int i = 0; i < itemList.Length; i++)
        {
            itemList[i] = null;
        }

        itemList = new GridItem[0];

        itemPool.Clear();

        if(!isKeepPosition)
        {
            clearSpine();
            //toStart();
            scrollPanel.transform.localPosition = new Vector3(panelInitPosition.x, panelInitPosition.y, panelInitPosition.z);
            scrollPanel.clipOffset = new Vector2(panelInitOffset.x, panelInitOffset.y);
        }

        this.isKeepPosition = isKeepPosition;


    }


    void OnDestroy()
    {
        dataList.Clear();
    
        for(int i = 0; i < itemList.Length; i++)
        {
            itemList[i] = null;
        }
        itemList = null;


        UTools.DestroyMonoRef(ref gobj_null);
    }
    
}
