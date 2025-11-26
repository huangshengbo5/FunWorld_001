using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightControl : MonoBehaviour
{
    public static FightControl instance;
    public const int FC_READY = 0;
    public const int FC_SELECT = 1;
    public const int FC_MOVE = 2;
    public const int FC_FIGHT = 3;
    public const int FC_END = 4;

    public FightUI fui;

    public GameObject fightmap;
    public GameObject fightbg, fightbgfrontfrontnode;
    public UITexture fightbgfront;
    public UITexture fightbgbg;
    public UITexture fightbgfrontfront;

    public GameObject ruchangnode;
    public UILabel ruchanglabel;

    private APaintNodeSpine xgpaintnode;

    public int fcsta = 0;
    public int fcsmsta = 0;
    public List<FightObject> fightlist = new List<FightObject>();
    public FightObject nowfighter;
    public List<FightObject> fightwholist = new List<FightObject>();
    public Vector3 backpos, movetopos, v;

    public List<FightXG> xglist = new List<FightXG>();

    public int[] testid = new int[2];

    public int putongtuivx = 20;
    public int tuivx = 25;
    public int feivx = 30;
    public int feivy = 65;
    public int feidefeivy = 40;
    public float vax = 1.4f;
    public float g = 6;



    public bool isPauseBattle = false;

    void Awake()
    {
        instance = this;
        fui.setFC(this);
        FightControl.jiasulevel = 1;
        sta = -2;
        delay = 120;

        {
            GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
            xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
            xgpaintnode.create1(fui.gameObject, "ZhanDouKaiShi", "ZhanDouKaiShi");
            if (fbtype == 5 && GameGlobal.fromTianqiFight)
            {
                // if (GameGlobal.fromTianqiFight)
                {
                    xgpaintnode.playActionAuto(GameGlobal.gamedata.tianqiReportIndex + 1, false, () =>
                    {
                        xgpaintnode.gameObject.SetActive(false);
                        setSta(-22);
                    });
                }

            }
            else
            {
                xgpaintnode.playActionAuto(0, false, () =>
                {
                    xgpaintnode.gameObject.SetActive(false);
                    setSta(-22);
                });
            }

            xgpaintnode.setdepth(1000);
            xgpaintnode.transform.localPosition = new Vector3(0, 0, 0);
            xgpaintnode.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
    // Use this for initialization
    void Start()
    {
        if (debugflag)
        {
            fui.gameObject.SetActive(true);
            init();
            return;
        }

        TJumpTo.mocaa = vax;
        TJumpTo.g = g;
        if (fb.type == 6)
        {
            isPauseBattle = true;
            //新手引导，出背景出对话
            showBattleBg();
            fui.gameObject.SetActive(false);
            Debug.Log("播放对话=====================" + 10001);
            UIManager.showDialogue(10001, OnPlayDialogueStep0);
        }
        else
        {
            isPauseBattle = false;
            fui.gameObject.SetActive(true);
            init();
        }
        MusicPlayer.getInstance().PlayW("Battle", true);
    }

    #region 新手引导使用写死逻辑,只在type是6的战斗中用

    private void OnPlayDialogueStep0()
    {
        UIManager.showDialogue(10002, OnPlayDialogueStep1);
    }

    private void OnPlayDialogueStep1()
    {
        //进入战斗
        isPauseBattle = false;
        fui.gameObject.SetActive(true);
        init();
    }

    private void PlayGuideEndDialogue()
    {
        fui.gameObject.SetActive(false);
        //进入战斗
        UIManager.showDialogue(10003, OnPlayDialogueStep4);
    }

    private void OnPlayDialogueStep4()
    {
        //进入战斗
        UIManager.showDialogue(10004, OnPlayDialogueStep5);
    }

    private void OnPlayDialogueStep5()
    {
        //进入战斗
        GameGlobal.enterMenuScene();
    }

    #endregion

    void Update()
    {
        //  ftlogic();
        //logic(Time.deltaTime);
        if (fb.type == 6)
        {
            if (!isPauseBattle)
            {
                logic();
            }
        }
        else
        {
            logic();
        }
    }
    public static int jiasulevel = 1;
    public int sta = -2;
    public int delay = 40;
    public void logic()
    {
        if (sta == -2)
        {


            delay--;
            if (delay < 0)
            {
                //  setSta(-1);
            }
            // return;
        }
        else if (sta == -22)
        {


            delay--;
            if (delay < 0)
            {
                setSta(-1);
            }
            // return;
        }
        else
        if (sta == -1)
        {
            //  ruchangpaintnode.logic(0.0333f);
            //  return;



        }
        else if (sta == 10)
        {
            //  chuchangpaintnode.logic(0.0333f);
        }

        if (jiasulevel == -1)
        {
            logic(0.007f);
        }
        else
        {
            // for (int i = 0; i < jiasulevel; i++)
            logic(0.0333f);
            //logic(Time.deltaTime);

        }

    }

    public void setSta(int v)
    {
        // throw new NotImplementedException();
        sta = v;

        if (sta == -22)
        {
            for (int i = 0; i < fightlist.Count; i++)
            {
                fightlist[i].haojiao();
            }
        }
    }

    int tingduntimes = 0;
    public void setTingDun(int td)
    {
        tingduntimes = td;
    }

    int shockRemain = 0;
    int shockType = 1;
    int shockY = 0;
    int shockX = 0;
    int shock_displace = 1;
    bool shuaijian = false;
    bool shockUp = true;
    bool shockLeft = true;
    void Shock()
    {

        shockRemain--;

        //if (shockRemain % 2 == 1)
        //{
        //    shockY = shock_displace;

        //}
        //else
        //{
        //    shockY = -shock_displace;
        //}

        //if (shockType == 1)
        //{ // V形震平屏
        //    if (shockLeft)
        //    {
        //        shockX = shockUp ? shockY : -shockY;
        //    }
        //    else
        //    {
        //        shockX = shockUp ? -shockY : shockY;
        //    }
        //}

        //if (shockType == 2)
        //{//横阵

        //    shockY = 0;
        //    if (shockRemain % 2 == 1)
        //    {
        //        shockX = shock_displace;
        //    }
        //    else
        //    {
        //        shockX = -shock_displace;
        //    }
        //}

        if (shockType == 0)
        {
            shockY = 0;
            if (shockindex < shockyarr.Length)
            {
                shockY = shockyarr[shockindex];
                shockindex++;
            }

        }
        else if (shockType == 1)
        {
            shockY = 0;
            if (shockindex < shockyarr0.Length)
            {
                shockY = shockyarr0[shockindex];
                shockindex++;
            }
        }

        fightmap.transform.localPosition = fightmap.transform.localPosition + new Vector3(shockX, shockY);

        if (shuaijian)
        {
            if (shock_displace > 1)
            {
                shock_displace--;
            }
        }


    }

    public int[] shockyarr = { 0, 20, -20, 10, -10, 0 };
    public int[] shockyarr0 = { 0, 30, -30, 20, -20, 5, -5, 0 };
    public int shockindex;
    public void setShock(int times, int weiyi, int type, bool sj)
    {

        shock_displace = weiyi;
        shockType = type;
        shockY = 0;
        shockX = 0;
        shuaijian = sj;
        shockindex = 0;

        if (type == 0)
        {
            shockRemain = shockyarr.Length;
        }
        else
        {
            shockRemain = shockyarr0.Length;
        }
    }
    public static float mapwidth = 2680;
    public static float mapheight = 2340;
    public void logic(float dt)
    {

        if (shockRemain > 0)
        {
            Shock();
        }
        else
        {

            Vector3 tempd = objcamscale;
            if (mapwidth * objcamscale.x < CTGlobal.realWidth || mapheight * objcamscale.x < CTGlobal.realHeight)
            {
                float f0 = Mathf.Max(CTGlobal.realWidth / mapwidth, CTGlobal.realHeight / mapheight);
                //Debug.Log(CTGlobal.realWidth / mapwidth + " " + CTGlobal.realHeight +" "+ mapheight);
                tempd = Vector3.one * f0;
            }


            fightmap.transform.localScale = tempd;

            Vector3 temp = Vector3.zero - objcampos - objcamscaleoffset - mapcampos;
            if (temp.x > 0)
            {
                temp.x = 0;
            }
            if (temp.x < CTGlobal.realWidth - mapwidth * tempd.x)
            {
                temp.x = CTGlobal.realWidth - mapwidth * tempd.x;
            }

            if (temp.y > 0)
            {
                temp.y = 0;
            }
            if (temp.y < CTGlobal.realHeight - mapheight * tempd.x)
            {
                temp.y = CTGlobal.realHeight - mapheight * tempd.x;
            }

            fightmap.transform.localPosition = temp;

            fightbgbg.transform.localPosition = new Vector3(-temp.x / 2 - 650, fightbgbg.transform.localPosition.y, 0);
            fightbgfrontfront.transform.localPosition = new Vector3(temp.x * 1.5f, fightbgfrontfront.transform.localPosition.y, 0);

        }

        {
            float delta = Time.deltaTime;
            // delta = 0.1f;

            objcampos = Vector3.Lerp(objcampos, objcamposfocus, delta * smooth);
            objcamscale = Vector3.Lerp(objcamscale, objcamscalefocus, delta * smooth);
            objcamscaleoffset = Vector3.Lerp(objcamscaleoffset, objcamscaleoffsetfocus, delta * smooth);
            mapcampos = Vector3.Lerp(mapcampos, mapcamfocus, delta * smooth0);
        }
        //  Vector3 position = fightmap.transform.InverseTransformPoint();

        //jiaodian = zhujue.transform.localPosition - new Vector3(CTGlobal.realWidth / 2, CTGlobal.realHeight / 2, 0);
        //fightmap.transform.localPosition = -jiaodian;

        if (tingduntimes > 0)
        {
            tingduntimes--;
            return;
        }

        if (sta != -2 && sta != -22)
            ftlogic(dt);

        if (nowfighter != null)
        {
            // setjingtou(nowfighter, 1);

            Vector3 zupos = GetCenterPoint();
            setjingtou(zupos, 1);
        }

        for (int i = 0; i < fightlist.Count; i++)
        {
            fightlist[i].logic(dt);
        }

        for (int i = 0; i < xglist.Count; i++)
        {
            xglist[i].logic(dt);
            if (xglist[i].over)
            {
                Destroy(xglist[i].gameObject);
                xglist.RemoveAt(i);
                i--;
            }
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            // nowfighter = fightarray[0];

            // setShock(4, 3, 1, false);

            // setjingtou(zhujue, 1.5f);

            // reset();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // nowfighter = fightarray[0];

            // setShock(4, 3, 1, false);

            // setjingtou(fightlist[1], 2.0f);

            // reset();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // nowfighter = fightarray[0];

            // setShock(4, 3, 1, false);

            //setjingtou(null, 1.0f);

            // reset();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            setShock(4, 10, 1, false);
        }


        if (Input.GetKeyDown(KeyCode.H))
        {
            // fightlist[1].attr.addhp(-1000000);
        }

    }
    bool showle = false;
    public void showresult()
    {
        if (showle)
        {
            return;
        }

        showle = true;

        if (fbtype == 5)
        {
            if (GameGlobal.fromTianqiFight)
            {
                GameGlobal.gamedata.tianqiReportIndex += 1;
                int index = GameGlobal.gamedata.tianqiReportIndex;
                if (index <= 2)
                {
                    if (GameGlobal.gamedata.tianqiReport.Count > index)
                    {
                        fui.showheiping(() =>
                        {
                            FightControl.fb = GameGlobal.gamedata.tianqiReport[index];
                            FightControl.fbtype = 5;
                            GameGlobal.enterFightScene(false);
                        });
                        return;
                    }
                }
                List<Reward> list = GameGlobal.gamedata.tianqiRewards;
                bool isWin = GameGlobal.gamedata.tianqiWin;
                UIManager.showFightResult(isWin, list);

            }
            return;
        }

        if (fbtype == 6)
        {
            PlayGuideEndDialogue();
            return;
        }

        if (fb.win)
        {
            if (fbtype == 1)
            {
                UIManager.showFightWin(fb.rewards, 1);
            }
            else
            {
                UIManager.showFightWin(fb.rewards);
            }

            //fightlist[0].setSta(FightObject.FT_WIN);
            //fightlist[1].setSta(FightObject.FT_DEAD);
        }
        else
        {
            GameGlobal.gamedata.stageId = -1;
            UIManager.showFightLose();
            //fightlist[0].setSta(FightObject.FT_DEAD);
            //fightlist[1].setSta(FightObject.FT_WIN);


            //fui.showheiping(() =>
            //{
            //    GameGlobal.gamedata.stageId = -1;
            //    UIManager.showFightLose();
            //});
        }
    }

    private void setfcsta(int fcs)
    {
        fcsta = fcs;
        fcsmsta = 0;
    }
    public static int fbtype = 0;
    public static Monster monster;
    public static string tempjson = "{\"Type\":1,\"AttackerID\":1111,\"AttackerName\":\"崔亮\",\"AttackerLevel\":10,\"DefenderID\":2222,\"DefenderName\":\"加班哥\",\"DefenderLevel\":5,\"Actions\":[[0, 0, 1111, 2222, 0, 10013, false, false],[1, true, 1111, 2222, 117],[3],[0, 0, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 19],[3],[0, 0, 2222, 1111, 0,10033, true, true],[1, true, 2222, 1111, 235],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 1000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 99],[3],[0, 1000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 16],[3],[0, 1000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500,0],[3],[0, 2000, 1111, 2222, 0, 10011, true, false],[1, true, 1111, 2222, 83],[3],[0, 2000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 16],[3],[0, 2000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 3000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 3000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 4000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 4000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 4000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 5000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 5000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[2, 1, 2222, 8, 0, 0, 0, 6000],[0, 6000, 1111, 2222, 0, 10011, false, false],[3],[0, 6000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 7000, 1111, 2222, 0, 10013, false, true],[3],[0, 7000, 1111, 2222, 0, 10012, false, true],[3],[0, 7000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 8000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 99],[3],[0, 8000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 9000, 1111, 2222, 0, 10013, true, false],[1,true, 1111, 2222, 99],[3],[0, 9000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[2, 1, 2222, 8, 0, 0, 0, 10000],[0, 10000, 1111, 2222, 0, 10011, false, false],[3],[0, 10000, 1111, 2222, 0, 10012, false, false],[3],[0, 10000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 11000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 196],[3],[0, 11000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 39],[3],[0, 11000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 12000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 99],[3],[0, 12000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 13000, 1111, 2222, 0, 10013, false, true],[3],[0, 13000, 1111, 2222, 0, 10012, false, true],[3],[0, 13000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[2, 1, 2222, 8, 0, 0, 0, 14000],[0, 14000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 117],[3],[0, 14000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 19],[3],[0, 14000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 15000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 196],[3],[0, 15000, 2222, 1111, 0, 10033, true, true],[1, true, 2222, 1111, 235],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 15000, 2222, 1111, 0, 10022, true, true],[2, 2, 1111, 1, 1000, 0, 0],[3],[2, 1, 1111, 1, 0, 0, 0, 16000],[2, 0, 1111, 1, 0, 0, 0, 16000],[0, 16000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 17000, 1111, 2222, 0, 10011, true, false],[1, true, 1111, 2222, 83],[3],[0, 17000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 17000, 2222, 1111, 0, 10022, true, false],[2, 2, 1111, 1, 1000, 0, 0],[3],[2, 1, 1111, 1, 0, 0, 0, 18000],[2, 0, 1111, 1, 0, 0, 0, 18000],[0, 18000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 19000, 1111, 2222, 0, 10011, true, false],[1, true, 1111, 2222, 83],[3],[0, 19000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 16],[3],[0, 19000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 20000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 20000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 20000, 2222, 1111, 0, 10033, true, true],[1, true, 2222, 1111, 235],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 21000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 21000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 21000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 22000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 22000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 22000, 2222, 1111, 0, 10022, true, true],[2, 2, 1111, 1, 1000, 0, 0],[3],[2, 1, 1111, 1, 0, 0, 0, 23000],[2, 0, 1111, 1, 0, 0, 0, 23000],[0, 23000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[2, 1, 2222, 8, 0, 0, 0, 24000],[0, 24000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 235],[3],[0, 24000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 39],[3],[0, 24000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 25000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 117],[3],[0, 25000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 19],[3],[0, 25000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 26000, 1111, 2222, 0, 10011, false, false],[3],[0, 26000, 1111, 2222, 0, 10012, false, false],[3],[0,26000, 2222, 1111, 0, 10033, true, true],[1, true, 2222, 1111, 235],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 27000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 99],[3],[0, 27000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 16],[3],[0, 27000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 28000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 28000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 28000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 29000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 29000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 30000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0,30000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 30000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000,1500, 0],[3],[0, 31000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 31000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 31000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 32000, 1111, 2222, 0, 10011, false, false],[3],[0, 32000, 1111, 2222, 0, 10012, false, false],[3],[0, 32000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 33000,1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 33000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 33000, 2222, 1111, 0, 10033, true, true],[1, true, 2222, 1111, 86],[2, 2, 2222, 8, 3000, 1500, 0],[3]],\"Result\":{\"Result\":1,\"AttackerHP\":0,\"DefenderHP\":857,\"Background\":1}}";
    public static FightBean fb;
    public FightBeanAction nowaction, nextaction;
    public int fbactindex = 0;
    public float fbdelay = 0;

    public static string zhanbaofile = "0929.txt";
    public static bool debugflag = false;
    private void init()
    {
        if (debugflag)
        {
            tempjson = ResManager.getText("ftest/" + zhanbaofile);
            Debug.Log(tempjson.Length);
            fb = (FightBean)XLH.readToObject(tempjson, "FightBean");
            fb.win = true;
        }

        showle = false;
        fb.format();
        Debug.LogError(fb.actions.Length);

        fbactindex = 0;
        fbdelay = 0;
        nowaction = fb.actionlist[fbactindex];
        if (fbactindex < fb.actionlist.Count - 2)
        {
            nextaction = fb.actionlist[fbactindex + 1];
        }
        else
        {
            nextaction = null;
        }

        {
            GameObject preobj = ResManager.getGameObject("allpre", "vfobj");
            FightObject pp0 = preobj.GetComponent<FightObject>();
            pp0.create(0, this, fightmap);
            pp0.init(0, 0);
            pp0.setBean(fb.attacker);
            fightlist.Add(pp0);
        }
        {
            GameObject preobj = ResManager.getGameObject("allpre", "vfobj");
            FightObject pp0 = preobj.GetComponent<FightObject>();
            pp0.create(0, this, fightmap);
            pp0.init(1, 1);
            pp0.setBean(fb.defender);
            fightlist.Add(pp0);
        }

        fui.init(fightlist);




        Vector3 zupos = GetCenterPoint();
        setjingtou(zupos, 1);
        objcampos = objcamposfocus;


        fightbgbg.mainTexture = ResManager.getTex("texres/fightbg" + fb.background + "1.png");
        fightbgbg.MakePixelPerfect();
        fightbgfront.mainTexture = ResManager.getTex("texres/fightbg" + fb.background + "0.png");


        GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
        spaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
        spaintnode.create1(fightbgfront.gameObject, agname1[fb.background], agname1[fb.background]);
        spaintnode.transform.localPosition = new Vector3(1365, 1100, 0);
        spaintnode.transform.localScale = Vector3.one;
        spaintnode.playActionAuto(0, false, () =>
        {
            playrdmaction();
        });
        spaintnode.setdepth(4);
        if (fb.rewards == null && fbtype != 1)
        {
            var selectdbbean = data_campaignDef.datas[GameGlobal.gamedata.stageindex];
            fb.rewards = Reward.decodeList(selectdbbean.rewards);
        }
    }

    private void playrdmaction()
    {
        spaintnode.playActionAuto(CTTools.rd.Next(spaintnode.getActionNum()), false, () =>
        {
            playrdmaction();
        });
    }

    public static string[] agname1 = { "", "cunWai", "senLin", "shaMo" };
    public APaintNodeSpine spaintnode;
    //显示战斗背景
    private void showBattleBg()
    {
        Vector3 zupos = GetCenterPoint();
        setjingtou(zupos, 1);
        objcampos = objcamposfocus;


        fightbgbg.mainTexture = ResManager.getTex("texres/fightbg" + fb.background + "1.png");
        fightbgbg.MakePixelPerfect();
        fightbgfront.mainTexture = ResManager.getTex("texres/fightbg" + fb.background + "0.png");


    }

    private void ftlogic(float dt)
    {
        switch (fcsta)
        {
            case FC_READY:
                readylogic();
                break;
            case FC_SELECT:
                selectlogic(dt);
                break;
            case FC_MOVE:
                movelogic();
                break;
        }


    }

    public FightObject getfightobjectbyid(int toid)
    {
        for (int i = 0; i < fightlist.Count; i++)
        {
            if (fightlist[i].fbean.id == toid)
            //  if (fightlist[i].zhen != nowfighter.zhen)
            {
                return fightlist[i];
            }
        }
        return null;
    }

    private void readylogic()
    {
        for (int i = 0; i < fightlist.Count; i++)
        {
            fightlist[i].fightle = false;
        }
        fightwholist.Clear();
        nowfighter = null;
        setfcsta(FC_SELECT);
    }
    private void selectlogic(float dt)
    {
        if (nowfighter != null && nowfighter.ftsta != FightObject.FT_STAND)
        {
            return;
        }

        fbdelay += dt;
        fui.time -= (long)(dt * 1000);
        if (fbdelay > nowaction.timestamp / 1000.0f)
        {
            fui.time = 2 * 60 * 1000L - nowaction.timestamp;
            if (nowaction.type == 0)
            {
                for (int i = 0; i < fightlist.Count; i++)
                {
                    if (fightlist[i].fbean.id == nowaction.fromid)
                    {
                        nowfighter = fightlist[i];
                        break;
                    }
                }

                fightwholist.Clear();
                for (int i = 0; i < fightlist.Count; i++)
                {
                    if (fightlist[i].fbean.id == nowaction.toid)
                    //  if (fightlist[i].zhen != nowfighter.zhen)
                    {
                        fightwholist.Add(fightlist[i]);
                        break;
                    }
                }
                setfcsta(FC_MOVE);
            }
            else if (nowaction.type == 2)
            {
                //for (int i = 0; i < fightlist.Count; i++)
                //{
                //    if (fightlist[i].fbean.id == nowaction.toid)
                //    {
                //        nowfighter = fightlist[i];
                //        break;
                //    }
                //}
                nowfighter = getfightobjectbyid(nowaction.toid);
                nowfighter.refreshbuff(nowaction.buffopttype, nowaction.buffid, nowaction.bufftime, nowaction.buffeff0);
                tonextaction();

            }
        }
    }

    private void tonextaction()
    {
        fbactindex++;
        // fbdelay = 0;
        if (fbactindex >= fb.actionlist.Count)
        {
            setfcsta(FC_END);
            if (fb.win)
            {
                //  UIManager.showFightWin();
                //fightlist[0].setSta(FightObject.FT_WIN);
                //fightlist[1].setSta(FightObject.FT_DEAD);
            }
            else
            {
                // UIManager.showFightLose();
                //fightlist[0].setSta(FightObject.FT_DEAD);
                //fightlist[1].setSta(FightObject.FT_WIN);
            }
        }
        else
        {
            nowaction = fb.actionlist[fbactindex];
            Debug.LogError(nowaction);
            if (fbactindex < fb.actionlist.Count - 2)
            {
                nextaction = fb.actionlist[fbactindex + 1];
            }
            else
            {
                nextaction = null;
            }
        }

    }

    private void selectlogicbackup(float dt)
    {
        for (int i = 0; i < fightlist.Count; i++)
        {
            if (!fightlist[i].fightle)
            {
                nowfighter = fightlist[i];
                break;
            }
        }

        nowfighter = fightlist[CTTools.rd.Next(100) % fightlist.Count];

        fightwholist.Clear();
        for (int i = 0; i < fightlist.Count; i++)
        {
            if (fightlist[i].zhen != nowfighter.zhen)
            {
                fightwholist.Add(fightlist[i]);
                break;
            }
        }
        setfcsta(FC_MOVE);
    }

    private void movelogic()
    {
        switch (fcsmsta)
        {
            case 0:
                if (nowfighter.ftsta == FightObject.FT_STAND)
                {
                    backpos = nowfighter.transform.localPosition;
                    movetopos = fightwholist[0].transform.localPosition;
                    v = movetopos - backpos;
                    v = v / 10;
                    fcsmsta = 1;
                    nowfighter.setSta(15);

                    if (nowaction.skillid > 10000)
                    {
                        string fighttypestr = data_skillDef.dicdatas[nowaction.skillid].action;

                        if (fighttypestr == "0")
                        {
                            fcsmsta = 2;
                        }
                    }


                }

                break;
            case 1:
                nowfighter.transform.localPosition += v;
                if (Vector3.Distance(nowfighter.transform.localPosition, movetopos) < 300)
                {
                    fcsmsta = 2;
                }
                break;
            case 11://shanup
                break;
            case 12://shandown
                break;
            case 2:
                nowfighter.fight(fightwholist);
                fcsmsta = 3;
                break;
            case 3:
                if (nowfighter.ftsta == FightObject.FT_STAND)
                {
                    //v = backpos - nowfighter.transform.localPosition;
                    //v = v / 10;
                    //fcsmsta = 4;

                    fcsmsta = 5;
                }

                break;
            case 4:
                nowfighter.transform.localPosition += v;
                if (Vector3.Distance(nowfighter.transform.localPosition, backpos) < 10)
                {
                    fcsmsta = 5;
                }
                break;
            case 44://shanhui
                break;
            case 5:
                nowfighter.fightle = true;
                if (endle())
                {
                    setfcsta(FC_END);
                }
                //else if (lunend())
                //{
                //    setfcsta(FC_READY);
                //}
                else
                {
                    setfcsta(FC_SELECT);
                    tonextaction();
                }
                break;
        }
    }
    private bool lunend()
    {
        for (int i = 0; i < fightlist.Count; i++)
        {
            if (!fightlist[i].fightle)
            {
                return false;
            }
        }
        return true;
    }

    private bool endle()
    {
        for (int i = 0; i < fightlist.Count; i++)
        {
            if (fightlist[i].zhen != nowfighter.zhen)
            {
                if (!fightlist[i].sile)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public Vector3 mapcampos = Vector3.zero;
    public Vector3 mapcamfocus = Vector3.zero;

    public Vector3 objcampos = Vector3.zero;
    Vector3 objcamposfocus;
    Vector3 objcamscale = Vector3.one;
    Vector3 objcamscalefocus = Vector3.one;
    Vector3 objcamscaleoffset = Vector3.zero;
    Vector3 objcamscaleoffsetfocus = Vector3.zero;


    float smooth = 4;
    float smooth0 = 2;
    public void setjingtou(FightObject zhujue, float fd, float sm = 4)
    {

        if (zhujue == null)
        {
            smooth = 12;
            objcamscalefocus = Vector3.one;
            objcamposfocus = Vector3.zero;
            objcamscaleoffsetfocus = Vector3.zero;
        }
        else
        {
            smooth = sm;
            objcamscalefocus = Vector3.one * fd;
            objcamposfocus = fightmap.transform.InverseTransformPoint(zhujue.transform.position) - mapcampos - new Vector3(CTGlobal.realWidth / 2, CTGlobal.realHeight / 2, 0) + new Vector3(0, 0, 0);
            objcamscaleoffsetfocus = fightmap.transform.InverseTransformPoint(zhujue.transform.position) * (fd - 1);

            // Debug.LogError(objcamposfocus+" "+ fightmap.transform.InverseTransformPoint(zhujue.transform.position)+" "+ new Vector3(CTGlobal.realWidth / 2, CTGlobal.realHeight / 2, 0));
        }
    }
    public void setjingtou(Vector3 zhujuepos, float fd, float sm = 4)
    {

        smooth = sm;
        objcamscalefocus = Vector3.one * fd;
        objcamposfocus = zhujuepos - mapcampos - new Vector3(CTGlobal.realWidth / 2, CTGlobal.realHeight / 2, 0) + new Vector3(0, 0, 0);
        objcamscaleoffsetfocus = zhujuepos * (fd - 1);


    }

    public Vector3 GetCenterPoint()
    {
        List<Vector3> p = new List<Vector3>();

        for (int i0 = 0; i0 < fightlist.Count; i0++)
        {
            // if (fightlist[i0].zhen == 0 && fightlist[i0].mainflag && !fightlist[i0].sile)
            {
                p.Add(fightlist[i0].transform.localPosition);
            }
        }


        Vector3 ptCenter = new Vector3(0, 0);
        int i, j;
        double ai, atmp = 0, xtmp = 0, ytmp = 0;

        if (p.Count == 1)
            return p[0];
        if ((p.Count == 2) || (p.Count == 3 && p[0] == p[2]))
            return new Vector3((p[1].x + p[0].x) / 2, (p[1].y + p[0].y) / 2);

        int n = p.Count;
        for (i = n - 1, j = 0; j < n; i = j, j++)
        {
            ai = p[i].x * p[j].y - p[j].x * p[i].y;
            atmp += ai;
            xtmp += (p[j].x + p[i].x) * ai;
            ytmp += (p[j].y + p[i].y) * ai;
        }

        if (atmp != 0)
        {
            ptCenter.x = Convert.ToInt32(xtmp / (3 * atmp));
            ptCenter.y = Convert.ToInt32(ytmp / (3 * atmp));

        }
        return ptCenter;

    }

    void OnDestroy()
    {
        fightbgbg.mainTexture = null;
        fightbgfront.mainTexture = null;
        Resources.UnloadUnusedAssets();
    }
}
