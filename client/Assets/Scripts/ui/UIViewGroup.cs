using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class UIViewGroup : MuScene
{


    public static List<DialogMonoBehaviour> viewlist = new List<DialogMonoBehaviour>();
    public static List<DialogMonoBehaviour> dlglist = new List<DialogMonoBehaviour>();
    private static GameObject uiRoot = null;
    private static GameObject uiRoot0 = null;

    public static GameObject getUIRoot()
    {
        if (uiRoot == null)
        {
            setUIRoot();

        }

        return uiRoot;
    }



    private static void setUIRoot()
    {
        if (uiRoot != null)
        {
            return;
        }



        GameObject preobj = ResManager.getGameObject("allpre", "ui/otherroot");
        preobj.name = "uiroot";
        preobj.SetActive(true);
        uiRoot = preobj;
        uiRoot.transform.position = Vector3.zero;
        //uiRoot.AddComponent<PopInput>();
        //uiRoot.AddComponent<Loom>();
        //uiRoot.AddComponent<MusicPlayer>();
        //uiRoot.AddComponent<SoundPlayer>();
        //uiRoot.AddComponent<VoicePlayer>();
    }



    public static UICamera mapcamera;

    public static void setmapcamera(UICamera mc)
    {
        mapcamera = mc;
        removeAllDlg();
    }

    void Awake()
    {

        setUIRoot();
        //  DontDestroyOnLoad(gameObject);
        //  DontDestroyOnLoad(uiRoot);
        uiRoot0 = gameObject;

    }
    void Start()
    {
        // mapcamera = SimpleGame.instance.mapcamera;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static bool isBeBlockShadow(GameObject obj)
    {
        if (obj.transform.parent == null) return false;
        if (obj.transform.parent.name.Equals(obj.transform.name))
            //if (obj.transform.parent.tag.Equals("BlockShadow"))
            return true;
        return false;
    }



    //找到本Panel中最大的Death
    public static int getPanelMaxDeath(GameObject obj)
    {
        int maxDeath = int.MinValue;
        UIPanel[] panel = obj.GetComponentsInChildren<UIPanel>(true);

        for (int i = 0; i < panel.Length; i++)
        {
            maxDeath = panel[i].depth > maxDeath ? panel[i].depth : maxDeath;
        }
        return maxDeath;
    }

    //纠正Panel内的death数字
    public static void orderPanelDeath(GameObject obj, int minDepth)
    {
        UIPanel rootPanel = obj.GetComponent<UIPanel>();
        if (rootPanel == null)
        {
            Debug.Log("传入参数非UIPanel");
            return;
        }

        //排序
        UIPanel[] panel = obj.GetComponentsInChildren<UIPanel>(true);
        UIPanel temp = null;
        for (int i = panel.Length - 1; i > 0; --i)
        {
            for (int j = 0; j < i; ++j)
            {
                if (panel[j + 1].depth < panel[j].depth)
                {
                    temp = panel[j];
                    panel[j] = panel[j + 1];
                    panel[j + 1] = temp;
                }
            }
        }
        //重新赋值
        int baseDepth = 0;
        for (int i = 0; i < panel.Length; i++)
        {
            panel[i].depth = minDepth + baseDepth;
            baseDepth += 1;
        }
    }



    public static void removeNotification(DialogMonoBehaviour obj)
    {
        if (obj == null) return;



        removeNotification(obj, false, true);

        if (obj.isFullState())
        {

        }
    }





    /// <summary>
    /// 关闭dialog
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isImClose">是否立即关闭  忽略动画</param>
    /// <param name="isExceptCallback">是否执行callback</param>
    public static void removeNotification(DialogMonoBehaviour obj, bool isImClose, bool isExceptCallback)
    {


        if (!obj.getShowAnim() || isImClose)
        {
            removeNotificationNoAnim(obj, isExceptCallback);
            return;
        }

        if (obj.getShowAnim())
        {
            if (isBeBlockShadow(obj.gameObject))
            {
                UISprite texture = obj.transform.parent.Find("BlockShadow").GetComponent<UISprite>();

                TweenAlpha tweenAlpha = texture.gameObject.GetComponent<TweenAlpha>();
                if (tweenAlpha != null)
                {
                    tweenAlpha.enabled = true;
                    tweenAlpha.PlayReverse();
                }
            }

            /*

            TweenScale scale = obj.GetComponent<TweenScale>();
            if (scale == null)
            {
                obj.gameObject.AddComponent<TweenScale>();
            }


            //scale.RemoveOnFinished();
            //scale.AddOnFinished(() =>
            //{
            //    removeNotificationNoAnim(obj, isExceptCallback);
            //});

            //scale.ResetToBeginning();
            scale.enabled = true;
            scale.PlayReverse();

            */

            addani(obj.gameObject, false);

            TweenAlpha alpha = obj.GetComponent<TweenAlpha>();
            if (alpha != null)
            {
                alpha.enabled = true;
                alpha.PlayReverse();
            }
            alpha.RemoveOnFinished();
            alpha.AddOnFinished(() =>
            {
                removeNotificationNoAnim(obj, isExceptCallback);
            });
        }
    }


    //无动画直接关闭  或者  有动画 动画结束时关闭
    public static void removeNotificationNoAnim(DialogMonoBehaviour obj, bool isExceptCallback)
    {

        dlglist.Remove(obj);
        if (dlglist.Count == 0)
        {
            setblur(false);
        }

        if (obj.updlg == null)
        {
            topdlg = obj.downdlg;
            if (topdlg != null)
            {
                if (topdlg.blockObject != null)
                {
                    topdlg.blockObject.transform.SetParent(uiRoot.transform);
                    topdlg.blockObject.transform.localPosition = Vector3.zero;
                }
                else
                {
                    topdlg.gameObject.transform.SetParent(uiRoot.transform);
                    topdlg.transform.localPosition = Vector3.zero;
                }
            }
        }
        else
        {
            obj.updlg.downdlg = obj.downdlg;
        }





        obj.setAniReadyFlag(false);



        clearDestoryDialog();

        for (int i = 0; i < viewlist.Count; i++)
        {
            if (viewlist[i] == obj)
            {

                viewlist[i].closeDilaogEvent();

                if (obj.isHideTypeed())
                {
                    obj.setVisible(false);

                    hidePanel(obj.gameObject);


                    if (isExceptCallback)
                        obj.excateCloseCallback();
                }
                else
                {
                    destoryPanel(obj.gameObject);
                    viewlist.Remove(obj);

                    if (isExceptCallback)
                        obj.excateCloseCallback();

                }

                //把焦点交给 下面的那个ui
                if (i > 0 && !(obj is Tip))
                {
                    viewlist[i - 1].backForce();
                }

                dialogChange(false);

                if (obj.downdlg != null)
                {
                    obj.downdlg.updlg = null;
                }

                return;
            }
        }
    }

    //跳转场景时使用
    public static void removeAllDlg()
    {
        if (uiRoot != null)
        {
            NGUITools.DestroyChildren(uiRoot.transform);
            uiRoot = null;
        }
        if (uiRoot0 != null)
        {
            NGUITools.DestroyChildren(uiRoot0.transform);
        }



        topdlg = null;
        dlglist.Clear();
        viewlist.Clear();
        DialogPool.clearPool();

    }
    public static void dialogChange(bool isVisible)
    {
        //显示新的界面了
        if (isVisible)
        {
            //if (UIHelp.isShowFullDialog())
            //{
            //    if (SimpleGame.instance != null && SimpleGame.instance.map.map != null)
            //    {
            //        SimpleGame.instance.map.map.pmap.gameObject.SetActive(false);
            //    }

            //}
            //if (SimpleGame.instance != null && SimpleGame.instance.map.map != null)
            //{
            //    SimpleGame.instance.map.map.nowche = null;
            //}
        }
        else//关闭了界面
        {
            //if (!UIHelp.isShowFullDialog())
            //{
            //    if (SimpleGame.instance != null && SimpleGame.instance.map.map != null)
            //    {
            //        SimpleGame.instance.map.map.pmap.gameObject.SetActive(true);
            //        SimpleGame.instance.map.map.repaint();
            //    }

            //}
        }
    }
    public static GameObject addBindBlockShadow(GameObject parent, float alpha)
    {
        GameObject blockPrefab = ResManager.getGameObject("allpre", "ui/exui/BlockShadow");
        GameObject blockShadow = UTools.AddChild(parent, blockPrefab);

        UISprite texture = blockShadow.transform.Find("BlockShadow").GetComponent<UISprite>();
        //         texture.alpha = alpha;

        TweenAlpha tweenAlpha = texture.gameObject.AddComponent<TweenAlpha>();
        tweenAlpha.from = 0.01f;
        tweenAlpha.to = alpha;

        tweenAlpha.duration = time;
        tweenAlpha.delay = 0;

        tweenAlpha.enabled = true;
        tweenAlpha.PlayForward();

        return blockShadow;
    }
    public static GameObject getParentBlockShadow(GameObject gameObject)
    {
        if (isBeBlockShadow(gameObject))
        {
            return gameObject.transform.parent.gameObject;
        }
        return null;
    }

    //同一个场景的时候使用
    public static void removeAllNotification(bool isImClear)
    {
        clearDestoryDialog();

        for (int i = viewlist.Count - 1; i >= 0; i--)
        {
            DialogMonoBehaviour monoBehaviour = viewlist[i];
            removeNotification(monoBehaviour, isImClear, false);
        }

        DialogPool.clearPool();
        //   LevelUpPool.clearPool();
    }

    public static int MaxDepth
    {
        get { return depth; }
    }

    private static readonly int minDepth = 100010;
    private static int depth = -1;
    private static DialogMonoBehaviour topdlg;
    public static void addNotification(DialogMonoBehaviour monoBehaviour, bool addblur, float scaleDialog = 1)
    {

        clearDestoryDialog();

       // Debug.Log("=============" + monoBehaviour.ToString());
        GameObject blockObject = getParentBlockShadow(monoBehaviour.gameObject);
        GameObject newObject = monoBehaviour.gameObject;

        //========更改depth顺序 排序=========
        {
            if (viewlist.Count > 0)
            {
                GameObject lastObject = null;
                //纠错
                if (viewlist[viewlist.Count - 1] != null && viewlist[viewlist.Count - 1].gameObject != null)
                {
                    lastObject = viewlist[viewlist.Count - 1].gameObject;
                    depth = getPanelMaxDeath(lastObject) + 1;
                }
                else
                {
                    DialogMonoBehaviour monoErr = viewlist[viewlist.Count - 1];
                    if (monoErr != null)
                    {
                        Debug.LogError("出问题了。。。。。 啊亲======" + monoErr.ToString());
                    }
                    else
                    {
                        Debug.LogError("出问题了。。。。。 啊亲====== notifyArray 中存在null的占位符");
                    }

                    //TODO
                    depth += 1;
                }
            }

            UIPanel newPanel = newObject.GetComponent<UIPanel>();
            //基础Depth
            int baseDepth = (depth < 0) ? minDepth : depth;
            //是否排序深度
            if (blockObject != null)
            {
                blockObject.GetComponent<UIPanel>().depth = baseDepth;
                orderPanelDeath(newObject, baseDepth + 1);
            }
            else
            {
                orderPanelDeath(newObject, baseDepth);
            }
        }
        //-------end--------

        showPanel(monoBehaviour, scaleDialog);
        viewlist.Add(monoBehaviour);

        dialogChange(true);
        monoBehaviour.setVisible(true);


        dlglist.Add(monoBehaviour);

        if (addblur&&false)
        {
            if (topdlg != null)
            {
                if (topdlg.blockObject != null)
                {
                    topdlg.blockObject.transform.SetParent(uiRoot0.transform);
                    topdlg.blockObject.transform.localPosition = Vector3.zero;
                }
                else
                {
                    topdlg.gameObject.transform.SetParent(uiRoot0.transform);
                    topdlg.transform.localPosition = Vector3.zero;
                }
            }
            setblur(true);
        }


        monoBehaviour.downdlg = topdlg;
        if(topdlg!=null)
        topdlg.updlg = monoBehaviour;

        topdlg = monoBehaviour;
    }

    public static bool isPanelShow(GameObject gameObject)
    {
        if (gameObject == null) return false;

        if (gameObject.activeSelf == false) return false;

        if (isBeBlockShadow(gameObject))
        {
            if (gameObject.transform.parent.gameObject.activeSelf == false)
            {
                return false;
            }
        }
        return true;
    }
    public static void destoryPanel(GameObject obj)
    {
        if (isBeBlockShadow(obj))
        {
            NGUITools.Destroy(obj.transform.parent);
        }
        else
        {
            NGUITools.Destroy(obj);
        }
    }
    private const float time = 0.15f;
    public static GameObject showPanel(DialogMonoBehaviour dialogMono, float scaleDialog)
    {

        GameObject obj = null;
        //         if (!isPanelShow(gameObject))
        //         {
        obj = dialogMono.gameObject;
        if (isBeBlockShadow(obj))
        {
            obj.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            obj.SetActive(true);
        }

        if (!dialogMono.getShowAnim())
        {
            dialogMono.setAniReadyFlag(true);
            return obj;
        }
        else
        {
            /*
            TweenScale scale = obj.GetComponent<TweenScale>();
            if (scale == null)
            {
                scale = obj.AddComponent<TweenScale>();
            }
            scale.animationCurve = new AnimationCurve();
            scale.animationCurve.AddKey(0f, 0f);
            scale.animationCurve.AddKey(0.8f * scaleDialog, 1.05f * scaleDialog);
            scale.animationCurve.AddKey(1.0f * scaleDialog, 1.0f * scaleDialog);

            scale.onFinished.Clear();
            //scale.from = new Vector3(0.7f * scaleDialog, 0.7f * scaleDialog, 1);
            scale.from = new Vector3(0.1f * scaleDialog, 1.0f * scaleDialog, 1);
            scale.to = new Vector3(1 * scaleDialog, 1 * scaleDialog, 1 * scaleDialog);

            scale.duration = time;
            scale.delay = 0;

            scale.ResetToBeginning();
            scale.enabled = true;
            scale.PlayForward();
            */
            addani(obj, true);

            TweenAlpha alpha = obj.GetComponent<TweenAlpha>();
            if (alpha == null)
            {
                alpha = obj.AddComponent<TweenAlpha>();
            }
            alpha.onFinished.Clear();

            if (obj.GetComponent<UIPanel>() != null)
            {
                obj.GetComponent<UIPanel>().alpha = 0.01f;
            }

            alpha.from = 0.01f;


            //alpha.delay = 0;
            //alpha.from = 0.98f;
            alpha.to = 1.0f;


            alpha.delay = 0.18f;

            alpha.duration = time;


            alpha.onFinished.Clear();
            alpha.AddOnFinished(() =>
            {
                dialogMono.setAniReadyFlag(true);
            });
            alpha.ResetToBeginning();
            alpha.enabled = true;
            alpha.PlayForward();
        }

        return obj;
    }

    private static void addani(GameObject gobj, bool chuxianxiaoshi)
    {
        if (true)
        {
            return;
        }

        Animator ani = gobj.GetComponent<Animator>();
        if (ani == null)
        {
            ani = gobj.gameObject.AddComponent<Animator>();
        }
        if (chuxianxiaoshi)
        {
            UnityEngine.Object obj = Resources.Load("AnimatorController/chuxiancontrol") as UnityEngine.Object;
            ani.enabled = true;
            ani.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);
        }
        else
        {
            UnityEngine.Object obj = Resources.Load("AnimatorController/xiaoshicontrol") as UnityEngine.Object;
            ani.enabled = true;
            ani.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);

            //RuntimeAnimatorController controller  = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(obj);
            //AnimationClip runClip= controller.animationClips[0];
            ////动态添加事件
            //AnimationEvent aEvent1 = new AnimationEvent();
            //aEvent1.time = runClip.length;
            //aEvent1.functionName = "OnOpenComplete";
            //aEvent1.stringParameter = runClip.length.ToString();
            //runClip.AddEvent(aEvent1);
        }


    }

    public static void hidePanel(GameObject gameObject)
    {
        if (isPanelShow(gameObject))
        {
            if (isBeBlockShadow(gameObject))
            {
                gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
    public static GameObject addPanel(GameObject obj, bool addBlockShadow,bool addblur)
    {
        return addPanel(obj, addBlockShadow,addblur, 0.7f);
    }
    public static GameObject addPanel(GameObject obj, bool addBlockShadow)
    {
        return addPanel(obj, addBlockShadow,true, 0.7f);
    }
    public static GameObject addPanel(GameObject obj, bool addBlockShadow,bool addblur, float alpha, float scaleDilaog = 1)
    {
        clearDestoryDialog();

        obj.SetActive(true);
        //检查是否为 隐藏类型  只创建一个
        DialogMonoBehaviour typeBehaviour = obj.GetComponent<DialogMonoBehaviour>();
        for (int i = 0; i < viewlist.Count; i++)
        {
            if (viewlist[i].GetType() == typeBehaviour.GetType())
            {
                if (viewlist[i].isHideTypeed())
                {

                    removeNotification(viewlist[i]);

                    addNotification(viewlist[i], addblur,scaleDilaog);

                    return viewlist[i].gameObject;
                }
            }
        }
        //--------end

        GameObject blockObject = null;
        GameObject newObject = null;
        //==========是否添加黑框=============
        if (addBlockShadow)
        {
            blockObject = addBindBlockShadow(getUIRoot(), alpha);



            blockObject.name = obj.name;

            newObject = UTools.AddChild(blockObject, obj);

            newObject.SetActive(true);

            newObject.GetComponent<DialogMonoBehaviour>().setBlockShadow(blockObject);
        }
        else
        {

            newObject = UTools.AddChild(getUIRoot(), obj);

            newObject.SetActive(true);
        }
        //-----------end---------------


        addNotification(newObject.GetComponent<DialogMonoBehaviour>(), addblur,scaleDilaog);


        return newObject;
    }
    private static void clearDestoryDialog()
    {
        if (viewlist.Count < 1) return;

        for (int i = viewlist.Count - 1; i >= 0; i--)
        {
            if (viewlist[i] == null || (viewlist[i] != null && viewlist[i].gameObject == null))
            {
                viewlist.RemoveAt(i);
            }
        }
    }

  




    private static void setblur(bool v)
    {


        if (mapcamera.GetComponent<Blur>() != null)
        {
            mapcamera.GetComponent<Blur>().enabled = v;
        }
    }


}
