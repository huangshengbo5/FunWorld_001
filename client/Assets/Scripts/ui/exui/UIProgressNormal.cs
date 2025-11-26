using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class UIProgressNormal : MonoBehaviour
{


    public UISprite spr_Forg;
    public UISprite spr_Pre;
    public UILabel label_Progress;
    
    public int initForgWidth = 0;

    public float value;

    bool fillflag = false;

    void Awake()
    {
        
        initForgWidth = spr_Forg.width;
        if (fillflag)
        {
            spr_Forg.type = UIBasicSprite.Type.Filled;
            spr_Forg.fillDirection = UIBasicSprite.FillDirection.Horizontal;
        }


        setValue(0.2f);
    }

    void Start()
    {

    }

    public void setValue(float value)
    {
        this.value = value;

     
        float showValue = value;
        if (showValue < 0)
            showValue = 0;
        if (showValue > 1)
            showValue = 1;
        if (spr_Forg.type == UIBasicSprite.Type.Filled)
        {
            spr_Forg.fillAmount = showValue;
        }
        else
        {
            spr_Forg.enabled = true;
            spr_Forg.drawRegion = new Vector4(0f, 0f, showValue, 1f);
            if (showValue < 0.001f)
            {
                spr_Forg.enabled = false;
            }
        }
      

    }


    private float preShowProgress = -1;
    private float baseShowValue = -1;
    public void setPreShow(float preShowValue, float value)
    {
        this.preShowProgress = preShowValue;
        this.baseShowValue = value;
        setValue(baseShowValue);
    }

    void Update()
    {
        if(preShowProgress != -1)
        {
            if(baseShowValue+0.01f < preShowProgress)
            {
                baseShowValue += 0.01f;
            }
            else
            {
                baseShowValue = preShowProgress;
                preShowProgress = -1;
            }
            setValue(baseShowValue);
        }
    }

    public int getProgressPositionX(float progress)
    {
        return (int)((0 - initForgWidth / 2) + progress * initForgWidth);
    }


    public int getProgressWidth()
    {
        return initForgWidth;
    }

    public float getValue()
    {
        return value;
    }


    public void showText(string str)
    {
        if(label_Progress != null)
        {
            label_Progress.text = str;
        }
    }




	void OnDestroy(){


        UTools.DestroyMonoRef(ref  label_Progress);
        UTools.DestroyMonoRef(ref  spr_Forg);
	}
}

