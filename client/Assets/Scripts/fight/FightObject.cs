using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spine;

public class FightObject : MonoBehaviour {
    public const int FT_STAND = 0;
    public const int FT_FIGHT = 1;
    public const int FT_SKILL = 2;
    public const int FT_BEIDA = 3;
    public const int FT_DEAD = 4;
    public const int FT_HUADONG = 5;
    public const int FT_SHANMEI = 6;
    public const int FT_SHANYOU = 7;
    public const int FT_RENG = 8;
    public const int FT_NOACTSKILL = 9;
    public const int FT_FEI = 10;
    public const int FT_FEIPA = 11;
    public const int FT_TUI = 12;
    public const int FT_BEIDATUI = 13;
    public const int FT_TUIPA = 14;
    public const int FT_PAO = 15;
    public const int FT_DAKONG = 16;
    public const int FT_SHANBI = 17;
    public const int FT_SHANHUI = 18;
    public const int FT_WIN = 19;
    public const int FT_DEADEND = 20;
    public int ftsta;
    public GameObject agroot, agroot1;
    public APaintNodeSpine spaintnode;
    public int zhen = 0;
    internal bool fightle;
    internal bool sile;
    public int dp;

    public int fighttype = 0;
    public string fighttypestr;

    public string agname;


    public int nowAction;

    void Start () {
        //dongzuo = gameObject.GetComponent<SkeletonGraphic>();

        //skg.skeletonDataAsset = sda;
        //// 重置动画
        //skg.Initialize(true);
        //skg.AnimationState.SetAnimation(0, animName, loop);
        //skg.AnimationState.TimeScale = timeScale;

    }


    FightControl fc;
    internal void create(int v, FightControl fightControl, GameObject target)
    {
        fc = fightControl;
        gameObject.transform.SetParent(target.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

   // public static string[] agname = { "luoya", "airen", "buluoluo", "fashi", "shitouren", "shilaimu", "xixuegui"};
    public static string[] agname1 = { "liesha", "kuilei", "tuling" };
    public void init(int i,int z)
    {
        zhen = z;

        if (true)
        {
            return;
        }


        GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
        spaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
        if (i == 0)
        {
            spaintnode.create1(agroot, data_heroDef.getdatabyheroid(fc.testid[0]).resource, data_heroDef.getdatabyheroid(fc.testid[0]).resource);
        }
        else
        {
            spaintnode.create1(agroot, agname1[fc.testid[1]], agname1[fc.testid[1]]);
        }
        spaintnode.transform.localScale = Vector3.one * 0.3f;
        spaintnode.playAction2Auto("idle", true);
        //  spaintnode.setSkin(i, 0);
        spaintnode.addEvent(HandleEvent);
        if (zhen==0)
        {
            gameObject.transform.localPosition = new Vector3(300, 700,0);
            agroot.transform.localScale = new Vector3(1, 1, 1);
           // zhen = 0;
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(800, 700, 0);
            agroot.transform.localScale = new Vector3(-1, 1, 1);
           // zhen = 1;
        }
        gameObject.name = "f" + i;
        ftsta = 0;

        logevent();
    }
    public bool zhensiflag = true;
    private void actlogic(float dt)
    {

        if (jt != null && !jt.overflag)
        {
            jt.logic(dt);
            if (jt.overflag)
            {
                //if (ftsta == FT_SHANBI)
                //{
                //    setSta(FT_SHANHUI);
                //    addxgpiaozi(5, 0, -1);
                //}
                //else
                {
                    ftsta = FT_STAND;
                }
              
            }
        }
        if (jth != null && !jth.overflag)
        {
            jth.logic(dt);

        }

        if (ftsta == FT_BEIDATUI)
        {
            tjt.logic(dt);
            if (tjt.overflag)
            {
                ftsta = FT_STAND;
                if (fbean.hp <= 0)
                {
                    setSta(FT_DEAD);
                }
            }
        }
        else if (ftsta == FT_FEI)
        {
            tjt.logic(dt);
            if (tjt.overflag)
            {
                ftsta = FT_FEIPA;
                if (fbean.hp <= 0)
                {
                    setSta(FT_DEAD);
                }
            }
        }
        else if (ftsta == FT_TUI)
        {
            tjt.logic(dt);
            if (tjt.overflag)
            {
                ftsta = FT_TUIPA;
                if (fbean.hp <= 0)
                {
                    setSta(FT_DEAD);
                }
            }
        }
        //else if (ftsta == FT_KONG)
        //{
        //    tjt.logic();
        //    if (tjt.overflag)
        //    {
        //        ftsta = FT_STAND;
        //    }
        //}

        int zbian = 100;
        int ybian = 100;

        if (zhen==0)
        {
            ybian = 300;
        }else 
        {
            zbian = 300;
        }

        if (gameObject.transform.localPosition.x > FightControl.mapwidth- ybian)
        {
            gameObject.transform.localPosition = new Vector3(FightControl.mapwidth- ybian, gameObject.transform.localPosition.y, 0);
        }
        if (gameObject.transform.localPosition.x < zbian)
        {
            gameObject.transform.localPosition = new Vector3(zbian, gameObject.transform.localPosition.y, 0);
        }


        hongdelay--;
        if (hongdelay == 0)
        {
            spaintnode.setColor(new Color(1, 1, 1));
        }


        if (nowActionStr != getNowActionStr())
        {
            setActionStr();
        }

        spaintnode.logic(dt);




        if (spaintnode.isCurrEnd)
        {
            switch (ftsta)
            {
                case FT_STAND:
                case FT_FIGHT:
                case FT_SKILL:
                case FT_SHANYOU:
                case FT_RENG:
                case FT_NOACTSKILL:
                case FT_FEIPA:
                case FT_TUIPA:
                case FT_SHANBI:
               // case FT_WIN:
               // case FT_FEI:
                    ftsta = FT_STAND;
                    break;
                case FT_WIN:
                    // case FT_FEI:
                    ftsta = FT_STAND;

                    fc.showresult();
                    break;
                case FT_SHANMEI:
                    ftsta = FT_STAND;
                    break;
                case FT_DEAD:
                    ftsta = FT_DEADEND;
                    FightControl.jiasulevel = 1;
                    if(zhensiflag)
                    {
                        whofightme.setSta(FT_WIN);
                    }
   
                    break;
                case FT_BEIDA:
                    ftsta = FT_STAND;
                    if (fbean.hp <= 0)
                    {
                        setSta(FT_DEAD);
                    }
                    break;
            }
        }
    }

    public void logic(float dt)
    {

       // actlogic(0.033f);
        actlogic(dt);

        setdepth(1000 - (int)gameObject.transform.localPosition.y);

        xglogic(dt);

    }

    private void setdepth(int v)
    {
        spaintnode.setdepth(v + (this==fc.nowfighter?4:2));
    }
    public string nowActionStr = null;
    
    string getNowActionStr()
    {
        string temp = null;
        switch (ftsta)
        {
            case FT_STAND:
                if (getbuffNum(1) > 0)
                {
                    temp = "vertigo";
                }
                else
                {
                    temp = "idle";
                }
              
                break;
            case FT_HUADONG:
                temp = "move";
                break;
            case FT_FIGHT:
                if (fc.nowaction.hitresult)
                {
                    //if (fighttype == 0)
                    //{
                    //    temp = "attack";
                    //}
                    //else
                    //{
                    //    temp = "skill";
                    //}
                    temp = fighttypestr;
                }
                else
                {
                    if (fighttype == 0)
                    {
                        temp = "attack_miss";
                    }
                    else
                    {
                        temp = "skill_miss";
                    }
                }


                break;
            case FT_FEI:
                temp = "fall";
                break;
            case FT_FEIPA:
                temp = "fall_re";
                break;
            case FT_TUI:
                temp = "repel";
                break;
            case FT_TUIPA:
                temp = "repel_re";
                break;
            case FT_PAO:
                temp = "move";
                break;
            case FT_BEIDA:
            case FT_BEIDATUI:
                temp = "injured";
                break;
            case FT_DAKONG:
            case FT_SHANHUI:
                temp = "idle";
                break;
            case FT_SHANBI:
                temp = "dodge";
                break;
            case FT_WIN:
                temp = "win";
                break;
            case FT_DEAD:
            case FT_DEADEND:
                temp = "lose";
                break;
        }
        return temp;
    }
    void setActionStr()
    {
        // lastAction = nowAction;
        nowActionStr = getNowActionStr();




        //if (fxrotx[direct] == 1)
        //{
        //    agroot.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        //}
        //else
        //{
        //    agroot.gameObject.transform.localScale = new Vector3(1, 1, 1);
        //}
        //rotx = fxrotx[direct];






        if (spaintnode != null)
        {

            //bool loop = true;
            //if (nowActionStr.IndexOf("attack") != -1|| nowActionStr.IndexOf("skill") != -1 || nowActionStr.IndexOf("fall") != -1  || nowActionStr.IndexOf("lose") != -1)
            //{
            //    loop = false;
            //}

            bool loop = false;
            if (nowActionStr.IndexOf("idle") != -1|| nowActionStr.IndexOf("vertigo")!=-1)
            {
                loop = true;
            }
            spaintnode.playAction2(nowActionStr, loop);

            if (ftsta == FT_FIGHT)
            {
                atkventget();
            }


        }
    }

    int atkcount;
    int atkquancount;
    int atkindex;
    private void atkventget()
    {
        atkcount=0;
        atkquancount = 0;
        atkindex = 0;
        Spine.Animation a = spaintnode.zhengstate.Data.skeletonData.Animations.Items[spaintnode.nowAction];
        for (int j = 0; j < a.timelines.Count; j++)
        {
            Spine.Timeline t = a.timelines.Items[j];
            if (t.GetType() == typeof(Spine.EventTimeline))
            {
               // Debug.Log("shijiantimeline");

                Spine.EventTimeline tt = (Spine.EventTimeline)t;
               // Debug.Log(tt.frames.Length);
              //  Debug.Log(tt.Events.Length);
                for (int i0 = 0; i0 < tt.Events.Length; i0++)
                {
                    if (tt.Events[i0] != null)
                    {
                      //  Debug.LogError(i0 + "  " + tt.Events[i0].ToString() + "  " + tt.frames[i0]);
                        if (tt.Events[i0].data.name.Equals("attack"))
                        {
                            atkcount++;
                            atkquancount += attackquan[tt.Events[i0].intValue];
                        }
                    }
                }
            }
        }
      //  Debug.LogError(atkcount + " " + atkquancount);
    }

    void Update()
    {

    }
    FightObject whofightme;
    List<FightObject> fightwholist;
    public Vector3 backuppos,shanbackuppos;
    public void fight(List<FightObject> fightwholist)
    {
        fighttype = CTTools.rd.Next(100) % 2;
      //  Debug.Log(fc.nowaction.skillid);

        if(fc.nowaction.skillid<= 10000)
        {
            if (data_equip_skillDef.dicdatas[fc.nowaction.skillid].special_name != "0")
            {
                Debug.LogError(data_equip_skillDef.dicdatas[fc.nowaction.skillid].special_name);
                addxgspine(data_equip_skillDef.dicdatas[fc.nowaction.skillid].special_name);
            }

            data_equipBean dbdata2 = data_equipDef.dicdatas2[fc.nowaction.skillid][0];

            addxgskill(dbdata2.icon);

            

            for (int i = 0; i < fightwholist.Count; i++)
            {
                fightwholist[i].whofightme = this;
                fightwholist[i].aida(-1, fc.nowaction, atkindex, atkcount, atkquancount);
            }
            ftsta = FT_STAND;
            return;
        }


        fighttype = data_skillDef.dicdatas[fc.nowaction.skillid].skill_type - 1;
        fighttypestr = data_skillDef.dicdatas[fc.nowaction.skillid].action;
        backuppos = gameObject.transform.localPosition;

        if (data_skillDef.dicdatas[fc.nowaction.skillid].skillname_picture != "0")
        {
            addxgtex(data_skillDef.dicdatas[fc.nowaction.skillid].skill_name);
        }
        if (data_skillDef.dicdatas[fc.nowaction.skillid].special_name != "0")
        {
            addxgspine(data_skillDef.dicdatas[fc.nowaction.skillid].special_name);
        }

        if (fighttypestr == "0")
        {
            for (int i = 0; i < fightwholist.Count; i++)
            {
                fightwholist[i].whofightme = this;
                fightwholist[i].aida(-1, fc.nowaction, atkindex, atkcount, atkquancount);

                //if (fightwholist[i].fbean.hp <= 0)
                //{
                //    //setSta(FT_WIN);
                //    fc.setTingDun(4);
                //}
            }
            ftsta = FT_STAND;
            return;
        }else if (fighttypestr.IndexOf("skill") != -1)
        {
          //  string sname = soundtou + "_skill0" + CTTools.rd.Next(1, 4);
          //  SoundPlayer.getInstance().PlayW(sname, false);
        }

        ftsta = FT_FIGHT;
        this.fightwholist = fightwholist;

    }
    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.data.name.Equals("move"))
        {
            int duan = e.intValue;
          //  Debug.LogError(duan);
            gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(duan * agroot.transform.localScale.x, 0, 0);
        }else if (e.data.name.Equals("attack"))
        {
            //if (fighttypestr.IndexOf("attack") != -1)
            //    SoundPlayer.getInstance().PlayW(soundtou+ "_attack01", false);

            int duan = e.intValue;

            for (int i = 0; i < fightwholist.Count; i++)
            {
                fightwholist[i].whofightme = this;
                fightwholist[i].aida(duan,fc.nowaction, atkindex,atkcount,atkquancount);

                if (fightwholist[i].fbean.hp <= 0)
                {
                    //setSta(FT_WIN);
                    fc.setTingDun(4);
                }
            }
            if (!fc.nowaction.hitresult)
            {
               // setSta(FT_DAKONG);
            }
            atkindex++;
            //if (duan == 0)
            //{
            //    for (int i = 0; i < fightwholist.Count; i++)
            //    {
            //        fightwholist[i].setSta(FT_BEIDA);
            //    }
            //}else if (duan == 1)
            //{
            //    for (int i = 0; i < fightwholist.Count; i++)
            //    {
            //        fightwholist[i].setSta(FT_BEIDATUI);
            //    }
            //}
            //else if (duan == 2)
            //{
            //    for (int i = 0; i < fightwholist.Count; i++)
            //    {
            //        fightwholist[i].setSta(FT_TUI);
            //    }
            //}
            //else if (duan == 3)
            //{
            //    for (int i = 0; i < fightwholist.Count; i++)
            //    {
            //        fightwholist[i].setSta(FT_FEI);
            //    }
            //}
        }else if (e.data.name.Equals("shock"))
        {
            fc.setShock(0, 0, 0, true);
        }
        else if (e.data.name.Equals("acoustics"))
        {
            if (ftsta == FT_FIGHT)
            {
                SoundPlayer.getInstance().PlayW(e.stringValue, false);

            }
            else
            {
              //  SoundPlayer.getInstance().PlayW(soundtou + "_hurt01", false);
            }

           // SoundPlayer.getInstance().PlayW(e.stringValue, false);
          //  Debug.Log("playsound======" + e.stringValue);
        }
        //if (e.data.name.Equals("fall"))
        //{

        //    for (int i = 0; i < fightwholist.Count; i++)
        //    {
        //        fightwholist[i].setSta(FT_FEI);
        //    }

        //}
        //else if (e.data.name.Equals("repel"))
        //{

        //    for (int i = 0; i < fightwholist.Count; i++)
        //    {
        //        fightwholist[i].setSta(FT_TUI);
        //    }

        //}
        //else if (e.data.name.Equals("Hit"))
        //{

        //    for (int i = 0; i < fightwholist.Count; i++)
        //    {
        //        fightwholist[i].setSta(FT_BEIDA);
        //    }

        //}
    }
    public int hongdelay = 2;
    public static int[] attackquan = { 1, 2, 4, 8 };
    public void aida(int type, FightBeanAction nowaction, int atkindex,int atkcount,int atkquancount)
    {
        if (nowaction.hitresult)
        {
            if (nowaction.resultaction != null)
            {
                if (type == -1)
                {
                    int zhi = Mathf.CeilToInt(nowaction.resultaction.hpcost);
                    addxgpiaozi(0, nowaction.doubleresult ? 1 : 0, -zhi);
                    fbean.hp -= zhi;
                }
                else
                {
                    int zhi = Mathf.CeilToInt(nowaction.resultaction.hpcost * attackquan[type] * 1.0f / atkquancount);
                    addxgpiaozi(0, nowaction.doubleresult ? 1 : 0, -zhi);
                    fbean.hp -= zhi;
                }
               

                if (nowaction.exskillmsg == 22)
                {
                    fbean.hp = 1;
                }


                if (nowaction.skillid == 26)
                {
                    // fbean.hp = 1;
                    setSta(FT_STAND);
                }

                if (whofightme != this)
                {
                    SoundPlayer.getInstance().PlayW(soundtou + "_hurt01", false);
                    if (whofightme.fighttypestr.IndexOf("skill") != -1)
                    {
                        if(GameGlobal.fireworksOn)
                            Handheld.Vibrate();
                    }
                        
                }
                 

                addxgaida(whofightme);

                spaintnode.setColor(new Color(0xff / 255.0f, 0xcb / 255.0f, 0x17 / 255.0f));
                hongdelay = 7;

                if (type == 0)
                {
                    setSta(FT_BEIDA);
                }
                else if (type == 1)
                {
                    setSta(FT_BEIDATUI);
                }
                else if (type == 2)
                {
                    setSta(FT_TUI);
                }
                else if (type == 3)
                {
                    setSta(FT_FEI);
                }

                if (fbean.hp <= 0)
                {
                    FightControl.jiasulevel = -1;

                    if(fc.nextaction != null && fc.nextaction.skillid == 26)
                    {
                        zhensiflag = false;
                        FightControl.jiasulevel = 1;
                    }
                    else
                    {
                        zhensiflag = true;

                        Invoke("jiasuend", 1.5f);
                    }
                }
            }

            if (atkindex == atkcount-1|| type == -1)
            {
                if (nowaction.buffactionlist.Count>0)
                {
                    for(int i=0;i< nowaction.buffactionlist.Count; i++)
                    {
                        FightBeanAction buffaction = nowaction.buffactionlist[i];
                        FightObject buffto = fc.getfightobjectbyid(buffaction.toid);
                        if (buffto != null)
                        {
                            buffto.refreshbuff(buffaction.buffopttype, buffaction.buffid, buffaction.bufftime, buffaction.buffeff0);
                        }
                    }

                   
                }
            }

        }
        else
        {
            setSta(FT_SHANBI);
            addxgpiaozi(5, 0,-1);
        }

    }



    public void jiasuend()
    {
        FightControl.jiasulevel = 1;
    }
    public List<FightXG> xgzilist = new List<FightXG>();
    private void addxgpiaozi(int sk, int ok, int zhi)
    {

        FightXG fxg = null;
        GameObject preobj = ResManager.getGameObject("allpre", "vfxgzi");
        fxg = preobj.GetComponent<FightXG>();
        fxg.create(0, null, fc.fightmap);

        fxg.iinitpiaozi(0, sk, ok, this, null, 0, zhi);

        //if (fxg.piaotype == 2)
        //{
        //    ziceng++;
        //    zinum++;
        //}


        xgzilist.Add(fxg);
    }
    private void addxgtex(string texname)
    {

        FightXG fxg = null;
        GameObject preobj = ResManager.getGameObject("allpre", "vfxg");
        fxg = preobj.GetComponent<FightXG>();
        fxg.create(0, null, fc.fightmap);

        fxg.inittextoworld(this, texname , 1000, Vector3.zero);


        xgzilist.Add(fxg);
    }
    private void addxgspine(string texname)
    {

        FightXG fxg = null;
        GameObject preobj = ResManager.getGameObject("allpre", "vfxg");
        fxg = preobj.GetComponent<FightXG>();
        fxg.create(0, null, this.agroot);

        fxg.iinit5buf2(this, texname);


        xgzilist.Add(fxg);
    }

    private void addxgskill(string texname)
    {

        FightXG fxg = null;
        GameObject preobj = ResManager.getGameObject("allpre", "vfxg");
        fxg = preobj.GetComponent<FightXG>();
        fxg.create(0, null, fc.fightmap);

        fxg.initskillxg(this, texname);


        xgzilist.Add(fxg);
    }
    private void addxgaida(FightObject whofightme)
    {
        FightXG fxg = null;
        GameObject preobj = ResManager.getGameObject("allpre", "vfxg");
        fxg = preobj.GetComponent<FightXG>();
        fxg.create(0, null, this.agroot);

        fxg.iinit3buf(this, data_fight_positionDef.dicdatas[whofightme.agname][0].effects_hittarget, data_fight_positionDef.dicdatas[agname][0].effects_hittarget_position);


        xgzilist.Add(fxg);
    }
    public void xglogic(float dt)
    {


        for (int i = 0; i < xgzilist.Count; i++)
        {
            xgzilist[i].logic(dt);
            if (xgzilist[i].over)
            {
                 Destroy(xgzilist[i].gameObject);
                xgzilist.RemoveAt(i);
                i--;
            }
        }


    }

    public JumpTo jt, jth;
    public TJumpTo tjt;
    public void setSta(int s)
    {
        if (ftsta == FT_FEI)
        {
            if (s == FT_TUI || s == FT_BEIDATUI)
            {
                return;
            }
        }

        if (ftsta == s)
        {
            if (ftsta == FT_BEIDA)
            {
               // spaintnode.playAction2Force("injured", false);
            }
            else
             if (ftsta == FT_BEIDATUI)
            {
                tjt.setv(fc.putongtuivx, 0, true);
            }
            else if(ftsta == FT_TUI)
            {
                tjt.setv(fc.tuivx, 0, true);
            }
            else
             if (ftsta == FT_FEI)
            {

                tjt.setv(fc.feivx, fc.feidefeivy);
            }
            return;
        }
        ftsta = s;


        if (ftsta == FT_BEIDATUI)
        {
            tjt = gameObject.GetComponent<TJumpTo>();
            if (tjt == null)
            {
                tjt = gameObject.AddComponent<TJumpTo>();
                tjt.sethroot(agroot);
            }
            tjt.setv(fc.putongtuivx, 0, true);
        }
        else
        if (ftsta == FT_TUI)
        {
            //if (daotuifeipos.x <= gameObject.transform.localPosition.x)
            //{
            //    agroot.transform.localScale = new Vector3(-1, 1, 1);
            //}
            //else
            //{
            //    agroot.transform.localScale = new Vector3(1, 1, 1);
            //}

            // tuistopdelaytime = 0.1f;

            tjt = gameObject.GetComponent<TJumpTo>();
            if (tjt == null)
            {
                tjt = gameObject.AddComponent<TJumpTo>();
                tjt.sethroot(agroot);
            }
            tjt.setv(fc.tuivx, 0, true);


            //jt = gameObject.GetComponent<JumpTo>();
            //if (jt == null)
            //    jt = gameObject.AddComponent<JumpTo>();

            //jt.creat(gameObject.transform.localPosition - new Vector3(agroot.transform.localScale.x * 200, 0, 0), 0, 1, 0.2f, true, false);

        }
        else
        if (ftsta == FT_FEI)
        {

            //{
            //    jt = gameObject.GetComponent<JumpTo>();
            //    if (jt == null)
            //        jt = gameObject.AddComponent<JumpTo>();

            //    {
            //        jt.creat(gameObject.transform.localPosition - new Vector3(agroot.transform.localScale.x * 250, 0, 0), 0, 1, 0.45f, true, false);
            //    }


            tjt = gameObject.GetComponent<TJumpTo>();
            if (tjt == null)
            {
                tjt = gameObject.AddComponent<TJumpTo>();
                tjt.sethroot(agroot);
            }
            tjt.setv(fc.feivx, fc.feivy);


            //}
            //{
            //    jth = agroot.GetComponent<JumpTo>();
            //    if (jth == null)
            //        jth = agroot.AddComponent<JumpTo>();

            //    jth.creat(new Vector3(0, 0, 0), 200, 1, 0.45f, true, false);
            //}
        }
        else if (ftsta == FT_DAKONG)
        {
            
                jt = gameObject.GetComponent<JumpTo>();
                if (jt == null)
                    jt = gameObject.AddComponent<JumpTo>();

                {
                    jt.creat(backuppos, 0, 1, 0.45f, true, false);
                }
        }
        else if (ftsta == FT_SHANBI)
        {
            shanbackuppos = gameObject.transform.localPosition;
            //jt = gameObject.GetComponent<JumpTo>();
            //if (jt == null)
            //    jt = gameObject.AddComponent<JumpTo>();

            //{
            //    jt.creat(gameObject.transform.localPosition - new Vector3(agroot.transform.localScale.x * 150, 0, 0), 0, 1, 0.10f, true, false);
            //}
        }else if (ftsta == FT_SHANHUI)
        {
            jt = gameObject.GetComponent<JumpTo>();
            if (jt == null)
                jt = gameObject.AddComponent<JumpTo>();

            {
                jt.creat(shanbackuppos, 0, 1, 0.10f, true, false);
            }
        }else if (ftsta == FT_DEAD)
        {
            // whofightme.setSta(FT_WIN);
            SoundPlayer.getInstance().PlayW(soundtou + "_death01", false);
        }
        else if (ftsta == FT_WIN)
        {
            SoundPlayer.getInstance().PlayW(soundtou + "_win01", false);
        }

    }
    public FightObjBean fbean;
    public string soundtou;
    public void setBean(FightObjBean attacker)
    {
        fbean = attacker;
        fbean.maxhp = fbean.hp;


        GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
        spaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
        if (attacker.fType==1)
        {
            spaintnode.create1(agroot, data_heroDef.getdatabyheroid(attacker.heroID).resource, data_heroDef.getdatabyheroid(attacker.heroID).resource);

            string sname = attacker.heroID + "_star01" 
              //  + CTTools.rd.Next(1, 4)
                ;
            haojiaoname = sname;
            soundtou = attacker.heroID + "";
          //  SoundPlayer.getInstance().PlayW(sname, false);

            agname = data_heroDef.getdatabyheroid(attacker.heroID).resource;
        }
        else
        {
            spaintnode.create1(agroot, data_npcDef.getdatabynpcid(attacker.heroID).resource, data_npcDef.getdatabynpcid(attacker.heroID).resource);


            string sname = data_npcDef.getdatabynpcid(attacker.heroID).resource + "_star01";
            soundtou = data_npcDef.getdatabynpcid(attacker.heroID).resource;
           // SoundPlayer.getInstance().PlayW(sname, false);
            haojiaoname = sname;
            agname = data_npcDef.getdatabynpcid(attacker.heroID).resource;
        }
        spaintnode.transform.localScale = Vector3.one * 0.24f;
      //  spaintnode.playAction2Auto("idle", true);
        //  spaintnode.setSkin(i, 0);
        spaintnode.addEvent(HandleEvent);
        if (zhen == 0)
        {
            gameObject.transform.localPosition = new Vector3(1340-250, 900, 0);
            agroot.transform.localScale = new Vector3(1, 1, 1);
            // zhen = 0;
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(1340+250, 900, 0);
            agroot.transform.localScale = new Vector3(-1, 1, 1);
            // zhen = 1;
        }
        gameObject.name = "f" + attacker.heroID;
        ftsta = 0;

        logevent();

    }
    public string haojiaoname;
    public void haojiao()
    {
        SoundPlayer.getInstance().PlayW(haojiaoname, false);
    }
    private void logevent()
    {
        for (int i = 0; i < spaintnode.zhengmian.animation.state.Data.skeletonData.Animations.Count; i++)
        {
          //  Debug.Log(i + " " + spaintnode.zhengmian.animation.state.Data.skeletonData.Animations.Items[i].name + " " + spaintnode.zhengmian.animation.state.Data.skeletonData.Animations.Items[i].duration);
            Spine.Animation a = spaintnode.zhengmian.animation.state.Data.skeletonData.Animations.Items[i];
            for (int j = 0; j < a.timelines.Count; j++)
            {
                Spine.Timeline t = a.timelines.Items[j];
                if (t.GetType() == typeof(Spine.EventTimeline))
                {
                  //  Debug.Log("shijiantimeline");

                    Spine.EventTimeline tt = (Spine.EventTimeline)t;
                  //  Debug.Log(tt.frames.Length);
                  //  Debug.Log(tt.Events.Length);
                    for (int i0 = 0; i0 < tt.Events.Length; i0++)
                    {
                        if (tt.Events[i0] != null)
                        {
                          //  Debug.Log(i0 + "  " + tt.Events[i0].ToString() + "  " + tt.frames[i0]);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < spaintnode.zhengmian.animation.state.Data.skeletonData.Events.Count; i++)
        {
           // Debug.Log(i + " " + spaintnode.zhengmian.animation.state.Data.skeletonData.Events.Items[i].name);
        }
    }
    //第二位buff状态  0 结算 1 清除  2创建
    public List<FightBuff> bufflist = new List<FightBuff>();
    public void refreshbuff(int opttype, int buffid, long btime,int duzhi)
    {
        if (opttype == 0)
        {
            for (int i = 0; i < bufflist.Count; i++)
            {
                if (bufflist[i].buffid == buffid)
                {
                    bufflist[i].settime(btime);
                    break;
                }
            }
        }
        else if (opttype == 1)
        {
            for (int i = 0; i < bufflist.Count; i++)
            {
                if (bufflist[i].buffid == buffid)
                {
                    Destroy(bufflist[i].gameObject);
                    bufflist.RemoveAt(i);
                    break;
                }
            }
            FightControl.instance.fui.refreshbufficon();
        }
        else if (opttype == 2)
        {
            for (int i = 0; i < bufflist.Count; i++)
            {
                if (bufflist[i].buffid == buffid)
                {
                    bufflist[i].settime(btime);
                    return;
                }
            }

            GameObject preobj = ResManager.getGameObject("allpre", "vfb");
            FightBuff fxg = preobj.GetComponent<FightBuff>();
            fxg.create(0, null, agroot1);
            fxg.iinit(buffid,btime, this);

            bufflist.Add(fxg);

            FightControl.instance.fui.refreshbufficon();
        }

        if (buffid == 0&&duzhi>0)
        {
            addxgpiaozi(0, 0, -duzhi);
            fbean.hp -= duzhi;
          //  Debug.LogError(fbean.id + "->" + duzhi);
            if (fbean.hp <= 0)
            {
                FightControl.jiasulevel = -1;

                if (fc.nextaction != null && fc.nextaction.skillid == 26)
                {
                    zhensiflag = false;
                    FightControl.jiasulevel = 1;
                }
                else
                {
                    zhensiflag = true;

                    Invoke("jiasuend", 1.5f);
                }
            }
        }else if (buffid == 5 && duzhi > 0)
        {
            addxgpiaozi(0, 0, duzhi);
            fbean.hp += duzhi;
            //  Debug.LogError(fbean.id + "->" + duzhi);           
        }

    }
    public int getbuffNum(int type)
    {

        int temp = 0;
        for (int i = 0; i < bufflist.Count; i++)
        {
            if (type == -1)
            {
                temp++;
            }
            else
            {
                if (bufflist[i].buffid == type)
                {
                    temp++;
                }
            }

        }
        return temp;
    }
    //public List<FightBuffBean> bufflist = new List<FightBuffBean>();
    //public void refreshbuff(int opttype,int buffid,long btime)
    //{
    //    if (opttype == 0)
    //    {
    //        for (int i = 0; i < bufflist.Count; i++)
    //        {
    //            if (bufflist[i].buffid == buffid)
    //            {
    //                bufflist[i].time = btime;
    //                break;
    //            }
    //        }
    //    }else if (opttype == 1)
    //    {
    //        for (int i = 0; i < bufflist.Count; i++)
    //        {
    //            if (bufflist[i].buffid == buffid)
    //            {
    //                bufflist.RemoveAt(i);
    //                break;
    //            }
    //        }
    //    }else if (opttype == 2)
    //    {
    //        FightBuffBean fbb = new FightBuffBean();
    //        fbb.buffid = buffid;
    //        fbb.time = btime;
    //        bufflist.Add(fbb);
    //    }

    //}


}
