using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APaintGroupSpine : MonoBehaviour
{
    public APaintNodeSpine[] renpaintnode = new APaintNodeSpine[5];
    public void create1(GameObject agroot, string v2, string v3,float scale=1.0f)
    {
        gameObject.transform.SetParent(agroot.gameObject.transform);
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        create2(2, v2, v3,scale);

    }
    public void create2(int v1, string v2, string v3, float scale = 1.0f)
    {


        GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
        renpaintnode[v1] = texframeobjs.GetComponent<APaintNodeSpine>();
        renpaintnode[v1].create1(gameObject, v2, v3, scale);


    }
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void setAlpha(int v)
    {
        for(int i=0;i< renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].setAlpha(v);
            }
        }
    }

    public bool isCurrEnd()
    {
        return renpaintnode[2].isCurrEnd;
    }
    public void logic(float delta)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].logic(delta);
            }
        }
        //if (renpaintnode[3] != null && renpaintnode[3].gameObject.activeSelf)
        //{
        //    if (renpaintnode[3].isCurrEnd)
        //    {
        //        renpaintnode[3].gameObject.SetActive(false);
        //    }
        //}
    }

    public void playAction2(string nowActionStr, bool loop)
    {
        //for (int i = 0; i < renpaintnode.Length; i++)
        //{
        //    if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
        //    {
        //        renpaintnode[i].playAction2(nowActionStr,loop);
        //    }
        //}

        renpaintnode[2].playAction2(nowActionStr, loop);


        if (fightxgflag)
        {

            if (renpaintnode[3] != null)
            {
                renpaintnode[3].playAction2ftxg(renpaintnode[2].attachname + "/" + nowActionStr, loop);
            }
            //if (renpaintnode[1] != null)
            //{
            //    renpaintnode[1].playAction2ftxg(renpaintnode[2].attachname + "/" + nowActionStr+"_D", loop);
            //}
        }

        if (jjxgflag)
        {

            if (renpaintnode[3] != null)
            {
                renpaintnode[3].playAction2ftxg(nowActionStr + "_A", loop);
            }
            if (renpaintnode[1] != null)
            {
                renpaintnode[1].playAction2ftxg(nowActionStr + "_B", loop);
            }
        }

    }

    public void playAction2force(string nowActionStr, bool loop)
    {
        //for (int i = 0; i < renpaintnode.Length; i++)
        //{
        //    if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
        //    {
        //        renpaintnode[i].playAction2(nowActionStr,loop);
        //    }
        //}

        renpaintnode[2].playAction2Force2(nowActionStr, loop);

        if (fightxgflag)
        {
            if (renpaintnode[3] != null)
            {
                renpaintnode[3].playAction2ftxg(renpaintnode[2].attachname + "/" + nowActionStr, loop);
            }

            //if (renpaintnode[1] != null)
            //{
            //    renpaintnode[1].playAction2ftxg(renpaintnode[2].attachname + "/" + nowActionStr + "_D", loop);
            //}
        }

        if (jjxgflag)
        {

            if (renpaintnode[3] != null)
            {
                renpaintnode[3].playAction2ftxg(nowActionStr + "_A", loop);
            }
            if (renpaintnode[1] != null)
            {
                renpaintnode[1].playAction2ftxg(nowActionStr + "_B", loop);
            }
        }


    }



    public void setguang(bool v1, bool v2=false)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].setguang(v1,v2);
            }
        }
    }

    public void setguangcolor(int v)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].setguangcolor(v);
            }
        }
    }

    public void setdepth(int v)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].setdepth(v+i);
            }
        }
    }

    public void setSkin(int monitorID, int v)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null&& renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].setSkin(monitorID,v);
            }
        }
    }

    public void change(string v1, string v2)
    {
        //for (int i = 0; i < renpaintnode.Length; i++)
        //{
        //    if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
        //    {
        //        renpaintnode[i].change(v1, v2);
        //    }
        //}
        renpaintnode[2].change(v1, v2);
    }
    bool fightxgflag = false;
    public void addfightxg()
    {
        fightxgflag = true;
        create2(3, "Effect_Fight_PK", "Effect_Fight_PK");
       // create2(1, "Effect_Fight_PK", "Effect_Fight_PK");
    }
    bool jjxgflag = false;
    public void addjjxg()
    {
        jjxgflag = true;
        create2(3, "Skin_Items", "Skin_Items");
        create2(1, "Skin_Items", "Skin_Items");
    }

    public void playAction(int v1, bool v2)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].playAction(v1, v2);
            }
        }
    }

    

    public void setlv(bool v)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].setlv(v);
            }
        }
    }
    public void setColor(int r, int g, int b, int alpha)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].setColor(r,g,b,alpha);
            }
        }
    }
    public void setColor(int r, int g, int b)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].setColor(r, g, b);
            }
        }
    }
    public void setColor(Color col)
    {
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].setColor(col);
            }
        }
    }
    public void setColorNOXG(Color col)
    {
        renpaintnode[2].setColor(col);
    }
    public void setAutoAlpha(bool v)
    {
        renpaintnode[2].setAutoAlpha(v);

    }
    public void addEvent(Spine.AnimationState.TrackEntryEventDelegate handleEvent)
    {
        renpaintnode[2].zhengmian.animation.state.Event += handleEvent;
    }
    public Vector3 localScale;
    public void setScale(Vector3 vector3)
    {
        localScale = vector3;
        for (int i = 0; i < renpaintnode.Length; i++)
        {
            if (renpaintnode[i] != null && renpaintnode[i].gameObject.activeSelf)
            {
                renpaintnode[i].gameObject.transform.localScale = vector3;
            }
        }
    }

    public void setdong(bool v)
    {
        renpaintnode[2].setdong(v);

        if (v)
        {
            for (int i = 0; i < renpaintnode.Length; i++)
            {
                if (i!=2&&renpaintnode[i] != null )
                {
                   UTools.setActive(renpaintnode[i].zhengmian.gameObject,false);
                }
            }
        }
    }
}
