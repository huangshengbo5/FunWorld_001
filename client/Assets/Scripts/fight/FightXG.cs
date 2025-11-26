using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightXG : MonoBehaviour {

    public GameObject agroot, agroot1;



    private APaintNodeSpine xgpaintnode;


    public APaintNodeSpine renpaintnode;
    public float alpha;

    public GameObject labelgen, spritegen, texgen;



    public ParticleSystem[] particle;
    public UIParticleNode[] particlenodes;


     public static NGUIFont[] font;

    int nowAction = 0;


    float jumpheight;
    Vector3 delta, previousPos, startPosition, endPosition;
    int jumpnum;
    float elapsed, duration;

    int actiondelay;
    int delayindex = 0;
    int delaytotal;



    public FightControl fc;
    public void create(int v, FightControl fightControl, GameObject target)
    {
        fc = fightControl;

        //  gameObject.transform.parent = target.transform;

        gameObject.transform.SetParent(target.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);



        if (font == null)
        {
            font = new NGUIFont[6];
            for (int i = 0; i < font.Length; i++)
            {
                font[i] = Resources.Load("Atlas/FightFonts/FightFont" + i) as NGUIFont;

            }
        }


    }

    public int bigkind;
    public int smallkind;
    public int otherkind;
    public string actionname;
    public int fx;
    public FightObject rolefrom;
    public FightObject roleto;
    public int sta;
    public bool over = false;

    //    ff2a00 红 被击
    //00ff12 绿 治疗
    //ea00ff 紫 暴击
    //f6cf08 黄 能量
//    普通攻击 描边3e2a25
//加血 上c5ec53下70b832 描边1e4900
//暴击 上dd3e0c下ffe895 描边360500
//能量 上f9fc55下dfa33a 描边dfa33a
    public static string[][] zicolorarry = new string[5][]
    {
       new string[3]{ "[ffffff]","[ffffff]","[3e2a25]" },
       new string[3]{ "[c5ec53]", "[70b832]", "[1e4900]" },
       new string[3]{ "[dd3e0c]", "[ffe895]", "[360500]" },
       new string[3]{ "[f9fc55]", "[dfa33a]", "[602616]" },
       new string[3]{ "[ffffff]","[ffffff]","[ffffff]" },
    };
    public static string[] zicolor = new string[5]
{

        "[ffffff]",
                "[ea00ff]",

                "[ff2a00]",
        "[f6cf08]",

                "[00ff12]",

};
    public static Vector3[] zipos = new Vector3[7]
    {
        new Vector3(0,0,0),
         new Vector3(-40,-20,0),
          new Vector3(40,-40,0),
           new Vector3(-40,-40,0),
            new Vector3(-20,-50,0),
             new Vector3(-60,-170,0),
              new Vector3(-90,-210,0),

    };
    public int piaotype = 0;
    public void iinitpiaozi(int k, int sk, int ok, FightObject rfrom, FightObject rto, int delayindex, int delaytotal)
    {
        bigkind = k;
        smallkind = sk;
        otherkind = ok;
        rolefrom = rfrom;
        roleto = rto;


        sta = 0;

        labelgen.transform.localPosition = new Vector3(0, 0, 0);
        labelgen.GetComponentInChildren<UILabel>().alpha = 1;
      //  gameObject.GetComponent<UIPanel>().depth = rfrom.gameObject.GetComponent<UIPanel>().depth + 1;
        gameObject.transform.localPosition = rfrom.transform.localPosition + new Vector3(0, 300, 0);

        UILabel paolabel = labelgen.GetComponentInChildren<UILabel>();
        paolabel.depth = 1000;
        int piaotype = 0;
        if (sk == 5)
        {
            paolabel.bitmapFont = font[5];
            paolabel.text = "&";
        }else if (sk == 0)
        {
            paolabel.bitmapFont = font[0];

            if (delaytotal > 0)
            {
                paolabel.bitmapFont = font[1];
                paolabel.text = "+" + delaytotal;
            }
            else
            {
                paolabel.text = "" + delaytotal;
            }



            if (ok == 1)
            {
                paolabel.text = "&" + delaytotal;
            }
        }

        if (piaotype == 0)
        {
            labelgen.gameObject.transform.localScale = new Vector3(1, 1, 1);

            TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
            tc.from = labelgen.transform.localPosition;
            tc.to = new Vector3(0, 100, 0);
            tc.duration = 0.6f;
            tc.delay = 0.07f + 0.1f * delayindex;
            tc.PlayForward();



            TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
            ts.from = new Vector3(1, 1, 1);
            ts.to = new Vector3(1.5f, 1.5f, 1.5f);
            ts.duration = 0.6f;
            ts.delay = 0.07f + 0.1f * delayindex;
            ts.PlayForward();

            EventDelegate.Add(tc.onFinished, delegate ()
            {
                Destroy(tc);
                Destroy(ts);
                over = true;
            });
        }
        else if (piaotype == 1)
        {
            labelgen.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
            ts.from = new Vector3(0.1f, 0.1f, 0.1f);
            ts.to = new Vector3(1.5f, 1.5f, 1.5f);
            ts.duration = 0.2f;
            ts.delay = 0.07f;
            ts.PlayForward();


            TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
            tc.from = labelgen.transform.localPosition;
            tc.to = new Vector3(0, 100, 0);
            tc.duration = 0.5f;
            tc.delay = 0.47f;
            tc.PlayForward();

            EventDelegate.Add(tc.onFinished, delegate ()
            {
                Destroy(tc);
                Destroy(ts);
                over = true;
            });
        }


        /*

        if (bigkind == 0||bigkind==10)
        {
            over = false;

            labelgen.transform.localPosition = new Vector3(0, 0, 0);
            labelgen.GetComponentInChildren<UILabel>().alpha = 1;
            gameObject.GetComponent<UIPanel>().depth = rfrom.gameObject.GetComponent<UIPanel>().depth + 1;
            gameObject.transform.localPosition = rfrom.transform.localPosition+ new Vector3(0, 250, 0);

         

            UILabel paolabel = gameObject.GetComponentInChildren<UILabel>();

          //  paolabel.fontSize = 50;
          //  paolabel.spacingX =-3;
            paolabel.gameObject.transform.localPosition = new Vector3(0, 0, 0);

            paolabel.fontSize = 27;
            piaotype = 0;

            if (sk == -1)//miss
            {

                 paolabel.bitmapFont = font[5];
               // paolabel.color = NGUIText.ParseColor(zicolor[0], 1);
               paolabel.gradientTop= NGUIText.ParseColor(zicolorarry[0][0], 1);
                paolabel.gradientBottom = NGUIText.ParseColor(zicolorarry[0][1], 1);
                paolabel.effectColor= NGUIText.ParseColor(zicolorarry[0][2], 1);


                paolabel.text = "Miss";
                // paolabel.fontSize = 36;
                paolabel.text = "&";
            }
            else if (sk == 1)//jiamp
            {
                 paolabel.bitmapFont = font[3];
                //  paolabel.color = NGUIText.ParseColor(zicolor[3], 1);
                paolabel.gradientTop = NGUIText.ParseColor(zicolorarry[3][0], 1);
                paolabel.gradientBottom = NGUIText.ParseColor(zicolorarry[3][1], 1);
                paolabel.effectColor = NGUIText.ParseColor(zicolorarry[3][2], 1);

                paolabel.text = "[i]" + delaytotal;

                piaotype = 2;
            }
            else if (sk ==0)
            {
                if (ok == 1) //baoji
                {
                      paolabel.bitmapFont = font[1];
                    // paolabel.color = NGUIText.ParseColor(zicolor[1], 1);


                    paolabel.gradientTop = NGUIText.ParseColor(zicolorarry[2][0], 1);
                    paolabel.gradientBottom = NGUIText.ParseColor(zicolorarry[2][1], 1);
                    paolabel.effectColor = NGUIText.ParseColor(zicolorarry[2][2], 1);

                    paolabel.text = "[i]" + Math.Abs(delaytotal);

                   // labelgen.GetComponentInChildren<UILabel>().fontSize = 93;

                    piaotype = 1;
                 //   piaotype = 2;
                }
                else if (ok == 0)
                {
                    if (delaytotal > 0)//jiaxie
                    {
                        // paolabel.color = NGUIText.ParseColor(zicolor[4], 1);
                          paolabel.bitmapFont = font[4];

                        paolabel.gradientTop = NGUIText.ParseColor(zicolorarry[1][0], 1);
                        paolabel.gradientBottom = NGUIText.ParseColor(zicolorarry[1][1], 1);
                        paolabel.effectColor = NGUIText.ParseColor(zicolorarry[1][2], 1);

                        paolabel.text = "[i]" + delaytotal;

                        piaotype = 2;
                    }
                    else//shanghai
                    {
                        if (rolefrom.zhen == 0)
                        {
                            // paolabel.bitmapFont = font[0];
                          //  paolabel.color = NGUIText.ParseColor(zicolor[2], 1);
                        }
                        else
                        {
                            // paolabel.bitmapFont = font[2];
                           // paolabel.color = NGUIText.ParseColor(zicolor[0], 1);
                        }

                        paolabel.bitmapFont = font[2];

                        paolabel.gradientTop = NGUIText.ParseColor(zicolorarry[0][0], 1);
                        paolabel.gradientBottom = NGUIText.ParseColor(zicolorarry[0][1], 1);
                        paolabel.effectColor = NGUIText.ParseColor(zicolorarry[0][2], 1);

                        paolabel.text = "[i]" + Math.Abs(delaytotal);

                        // piaotype = 1;
                        // piaotype = 2;
                    }
                }
              
            }

            //if (piaotype == 2)
            //{
            //    agroot.transform.localPosition = zipos[(rfrom.zinum % 2) + 5];
            //    rfrom.zinum++;
            //}
            //else
            {
                // agroot.transform.localPosition = zipos[rfrom.ziceng % 5];
                //  rfrom.ziceng++;
                Vector3 temppos = new Vector3(CTTools.rd.Next(-50, 50), CTTools.rd.Next(-50, 50), 0);
                agroot.transform.localPosition = temppos;
            }




            // gameObject.transform.localScale = Vector3.one*0.5f;

            NGUITools.MarkParentAsChanged(gameObject);



            UITweener[] uts = labelgen.GetComponentsInChildren<UITweener>();
            for (int i = 0; i < uts.Length; i++)
            {
                Destroy(uts[i]);
            }

//            普通攻击：0——9（Y: 80 Alpha: 100）——12（Y: 85 Alpha: 0）
//回血回能：0——2（Y: 0 缩放: 1.2）——4(缩放: 1）——14(Alpha: 100)——24（Y: 100 Alpha: 0）
//暴击: 0——1（缩放1.5）——2（缩放1.6）——3（缩放1.2）——12（Y: 0 Alpha: 100）——20（Y: 20 Alpha: 0）
            if (piaotype == 0)
            {
                labelgen.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                {
                    TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
                    tc.from = labelgen.transform.localPosition;
                    tc.to = new Vector3(0, 80, 0);
                    tc.duration = 9.0f / 30;
                    tc.delay = 0;
                    tc.PlayForward();
                }
                {
                    TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
                    tc.from = new Vector3(0, 80, 0);
                    tc.to = new Vector3(0, 85, 0);
                    tc.duration = 4.0f / 30;
                    tc.delay = 9.0f / 30;
                    tc.PlayForward();
                }

                {
                    TweenAlpha ta = labelgen.gameObject.AddComponent<TweenAlpha>();
                    ta.from = 1;
                    ta.to = 0;
                    ta.duration = 4.0f / 30;
                    ta.delay = 9.0f / 30;
                    ta.PlayForward();
                    EventDelegate.Add(ta.onFinished, delegate ()
                    {
                        over = true;
                    });
                }

            }else if (piaotype == 2)
            {
                labelgen.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                {
                    TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
                    ts.from = new Vector3(1.0f, 1.0f, 1.0f);
                    ts.to = new Vector3(1.2f, 1.2f, 1.0f);
                    ts.duration = 2.0f / 30;
                    ts.delay = 0;
                    ts.PlayForward();
                }
                {
                    TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
                    ts.from = new Vector3(1.2f, 1.2f, 1.0f);
                    ts.to = new Vector3(1.0f, 1.0f, 1.0f);
                    ts.duration = 2.0f / 30;
                    ts.delay = 2.0f / 30;
                    ts.PlayForward();
                }
                {
                    TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
                    tc.from = labelgen.transform.localPosition;
                    tc.to = new Vector3(0, 100, 0);
                    tc.duration = 10.0f / 30;
                    tc.delay = 14.0f / 30;
                    tc.PlayForward();
                }
                {
                    TweenAlpha ta = labelgen.gameObject.AddComponent<TweenAlpha>();
                    ta.from = 1;
                    ta.to = 0;
                    ta.duration = 10.0f / 30;
                    ta.delay = 14.0f / 30;
                    ta.PlayForward();
                    EventDelegate.Add(ta.onFinished, delegate ()
                    {
                        over = true;
                    });
                }
            }else if (piaotype == 1)
            {
                labelgen.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                {
                    TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
                    ts.from = new Vector3(1.0f, 1.0f, 1.0f);
                    ts.to = new Vector3(1.5f, 1.5f, 1.0f);
                    ts.duration = 1.0f / 30;
                    ts.delay = 0;
                    ts.PlayForward();
                }
                {
                    TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
                    ts.from = new Vector3(1.5f, 1.5f, 1.0f);
                    ts.to = new Vector3(1.6f, 1.6f, 1.0f);
                    ts.duration = 1.0f / 30;
                    ts.delay = 1.0f / 30;
                    ts.PlayForward();
                }
                {
                    TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
                    ts.from = new Vector3(1.6f, 1.6f, 1.0f);
                    ts.to = new Vector3(1.2f, 1.2f, 1.0f);
                    ts.duration = 1.0f / 30;
                    ts.delay = 2.0f / 30;
                    ts.PlayForward();
                }
                {
                    TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
                    tc.from = labelgen.transform.localPosition;
                    tc.to = new Vector3(0, 20, 0);
                    tc.duration = 8.0f / 30;
                    tc.delay = 12.0f / 30;
                    tc.PlayForward();
                }
                {
                    TweenAlpha ta = labelgen.gameObject.AddComponent<TweenAlpha>();
                    ta.from = 1;
                    ta.to = 0;
                    ta.duration = 8.0f / 30;
                    ta.delay = 12.0f / 30;
                    ta.PlayForward();
                    EventDelegate.Add(ta.onFinished, delegate ()
                    {
                        over = true;
                    });
                }
            }


            //labelgen.transform.localScale = new Vector3(2.0f, 0.5f, 0);
            //{
            //    TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
            //    ts.from = new Vector3(2.0f, 0.5f, 0);
            //    ts.to = new Vector3(0.5f, 2.0f, 0);
            //    ts.duration = 3.0f / 30;
            //    ts.delay =0;
            //    ts.PlayForward();
            //}
            //{
            //    TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
            //    ts.from = new Vector3(0.5f, 2.0f, 0);
            //    ts.to = Vector3.one;
            //    ts.duration = 3.0f / 30;
            //    ts.delay = 3.0f / 30;
            //    ts.PlayForward();
            //}
            //{
            //    TweenAlpha ta= labelgen.gameObject.AddComponent<TweenAlpha>();
            //    ta.from = 1;
            //    ta.to = 0;
            //    ta.duration = 10.0f / 30;
            //    ta.delay = 10.0f / 30;
            //    ta.PlayForward();

            //    EventDelegate.Add(ta.onFinished, delegate ()
            //    {
            //        over = true;
            //    });
            //}
            //if (piaotype == 2)
            //{
            //    {
            //        TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
            //        tc.from = labelgen.transform.localPosition;
            //        tc.to = new Vector3(0, 180, 0);
            //        tc.duration = 20.0f / 30;
            //        tc.delay = 0;
            //        tc.PlayForward();
            //    }
            //}
            //else
            //{
            //    {
            //        TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
            //        tc.from = labelgen.transform.localPosition;
            //        tc.to = new Vector3(0, 50, 0);
            //        tc.duration = 10.0f/30;
            //        tc.delay = 10.0f/30;
            //        tc.PlayForward();
            //    }
            //}




            //if (piaotype == 0)
            //{
            //    labelgen.gameObject.transform.localScale = new Vector3(1, 1, 1);

            //    TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
            //    tc.from = labelgen.transform.localPosition;
            //    tc.to = new Vector3(0, 100, 0);
            //    tc.duration = 0.6f;
            //    tc.delay = 0.07f + 0.1f * delayindex;
            //    tc.PlayForward();



            //    TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
            //    ts.from = new Vector3(1, 1, 1);
            //    ts.to = new Vector3(1.5f, 1.5f, 1.5f);
            //    ts.duration = 0.3f;
            //    ts.delay = 0.07f + 0.1f * delayindex;
            //    ts.PlayForward();

            //    EventDelegate.Add(tc.onFinished, delegate ()
            //    {
            //        Destroy(tc);
            //        Destroy(ts);
            //        over = true;
            //    });
            //}
            //else if (piaotype == 1)
            //{
            //    labelgen.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            //    TweenScale ts = labelgen.gameObject.AddComponent<TweenScale>();
            //    ts.from = new Vector3(0.5f, 0.5f, 0.5f);
            //    ts.to = new Vector3(1.2f, 1.2f, 1.2f);
            //    ts.duration = 0.05f;
            //    ts.delay = 0.01f + 0.1f * delayindex;
            //    ts.PlayForward();


            //    TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
            //    tc.from = labelgen.transform.localPosition;
            //    tc.to = new Vector3(0, 100, 0);
            //    tc.duration = 0.3f;
            //    tc.delay = 0.15f + 0.1f * delayindex;
            //    tc.PlayForward();

            //    EventDelegate.Add(tc.onFinished, delegate ()
            //    {
            //        Destroy(tc);
            //        Destroy(ts);
            //        over = true;
            //    });
            //}else if (piaotype == 2)
            //{
            //    labelgen.gameObject.transform.localScale = new Vector3(1, 1, 1);
            // //   Debug.LogError(delayindex);
            //    labelgen.transform.localPosition = new Vector3(0, (delayindex%7)*55, 0);
            //    TweenPosition tc = labelgen.gameObject.AddComponent<TweenPosition>();
            //    tc.from = labelgen.transform.localPosition;
            //    tc.to = labelgen.transform.localPosition;
            //    tc.duration = 0.1f;
            //    tc.delay = 0.47f + 0.1f ;
            //    tc.PlayForward();

            //    EventDelegate.Add(tc.onFinished, delegate ()
            //    {
            //        Destroy(tc);
            //        over = true;
            //    });
            //}


        }

        //if (FightControl.jiasulevel == 2)
        //{
        //    UITweener[] tps = labelgen.GetComponents<UITweener>();
        //    for (int i = 0; i < tps.Length; i++)
        //    {
        //        tps[i].timeScale = 2.0f;
        //    }
        //}

    */


    }
    //public void iinit2(int dk, FightObject rfrom, FightObject rto)
    //{
    //    bigkind = 1;
    //    gameObject.transform.localPosition = new Vector3(0, 0, 0);
    //    GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
    //    xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
    //    xgpaintnode.create1(agroot, "fly", "Fly");
    //    xgpaintnode.playAction2(FightBulletDef.datas[dk].ActionBeHit, false);
    //    xgpaintnode.setdepth(100);
    //    xgpaintnode.transform.localPosition = new Vector3(0, 100, 0);
    //    xgpaintnode.gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
    //}

    public void iinit3buf(FightObject rfrom,string actionname,int dep)
    {
        bigkind = 2;
        rolefrom = rfrom;
        this.actionname = actionname;
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
        xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
        xgpaintnode.create1(agroot, "jiZhong", "jiZhong");
        xgpaintnode.playAction2(actionname, false);
        xgpaintnode.setdepth(rfrom.spaintnode.dp + 1);
        xgpaintnode.transform.localPosition = new Vector3(0, dep, 0);
        xgpaintnode.gameObject.transform.localScale = rfrom.spaintnode.transform.localScale;
    }
    public void iinit5buf(FightObject rfrom, string actionname, int dep)
    {
        bigkind = 5;
        rolefrom = rfrom;
        this.actionname = actionname;
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
        xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
        xgpaintnode.create1(agroot, "Moe3Buff", "Moe3Buff");
        xgpaintnode.playAction2(actionname, true);
        xgpaintnode.setdepth(dep);
        xgpaintnode.transform.localPosition = new Vector3(0, 0, 0);
        xgpaintnode.gameObject.transform.localScale = rfrom.spaintnode.transform.localScale;
        sta = 0;
        alpha = 0.0f;
        xgpaintnode.setAlpha(0);
    }
    public void iinit5buf2(FightObject rfrom, string actionname)
    {
        bigkind = 2;
        rolefrom = rfrom;
        this.actionname = actionname;
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
        xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
        xgpaintnode.create1(agroot, actionname, actionname);
        xgpaintnode.playAction(0, false);
       // xgpaintnode.playAction2(actionname, true);
        xgpaintnode.setdepth(rfrom.spaintnode.dp+1);
        if (actionname.IndexOf("jingHua") != -1)
        {
            xgpaintnode.transform.localPosition = new Vector3(0, data_fight_positionDef.dicdatas[rfrom.agname][0].effects_purify_position, 0);
        }
        else if (actionname.IndexOf("jiaSu") != -1)
        {
            xgpaintnode.transform.localPosition = new Vector3(0, data_fight_positionDef.dicdatas[rfrom.agname][0].effects_accelerate_position, 0);
        }
        else if (actionname.IndexOf("xuanyun") != -1)
        {
            xgpaintnode.transform.localPosition = new Vector3(0, data_fight_positionDef.dicdatas[rfrom.agname][0].effects_vertigo_position, 0);
        }
        else if (actionname.IndexOf("faguang") != -1)
        {
            xgpaintnode.transform.localPosition = new Vector3(0, data_fight_positionDef.dicdatas[rfrom.agname][0].effects_luminescence_position, 0);
        }
        else
        {
            xgpaintnode.transform.localPosition = new Vector3(0, data_fight_positionDef.dicdatas[rfrom.agname][0].effects_hittarget_position, 0);
        }
        
        xgpaintnode.gameObject.transform.localScale =Vector3.one;

    }
    public void iinitlizi(FightObject rfrom, string actionname, int dep,float duration)
    {
        bigkind = 20;
        rolefrom = rfrom;
        this.actionname = actionname;
        gameObject.transform.localPosition = new Vector3(0, 100, 0);

        GameObject texframeobjsyan = ResManager.getGameObjectNoInit("allpre", "lizi/" + actionname);
        texframeobjsyan = NGUITools.AddChild(agroot, texframeobjsyan);
        //  texframeobjsyan.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        particle = texframeobjsyan.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particle.Length; i++)
        {
            particle[i].Play();
        }

        particlenodes = texframeobjsyan.GetComponentsInChildren<UIParticleNode>();
        for(int i = 0; i < particlenodes.Length; i++)
        {
            particlenodes[i].depth = dep+i;
        }
       

        this.duration = duration;
    }
    public void iinitlizitoworld(FightObject rfrom, string actionname, int dep, Vector3 pos)
    {
        bigkind = 20;
        rolefrom = rfrom;
        this.actionname = actionname;
        gameObject.transform.localPosition = rfrom.transform.localPosition+pos;

        GameObject texframeobjsyan = ResManager.getGameObjectNoInit("allpre", "lizi/" + actionname);
        texframeobjsyan = NGUITools.AddChild(agroot, texframeobjsyan);
        //  texframeobjsyan.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        particle = texframeobjsyan.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particle.Length; i++)
        {
            particle[i].Play();
        }
        gameObject.AddComponent<UIPanel>().depth = rfrom.gameObject.GetComponent<UIPanel>().depth + 10;
        particlenodes = texframeobjsyan.GetComponentsInChildren<UIParticleNode>();
        for (int i = 0; i < particlenodes.Length; i++)
        {
            particlenodes[i].depth = dep + i;
        }


        this.duration = particle[0].main.duration;

     

    }
    public void inittextoworld(FightObject rfrom, string actionname, int dep, Vector3 pos)
    {
        GameObject texframeobjs = ResManager.getGameObject("allpre", "vlabel");
        UILabel temppaintnode = texframeobjs.GetComponent<UILabel>();
        // temppaintnode.pivot = UIWidget.Pivot.TopLeft;
        temppaintnode.color = Color.yellow;
        temppaintnode.text = actionname;
        temppaintnode.MakePixelPerfect();
        temppaintnode.transform.parent = agroot.transform;
        temppaintnode.transform.localPosition = Vector3.zero;
        temppaintnode.transform.localScale = Vector3.one;
        temppaintnode.depth = 1000;

        gameObject.transform.localPosition = rfrom.transform.localPosition + new Vector3(0, 400, 0);


        TweenPosition tc = agroot.gameObject.AddComponent<TweenPosition>();
        tc.from = agroot.transform.localPosition;
        tc.to = new Vector3(0, 100, 0);
        tc.duration = 0.6f;
        tc.delay = 0.07f + 0.1f * delayindex;
        tc.PlayForward();



        //TweenScale ts = agroot.gameObject.AddComponent<TweenScale>();
        //ts.from = new Vector3(1, 1, 1);
        //ts.to = new Vector3(1.5f, 1.5f, 1.5f);
        //ts.duration = 0.6f;
        //ts.delay = 0.07f + 0.1f * delayindex;
        //ts.PlayForward();

        EventDelegate.Add(tc.onFinished, delegate ()
        {
            Destroy(tc);
            //   Destroy(ts);
            over = true;
        });
    }
    public void inittextoworld2(FightObject rfrom, string actionname, int dep, Vector3 pos)
    {
        GameObject texframeobjs = ResManager.getGameObject("allpre", "umodtex");
        UITexture temppaintnode = texframeobjs.GetComponent<UITexture>();
       // temppaintnode.pivot = UIWidget.Pivot.TopLeft;
        temppaintnode.mainTexture = getTex(actionname);
        temppaintnode.MakePixelPerfect();
        temppaintnode.transform.parent = agroot.transform;
        temppaintnode.transform.localPosition = Vector3.zero;
        temppaintnode.transform.localScale = Vector3.one;
        temppaintnode.depth = 1000;

        gameObject.transform.localPosition = rfrom.transform.localPosition + new Vector3(0, 400, 0);


        TweenPosition tc = agroot.gameObject.AddComponent<TweenPosition>();
        tc.from = agroot.transform.localPosition;
        tc.to = new Vector3(0, 100, 0);
        tc.duration = 0.6f;
        tc.delay = 0.07f + 0.1f * delayindex;
        tc.PlayForward();



        //TweenScale ts = agroot.gameObject.AddComponent<TweenScale>();
        //ts.from = new Vector3(1, 1, 1);
        //ts.to = new Vector3(1.5f, 1.5f, 1.5f);
        //ts.duration = 0.6f;
        //ts.delay = 0.07f + 0.1f * delayindex;
        //ts.PlayForward();

        EventDelegate.Add(tc.onFinished, delegate ()
        {
            Destroy(tc);
         //   Destroy(ts);
            over = true;
        });
    }

    public void initskillxg(FightObject rfrom, string actionname)
    {
        GameObject texframeobjs = ResManager.getGameObject("allpre", "skillxg");
        texframeobjs.transform.parent = agroot.transform;
        texframeobjs.transform.localPosition = Vector3.zero;
        texframeobjs.transform.localScale = Vector3.one;

        {
            GameObject texframeobjs0 = ResManager.getGameObject("allpre", "vtexpaintnode");
            TexPaintNode temppaintnode = texframeobjs0.GetComponent<TexPaintNode>();
            string path = "equip";
            temppaintnode.create1(texframeobjs.GetComponentInChildren<UITexture>().gameObject, path);
            temppaintnode.setdepth(1502);
            temppaintnode.setShowRectLimit(80);
            temppaintnode.playAction(actionname);
            temppaintnode.gameObject.SetActive(true);
        }


        gameObject.transform.localPosition = rfrom.transform.localPosition + new Vector3(0, 400, 0);


        TweenPosition tc = agroot.gameObject.AddComponent<TweenPosition>();
        tc.from = agroot.transform.localPosition;
        tc.to = new Vector3(0, 100, 0);
        tc.duration = 0.6f;
        tc.delay = 0.07f + 0.1f * delayindex;
        tc.PlayForward();



        //TweenScale ts = agroot.gameObject.AddComponent<TweenScale>();
        //ts.from = new Vector3(1, 1, 1);
        //ts.to = new Vector3(1.5f, 1.5f, 1.5f);
        //ts.duration = 0.6f;
        //ts.delay = 0.07f + 0.1f * delayindex;
        //ts.PlayForward();

        EventDelegate.Add(tc.onFinished, delegate ()
        {
            Destroy(tc);
            //   Destroy(ts);
            over = true;
        });
    }

    public void setaniover(bool v)
    {
        if (v)
        {
            sta = 2;
        }
        else
        {
            over = true;
        }
       
    }

    public void iinit4fly(FightObject rfrom, string actionname,int high)
    {
        bigkind = 10;
        rolefrom = rfrom;
        this.actionname = actionname;
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        gameObject.transform.localPosition = rfrom.transform.localPosition;
        if (xgpaintnode == null)
        {
            gameObject.AddComponent<UIPanel>().depth = 40000;
            GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
            xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
            xgpaintnode.create1(agroot, "Effect_Fight_PK", "Effect_Fight_PK");
            xgpaintnode.playAction2(actionname, true);
            xgpaintnode.setdepth(100);
            xgpaintnode.transform.localPosition = new Vector3(0, high, 0);
            xgpaintnode.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            xgpaintnode.playAction2(actionname, true);
        }
        gameObject.name = "shouji";
        over = false;
    }

    public void iinittoworld(GameObject rfrom, string actionname,int offx=0)
    {
        bigkind = 1;
        this.actionname = actionname;
        gameObject.AddComponent<UIPanel>().depth = rfrom.gameObject.GetComponent<UIPanel>().depth + 10;
        gameObject.transform.localPosition = rfrom.transform.localPosition;
        GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
        xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
        xgpaintnode.create1(agroot, "Effect_Fight_PK", "Effect_Fight_PK");
        xgpaintnode.playAction2(actionname, true);
        xgpaintnode.setdepth(100);
      //  xgpaintnode.transform.localPosition = rfrom.transform.localPosition;
        xgpaintnode.transform.localPosition = new Vector3(offx, 0, 0);
        xgpaintnode.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    public void iinittoworldquan(FightObject rfrom, string actionname)
    {
        bigkind = 4;
        rolefrom = rfrom;
        this.actionname = actionname;
        gameObject.AddComponent<UIPanel>().depth = rfrom.gameObject.GetComponent<UIPanel>().depth + 10;
        gameObject.transform.localPosition = rfrom.transform.localPosition;
        GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
        xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
        xgpaintnode.create1(agroot, "Effect_Fight_PK", "Effect_Fight_PK");
        xgpaintnode.playAction2(actionname, false);
        xgpaintnode.setdepth(100);
        xgpaintnode.transform.localPosition =  new Vector3(0, 100, 0);
        // xgpaintnode.transform.localPosition = rfrom.transform.localPosition+new Vector3(0,100,0);
        xgpaintnode.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void iinittoworldcanying(FightObject rfrom)
    {
        bigkind = 3;
        rolefrom = rfrom;
        gameObject.transform.localPosition = rfrom.transform.localPosition;
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
  
        if (xgpaintnode == null)
        {
            agroot.transform.localScale = rfrom.agroot.transform.localScale;
            GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
            xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
         //   xgpaintnode.create1(agroot, rfrom.spaintnode.renpaintnode[2]);
            xgpaintnode.setdepth(100);
            xgpaintnode.transform.localPosition = new Vector3(0, 0, 0);
            xgpaintnode.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            xgpaintnode.setColor(0x01, 0x55, 0xbc);
        }
        else
        {
            xgpaintnode.zhengmian.animation.Update(0);
        }

      //  Debug.LogError(xgpaintnode.zhengmian.transform.localScale.x);
        alpha = 0.8f;
        over = false;

        xgpaintnode.setAlpha((int)(alpha * 255));
    }

    void OnDestroy()
    {
       // paintnode = null;


        xgpaintnode = null;

        labelgen = null;
        spritegen = null;
        texgen = null;
       // paintnodeex = null;
        rolefrom = null;
        roleto = null;
        jt = null;

        renpaintnode = null;
    }

    JumpTo jt;
    public void setdepth(int v)
    {


        

        if (gameObject.GetComponent<UIPanel>() != null)
        {
            gameObject.GetComponent<UIPanel>().depth = v;
        }




        if (xgpaintnode != null)
        {
            xgpaintnode.setdepth(10);
        }


        if (labelgen != null)
        {
            labelgen.GetComponentInChildren<UILabel>().depth = rolefrom.dp + 38;




            if (spritegen != null)
            {
                spritegen.GetComponentInChildren<UISprite>().depth = rolefrom.dp + 38;

            }
        }

        if (texgen != null)
        {
            texgen.GetComponentInChildren<UITexture>().depth = rolefrom.dp + 38;

        }


    }


    public  void setAlpha(int v)
    {


        if (bigkind == 18)
        {


            if (labelgen != null)
            {
                labelgen.GetComponentInChildren<UILabel>().alpha = v / 255f;
            }
        }


    }



    public void setsmallkind(int v)
    {
        smallkind = v;

    }

    public void logic(float dt)
    {


        

        if (bigkind == 0)
        {
            //float alpha = labelgen.GetComponentInChildren<UILabel>().alpha;
            //if (alpha > 0.75f) { alpha -= 0.007f; } else { alpha -= 0.05f; }
            //if (alpha <= 0)
            //{
            //    alpha = 0.01f;
            //}
            //labelgen.GetComponentInChildren<UILabel>().alpha = alpha;
           // setdepth(40020);
        }else if (bigkind == 1||bigkind==10)
        {
            xgpaintnode.logic(dt);
            if (xgpaintnode.isCurrEnd)
            {
                over = true;
              //  Debug.LogError(" " + xgpaintnode.animationname);
            }

            if (bigkind == 10)
            {
                if(rolefrom!=null)
                gameObject.transform.localPosition = rolefrom.transform.localPosition;
            }

        }else if (bigkind == 2)
        {
            xgpaintnode.logic(dt);
            if (xgpaintnode.isCurrEnd)
            {
                over = true;
            }
        }
        else if (bigkind == 3)
        {
            // gameObject.GetComponent<UIPanel>().depth = rolefrom.gameObject.GetComponent<UIPanel>().depth - 10;
          //  gameObject.GetComponent<UIPanel>().depth = 100;
            alpha -= 0.2f;
            xgpaintnode.setAlpha((int)(alpha * 255));
            if (alpha <= 0)
            {
                over = true;
            }
        }else if (bigkind == 4)
        {
            gameObject.transform.localPosition = rolefrom.transform.localPosition;
            //if (!xgpaintnode.isCurrEnd)
            {
                xgpaintnode.logic(dt);
            }
        }else if (bigkind == 5)
        {
            xgpaintnode.logic(dt);
            if (sta == 0)
            {
                alpha += 0.1f;
                xgpaintnode.setAlpha((int)(alpha * 255));

                if (alpha >= 1)
                {
                    sta = 1;
                }
            }
            else
            if (sta == 2)
            {
                alpha -= 0.1f;
                xgpaintnode.setAlpha((int)(alpha * 255));

                if (alpha <= 0)
                {
                    over = true;
                }
            }
        }else if (bigkind == 20)
        {
            duration -= dt;
            if (duration < 0)
            {
                over = true;
            }
        }

        //if (bigkind == 2 || bigkind == 5 || bigkind == 10)
        //{
        //    if (xgpaintnode != null)
        //    {
        //        xgpaintnode.transform.localScale = (Vector3.one * (FightControl.scaleMax - 0.1f * ((rolefrom.transform.localPosition.y - FightControl.bottomY) / (FightControl.topY - FightControl.bottomY))));
        //    }
        //}
        //else
        //{
        //    if (xgpaintnode != null)
        //    {
        //        xgpaintnode.transform.localScale = (Vector3.one * (FightControl.scaleMax - 0.1f * ((transform.localPosition.y - FightControl.bottomY) / (FightControl.topY - FightControl.bottomY))));
        //    }
        //}



        

    }


    private void update(float t)
    {
        float frac = t * jumpnum % 1.0f;
        float y = jumpheight * 4 * frac * (1 - frac);
        y += delta.y * t;

        float x = delta.x * t;
        Vector3 currentPos = gameObject.transform.localPosition;

        Vector3 diff = currentPos - previousPos;
        startPosition = diff + startPosition;

        Vector3 newPos = startPosition + new Vector3(x, y, 0);


        gameObject.transform.localPosition = newPos;

        previousPos = newPos;


    }



    public Vector2 xiuzheng(float x, float y)
    {
        float x45 = x + y;
        float y45 = 0 - (x - y) / 2;

        return new Vector2(x45, y45);
    }

    public static Dictionary<string, Texture2D> textureBuffers = new Dictionary<string, Texture2D>();
    public static void cleartex()
    {
        foreach (string key in textureBuffers.Keys)
        {
            Destroy(textureBuffers[key]);
        }
        textureBuffers.Clear();
    }
    public static Texture2D getTex(string textureName)
    {
        if (textureBuffers.ContainsKey(textureName))
        {
            return textureBuffers[textureName];
        }
        else
        {
            Texture2D img = ResManager.getTex("texres/" + textureName);
            textureBuffers.Add(textureName, img);
            return img;
        }
    }


}
