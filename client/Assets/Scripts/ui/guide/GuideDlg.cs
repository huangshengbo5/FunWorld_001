using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideDlg : DialogMonoBehaviour
{
    public static GuideDlg instance;

    public GameObject guideRoot;//按钮跟随位置
    public UISprite guideBg;//背景
    public UIButton guideButton;//引导按钮
    public GameObject uiroot;//ui的根节点
    public GameObject guideObj;//当前指引的物体
    public GameObject root;//小手

    private data_guidesBean _currentGuide;//当前的引导信息
    APaintNodeSpine temppaintnode;

    private bool isClick = false;//玩家是否点击了
    private int index;//当前播放到打几句了
    private List<data_guidesBean> guideList = new List<data_guidesBean>();

    private void Awake()
    {

        uiroot = GameObject.Find("UI Root");
        instance = this;
        UIEventListener.Get(guideBg.gameObject).onClick = OnDialogueBgClick;
        UIEventListener.Get(guideButton.gameObject).onClick = OnGuideButtonClick;

        GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
        temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
        temppaintnode.create1(root, "xinShou", "xinShou");
        temppaintnode.transform.localPosition = Vector3.zero;
        //temppaintnode.transform.localScale = Vector3.one * 0.15f;
        temppaintnode.setdepth(root.GetComponent<UISprite>().depth + 10);
        temppaintnode.playAction2Auto("xinShou2", true, null);
    }

    private void OnGuideButtonClick(GameObject go)
    {
        if (isClick)
        {
            return;
        }
        isClick = true;
        if (_currentGuide != null)
        {
            if (_currentGuide.guidetype == 0)
            {
                //传递给目标
                if (guideObj != null)
                {
                    UIManager.hideGuideStep(this.gameObject);
                    guideObj.gameObject.SendMessage("OnClick");
                    //关闭当前
                    //this.gameObject.SetActive(false);
                }
                CheckServerStep();
            }
            else if (_currentGuide.guidetype == 1)
            {
                if (guideObj != null)
                {
                    UIManager.hideGuideStep(this.gameObject);
                    guideObj.gameObject.SendMessage("OnClick");
                    //关闭当前
                    //this.gameObject.SetActive(false);
                }
                CheckServerStep();
                if (_currentGuide.next != -1)
                {
                    //SetData(_currentGuide.next);
                    UIManager.showGuideStep(_currentGuide.next);
                }
                else
                {
                    GuideComplate(_currentGuide.group);
                }
            }
            else if (_currentGuide.guidetype == 2)
            {

                int id = _currentGuide.id;
                ComplateStepType(1, 0);
            }
        }
    }

    private void CheckServerStep()
    {
        if (_currentGuide != null)
        {
            if (_currentGuide.over == 1)
            {
                //标志着引导结束
                HttpManager.instance.sendGuideStep(_currentGuide.group, (code1) =>
                {
                    if (code1 == Callback.SUCCESS)
                    {
                        Debug.Log("成功同步引导步骤" + GameGlobal.gamedata.guideStep);

                    }
                });
            }
        }
    }

    //点击背景
    private void OnDialogueBgClick(GameObject go)
    {
        Debug.Log("玩家点击了背景图");
    }

    //设置引导的id
    public void SetData(int startId)
    {
        Debug.Log("当前引导id"+startId);
        if (data_guidesDef.guideDic.ContainsKey(startId))
        {
            var guideInfo = data_guidesDef.guideDic[startId];
            if (guideInfo != null)
            {
                _currentGuide = guideInfo;
                this.gameObject.SetActive(true);
                isClick = false;
                StartGuide();
            }
        }
    }

    //完成引导的类型
    public void ComplateStepType(int type,int param)
    {
        Debug.Log("检查当前引导步骤"+type);
        if (_currentGuide != null)
        {
            if (_currentGuide.guidetype == 0)
            {
                //功能
                if (type == _currentGuide.complatetype)
                {
                    if (param == _currentGuide.comparam)
                    {
                        //符合，走下一个
                        if (_currentGuide.next != -1)
                        {
                            //SetData(_currentGuide.next);
                            UIManager.showGuideStep(_currentGuide.next);
                        }
                        else
                        {
                            closeDialog(null);
                            GuideComplate(_currentGuide.group);
                        }
                    }
                }
            }
            else if (_currentGuide.guidetype == 1)
            {
                //不再受理
            }
            else if (_currentGuide.guidetype == 2)
            {
                //剧情
                //符合，走下一个
                if (_currentGuide.next != -1)
                {
                    //SetData(_currentGuide.next);
                    UIManager.showGuideStep(_currentGuide.next);
                }
                else
                {
                    closeDialog(null);
                    GuideComplate(_currentGuide.group);
                }
            }
        }
    }

    private void GuideComplate(int group)
    {
        _currentGuide = null;
        GuideManager.Instance.CheckGuideActive(3, group);//检查应该显示的引导
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void StartGuide()
    {
        if (_currentGuide != null)
        {
            if (_currentGuide.guidetype == 0 || _currentGuide.guidetype == 1)
            {
                //功能
                string path = _currentGuide.path;
                FindChildByPath(path);
                if (guideObj != null)
                {
                    guideRoot.transform.position = guideObj.transform.position;
                }
            }
            else if (_currentGuide.guidetype == 2)
            {
                //剧情
                UIManager.showDialogue(_currentGuide.comparam,OnDialogueCallback);
                closeDialog(null);
            }
            
        }
    }

    private void OnDialogueCallback()
    {
        Debug.Log("对话=======================事件完成");
        if (DialogueDlg.instance != null)
        {
            DialogueDlg.instance.closeDialog(null);
        }
        CheckServerStep();
        ComplateStepType(2, 0);//回调剧情的完成条件
    }

    public GameObject FindChildByPath(string path)
    {
        guideObj = null;
        string[] pathList = path.Split('/');
        for (int i = 0; i < pathList.Length; i++)
        {
            if (guideObj == null)
            {
                var child = GameObject.Find(pathList[i]);
                if (child != null)
                {
                    guideObj = child.gameObject;
                }
            }
            else
            {
                var child = guideObj.transform.Find(pathList[i]);
                if (child != null)
                {
                    guideObj = child.gameObject;
                }
            }
        }
        if (guideObj != null)
        {
            Debug.Log("已经找到需要的控件" + guideObj.ToString()+"路径是"+path);
        }
        else
        {
            Debug.Log("没有找到空间" + path.ToString());
        }
        return guideObj;
    }

    // Update is called once per frame
    void Update()
    {
        if (guideObj != null)
        {
            guideRoot.transform.position = guideObj.transform.position;
        }
    }
}
