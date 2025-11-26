using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
  
    public enum DR
    {
        dr_up = 0,
        dr_upright = 1,
        dr_right = 2,
        dr_downright = 3,
        dr_down = 4,
        dr_downleft = 5,
        dr_left = 6,
        dr_upleft = 7,
    };
    public const int aik_astar = 0;
    public const int aik_closeup = 2;
    public const int aik_moveaway = 3;
    public const int aik_random = 1;
    public const int aik_follow = 4;


    public virtual void create(int id)
    {
        this.id = id;


        over = false;
        useflag = true;

        pIndex = 0;
        movejiao = 0;
        aikind = 0;
        sta = 0;
        laststa = 0;
        movex = 0;
        movey = 0;



        //  hight = CTTools.rd.Next(1000) * 0.001f;

        // Debug.Log("hight=" + hight);
    }
    Role target;
    public void setTarget(Role r)
    {
        target = r;
    }

    public virtual void setdepth(int v)
    {
        dp = v;
    }
    public virtual void setSelect(bool v)
    {

    }

    public int id;

    public int cellkind;
    public int bigkind;
    public int smallkind;
    public int otherkind;
    public int lastcellx;
    public int lastcelly;
    public int cellx;
    public int celly;
    public float v, vbackup;
    public float vx;
    public float vy;
    public int width;
    public float height = 60;
    public float cellwhou;
    public float cellhhou;
    public int cr;
    public int sta;
    public int laststa;
    public bool over;
    public int direct;
    public float movejiao;
    public int rotx;
    public int zbvisible;
    public int high;

    public int cellw, cellh;

    public int nextbigkind = -1;
    public int caiid = -1;
    public int wantcaiid = -1;
    public int likecaiid = -1;
    public int xingbie = -1;

    public int fx;
    public int area;

    public int backcellx, backcelly, backcellw, backcellh, backfx;
    public int yuanshicellw, yuanshicellh;

    public int shantocellx, shantocelly;
    public bool shantoupflag;

    public int dp;
    public bool upflag;
    public int upkind = 0;
    public bool upflagchangflag = false;


    public bool upingflag = false;
    public bool autoflag = false;

    public int delay;

    public bool fengflag = false;
    public bool tanflag = false;


    public bool pauseflag = false;

    public bool useflag = false;

    public int tempceng0 = 200000;
    public int tempceng = 300000;

    public List<Vector2> path = new List<Vector2>();

    public Vector3[] bianlist = new Vector3[6];
    public Vector3[] bianlist1 = new Vector3[4];
    public List<GameObject> bianobjlist = new List<GameObject>();


    public GameObject xgroot;



    int aikind = 0;
    public void setAIStyle(int aik)
    {
        aikind = aik;//0astar 1random
    }

    public int pIndex;
    public bool zhaobudao = false;

    public int idx;



    public float movex;
    public float movey;
    public void move()
    {



        if (exMove())
        {

        }
        else
        {



            if (isStand())
            {
                autoMove();
            }

            bool movele = false;


            setv();

            //Debug.Log(vx+" "+vy+" "+movex+""+movey);

            float orgx = this.getPositionX1();
            float orgy = this.getPositionY1();
            float x = this.getPositionX1();
            float y = this.getPositionY1();
            float movexbackup = movex;
            float moveybackup = movey;
            if (movex != 0)
            {
                x += vx;
                movex -= vx;
                movele = true;
            }
            if (movey != 0)
            {
                y += vy;
                movey -= vy;
                movele = true;
            }
            if (Math.Abs(movex) < Math.Abs(v))
            {
                movex = 0;
                if (v > 60)
                    x = cellx * 60;
            }
            if (Math.Abs(movey) < Math.Abs(v))
            {
                movey = 0;
                if (v > 60)
                    y = celly * 60;
            }


            if (movele)
            {
                laststa = sta;
                sta = 1;
                bool canMove = canmove(orgx, orgy, x, y);
                if (canMove)
                {
                    this.setPosition1(new Vector2(x, y));
                }
            }
            else
            {
                sta = 0;
                stopact?.Invoke();
                stopact = null;
            }
        }
    }
    public Action stopact;
    public virtual void resetv()
    {

    }
    public void setv()
    {
        if (!isStand())
        {
            float movejiaotemp = getJiao(movex, movey);
            vx = (float)(Math.Sin(CTTools.degreestoradians(movejiaotemp)) * v);
            vy = (float)(Math.Cos(CTTools.degreestoradians(movejiaotemp)) * v);

            if (id == 10)
            {
                vx = 12;
                if (movey > 0)
                {
                    vy = 5;
                }
                else if (movey < 0)
                {
                    vy = -5;
                }

            }
            //vx=abs(sinf(CC_DEGREES_TO_RADIANS(movejiao))*v);
            //vy=abs(cosf(CC_DEGREES_TO_RADIANS(movejiao))*v);
        }
        else
        {
            vx = 0;
            vy = 0;
        }
    }
    public bool canmove(float yx, float yy, float nx, float ny)
    {
        return true;
    }
    public bool isStand()
    {
        if (movex != 0 || movey != 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public virtual void autoMove()
    {


        if (path.Count > 0 && pIndex < path.Count)
        {
            float nextx = (path[pIndex]).x;
            float nexty = (path[pIndex]).y;



            float rx = getPositionX1();
            float ry = getPositionY1();

            movex = nextx  - rx;
            movey = nexty  - ry;


            movejiao = getJiao(movex, movey);
            setDirect(getDirectByJiao(movejiao));

            //log("role->pIndex=%d:%f,%f",role->pIndex,role->path[role->pIndex].x,role->path[role->pIndex].y);
            lastcellx = cellx;
            lastcelly = celly;
            cellx = (int)(path[pIndex]).x;
            celly = (int)(path[pIndex]).y;
            pIndex++;




            //	CCLOG("AstarAI::move()");
        }
        else
        {
            switch (aikind)
            {
                case aik_astar:

                    break;
                case aik_random:

                    int nx = cellx + CTTools.rd.Next(-3, 4);
                    int ny = celly + CTTools.rd.Next(-3, 4);

                    //int neimenx = (int)JiaJu.menpointarray[0].x;
                    //int neimeny = (int)JiaJu.menpointarray[0].y;
                    //int neimenx1 = (int)JiaJu.menpointarray[1].x;
                    //int neimeny1 = (int)JiaJu.menpointarray[1].y;

                    //if (nx >= CellData.DOWNBEGINX && ny >= CellData.DOWNBEGINY && nx <= CellData.DOWNENDX && ny < CellData.DOWNENDY)
                    //{
                    //    if (!JiaJu.indoorcell(this, nx, ny))
                    //    {
                    //        if (PMap.dataArray[nx, ny].inorout == -1 && PMap.dataArray[nx, ny].szhi == 0)
                    //        {
                    //            PathSearchInfo.AStar(this, nx, ny, -1);
                    //        }
                    //    }
                    //}
                    break;
            }
        }

    }
    public virtual void setAlpha(int v)
    {

    }
    public virtual bool exMove()
    {
        return false;
    }
    public bool visible = true;
    public virtual void aida()
    {
       
    }




    public void destroyself()
    {

        Destroy(gameObject);
    }
    public void setActive(bool param1)
    {
        gameObject.SetActive(param1);
        visible = param1;
        


    }
   
    public bool collideWith(Role role, int type0, int type1)
    {
        return false;
    }



    public static int[] fxduiying = { 0, 0, 0, 0, 0, 0, 0, 0 };
    public static int[] fxrotx = { 0, 0, 0, 0, 1, 1, 1, 1 };
    public static int[] fxzb = { 1, 0, 0, 0, 0, 1, 1, 1 };


    public void setDirect(int d)
    {
        direct = d;

    }
    public float getJiao(float mx, float my)
    {
        float jiao = CTTools.radianstodegrees(Math.Atan2(mx, my));
        if (jiao < 0)
        {
            jiao += 360.0f;
        }
        return jiao;
    }
    public float getJiao(int dx, int dy, int mx, int my)
    {
        float jiao = CTTools.radianstodegrees(Math.Atan2(mx - dx, my - dy));
        if (jiao < 0)
        {
            jiao += 360.0f;
        }
        return jiao;
    }
    public int getDirectByJiao(float j)
    {
        int d = 0;
        if (j < 22.5f)
        {
            d = 0;
        }
        else if (j < 67.5f)
        {
            d = 1;
        }
        else if (j < 112.5f)
        {
            d = 2;
        }
        else if (j < 157.5f)
        {
            d = 3;
        }
        else if (j < 202.5f)
        {
            d = 4;
        }
        else if (j < 247.5f)
        {
            d = 5;
        }
        else if (j < 292.5f)
        {
            d = 6;
        }
        else if (j < 337.5f)
        {
            d = 7;
        }
        else
        {
            d = 0;
        }
        return d;
    }
    public float getDis(int mx, int my)
    {
        return 0;
    }
    public float getDis(Role target)
    {
        int disx = Math.Abs(target.cellx - cellx);
        int disy = Math.Abs(target.celly - celly);

        // Debug.Log(disx + " " + disy);

        return disx > disy ? disx : disy;
    }
    public bool closeTo(Role target, int dis)
    {
        return false;
    }
    public virtual bool inScreen(int x, int y, int w, int h)
    {
        return false;
    }

    public float x90;
    public float y90;
    public float getPositionX1()
    {
        return x90;
    }
    public float getPositionY1()
    {
        return y90;
    }
    public Vector2 getPosition1()
    {
        return new Vector2(x90, y90);
    }
    public void setPositionX1(float x1)
    {
        x90 = x1;
        // setPositionX(x1);
    }



    public void setPositionY1(float y1)
    {
        y90 = y1;
        // setPositionY(y1);
    }

    public float xtemp;
    public float ytemp;
    public void setPosition1(Vector3 pos)
    {
        x90 = pos.x;
        y90 = pos.y;



        // setPosition(Vec2(x45 + 60, y45));

        gameObject.transform.localPosition = new Vector3(pos.x, pos.y, 0);

        //cellx = pos.x;
        //celly = pos.y;

        xtemp = gameObject.transform.localPosition.x;
        ytemp = gameObject.transform.localPosition.y;




    }
    public void setPosition2(Vector3 pos)
    {


        gameObject.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        xtemp = gameObject.transform.localPosition.x;
        ytemp = gameObject.transform.localPosition.y;


    }


    public void resetcellxy()
    {
        //Vector2 pos = maplayer.pmap.to90pos(gameObject.transform.localPosition);

        //x90 = pos.x;
        //y90 = pos.y;
        //cellx = (int)pos.x;
        //celly = (int)pos.y;
        //xtemp = gameObject.transform.localPosition.x;
        //ytemp = gameObject.transform.localPosition.y;

        // Debug.Log("cellx=================" + cellx + " celly===================" + celly);
    }
    public void backupcell()
    {
        backcellx = cellx;
        backcelly = celly;
        backcellw = cellw;
        backcellh = cellh;
        backfx = fx;
    }

    public void resetbian()
    {
        //bianlist.Add(new Vector3(-CellData.CELLW * cellh, CellData.CELLW/2 * (cellh - 1),0));
        //bianlist.Add(new Vector3(0,-CellData.CELLW/2, 0));
        //bianlist.Add(new Vector3(CellData.CELLW*cellw, CellData.CELLW/2 * (cellw-1), 0));
        //bianlist.Add(new Vector3(CellData.CELLW * cellw, CellData.CELLW/2 * (cellw - 1) + height, 0));
        //bianlist.Add(new Vector3(CellData.CELLW * cellw- CellData.CELLW*cellh, CellData.CELLW / 2 * (cellw - 1) + height + CellData.CELLW / 2*cellh, 0));
        //bianlist.Add(new Vector3(-CellData.CELLW * cellh, CellData.CELLW/2 * (cellh - 1) + height, 0));

        for (int i = 0; i < bianobjlist.Count; i++)
        {
            Destroy((bianobjlist[i]));
        }

        bianobjlist.Clear();


        int temp = 60;
        int htemp = 30;
        Vector3 tempv = new Vector3(temp * Mathf.Abs(cellhhou), htemp * Mathf.Abs(cellhhou), 0);
        Vector3 tempv1 = new Vector3(temp * cellwhou, -htemp * cellwhou, 0);
        bianlist[0] = new Vector3(-temp, 0, 0);
        bianlist[1] = bianlist[0] + tempv1;
        bianlist[2] = bianlist[1] + tempv;
        bianlist[3] = bianlist[2] + new Vector3(0, height, 0);
        bianlist[4] = bianlist[1] + new Vector3(0, height, 0) + (tempv - tempv1);
        bianlist[5] = bianlist[0] + new Vector3(0, height, 0);

        bianlist1[0] = new Vector3(-temp, 0, 0);
        bianlist1[1] = bianlist1[0] + tempv1;
        bianlist1[2] = bianlist1[1] + tempv;
        bianlist1[3] = bianlist1[1] + (tempv - tempv1);


        // Debug.Log(tempv + "=========="+cellwhou+" "+cellhhou+"==========" + bianlist[1]);
        //bianlist[0]=new Vector3(-CellData.CELLW * cellwhou, CellData.CELLW / 2 * (cellwhou - 1), 0);
        //bianlist[1] = new Vector3(0, -CellData.CELLW / 2, 0);
        //bianlist[2] = new Vector3(CellData.CELLW * cellhhou, CellData.CELLW / 2 * (cellhhou - 1), 0);
        //bianlist[3] = new Vector3(CellData.CELLW * cellhhou, CellData.CELLW / 2 * (cellhhou - 1) + height, 0);
        //bianlist[4] = new Vector3(CellData.CELLW * cellhhou - CellData.CELLW * cellwhou, CellData.CELLW / 2 * (cellhhou - 1) + height + CellData.CELLW / 2 * cellwhou, 0);
        //bianlist[5] = new Vector3(-CellData.CELLW * cellwhou, CellData.CELLW / 2 * (cellwhou - 1) + height, 0);

        //if (fx == 1|| fx == 3)
        //{
        //   for(int i = 0; i < bianlist.Length; i++)
        //    {
        //        bianlist[i] = bianlist[i] + new Vector3(CellData.CELLW*(cellhhou-1), -CellData.CELLW / 2 * (cellhhou - 1), 0);

        //    }
        //}
        //else
        //{

        // }

        if (cellw != cellwhou || cellh != cellhhou)
        {
            if (fx == 1 || fx == 3)
            {
                if (cellhhou < 0)
                {
                    for (int i = 0; i < bianlist.Length; i++)
                    {
                        bianlist[i] = bianlist[i] + new Vector3(temp * (cellh + cellhhou), htemp * (cellh + cellhhou), 0);
                    }
                }
                else
                {
                    for (int i = 0; i < bianlist.Length; i++)
                    {
                        bianlist[i] = bianlist[i] + new Vector3(temp * (cellw - cellwhou), -htemp * (cellw - cellwhou), 0);
                    }
                }

            }
            else if (fx == 0 || fx == 2)
            {


                for (int i = 0; i < bianlist.Length; i++)
                {
                    //  bianlist[i] = bianlist[i] + new Vector3(temp * (cellh - cellhhou), htemp * (cellh - cellhhou), 0);
                }

            }
        }






        //for (int i = 0; i < bianlist.Length; i++)
        //{
        //    Vector3 p = (Vector3)bianlist[i];
        //    GameObject preobj = Instantiate(Resources.Load("Prefabs/test/vpoint")) as GameObject;
        //    preobj.transform.parent = gameObject.transform;
        //    preobj.transform.localScale = new Vector3(1, 1, 1);
        //    preobj.transform.localPosition = new Vector3(p.x, p.y, 0);
        //    bianobjlist.Add(preobj);
        //}


    }


    public bool containspoint(Vector3 v)
    {
        if (!gameObject.activeSelf)
        {
            return false;
        }

        Vector3 position = gameObject.transform.InverseTransformPoint(v);



        if (CTTools.isinside(position, bianlist))
        {
            //  Debug.Log("true");
            return true;
        }
        else
        {
            //  Debug.Log("false");
            return false;
        }

    }
    public bool containspoint1(Vector3 v)
    {
        if (!gameObject.activeSelf)
        {
            return false;
        }

        Vector3 position = gameObject.transform.InverseTransformPoint(v);
        if (CTTools.isinside(position, bianlist1))
        {
            //  Debug.Log("true");
            return true;
        }
        else
        {
            //  Debug.Log("false");
            return false;
        }

    }
   
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {



        bianobjlist.Clear();


        xgroot = null;
       



    }


   
}
