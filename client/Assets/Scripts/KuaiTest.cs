using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuaiTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * 90;
        gotoAndStop(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string[] colorstr = new string[9]
{

        "[ffffff]",
                "[000000]",
                        "[00ff00]",
        "[05df48]",
        "[00eaff]",
        "[cc7bff]",
        "[fff600]",
        "[ff85a5]",
        "[0000ff]",
};

    public void gotoAndStop(int v)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = NGUIText.ParseColor(colorstr[v],1);
        transform.localScale = Vector3.one * 90;
    }

    public void SetActive(bool v)
    {
        gameObject.SetActive(v);
    }

    public void gotoAndPlay(int v)
    {
        TweenScale ts = gameObject.GetComponent<TweenScale>();
        if (ts == null)
        {
            ts = gameObject.AddComponent<TweenScale>();
            ts.from = Vector3.one *80;
            ts.to = Vector3.one ;
            ts.duration = 0.2f;
            ts.ResetToBeginning();

        }
        ts.onFinished.Clear();
        ts.PlayForward();

        ts.AddOnFinished(() =>
        {
            SetActive(false);
        });
    }
}
