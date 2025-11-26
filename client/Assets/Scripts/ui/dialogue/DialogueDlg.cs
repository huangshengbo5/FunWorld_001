using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDlg : DialogMonoBehaviour
{
    public static DialogueDlg instance;

    public UILabel dialogueName;//对话名字
    public UILabel content;//对话内容
    public TypewriterEffect typeWriter;//打字
    public UISprite close;//关闭
    public UISprite dialogueBg;//背景
    public UISprite black;//黑色背景
    public TweenAlpha blackTween;//
    public UILabel blackText;//
    public GameObject bg;

    public GameObject leftRoot;//左边
    public GameObject rightRoot;//右边

    public APaintNodeSpine renpaintnode;
    public TexPaintNode texpaintnode;

    private bool isWaitFor = false;
    private float waitTime = 0;

    private bool isNeedBack = false;
    private float backTimer = 0;

    private float timer = 0f;
    private bool isNeedShank = false;
    private data_dialogueBean dialogueContent;
    private int lastType;//
    private int dialogueGroupId;
    private bool isPlayType = false;//是否正在播放打字
    private bool isPlayOver = false;
    private int index;//当前播放到打几句了
    private List<data_dialogueBean> DialogueList = new List<data_dialogueBean>();

    private void Awake()
    {
        instance = this;
        blackText.text = "";
        UIEventListener.Get(close.gameObject).onClick = OnCloseClick;
        UIEventListener.Get(dialogueBg.gameObject).onClick = OnDialogueBgClick;
        EventDelegate.Add(typeWriter.onFinished, OnTypeEffectFinish);
    }

    private void OnTypeEffectFinish()
    {
        isPlayOver = true;
        isPlayType = false;
    }

    //点击关闭
    private void OnCloseClick(GameObject go)
    {
        ComplateDialogue();
    }

    //点击背景
    private void OnDialogueBgClick(GameObject go)
    {
        if (isPlayType)
        {
            typeWriter.Finish();
            typeWriter.enabled = false;
            return;
        }
        if (isPlayOver)
        {
            index += 1;
            initDialogue();
        }
    }

    public void SetData(int groupId,System.Action callback)
    {
        isPlayOver = false;
        isPlayType = false;
        dialogueGroupId = groupId;
        if (data_dialogueDef.datasDic.ContainsKey(groupId))
        {
            DialogueList = data_dialogueDef.datasDic[groupId];
            index = 0;
            _Callback = callback;
        }
        else
        {
            Debug.Log("播放了一个表格里没有的对话组"+ groupId);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initDialogue();
    }

    //初始化boss信息
    private void initDialogue()
    {
        isPlayOver = false;
        typeWriter.enabled = false;
        if (index < DialogueList.Count)
        {
            //可以播放
            dialogueContent = DialogueList[index];
            blackText.text = "";
            if (dialogueContent.dialogue_type == 3 || dialogueContent.dialogue_type == 2)
            {
                bg.gameObject.SetActive(false);
                renpaintnode.gameObject.SetActive(false);
                if (lastType == 3 || lastType == 2)
                {
                    if (int.Parse(dialogueContent.dialogue_name) != 0)
                    {
                        blackText.text = dialogueContent.dialogue_content;
                    }
                }
                else
                {
                    black.gameObject.SetActive(true);
                    blackText.gameObject.SetActive(true);
                    blackTween.ResetToBeginning();
                    isNeedShank = true;
                    if (int.Parse(dialogueContent.dialogue_name) != 0)
                    {
                        blackText.text = dialogueContent.dialogue_content;
                    }
                }
            }
            else
            {
                black.gameObject.SetActive(false);
                blackText.gameObject.SetActive(false);
                bg.gameObject.SetActive(true);
                lastType = dialogueContent.dialogue_type;
                isPlayType = true;
                typeWriter.enabled = true;
                dialogueName.text = dialogueContent.dialogue_name;
                content.text = dialogueContent.dialogue_content;
                typeWriter.ResetToBeginning();

                //Debug.Log("播放顺序" + dialogueContent.dialogue_content);
                GameObject boosRoot = leftRoot;
                if (dialogueContent.model_position == 2)
                {
                    boosRoot = rightRoot;
                }
                if (renpaintnode == null)
                {
                    GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
                    texframeobjs.name = "" + dialogueContent.dialogue_model;
                    APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
                    temppaintnode.create1(boosRoot, dialogueContent.dialogue_model, dialogueContent.dialogue_model);
                    temppaintnode.transform.localPosition = Vector3.zero;
                    temppaintnode.transform.localRotation = Quaternion.identity;
                    temppaintnode.transform.localScale = Vector3.one * 0.5f;
                    temppaintnode.setdepth(11);
                    temppaintnode.setAutoAlpha(true);
                    renpaintnode = temppaintnode;
                }
                else
                {
                    renpaintnode.create1(boosRoot, dialogueContent.dialogue_model, dialogueContent.dialogue_model);
                    renpaintnode.transform.localPosition = Vector3.zero;
                    renpaintnode.transform.localRotation = Quaternion.identity;
                    renpaintnode.transform.localScale = Vector3.one * 0.5f;
                    renpaintnode.setdepth(11);
                    renpaintnode.setAutoAlpha(true);
                }
                renpaintnode.gameObject.SetActive(true);
                renpaintnode.playAction2Auto(dialogueContent.face_action, true);
            }
        }
        else
        {
            ComplateDialogue();
        }
    }

    public void ComplateDialogue()
    {
        //播放完毕
        if (_Callback != null)
        {
            _Callback();
        }
        Debug.Log("完成了一个剧情" + dialogueGroupId);
        closeDialog(null);
        GuideManager.Instance.CheckGuideActive((int)GuideConditionType.DialogueId, dialogueGroupId);
    }

    private System.Action _Callback; 

    // Update is called once per frame
    void Update()
    {
        if (isNeedShank)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                timer = 0;
                isNeedShank = false;
                //isPlayType = true;
                //typeWriter.enabled = true;
                if (dialogueContent.dialogue_content != null && dialogueContent.dialogue_content !="0")
                {
                    bg.gameObject.SetActive(false);
                    renpaintnode.gameObject.SetActive(false);
                    blackText.text = dialogueContent.dialogue_content;
                    //typeWriter.ResetToBeginning();
                    isWaitFor = true;

                }
                else
                {
                    blackTween.PlayReverse();
                    isNeedBack = true;
                }
            }
        }
        if (isWaitFor)
        {
            waitTime += Time.deltaTime;
            if (waitTime > 0.5f)
            {
                waitTime = 0;
                isWaitFor = false;
                blackTween.PlayReverse();
                isNeedBack = true;
            }
        }

        if (isNeedBack)
        {
            backTimer += Time.deltaTime;
            if (backTimer > 0.5f)
            {
                isNeedBack = false;
                backTimer = 0;
                black.gameObject.SetActive(false);
                index += 1;
                initDialogue();
            }
        }
    }
}
