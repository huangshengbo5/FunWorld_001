using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLayer : MonoBehaviour
{
    public static WorldLayer instance;
    public int width, height;
    public PMap pmap;

    public People pp, focus;

    public List<Monster> monsters = new List<Monster>();
    public int zhongzi = 10;


    public GameObject pointobj, lineobj;

    //public List<StagePoint> pointlist = new List<StagePoint>();
    public StagePoint[] pointarray;
    public List<Line> linelist = new List<Line>();

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pmap.setworld(this);
        pointarray = new StagePoint[StagePointDef.datas.Count];

        SActionGroup.getAGFromBuffer("liesha", "liesha");
        SActionGroup.getAGFromBuffer("gebulin", "gebulin");
        SActionGroup.getAGFromBuffer("nvzhanshi", "nvzhanshi");
        SActionGroup.getAGFromBuffer("kuilei", "kuilei");
        SActionGroup.getAGFromBuffer("shirenmo", "shirenmo");
        SActionGroup.getAGFromBuffer("tuling", "tuling");

        refreshdata();
        ppnowindex = shangyigeindex;

        width = CTGlobal.realWidth;
        height = CTGlobal.scrHeight;
        gameObject.transform.localPosition = new Vector3(-width / 2, -height / 2, 0);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        MainUI.instance.setworldmsg(selectdbbean);
        pointobj.SetActive(false);
        lineobj.SetActive(false);
        resetpoints();

        GameObject preobj = ResManager.getGameObject("allpre", "vpeople");
        pp = preobj.GetComponent<People>();
        pp.create(this, pmap.gameObject);
        pp.setPoint(ppnowindex);
        GameGlobal.instance.SecondTimerCall += worldMonsterTimerCall;

    }
    private void OnDestroy()
    {
        GameGlobal.instance.SecondTimerCall -= worldMonsterTimerCall;
    }



    public data_campaignBean selectdbbean;
    public int shangyigeindex = -1;
    public void refreshdata()
    {
        for (int i = 0; i < StagePointDef.datas.Count; i++)
        {
            StagePointBean temp = StagePointDef.datas[i];
            temp.lockflag = true;
            if (i == 0)
            {
                temp.lockflag = false;
            }
            else
            {
                for (int j = 0; j < GameGlobal.gamedata.stageindex + 1; j++)
                {
                    data_campaignBean dcb = data_campaignDef.datas[j];
                    if (dcb.spot == temp.id)
                    {
                        temp.lockflag = false;
                        break;
                    }
                }
            }
        }


            selectdbbean = data_campaignDef.datas[GameGlobal.gamedata.stageindex];

        if (GameGlobal.gamedata.stageindex >= 1)
        {
            if (GameGlobal.gamedata.tongguan)
            {
                shangyigeindex = data_campaignDef.datas[GameGlobal.gamedata.stageindex].spot;
            }
            else
            {
                shangyigeindex = data_campaignDef.datas[GameGlobal.gamedata.stageindex - 1].spot;
            }

        }
        else
        {
            shangyigeindex = 0;
        }


    }

    public data_campaignBean getstagedatabyposid(int posid)
    {
        for (int j = GameGlobal.gamedata.stageindex; j >= 0; j--)
        {
            data_campaignBean dcb = data_campaignDef.datas[j];
            if (dcb.spot == posid)
            {
                return dcb;
            }
        }
        return null;
    }
    private void refreshMonster()
    {



        // int tempindex = shangyigeindex;
        System.Random rd = new System.Random(zhongzi);

        //for (int i = 0; i < monsters.Count; i++)
        //{
        //    tempindex = tempindex - rd.Next(2, 4);
        //    if (tempindex > 0)
        //    {
        //        data_campaignBean dcb = data_campaignDef.datas[tempindex];

        //    }

        //}
        List<int> templist = new List<int>();
        for (int i = 0; i < StagePointDef.datas.Count; i++)
        {
            StagePointBean temp = StagePointDef.datas[i];
            if (!temp.lockflag && temp.id != 0 && temp.id != selectdbbean.spot && temp.id != shangyigeindex)
            {
                if (PathSearchInfo.AStarTest(selectdbbean.spot, temp.id) < 40)
                {
                    templist.Add(temp.id);
                }
            }

        }

        templist.Sort(delegate (int s1, int s2) { return s1.CompareTo(s2); });

        List<int> templist1 = new List<int>();
        //for(int i = 0; i < templist.Count; i++)
        //{

        //}
        int tempindex = 0;
        while (tempindex < templist.Count)
        {
            templist1.Add(templist[tempindex]);
            tempindex += 3;
        }

        // if (templist1.Count >= 10)
        {
            for (int i = 0; i < 15; i++)
            {
                int temp0 = rd.Next(templist1.Count);
                int temp1 = rd.Next(templist1.Count);
                int temptemp = templist1[temp0];
                templist1[temp0] = templist1[temp1];
                templist1[temp1] = temptemp;
            }

            for (int i = 0; i < monsters.Count && i < templist1.Count; i++)
            {
                pointarray[templist1[i]].setMonster(monsters[i]);
            }
        }

    }

    internal void ppback()
    {
        ppnowindex = shangyigeindex;
        pp.setPoint(ppnowindex);
        movePMapTo(pp.transform.position, PMap.mapscale, null);
    }

    public void refresh()
    {
        refreshdata();
        resetpoints();
    }
    public static int ppnowindex = 0;
    // Start is called before the first frame update
    //void Start()
    //{


    //    // movePMapTo(pointlist[0].transform.localPosition.x, pointlist[0].transform.localPosition.y, 1);
    //}
    public StagePoint nowpoint;


    void worldMonsterTimerCall()
    {
        geguai();
    }

    long nextfreshTime = -1;
    bool setNextRefreshMonsterTiem()
    {
        bool isNeedRefreshConfig = true;
        long now = GameGlobal.getCurrTimeMM();

        for (int i = 0; i < GameGlobal.gamedata.towerfromtolist.Count; i++)
        {
            FromTo ft = GameGlobal.gamedata.towerfromtolist[i];
            if (ft == null)
            {
                continue;
            }
            if (now >= ft.from && now <= ft.to && nextfreshTime != ft.to)
            {
                nextfreshTime = ft.to;
                isNeedRefreshConfig = false;
            }
        }
        return isNeedRefreshConfig;
    }

    public void geguai()
    {
        long now = GameGlobal.getCurrTimeMM();
        if (GameGlobal.gamedata.stageindex >= Statics.shuaStageindex && now >= nextfreshTime)
        {
            if (setNextRefreshMonsterTiem())
            {
                getguai1();
            }
            else
            {
                getguai2();
            }
        }
    }

    public void getguai1()
    {
        HttpManager.instance.sendConfig((int code) =>
        {
            setNextRefreshMonsterTiem();
            getguai2();
        });
    }
    public void getguai2()
    {
        HttpManager.instance.sendMonsterShow((code1) =>
        {
            if (code1 == Callback.SUCCESS)
            {
                refreshMonster();
            }

        });
    }

    public void resetpoints()
    {

        //for (int i = 0; i < pointlist.Count; i++)
        //{
        //    Destroy(pointlist[i].gameObject);
        //}
        for (int i = 0; i < pointarray.Length; i++)
        {
            if (pointarray[i] != null)
            {
                Destroy(pointarray[i].gameObject);
                pointarray[i] = null;
            }

        }
        for (int i = 0; i < linelist.Count; i++)
        {
            Destroy(linelist[i].gameObject);
        }
        //   pointlist.Clear();
        linelist.Clear();

        Debug.LogError("======================resetpoints===========================" + StagePointDef.datas.Count);
        for (int i = 0; i < StagePointDef.datas.Count; i++)
        {
            if (!StagePointDef.datas[i].lockflag)
            {
                GameObject temp = NGUITools.AddChild(pmap.gameObject, pointobj);
                temp.transform.localScale = Vector3.one * 0.5f;
                StagePoint sp = temp.GetComponent<StagePoint>();
                sp.setworld(this);
                sp.setData(StagePointDef.datas[i], i);
                if (selectdbbean.spot == StagePointDef.datas[i].id)
                {
                    nowpoint = sp;
                }
                //  pointlist.Add(sp);
                pointarray[i] = sp;
                addlines(StagePointDef.datas[i]);
            }

        }
        if (nowpoint == null)
        {
            nowpoint = pointarray[0];
        }
        
        // Debug.LogError(nowpoint.index);
        movePMapTo(nowpoint.index);

        //   PathSearchInfo.mappointlist = pointlist;
        PathSearchInfo.mappointarray = pointarray;



        if (false)
        {
            for (int i = 0; i < 10; i++)
            {
                Monster m = new Monster();
                m.towerID = i;
                m.status = 0;
                monsters.Add(m);
            }
            refreshMonster();
        }
        else
        {
            //  getguai1();

        }
    }

    private void addlines(StagePointBean stagePointBean)
    {
        for (int i = 0; i < stagePointBean.aiidlist.Count; i++)
        {
            addlines(stagePointBean, StagePointDef.datas[stagePointBean.aiidlist[i]]);
        }
    }

    private void addlines(StagePointBean stagePointBean1, StagePointBean stagePointBean2)
    {
        if (stagePointBean1.lockflag || stagePointBean2.lockflag)
        {
            return;
        }

        bool you = false;
        for (int i = 0; i < linelist.Count; i++)
        {
            if ((linelist[i].bindex == stagePointBean1.id && linelist[i].eindex == stagePointBean2.id)
                || (linelist[i].bindex == stagePointBean2.id && linelist[i].eindex == stagePointBean1.id))
            {
                you = true;
                break;
            }
        }
        if (!you)
        {
            GameObject temp = NGUITools.AddChild(pmap.gameObject, lineobj);
            Line l = temp.GetComponent<Line>();
            l.setdepth(9);
            l.setpoint(new Vector3(stagePointBean1.x, stagePointBean1.y), new Vector3(stagePointBean2.x, stagePointBean2.y), 12);
            l.setalpha(0.5f);
            l.bindex = stagePointBean1.id;
            l.eindex = stagePointBean2.id;
            linelist.Add(l);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (focus != null)
        {
            movePMapTo(focus.transform.position, PMap.mapscale, null);

        }
        if(pp!=null)
        pp.move();
        automove();
    }

    Vector3 beginposition = new Vector3();
    Vector3 mapbeginposition;
    bool dragflag = false;
    bool dragselect = false;



    public void OnPress(bool pressed)
    {
        // Debug.LogError(width + " " + height);
        if (pressed)
        {
            mapbeginposition = pmap.gameObject.transform.localPosition;
            Vector3 position1 = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
            beginposition = gameObject.transform.InverseTransformPoint(position1);
            dragflag = true;
        }
        else
        {
            dragflag = false;
        }




    }

    public static int getindexinlist(List<StagePoint> pointlist, int v)
    {
        for (int i = 0; i < pointlist.Count; i++)
        {
            if (pointlist[i].index == v)
            {
                v = i;
                break;
            }
        }
        return v;
    }

    public void movePMapTo(int v)
    {


        // movePMapTo(pointlist[v].transform.localPosition.x, pointlist[v].transform.localPosition.y, PMap.mapscale);

        movePMapTo(pointarray[v].transform.localPosition.x, pointarray[v].transform.localPosition.y, PMap.mapscale);
        Debug.LogError("======================movePMapTo===========================" + v);
    }

    public void OnDrag(Vector2 delta)
    {
        if (dragflag)
        {



            Vector3 position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
            position = gameObject.transform.InverseTransformPoint(position);



            Vector3 temppos = mapbeginposition + position - beginposition;

            if (temppos.x > -PMap.mappixbeginx * PMap.mapscale)
            {
                temppos.x = -PMap.mappixbeginx * PMap.mapscale;
            }
            if (temppos.x < width - PMap.mappixwidth * PMap.mapscale)
            {
                temppos.x = width - PMap.mappixwidth * PMap.mapscale;
            }
            if (temppos.y > -PMap.mappixbeginy * PMap.mapscale)
            {
                temppos.y = -PMap.mappixbeginy * PMap.mapscale;
            }
            if (temppos.y < height - PMap.mappixheight * PMap.mapscale)
            {
                temppos.y = height - PMap.mappixheight * PMap.mapscale;
            }
            pmap.gameObject.transform.localPosition = temppos;




        }
    }
    bool automoveflag = false;
    float automovex;
    float automovey;
    float automovescale;
    bool guideTime = false;
    public void movePMapTo(float mx, float my, float scale)
    {
        automovex = mx;
        automovey = my;
        automovescale = scale;
        automoveflag = true;
        guideTime = false;
    }
    public void movePMapTo(Vector3 worldpos, float scale, automovecallback m)
    {
        Vector3 position = pmap.gameObject.transform.InverseTransformPoint(worldpos);
        automovex = position.x;
        automovey = position.y;
        automovescale = scale;
        automoveflag = true;
        guideTime = false;
        mautomovecallback = m;
    }
    public delegate void automovecallback();
    automovecallback mautomovecallback;
    public void automove()
    {
        if (!automoveflag)
        {
            return;
        }
        if (Math.Abs(PMap.mapscale - automovescale) > 0.01)
        {
            PMap.mapscale = PMap.mapscale - (PMap.mapscale - 1) / 2;
        }
        else
        {
            PMap.mapscale = automovescale;
        }




        //  Debug.Log(PMap.mapscale);


        Vector3 temppos = pmap.gameObject.transform.localPosition;
        Vector3 autopos = new Vector3(-automovex * PMap.mapscale, -automovey * PMap.mapscale, 0);
        Vector3 mudipos = autopos + new Vector3(CTGlobal.realWidth / 2, CTGlobal.scrHeight / 2, 0);
        Vector3 dis = mudipos - temppos;

        temppos += (mudipos - temppos) / 2;






        pmap.gameObject.transform.localScale = new Vector3(PMap.mapscale, PMap.mapscale, 1);


        if (temppos.x > -PMap.mappixbeginx * PMap.mapscale)
        {
            temppos.x = -PMap.mappixbeginx * PMap.mapscale;
            dis.x = 0;
        }
        if (temppos.x < width - PMap.mappixwidth * PMap.mapscale)
        {
            temppos.x = width - PMap.mappixwidth * PMap.mapscale;
            dis.x = 0;
        }
        if (temppos.y > -PMap.mappixbeginy * PMap.mapscale)
        {
            temppos.y = -PMap.mappixbeginy * PMap.mapscale;
            dis.y = 0;
        }
        if (temppos.y < height - PMap.mappixheight * PMap.mapscale)
        {
            temppos.y = height - PMap.mappixheight * PMap.mapscale;
            dis.y = 0;
        }
        pmap.gameObject.transform.localPosition = temppos;

        if (Vector3.Distance(dis, Vector3.zero) < 1)
        {
            automoveflag = false;

            mautomovecallback?.Invoke();

            //原来在这里弹出引导，现在去掉
        }

        if (!guideTime)
        {
            if (Vector3.Distance(dis, Vector3.zero) < 2)
            {
                guideTime = true;
                GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow, (int)GuideAcitveWindow.Level);
                if (GameGlobal.gamedata.isWaitWorld)
                {
                    GameGlobal.gamedata.isWaitWorld = false;
                    Debug.LogError("yindao=" + GameGlobal.gamedata.guideStep);
                    if (GameGlobal.gamedata.guideStep >= 9 && GameGlobal.gamedata.guideStep < 16)
                    {
                        int step = GameGlobal.gamedata.guideStep + 1;
                        //已经过了一个引导，并且没全过，那么播放第一个
                        if (data_guidesDef.guideList.ContainsKey(step))
                        {
                            var list = data_guidesDef.guideList[step];
                            if (list != null && list.Count > 0)
                            {
                                data_guidesBean bean = list[0];
                                if (bean.activetype == 1 && bean.activeparam == 2)
                                {
                                    UIManager.showGuideStep(bean.id);
                                }
                            }
                        }
                    }
                }
            }
        }

        //   Debug.Log("dis=" + dis.x + " " + dis.y);
    }


}
