using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public GameObject[] clouds = new GameObject[4];
    public TweenPosition tp;
    void Start()
    {
        settype(CTTools.rd.Next(0, 3));
        tp = gameObject.GetComponent<TweenPosition>();

        begin();

    }
    static bool firstflag = true;
    private void begin()
    {
        Vector3 bv = new Vector3(-1700 - CTTools.rd.Next(1700), 686 + CTTools.rd.Next(400), 0);
        if (firstflag)
        {
            bv = new Vector3(0, 686 + CTTools.rd.Next(400), 0);
            firstflag = false;
        }
        tp.from = bv;
        tp.to = bv+new Vector3(3400 + CTTools.rd.Next(1700),0 , 0);
        tp.duration = 60 + CTTools.rd.Next(90);
        tp.delay = 0.6f;
        tp.ResetToBeginning();
        tp.PlayForward();

        EventDelegate.Add(tp.onFinished, delegate ()
        {
            settype(CTTools.rd.Next(0, 3));

            begin();
        });
    }

    private void settype(int v)
    {
        for(int i = 0; i < clouds.Length; i++)
        {
            if (i == v)
            {
                clouds[i].SetActive(true);
            }
            else
            {
                clouds[i].SetActive(false);
            }
         
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
