using UnityEngine;

using System.Collections;
using System;
using System.Collections.Generic;


public class LoopScrollBar : MonoBehaviour
{

    public Transform fore;
    public Transform back;

    private Vector2 leftRightRect = new Vector2();
    private Vector2 topDownRect = new Vector2();
    void Awake()
    {
        
        UIWidget backWidget = back.GetComponent<UIWidget>();
        float backX = backWidget.transform.localPosition.x;
        float backWidth = backWidget.localSize.x;

        float backY = backWidget.transform.localPosition.y;
        float backHeight = backWidget.localSize.y;

        UIWidget forgWidget = fore.GetComponent<UIWidget>();
        float forgWidth = forgWidget.localSize.x;


        leftRightRect.x = backX - backWidth / 2;// + forgWidth / 2;
        leftRightRect.y = backX + backWidth / 2;// - forgWidth / 2;

        topDownRect.x = backY + backHeight / 2;// + forgWidth / 2;
        topDownRect.y = backY - backHeight / 2;// - forgWidth / 2;
        
        //Debug.Log("totleLength ====    "+ moveRect.x + "   "+ moveRect.y);
    }



    public void updataBar(List<LoopItemData> datasList, LoopItemObject firstItem, LoopScrollView loopScrollView)
    {

        if(loopScrollView.arrangeDirection == LoopScrollView.ArrangeDirection.Left_to_Right || loopScrollView.arrangeDirection == LoopScrollView.ArrangeDirection.Right_to_Left)
        {
            float totleLength = datasList.Count * firstItem.widget.localSize.x - loopScrollView.totlePanelWidth;

            float xx2 = loopScrollView.transform.localPosition.x;

            float progress = (loopScrollView.initLocalX - xx2) / totleLength;
            setHorProgress(progress);
            /*float goalPos = ((loopScrollView.initLocalX - xx2) / totleLength) * (leftRightRect.y - leftRightRect.x) + leftRightRect.x;

            if (goalPos < leftRightRect.x) goalPos = leftRightRect.x;
            if (goalPos > leftRightRect.y) goalPos = leftRightRect.y;

            fore.localPosition = new Vector3(goalPos, fore.localPosition.y, fore.localPosition.z);*/
        }
        else
        {

            float totleLength = datasList.Count * firstItem.widget.localSize.y - loopScrollView.totlePanelWidth;

            float yy2 = loopScrollView.transform.localPosition.y;

            /*
                        float goalPos = topDownRect.x - ((yy2 - loopScrollView.initLocalY) / totleLength) * (topDownRect.x - topDownRect.y);

                        if (goalPos > topDownRect.x) goalPos = topDownRect.x;
                        if (goalPos < topDownRect.y) goalPos = topDownRect.y;

                        fore.localPosition = new Vector3(fore.localPosition.x, goalPos, fore.localPosition.z);*/

            float progress = (yy2 - loopScrollView.initLocalY) / totleLength;
            setVerticalProgress(progress);
        }

    }



    public void updataBar(UIScrollView scrollView, float cellHeight, int itemCount, float offset, float zoneLength)
    {

        if (scrollView.canMoveHorizontally)
        {
            float totleLength = itemCount * cellHeight - zoneLength;

            float xx2 = offset;

            //float goalPos = ((offset) / totleLength) * (leftRightRect.y - leftRightRect.x) + leftRightRect.x;
            float progress = (offset) / totleLength;
/*
            if (goalPos < leftRightRect.x) goalPos = leftRightRect.x;
            if (goalPos > leftRightRect.y) goalPos = leftRightRect.y;

            fore.localPosition = new Vector3(goalPos, fore.localPosition.y, fore.localPosition.z);*/

            setHorProgress(progress);
        }
        else if(scrollView.canMoveVertically)
        {

            float totleLength = itemCount * cellHeight - zoneLength;

            float xx2 = offset;

            float progress = (-offset) / totleLength;
          /*  float goalPos = topDownRect.x - ((-offset) / totleLength) * (topDownRect.x - topDownRect.y);

            if (goalPos > topDownRect.x) goalPos = topDownRect.x;
            if (goalPos < topDownRect.y) goalPos = topDownRect.y;

            fore.localPosition = new Vector3(fore.localPosition.x, goalPos, fore.localPosition.z);*/

            setVerticalProgress(progress);
        }

    }


    public void setVerticalProgress(float value)
    {
        float goalPos = topDownRect.x - (value) * (topDownRect.x - topDownRect.y);

        if (goalPos > topDownRect.x) goalPos = topDownRect.x;
        if (goalPos < topDownRect.y) goalPos = topDownRect.y;

        fore.localPosition = new Vector3(fore.localPosition.x, goalPos, fore.localPosition.z);
    }

    public void setHorProgress(float value)
    {
        float goalPos = value * (leftRightRect.y - leftRightRect.x) + leftRightRect.x;

        if (goalPos < leftRightRect.x) goalPos = leftRightRect.x;
        if (goalPos > leftRightRect.y) goalPos = leftRightRect.y;

        fore.localPosition = new Vector3(goalPos, fore.localPosition.y, fore.localPosition.z);
    }
}
