using UnityEngine;
using System.Collections;
using System;


public delegate void TipDelegate(int code);


//TIP 缓存
public class TipPoolDat : BasePoolDat
{
    public const int TIP_TYPE_TIP = 0;
    public const int TIP_TYPE_TIPFONT = 1;
    public const int TIP_TYPE_TIPNoTitle = 2;
    public const int TIP_TYPE_TWO = 3;
    public const int TIP_TYPE_ZUAN = 4;

    public int TipType;
    public int TipZuanValue;

    //是否包含关闭按钮
    public bool isHaveCloseButton = false;

    //按钮的样式
    public BUTTON_COLOR button1_Color = BUTTON_COLOR.RED;
    public BUTTON_COLOR button2_Color = BUTTON_COLOR.YELLOR;

    public string title = "";

    public string text;
    public string confirmText;
    public string centleText;
    public TipDelegate dele;

    public callbackInt tipEnableCallback = null;

    public bool blurflag;

    public Tip tip;

    public bool isAutoClose = true;

}

//TIP的xxxxxx
public class Tip : DialogMonoBehaviour
{


    public const int TIP_CANCEL = 0;
    public const int TIP_CONFIRM = 1;
    public const int TIP_CLOSE = 2;


    public const int TIP_ENABLE_SELECTED = 0;//选中了
    public const int TIP_ENABLE_UNSELETED = 1;//未选中

    private GameObject closeButton;

    public UILabel textLabel;

    public UIEmojiLabel datextLabel,xiaotextLabel;
    public GameObject zuannode;
    public GameObject zuanicon;
    public UILabel zuanLabel,titlelabel;

    public GameObject confirmObject;
    public GameObject centleObject;

    private UIToggle toggle;

    private TipDelegate callback = null;

    private TipPoolDat tipLoop;

    void Awake() {

        setShowAnim(true);
        setClickOutZoneClose(false);

        closeButton = this.transform.Find("close").gameObject;
        UIEventListener.Get(closeButton).onClick = onEvent_buttonClose;
        SoundEventer.add_But_ClickSound(closeButton);

        toggle = this.transform.Find("UserProtocol").GetComponent<UIToggle>();
        
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        UILabel label_Title = this.transform.Find("Title/Label").GetComponent<UILabel>();
     //   label_Title.text = Lan.tip;


    }

    public void setText(TipPoolDat tipLoop)
    {
        tipLoop.tip = this;

        //是否包含关闭按钮
        //if(tipLoop.isHaveCloseButton)
        //{
            UTools.setActive(closeButton, true);
        //}else
        //{
        //    UTools.setActive(closeButton, false);
        //}

        setTipEnableCallback(tipLoop.tipEnableCallback);


        this.tipLoop = tipLoop;
        this.tipLoop.setDialogMonoBehaviour(this);

        string str = this.tipLoop.text;
        string confirmText = this.tipLoop.confirmText;
        string centleText = this.tipLoop.centleText;
        TipDelegate callback = this.tipLoop.dele;
        titlelabel.text = tipLoop.title;
        //   textLabel.setText(str, false, true);
        textLabel.text = str;

        UIEventListener.Get(confirmObject).onClick = onEvent_buttonConfrim;
        UIEventListener.Get(centleObject).onClick = onEvent_buttonCenter;

        //if (confirmText!=null&& centleText != null)
        //{
        //    confirmObject.SetActive(true);
        //    centleObject.SetActive(true);
        //    confirmObject.GetComponentInChildren<UILabel>().text = confirmText;
        //    centleObject.GetComponentInChildren<UILabel>().text = centleText;
        //}
        //else
        //{
        //    confirmObject.SetActive(false);
        //    centleObject.SetActive(false);
        //}


        ////底端两个按钮
        //if (tipLoop.TipType == TipPoolDat.TIP_TYPE_TIP)
        //{
        //    confirmObject = FairyButton.addButton(gameObject, BUTTON_SIZE.NORMAL, tipLoop.button1_Color, Lan.but_confirm, 254, -168, onEvent_buttonConfrim);
        //    centleObject = FairyButton.addButton(gameObject, BUTTON_SIZE.NORMAL, tipLoop.button2_Color, Lan.but_contle, -254, -168, onEvent_buttonCenter);

        //    str = UIEmojiWrapper.ReplaceEmoji(str);

        //    UTools.setActive(textLabel.gameObject, true);
        //    UTools.setActive(datextLabel.gameObject, false);
        //    UTools.setActive(xiaotextLabel.gameObject, false);
        //    UTools.setActive(zuannode.gameObject, false);

        //    textLabel.setText("[ffffff]" + str, false, true);
        //}
        //else if (tipLoop.TipType == TipPoolDat.TIP_TYPE_TIPNoTitle)
        //{
        //    confirmObject = FairyButton.addButton(gameObject, BUTTON_SIZE.BIG, tipLoop.button1_Color, Lan.but_confirm, 254, -168, onEvent_buttonConfrim);
        //    centleObject = FairyButton.addButton(gameObject, BUTTON_SIZE.BIG, tipLoop.button2_Color, Lan.but_contle, -254, -168, onEvent_buttonCenter);

        //    str = UIEmojiWrapper.ReplaceEmoji(str);

        //    UTools.setActive(textLabel.gameObject, true);
        //    UTools.setActive(datextLabel.gameObject, false);
        //    UTools.setActive(xiaotextLabel.gameObject, false);
        //    UTools.setActive(zuannode.gameObject, false);

        //    textLabel.setText("[ffffff]" + str, false, true);
        //}
        //else if (tipLoop.TipType == TipPoolDat.TIP_TYPE_TIPFONT)
        //{
        //    confirmObject = FairyButton.addButton(gameObject, BUTTON_SIZE.BIG, tipLoop.button1_Color, Lan.but_confirm, 230, -263, onEvent_buttonConfrim);
        //    centleObject = FairyButton.addButton(gameObject, BUTTON_SIZE.BIG, tipLoop.button2_Color, Lan.but_contle, -230, -263, onEvent_buttonCenter);


        //    textLabel.fontSize = 26;

        //    textLabel.text = ("[ffffff]" + str);

        //    textLabel.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        //}
        //else if (tipLoop.TipType == TipPoolDat.TIP_TYPE_TWO)
        //{
        //    confirmObject = FairyButton.addButton(gameObject, BUTTON_SIZE.NORMAL, tipLoop.button1_Color, Lan.but_confirm, 254, -168, onEvent_buttonConfrim);
        //    centleObject = FairyButton.addButton(gameObject, BUTTON_SIZE.NORMAL, tipLoop.button2_Color, Lan.but_contle, -254, -168, onEvent_buttonCenter);

        //    string[] strsplit = str.Split('|');

        //    if (strsplit.Length == 2)
        //    {
        //        string str0 = UIEmojiWrapper.ReplaceEmoji(strsplit[0]);
        //        string str1 = UIEmojiWrapper.ReplaceEmoji(strsplit[1]);

        //        UTools.setActive(textLabel.gameObject, false);
        //        UTools.setActive(datextLabel.gameObject, true);
        //        UTools.setActive(xiaotextLabel.gameObject, true);
        //        UTools.setActive(zuannode.gameObject, false);

        //        datextLabel.setText("[ffffff]" + str0, false, true);
        //        xiaotextLabel.setText("[aeaeae]" + str1+ "", false, true);

        //        xiaotextLabel.transform.localPosition = datextLabel.transform.localPosition - new Vector3(0, datextLabel.height + 32, 0);
        //    }
        //    else
        //    {
        //        str = UIEmojiWrapper.ReplaceEmoji(str);

        //        UTools.setActive(textLabel.gameObject, true);
        //        UTools.setActive(datextLabel.gameObject, false);
        //        UTools.setActive(xiaotextLabel.gameObject, false);
        //        UTools.setActive(zuannode.gameObject, false);

        //        textLabel.setText("[ffffff]" + str, false, true);
        //    }


        //}
        //else if (tipLoop.TipType == TipPoolDat.TIP_TYPE_ZUAN)
        //{
        //    confirmObject = FairyButton.addButton(gameObject, BUTTON_SIZE.NORMAL, tipLoop.button1_Color, Lan.but_confirm, 197, -134, onEvent_buttonConfrim);
        //    centleObject = FairyButton.addButton(gameObject, BUTTON_SIZE.NORMAL, tipLoop.button2_Color, Lan.but_contle, -197, -134, onEvent_buttonCenter);

        //    str = UIEmojiWrapper.ReplaceEmoji(str);

        //    UTools.setActive(textLabel.gameObject, true);
        //    UTools.setActive(datextLabel.gameObject, false);
        //    UTools.setActive(xiaotextLabel.gameObject, false);
        //    UTools.setActive(zuannode.gameObject, true);

        //    textLabel.setText("[ffffff]" + str, false, true);

        //    zuanLabel.text = GameGlobal.user.getCrystals()+ "";
        //}






        UTools.setActive(confirmObject.gameObject, false);
        UTools.setActive(centleObject.gameObject, false);

        if (confirmText != null)
        {
            UTools.setActive(confirmObject.gameObject, true);
            confirmObject.GetComponentInChildren<UILabel>().text=confirmText;

            if(centleText == null || centleText.Equals(""))
            {
                UTools.setActive(centleObject.gameObject, false);
                confirmObject.transform.localPosition = new Vector3(0, confirmObject.transform.localPosition.y, confirmObject.transform.localPosition.z);
            }
        }
        if(centleText != null)
        {
            UTools.setActive(centleObject.gameObject, true);

            centleObject.GetComponentInChildren<UILabel>().text = centleText;

            if (confirmText == null || confirmText.Equals(""))
            {
                UTools.setActive(confirmObject.gameObject, false);
                centleObject.transform.localPosition = new Vector3(0, centleObject.transform.localPosition.y, centleObject.transform.localPosition.z);
            }
        }

        
        this.callback = callback;

       // CTUTools.tofan(gameObject);
    }

    private callbackInt tipEnableCallback;
    public void setTipEnableCallback(callbackInt callInt)
    {

        if(callInt == null)
        {
            UTools.setActive(toggle.gameObject, false);
        }
        else
        {
            UTools.setActive(toggle.gameObject, true);

            this.transform.Find("bg/Sprite").GetComponent<UISprite>().height += 40;
            this.transform.Find("bg/bg_wenli").GetComponent<UISprite>().height += 40;

            this.tipEnableCallback = callInt;
        }
    }

	
    public void onEvent_buttonConfrim(GameObject obj)
    {


        if(tipLoop.isAutoClose)
        {
            DialogPool.removeDialog(this.tipLoop);
        }

        if (this.callback != null)
        {
            this.callback(TIP_CONFIRM);
        }

    }

    public void onEvent_buttonCenter(GameObject obj)
    {


        if (tipLoop.isAutoClose)
        {
            DialogPool.removeDialog(this.tipLoop);
        }

        if (this.callback != null)
        {
            this.callback(TIP_CANCEL);
        }
    }



    public void onEvent_buttonClose(GameObject go)
    {


        if (tipLoop.isAutoClose)
        {
            DialogPool.removeDialog(this.tipLoop);
        }

        if (this.callback != null)
        {
            this.callback(TIP_CLOSE);
        }
    }






    void OnDestroy()
    {
        if(tipEnableCallback != null)
        {
            tipEnableCallback(toggle.value ? TIP_ENABLE_SELECTED : TIP_ENABLE_UNSELETED);
        }
    }

}
