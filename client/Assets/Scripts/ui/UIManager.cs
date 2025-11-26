using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{


    public static UIToast showToast(string content)
    {

        GameObject prefabUI = ResManager.getGameObject("allpre", "ui/toast/Toast");
        prefabUI.gameObject.SetActive(true);
        prefabUI.gameObject.transform.SetParent(UIViewGroup.getUIRoot().transform);
        prefabUI.gameObject.transform.localScale = new Vector3(1, 1, 1);
        prefabUI.transform.localPosition = new Vector3(0, 200, 0);

        ToastPoolDat poolDat = new ToastPoolDat(content);

        //ToastPool.addToast(poolDat);

        UIToast tip = prefabUI.GetComponent("UIToast") as UIToast;
        tip.setText(poolDat);
        return tip;
    }

    public static TipPoolDat showTip(string text, int type, int value, string confirmText, string centleText, TipDelegate dele, callbackInt tipEnableCallback = null, bool blurflag = true)
    {
        TipPoolDat poolDat = new TipPoolDat();
        poolDat.text = text;
        poolDat.confirmText = confirmText;
        poolDat.centleText = centleText;
        poolDat.dele = dele;

        poolDat.TipType = type;
        poolDat.TipZuanValue = value;

        poolDat.tipEnableCallback = tipEnableCallback;
        poolDat.blurflag = blurflag;

        DialogPool.addDialog(poolDat);
        return poolDat;
    }



    public static TipPoolDat showTip(string text, string confirmText, string centleText, TipDelegate dele, callbackInt tipEnableCallback = null,string title="")
    {
        TipPoolDat poolDat = new TipPoolDat();
        poolDat.text = text;
        poolDat.title = title;
        poolDat.confirmText = confirmText;
        poolDat.centleText = centleText;
        poolDat.dele = dele;

        poolDat.TipType = TipPoolDat.TIP_TYPE_TIP;

        poolDat.tipEnableCallback = tipEnableCallback;

        DialogPool.addDialog(poolDat);
        return poolDat;
    }


    public static TipPoolDat showTip_NoTitle(string text, string confirmText, string centleText, TipDelegate dele, callbackInt tipEnableCallback = null)
    {
        TipPoolDat poolDat = new TipPoolDat();
        poolDat.text = text;
        poolDat.confirmText = confirmText;
        poolDat.centleText = centleText;
        poolDat.dele = dele;

        //poolDat.TipType = TipPoolDat.TIP_TYPE_TIPNoTitle;
        poolDat.TipType = TipPoolDat.TIP_TYPE_TIP;

        poolDat.tipEnableCallback = tipEnableCallback;

        return showTip_NoTitle(poolDat);
    }

    public static TipPoolDat showTip_NoTitle(TipPoolDat poolDat)
    {
        DialogPool.addDialog(poolDat);
        return poolDat;
    }

    public static TipPoolDat showTipFont(string text, string confirmText, string centleText, TipDelegate dele)
    {
        TipPoolDat poolDat = new TipPoolDat();
        poolDat.text = text;
        poolDat.confirmText = confirmText;
        poolDat.centleText = centleText;
        poolDat.dele = dele;

        poolDat.TipType = TipPoolDat.TIP_TYPE_TIPFONT;

        DialogPool.addDialog(poolDat);
        return poolDat;
    }


    //显示Tip
    public static Tip showTip(TipPoolDat poolDat)
    {
        GameObject rankObj = null;
        //if (poolDat.TipType == TipPoolDat.TIP_TYPE_TIP)
        //{
        rankObj = ResManager.getGameObject("allpre", "ui/Tip/Tip");
        //}
        //else if (poolDat.TipType == TipPoolDat.TIP_TYPE_TIPFONT)
        //{
        //    rankObj = ResManager.getGameObject("allpre", "ui/Tip/TipFont");
        //}
        //else if (poolDat.TipType == TipPoolDat.TIP_TYPE_TIPNoTitle)
        //{
        //    rankObj = ResManager.getGameObject("allpre", "ui/Tip/TipNoTitle");
        //}


        GameObject rankGameObj = UIViewGroup.addPanel(rankObj, true, poolDat.blurflag);

        Tip tip = rankGameObj.GetComponent("Tip") as Tip;
        tip.setText(poolDat);
        return tip;
    }

    //public static BuildClassroomDlg showClassroom()
    //{
    //    GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/classroom/buildclassroomdlg");
    //    GameObject dlgobj = addPanel(dlgprefab, true);
    //    BuildClassroomDlg dlg = dlgobj.GetComponent<BuildClassroomDlg>();
    //    return dlg;
    //}

    public static HeroInfoDlg showHeroInfo()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/heroinfo/heroinfodlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        HeroInfoDlg dlg = dlgobj.GetComponent<HeroInfoDlg>();
        return dlg;
    }

    public static SkilinfoUI showSkillInfo(SkillItemData skilldata)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/heroinfo/SkilinfoUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        SkilinfoUI dlg = dlgobj.GetComponent<SkilinfoUI>();
        dlg.skildata = skilldata;
        return dlg;
    }

    public static PartnerInfoDlg showPartnerInfo()
    {
        if (GameGlobal.gamedata.partnerlist.Count == 0)
        {
            UIManager.showToast("未达到开放条件！");
            return null;
        }
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/partnerinfo/partnerinfodlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        PartnerInfoDlg dlg = dlgobj.GetComponent<PartnerInfoDlg>();
        return dlg;
    }
    public static BuildingInfoDlg showBuildingInfo(int index)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/buildinginfo/buildinginfodlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        BuildingInfoDlg dlg = dlgobj.GetComponent<BuildingInfoDlg>();
        BuildingInfoDlg.initindex = index;
        return dlg;
    }

    public static BuildingInfoUI showBuildingInfo()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/buildinginfo/BuildingInfoUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        BuildingInfoUI dlg = dlgobj.GetComponent<BuildingInfoUI>();
        return dlg;
    }

    public static TechnicalInfoUI showTechnicalInfo()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/technicalinfo/TechnicalInfoUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        TechnicalInfoUI dlg = dlgobj.GetComponent<TechnicalInfoUI>();
        return dlg;
    }
    public static ShopDlg showShop()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/shop/shopdlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        ShopDlg dlg = dlgobj.GetComponent<ShopDlg>();
        return dlg;
    }
    public static EquipInfoDlg showEquipInfo()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/equipinfo/equipinfodlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        EquipInfoDlg dlg = dlgobj.GetComponent<EquipInfoDlg>();
        return dlg;
    }

    public static TreasureInfoTip showTreasureInfoTip(Equip eq, int selectpos = 0, int fromType = 0, Action closeCall = null)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/tresurehunt/TreasureInfoTip");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        TreasureInfoTip dlg = dlgobj.GetComponent<TreasureInfoTip>();
        dlg.initdata(eq, selectpos, fromType, closeCall);
        return dlg;
    }
    public static TreasureselectUI showEquipSelect(int formPos = 0)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/tresurehunt/TreasureselectUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        TreasureselectUI dlg = dlgobj.GetComponent<TreasureselectUI>();
        dlg.selectPos = formPos;
        return dlg;
    }

    public static TreasurehuntInfoTip showTeasurehuntInfoTip()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/tresurehunt/TreasurehuntInfoTip");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        TreasurehuntInfoTip dlg = dlgobj.GetComponent<TreasurehuntInfoTip>();

        return dlg;
    }

    public static TreasurehuntOKTip showTreasurehuntOKTip()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/tresurehunt/TreasurehuntOKTip");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        TreasurehuntOKTip dlg = dlgobj.GetComponent<TreasurehuntOKTip>();
        return dlg;
    }

    public static ItemInfoTip showItemInfoTip(Reward reward)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/ItemInfoTip");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        ItemInfoTip dlg = dlgobj.GetComponent<ItemInfoTip>();
        dlg.InitData(reward);
        return dlg;
    }



    public static StageInfoDlg showStageInfo(data_campaignBean selectdbbean, int type, Monster monster = null)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/stageinfo/stageinfodlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        StageInfoDlg dlg = dlgobj.GetComponent<StageInfoDlg>();
        dlg.setData(selectdbbean, type, monster);
        return dlg;
    }
    public static FightWinDlg showFightWin(List<Reward> rblist, int fightType = 0)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/fightui/fightwindlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        FightWinDlg dlg = dlgobj.GetComponent<FightWinDlg>();
        dlg.setData(rblist, fightType);
        return dlg;
    }
    public static FightLoseDlg showFightLose()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/fightui/fightlosedlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        FightLoseDlg dlg = dlgobj.GetComponent<FightLoseDlg>();
        return dlg;
    }

    public static void showFightResult(bool win, List<Reward> rblist)
    {
        if (win)
        {
            UIManager.showFightWin(rblist, 5);

        }
        else
        {
            UIManager.showFightWin(rblist, 5);//世界boss特殊处理
            //UIManager.showFightLose();

        }
    }

    public static void showNetLoad(string v, long delayTime, long lastTime, bool couldCancel, bool notAllowClose = false)
    {
        hideNetLoad();

        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/netload/netloaddlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        NetLoadDlg dlg = dlgobj.GetComponent<NetLoadDlg>();
        dlg.setContent(v, delayTime, lastTime, couldCancel, notAllowClose);


    }

    public static void hideNetLoad()
    {
        if (NetLoadDlg.instance != null)
        {
            NetLoadDlg.instance.closeDialog(null);
        }
    }

    //打开竞技场
    public static PKDlg showPkInfo()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/pk/pkdlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        PKDlg dlg = dlgobj.GetComponent<PKDlg>();
        return dlg;
    }

    //打开竞技场奖励
    public static PKRewardDlg showRewardInfo()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/pk/pkrewarddlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        PKRewardDlg dlg = dlgobj.GetComponent<PKRewardDlg>();
        return dlg;
    }

    public static MailDlg showMailInfo()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/mail/maildlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        MailDlg dlg = dlgobj.GetComponent<MailDlg>();
        return dlg;
    }
    public static RTip showRTip()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/tip/rtip");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        RTip dlg = dlgobj.GetComponent<RTip>();
        return dlg;
    }

    public static MailMsgDlg showMailMsgInfo(MailInfo mail)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/mail/mailmsgdlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        MailMsgDlg dlg = dlgobj.GetComponent<MailMsgDlg>();
        dlg.SetData(mail);
        return dlg;
    }

    public static SpecialDoorUI showSpecialDoorUI()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/tianqiactivity/SpecialDoorUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        SpecialDoorUI dlg = dlgobj.GetComponent<SpecialDoorUI>();
        return dlg;
    }

    public static TianqiActivityDlg showTianqiActivity()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/tianqiactivity/TianQiActivityUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        TianqiActivityDlg dlg = dlgobj.GetComponent<TianqiActivityDlg>();
        return dlg;
    }

    public static tanxianDlg showTanxianDlg(GetTianxianInfoResponse response)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/tanxian/tanxianinfodlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        tanxianDlg dlg = dlgobj.GetComponent<tanxianDlg>();
        dlg.InitData(response);
        return dlg;
    }

    public static settingUI showsettingUI()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/heroinfo/HeroBaseInfoUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        settingUI dlg = dlgobj.GetComponent<settingUI>();
        return dlg;
    }

    public static ChangeNameTip showChangeNameTip()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/heroinfo/HerodChangeNameUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        ChangeNameTip dlg = dlgobj.GetComponent<ChangeNameTip>();
        return dlg;
    }

    public static ChangeHeadIconTip showChangeHeadIconTip()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/heroinfo/HeadIconSelectUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        ChangeHeadIconTip dlg = dlgobj.GetComponent<ChangeHeadIconTip>();
        return dlg;
    }

    //打开匹配界面
    public static TianqiMatchingDlg showTianqiMatching(List<Player> players)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/tianqiactivity/TianQiMatchingUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        TianqiMatchingDlg dlg = dlgobj.GetComponent<TianqiMatchingDlg>();
        dlg.initPalyers(players);
        return dlg;
    }

    //打开引导界面
    public static GuideDlg showGuideStep(int stepId)
    {
        if (GuideDlg.instance != null)
        {
            GuideDlg.instance.gameObject.SetActive(true);
            GuideDlg.instance.SetData(stepId);
            if (UIViewGroup.MaxDepth <= 100010)
            {
                UIViewGroup.orderPanelDeath(GuideDlg.instance.gameObject, 100010 + 2);
            }
            else
            {
                UIViewGroup.orderPanelDeath(GuideDlg.instance.gameObject, UIViewGroup.MaxDepth + 5);
            }
            return GuideDlg.instance;
        }
        else
        {
            GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/guide/guidedlg");
            GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, false);
            GuideDlg dlg = dlgobj.GetComponent<GuideDlg>();
            if (UIViewGroup.MaxDepth <= 100010)
            {
                UIViewGroup.orderPanelDeath(GuideDlg.instance.gameObject, 100010 + 2);
            }
            else
            {
                UIViewGroup.orderPanelDeath(GuideDlg.instance.gameObject, UIViewGroup.MaxDepth + 2);
            }
            dlg.SetData(stepId);
            return dlg;
        }
    }

    public static void hideGuideStep(GameObject go)
    {
        UIViewGroup.hidePanel(go);
    }


    //打开匹成就界面
    public static HistoryUI showHistoryUI()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/HistoryUI");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        HistoryUI dlg = dlgobj.GetComponent<HistoryUI>();
        return dlg;
    }

    public static DialogueDlg showDialogue(int groupId, System.Action callback)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/dialogue/dialoguedlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        DialogueDlg dlg = dlgobj.GetComponent<DialogueDlg>();
        dlg.SetData(groupId, callback);
        return dlg;
    }

    public static AwardDlg showAwardDlg(List<Reward> list)
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/award/awarddlg");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        AwardDlg dlg = dlgobj.GetComponent<AwardDlg>();
        dlg.SetData(list);
        return dlg;
    }


    public static BufferAddInfoTip showBufferAddInfoTip()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/BufferAddInfoTip");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        BufferAddInfoTip dlg = dlgobj.GetComponent<BufferAddInfoTip>();
        return dlg;
    }


    public static BusinessInfoTip showBusinessInfoTip()
    {
        GameObject dlgprefab = ResManager.getGameObject("allpre", "ui/BusinessInfoTip");
        GameObject dlgobj = UIViewGroup.addPanel(dlgprefab, true);
        BusinessInfoTip dlg = dlgobj.GetComponent<BusinessInfoTip>();
        return dlg;
    }
}
