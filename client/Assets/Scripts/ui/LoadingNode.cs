using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingNode : MonoBehaviour
{
    public int progress;
    public UILabel label;
    public UIProgressBar bar;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        progress+= step;
        bar.value = progress * 1.0f / 100;
        label.text = progress + "";
        if (progress == 50)
        {
            act50();
        }else if (progress == 100)
        {
          //  act100?.Invoke();
            act100();
        }
        else if (progress > 100)
        {
            gameObject.SetActive(false);
           // GameGlobal.loadgen.SetActive(false);
        }
    }
    public int step;
    Action act50,act100;
    public void setstart(Action act,int step=1)
    {
        this.step = step;
        act50 = act;
        gameObject.SetActive(true);
        progress = 0;
       // GameGlobal.loadgen.SetActive(true);
    }
    public void setend(Action act)
    {
        act100 = act;

        // GameGlobal.loadgen.SetActive(true);
    }
}
