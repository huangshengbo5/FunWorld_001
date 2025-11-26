using UnityEngine;
using System.Collections;



public class FairyButtonClose : FairyButtonNode
{
    RuntimeAnimatorController pressController;
    RuntimeAnimatorController releaseController;

    Animator ani;



    private BoxCollider boxCollider;
    private float sScale;
    private Vector3 sRect = new Vector3();


    void Awake()
    {

        GameObject actualObject = this.transform.Find("CloseButton").gameObject;

        ani = actualObject.GetComponent<Animator>();
        if (ani == null)
        {
            ani = actualObject.AddComponent<Animator>();
        }


        Object obj = Resources.Load("AnimatorController/ButtonPressController") as Object;
        pressController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);
        obj = Resources.Load("AnimatorController/ButtonReleaseController") as Object;
        releaseController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);




        UIEventListener.Get(gameObject).onPress += onEvent_Press;
       // SoundEventer.add_But_ClickClose(gameObject);

        add_But_ClickColor(actualObject);

        sScale = this.transform.localScale.x;

        boxCollider = this.GetComponent<BoxCollider>();
        Vector3 tempBox = boxCollider.size;
        sRect = new Vector3(tempBox.x, tempBox.y, tempBox.z);
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



    void Start()
    {

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





    public static FairyButtonClose addButton(GameObject preObj, int x, int y, UIEventListener.VoidDelegate clickMethod)
    {
        GameObject obj = ResManager.getGameObject("allpre", "ui/exui/CloseButton").gameObject;
        obj = NGUITools.AddChild(preObj, obj);
        UTools.setActive(obj, true);
        obj.transform.localPosition = new Vector3(x, y, 0);

        FairyButtonClose but = obj.GetComponent<FairyButtonClose>();

        if (clickMethod != null)
        {
            but.addEventMethod(clickMethod);
        }


        return but;
    }





}
