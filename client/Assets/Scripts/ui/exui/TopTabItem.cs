using System;
using System.Collections;
using UnityEngine;


public class TopTabItem_Data : IconBaseItem_Data
{
    public UISprite icon;

    public bool isNoIcon;

    public int offsetX;
    public float scale = 1;

    public int width = -1;

   // public UICatPlayer iconPlayer;

}


public class TopTabItem : IconBaseItem {

    private UISprite obj_TabSelect;
    private UISprite obj_UnTabSelect;

    private UILabel label;
    private UISprite flowerSprite;

    //提示
    private tipcount tipPoint;
    //可以升级的提示
    private GameObject obj_upgradeTip;

    private const int icon_InitX = 38;
    


    public override void initWidget()
    {
        label = this.transform.Find("TabTitle").GetComponent<UILabel>();
        label.depth = 101;

        flowerSprite = this.transform.Find("flower").GetComponent<UISprite>();
        flowerSprite.depth = 101;

        obj_TabSelect = this.transform.Find("tab_red").GetComponent< UISprite>();
        obj_UnTabSelect = this.transform.Find("tab_blue").GetComponent<UISprite>();


        UIEventListener.Get(gameObject).onPress = onEvent_Press;


        tipPoint = this.transform.Find("Tip").GetComponent<tipcount>();

        obj_upgradeTip = this.transform.Find("upgradeTip").gameObject;
        UTools.setActive(obj_upgradeTip, false);
    }

    public TopTabItem_Data temp;
    public override void initItem(IconBaseItem_Data data)
    {
        temp = (TopTabItem_Data)data;


        setTitle(temp.itemTitle);

        if(temp.width != -1)
        {
            TopTabItem.setTopTabWidth(gameObject, temp.width);
        }
        

        setOffsetX(temp.offsetX);

        if (temp.isNoIcon)
        {
            setNoIcon();
        } else if (temp.icon != null)
        {
            setIcon(temp.icon);
        }
        //else if(temp.iconPlayer != null)
        //{
        //    setPlayerIcon(temp.iconPlayer);
        //}

        setItemChange(false);
    }

    void Start()
    {
        //  UIUntils.setLabelEffect(label, 4,26);

       // CTUTools.tofan(gameObject);
    }



    public void setWidth(int width)
    {
        obj_TabSelect.width = width;
        obj_UnTabSelect.width = width;

        BoxCollider box = GetComponent<BoxCollider>();
        if(box != null)
        {
            box.size = new Vector3(width, box.size.y, 0);
            box.center = new Vector3(width/2, 0, 0);
        }
    }
    

    //上层调用
    public void showTip(int num)
    {
        tipPoint.setCount(num);
    }
    

    public void setUpgradeTip(bool iscouldUpgrade)
    {
        if(iscouldUpgrade)
        {
            UTools.setActive(obj_upgradeTip, true);
        }else
        {
            UTools.setActive(obj_upgradeTip, false);
        }
    }

    public void setIconName(string str)
    {
        flowerSprite.spriteName = str;

        flowerSprite.MakePixelPerfect();
    }

    private UISprite icon;
    public void setIcon(UISprite icon)
    {
        this.icon = icon;
        UTools.setPresent(gameObject, icon.gameObject);
        icon.depth = 101;
        icon.transform.localPosition = new Vector3(icon_InitX + offsetX, -4, 0);

        icon.transform.localScale = new Vector3(temp.scale, temp.scale, temp.scale);

        UTools.setActive(flowerSprite.gameObject,false);
    }

    //private UICatPlayer iconPlayer;
    //public void setPlayerIcon(UICatPlayer icon)
    //{
    //    this.iconPlayer = icon;
    //    PlatformTools.setPresent(gameObject, icon.gameObject);
    //    iconPlayer.depth = 101;

    //    iconPlayer.transform.localPosition = new Vector3(icon_InitX + offsetX, -4, 0);
    //    iconPlayer.transform.localScale = new Vector3(temp.scale, temp.scale, temp.scale);


    //    UIUntils.setActive(flowerSprite.gameObject,false);
    //}

    public GameObject getIcon()
    {
        if (icon != null)
        {
            return icon.gameObject;
        }
        else
        {
            return flowerSprite.gameObject;
        }
    }

    private int offsetX = 0;
    public void setOffsetX(int offx)
    {
        this.offsetX = offx;
    }
    public void setNoIcon()
    {
        UTools.setActive(flowerSprite.gameObject, false);

       // label.transform.localPosition = new Vector3(82, label.transform.localPosition.x, label.transform.localPosition.z);
       // label.fontSize = 40;
    }
    

    public void setTitle(string str)
    {
        label.text = str;
    }


    public override void setItemChange(bool isSelectd)
    {
        if (isSelectd)
        {
            UTools.setActive(obj_TabSelect.gameObject, true);
            UTools.setActive(obj_UnTabSelect.gameObject, false);

            if (icon != null)
            {
                icon.transform.localPosition = new Vector3(icon_InitX + offsetX, 0, 0);
            }
            //else if(iconPlayer != null)
            //{
            //    iconPlayer.transform.localPosition = new Vector3(icon_InitX + offsetX, 0, 0);
            //}

            label.transform.localPosition = new Vector3(label.transform.localPosition.x, 0, 0);

            obj_upgradeTip.transform.localPosition = new Vector3(25, 3, 0);
        }
        else
        {
            if (icon != null)
            {
                icon.transform.localPosition = new Vector3(icon_InitX + offsetX, -4, 0);
            }
            //else if (iconPlayer != null)
            //{
            //    iconPlayer.transform.localPosition = new Vector3(icon_InitX + offsetX, -4, 0);
            //}

            UTools.setActive(obj_TabSelect.gameObject, false);
            UTools.setActive(obj_UnTabSelect.gameObject, true);

            label.transform.localPosition = new Vector3(label.transform.localPosition.x, -4, 0);

            obj_upgradeTip.transform.localPosition = new Vector3(25, 0, 0);
        }
    }



    private void onEvent_Press(GameObject go, bool state)
    {
        //SoundEventer.play_Tab();
        if(state)
        {
           // SoundEventer.play_Tab();
            excateCallBack(this.gridData.itemId);
        }
    }







    public static GameObject getTopTabPrefab(IconTable table)
    {
        table.padding = new Vector2(36, 0);
        GameObject obj = ResManager.getGameObjectNoInit("allpre", "ui/exui/TopTab");
        return obj;
    }

    public static GameObject setTopTabWidth(GameObject obj, int width)
    {
        TopTabItem tabItem = obj.GetComponent<TopTabItem>();
        GameObject icon = obj.transform.Find("flower").gameObject;
        GameObject name = obj.transform.Find("TabTitle").gameObject;

        tabItem.setWidth(width);

        //调整位置 大小等
        float sourceWidth = 164;
        float PosPrecent = width /sourceWidth;

        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x * PosPrecent,  icon.transform.localPosition.y, icon.transform.localPosition.z);
        name.transform.localPosition = new Vector3(name.transform.localPosition.x * PosPrecent, name.transform.localPosition.y, name.transform.localPosition.z);
        
        return obj;
    }

	void OnDestroy(){


		UTools.DestroyMonoRef(ref  flowerSprite);
        UTools.DestroyMonoRef(ref  label);
        UTools.DestroyMonoRef(ref  obj_UnTabSelect);
        UTools.DestroyMonoRef(ref  obj_TabSelect);
        UTools.DestroyMonoRef(ref  icon);
	}
}
