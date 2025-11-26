using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkeletonAnimation))]
public class ZDSpine : MonoBehaviour
{
    public SkeletonAnimation animation;
    public Spine.AnimationState zhengstate;
    public SActionGroup sag;

    public bool creatover = false;
    public string attachname = "", modelname = "";
    public void create1(GameObject die, string attachname, string modelname)
    {
        gameObject.transform.SetParent(die.gameObject.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);


        long btime = CTTools.currentTimeMillis();

        if (creatover)
        {
            change(attachname, modelname);
            return;
        }
        this.attachname = attachname;
        this.modelname = modelname;

        sag = SActionGroup.getAGFromBuffer(attachname, modelname);
        //  sag= new SActionGroup(attachname, modelname);


        Material mattemp = new Material(Shader.Find("Spine/Skeleton"));
        mattemp.mainTexture = sag.loaderbean.img;
        mattemp.name = "test";
        {
            animation = gameObject.GetComponent<SkeletonAnimation>();

            animation.skeletonDataAsset = sag.getSkeletonDataAssetInstance();
            animation.mymaterial = mattemp;

            // zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAsset();
            animation.Initialize(false);
            animation.logicflag = false;
            animation.loop = true;
            animation.state.Complete += Complete;
            animation.state.Event += HandleEvent;
            zhengstate = animation.state;


        }
        creatover = true;

        //   StartCoroutine(refreshpanel(gameObject));
        //  gameObject.GetComponentInParent<UIPanel>().Refresh();

    }

    public void create1nopatch(GameObject die, string attachname, string modelname)
    {
        create1(die, attachname, modelname);
    }

    public UIWidget alphawihget;
    public UIPanel alphapanel;
    public bool autoalpha = false;
    public void setAutoAlpha(bool v)
    {
        autoalpha = v;
        if (autoalpha)
        {
            alphawihget = gameObject.GetComponentInParent<UIWidget>();
            alphapanel = gameObject.GetComponentInParent<UIPanel>();
        }

    }



    internal void addEvent(Spine.AnimationState.TrackEntryEventDelegate handleEvent)
    {
        animation.state.Event += handleEvent;
    }

    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (gameObject.activeSelf)
        {
            //   Debug.LogError(e.ToString());
        }
    }
    void Complete(Spine.TrackEntry trackEntry)
    {
        if (gameObject.activeSelf)
        {
            isCurrEnd = true;
            if (endaciton != null)
            {
                endactionbackup = endaciton;
                endaciton = null;
                endactionbackup();

            }
        }

    }
   



    public void clear()
    {
        SActionGroup.delAGFromBuffer(attachname, modelname);
    }
    public string gettempstring()
    {
        Debug.Log("dll");
        return "dll";
    }
    public void setFrameSpr(int currActionId, int currentFrameID)
    {


    }





    public int[] skinnameid = new int[4];
    public string[] skinname = new string[4];
    public Skin[,] skinarray = new Skin[2, 4];
    public static string[] skinqianzhui = { "Skin_", "Skin_", "Clo_", "Hat_", "Face_" };
    public void setSkin(string skinname0, int k)
    {
        this.skinname[k] = skinname0;


        {
            Skin tempSkin = animation.skeleton.Data.FindSkin(skinname0);
            Skin newSkin = new Skin(skinname0 + "_zheng");
            ExposedList<Slot> slots = animation.skeleton.Slots;
            for (int i = 0, n = slots.Count; i < n; i++)
            {
                Slot slot = slots.Items[i];
                string name = slot.Data.Name;
                if (slot.Data.AttachmentName != null)
                {
                    Attachment attachment = null;
                    try
                    {
                        attachment = tempSkin.GetAttachment(i, slot.Data.AttachmentName);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(skinname0 + "    " + k);
                    }

                    if (attachment != null)
                    {
                        //RegionAttachment newatt = ((RegionAttachment)attachment).clone();
                        //newatt.UpdateOffset();
                        //newSkin.AddAttachment(i, slot.data.AttachmentName, newatt);
                        newSkin.SetAttachment(i, slot.Data.AttachmentName, attachment);
                    }
                }
            }
            animation.skeleton.SetSkin(newSkin, true);

        }



    }

    internal void clone()
    {
        sag = SActionGroup.getAGFromBuffer(attachname, modelname);
        animation.skeletonDataAsset = sag.getSkeletonDataAsset();
        animation.Initialize(true);
        zhengstate = animation.state;

        playAction2(animationname, isloop);

    }
    public void change(string attachname, string modelname)
    {
        if (this.attachname == attachname && this.modelname == modelname)
        {
            return;
        }

        long btime = CTTools.currentTimeMillis();

        this.attachname = attachname;
        this.modelname = modelname;
        sag = SActionGroup.getAGFromBuffer(attachname, modelname);
        Debug.Log("000&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&" + (CTTools.currentTimeMillis() - btime));
        btime = CTTools.currentTimeMillis();
        animation.skeletonDataAsset = sag.getSkeletonDataAssetInstance();
        Debug.Log("111&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&" + (CTTools.currentTimeMillis() - btime));
        btime = CTTools.currentTimeMillis();
        animation.Initialize(true);
        zhengstate = animation.state;
        Debug.Log("222&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&" + (CTTools.currentTimeMillis() - btime));
        btime = CTTools.currentTimeMillis();
        animation.state.Complete += Complete;
    }

    public void setSkin(int skinid, int k)
    {
        string tempname = null;

        tempname = "Skin_" + string.Format("{0:D3}", skinid);

        skinnameid[k] = skinid;

       animation.skeleton.SetSkin(tempname);



        // setSkin(tempname, k);
    }
    public void addSkin(int skinid, int k)
    {
        if (skinid < 0)
        {
            skinname[k - 1] = "";
            return;
        }

        string tempname = skinqianzhui[k] + string.Format("{0:D3}", skinid);
        addSkin(tempname, k);
    }
    public void addSkin(string skinname0, int k)
    {
        if (skinname0 == null || skinname0.Equals(""))
        {
            return;
        }

        skinname[k - 1] = skinname0;
        animation.skeleton.SetSkin(skinname0, false);
    }





    Shader shader;
    internal void setShader(Shader shader)
    {
        Material mattemp = new Material(shader);
        mattemp.mainTexture = sag.loaderbean.img;
        //  zhengmian.animation.mymaterial = mattemp;
        this.shader = shader;
    }
    public bool isCurrEnd = false;
    public bool isloop = false;
    public string animationname;
    public int nowAction = -1;
    public void playAction(int id, bool loop)
    {

        nowAction = id;
        animation.loop = loop;
        animation.AnimationIndex = id;
        gameObject.SetActive(true);
        isCurrEnd = false;
    }
    public void playActionForce(int id, bool loop)
    {

        isloop = loop;
        animation.loop = loop;
        animation.setAnimationIndexForce(id);
        gameObject.SetActive(true);
        setdepth(0);
        isCurrEnd = false;
        logic();
    }
    public void playActionForce2(int id, bool loop)
    {


        isloop = loop;
        gameObject.SetActive(true);
        animation.loop = loop;
        animation.setAnimationIndexForce(id);
        isCurrEnd = false;
        animation.apply(0);
        animation.repaint();
        setdepth(1000);
        logic();
    }
    public void playAction2(string idname, bool loop)
    {

        if (idname == null)
        {
            return;
        }
        animationname = idname;
        isloop = loop;
        int id = animation.state.Data.SkeletonData.FindAnimationId(idname);
        if (id == -1)
        {
            Debug.LogError(idname + " not found");
        }
        nowAction = id;
        animation.loop = loop;
        animation.AnimationIndex = id;
        // UTools.setActive(zhengmian.gameObject, true);
        gameObject.SetActive(true);
        isCurrEnd = false;
        animation.apply(0);
        animation.repaint();
    }
    bool autoflag = false;
    float logicdelay;
    Action endaciton, endactionbackup, midaction;
    public void playAction2Auto(string idname, bool loop, Action endaciton = null, float delay = 0, Action midaction = null)
    {

        if (idname == null)
        {
            return;
        }
        gameObject.SetActive(true);
        logicdelay = delay;

        autoflag = true;
        this.endaciton = endaciton;
        this.midaction = midaction;
        animationname = idname;
        isloop = loop;
        int id = animation.state.Data.SkeletonData.FindAnimationId(idname);
        if (id == -1)
        {
            Debug.LogError(idname + " not found");
        }
        nowAction = id;
        animation.loop = loop;
        animation.setAnimationIndexForce(id);
        // UTools.setActive(zhengmian.gameObject, true);
        if (delay != 0)
            gameObject.SetActive(false);
        isCurrEnd = false;
        animation.apply(0);
        animation.repaint();
    }
    public void playAction2Force(string idname, bool loop)
    {

        if (idname == null)
        {
            return;
        }
        animationname = idname;
        isloop = loop;
        int id = animation.state.Data.SkeletonData.FindAnimationId(idname);
        if (id == -1)
        {
            Debug.LogError(idname + " not found");
        }
        nowAction = id;
        animation.loop = loop;
        animation.setAnimationIndexForce(id);
        // UTools.setActive(zhengmian.gameObject, true);
        gameObject.SetActive(true);
        isCurrEnd = false;
        animation.apply(0);
        animation.repaint();
    }

    public void logic()
    {


        zhengstate.Update(Time.deltaTime);
        animation.Update(Time.deltaTime);

        if (autoalpha)
        {
            float alphatemp = 1;

            if (alphapanel != null)
            {
                alphatemp *= alphapanel.alpha;
            }

            if (alphawihget != null)
            {
                alphatemp *= alphawihget.alpha;
            }
            setAlpha((int)(alphatemp * 255));
        }

    }
    public APaintNodeSpine linkspaintnode;
    public void logic(float delta)
    {


        if (linkspaintnode == null)
        {
            zhengstate.Update(delta);
        }



        animation.Update(delta);


    

        if (autoalpha)
        {
            float alphatemp = 1;

            if (alphapanel != null)
            {
                alphatemp *= alphapanel.alpha;
            }

            if (alphawihget != null)
            {
                alphatemp *= alphawihget.alpha;
            }
            setAlpha((int)(alphatemp * 255));
        }



    }

    // Use this for initialization
    void Start()
    {
        //gameObject.GetComponentInParent<UIPanel>().Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < modlist.Count; i++)
        //{
        //    GameObject gumod = (GameObject)modlist[i];
        //    UITexture tex = (UITexture)texlist[i];
        //    if (gumod.activeSelf)
        //    {
        //        tex.depth = dpbackup + i;
        //    }
        //}

        if (autoflag)
        {
            logicdelay -= Time.deltaTime;
            if (logicdelay < 0)
            {
                if (midaction != null)
                {
                    midaction();
                    midaction = null;
                }
                UTools.setActive(gameObject, true);
                logic(0.03f);
            }

        }
    }

    void OnDestroy()
    {
        //  Debug.Log("OnDestory");


        zhengstate = null;
        sag = null;
    }
    public int dp;
    internal void setdepth(int v)
    {

        dp = v;
        // gameObject.GetComponent<UIWidget>().depth = v;

        //if (gameObject.GetComponent<UIWidget>() != null)
        //{
        //    Destroy(gameObject.GetComponent<UIWidget>());
        //}
        //GetComponent<NGUISpine>().depth = v;
    }
    public int alphavalue = 255;
    int colorr = 255;
    int colorg = 255;
    int colorb = 255;
    public void setAlpha(int v)
    {

        if (alphavalue == v)
        {
            return;
        }

        alphavalue = v;
        animation.skeleton.SetColor(new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
    }

    public void setColor(int r, int g, int b)
    {

        colorr = r;
        colorg = g;
        colorb = b;
        animation.skeleton.SetColor(new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
    }
    public void setColor(Color col)
    {

        colorr = (int)col.r * 255;
        colorg = (int)col.g * 255;
        colorb = (int)col.b * 255;
        animation.skeleton.SetColor(col);
    }
    public void setColor(int r, int g, int b, int alpha)
    {

        colorr = r;
        colorg = g;
        colorb = b;
        alphavalue = alpha;
       animation.skeleton.SetColor(new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
    }
   
    internal void reset()
    {

    }

    public void refresh()
    {

    }
    internal void addmodscheme(int zid, int id)
    {
        if (id == -1)
        {
            return;
        }


        String skinname0 = "";
        // if(zid== Statics.SCHEME_CLOTHES)
        {
            skinname0 = "Clo_";
        }
        //else if(zid== Statics.SCHEME_HEADWEAR)
        //{
        //    skinname0 = "Hat_";
        //}
        //else if (zid == Statics.SCHEME_FACEWEAR)
        //{
        //    skinname0 = "Face_";
        //}
        skinname0 = skinname0 + string.Format("{0:D2}", id);
        int k = zid;


        animation.skeleton.SetSkin(skinname0, false);
    }
    private bool slotisclothtype(string name, int k)
    {
        if (k == 2)
        {
            return name != null
                && (name.Equals("Body")
                || name.Equals("L_arm")
                || name.Equals("L_bracer")
                || name.Equals("L_shin")
                || name.Equals("L_thigh")
                || name.Equals("R_arm")
                || name.Equals("R_bracer")
                || name.Equals("R_shin")
                || name.Equals("R_thigh")
                || name.Equals("Skirt")
                || name.Equals("Tail")
                );
        }
        else if (k == 3)
        {
            return name != null
                && (name.Equals("Hat")

                );
        }
        else if (k == 4)
        {
            return name != null
                && (name.Equals("Glass")
                );
        }

        return false;
    }
    internal void setmodscheme(int zid, int id)
    {
        string skinname = "Skin_" + string.Format("{0:D2}", id);

        // Debug.Log(string.Format("{0:D2}", 190));
        Skin tempSkin = animation.skeleton.Data.FindSkin(skinname);
        Skin newSkin = new Skin(skinname + "_temp");
        ExposedList<Slot> slots = animation.skeleton.Slots;
        for (int i = 0, n = slots.Count; i < n; i++)
        {
            Slot slot = slots.Items[i];
            string name = slot.Data.AttachmentName;
            if (name != null)
            {
                Attachment attachment = tempSkin.GetAttachment(i, name);
                if (attachment != null)
                {
                    newSkin.SetAttachment(i, slot.Data.AttachmentName, attachment);
                }
            }

        }

        animation.skeleton.SetSkin(newSkin, true);
    }



}
