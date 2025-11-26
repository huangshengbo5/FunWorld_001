using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProgressNew : MonoBehaviour
{
    public GameObject icon;
    public UISprite hongtiao, lvtiao;
    public int now,next;
    public UILabel label;
    public List<int> maxlist = new List<int>();
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (now != next&&step!=0)
        {
            now += step;
            if (now > next)
            {
                now = next;
            }
            updataValue(now);
        }
    }

    public void setlist(List<int> maxlist)
    {
        this.maxlist = maxlist;
    }
    int add;
    public void setAddValue(int v)
    {
        add = v;
        updataValue(now);
    }
    public void addCurValue(int v)
    {
        if (now != next)
        {
           now=next;
        }

        next = now + v;

        step = (high - low) / 10;
    }
    public void setCurValue(int v)
    {
        now = v;
        next = now;
        updataValue(now);
    }
    
    public int low, high,step;
    public void updataValue(int v)
    {
       
        index = maxlist.Count;
        for(int i = 0; i < maxlist.Count; i++)
        {
            if (v < maxlist[i])
            {
                index = i-1;
                break;
            }
        }
        
        low = 0;



        float value = 0;
        if (index == maxlist.Count)
        {
            low = maxlist[maxlist.Count-1];

            high = int.MaxValue;

            value = 1;
            label.text = "MAX";
        }
        else
        {
            low = maxlist[index];

            high = maxlist[index+1];

            value = (v - low) * 1.0f / (maxlist[index+1] - low);

            if (add == 0)
            {
                label.text = (now - low) + "/" + (maxlist[index+1] - low);
            }
            else
            {
                label.text ="[00ff00] "+(add+next - low) + " [ffffff]"+ "/" + (maxlist[index+1] - low);
            }

           // label.text = (v - low) + "/" + (maxlist[index] - low);

        }
        setValue(value);
    }
    float value;
    public void setValue(float value)
    {
        this.value = value;


        float showValue = value;
        if (showValue < 0)
            showValue = 0;
        if (showValue > 1)
            showValue = 1;

        if (hongtiao.type == UIBasicSprite.Type.Filled)
        {
            hongtiao.fillAmount = showValue;
        }
        else
        {
            hongtiao.enabled = true;
            hongtiao.drawRegion = new Vector4(0f, 0f, showValue, 1f);
            if (showValue < 0.001f)
            {
                hongtiao.enabled = false;
            }
        }
    }


}
