using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

public class NewStoryPaly : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private RenderTexture renderTexture;
    public UITexture texture;
    public Action ActOnStoryPalyEnd;
    void Start()
    {

        videoPlayer.loopPointReached += OnPlayerFinish;
        renderTexture = new RenderTexture(texture.width, texture.height, 24);
        renderTexture.useMipMap = false;
        videoPlayer.targetTexture = renderTexture;
        texture.mainTexture = renderTexture;
    }
    private void OnPlayerFinish(VideoPlayer player)
    {
        texture.gameObject.SetActive(false);
        ActOnStoryPalyEnd();
    }

    internal void PlayStroy()
    {
        texture.gameObject.SetActive(true);
    }
    /*
public UITexture storyTexture;
public UILabel storyLab;
public GameObject rootGo;
public Action ActOnStoryPalyEnd;
StoryData[] storyDatas;
public TweenAlpha stroryTextureTA;
public TweenPosition storyLabTP;
TriggerCheck clipPoint;
public GameObject clipPointCheckerGo;
public TweenAlpha whiteBg;//白色背景

private void Awake()
{
   storyDatas = new StoryData[8]
   {
      new StoryData{storyTextureName = "storybg1",storyDes = "莱昂：嗯？这是哪呀？我不是和审判军团的那群混蛋同归于尽了吗..."},
      new StoryData{storyTextureName = "storybg1",storyDes = "谜之声：哈!哈!哈!本大爷的血脉怎会如此轻易的断绝？ "},
      new StoryData{storyTextureName = "storybg1",storyDes = "莱昂：什么你的血脉？你是谁呀？ "},
      new StoryData{storyTextureName = "storybg1",storyDes = "谜之声：我当然就是你伟大的父亲呀，哈！哈！哈！"},
      new StoryData{storyTextureName = "storybg1",storyDes = "莱昂：我特么还是你爷爷呢！少在这里认便宜亲戚！"},
      new StoryData{storyTextureName = "storybg1",storyDes = "谜之声：呵呵！好！很好！"},
      new StoryData{storyTextureName = "storybg1",storyDes = "莱昂：喂！你在干什么？！！！ "},
      new StoryData{storyTextureName = "storybg1",storyDes = "莱昂：啊~啊~啊~啊~啊~啊~啊~啊~啊~！ "},
   };
   clipPoint = clipPointCheckerGo.GetComponent<TriggerCheck>();
   clipPoint.onTriggerChecked += OnClipContenPlayEnd;
   var img = ResManager.getTex999("Atlas/stroyBgs/storybg1");
   storyTexture.mainTexture = img;
   whiteBg.gameObject.SetActive(false);


   GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
   temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
   temppaintnode.create1(stroryTextureTA.gameObject, "xuanWo", "xuanWo");
   temppaintnode.transform.localPosition = Vector3.zero;
   //temppaintnode.transform.localScale = Vector3.one * 0.15f;
   temppaintnode.setdepth(stroryTextureTA.GetComponent<UITexture>().depth+1);
   temppaintnode.playAction2Auto("xunHuan-WuKuang", true, null);
}
APaintNodeSpine temppaintnode;
void Update()
{
   if (temppaintnode != null)
   {
       temppaintnode.setAlpha((int)(stroryTextureTA.GetComponent<UITexture>().alpha*255));
   }
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
   whiteBg.gameObject.SetActive(false);
   isEndPlay = false;
   rootGo.SetActive(true);
   setCurData();
   MusicPlayer.getInstance().PlayW("storybgm", false);
}

void setCurData()
{
   var curData = storyDatas[curClipIndex];
   stroryTextureTA.ResetToBeginning();
   storyLab.text = curData.storyDes;
   var h = storyLab.localSize.y;
   clipPointCheckerGo.transform.localPosition = new Vector3(0, 300 + h, 0);
   storyLabTP.to = new Vector3(0, -400, 0);
   if (curClipIndex == 0)
   {
       storyLabTP.duration = 6;
   }
   else if (curClipIndex == 1)
   {
       storyLabTP.duration = 8;
   }
   else if (curClipIndex == 2)
   {
       storyLabTP.duration = 3;
   }
   else if (curClipIndex == 3)
   {
       storyLabTP.duration = 6;
   }
   else if (curClipIndex == 4)
   {
       storyLabTP.duration = 4;
   }
   else if (curClipIndex == 5)
   {
       storyLabTP.duration = 6;
   }
   else if (curClipIndex == 6)
   {
       storyLabTP.duration = 2;
   }
   else if (curClipIndex == 7)
   {
       storyLabTP.duration = 7;
   }
   else
   {
       storyLabTP.duration = 5;
   }

   storyLabTP.ResetToBeginning();
   string music = "gamestory0" + (curClipIndex + 1);
   SoundPlayer.getInstance().PlayW(music, false);
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
       if (curClipIndex >= storyDatas.Length-1)
       {
           setCurData();
           whiteBg.gameObject.SetActive(true);
           whiteBg.ResetToBeginning();
           whiteBg.PlayForward();
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

public void OnWhiteBgOver()
{
   onStoryPlayEnd();
}
*/
}