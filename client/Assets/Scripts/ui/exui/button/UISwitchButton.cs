using UnityEngine;
using System.Collections;



public delegate void switchButtonHandler(GameObject obj, bool flag);


public class UISwitchButton : MonoBehaviour {


    public event switchButtonHandler onSelectedChange;

    public bool isON = false;

    public GameObject off_GameObject;
    public GameObject on_GameObject;


    private bool isInit = false;

    void Awake()
    {
        if(!isInit)
        {
            isInit = true;

            UIEventListener.Get(gameObject).onClick = onEvent_but_Switch;

            setSelectedState(false);
        }

    }

    void Start () {
	
	}



	
    public void setSelectedState(bool flag)
    {
        Awake();

        this.isON = flag;

        if(flag)
        {
            on_GameObject.SetActive(true);
            off_GameObject.SetActive(false);
        }else
        {
            on_GameObject.SetActive(false);
            off_GameObject.SetActive(true);
        }
    }

    private void onEvent_but_Switch(GameObject obj)
    {
        setSelectedState(!isON);

        if(onSelectedChange != null)
            onSelectedChange(gameObject, isON);
    }

}
