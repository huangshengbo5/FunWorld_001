using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDoorUI : DialogMonoBehaviour
{
    public static SpecialDoorUI instance;
    // Start is called before the first frame update
    public GameObject close;

    private void Awake()
    {
        instance = this;
        UIEventListener.Get(close.gameObject).onClick = closeDialog;
        SoundEventer.add_But_ClickSound(close.gameObject);


    }
    public void OnTianQiActiveBtnCkick(GameObject go)
    {
        HttpManager.instance.TianqiShow((code1) =>
        {
            if (code1 == Callback.SUCCESS)
            {
                UIManager.showTianqiActivity();
            }
        });
    }
}
