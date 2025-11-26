using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void popupEvent(int index);

public class FairyPopupList : MonoBehaviour {

    public event popupEvent onPopupEvent;

    private List<Transform> optionList = new List<Transform>();
    public Transform itemTransform;

    public List<string> optionName = new List<string>();
    public List<UISprite> optionIcon;


    public int extraHeight = 0;

    private int index = 0;

    public UIWidget bgFrame;
    private BoxCollider boxCollider;

    public bool isPopup = false;

    void Awake()
    {


        UIEventListener.Get(bgFrame.gameObject).onClick = onEvent_buttonBG;
        bgFrame.GetComponent<UISprite>().spriteName = "new_poplistdi";

        boxCollider = bgFrame.gameObject.GetComponent<BoxCollider>();
    }

    private void onEvent_buttonBG(GameObject go)
    {
        closePopup();
    }

    public void initPopup(string[] name)
    {
        if (name != null)
        {
            for (int i = 0; i < name.Length; i++)
            {
                optionName.Add(name[i]);
            }
        }

        for(int i = 0; i < optionName.Count; i++)
        {
            GameObject obj = NGUITools.AddChild(this.gameObject, itemTransform.gameObject);
            obj.name = ""+ i;

            UILabel temp = obj.transform.Find("name").GetComponent<UILabel>();
            temp.text = optionName[i];

            UTools.setLabelEffect(temp, 4, 24);

            if(optionIcon != null && optionIcon.Count > i)
            {
                GameObject icon = obj.transform.Find("icon").gameObject;

                UTools.setPresent(obj, optionIcon[i].gameObject);

                optionIcon[i].transform.localPosition = new Vector3(icon.transform.localPosition.x, icon.transform.localPosition.y, icon.transform.localPosition.z);
                Destroy(icon);
            }



            UTools.setActive(obj, true);
            optionList.Add(obj.transform);
        }
        
        setOptionIndex(0);
        bindItemEvent();
    }

    public void refreshname(string[] name)
    {
        for (int i = 0; i < optionList.Count; i++)
        {
            optionList[i].gameObject.GetComponentInChildren<UILabel>().text= name[i];
        }
    }


/*
    //添加Item
    public void addOptionItem(Transform trans)
    {
        optionList.Add(trans);
    }*/

    //设置显示xx
    public void setOptionIndex(int index)
    {
        if (index >= optionList.Count) return;
        for(int i = 0; i < optionList.Count; i++)
        {
            optionList[i].gameObject.SetActive(false);
        }
        optionList[index].gameObject.SetActive(true);
        optionList[index].localPosition = new Vector3(0, 0, 0);
        this.index = index;
    }

    //绑定事件
    private void bindItemEvent()
    {
        for (int i = 0; i < optionList.Count; i++)
        {
            Transform form = optionList[i];
            //事件
            UIEventListener.Get(form.gameObject).onClick = switchPopup;
        }
    }

    //切换弹出状态
    public void switchPopup(GameObject obj)
    {
        isPopup = !isPopup;


        if(isPopup)
        {
            int heightGap = itemTransform.GetComponent<UIWidget>().height;

            bgFrame.height = heightGap * optionList.Count + 5 + extraHeight;
            boxCollider.size = new Vector3(3200, 1800,0);
            
            bgFrame.gameObject.SetActive(true);
            
            for (int i = 0; i < optionList.Count; i++)
            {
                optionList[i].localPosition = new Vector3(0, -i * heightGap, 0);
                optionList[i].gameObject.SetActive(true);
            }

            //if (FriendInterface.getInstance() != null)
            //{
            //    if (FriendInterface.getInstance().panelFollow.popupList.gameObject.Equals(gameObject))
            //    {
            //        FriendInterface.getInstance().panelFollow.popupListg.closePopup();
            //        int tempdepth = FriendInterface.getInstance().panelFollow.popupListg.GetComponent<UIPanel>().depth;
            //        FriendInterface.getInstance().panelFollow.popupListg.GetComponent<UIPanel>().depth = FriendInterface.getInstance().panelFollow.popupList.GetComponent<UIPanel>().depth;
            //        FriendInterface.getInstance().panelFollow.popupList.GetComponent<UIPanel>().depth = tempdepth;
            //    }
            //    else if (FriendInterface.getInstance().panelFollow.popupListg.gameObject.Equals(gameObject))
            //    {
            //        FriendInterface.getInstance().panelFollow.popupList.closePopup();
            //        int tempdepth = FriendInterface.getInstance().panelFollow.popupList.GetComponent<UIPanel>().depth;
            //        FriendInterface.getInstance().panelFollow.popupList.GetComponent<UIPanel>().depth = FriendInterface.getInstance().panelFollow.popupListg.GetComponent<UIPanel>().depth;
            //        FriendInterface.getInstance().panelFollow.popupListg.GetComponent<UIPanel>().depth = tempdepth;
            //    }
            //}


        }else
        {
            for(int i = 0; i < optionList.Count; i++)
            {
                if(obj.name.Equals(""+i))
                {
                    index = i;
                    break;
                }
            }
            setOptionIndex(index);
            
            bgFrame.gameObject.SetActive(false);

            //执行切换排序
            Debug.Log("切换排序=====" + index);

            onPopupEvent(index);

            
        }
    }



    public void closePopup()
    {
        if(isPopup)
        {
            isPopup = false;
            setOptionIndex(index);
            bgFrame.gameObject.SetActive(false);
        }
    }

	void OnDestroy(){


		UTools.DestroyMonoRef(ref optionIcon);
	}
}
