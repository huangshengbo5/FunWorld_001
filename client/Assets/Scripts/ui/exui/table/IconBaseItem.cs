using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;




public class IconBaseItem_Data
{

    public int itemId;
    public string itemTitle;

    public int extraInt0;
    public int extraInt1;

}


public class IconBaseItem : MonoBehaviour
{
    


    //携带的数据
    public IconBaseItem_Data gridData;
    public void setGridItem_Data(IconBaseItem_Data data)
    {
        isInited = false;   //重新设置了数据
        this.gridData = data;
    }

    public void refreshData()
    {
        isInited = true;
        initItem(gridData);
    }


    public virtual void initWidget()
    {

    }


    //是否已经初始化了+
    private bool isInited = false;
    public void _initItem()
    {
        if (isInited) return;

        isInited = true;
        initItem(gridData);
    }

    public virtual void initItem(IconBaseItem_Data data)
    {

    }

   


    private callbackInt itemCallBack;

    public void setItemCallBack(callbackInt callInt)
    {
        this.itemCallBack = callInt;
    }

    public void excateCallBack(int itemId)
    {
        if(itemCallBack != null)
        {
            itemCallBack(itemId);
        }
    }


    public virtual void setItemChange(bool isSelectd)
    {

    }

}
