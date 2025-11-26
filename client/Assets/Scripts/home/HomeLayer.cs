using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeLayer : MonoBehaviour
{
    public GameObject[] buildtexs = new GameObject[13];

    void Awake()
    {

        for (int i = 0; i < buildtexs.Length; i++)
        {
            UIEventListener.Get(buildtexs[i]).onClick = onClickBuild;
            SoundEventer.add_But_ClickSound(buildtexs[i]);
            if(i< data_buildingDef.datas.Count)
            {
                data_buildingBean tempdbbean = data_buildingDef.getdatabybuildid(Statics.BUILDID + i);
                buildtexs[i].transform.GetChild(0).GetComponent<UILabel>().text = tempdbbean.name;
            }

        }
    }

    private void OnEnable()
    {
        setBuildingOpenState();
    }

    void setBuildingOpenState()
    {
        for (int i = 0; i < buildtexs.Length; i++)
        {
            if (i < data_buildingDef.datas.Count)
            {
                data_buildingBean tempdbbean = data_buildingDef.getdatabybuildid(Statics.BUILDID + i);
                buildtexs[i].SetActive(tempdbbean.condition_val <= GameGlobal.gamedata.stageindex);
            }

        }
        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow, (int)GuideAcitveWindow.Home);
    }

    private void onClickBuild(GameObject go)
    {
        for (int i = 0; i < buildtexs.Length; i++)
        {
            if (go == buildtexs[i])
            {
                if (i < 5)
                {
                    //UIManager.showBuildingInfo(i);
                    UIManager.showBuildingInfo();
                }
                else if (i == 5)
                {
                    UIManager.showTechnicalInfo();
                }
                else if (i == 14)
                {
                    UIManager.showToast("敬请期待！");
                }
                else if (i == 15)
                {
                    UIManager.showShop();
                }
                else if (i == 13)
                {
                    UIManager.showBusinessInfoTip();//商人
                }
                else if (i == 12)
                {
                    HttpManager.instance.MailList((code1) =>
                    {
                        if (code1 == Callback.SUCCESS)
                        {
                            UIManager.showMailInfo();
                        }

                    });
                }
                else if (i == 11)
                {
                    UIManager.showTeasurehuntInfoTip();
                }
                else if (i == 10)
                {
                    HttpManager.instance.GetTianxianInfo((info) =>
                    {
                        UIManager.showTanxianDlg(info);
                    });

                }
                else if (i == 9)
                {
                    //Debug.Log("是否已经报名"+ GameGlobal.gamedata.userinfo.extend.arenaissign);
                    UIManager.showToast("敬请期待！");
                    //if (GameGlobal.gamedata.userinfo.extend.arenaissign)
                    //{
                    //    HttpManager.instance.ArenaSignup(null);
                    //    //UIManager.showRewardInfo();
                    //}
                    //else
                    //{
                    //    HttpManager.instance.ArenaList((code1) =>
                    //    {
                    //        if (code1 == Callback.SUCCESS)
                    //        {
                    //            UIManager.showPkInfo();
                    //        }

                    //    });
                    //}
                }
                else if (i == 8)
                {
                    // UIManager.showSpecialDoorUI();

                    HttpManager.instance.TianqiShow((code1) =>
                    {
                        if (code1 == Callback.SUCCESS)
                        {
                            UIManager.showTianqiActivity();
                        }
                    });
                }
                else if (i == 7)
                {
                    UIManager.showBufferAddInfoTip();
                }
                else if (i == 6)
                {

                    HttpManager.instance.GetHistoryInfo((code1) =>
                    {
                        if (code1 == Callback.SUCCESS)
                        {
                            UIManager.showHistoryUI();
                        }

                    });
                }
                break;
            }
        }
    }
}