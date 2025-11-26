using UnityEngine;
using System.Collections;


public enum BUTTON_COLOR
{
    NULL,
    YELLOR,
    RED,
    GRAY,
    NEW
};


public enum BUTTON_SIZE
{
    SMALL,
    SMALL104,
    NORMAL,
    BIG
};


public class FairyButton : FairyButtonNode
{


    private UISprite sprite_New,icon;
    private UILabel label;

    private Animator ani;


    private BoxCollider boxCollider;
    private float sScale;
    private Vector3 sRect = new Vector3();

    void Awake()
    {

        GameObject actualObject = this.transform.Find("Button").gameObject;


        ani = actualObject.GetComponent<Animator>();
        if (ani == null)
        {
            ani = actualObject.AddComponent<Animator>();
        }


        sprite_New = actualObject.transform.Find("newcolor").GetComponent<UISprite>();
        icon = actualObject.transform.Find("icon").GetComponent<UISprite>();
        label = actualObject.transform.Find("Label").GetComponent<UILabel>();

        UIEventListener.Get(gameObject).onPress = onEvent_Press;

        add_But_ClickColor(actualObject);

        //SoundEventer.add_But_ClickSound(gameObject);



        sScale = this.transform.localScale.x;

        boxCollider = this.GetComponent<BoxCollider>();
        Vector3 tempBox = boxCollider.size;
        sRect = new Vector3(tempBox.x, tempBox.y, tempBox.z);

    }
    void Start()
    {
       // CTUTools.tofan(gameObject);
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


    //增加事件
    public void addEventMethod(UIEventListener.VoidDelegate clickMethod)
    {
        UIEventListener.Get(gameObject).onClick = clickMethod;
    }

    private void playPressDown()
    {
        ani.enabled = true;
        Object obj = Resources.Load("AnimatorController/ButtonPressController") as Object;
        RuntimeAnimatorController controller = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);
        ani.runtimeAnimatorController = controller;

    }

    private void playReleaseUp()
    {
        ani.enabled = true;
        Object obj = Resources.Load("AnimatorController/ButtonReleaseController") as Object;
        RuntimeAnimatorController controller = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);

        ani.runtimeAnimatorController = controller;
    }

    void Update()
    {
        if (isPressDown)
        {
            boxCollider.size = new Vector3(sRect.x / 0.9f/* transform.localScale.x*/, sRect.y / 0.9f /*transform.localScale.y*/, 1);
        }
    }


    private bool isPressDown = false;
    private void onEvent_Press(GameObject go, bool state)
    {
        if (state)
        {
            isPressDown = true;
            playPressDown();
            boxCollider.size = new Vector3(sRect.x / 0.9f/* transform.localScale.x*/, sRect.y / 0.9f /*transform.localScale.y*/, 1);
        }
        else
        {
            isPressDown = false;
            playReleaseUp();
            boxCollider.size = new Vector3(sRect.x, sRect.y, 1);
        }
    }



    public void setText(string text)
    {
        label.text = text;
       
    }

    public BUTTON_COLOR color = BUTTON_COLOR.NULL;
    public BUTTON_SIZE size;

    public void setColor(BUTTON_COLOR color)
    {
        setColor(color, size);
    }
    string[] colorsprname = {"btm_0","btm_3", "icon_kuojianwan", "icon_cha" };
    public void setColor(BUTTON_COLOR color, BUTTON_SIZE size)
    {


        if (this.color == color) return;

        this.color = color;
        this.size = size;
        if (color == BUTTON_COLOR.RED)
        {
            sprite_New.spriteName = colorsprname[0];
            icon.spriteName = colorsprname[2];
        }
        else if (color == BUTTON_COLOR.YELLOR)
        {
            sprite_New.spriteName = colorsprname[1];
            icon.spriteName = colorsprname[3];
        }
        else if (color == BUTTON_COLOR.GRAY)
        {

        }
        else if (color == BUTTON_COLOR.NEW)
        {

        }

    }



    public static FairyButton addButton(GameObject preObj, BUTTON_SIZE butSize, BUTTON_COLOR color, string text, int x, int y, UIEventListener.VoidDelegate clickMethod)
    {


        GameObject obj = null;

        if (butSize == BUTTON_SIZE.SMALL)
        {
            obj = ResManager.getGameObject("allpre", "ui/exui/But_Small");
        }
        else if (butSize == BUTTON_SIZE.SMALL104)
        {
            obj = ResManager.getGameObject("allpre", "ui/exui/But_Small104");
        }
        else if (butSize == BUTTON_SIZE.NORMAL)
        {
            obj = ResManager.getGameObjectNoInit("allpre", "ui/exui/But_Mid178");
        }
        else if (butSize == BUTTON_SIZE.BIG)
        {
            obj = ResManager.getGameObject("allpre", "ui/exui/But_Big237");
        }

        obj = NGUITools.AddChild(preObj, obj);
        UTools.setActive(obj, true);
        obj.transform.localPosition = new Vector3(x, y, 0);

        FairyButton but = obj.GetComponent<FairyButton>();

        if (clickMethod != null)
        {
            but.addEventMethod(clickMethod);
        }

        but.setText(text);
        but.setColor(color, butSize);

        return but;
    }




    void OnDestroy()
    {


        UTools.DestroyMonoRef(ref label);
    }
}
