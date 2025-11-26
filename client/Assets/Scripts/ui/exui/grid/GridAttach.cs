using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 附加的Grid  优化显示 控制效率   active可减少滑动时对帧数的影响
/// </summary>

public class GridAttach : MonoBehaviour
{
    //滚动条
    public LoopScrollBar loopScrollBar = null;

    //依赖的空间
    public UIGrid uiGrid;
    public UIScrollView scrollView;
    public UIPanel scrollPanel;

    //内容项
    public List<Transform> obj = new List<Transform>();

    //初始化的位置信息
    private Vector3 scrollViewPosition = new Vector3();
    private Vector3 scrollPanelPosition = new Vector3();

    private bool isHor = false;
    private bool isVor = false;

    private int maxColumeLimit = 0;


    //Item
    private GameObject itemPrefab;



    void Awake()
    {

        uiGrid = GetComponent<UIGrid>();
        maxColumeLimit = uiGrid.maxPerLine;

        scrollView = this.transform.parent.GetComponent<UIScrollView>();
        scrollPanel = scrollView.GetComponent<UIPanel>();

        scrollViewPosition = new Vector3(scrollView.transform.localPosition.x, scrollView.transform.localPosition.y, scrollView.transform.localPosition.z);
        scrollPanelPosition = new Vector3(scrollPanel.transform.localPosition.x, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);


        if (scrollView.movement == UIScrollView.Movement.Vertical)
        {
            isVor = true;
        }
        else
        {
            isHor = true;
        }

        initProperty();
    }

    //滚动系数之类的
    private void initProperty()
    {
        //scrollView.momentumAmount = 80;
        //scrollView.disableDragIfFits = true;
        //scrollView.dragEffect = UIScrollView.DragEffect.CATSTUDIO;
    }
    
    public void setItem_Object(GameObject item)
    {
        this.itemPrefab = item;
    }

    private GridItem addItem()
    {
        GameObject obj = NGUITools.AddChild(gameObject, itemPrefab);
        obj.SetActive(true);


        //刷新 需要手动指定位置
        uiGrid.Reposition();
        uiGrid.repositionNow = true;

        GridItem item = obj.GetComponent<GridItem>();
        return item;
    }


    void Update()
    {
       
        refreshState();

        if (loopScrollBar != null)
        {
            if(isVor)
            {
                loopScrollBar.updataBar(scrollView, uiGrid.cellHeight, obj.Count, scrollPanel.clipOffset.y, scrollPanel.GetViewSize().y);
            }
            else
            {
                loopScrollBar.updataBar(scrollView, uiGrid.cellWidth, obj.Count, scrollPanel.clipOffset.x, scrollPanel.GetViewSize().x);
            }
        }
    }



    private void refreshState()
    {
        //偏移量
        float clipOffset = isVor ? scrollPanel.clipOffset.y : scrollPanel.clipOffset.x;
        //显示区域
        float showZone = isVor ? scrollPanel.GetViewSize().y : scrollPanel.GetViewSize().x;
        //Item的间隔
        float cell = isVor ? uiGrid.cellHeight : uiGrid.cellWidth;
        //超出屏幕多少区域 隐藏
        float beyondZone = cell * 1.5f;

        if (isVor)
        {
            float minY = 0 - showZone / 2 - beyondZone - uiGrid.transform.localPosition.y;
            float maxY = showZone / 2 + beyondZone - uiGrid.transform.localPosition.y;

            for (int i = 0; i < obj.Count; i++)
            {
                float y = obj[i].localPosition.y - clipOffset;

                if (y > minY && y < maxY)
                {
                    UTools.setActive(obj[i].gameObject, true);
                }
                else
                {
                    UTools.setActive(obj[i].gameObject, false);
                }
            }


        }
        else if (isHor)
        {
            float minX = 0 - showZone / 2 - beyondZone - uiGrid.transform.localPosition.x;
            float maxX = showZone / 2 + beyondZone - uiGrid.transform.localPosition.x;

            for (int i = 0; i < obj.Count; i++)
            {
                float x = obj[i].localPosition.x - clipOffset;

                if (x > minX && x < maxX)
                {
                    UTools.setActive(obj[i].gameObject, true);
                }
                else
                {
                    UTools.setActive(obj[i].gameObject, false);
                }
            }
        }


    }


    public List<Transform> getTransList()
    {
        return obj;
    }

    public void repositionNow()
    {
        repositionNow(false);
        refreshState();
    }

    public void repositionNow(bool savePosition)
    {
        Vector3 curPanelPositon = new Vector3(scrollPanel.transform.localPosition.x, scrollPanel.transform.localPosition.y, scrollPanel.transform.localPosition.z);


        //刷新 需要手动指定位置
        uiGrid.Reposition();

        //绑定位置
        obj.Clear();
        foreach (Transform child in transform)
        {
            obj.Add(child);
        }

        //消除动画
        SpringPanel spring = scrollPanel.GetComponent<SpringPanel>();
        if (spring != null)
        {
            spring.enabled = false;
        }


        refreshState();
        

        if(savePosition)
        {
            scrollPanel.transform.localPosition = curPanelPositon;

            scrollView.Scroll(0.001f);
        }
        else
        {
            scrollView.transform.localPosition = new Vector3(scrollViewPosition.x, scrollViewPosition.y, scrollViewPosition.z);
            scrollPanel.clipOffset = new Vector2(0, 0);
        }
    }


    public void RemoveNode(GameObject gameObject)
    {
        StartCoroutine(removeNode(gameObject));
    }

    private IEnumerator removeNode(GameObject gameObject)
    {
        for (int i = 0; i < obj.Count; i++)
        {
            obj[i].gameObject.SetActive(true);
        }
        //yield return new WaitForEndOfFrame();
        for (int i = 0; i < obj.Count; i++)
        {
            if (gameObject.transform == obj[i])
            {
                Destroy(obj[i].gameObject);
                uiGrid.RemoveChild(obj[i]);
                obj.Remove(obj[i]);
                break;
            }
        }
        yield return new WaitForEndOfFrame();
        uiGrid.Reposition();
        //不调用此方法  不能刷新icon
        scrollView.Scroll(0.001f);
    }


    public void clear()
    {
        clearGrid();
    }

    private void clearGrid()
    {
        obj.Clear();

        UTools.clearChildImmediate(uiGrid.gameObject, true);

        //P
        scrollView.transform.localPosition = new Vector3(scrollViewPosition.x, scrollViewPosition.y, scrollViewPosition.z);
        scrollPanel.clipOffset = new Vector2(0, 0);
    }

    void OnDestroy()
    {
        obj.Clear();
    }
}
