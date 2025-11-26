using UnityEngine;
using System.Collections;

public class GridItem : MonoBehaviour {


    //高度 或者  宽度
    public float itemSize = 0;


    //携带的数据
    public GridItem_Data gridData;
    public void setGridItem_Data(GridItem_Data data)
    {
        isInited = false;   //重新设置了数据
        this.gridData = data;
    }

    public void refreshData()
    {
        isInited = true;
        initItem(gridData);
    }

    void Start()
    {
       // CTUTools.tofan(gameObject);
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

    public virtual void initItem(GridItem_Data data)
    {

    }


    #region update
    public int itemDelay = 0;
    [HideInInspector]
    public int itemMaxLoop = 4;
    void Update()
    {

       /* itemDelay = (itemDelay > itemMaxLoop) ? 0 : itemDelay + 1;

        if (itemDelay == 0)
        {*/
            uiUpdate();
        /*}*/

    }

    public virtual void uiUpdate()
    {

    }
    #endregion


    #region 临时资源的处理
    private bool resourceLoaded = true;
    //卸载临时资源
    public void _uninstallTempResource()
    {
        if(resourceLoaded)
        {
            resourceLoaded = false;
            uninstallTempResource();
        }
    }
    //加载临时资源
    public void _installTempResource()
    {
        if(!resourceLoaded)
        {
            resourceLoaded = true;
            installTempResource();
        }
    }

    //卸载临时资源
    public virtual void uninstallTempResource()
    {


    }
    //加载临时资源
    public virtual void installTempResource()
    {

    }
    #endregion

    /*
        private callbackInt itemCallBack;

        public void setItemCallBack(callbackInt callInt)
        {
            this.itemCallBack = callInt;
        }*/


}
