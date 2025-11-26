using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Vector3 begin, end;
    UIWidget w;
    public int bindex, eindex;
    void Awake()
    {

        w = gameObject.GetComponent<UIWidget>();

    }
    public void setpoint(Vector3 b, Vector3 e, int lw)
    {
        begin = b;
        end = e;


        gameObject.transform.localPosition = b;
        w.width = (int)Math.Ceiling(Vector3.Distance(b, e));
        w.height = lw;

       Vector3 v1, v2, v3;
        v1 = e - b;
        v2 = Vector3.right;
        v3 = Vector3.Cross(v2, v1);
        float du = Vector3.Angle(v1, v2);
        du = v3.z > 0 ? du : (360 - du);
        gameObject.transform.localEulerAngles = new Vector3(0, 0, du);
    }
    public void setpoint(Vector3 b, int lw, int lh)
    {

        gameObject.transform.localPosition = b;
        w.width = lw;
        w.height = lh;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setdepth(int dp)
    {
        w.depth = dp;
    }

    internal void setalpha(float v)
    {
        w.alpha = v;
    }
}
