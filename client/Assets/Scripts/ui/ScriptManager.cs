using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

public class ScriptManager : MonoBehaviour
{
    public static bool scriptrunflag = false;

    public UITexture bgtex,mengban;
    public UILabel namelabel,dhlabel,label;

    public GameObject dhbut;

    LuaFunction func = null;
    LuaEnv lua;
    bool thread = false;


    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(dhbut).onClick = onclickdh;

        if (dhlabel.GetComponent<TypewriterEffect>() == null)
        {
            dhlabel.gameObject.AddComponent<TypewriterEffect>();
        }
        dhlabel.GetComponent<TypewriterEffect>().charsPerSecond = 20;
        EventDelegate.Add(dhlabel.GetComponent<TypewriterEffect>().onFinished, delegate ()
        {
            daziwanflag = true;

            if (autorunflag)
            {
                autoSleep(1000);
            }

        });

    }

    private void onclickdh(GameObject go)
    {
        Debug.Log("onclickdh");
        setautoflag(false);

        if (daziwanflag)
        {
            dhbut.gameObject.SetActive(false);
            Debug.Log("continuescript0");
            continuescript();
        }
        else
        {
            dhlabel.GetComponent<TypewriterEffect>().Finish();
        }
    }

    public string nowluaname;
    public bool autorunflag = false;
    public List<string> duihualist = new List<string>();
    public void loadluajq(string luafilename)
    {

        if (true)
        {
            //   return;
        }
        duihualist.Clear();
        setautoflag(false);


        if (luafilename.Equals("-1"))
        {
            return;
        }

        gameObject.SetActive(true);


        SoundPlayer.getInstance().Stop();




        if (lua == null)
        {
            lua = new LuaEnv();
            string hello = ResManager.getLuaText("mylua/" + luafilename);
            string temp = addyiald(hello);
            Debug.LogError(temp);
            lua.DoString(temp);
            LuaFunction func = lua.Global.Get<LuaFunction>("start");
            object obj = func.Call(this);
            thread = true;
        }

        continuescript();



    }
    private string addyiald(string hello)
    {
        string temp = "";
        string[] spstr1 = hello.Split('\n');


        string[] spstr = new string[spstr1.Length + 1];
        int lastdhindex = -1;
        for (int i = spstr1.Length - 1; i >= 0; i--)
        {
            if (spstr1[i].Trim().StartsWith("mission:showDialog"))
            {
                lastdhindex = i;
                break;
            }
        }

        for (int i = 0; i < lastdhindex; i++)
        {
            spstr[i] = spstr1[i];
        }

        spstr[lastdhindex] = "mission:stopauto()";

        for (int i = lastdhindex; i < spstr1.Length; i++)
        {
            spstr[i + 1] = spstr1[i];
        }

        for (int i = 0; i < spstr.Length; i++)
        {
            temp = temp + "\n" + spstr[i];


            string linestr = spstr[i].Trim();
            if (linestr.StartsWith("mission:"))
            {

                if (
                    linestr.StartsWith("mission:pause")
                    // || linestr.StartsWith("mission:setState")
                    || linestr.StartsWith("mission:resume")
                    || linestr.StartsWith("mission:hideDialog")
                    || linestr.StartsWith("mission:showCheckin")
                    //   || linestr.StartsWith("mission:centerShop")
                    || linestr.StartsWith("mission:setstep")
                    || linestr.StartsWith("mission:addEffect")
                    || linestr.StartsWith("mission:removeEffect")
                    || linestr.StartsWith("mission:removeBG")
                    || linestr.StartsWith("mission:removeCG")
                    || linestr.StartsWith("mission:setJJPosAndDirect")
                    || linestr.StartsWith("mission:setTeachMission")
                    || linestr.StartsWith("mission:setTeachMoney")
                    || linestr.StartsWith("mission:showTip")
                    || linestr.StartsWith("mission:jumpToUI")
                    || linestr.StartsWith("mission:returnToUI")
                    || linestr.StartsWith("mission:endTeach")
                    || linestr.StartsWith("mission:stopauto")
                    //   || linestr.StartsWith("mission:endSGMH")
                    || linestr.StartsWith("mission:endScript")
                    )
                {

                }
                else
                {

                    temp = temp + "\n" + "coroutine.yield()";

                }

            }
        }

        temp = temp + "\n" + @"
	function continue(mission)
	print('xlua--resume')
	coroutine.resume(co2,mission)
	end
";

        return temp;
    }

    private void continuescript()
    {
        Debug.Log("continuescript");



        if (thread)
        {
            LuaFunction func = lua.Global.Get<LuaFunction>("continue");
            func.Call(this);
        }
    }
    private void setautoflag(bool v)
    {
        autorunflag = v;
        //autobut.transform.Find("Sprite2").gameObject.SetActive(v);
        //zidongicon.SetActive(v);
        //shoudongicon.SetActive(!v);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void executeScriptLocal(string script)
    {
        scriptrunflag = true;
       // tmbg.SetActive(true);
        label.text = "executeScriptLocal";
        nowluaname = script;
        sleep(10);
    }
    public void autoSleep(int time)
    {
        Debug.Log(" autoSleep" + time);

        Invoke("continuescript", time / 1000f);
    }
    public void sleep(int time)
    {
        Debug.Log("sleep" + time);
        Invoke("continuescript", time / 1000f);
    }
    int changbgkind;
    Material mymat;
    float jiaodu = 0;
    float jiaodutemp = 0;

    public void changeBG(string bgname, int kind)
    {
        Debug.Log("changeBG");
        sleep(10);
        /*
        changbgkind = kind;
        Texture2D tex = new Texture2D(6, 6, TextureFormat.ARGB32, false);
        tex.name = "bgmengban";
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                if (kind == 0 || kind == 3)
                {
                    tex.SetPixel(i, j, Color.black);
                }
                else if (kind == 1)
                {
                    tex.SetPixel(i, j, Color.white);
                }
                else if (kind == 2)
                {
                    tex.SetPixel(i, j, new Color(1, 1, 1, 1f));
                }

            }
        }
        tex.Apply();


        mengban.mainTexture = tex;


        if (kind == 0 || kind == 1)
        {
            mengban.color = new Color32(255, 255, 255, 0);
            mengban.gameObject.SetActive(true);
            TweenColor tc = mengban.gameObject.AddComponent<TweenColor>();
            // tc.enabled = true;
            tc.from = new Color32(255, 255, 255, 0);
            tc.to = new Color32(255, 255, 255, 255);
            tc.duration = 0.8f;
            tc.delay = 0.1f;
            tc.PlayForward();

            EventDelegate.Add(tc.onFinished, delegate ()
            {
                Debug.Log(mengban.color.a);
                if (mengban.color.a * 255 > 200)
                {
                    tc.enabled = true;
                    tc.PlayReverse();

                    if (bgtex.mainTexture != null)
                    {
                        Destroy(bgtex.mainTexture);
                    }
                    setbg(bgtex, bgname);
                    bgtex.gameObject.SetActive(true);


                }
                else
                {
                    Destroy(tc);
                    Debug.Log("continuescript6");
                    continuescript();
                }
            });
        }
        else if (kind == 2)
        {
            mymat = new Material(Resources.Load("Shaders/xinniuqu") as Shader);
            mymat.SetVector("_MainTex_ST", new Vector4(1600, 900));
            mymat.SetVector("_MainTex_TexelSize", new Vector4(1600, 900));
            mymat.SetVector("_CenterRadius", new Vector4(0.5f, 0.5f, 0.8f, 0.8f));
            mymat.SetFloat("_Angle", jiaodu);
            bgtex.GetComponent<UITexture>().material = mymat;
            bgtex.gameObject.SetActive(false);
            bgtex.gameObject.SetActive(true);

            mengban.gameObject.SetActive(true);
            TweenAlpha tc = mengban.gameObject.AddComponent<TweenAlpha>();

            tc.from = 0;
            tc.to = 1;
            tc.duration = 1.5f;
            tc.delay = 0.1f;
            tc.PlayForward();

            EventDelegate.Add(tc.onFinished, delegate ()
            {
                Debug.Log(mengban.alpha);
                if (mengban.alpha * 255 > 200)
                {
                    tc.enabled = true;
                    tc.PlayReverse();

                    if (bgtex.mainTexture != null)
                    {
                        Destroy(bgtex.mainTexture);
                    }
                    setbg(bgtex, bgname);
                    bgtex.gameObject.SetActive(true);
                }
                else
                {
                    changbgkind = -1;
                    //jiaodu =0;
                    //mymat.SetFloat("_Angle", jiaodu);
                    //bgtex.RemoveFromPanel();
                    //bgtex.MarkAsChanged();


                    Destroy(tc);

                }
            });
        }
        else if (kind == 3)
        {
            if (bgtex.mainTexture != null)
            {
                Destroy(bgtex.mainTexture);
            }
            setbg(bgtex, bgname);


            bgtex.gameObject.SetActive(true);
            mengban.color = new Color32(255, 255, 255, 255);
            mengban.gameObject.SetActive(true);
            TweenColor tc = mengban.gameObject.AddComponent<TweenColor>();
            // tc.enabled = true;
            tc.from = new Color32(255, 255, 255, 255);
            tc.to = new Color32(255, 255, 255, 0);
            tc.duration = 1.5f;
            tc.delay = 0.1f;
            tc.PlayForward();

            EventDelegate.Add(tc.onFinished, delegate ()
            {
                Debug.Log(mengban.color.a);
                Destroy(tc);
                Debug.Log("continuescript6");
                continuescript();
            });
        }
        */

    }
    private void setbg(UITexture bgtex, string bgname)
    {
        if (ResManager.halftexflag)
        {
            bgtex.mainTexture = ResManager.getTexJPG("luares/" + bgname + "_s.jpg");
            bgtex.MakePixelPerfect();
          //  ResManager.MakePixelPerfectHalf(bgtex);
        }
        else
        {
            bgtex.mainTexture = ResManager.getTexJPG("luares/" + bgname + ".jpg");
            bgtex.MakePixelPerfect();
        }

    }
    bool daziwanflag = false;
    public void showDialog(string name, int renid, int emoindex, int agindex, int soundindex, string content)
    {
        Debug.Log(name + " " + agindex + " " + emoindex + " " + content);
        namelabel.text = name;
        daziwanflag = false;

        string temp = replacecolor(content);

        dhlabel.text = temp;


        dhlabel.GetComponent<TypewriterEffect>().ResetToBeginning();

        duihualist.Add(namelabel.text + '&' + content.Replace("${name}", temp));

        dhbut.gameObject.SetActive(true);

        if (name.Equals("玩家"))
        {
            //  labeldi.GetComponent<UISprite>().spriteName = "gut_girl";
           // UIUntils.setLabelEffect(namelabel.GetComponentInChildren<UILabel>(), 38, 42);
        }
        else
        {
            //  labeldi.GetComponent<UISprite>().spriteName = "gut_manrect";
           // UIUntils.setLabelEffect(namelabel.GetComponentInChildren<UILabel>(), 39, 42);
        }

      

        if (renid != -1)
        {

            {
                if (agindex != -1)
                {
                   
                }

                if (emoindex != -1)
                {
                    
                }
            }


            if (soundindex != -1)
            {


            }
        }

        
    }

    private string replacecolor(string temp)
    {
        string tempstr=Regex.Replace(temp, "\\[[0-9a-fA-F]{6}\\]", "", RegexOptions.IgnoreCase);
        tempstr = Regex.Replace(tempstr, "\\[[0-9a-fA-F]{8}\\]", "", RegexOptions.IgnoreCase);
        tempstr = Regex.Replace(tempstr, "\\[[0-9a-fA-F]{2}\\]", "", RegexOptions.IgnoreCase);
        return tempstr;
    }
    public void addNPC(int renid, int replacerenid)
    {

        Debug.Log("addNPC");

        sleep(100);
    }
    public void removeNPC(int renid)
    {
        Debug.Log("removeNPC");
        sleep(10);

    }
    public void playSound(string soundfilename)
    {
        Debug.Log("playSound");

        // SoundPlayer.getInstance().PlayW("scriptuse/" + soundfilename, false, false);

        sleep(10);

    }
    public void hideDialog()
    {
        label.text = "hideDialog";
        Debug.Log("hideDialog");
    }
    public void stopauto()
    {
        setautoflag(false);
    }
    public void endScript(String script)
    {

        hideDialog();

        scriptrunflag = false;






        if (thread)
        {
            thread = false;
        }


        gameObject.SetActive(false);
        Debug.Log("-------------------endscriptend");

        //  Resources.UnloadUnusedAssets();
    }


}
