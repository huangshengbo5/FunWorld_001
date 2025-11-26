using System;
using System.Collections.Generic;

using UnityEngine;

public class UIProgressPanel : MonoBehaviour
{

    public UISprite spr_Forg;
    public UISprite spr_Star;

    public UIPanel panel;

    public float starOffset = 0;//偏移五像素

    public int initForgWidth = 0;

    void Awake()
    {
        UIPanel present = this.GetComponentInParent<UIPanel>();
        if (present != null && panel != null)
        {
            panel.depth = present.depth + 1;
        }
        //将其他panel放到上层
        UIPanel[] panels = this.GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != panel && present != null)
            {
                panels[i].depth = present.depth + 2;
            }
        }

        initForgWidth = spr_Forg.width;

        panel.clipping = UIDrawCall.Clipping.SoftClip;
        panel.baseClipRegion = new Vector4(0, 0, initForgWidth, panel.baseClipRegion.w);

        spr_Forg.type = UIBasicSprite.Type.Filled;
        spr_Forg.fillDirection = UIBasicSprite.FillDirection.Horizontal;

        setValue(0);
    }

    void Start()
    {

    }


    public float value;

    public void setValue(float value)
    {
        this.value = value;

        //float showValue = value * 0.85f + 0.15f;
        float showValue = value;
        if (showValue < 0)
            showValue = 0;
        if (showValue > 1)
            showValue = 1;

        /*
                if(value <= 0)
                {
                    UIUntils.setActive(spr_Star.gameObject, false);
                }else
                {
                    UIUntils.setActive(spr_Star.gameObject, true);
                }*/

        spr_Forg.fillAmount = showValue;

        int goalWidth = (int)(showValue * initForgWidth);
        //spr_Forg.width = goalWidth;

        if (spr_Star != null)
        {
            if (value < 0.1f)
            {
                spr_Star.transform.localScale = new Vector3(1, value * 10, 1);
            }
            else if (value > 0.9f)
            {
                spr_Star.transform.localScale = new Vector3(1, (1 - value) * 5 + 0.5f, 1);
            }
            else
            {
                spr_Star.transform.localScale = new Vector3(1, 1, 1);
            }

            spr_Star.transform.localPosition = new Vector3(spr_Forg.transform.localPosition.x - initForgWidth / 2 + goalWidth + starOffset, spr_Forg.transform.localPosition.y, spr_Forg.transform.localPosition.z);
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

    /*   void Update()
       {
           setValue(value);
       }*/

	void OnDestroy(){


        UTools.DestroyMonoRef(ref  spr_Star);
        UTools.DestroyMonoRef(ref  spr_Forg);
	}
}

