using UnityEngine;
using System.Collections;
using System.Globalization;

public enum BUTTON_CURRENCY
{
    COINS,
    CRYSTALS,
    FRIEND,
    CASH,
    POWER,

    FamilyCoin,
    Fight,
    FamilyXZ
};


public enum BUTTON_TYPE
{
    PURCHES,
    NORMAL,
    SMALL186,
};

public class FairyButtonCurrency : FairyButtonNode
{

    private UISprite sprite_Zi;
    private UISprite sprite_Yel;
    private UISprite sprite_Friend;
    private UISprite sprite_Cash;
    private UISprite sprite_Power;
    private UISprite sprite_FamilyCoins;
    private UISprite sprite_Fight;
    private UISprite sprite_FamilyXZ;

    //private UISprite spriteBar;

    private UILabel label_Price;
    private UILabel label;



    RuntimeAnimatorController pressController;
    RuntimeAnimatorController releaseController;


    private Animator ani;


    void Awake()
    {


        GameObject actualObject = this.transform.Find("Button").gameObject;

        ani = actualObject.GetComponent<Animator>();
        if (ani == null)
        {
            ani = actualObject.AddComponent<Animator>();
        }
        Object obj = Resources.Load("AnimatorController/ButtonPressController") as Object;
        pressController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);
        obj = Resources.Load("AnimatorController/ButtonReleaseController") as Object;
        releaseController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);




        sprite_Zi = actualObject.transform.Find("Background_Zi").GetComponent<UISprite>();
        sprite_Yel = actualObject.transform.Find("Background_Yel").GetComponent<UISprite>();
        sprite_Friend = actualObject.transform.Find("Background_F").GetComponent<UISprite>();
        sprite_Cash = actualObject.transform.Find("Background_Cash").GetComponent<UISprite>();

        sprite_Power = actualObject.transform.Find("Background_Power").GetComponent<UISprite>();

        sprite_FamilyCoins = actualObject.transform.Find("Background_FamilyCoins").GetComponent<UISprite>();

        sprite_Fight = actualObject.transform.Find("Background_Fight").GetComponent<UISprite>();
        sprite_FamilyXZ = actualObject.transform.Find("Background_FamilyXZ").GetComponent<UISprite>();

        //spriteBar = actualObject.transform.Find("bar").GetComponent<UISprite>();

        label = actualObject.transform.Find("Label").GetComponent<UILabel>();
        label_Price = actualObject.transform.Find("labelPrice").GetComponent<UILabel>();

        UIEventListener.Get(gameObject).onPress += onEvent_Press;


        add_But_ClickColor(actualObject);

        //SoundEventer.add_But_ClickSound(gameObject);
    }


    void OnEnable()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        ani.enabled = false;
        //Debug.Log("OnEnable====" + gameObject.name);
    }

    void OnDisable()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        ani.enabled = false;
        //Debug.Log("OnDisable====" + gameObject.name);
    }



    public void switchShortCurrency()
    {
        //label.transform.localPosition = new Vector3(43, 0, 0);
    }

    public void setText(string text)
    {
        label.text = text;
    }


    public void setPrice(float price)
    {
        if(butCurrcy == BUTTON_CURRENCY.CASH)
        {
           // label_Price.text = "￥" + FormulaHandle.getExchangedPrice(price, FormulaHandle.CURRENCY_CNY);


            label_Price.overflowMethod = UILabel.Overflow.ResizeFreely;


            label_Price.transform.localPosition = new Vector3(-34, 0, 0);
        }
        else if (butCurrcy == BUTTON_CURRENCY.FRIEND)
        {
            label_Price.text = price+"";
            label_Price.transform.localPosition = new Vector3(-26, 0, 0);
        }
        else if (butCurrcy == BUTTON_CURRENCY.CRYSTALS)
        {
            label_Price.text = price + "";
            label_Price.transform.localPosition = new Vector3(-26, 0, 0);
        }
        else
        {


            string s1 = UTools.toShortCount((int)price);

            label_Price.text = s1;
            label_Price.transform.localPosition = new Vector3(-26, 0, 0);
        }
    }

    public void setPriceColor(Color color)
    {
        label_Price.color = color;
    }

    public void setColor(int currencyType)
    {
        //if (currencyType == Statics.TYPE_CRYSTALS_PAY)
        //{
        //    setColor(butType, BUTTON_CURRENCY.CRYSTALS);
        //}
        /* else if (currencyType == Statics.TYPE_FRIENDSHIP)
         {
             setColor(butType, BUTTON_CURRENCY.FRIEND);
         }
         else if (currencyType == Statics.)
         {
             setColor(butType, BUTTON_CURRENCY.CRYSTALS);
         }*/
    }

    private BUTTON_TYPE butType;
    private BUTTON_CURRENCY butCurrcy;

    public void setColor(BUTTON_TYPE butType,BUTTON_CURRENCY butCurrcy)
    {
        this.butType = butType;
        this.butCurrcy = butCurrcy;
        if (butCurrcy == BUTTON_CURRENCY.COINS)
        {
            UTools.setActive(sprite_Yel.gameObject, true);
            sprite_Yel.SetNormal();
            UTools.setActive(sprite_Zi.gameObject, false);
            UTools.setActive(sprite_Friend.gameObject, false);
            UTools.setActive(sprite_Cash.gameObject, false);
            UTools.setActive(sprite_Power.gameObject, false);
            UTools.setActive(sprite_FamilyCoins.gameObject, false);
            UTools.setActive(sprite_Fight.gameObject, false);
            UTools.setActive(sprite_FamilyXZ.gameObject, false);
            UTools.setLabelEffect(label, 12, butType == BUTTON_TYPE.PURCHES ? 28 : 28);


        }
        else if (butCurrcy == BUTTON_CURRENCY.CRYSTALS)
        {
            UTools.setActive(sprite_Yel.gameObject, false);
            UTools.setActive(sprite_Zi.gameObject, true);
            sprite_Zi.SetNormal();
            UTools.setActive(sprite_Friend.gameObject, false);
            UTools.setActive(sprite_Cash.gameObject, false);
            UTools.setActive(sprite_Power.gameObject, false);
            UTools.setActive(sprite_FamilyCoins.gameObject, false);
            UTools.setActive(sprite_Fight.gameObject, false);
            UTools.setActive(sprite_FamilyXZ.gameObject, false);
            UTools.setLabelEffect(label, 12, butType == BUTTON_TYPE.PURCHES ? 28 : 28);
        }
        else if (butCurrcy == BUTTON_CURRENCY.FRIEND)
        {
            UTools.setActive(sprite_Yel.gameObject, false);
            UTools.setActive(sprite_Zi.gameObject, false);
            UTools.setActive(sprite_Friend.gameObject, true);
            sprite_Friend.SetNormal();
            UTools.setActive(sprite_Cash.gameObject, false);
            UTools.setActive(sprite_Power.gameObject, false);
            UTools.setActive(sprite_FamilyCoins.gameObject, false);
            UTools.setActive(sprite_Fight.gameObject, false);
            UTools.setActive(sprite_FamilyXZ.gameObject, false);
            UTools.setLabelEffect(label, 12, butType == BUTTON_TYPE.PURCHES ? 28 : 28);
        }
        else if (butCurrcy == BUTTON_CURRENCY.CASH)
        {
            UTools.setActive(sprite_Yel.gameObject, false);
            UTools.setActive(sprite_Zi.gameObject, false);
            UTools.setActive(sprite_Friend.gameObject, false);
            UTools.setActive(sprite_Cash.gameObject, true);
            UTools.setActive(sprite_Power.gameObject, false);
            UTools.setActive(sprite_FamilyCoins.gameObject, false);
            UTools.setActive(sprite_Fight.gameObject, false);
            UTools.setActive(sprite_FamilyXZ.gameObject, false);
            sprite_Cash.SetNormal();
            UTools.setLabelEffect(label, 12, butType == BUTTON_TYPE.PURCHES ? 28 : 28);
        }
        else if (butCurrcy == BUTTON_CURRENCY.POWER)
        {
            UTools.setActive(sprite_Yel.gameObject, false);
            UTools.setActive(sprite_Zi.gameObject, false);
            UTools.setActive(sprite_Friend.gameObject, false);
            UTools.setActive(sprite_Cash.gameObject, false);
            UTools.setActive(sprite_Power.gameObject, true);
            UTools.setActive(sprite_FamilyCoins.gameObject, false);
            UTools.setActive(sprite_Fight.gameObject, false);
            UTools.setActive(sprite_FamilyXZ.gameObject, false);
            sprite_Power.SetNormal();
            UTools.setLabelEffect(label, 12, butType == BUTTON_TYPE.PURCHES ? 28 : 28);
        }
        else if (butCurrcy == BUTTON_CURRENCY.FamilyCoin)
        {
            UTools.setActive(sprite_Yel.gameObject, false);
            UTools.setActive(sprite_Zi.gameObject, false);
            UTools.setActive(sprite_Friend.gameObject, false);
            UTools.setActive(sprite_Cash.gameObject, false);
            UTools.setActive(sprite_Power.gameObject, false);
            UTools.setActive(sprite_FamilyCoins.gameObject, true);
            UTools.setActive(sprite_Fight.gameObject, false);
            UTools.setActive(sprite_FamilyXZ.gameObject, false);
            sprite_FamilyCoins.SetNormal();
            UTools.setLabelEffect(label, 12, butType == BUTTON_TYPE.PURCHES ? 28 : 28);
        }
        else if (butCurrcy == BUTTON_CURRENCY.Fight)
        {
            UTools.setActive(sprite_Yel.gameObject, false);
            UTools.setActive(sprite_Zi.gameObject, false);
            UTools.setActive(sprite_Friend.gameObject, false);
            UTools.setActive(sprite_Cash.gameObject, false);
            UTools.setActive(sprite_Power.gameObject, false);
            UTools.setActive(sprite_FamilyCoins.gameObject, false);
            UTools.setActive(sprite_Fight.gameObject, true);
            UTools.setActive(sprite_FamilyXZ.gameObject, false);
            sprite_Fight.SetNormal();
            UTools.setLabelEffect(label, 12, butType == BUTTON_TYPE.PURCHES ? 28 : 28);

        }
        else if (butCurrcy == BUTTON_CURRENCY.FamilyXZ)
        {
            UTools.setActive(sprite_Yel.gameObject, false);
            UTools.setActive(sprite_Zi.gameObject, false);
            UTools.setActive(sprite_Friend.gameObject, false);
            UTools.setActive(sprite_Cash.gameObject, false);
            UTools.setActive(sprite_Power.gameObject, false);
            UTools.setActive(sprite_FamilyCoins.gameObject, false);
            UTools.setActive(sprite_Fight.gameObject, false);
            UTools.setActive(sprite_FamilyXZ.gameObject, true);
            sprite_FamilyXZ.SetNormal();
            UTools.setLabelEffect(label, 12, butType == BUTTON_TYPE.PURCHES ? 28 : 28);

        }
    }

    public void setGray()
    {
        UTools.setLabelEffect(label, 41, 28);
        if(sprite_Yel.gameObject.activeSelf)
        {
            sprite_Yel.SetGray();
        }
        if (sprite_Zi.gameObject.activeSelf)
        {
            sprite_Zi.SetGray();
        }
        if (sprite_Friend.gameObject.activeSelf)
        {
            sprite_Friend.SetGray();
        }
        if (sprite_Cash.gameObject.activeSelf)
        {
            sprite_Cash.SetGray();
        }
        if (sprite_Power.gameObject.activeSelf)
        {
            sprite_Power.SetGray();
        }
        if (sprite_FamilyCoins.gameObject.activeSelf)
        {
            sprite_FamilyCoins.SetGray();
        }
        if (sprite_Fight.gameObject.activeSelf)
        {
            sprite_Fight.SetGray();
        }
        if (sprite_FamilyXZ.gameObject.activeSelf)
        {
            sprite_FamilyXZ.SetGray();
        }
    }


    //增加事件
    public void addEventMethod(UIEventListener.VoidDelegate clickMethod)
    {
        UIEventListener.Get(gameObject).onClick += clickMethod;
    }
    private void playPressDown()
    {
        ani.enabled = true;
        //         Object obj = Resources.Load("AnimatorController/ButtonPressController") as Object;
        //         RuntimeAnimatorController controller = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);
        ani.runtimeAnimatorController = pressController;
    }

    private void playReleaseUp()
    {
        //         Object obj = Resources.Load("AnimatorController/ButtonReleaseController") as Object;
        //         RuntimeAnimatorController controller = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);

        ani.enabled = true;
        ani.runtimeAnimatorController = releaseController;
    }

    private void onEvent_Press(GameObject go, bool state)
    {
        if (state)
        {
            playPressDown();
        }
        else
        {
            playReleaseUp();
        }
    }







    public static FairyButtonCurrency addButton(GameObject preObj, BUTTON_TYPE butType, BUTTON_CURRENCY butCurrcy, string text, int x, int y, UIEventListener.VoidDelegate clickMethod, bool isShortCurrency = false)
    {
        GameObject obj = null;
        if(butType == BUTTON_TYPE.PURCHES)
        {
            obj = ResManager.getGameObject("allpre", "ui/exui/Button_Cucy_Small").gameObject;
        }
        else if(butType == BUTTON_TYPE.NORMAL)
        {
            obj = ResManager.getGameObject("allpre", "ui/exui/Button_Cucy_Normal").gameObject;
        }
        else if (butType == BUTTON_TYPE.SMALL186)
        {
            obj = ResManager.getGameObject("allpre", "ui/exui/Button_Cucy_Small186").gameObject;
        }

        obj = NGUITools.AddChild(preObj, obj);
        obj.transform.localPosition = new Vector3(x, y, 0);
        UTools.setActive(obj, true);

        FairyButtonCurrency but = obj.GetComponent<FairyButtonCurrency>();

        if (clickMethod != null)
        {
            but.addEventMethod(clickMethod);
        }

        but.setColor(butType, butCurrcy);
        but.setText(text);


        if(isShortCurrency)
        {
            but.switchShortCurrency();
        }

        return but;
    }

	void OnDestroy(){


        UTools.DestroyMonoRef(ref  label_Price);
        UTools.DestroyMonoRef(ref sprite_Power);
        UTools.DestroyMonoRef(ref sprite_FamilyCoins);
        UTools.DestroyMonoRef(ref sprite_FamilyXZ);
        UTools.DestroyMonoRef(ref  sprite_Cash);
        UTools.DestroyMonoRef(ref  sprite_Friend);
        UTools.DestroyMonoRef(ref  sprite_Yel);
        UTools.DestroyMonoRef(ref  sprite_Zi);
	}
}
