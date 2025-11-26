using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


public class IconTable : MonoBehaviour
{

    private callbackInt tableEvent;

    [HideInInspector]
    public List<IconBaseItem> iconItems = new List<IconBaseItem>();

    [HideInInspector]
    public List<IconBaseItem_Data> datas = new List<IconBaseItem_Data>();


    private GameObject prefabObject;
    

    public UIWidget.Pivot cellPivot = UIWidget.Pivot.Left;


    public Vector2 padding = new Vector2(0,0);


    private UIScrollView scrollView;


    void Awake()
    {
        scrollView = this.transform.parent.GetComponent<UIScrollView>();
    }


    public void setGridItems(List<IconBaseItem_Data> datas, GameObject prefabObject, callbackInt tableEvent,  int defaultShow = 0)
    {
        this.datas = datas;
        this.prefabObject = prefabObject;
        this.tableEvent = tableEvent;

        for (int i = 0; i < datas.Count; i++)
        {
            GameObject item = NGUITools.AddChild(gameObject, prefabObject);
            UTools.setActive(item,true);
            item.name = "item" + i;

            IconBaseItem script = item.GetComponent<IconBaseItem>();
            script.initWidget();

            script.setGridItem_Data(datas[i]);
            script._initItem();

            script.setItemCallBack(setShowItemIndex);

            iconItems.Add(script);
        }

        ResetPosotion();

        setShowItemIndex(defaultShow);
    }
    
    public void setShowItemIndex(int itemId)
    {
        //执行改变显示
        for(int i = 0; i < iconItems.Count; i++)
        {
            IconBaseItem item = iconItems[i];
            if (item.gridData.itemId == itemId)
            {
                item.setItemChange(true);
            }
            else
            {
                item.setItemChange(false);
            }
        }
        //返回给外部 执行具体操作的回调
        if(tableEvent != null)
        {
            tableEvent(itemId);
        }
    }



    private void ResetPosotion()
    {
        float startX = 0;
        float startY = 0;

        float gapX = 0;
        float gapY = 0;

        Vector3 itemSize;

        BoxCollider box = null;
        //prefabObject.GetComponent<BoxCollider>();
        if(iconItems.Count > 0)
        {
            box = iconItems[0].GetComponent<BoxCollider>();
        }
        if(box != null)
        {
            itemSize = box.size;
        }else
        {
            itemSize = Vector3.zero;
        }


        Vector2 pivotOffset = NGUIMath.GetPivotOffset(cellPivot);

        startX = padding.x/2 + itemSize.x * pivotOffset.x;
        //startY = padding.y + itemSize.y * pivotOffset.y;

        //有方向之后  可以设置为负数作为方向的发展
        gapX = itemSize.x + padding.x;
        gapY = itemSize.y;

        for (int i = 0; i < iconItems.Count; i++)
        {
            GameObject obj = iconItems[i].gameObject;

            obj.transform.localPosition = new Vector3(startX + gapX * i, startY /*+ gapY*i*/, 0);
        }

    }

    public void resetScrollView()
    {
        if(scrollView != null)
        {
            scrollView.ResetPosition();
        }
    }

    public List<IconBaseItem> GetChildList()
    {
        return iconItems;
    }

    public void clear()
    {
        UTools.clearChildImmediate(gameObject, true);
        iconItems.Clear();
        datas.Clear();


        tableEvent = null;
    }

    public void removeItem(TopTabItem removeItem)
    {
        for(int i = 0; i < iconItems.Count; i++)
        {
            if(removeItem == iconItems[i])
            {
                iconItems.RemoveAt(i);
                datas.RemoveAt(i);

                ResetPosotion();
                break;
            }
        }
        //纠正itemID
        for (int i = 0; i < datas.Count; i++)
        {
            datas[i].itemId = i;
        }

        DestroyImmediate(removeItem.gameObject);

    }

}

