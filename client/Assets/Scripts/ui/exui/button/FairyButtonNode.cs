using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FairyButtonNode : GridItem
{
    

    private List<TweenColor> tweenColors = new List<TweenColor>();

    private UIWidget[] spr = null;
    private Color[] sprColor = null;//原来的颜色

    #region 按钮的颜色改变
    public void add_But_ClickColor(GameObject gameObject)
    {
        UIEventListener.Get(gameObject).onPress += onEvent_but_Color;

        if (spr == null)
        {
            spr = gameObject.GetComponentsInChildren<UIWidget>();
            sprColor = new Color[spr.Length];
        }

    }

    public void resetspr()
    {
        spr = gameObject.GetComponentsInChildren<UIWidget>();
        sprColor = new Color[spr.Length];
        tweenColors.Clear();


    }

    public void onEvent_but_Color(GameObject obj, bool state)
    {
        //if (state)
        //    resetspr();

        if (tweenColors.Count < 1)
        {
            //检索出 或 添加tweenColor
            for (int i = 0; i < spr.Length; i++)
            {
                TweenColor col = spr[i].GetComponent<TweenColor>();
                if (col == null)
                {
                    col = spr[i].gameObject.AddComponent<TweenColor>();
                }
                tweenColors.Add(col);
            }


            //获取初始化的颜色
            for (int i = 0; i < spr.Length; i++)
            {
                sprColor[i] = new Color(spr[i].color.r, spr[i].color.g, spr[i].color.b);
            }
        }


        if (state)
        {
            for (int i = 0;  i < tweenColors.Count; i++)
            {
                spr[i].color = Color.white;
                tweenColors[i].from = Color.white;
                tweenColors[i].to = new Color(0.69f * sprColor[i].r, 0.69f* sprColor[i].g, 0.69f* sprColor[i].b);
                tweenColors[i].duration = 0.1f;
                tweenColors[i].PlayForward();
            }
        }else
        {
            for (int i = 0; i < tweenColors.Count; i++)
            {
                tweenColors[i].ResetToBeginning();
                spr[i].color = new Color(0.69f * sprColor[i].r, 0.69f * sprColor[i].g, 0.69f * sprColor[i].b);
                tweenColors[i].from = new Color(0.69f * sprColor[i].r, 0.69f * sprColor[i].g, 0.69f * sprColor[i].b);
                tweenColors[i].to = new Color(sprColor[i].r, sprColor[i].g, sprColor[i].g);
                tweenColors[i].duration = 0.2f;
                tweenColors[i].PlayForward();
            }
        }
    }

    #endregion



    

}
