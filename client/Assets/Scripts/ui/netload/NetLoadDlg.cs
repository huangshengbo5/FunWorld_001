using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetLoadDlg : DialogMonoBehaviour
{

    public static NetLoadDlg instance;
    //动画
    private float rotate = 0;
    private float angleGap = 25.714f;

    private int timeGap = 10;
    private int curTime = 0;


    //参数
    private string content = "";
    private string content2 = "";
    public long delayTime = 1000L;
    public long lastTime = 5000L;
    private bool couldCancel;//是否可以强制取消

    private long visibleTime;

    public bool notAllowClose;




    public UITexture bg;
    public UILabel loadContent;





    void Awake()
    {
        instance = this;

        UIEventListener.Get(bg.gameObject).onClick = onEvent_butClick;


    }

    void Start()
    {


    }


    void Update()
    {



        if (couldCancel && UTools.currentTimeMillis() - visibleTime > 5000)
        {
            setTipContent((content + "   " + "点击取消"));
        }

        //参数部分
        if (UTools.currentTimeMillis() - visibleTime > lastTime)
        {
            UIManager.showToast("通信失败，请重试");
            UIManager.hideNetLoad();
        }

    }




    private void show(bool isFlag)
    {
        if (isFlag)
        {
            bg.alpha = (float)100 / 255;

        }
        else
        {
            bg.alpha = (float)0.01f;

        }
    }

    public void setContent(string v, long delayTime, long lastTime, bool couldCancel, bool notAllowClose)
    {
        this.content = v;
        this.delayTime = delayTime;
        this.lastTime = lastTime;
        this.couldCancel = couldCancel;
        visibleTime = UTools.currentTimeMillis();

        this.notAllowClose = notAllowClose;

        setTipContent(content);

        if (delayTime > 0)
        {
            show(false);
        }
    }

    private void setTipContent(string content)
    {
        loadContent.text = content;
    }


    private void onEvent_butClick(GameObject obj)
    {
        if (couldCancel && UTools.currentTimeMillis() - visibleTime > 5000)
        {
            UIManager.hideNetLoad();
        }
    }
}
