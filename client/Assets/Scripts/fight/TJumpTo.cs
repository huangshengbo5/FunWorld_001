using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TJumpTo : MonoBehaviour
{
    public static float g = 6f;
    public static float mocaa = 1.4f;
    public static float tanli = 0.15f;
    public float groundy, aidavx, aidavy;
    public GameObject hroot;

    public bool overflag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float delay = 0;
    public void logic(float delta)
    {
        bool tempflag = false;
        delay += delta;
        if (delay < 0.033f)
        {
            //  return;
            tempflag = false;
        }
        else
        {
            delay = 0;
            tempflag = true;
        }


        float bili = delta / 0.033f;

        if (noyflag)
        {
            if (this.aidavx > 0)
            {
                gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(-this.aidavx * hroot.transform.localScale.x*bili, 0, 0);
                if(delay==0)
                this.aidavx -= mocaa;
                if (this.aidavx < 0)
                {
                    this.aidavx = 1.5f;
                    overflag = true;
                }              
            }
        }
        else
        {
            if (this.aidavx > 0)
            {
                gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(-this.aidavx * hroot.transform.localScale.x*bili, 0, 0);
                if (delay == 0)
                    this.aidavx -= mocaa;
                if (this.aidavx < 0)
                {
                    this.aidavx = 1.3f;
                }
                hroot.transform.localPosition = hroot.transform.localPosition + new Vector3(0, this.aidavy*bili, 0);
                //   Debug.LogError(agroot.transform.localPosition);
                if (delay == 0)
                    this.aidavy -= g ;
                if (hroot.transform.localPosition.y <= this.groundy)
                {
                    hroot.transform.localPosition = Vector3.zero;
                    this.aidavy = (-this.aidavy * tanli);
                    if (Math.Abs(this.aidavy) < 3)
                    {
                        this.aidavy = 0;
                        this.aidavx = 0;
                        overflag = true;
                    }
                }

            }
        }

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public bool noyflag = false;
    public void setv(int v1, int v2,bool noyflag=false)
    {
          this.aidavx = v1;
         this.aidavy = v2;
        overflag = false;
        this.noyflag = noyflag;
    }

    internal void sethroot(GameObject agroot)
    {
        hroot = agroot;
    }
}
