using System;
using System.Collections.Generic;

using UnityEngine;

public class UIProgress : MonoBehaviour
{

    //预进度条
    public UISprite pre_WeighB;
    public UISprite pre_WeightT;
    public UISprite pre_Star;
    private float buffValue;
    private float buffShowValue;

    //实际进度条
    public UISprite spr_Forg;
    public UISprite spr_Star;
    
    public float starOffset = 0;//偏移五像素

    public int initForgWidth = 0;

    void Awake()
    {

        initForgWidth = spr_Forg.width;
        spr_Forg.type = UIBasicSprite.Type.Filled;
        spr_Forg.fillDirection = UIBasicSprite.FillDirection.Horizontal;

        if(spr_Star != null)
        {
            spr_Star.type = UIBasicSprite.Type.Filled;
            spr_Star.flip = UIBasicSprite.Flip.Nothing;
            spr_Star.fillDirection = UIBasicSprite.FillDirection.Horizontal;
            spr_Star.invert = true;
        }
        
        setValue(0);
    }

    void Start()
    {

    }


    public float value;

    void Update()
    {
        update_Buff();
    }
    public void setValue(float value)
    {
        this.value = value;
        
        float showValue = value;
        if (showValue < 0)
            showValue = 0;
        if (showValue > 1)
            showValue = 1;
        

        spr_Forg.fillAmount = showValue;

        int goalWidth = (int)(showValue * initForgWidth);


        if(spr_Star != null && value < 1)
        {
            UTools.setActive(spr_Star.gameObject, true);

            //缩放
            float scaleY = 1;
            if(value < 0.1f)
            {
                scaleY = value * 10;
            }
            else if(value > 0.9f)
            {
                scaleY = (1 - value) * 5 + 0.5f;
            }
            if (scaleY < 0) scaleY = 0;
            if (scaleY > 1) scaleY = 1;
            spr_Star.transform.localScale = new Vector3(1, scaleY, 1);
            //位置
            float goalX = spr_Forg.transform.localPosition.x - initForgWidth / 2 + goalWidth + starOffset;
            spr_Star.transform.localPosition = new Vector3(goalX, spr_Forg.transform.localPosition.y, spr_Forg.transform.localPosition.z);

            //星星的fillamount
            float goalAmount = (float)(goalWidth + starOffset /*- (spr_Star.width/2 + starOffset)*/) / spr_Star.width;
            goalAmount = goalAmount > 1 ? 1 : goalAmount;
            spr_Star.fillAmount = goalAmount;
        }else
        {
            if(spr_Star != null)
            {
                UTools.setActive(spr_Star.gameObject, false);
            }
        }
    }

    public int getProgressPositionX(float progress)
    {
        return (int)((0 - initForgWidth / 2) + progress*initForgWidth); 
    }


    private float addA = 0;
    public void setValueBuff(float buff)
    {
        this.buffValue = buff;
        addA = 0;
        if (buff < value)
        {
            if (pre_WeighB != null || pre_WeightT != null)
            {
                if(pre_WeighB != null)
                    UTools.setActive(pre_WeighB.gameObject, false);

                if(pre_WeightT != null)
                    UTools.setActive(pre_WeightT.gameObject, false);



                if(pre_Star != null)
                {
                    UTools.setActive(pre_Star.gameObject, false);
                }
            }

            UTools.setActive(spr_Forg.gameObject, true);
            if (spr_Star != null)
            {
                UTools.setActive(spr_Star.gameObject, true);
            }

            buffShowValue = 0;
        }
        else
        {
            if(buffShowValue == 0)
            {
                buffShowValue = value;
            }
        }

    }

    public float getbuffShowValue()
    {
        return buffShowValue;
    }


    private void update_Buff()
    {
        if (buffValue < value && buffValue != 0 || pre_WeighB == null || pre_WeightT == null)
        {
            return;
        }

        if (buffShowValue <= value)
        {
            UTools.setActive(pre_WeighB.gameObject, false);
            UTools.setActive(pre_WeightT.gameObject, false);
            if (pre_Star != null)
            {
                UTools.setActive(pre_Star.gameObject, false);
            }

            UTools.setActive(spr_Forg.gameObject, true);
            if (spr_Star != null)
            {
                UTools.setActive(spr_Star.gameObject, true);
            }
        }
        else
        {
            
            if (spr_Star != null)
            {
                UTools.setActive(spr_Star.gameObject, false);
            }
            
            if (pre_Star != null)
            {
                UTools.setActive(pre_Star.gameObject, true);
            }


            ///buff的星星
            float showValue = buffShowValue % 1;


            if (buffShowValue > 1)
            {
                if (showValue == 0)
                {
                    pre_WeightT.fillAmount = 1;
                }
                else
                {
                    pre_WeightT.fillAmount = showValue;
                }
                UTools.setActive(spr_Forg.gameObject, false);
                UTools.setActive(pre_WeightT.gameObject, true);
                UTools.setActive(pre_WeighB.gameObject, false);
            }
            else
            {
                UTools.setActive(spr_Forg.gameObject, true);
                UTools.setActive(pre_WeightT.gameObject, false);
                UTools.setActive(pre_WeighB.gameObject, true);

                pre_WeighB.fillAmount = showValue;
                if (buffShowValue > 0 && showValue == 0)//满格的情况
                {
                    pre_WeighB.fillAmount = 1;
                }
                else
                {
                    pre_WeighB.fillAmount = showValue;
                }
            }



            int goalWidth = (int)(showValue * initForgWidth);
            if (pre_Star != null && pre_Star.gameObject.activeSelf)
            {
                float scaleY = 1;
                if (value < 0.1f)
                {
                    scaleY = value * 10;
                }
                else if (value > 0.9f)
                {
                    scaleY = (1 - value) * 5 + 0.5f;
                }

                if (scaleY < 0) scaleY = 0;
                if (scaleY > 1) scaleY = 1;

                pre_Star.transform.localScale = new Vector3(1, scaleY, 1);

                float goalX = spr_Forg.transform.localPosition.x - initForgWidth / 2 + goalWidth + starOffset;

                pre_Star.transform.localPosition = new Vector3(goalX, spr_Forg.transform.localPosition.y, spr_Forg.transform.localPosition.z);


                float goalAmount = (float)(goalWidth - starOffset - (pre_Star.width / 2 + starOffset)) / pre_Star.width;
                goalAmount = goalAmount > 1 ? 1 : goalAmount;
                pre_Star.fillAmount = goalAmount;
            }



        }

        if (buffShowValue < buffValue)
        {
            addA = (addA > 0.2f) ? 0.2f : addA + 0.02f;

            if (buffShowValue + addA >= buffValue)
            {
                buffShowValue = buffValue;
            }
            else
            {
                buffShowValue += addA;
            }
        }
        else if (buffShowValue > buffValue)
        {
            addA = (addA > 0.2f) ? 0.2f : addA + 0.02f;

            if (buffShowValue - addA <= buffValue)
            {
                buffShowValue = buffValue;
            }
            else
            {
                buffShowValue -= addA;
            }
        }

    }

    public int getProgressWidth()
    {
        return initForgWidth;
    }

    public float getValue()
    {
        return value;
    }

	void OnDestroy(){


        UTools.DestroyMonoRef(ref  spr_Star);
        UTools.DestroyMonoRef(ref  spr_Forg);
        UTools.DestroyMonoRef(ref  pre_Star);
        UTools.DestroyMonoRef(ref  pre_WeightT);
        UTools.DestroyMonoRef(ref  pre_WeighB);
	}
}

