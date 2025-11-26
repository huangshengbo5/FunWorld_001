using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager
{
    private static GuideManager _instance = new GuideManager();//

    public static GuideManager Instance
    {
        get { return _instance; }
    }

    //检查游戏中的引导事件类型
    public void GameGuideEventCheck(GuideActiveType type,int param)
    {
        if (GuideDlg.instance != null)
        {
            GuideDlg.instance.ComplateStepType((int)type, param);
        }
    }

    //检查引导的出现条件
    public void CheckGuideActive(int type,int param)
    {
        for (int i = 0; i < data_guidesDef.datas.Count; i++)
        {
            if (data_guidesDef.datas[i].conditiontype == type)
            {
                if (data_guidesDef.datas[i].conparam == param)
                {
                    Debug.Log("匹配到新手引导功能");
                    //匹配到新手引导，开始.
                    UIManager.showGuideStep(data_guidesDef.datas[i].id);
                }
            }
        }
    }
}
