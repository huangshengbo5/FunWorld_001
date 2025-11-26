using UnityEngine;
using System.Collections;



public delegate void pushButtonHandler(GameObject obj, bool flag);


public class UIPushButton : MonoBehaviour {


    public event pushButtonHandler onSelectedChange;

    public bool isSelected = false;

    public GameObject selected_GameObject;
    public GameObject unselect_GameObject;

    public UILabel label_Title;

    public int id;

    void Awake()
    {

        UIEventListener.Get(gameObject).onClick = onEvent_but_Switch;
        setSelectedState(false);
    }

    void Start () {
	
	}
	
    public void setTitle(string text)
    {
        if(label_Title != null)
            label_Title.text = text;
    }

    public UILabel getTitleLabel()
    {
        return label_Title;
    }

    public void setSelectedState(bool flag)
    {
        this.isSelected = flag;

        if(flag)
        {
            selected_GameObject.SetActive(true);
            unselect_GameObject.SetActive(false);
        }else
        {
            selected_GameObject.SetActive(false);
            unselect_GameObject.SetActive(true);
        }
    }

    private void onEvent_but_Switch(GameObject obj)
    {
        setSelectedState(!isSelected);

        if(onSelectedChange != null)
            onSelectedChange(gameObject, isSelected);
    }

	void OnDestroy(){


        UTools.DestroyMonoRef(ref  label_Title);
	}
}
