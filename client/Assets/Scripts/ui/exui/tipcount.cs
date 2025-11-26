using UnityEngine;


public class tipcount : MonoBehaviour
{

    public UILabel label_Count;

    private int count;

    //tippoint
    void Awake()
    {
        UTools.setActive(gameObject, false);
    }

    void Start()
    {

    }


    public void setCount(int count)
    {

        this.count = count;

        if (gameObject == null || label_Count == null) return;

        if (count == -1)
        {
            UTools.setActive(gameObject, true);
            label_Count.text = "New";
        }
        else
        if (count > 0)
        {
            UTools.setActive(gameObject, true);
            label_Count.text = ""+count;

            if (count > 99)
            {
                gameObject.GetComponent<UISprite>().width = 70;
            }
            else
            {
                gameObject.GetComponent<UISprite>().width = 60;
            }

        }
        else
        {
            UTools.setActive(gameObject, false);
        }
    }
    

    public void setNewFlag(bool flag)
    {

        if (flag)
        {
            UTools.setActive(gameObject, true);
            label_Count.text = "!";
        }
        else
        {
            UTools.setActive(gameObject, false);
        }
    }


    public int getCount()
    {
        return count;
    }


	void OnDestroy(){


        UTools.DestroyMonoRef(ref  label_Count);

	}
}
