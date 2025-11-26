using System;
using System.Collections.Generic;

using UnityEngine;

public class UIProgressBuff : MonoBehaviour
{

    //实际进度条
    public UISprite spr_Forg;
    //预进度条
    public UISprite pre_WeighB;
    public UISprite pre_WeightT;

    public UILabel label_Show;


    public int initForgWidth = 0;


    public float value;
    

    void Awake()
    {

        initForgWidth = spr_Forg.width;
        spr_Forg.type = UIBasicSprite.Type.Filled;
        spr_Forg.fillDirection = UIBasicSprite.FillDirection.Horizontal;

        setValue(0);
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
    }



    private int buffType = 0;
    private int mapId;

    public void setMapInfo(int mapId)
    {
        buffType = 0;
        this.mapId = mapId;
    }



    private int curExp = 0;
    private int goalExp = 0;
    public void setValueBuff(int curExp, int goalExp)
    {
        if(goalExp > curExp && goalExp != 0 && this.curExp > curExp && this.curExp <= goalExp)
        {

        }else
        {
            this.curExp = curExp;
        }

        
        this.goalExp = goalExp;

        if(goalExp <= curExp)
        {
            UTools.setActive(spr_Forg.gameObject, true);
            UTools.setActive(pre_WeighB.gameObject, false);
            UTools.setActive(pre_WeightT.gameObject, false);
        }
    }
/*
    private void update_Buff()
    {
        if (curExp >= goalExp)
        {
            int[] numerator = NetSender.getInstance().user.getExploreMapExp_Numerator(mapId, curExp);
            showText(numerator[0] + "/" + numerator[1]);
            return;
        }

        //宣传的经验条
        if(buffType == 0)
        {
            int mapId = this.mapId;
            //当前进度条的等级及 需要经验值
            int progressLv = NetSender.getInstance().user.getExploreMapLevelFromExp(mapId, curExp);
            int progressMaxExp = Explore_AreaLv.datas[progressLv].AreaEXP[mapId];




            if (curExp < goalExp)
            {
                int zengGap = (goalExp - curExp) / 10;
                int gap = zengGap;
                if(gap > progressMaxExp / 10)
                {
                    gap = progressMaxExp / 10;
                }else if(gap < 1)
                {
                    gap = 1;
                }

                int addA = gap;

                if (curExp + addA >= goalExp)
                {
                    curExp = goalExp;
                }
                else
                {
                    curExp += addA;
                }
            }
            else if (curExp > goalExp)
            {
                int zengGap = (curExp - goalExp) / 10;
                int gap = zengGap;
                if (gap > progressMaxExp / 10)
                {
                    gap = progressMaxExp / 10;
                }
                else if (gap < 1)
                {
                    gap = 1;
                }

                int addA = gap;

                if (curExp - addA <= goalExp)
                {
                    curExp = goalExp;
                }
                else
                {
                    curExp -= addA;
                }
            }

            int maxLv = Explore_AreaLv.datas[Explore_AreaLv.datas.Count - 1].ID;
            //正经的lv 和 需要的经验值
            int mapLv = NetSender.getInstance().user.getExploreMapLevel(mapId);
            int mapMaxExp = Explore_AreaLv.datas[mapLv].AreaEXP[mapId];

            //当前进度条的等级及 需要经验值
            progressLv = NetSender.getInstance().user.getExploreMapLevelFromExp(mapId, curExp);
            progressMaxExp = Explore_AreaLv.datas[progressLv].AreaEXP[mapId];


           
            if (progressLv <= mapLv)
            {
                UIUntils.setActive(spr_Forg.gameObject, true);
                UIUntils.setActive(pre_WeighB.gameObject, true);
                UIUntils.setActive(pre_WeightT.gameObject, false);

                //
                pre_WeighB.fillAmount = (float)NetSender.getInstance().user.getExploreMapPercent(mapId, curExp) / 100;

                int[] numerator = NetSender.getInstance().user.getExploreMapExp_Numerator(mapId, curExp);
                showText(numerator[0] + "/" + numerator[1]);
            }
            else
            {
                UIUntils.setActive(spr_Forg.gameObject, false);
                UIUntils.setActive(pre_WeighB.gameObject, false);
                UIUntils.setActive(pre_WeightT.gameObject, true);

                pre_WeightT.fillAmount = (float)NetSender.getInstance().user.getExploreMapPercent(mapId, curExp) / 100;

                int[] numerator = NetSender.getInstance().user.getExploreMapExp_Numerator(mapId, curExp);
                showText(numerator[0] + "/" + numerator[1]);
            }
        }
        else
        {
            UIUntils.setActive(spr_Forg.gameObject, true);
            UIUntils.setActive(pre_WeighB.gameObject, false);
            UIUntils.setActive(pre_WeightT.gameObject, false);

        }

    }
    */

    public void showText(string s)
    {
        label_Show.text = s;
    }




    void Update()
    {
       // update_Buff();
    }

    /*
    public int getExploreBuffLv(int mapId, int realExp)
    {
        if(curExp > 0)
        {
            return NetSender.getInstance().user.getExploreMapLevelFromExp(mapId, curExp);
        }else
        {
            return NetSender.getInstance().user.getExploreMapLevelFromExp(mapId, realExp);
        }
    }

*/

    public int getProgressPositionX(float progress)
    {
        return (int)((0 - initForgWidth / 2) + progress * initForgWidth);
    }


  /*  private float addA = 0;
    public void setValueBuff(float buff)
    {
        this.buffValue = buff;
        addA = 0;
        if (buff < value)
        {
            if (pre_WeighB != null || pre_WeightT != null)
            {
                if (pre_WeighB != null)
                    UIUntils.setActive(pre_WeighB.gameObject, false);

                if (pre_WeightT != null)
                    UIUntils.setActive(pre_WeightT.gameObject, false);


            }

            UIUntils.setActive(spr_Forg.gameObject, true);
           


            buffShowValue = 0;
        }
        else
        {
            if (buffShowValue == 0)
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
            UIUntils.setActive(pre_WeighB.gameObject, false);
            UIUntils.setActive(pre_WeightT.gameObject, false);
           


            UIUntils.setActive(spr_Forg.gameObject, true);
            

        }
        else
        {
            
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
                UIUntils.setActive(spr_Forg.gameObject, false);
                UIUntils.setActive(pre_WeightT.gameObject, true);
                UIUntils.setActive(pre_WeighB.gameObject, false);
            }
            else
            {
                UIUntils.setActive(spr_Forg.gameObject, true);
                UIUntils.setActive(pre_WeightT.gameObject, false);
                UIUntils.setActive(pre_WeighB.gameObject, true);

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

    }*/

    public int getProgressWidth()
    {
        return initForgWidth;
    }

    public float getValue()
    {
        return value;
    }

	void OnDestroy(){


        UTools.DestroyMonoRef(ref  label_Show);
        UTools.DestroyMonoRef(ref  pre_WeightT);
        UTools.DestroyMonoRef(ref  pre_WeighB);
        UTools.DestroyMonoRef(ref  spr_Forg);
	}
}

