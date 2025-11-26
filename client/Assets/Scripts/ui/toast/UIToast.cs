using UnityEngine;
using System.Collections;




//Toast缓存
public class ToastPoolDat : BasePoolDat
{
    public string content;

    public ToastPoolDat(string str)
    {
        this.content = str;
    }

}





public class UIToast : DialogMonoBehaviour {

    private UILabel textlabel;
    private UIWidget bgWidget;
    private TweenPosition tweenPosition;

    private ToastPoolDat pooldat;

    void Awake()
    {

        textlabel = this.transform.Find("Label").GetComponent<UILabel>();
        bgWidget = this.transform.Find("Sprite").GetComponent<UIWidget>();

        tweenPosition = this.GetComponent<TweenPosition>();
        tweenPosition.AddOnFinished(finish);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }


    public void setText(ToastPoolDat pooldat)
    {
        this.pooldat = pooldat;
        this.pooldat.setDialogMonoBehaviour(this);

        string content = this.pooldat.content;

        textlabel.text = content;
        textlabel.ProcessText();
     //   bgWidget.width = textlabel.width + 35;
    }
	
    public void finish()
    {
        // ToastPool.removeToast(this.pooldat);

        MonoBehaviour.Destroy(gameObject);
    }

	void OnDestroy(){


		
	}
}
