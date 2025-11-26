using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuaLaTest : MonoBehaviour//划拉
{
    public enum slideVector { nullVector, up, down, left, right };
    private Vector2 touchFirst = Vector2.zero; 
    private Vector2 touchSecond = Vector2.zero;
    private slideVector currentVector = slideVector.nullVector;

    public Action leftaction, rightaction;

    private long pressUpTime = 0;
    private long pressDownTime = 0;
    private long lastDragTime = 0;
    private float speed = 0;
    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(gameObject).onPress = onpress;
        UIEventListener.Get(gameObject).onDrag = ondrag;
    }

    public void setaction(Action la,Action ra)
    {
        leftaction = la;
        rightaction = ra;
    }

    private void ondrag(GameObject go, Vector2 delta)
    {
       // touchSecond = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);

       // timer += Time.deltaTime;  //计时器

        //if (timer > offsetTime)
        //{
        //    Vector3 position1 = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
        //    touchSecond = gameObject.transform.InverseTransformPoint(position1);

        //    Vector2 slideDirection = touchFirst - touchSecond;
        //    float x = slideDirection.x;
        //    float y = slideDirection.y;
        //  //  Debug.Log("drag  " + x);
        //    if (y + SlidingDistance < x && y > -x - SlidingDistance)
        //    {

        //        if (currentVector == slideVector.left)
        //        {
        //            return;
        //        }

        //        Debug.Log("left");

        //        currentVector = slideVector.left;
        //    }
        //    else if (y > x + SlidingDistance && y < -x - SlidingDistance)
        //    {
        //        if (currentVector == slideVector.right)
        //        {
        //            return;
        //        }

        //        Debug.Log("right");

        //        currentVector = slideVector.right;
        //    }
        //    else if (y > x + SlidingDistance && y - SlidingDistance > -x)
        //    {
        //        if (currentVector == slideVector.down)
        //        {
        //            return;
        //        }

        //        Debug.Log("down");

        //        currentVector = slideVector.down;
        //    }
        //    else if (y + SlidingDistance < x && y < -x - SlidingDistance)
        //    {
        //        if (currentVector == slideVector.up)
        //        {
        //            return;
        //        }

        //        Debug.Log("up");

        //        currentVector = slideVector.up;
        //    }

        //    timer = 0;
        //    touchFirst = touchSecond;
        //}

        lastDragTime = UTools.currentTimeMillis();
    }

    private void onpress(GameObject go, bool state)
    {
        if (state)
        {
            Vector3 position1 = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);

            touchFirst = gameObject.transform.InverseTransformPoint(position1);

            pressDownTime = UTools.currentTimeMillis();
            speed = 0;
        }
        else
        {
            pressUpTime = UTools.currentTimeMillis();

            Vector3 position1 = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
            touchSecond = gameObject.transform.InverseTransformPoint(position1);

            //滑动松开的时候 触发
            long clickTime = (int)(pressUpTime - pressDownTime);
            long releaseTime = pressUpTime - lastDragTime;

            float dis = 0;
            dis = touchSecond.x - touchFirst.x;

            float speed = dis / clickTime;
            if (Mathf.Abs(dis) > 15)
            {
                if (Mathf.Abs(speed) > 0.8f && clickTime < 1800 && releaseTime < 200)
                {
                    this.speed = (dis) / 28 * (1000 / (clickTime + 1));
                    this.speed *= 2f;//速度倍率
                //    Debug.Log(speed);

                    if (speed < 0)
                    {
                        currentVector = slideVector.left;

                        if (leftaction != null)
                        {
                            leftaction();
                        }
                    }
                    else
                    if (speed > 0)
                    {
                        currentVector = slideVector.right;

                        if (rightaction != null)
                        {
                            rightaction();
                        }
                    }
                   
                }
                else
                {
                    this.speed = 0;// 普通动作
                    currentVector = slideVector.nullVector;
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
