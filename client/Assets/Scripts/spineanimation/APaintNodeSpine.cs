
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using Spine.Unity;
using Spine;

public class APaintNodeSpine : MonoBehaviour
{

    public NGUISpine zhengmian;
    public Spine.AnimationState zhengstate;
    public SActionGroup sag;

    public static bool twocolormat = true;

    public bool creatover = false;
    public string attachname="", modelname="";
    public void create1(GameObject die, string attachname, string modelname, float scale = 1.0f)
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

        //  Material mattemp = new Material(Shader.Find("Spine/Skeleton"));
        Material mattemp = null;
        if (twocolormat)
        {
            mattemp = new Material(Shader.Find("Spine/Skeleton Tint"));
        }
        else
        {
            mattemp = new Material(Shader.Find("Spine/Skeleton"));
        }

        mattemp.mainTexture = sag.loaderbean.img;
        mattemp.name = "test";
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "splayer");
            zhengmian = texframeobjs.GetComponent<NGUISpine>();
          //  zhengmian.gameObject.SetActive(false);
            zhengmian.gameObject.transform.SetParent(gameObject.transform);
            zhengmian.gameObject.transform.localScale = Vector3.one * scale;
            zhengmian.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            if (false)
            {
                zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAsset();
            }
            else
            {//有bug 如何在排序前把材质设置进去 这样才能提高加载速度
                zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAssetInstance();
                zhengmian.animation.mymaterial = mattemp;
            }

           // zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAsset();
            zhengmian.animation.Initialize(false);
            zhengmian.animation.logicflag = false;
            zhengmian.animation.loop = true;
            zhengmian.animation.state.Complete += Complete;
            zhengmian.animation.state.Event += HandleEvent;
            zhengstate = zhengmian.animation.state;


        }
        creatover = true;

     //   StartCoroutine(refreshpanel(gameObject));
        //  gameObject.GetComponentInParent<UIPanel>().Refresh();

  

    }

    public void create1nopatch(GameObject die, string attachname, string modelname)
    {
        create1(die, attachname, modelname);
        zhengmian.patchdraw = false;
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
            alphapanel= gameObject.GetComponentInParent<UIPanel>();
        }
        else
        {
            setAlpha(255);
        }

    }

    private IEnumerator refreshpanel(GameObject gameObject)
    {
         //yield return zhengmian.gameObject.GetComponent<MeshRenderer>().sharedMaterials.Length>0&&zhengmian.gameObject.GetComponent<MeshRenderer>().sharedMaterials[0].name == "test";
        yield return null;
       // gameObject.GetComponentInParent<UIPanel>().Refresh();
        yield return null;
        gameObject.GetComponentInParent<UIPanel>().Refresh();
    }
    public void createft(GameObject die, string attachname, string modelname)
    {
        this.attachname = attachname;
        this.modelname = modelname;

        sag = SActionGroup.getAGFromBuffer(attachname, modelname);
        //  sag= new SActionGroup(attachname, modelname);
        gameObject.transform.SetParent(die.gameObject.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        Material mattemp = new Material(Shader.Find("Spine/Skeleton"));
        mattemp.mainTexture = sag.loaderbean.img;
        mattemp.name = "test";
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "splayer");
            zhengmian = texframeobjs.GetComponent<NGUISpine>();
            zhengmian.gameObject.SetActive(false);
            zhengmian.gameObject.transform.SetParent(gameObject.transform);
            zhengmian.gameObject.transform.localScale = new Vector3(1, 1, 1);
           // if (modelname.Equals("Dessert"))
            {
               // zhengmian.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            zhengmian.gameObject.transform.localPosition = new Vector3(0, 0, 0);
           // zhengmian.animation.mymaterial = mattemp;
            zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAsset();

            zhengmian.animation.Initialize(false);
            zhengmian.animation.logicflag = false;
            zhengmian.animation.loop = true;
            zhengmian.animation.state.Complete += Complete;
            zhengmian.animation.state.Event += HandleEvent;
            zhengstate = zhengmian.animation.state;


        }


    }

    internal void addEvent(Spine.AnimationState.TrackEntryEventDelegate handleEvent)
    {
        zhengmian.animation.state.Event += handleEvent;
    }

    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (zhengmian.gameObject.activeSelf)
        {
         //   Debug.LogError(e.ToString());
        }
    }
    void Complete(Spine.TrackEntry trackEntry)
    {
        if (zhengmian.gameObject.activeSelf)
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
    public void create1(GameObject die, int zuid, int sid, int aid, int wzindex)
    {
        sag = SActionGroup.getAGFromBuffer("Dessert", "model_A");
        gameObject.transform.SetParent(die.gameObject.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        Material mattemp = new Material(Shader.Find("Spine/Skeleton"));
        mattemp.mainTexture = sag.loaderbean.img;
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "splayer");
            zhengmian = texframeobjs.GetComponent<NGUISpine>();
            zhengmian.gameObject.SetActive(false);
            zhengmian.gameObject.transform.SetParent(gameObject.transform);
            zhengmian.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            zhengmian.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAsset();
            zhengmian.animation.Initialize(false);
            zhengmian.animation.logicflag = false;
            zhengmian.animation.loop = true;
            zhengmian.animation.state.Complete += Complete;
            zhengstate = zhengmian.animation.state;
            //zhengmian.animation.mymaterial = mattemp;

        }
    }
    public void create2(GameObject die, int zuid, int sid, int aid, int wzindex)
    {
        sag = SActionGroup.getAGFromBuffer("Dessert", "Dessert");
        gameObject.transform.SetParent(die.gameObject.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        Material mattemp = new Material(Shader.Find("Spine/Skeleton"));
        mattemp.mainTexture = sag.loaderbean.img;
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "splayer");
            zhengmian = texframeobjs.GetComponent<NGUISpine>();
            zhengmian.gameObject.SetActive(false);
            zhengmian.gameObject.transform.SetParent(gameObject.transform);
            zhengmian.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            zhengmian.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAsset();
            zhengmian.animation.Initialize(false);
            zhengmian.animation.logicflag = false;
            zhengmian.animation.loop = true;
            zhengmian.animation.state.Complete += Complete;
            zhengstate = zhengmian.animation.state;
           // zhengmian.animation.mymaterial = mattemp;

        }
    }
    public void create5(GameObject die, int zuid, int sid, int aid, int wzindex)
    {
        sag = SActionGroup.getAGFromBuffer("Dessert", "model_A");
        gameObject.transform.SetParent(die.gameObject.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

        Material mattemp = new Material(Shader.Find("Spine/Skeleton"));
        mattemp.mainTexture = sag.loaderbean.img;
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "splayer");
            zhengmian = texframeobjs.GetComponent<NGUISpine>();
            zhengmian.gameObject.SetActive(false);
            zhengmian.gameObject.transform.SetParent(gameObject.transform);
            zhengmian.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            zhengmian.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAsset();
            zhengmian.animation.Initialize(false);
            zhengmian.animation.logicflag = false;
            zhengmian.animation.loop = true;
            zhengmian.animation.state.Complete += Complete;
            zhengstate = zhengmian.animation.state;
            //zhengmian.animation.mymaterial = mattemp;

        }
    }

    public void create1(GameObject die, APaintNodeSpine spaintnode)
    {
        gameObject.transform.SetParent(die.gameObject.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);



        sag = SActionGroup.getAGFromBuffer(spaintnode.attachname, spaintnode.modelname);

        Material mattemp = new Material(Shader.Find("Spine/Skeleton"));
        mattemp.mainTexture = sag.loaderbean.img;
        mattemp.name = "test";
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "splayer");
            zhengmian = texframeobjs.GetComponent<NGUISpine>();

            zhengmian.gameObject.transform.SetParent(gameObject.transform);
           // zhengmian.gameObject.transform.localScale = spaintnode.gameObject.transform.localScale;
            zhengmian.gameObject.transform.localScale = Vector3.one;
            zhengmian.gameObject.transform.localPosition = new Vector3(0, 0, 0);

            zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAssetInstance();
            zhengmian.animation.mymaterial = mattemp;

            zhengmian.animation.Initialize(false);
            zhengmian.animation.logicflag = false;
            zhengmian.animation.loop = true;
            zhengmian.animation.state.Complete += Complete;
            zhengmian.animation.state.Event += HandleEvent;


            zhengmian.animation.state = spaintnode.zhengstate;
            zhengmian.animation.fubenflag = true;
            zhengmian.animation.Update(0);



            zhengmian.gameObject.SetActive(true);


            zhengstate = zhengmian.animation.state;



        }
        creatover = true;
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
            Skin tempSkin = zhengmian.animation.skeleton.Data.FindSkin(skinname0);
            Skin newSkin = new Skin(skinname0 + "_zheng");
            ExposedList<Slot> slots = zhengmian.animation.skeleton.Slots;
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
            zhengmian.animation.skeleton.SetSkin(newSkin, true);

        }



    }

    internal void clone()
    {
        sag = SActionGroup.getAGFromBuffer(attachname, modelname);
        zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAsset();
        zhengmian.animation.Initialize(true);
        zhengstate = zhengmian.animation.state;

        playAction2(animationname, isloop);

    }
    public void change(string attachname, string modelname)
    {
        if(this.attachname == attachname&& this.modelname == modelname)
        {
            return;
        }

        long btime = CTTools.currentTimeMillis();

        this.attachname = attachname;
        this.modelname = modelname;
        sag = SActionGroup.getAGFromBuffer(attachname, modelname);
        Debug.Log("000&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&" + (CTTools.currentTimeMillis() - btime));
        btime = CTTools.currentTimeMillis();
        Material mattemp = new Material(Shader.Find("Spine/Skeleton"));
        mattemp.mainTexture = sag.loaderbean.img;
        mattemp.name = "test";
        zhengmian.animation.mymaterial = mattemp;
        zhengmian.animation.skeletonDataAsset = sag.getSkeletonDataAssetInstance();
        Debug.Log("111&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&" + (CTTools.currentTimeMillis() - btime));
        btime = CTTools.currentTimeMillis();
        zhengmian.animation.Initialize(true);
        zhengstate = zhengmian.animation.state;
        Debug.Log("222&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&" + (CTTools.currentTimeMillis() - btime));
        btime = CTTools.currentTimeMillis();
        zhengmian.animation.state.Complete += Complete;

        zhengmian.animation.setAnimationIndexForce(0);


        //NGUITools.MarkParentAsChanged(transform.gameObject);
        //NGUITools.MarkParentAsChanged(transform.parent.gameObject);
        //zhengmian.onSortingChange(0);
        zhengmian.GetComponent<NGUISpine>().depth = 0;
    }

    public void setSkin(int skinid, int k)
    {
        string tempname = null;

        tempname = "Skin_" + string.Format("{0:D3}", skinid);

        skinnameid[k] = skinid;

        zhengmian.animation.skeleton.SetSkin(tempname);

        if (lvpaintnode != null)
        {
            lvpaintnode.setSkin(skinid, k);
        }

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
        zhengmian.animation.skeleton.SetSkin(skinname0, false);
    }
    




    Shader shader;
    internal void setShader(Shader shader)
    {
        Material mattemp = new Material(shader);
        mattemp.mainTexture = sag.loaderbean.img;
      //  zhengmian.animation.mymaterial = mattemp;
        this.shader = shader;
    }
    public int getActionNum()
    {
        return zhengstate.Data.skeletonData.Animations.Count;
    }
    public bool isCurrEnd = false;
    public bool isloop = false;
    public string animationname;
    public int nowAction = -1;
    public void playAction(int id, bool loop)
    {
        if (zhengmian == null)
        {
            return;
        }
        nowAction = id;
        zhengmian.animation.loop = loop;
        zhengmian.animation.AnimationIndex = id;
        zhengmian.gameObject.SetActive(true);
        isCurrEnd = false;
    }
    public void playActionAuto(int id, bool loop, Action endaciton = null)
    {
        if (zhengmian == null)
        {
            return;
        }
        nowAction = id;
        zhengmian.animation.loop = loop;
        zhengmian.animation.setAnimationIndexForce(id);
        zhengmian.gameObject.SetActive(true);
        isCurrEnd = false;
        autoflag = true;
        this.endaciton = endaciton;
    }
    public void playActionForce(int id, bool loop)
    {
        if (zhengmian == null)
        {
            return;
        }
        isloop = loop;
        zhengmian.animation.loop = loop;
        zhengmian.animation.setAnimationIndexForce(id);
        zhengmian.gameObject.SetActive(true);
        setdepth(0);
        isCurrEnd = false;
        logic();
    }
    public void playActionForce2(int id, bool loop)
    {
        if (zhengmian == null)
        {
            return;
        }

        isloop = loop;
        zhengmian.gameObject.SetActive(true);
        zhengmian.animation.loop = loop;
        zhengmian.animation.setAnimationIndexForce(id);
        isCurrEnd = false;
        zhengmian.animation.apply(0);
        zhengmian.animation.repaint();
        setdepth(1000);
        logic();
    }
    public void playAction2(string idname, bool loop)
    {
        if (zhengmian == null)
        {
            return;
        }
        if (idname == null)
        {
            return;
        }
        animationname = idname;
        isloop = loop;
        int id = zhengmian.animation.state.Data.SkeletonData.FindAnimationId(idname);
        if (id == -1)
        {
            Debug.LogError(idname + " not found");

            id = 0;
        }
        nowAction = id;
        zhengmian.animation.loop = loop;
        zhengmian.animation.AnimationIndex = id;
       // UTools.setActive(zhengmian.gameObject, true);
        zhengmian.gameObject.SetActive(true);
        isCurrEnd = false;
        zhengmian.animation.apply(0);
        zhengmian.animation.repaint();

      //  Debug.LogError(id+" "+ idname);
    }
    public void playAction2ftxg(string idname, bool loop)
    {
        if (zhengmian == null)
        {
            return;
        }
        if (idname == null)
        {
            return;
        }
        animationname = idname;
        isloop = loop;
        int id = zhengmian.animation.state.Data.SkeletonData.FindAnimationId(idname);
        if (id == -1)
        {
            zhengmian.gameObject.SetActive(false);
          //  Debug.Log(idname + " not found");
            return;
        }
        nowAction = id;
        zhengmian.animation.loop = loop;
        zhengmian.animation.setAnimationIndexForce(id);
        // UTools.setActive(zhengmian.gameObject, true);
        zhengmian.gameObject.SetActive(true);
        isCurrEnd = false;
        zhengmian.animation.apply(0);
        zhengmian.animation.repaint();
    }
    public void playAction2Force2(string idname, bool loop)
    {
        if (zhengmian == null)
        {
            return;
        }
        if (idname == null)
        {
            return;
        }
        animationname = idname;
        isloop = loop;
        int id = zhengmian.animation.state.Data.SkeletonData.FindAnimationId(idname);
        if (id == -1)
        {
            Debug.LogError(idname + " not found");
        }
        nowAction = id;
        zhengmian.animation.loop = loop;
       // zhengmian.animation.AnimationIndex = id;
        zhengmian.animation.setAnimationIndexForce(id);
        // UTools.setActive(zhengmian.gameObject, true);
        zhengmian.gameObject.SetActive(true);
        isCurrEnd = false;
        zhengmian.animation.apply(0);
        zhengmian.animation.repaint();
        logic();
    }
    bool autoflag = false;
    float logicdelay;
    Action endaciton,endactionbackup,midaction;
    public void playAction2Auto(string idname, bool loop,Action endaciton=null, float delay = 0, Action midaction = null)
    {
        if (zhengmian == null)
        {
            return;
        }
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
        int id = zhengmian.animation.state.Data.SkeletonData.FindAnimationId(idname);
        if (id == -1)
        {
            Debug.LogError(idname + " not found");
        }
        nowAction = id;
        zhengmian.animation.loop = loop;
        zhengmian.animation.setAnimationIndexForce(id);
        // UTools.setActive(zhengmian.gameObject, true);
        if(delay!=0)
        zhengmian.gameObject.SetActive(false);
        isCurrEnd = false;
        zhengmian.animation.apply(0);
        zhengmian.animation.repaint();
    }
    public void playAction2Force(string idname, bool loop)
    {
        if (zhengmian == null)
        {
            return;
        }
        if (idname == null)
        {
            return;
        }
        animationname = idname;
        isloop = loop;
        int id = zhengmian.animation.state.Data.SkeletonData.FindAnimationId(idname);
        if (id == -1)
        {
            Debug.LogError(idname + " not found");
        }
        nowAction = id;
        zhengmian.animation.loop = loop;
        zhengmian.animation.setAnimationIndexForce(id);
        // UTools.setActive(zhengmian.gameObject, true);
        zhengmian.gameObject.SetActive(true);
        isCurrEnd = false;
        zhengmian.animation.apply(0);
        zhengmian.animation.repaint();
    }

    public void logic()
    {
        if (zhengmian == null)
        {
            return;
        }

        zhengstate.Update(Time.deltaTime);
        zhengmian.animation.Update(Time.deltaTime);

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
        if (zhengmian == null)
        {
            return;
        }

        if (linkspaintnode == null)
        {
            zhengstate.Update(delta);

       
        }


        zhengmian.animation.Update(delta);



        if (guanggen != null)
        {
            //Debug.Log(gameObject.name);
            //Debug.Log(gameObject.GetComponentInParent<UIPanel>().gameObject.name);

            if (guangaddpanel)
            {
                 if(guanggen.GetComponent<UIPanel>() != null)
                {
                    guanggen.GetComponent<UIPanel>().depth = gameObject.GetComponentInParent<UIPanel>().depth - 1;
                }

            }

            for (int i = 0; i < guanglist.Count; i++)
            {
                guanglist[i].setdepth(zhengmian.depth - 1);
                guanglist[i].logic(delta);
            }
        }

        if (lvgen != null)
        {
            lvpaintnode.setdepth(zhengmian.depth + 1);
            lvpaintnode.logic(delta);
        }

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
       // gameObject.GetComponentInParent<UIPanel>().Refresh();

        //SkeletonUtility su = zhengmian.gameObject.AddComponent<SkeletonUtility>();
        //GameObject obj = su.SpawnHierarchy(SkeletonUtilityBone.Mode.Follow, true, true, true);
        //SkeletonUtilityBone sub = obj.GetComponent<SkeletonUtilityBone>();
        //addwuli(sub);
        

        // Bone bone = zhengmian.animation.Skeleton.FindBone("Leg_L1");
        // Debug.Log(bone);
        //Bone bone2 = zhengmian.animation.Skeleton.FindBone("Leg_R1");
        // Debug.Log(bone2);
        // if(bone!=null)
        //  su.SpawnBoneRecursively(bone, transform, SkeletonUtilityBone.Mode.Override, true, true, true);
    }

    void addwuli(SkeletonUtilityBone genbone)
    {
        var followerRigidbody = genbone.gameObject.AddComponent<Rigidbody>();
        followerRigidbody.mass = 1;
        followerRigidbody.isKinematic = true;
        //SkeletonUtilityKinematicShadow sks = genbone.gameObject.AddComponent<SkeletonUtilityKinematicShadow>();
        //sks.detachedShadow = true;
        //sks.hideShadow = false;


        var utilityBones = genbone.gameObject.GetComponentsInChildren<SkeletonUtilityBone>();
        //   var childBoneParentReference = obj.transform;
        foreach (var childBone in utilityBones)
        {
            if (childBone == genbone)
            {
                continue;
            }

          //  Debug.Log(childBone.name);
            childBone.parentReference = childBone.transform.parent.transform;
            // childBone.transform.SetParent(chainParentObject.transform, true); // we need a flat hierarchy of all Joint objects in Unity.
            //  AttachRigidbodyAndCollider(childBone);
            childBone.mode = SkeletonUtilityBone.Mode.Override;
            childBone.scale = false;

            HingeJoint joint = childBone.gameObject.AddComponent<HingeJoint>();
            joint.axis = Vector3.forward;
            joint.connectedBody = childBone.transform.parent.GetComponent<Rigidbody>();
            joint.useLimits = true;
            joint.limits = new JointLimits
            {
                min = -40,
                max = 40
            };


            float length = childBone.bone.Data.Length / 1000;
            BoxCollider box = childBone.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(length, length / 3f, 0.2f);
            box.center = new Vector3(length / 2f, 0, 0);
            box.enabled = true;

            //SphereCollider sphere = childBone.gameObject.AddComponent<SphereCollider>();
            //sphere.radius = 0.1f;
            //sphere.enabled = true;

            Rigidbody rb = childBone.gameObject.GetComponent<Rigidbody>();
            // rb.isKinematic = true;
            rb.useGravity = false;

            //  childBone.GetComponent<Rigidbody>().mass = childBone.transform.GetComponent<Rigidbody>().mass * 0.75f;
            rb.mass = 100.00001f;
            rb.drag = 10.01f;
            rb.angularDrag = 10.01f;
            // childBoneParentReference = childBone.transform;
        }
    }

    void CreateHingeChain(SkeletonUtilityBone utilityBone)
    {
        var kinematicParentUtilityBone = utilityBone.transform.parent.GetComponent<SkeletonUtilityBone>();
        if (kinematicParentUtilityBone == null)
        {
           // UnityEditor.EditorUtility.DisplayDialog("No parent SkeletonUtilityBone found!", "Please select the first physically moving chain node, having a parent GameObject with a SkeletonUtilityBone component attached.", "OK");
            return;
        }

        SetSkeletonUtilityToFlipByRotation();

        kinematicParentUtilityBone.mode = SkeletonUtilityBone.Mode.Follow;
        kinematicParentUtilityBone.position = kinematicParentUtilityBone.rotation = kinematicParentUtilityBone.scale = kinematicParentUtilityBone.zPosition = true;
        /*
        // HingeChain Parent
        // Needs to be on top hierarchy level (not attached to the moving skeleton at least) for physics to apply proper momentum.
        GameObject chainParentObject = new GameObject(skeletonUtility.name + " HingeChain Parent " + utilityBone.name);
        var followRotationComponent = chainParentObject.AddComponent<FollowSkeletonUtilityRootRotation>();
        followRotationComponent.reference = skeletonUtility.boneRoot;
       
        // Follower Kinematic Rigidbody
        GameObject followerKinematicObject = new GameObject(kinematicParentUtilityBone.name + " Follower");
        followerKinematicObject.transform.parent = chainParentObject.transform;
        var followerRigidbody = followerKinematicObject.AddComponent<Rigidbody>();
        followerRigidbody.mass = 10;
        followerRigidbody.isKinematic = true;
        followerKinematicObject.AddComponent<FollowLocationRigidbody>().reference = kinematicParentUtilityBone.transform;
        followerKinematicObject.transform.position = kinematicParentUtilityBone.transform.position;
        followerKinematicObject.transform.rotation = kinematicParentUtilityBone.transform.rotation;

       

        // Child Bones
        var utilityBones = utilityBone.GetComponentsInChildren<SkeletonUtilityBone>();
        var childBoneParentReference = followerKinematicObject.transform;
        foreach (var childBone in utilityBones)
        {
            childBone.parentReference = childBoneParentReference;
            childBone.transform.SetParent(chainParentObject.transform, true); // we need a flat hierarchy of all Joint objects in Unity.
            AttachRigidbodyAndCollider(childBone);
            childBone.mode = SkeletonUtilityBone.Mode.Override;

            HingeJoint joint = childBone.gameObject.AddComponent<HingeJoint>();
            joint.axis = Vector3.forward;
            joint.connectedBody = childBoneParentReference.GetComponent<Rigidbody>();
            joint.useLimits = true;
            joint.limits = new JointLimits
            {
                min = -20,
                max = 20
            };
            childBone.GetComponent<Rigidbody>().mass = childBoneParentReference.transform.GetComponent<Rigidbody>().mass * 0.75f;

            childBoneParentReference = childBone.transform;
        }
        UnityEditor.Selection.activeGameObject = chainParentObject;

     */
    }
    void SetSkeletonUtilityToFlipByRotation()
    {
        //if (!skeletonUtility.flipBy180DegreeRotation)
        //{
        //    skeletonUtility.flipBy180DegreeRotation = true;
        //    Debug.Log("Set SkeletonUtility " + skeletonUtility.name + " to flip by rotation instead of negative scale (required).", skeletonUtility);
        //}
    }
    static void AttachRigidbodyAndCollider(SkeletonUtilityBone utilBone, bool enableCollider = false)
    {
        if (utilBone.GetComponent<Collider>() == null)
        {
            //if (utilBone.bone.Data.Length == 0)
            //{
            //    SphereCollider sphere = utilBone.gameObject.AddComponent<SphereCollider>();
            //    sphere.radius = 0.1f;
            //    sphere.enabled = enableCollider;
            //}
            //else
            {
                float length = utilBone.bone.Data.Length;
                BoxCollider box = utilBone.gameObject.AddComponent<BoxCollider>();
                box.size = new Vector3(length, length / 3f, 0.2f);
                box.center = new Vector3(length / 2f, 0, 0);
                box.enabled = enableCollider;
                box.enabled = true;
            }
        }
        Rigidbody rb = utilBone.gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = utilBone.gameObject.AddComponent<Rigidbody>();
            //  rb.isKinematic = true;
        }
        rb.isKinematic = true;
       // rb.mass = utilBone.transform.parent.transform.GetComponent<Rigidbody>().mass * 0.75f;

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
                UTools.setActive(zhengmian.gameObject, true);
                logic(0.03f);
            }
            
        }

        setrealalpha(0);
    }

    void OnDestroy()
    {
      //  Debug.Log("OnDestory");

           zhengmian=null;
           zhengstate = null;
           sag = null;
    }
    public int dp;
    public void setdepth(int v)
    {
        if (zhengmian == null)
        {
            return;
        }
        dp = v;
        // gameObject.GetComponent<UIWidget>().depth = v;

        if (gameObject.GetComponent<UIWidget>() != null)
        {
            Destroy(gameObject.GetComponent<UIWidget>());
        }
        zhengmian.GetComponent<NGUISpine>().depth = v;
    }
    public int getdepth()
    {
        return zhengmian.GetComponent<NGUISpine>().depth;
    }
    public void refreshdepth()
    {
        int dptemp = dp;
        setdepth(0);
        setdepth(dptemp+1);
    }
    public int alphavalue = 255;
    int colorr = 255;
    int colorg = 255;
    int colorb = 255;
    public void setAlpha(int v)
    {
        if (zhengmian == null)
        {
            return;
        }
        if (alphavalue == v)
        {
            return;
        }

        alphavalue = v;

        if (alphavalue < 0)
        {
            alphavalue = 0;
        }

        //if (twocolormat)
        //{
        //    if (meshRenderer == null)
        //    {
        //        meshRenderer = zhengmian.GetComponent<MeshRenderer>();
        //        block = new MaterialPropertyBlock();
        //    }
        //    block.SetColor(colorProperty, new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
        //    block.SetColor(blackTintProperty, Color.black);
        //    meshRenderer.SetPropertyBlock(block);
        //}
        //else
        {
            zhengmian.animation.skeleton.SetColor(new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
        }


    }
    public void setrealalpha(float value)
    {
        value = zhengmian.GetComponent<NGUISpine>().finalAlpha;
        zhengmian.animation.skeleton.SetColor(new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue*value / 255.0f));
    }

    public void setColor(int r, int g, int b)
    {
        if (zhengmian == null)
        {
            return;
        }
        colorr = r;
        colorg = g;
        colorb = b;

        //if (twocolormat)
        //{
        //    if (meshRenderer == null)
        //    {
        //        meshRenderer = zhengmian.GetComponent<MeshRenderer>();
        //        block = new MaterialPropertyBlock();
        //    }
        //    block.SetColor(colorProperty, new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
        //    block.SetColor(blackTintProperty, Color.black);
        //    meshRenderer.SetPropertyBlock(block);
        //}
        //else
        {
            zhengmian.animation.skeleton.SetColor(new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
        }
           
    }
    public void setColor(Color col)
    {
       // Debug.LogError(col);
        if (zhengmian == null)
        {
            return;
        }


        colorr = (int)(col.r*255);
        colorg = (int)(col.g * 255);
        colorb = (int)(col.b * 255);

      //  Debug.LogError(colorr + " " + colorg + " " + colorb);

        //if (twocolormat)
        //{
        //    if (meshRenderer == null)
        //    {
        //        meshRenderer = zhengmian.GetComponent<MeshRenderer>();
        //        block = new MaterialPropertyBlock();
        //    }
        //    //  block.SetColor(colorProperty, Color.whdite);
        //    //  block.SetColor(blackTintProperty, Color.white);
        //    block.SetColor(colorProperty, new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
        //    block.SetColor(blackTintProperty, Color.black);

        //    meshRenderer.SetPropertyBlock(block);
        //}
        //else
        {
            zhengmian.animation.skeleton.SetColor(new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
        }



    }
    public void setColor(int r, int g, int b,int alpha)
    {
        if (zhengmian == null)
        {
            return;
        }
        colorr = r;
        colorg = g;
        colorb = b;
        alphavalue = alpha;


        //if (twocolormat)
        //{
        //    if (meshRenderer == null)
        //    {
        //        meshRenderer = zhengmian.GetComponent<MeshRenderer>();
        //        block = new MaterialPropertyBlock();
        //    }
        //    block.SetColor(colorProperty, new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
        //    block.SetColor(blackTintProperty, Color.black);
        //    meshRenderer.SetPropertyBlock(block);
        //}
        //else
        {

            zhengmian.animation.skeleton.SetColor(new Color(colorr / 255.0f, colorg / 255.0f, colorb / 255.0f, alphavalue / 255.0f));
        }

    }
    MaterialPropertyBlock block;
    public Color freezeColor=new Color(0xb6 / 255.0f, 0xee / 255.0f, 0xf9/255.0f,1);
    public Color freezeBlackColor = new Color(0x5 / 255.0f, 0xa8 / 255.0f, 0xea / 255.0f,1);
    MeshRenderer meshRenderer;
    public string colorProperty = "_Color";
    public string blackTintProperty = "_Black";
    bool dongflag=false;
    public void setdong(bool flag)
    {
        if (dongflag == flag)
        {
            return;
        }
        if (meshRenderer == null)
        {
            meshRenderer = zhengmian.GetComponent<MeshRenderer>();
            block = new MaterialPropertyBlock();
        }

        if (flag)
        {
            block.SetColor(colorProperty, new Color(0xb6 / 255.0f, 0xee / 255.0f, 0xf9 / 255.0f, 1f));
            block.SetColor(blackTintProperty, new Color(0x5 / 255.0f, 0xa8 / 255.0f, 0xea / 255.0f, 1f));
            meshRenderer.SetPropertyBlock(block);
        }
        else
        {
            block.SetColor(colorProperty, new Color(1, 1, 1, 1));
            block.SetColor(blackTintProperty, Color.black);
            meshRenderer.SetPropertyBlock(block);
        }

        dongflag = flag;
    }

    public bool lvflag = false;
    public APaintNodeSpine lvpaintnode;
    public GameObject lvgen;
    public Material lvmat;
    public void setlv(bool lflag)
    {
        if (lvflag==lflag) {
            return;
        }

        if (!zhengmian.gameObject.activeSelf)
        {
            return;
        }

        lvflag = lflag;
        if (lflag)
        {
            if (lvmat == null)
            {
                lvmat = new Material(Shader.Find("myshader/guang"));
                lvmat.mainTexture = sag.loaderbean.img;

                Vector4 color = new Vector4(0, 1, 0x22 / 0xff, 1);
                lvmat.SetVector
                   (
                       Shader.PropertyToID("_OutlineColor"), color

                   );
            }
            if (lvgen != null)
            {
                lvgen.SetActive(true);
                return;
            }
            lvgen = NGUITools.AddChild(gameObject.transform.parent.gameObject);
            lvgen.name = "lvgen";


            {
                GameObject guangobj = NGUITools.AddChild(lvgen, gameObject);
                guangobj.name = "lv";
                guangobj.transform.localPosition = gameObject.transform.localPosition;
                APaintNodeSpine temppaintnode = guangobj.GetComponent<APaintNodeSpine>();
                temppaintnode.lvgen = null;
               // temppaintnode.setSkin(skinnameid[0], 0);
                temppaintnode.zhengstate = zhengstate;
                temppaintnode.linkspaintnode = this;

                temppaintnode.playAction2(animationname, isloop);

                NGUISpine texframeg = guangobj.GetComponentInChildren<NGUISpine>();
                texframeg.depth = zhengmian.depth + 1;
                texframeg.animation.mymaterial = lvmat;
                texframeg.animation.state = zhengstate;
                lvpaintnode = temppaintnode;
            }

        }
        else
        {
            lvgen.SetActive(false);
            //Destroy(lvpaintnode.gameObject);
            //Destroy(lvgen);
            //lvgen = null;
        }
    }

    public List<APaintNodeSpine> guanglist = new List<APaintNodeSpine>();
    public GameObject guanggen;
    public bool guangaddpanel=false;
    public Material guangmat;
    public static int[,] pianyiarray = {{ 0, 4 }, { 0, -4 }, { 4, 0 }, { -4, 0 } };
    public void setguang(bool gflag,bool haveskin=false)
    {
        if (gflag)
        {
            if (guangmat == null)
            {
                guangmat = new Material(Shader.Find("myshader/guang"));
               // guangmat = new Material(Shader.Find("Spine/Outline/Skeleton"));
                guangmat.mainTexture = sag.loaderbean.img;
            }
            if (guanggen != null)
            {
                return;
            }
            guanggen = NGUITools.AddChild(gameObject.transform.parent.gameObject);
            guanggen.name = "guanggen";
            if(guangaddpanel)
                guanggen.AddComponent<UIPanel>();
            for (int i = 0; i < 4; i++)
            {
                GameObject guangobj = NGUITools.AddChild(guanggen, gameObject);
                guangobj.name = "guang" + i;
                guangobj.transform.localPosition = new Vector3(pianyiarray[i, 0], pianyiarray[i, 1], 0);
                APaintNodeSpine temppaintnode = guangobj.GetComponent<APaintNodeSpine>();
                temppaintnode.guanggen = null;
                if (haveskin)
                {

                   // temppaintnode.setSkin(skinnameid[0], 0);
                }
                temppaintnode.zhengstate = zhengstate;
                temppaintnode.linkspaintnode = this;

                temppaintnode.playAction2(animationname, isloop);

                NGUISpine texframeg = guangobj.GetComponentInChildren<NGUISpine>();
                texframeg.depth = zhengmian.depth - 1;
                texframeg.animation.mymaterial = guangmat;
                texframeg.animation.state = zhengstate;
                guanglist.Add(temppaintnode);
            }
            if (haveskin)
            {
               if(guangaddpanel)
                guanggen.GetComponent<UIPanel>().Refresh();
            }
            // guanggen.GetComponent<UIPanel>().Refresh();
            // NGUITools.MarkParentAsChanged(guanggen.gameObject);
            if (guangaddpanel)
            gameObject.GetComponentInParent<UIPanel>().Refresh();

        }
        else
        {
            for (int i = 0; i < guanglist.Count; i++)
            {
                Destroy(guanglist[i].gameObject);
            }
            guanglist.Clear();
            Destroy(guanggen);
            guanggen = null;
        }

    }
    int guangcolor = -1;
    internal void setguangcolor(int v)
    {
        if (guangmat == null)
        {
            return;
        }
        if (guangcolor == v)
        {
            return;
        }
        guangcolor = v;
        Vector4 color = new Vector4(1, 0, 0, 1);
        if (v == 1)
        {
            color = new Vector4(1, 0, 0, 1);
        }
        else if (v == 2)
        {
            color = new Vector4(1, 1, 0, 1);
        }else if (v == 3)
        {
            color = new Vector4(1, 0, 1, 0.3f);
        }
      //  color = new Vector4(0, 1, 0x22 / 0xff, 1);
        guangmat.SetVector
           (
               Shader.PropertyToID("_OutlineColor"), color

           );
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
        skinname0=skinname0+ string.Format("{0:D2}", id);
        int k = zid;

        
        zhengmian.animation.skeleton.SetSkin(skinname0, false);
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
        Skin tempSkin = zhengmian.animation.skeleton.Data.FindSkin(skinname);
        Skin newSkin = new Skin(skinname + "_temp");
        ExposedList<Slot> slots = zhengmian.animation.skeleton.Slots;
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

        zhengmian.animation.skeleton.SetSkin(newSkin, true);
    }



}
