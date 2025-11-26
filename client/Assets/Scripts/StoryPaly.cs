using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryPaly : MonoBehaviour
{
    public UITexture storyTexture;
    public UILabel storyLab;
    public GameObject rootGo;
    public Action ActOnStoryPalyEnd;
    StoryData[] storyDatas;
    public TweenAlpha stroryTextureTA;
    public TweenPosition storyLabTP;
    TriggerCheck clipPoint;
    public GameObject clipPointCheckerGo;

    private void Awake()
    {
        storyDatas = new StoryData[3]
        {
           new StoryData{storyTextureName = "storybg1",storyDes = "于虚无中诞生的创世神\n以神力创造出不同的世界\n然而，随着创世神的隐去\n强大的生灵们以各自的世界为依托\n划分成不同的阵营\n光明和黑暗\n在无数年的对立和战争中不断扩张\n两大阵营之间\n从来不存在“和平”这种东西\n这是无关正义的对立\n征服，或者消灭，至高的权柄\n只会掌握在胜利者的手中"},
           new StoryData{storyTextureName = "storybg2",storyDes = "混乱中的秩序，残酷中的法则\n这是魔界从未改变的信条\n对光明不屑一顾的他们\n与审判军团的争斗从未停息\n 然而，某一天\n审判军团的大军突然降临魔界\n并且没有受到任何阻挠\n于是，他们轻易的进入深渊魔域\n包围措不及防的熔岩魔王\n濒临绝地的熔岩魔王陷入暴怒\n燃烧灵魂发动了最后一击\n结果，深渊魔域被彻底摧毁\n而熔岩魔王与审判军团的军队\n在可怖的洪流中灰飞烟灭"},
           new StoryData{storyTextureName = "storybg3",storyDes = "元素之主的目光\n似乎不再关注虔诚的信徒\n失去神的指引后\n千年战火使坎尼斯大陆满目疮痍\n神历3512年\n各种族终于停止了大规模的战争\n暗流涌动下\n脆弱的和平似乎已经来临\n然而，随着预言的传开\n难得的平静再次被打破……"},
        };
        clipPoint = clipPointCheckerGo.GetComponent<TriggerCheck>();
        clipPoint.onTriggerChecked += OnClipContenPlayEnd;
    }

    public void InitStoryData(StoryData[] storyData, Action actOnStoryPalyEnd = null)
    {
        storyDatas = storyData;
        if (actOnStoryPalyEnd != null)
        {
            ActOnStoryPalyEnd = actOnStoryPalyEnd;
        }
    }
    
    public void PlayStroy()
    {
        curClipIndex = 0;
        isEndPlay = false;
        rootGo.SetActive(true);
        setCurData();
    }

    void setCurData()
    {
        var curData = storyDatas[curClipIndex];

         var img = ResManager.getTex999("Atlas/stroyBgs/" + curData.storyTextureName);
          storyTexture.mainTexture = img;




        stroryTextureTA.ResetToBeginning();
        storyLab.text = curData.storyDes;
        var h = storyLab.localSize.y;
        clipPointCheckerGo.transform.localPosition = new Vector3(0, 300 + h, 0);
        storyLabTP.to = new Vector3(0, 960 + h, 0);
        storyLabTP.duration = 20;
        storyLabTP.ResetToBeginning();

    }

    public void onStoryPlayEnd()
    {
        ActOnStoryPalyEnd?.Invoke();
    }

    int curClipIndex = 0;
    bool isEndPlay = false;
    public void OnClipContenPlayEnd()
    {
        isEndPlay = true;
        stroryTextureTA.PlayReverse();
    }

    public void onClipPlayEnd()
    {
        if (isEndPlay)
        {
            curClipIndex += 1;
            if (curClipIndex >= storyDatas.Length)
            {

                onStoryPlayEnd();
            }
            else
            {
                isEndPlay = false;
                setCurData();
                stroryTextureTA.PlayForward();
                storyLabTP.PlayForward();
            }

        }
    }
}

public class StoryData
{
    public string storyTextureName;
    public string storyDes;
}