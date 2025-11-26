using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : Role
{
    public const int STA_STAND = 0;
    public const int STA_WALK = 1;
    public GameObject agroot;
    public APaintNodeSpine spaintnode;
    public string nowActionStr = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    WorldLayer worldlayer;
    public void create(WorldLayer mapLayer, GameObject target)
    {

        worldlayer = mapLayer;

        gameObject.transform.SetParent(target.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);


        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
            texframeobjs.name = "tiao";
            APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
            temppaintnode.create1(agroot, "shilaimu", "shilaimu");
            temppaintnode.transform.localPosition = Vector3.zero;
            temppaintnode.transform.localScale = Vector3.one * 0.075f;
            temppaintnode.setdepth(21);
            temppaintnode.playAction2Auto("idle", true);
            spaintnode = temppaintnode;
        }
        v = 12;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
    string getNowActionStr()
    {
        string temp = null;
        switch (sta)
        {
            case STA_STAND:
                temp = "idle";
                break;
            case STA_WALK:
                temp = "move";
                break;
        }
        return temp;
    }
    void setActionStr()
    {
        // lastAction = nowAction;
        nowActionStr = getNowActionStr();




        if (fxrotx[direct] == 1)
        {
            agroot.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            agroot.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        rotx = fxrotx[direct];






        if (spaintnode != null)
        {

            bool loop = true;
            //if (nowAction == STA_GETCAI || nowAction == STA_PUTCAI || nowAction == STA_PAOJI0 || nowAction == STA_CAITAI0 || nowAction == STA_CAITAI2 || sta == STA_CAFF0 || sta == STA_CAFF2 || sta == STA_READ0 || sta == STA_READ2 || sta == STA_JIEJI0 || sta == STA_JIEJI2 || sta == STA_PC0 || sta == STA_PC2 || sta == STA_TV0 || sta == STA_TV2 || sta == STA_JITA0 || sta == STA_JITA2 || sta == STA_ANYI0 || sta == STA_ANYI2 || sta == STA_CHUANG0 || sta == STA_CHUANG2 || sta == STA_SHAFA0 || sta == STA_SHAFA2 || sta == STA_CHIGUI || sta == STA_HEGUI || sta == STA_ZHAOHUANQIAN || sta == STA_ZHAOHU || sta == STA_CHISHU || sta == STA_CHISHU2
            //    || sta == STA_FENGJING0 || sta == STA_FENGJING2
            //    || sta == STA_BIANDANG0 || sta == STA_BIANDANG2
            //    || sta == STA_WANJI0 || sta == STA_WANJI2
            //    || sta == STA_DIAO || sta == STA_TOULANDA
            //    )
            //{
            //    loop = false;
            //}
            spaintnode.playAction2(nowActionStr, loop);



        }
    }



    public override bool exMove()
    {


        if (nowActionStr != getNowActionStr() || rotx != fxrotx[direct])
        {
            setActionStr();
        }

        //  Debug.Log(sta+" "+aista);

        if (pauseflag)
        {
            return true;
        }


        if (spaintnode != null)
        {
            spaintnode.logic(0.03f);
        }






        if (sta < 2)
        {
            return false;
        }
        else
        {


            //if (sta == STA_ZHAOHUAN || sta == STA_ZUO || sta == STA_COMBOZUO || sta == STA_EAT || sta == STA_ZUOCAI || sta == STA_SHOUSHI || sta == STA_SHANMEI || sta == STA_ANYI1 || sta == STA_CHUANG1 || sta == STA_PAOJI1 || sta == STA_TIAOJI1 || sta == STA_TV1 || sta == STA_JIEJI1 || sta == STA_PC1 || sta == STA_CAITAI1 || sta == STA_SHAFA1 || sta == STA_CAFF1 || sta == STA_READ1 || sta == STA_JITA1 || sta == STA_TOULAN || sta == 1001 || (sta == STA_SKILL2 && (skillindex == 130 || skillindex == 134 || skillindex == 148)))
            //{

            //}
            //else if (sta == STA_GUKEHAPPY)
            //{
            //    if (spaintnode.isCurrEnd())
            //    // if (agdata.isCurrEnd)
            //    {
            //        setSta(STA_EAT);
            //    }

            //}
            //else
            //{
            //    if (spaintnode.isCurrEnd())
            //    // if (agdata.isCurrEnd)
            //    {
            //        setSta(STA_STAND);
            //    }
            //}

            return true;
        }

    }
    public void setSta(int s)
    {
        sta = s;

    }

    public void setPoint(int v)
    {
        idx = v;
        gameObject.transform.localPosition = worldlayer.pointarray[v].transform.localPosition;
        setPosition1(gameObject.transform.localPosition);
    }
    public void walkto(int idx,Action act)
    {
        movex = 0;
        movey = 0;
        setSta(STA_STAND);

        PathSearchInfo.AStar1(this, idx);

        if (path.Count >= 10)
        {
            path.Clear();
            setPoint(idx);
            worldlayer.movePMapTo(this.transform.position, PMap.mapscale, null);
          //  worldlayer.focus = null;
            act();
        }
        else
        {
            worldlayer.focus = this;

            stopact = new Action(() =>
            {
                setPoint(idx);
                worldlayer.focus = null;
                act();
            });
        }


    }
    public void clearpath(bool flag = true)
    {
        movex = 0;
        movey = 0;
        path.Clear();
        pIndex = 0;

        if (flag)
            this.setPosition1(new Vector2(cellx, celly));

        setAIStyle(aik_astar);

        setSta(STA_STAND);
    }

}
