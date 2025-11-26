using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class TableAdapter : MonoBehaviour {
    

    //滚动条
    public LoopScrollBar loopScrollBar = null;

    //依赖的空间
    public UIScrollView scrollView;
    public UIPanel scrollPanel;

    //内容项
    public List<GridItem_Data> dataList = new List<GridItem_Data>();
    public GridItem[] itemList = new GridItem[0];


    //最初的位置信息
    private Vector3 panelInitPosition = new Vector3();
    private Vector2 panelInitOffset = new Vector2();
    
    //中心点
    private Vector2 centerPosition = new Vector2();
    

    private int buffRange = 0;    //缓冲填充补充部分
    private float showRange = 0;    //整体显示范围


    public int padding = 5;

    //Item
    public GameObject itemPrefab;

    void Awake()
    {
        initWidget();

    }


    private bool isInit = false;
    private void initWidget()
    {
        if (isInit) return;
        isInit = true;

        scrollView = this.transform.parent.GetComponent<UIScrollView>();
        scrollPanel = scrollView.GetComponent<UIPanel>();

        buffRange = 5;

        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            showRange = scrollPanel.GetViewSize().y;
        }
        else
        {
            showRange = scrollPanel.GetViewSize().x;
        }

        initProperty();

        //scrollview 中心localPosition点
        centerPosition = new Vector2(scrollPanel.clipOffset.x, scrollPanel.clipOffset.y);
        panelInitPosition = new Vector3(scrollPanel.transform.localPosition.x, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);
        panelInitOffset = new Vector2(scrollPanel.clipOffset.x, scrollPanel.clipOffset.y);
    }


    //滚动系数之类的
    private void initProperty()
    {
        scrollView.disableDragIfFits = true;
        scrollView.dragEffect = UIScrollView.DragEffect.CATSTUDIO;
        //catstudio方式不行也要此参数
        //scrollView.momentumAmount = 80;
    }

    private int limitCount = -1;
    public void setLimitCount(int limitCount)
    {
        this.limitCount = limitCount;
    }

    public void setItemGameObject(GameObject item)
    {
        this.itemPrefab = item;
    }

    /// <summary>
    /// 设置待现实数据列表
    /// </summary>
    /// <param name="dataList">显示的数据列表</param>
    /// <param name="item">基础item</param>
    /// <param name="useItemPool">是否使用池   放弃此参数  比较难用数据 算出item的高度</param>
    public void setListData(List<GridItem_Data> dataList, GameObject item)
    {

        initWidget();

        this.dataList = dataList;
        itemList = new GridItem[dataList.Count];

        this.itemPrefab = (item);
        //UIUntils.setActive(itemPrefab, true);

        //初始化 grid的位置  本gameobject的位置
        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            this.transform.localPosition = new Vector3(centerPosition.x, centerPosition.y + scrollPanel.GetViewSize().y / 2, 0);
        }
        else
        {
            this.transform.localPosition = new Vector3(centerPosition.x - scrollPanel.GetViewSize().x / 2, centerPosition.y);
        }

        initShowItem();
    }
    
    //增加数据
    public void addListData(List<GridItem_Data> newList, bool isAddIm/*, GameObject item = null*/)
    {
      /*  if(item != null)
        {
            this.itemPrefab = (item);
        }*/

        initWidget();
        //新GridItem的开头
        int newIndex = dataList.Count;

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

        if(isAddIm)
        {
            for (int i = newIndex; i < newIndex + newList.Count; i++)
            {
                addItem(dataList[i], i);
            }

        }

        for (int i = 0; i < 1000; i++)
        {
            //删掉
            if (limitCount != -1 && dataList.Count > limitCount)
            {
                dataList.RemoveAt(0);
                RemoveItemListNode(0);
            }else
            {
                break;
            }
        }

        //不刷新 等待逻辑自己刷新  可保证整个界面是显示状态  text等可计算高度
        if(gameObject.activeSelf)
        {
            refreshState();
        }
        if(isAddIm)
        {
            repositionNow();
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
                if(itemList[i] != null && itemList[i].gameObject != null)
                {
                    MonoBehaviour.DestroyImmediate(itemList[i].gameObject);
                }
            }
            else
            {
                tempList[tempIndex] = itemList[i];
                tempIndex++;
            }
        }
        itemList = tempList;
    }


    #endregion



    #region Item的创建与使用

    private void initShowItem()
    {
        float fullHeight = 0;
        for(int i = 0; i < dataList.Count; i++)
        {
            GridItem gridItem = addItem(dataList[i],i);
            
            /*fullHeight += gridItem.itemSize;
            
            if(fullHeight >= showRange)
            {
                break;
            }*/
        }

        repositionNow();
        resetPosition();
    }
    //置顶
    public void resetPosition()
    {
        if (scrollView == null) return;
        scrollView.ResetPosition();
        
    }
    private GridItem addItem(GridItem_Data data, int index)
    {
        GameObject obj = null;
        

        obj = NGUITools.AddChild(gameObject, itemPrefab);
        UTools.setActive( obj,true);



        GridItem item = obj.GetComponent<GridItem>();
        item.initWidget();
        item.setGridItem_Data(data);
        item._initItem();
        
        BoxCollider box = item.GetComponent<BoxCollider>();
        if (box != null)
        {
            if (scrollView.movement == UIScrollView.Movement.Vertical)
            {
                box.size = new Vector3(box.size.x, box.size.y + padding, box.size.z);
                item.itemSize = box.size.y;
              //  Debug.LogError("size=" + box.size.y);
            }
            else
            {
                box.size = new Vector3(box.size.x + padding, box.size.y, box.size.z);
                item.itemSize = box.size.x;
            }
        }


        itemList[index] = (item);     //增加到对应的对象池中



        return item;
    }

    public void removeItem(GridItem item)
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            if(itemList[i].Equals(item))
            {
                RemoveItemListNode(i);
                dataList.RemoveAt(i);
                repositionNow();
                return;
            }
        }
    }

    #endregion



    #region 刷新 和 位置 部分

    private void repositionNow()
    {
        float startX = 0;
        float startY = 0;

        float curItemSize = 0;
        float lastItemSize = 0;


        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i] == null) continue;

                curItemSize = itemList[i].itemSize;
                if (i < 1)
                {
                    startY -= lastItemSize;
                }
                else
                {
                    startY -= (lastItemSize);
                }

                lastItemSize = curItemSize;

                itemList[i].transform.localPosition = new Vector3(startX, startY);
            }
        }
        else
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i] == null) continue;

                curItemSize = itemList[i].itemSize;
                if (i < 1)
                {
                    startY -= lastItemSize;
                }
                else
                {
                    startY -= (lastItemSize);
                }

                lastItemSize = curItemSize;

                itemList[i].transform.localPosition = new Vector3(startX, startY);
            }
        }


    }

 

    public float getItemsSize()
    {
        float size = 0;
        
        for (int i = 0; i < itemList.Length; i++)
        {
            if(itemList[i] != null)
            {
                if (i < 1)
                {
                    size += itemList[i].itemSize;
                }
                else
                {
                    size += (itemList[i].itemSize);
                }
            }
        }
        if(itemList.Length > 0)
        {
            size -= 2*padding;
        }
        return size;
    }

    public void toStart()
    {
        //如果现有项 大于 
        if (getItemsSize() < showRange) return;

        //toStart();
        scrollPanel.transform.localPosition = new Vector3(panelInitPosition.x, panelInitPosition.y, panelInitPosition.z);
        scrollPanel.clipOffset = new Vector2(panelInitOffset.x, panelInitOffset.y);

        refreshState();
    }


    //判断是否处在 最下端
    public bool isEndPosition()
    {
        //如果现有项 大于 
        float size = getItemsSize();
        //如果现有项 大于 
        if (size < showRange) return true;
        
        float offset = (size - showRange);

        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            float t1 = Mathf.Abs(scrollPanel.clipOffset.y - (panelInitOffset.y - offset));
            float t2 = Mathf.Abs(scrollPanel.transform.localPosition.y - (panelInitPosition.y + offset));

            if (t1 < 15 && t2 < 15)
            {
                return true;
            }
        }
        else
        {
            if (Mathf.Abs(scrollPanel.clipOffset.x - (panelInitOffset.x + offset)) < 15 && Mathf.Abs(scrollPanel.transform.localPosition.x - (panelInitPosition.x - offset)) < 15)
            {
                return true;
            }
        }

        return false;
    }

    public void toEnd()
    {
        float size = getItemsSize();

      //  Debug.LogError(size+" "+showRange);

        //如果现有项 大于 
        if (size < showRange) return;

        clearSpine();

        float offset = (size - showRange);
     //   Debug.LogError(offset);
        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            
            scrollPanel.clipOffset = new Vector3(scrollPanel.clipOffset.x, panelInitOffset.y - offset-1);
            scrollPanel.transform.localPosition = new Vector3(scrollPanel.transform.localPosition.x, panelInitPosition.y + offset, scrollPanel.transform.localPosition.z);
        }
        else
        {
            scrollPanel.clipOffset = new Vector3(panelInitOffset.x + offset, scrollPanel.clipOffset.y);
            scrollPanel.transform.localPosition = new Vector3(panelInitPosition.x - offset, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);
        }

        scrollView.currentMomentum = Vector3.zero;
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

    void Update()
    {
        refreshState();

        /*if (loopScrollBar != null)
        {
            if (isVor)
            {
                loopScrollBar.updataBar(scrollView, uiGrid.cellHeight, itemList.Length, scrollPanel.clipOffset.y, scrollPanel.GetViewSize().y);
            }
            else
            {
                loopScrollBar.updataBar(scrollView, uiGrid.cellWidth, itemList.Length, scrollPanel.clipOffset.x, scrollPanel.GetViewSize().x);
            }
        }*/
    }
    

    //刷新状态
    private void refreshState()
    {
        //偏移量
        float clipOffset = scrollView.movement == UIScrollView.Movement.Vertical ? (panelInitOffset.y - scrollPanel.clipOffset.y) : (scrollPanel.clipOffset.x - panelInitOffset.x);

       /* for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] == null) continue;
            UIUntils.setActive(itemList[i].gameObject, false);
        }*/
        //是否需要排列一下
        bool isAddItem = false;


        float minPos = clipOffset;
        float maxPos = clipOffset + showRange;


        int minCell = -1;
        int maxCell = -1;

        //纠正minCell 和 maxCell
        float addSize = 0;
        bool isGetMax = false;
        for (int i = 0; i < dataList.Count; i++)
        {
            if (itemList[i] == null) continue;
            if (addSize <= minPos)
            {
                minCell = i;
            }

            if (addSize >= maxPos && !isGetMax)
            {
                maxCell = i;
                isGetMax = true;
            }
            else if(!isGetMax)
            {
                maxCell = i;
            }
            addSize += itemList[i].itemSize;
        }

        minCell -= buffRange;
        maxCell += buffRange;
        if (minCell < 0) minCell = 0;
        if (maxCell >= itemList.Length) maxCell = itemList.Length - 1;


        for (int i = 0; i < itemList.Length; i++)
        {
            if (i < minCell || i > maxCell)
            {
                if (itemList[i] == null) continue;
                UTools.setActive(itemList[i].gameObject, false);

            }
        }

        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            for (int i = minCell; i <= maxCell; i++)
            {
                if (i < 0) continue;
                if (i >= dataList.Count) break;

                if (itemList[i] == null)
                {
                    addItem(dataList[i], i);
                    isAddItem = true;
                }
                else
                {
                    UTools.setActive(itemList[i].gameObject, true);
                }
            }
        }
        else if (scrollView.movement == UIScrollView.Movement.Horizontal)
        {
    
            for (int i = minCell; i <= maxCell; i++)
            {
                if (i < 0) continue;
                if (i >= dataList.Count) break;

                if (itemList[i] == null)
                {
                    addItem(dataList[i], i);
                    isAddItem = true;
                }
                else
                {
                    UTools.setActive(itemList[i].gameObject, true);
                }
            }
        }

        if(isAddItem)
        {
            repositionNow();
        }
        
    }

    #endregion


    public List<GridItem_Data> getTranData()
    {
        return dataList;
    }

    public void clear()
    {
        initWidget();
        dataList.Clear();
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] != null)
            {
                Destroy(itemList[i].gameObject);
            }
        }
        itemList = new GridItem[0];

        clearSpine();
        //toStart();
        scrollPanel.transform.localPosition = new Vector3(panelInitPosition.x, panelInitPosition.y, panelInitPosition.z);
        scrollPanel.clipOffset = new Vector2(panelInitOffset.x, panelInitOffset.y);
    }

    void OnDestroy()
    {
        dataList.Clear();

        for (int i = 0; i < itemList.Length; i++)
        {
            itemList[i] = null;
        }
        itemList = null;
    }
}
