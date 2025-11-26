using UnityEngine;
using System.Collections;



public class FairyImgButton : FairyButtonNode
{

    RuntimeAnimatorController pressController;
    RuntimeAnimatorController releaseController;

    Animator ani;

    private BoxCollider boxCollider;
    private float sScale;
    private Vector3 sRect = new Vector3();


    void Awake()
    {

        ani = this.GetComponent<Animator>();
        if (ani == null)
        {
            ani = this.gameObject.AddComponent<Animator>();
        }
        Object obj = Resources.Load("AnimatorController/ButtonPressController") as Object;
        pressController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);
        obj = Resources.Load("AnimatorController/ButtonReleaseController") as Object;
        releaseController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);


        UIEventListener.Get(gameObject).onPress += onEvent_Press;

        //颜色
        add_But_ClickColor(gameObject);

        //声音 
      //  SoundEventer.add_But_ClickClose(gameObject);


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



    void Update()
    {
        if(isPressDown)
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
            boxCollider.size = new Vector3(sRect.x / 0.9f/* transform.localScale.x*/, sRect.y /0.9f /*transform.localScale.y*/, 1);
        }
        else
        {
            isPressDown = false;
            playReleaseUp();
            boxCollider.size = new Vector3(sRect.x, sRect.y, 1);
        }
    }

    




}
