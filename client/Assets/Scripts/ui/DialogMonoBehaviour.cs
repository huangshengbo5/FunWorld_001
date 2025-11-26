using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DialogMonoBehaviour : MonoBehaviour {

    #region 界面或者网络数据 是否已经准备充分
    private bool isAniReadyFinish = false;
    private bool isNetReadyFinish = true;
    public void setAniReadyFlag(bool flag)
    {
        this.isAniReadyFinish = flag;
    }

    public void setNetReadyFlag(bool flag)
    {
        this.isNetReadyFinish = flag;
    }

    public bool isReadyFlag()
    {
        return isAniReadyFinish && isNetReadyFinish;
    }

    #endregion


    #region 全屏标记
    private bool isFullDialog = false;

    public void setFullState(bool state)
    {
        this.isFullDialog = state;
    }

    public bool isFullState()
    {
        return isFullDialog;
    }

    #endregion


    #region 是否显示
    private bool isVisible = true;

    public virtual void setVisible(bool flag)
    {
        this.isVisible = flag;
    }

    public bool isVisibleing()
    {
        return isVisible;
    }

    //重新得到 成为主界面的时候
    public virtual void backForce()
    {

    }

    #endregion

    #region 回调

    //预留的任意使用的 callback
    [HideInInspector]
    public callbackInt dialogCallInt;
    public void setDialogCallInt(callbackInt call)
    {
        this.dialogCallInt = call;
    }


    //关闭时调用的callback
    public callbackInt closeCallBack;
    public void setCloseCallback(callbackInt callInt)
    {
        this.closeCallBack = callInt;
    }

    public void excateCloseCallback()
    {
        if (closeCallBack != null)
        {
            closeCallBack(Callback.SUCCESS);
        }
    }

    #endregion


    #region 是否隐藏关闭

    //设置是否为隐藏类型
    private bool isHideType = false;
    public void setHideType(bool flag)
    {
        this.isHideType = flag;
    }

    public bool isHideTypeed()
    {
        return isHideType;
    }
    #endregion

    #region 展示动画
    //是否有展开动画
    [HideInInspector]
    public bool isShowAnim = false;
    public void setShowAnim(bool flag)
    {
        this.isShowAnim = flag;
    }
    public bool getShowAnim()
    {
        return isShowAnim;
    }
    #endregion


    #region 旁白关闭
    //点击其他区域 是否关闭
    public bool isclickClose = true;
    public void setClickOutZoneClose(bool flag)
    {
        this.isclickClose = flag;
    }

    public bool isClickOutZoneClose()
    {
        return isclickClose;
    }
    #endregion


    #region update 10帧
    [HideInInspector]
    public int behavourMaxLoop = 0;
    private int behavourDelay = 0;
    void Update()
    {

        if (isAniReadyFinish == false) return;

        behavourDelay = (behavourDelay > behavourMaxLoop) ? 0 : behavourDelay + 1;

        if (behavourDelay == 0)
        {
            uiUpdate();
        }

    }

    public virtual void uiUpdate()
    {

    }
    #endregion


    /*public virtual void clickBlockZone(GameObject obj)
    {
        UIHelp.removeNotification(this);
    }*/

    public DialogMonoBehaviour downdlg;
    public DialogMonoBehaviour updlg;
    public virtual void closeDialog(GameObject obj)
    {
        if (isFullDialog)
        {
            Resources.UnloadUnusedAssets();
        }
        UIViewGroup.removeNotification(this);        
    }

    //关闭的通知 事件
    public virtual void closeDilaogEvent()
    {

    }


    public GameObject blockObject = null;
    public void setBlockShadow(GameObject obj)
    {
        this.blockObject = obj;

        bindBlockEvent();
    }
    private void bindBlockEvent()
    {
        if (blockObject != null)
        {
            GameObject block = blockObject.transform.Find("BlockShadow").gameObject;

            UIEventListener.Get(block).onClick = clickNullZone;
            UIEventListener.Get(block).onPress = pressNullZone;

            // SoundEventer.add_But_ClickClose(block);
        }
    }

    public virtual void pressNullZone(GameObject go, bool state)
    {
        if (isclickClose == false) return;
          closeDialog(go);
    }

    public virtual void clickNullZone(GameObject obj)
    {
        if (isclickClose == false) return;
        closeDialog(obj);
    }


    public Dictionary<string, Texture2D> textureBuffers = new Dictionary<string, Texture2D>();
    public void cleartex()
    {
        foreach (string key in textureBuffers.Keys)
        {
            Destroy(textureBuffers[key]);
        }
        textureBuffers.Clear();
    }
    public Texture2D getTex(string textureName)
    {
        if (textureBuffers.ContainsKey(textureName))
        {
            return textureBuffers[textureName];
        }
        else
        {
            Texture2D img = ResManager.getTex("texres/" + textureName);
            textureBuffers.Add(textureName, img);
            return img;
        }
    }

}
