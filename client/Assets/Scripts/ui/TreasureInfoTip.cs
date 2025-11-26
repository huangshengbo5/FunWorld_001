using System;
using UnityEngine;
public class TreasureInfoTip : DialogMonoBehaviour
{
    public GameObject closebtn;
    Equip equipdata;
    int selectPos = 0;
    int formtype = 0;

    public GameObject btnChange;
    public UILabel labbtnChange;
    public GameObject btnOffgo;
    public GameObject superMarkgo;

    public GameObject icongo;
    public UILabel labName;
    public UILabel labPwor;
    public UILabel labDes;
    public UILabel labDes2;
    private void Awake()
    {
        setShowAnim(true);
        setClickOutZoneClose(true);
        UIEventListener.Get(closebtn).onClick = closeDialog;
        UIEventListener.Get(btnChange).onClick = onbtnChangeClick;
        UIEventListener.Get(btnOffgo).onClick = onbtnOffClick;
        SoundEventer.add_But_ClickSound(closebtn);
        SoundEventer.add_But_ClickSound(btnChange);
        SoundEventer.add_But_ClickSound(btnOffgo);
    }

    private void onbtnOffClick(GameObject go)
    {
        HttpManager.instance.OffHeroEquip(equipdata, (callint) =>
         {
             if (callint == Callback.SUCCESS)
             {
                 //UIManager.showToast("装备卸下成功！");
                 closeDialog(closebtn);
             }
         });
    }

    private void onbtnChangeClick(GameObject go)
    {

        if (formtype == 1)
        {
            UIManager.showEquipSelect(selectPos);
            closeDialog(closebtn);
        }
        else
        {
            if (equipdata.pos > 0 && equipdata.pos != selectPos)
            {
                UIManager.showToast("已装备其他位置！");
                //closeDialog(closebtn);
                return;
            }
            if (checkEquipIsUnUse())
            {
                UIManager.showToast("已装备了该类型！");
                return;
            }
            HttpManager.instance.ChangeHeroEquip(equipdata, selectPos, (callint) =>
            {
                if (callint == Callback.SUCCESS)
                {
                    UIManager.showToast("装备成功！");
                    onCloseCall?.Invoke();
                    closeDialog(closebtn);
                }
            });
        }
    }



    private bool checkEquipIsUnUse()
    {
        bool isUnUse = false;
        for (int i = 0; i < GameGlobal.gamedata.equiplist.Count; i++)
        {
            if (GameGlobal.gamedata.equiplist[i].pos > 0 && GameGlobal.gamedata.equiplist[i].equipID == equipdata.equipID && GameGlobal.gamedata.equiplist[i].pos != selectPos)
            {
                isUnUse = true;
                break;
            }
        }
        return isUnUse;
    }

    Action onCloseCall;
    public void initdata(Equip eqdata, int pos = 0, int formType = 0, Action closeCall = null)
    {
        equipdata = eqdata;
        selectPos = pos;
        formtype = formType; //0无按钮,1英雄界面,2选择界面
        onCloseCall = closeCall;
        setUI();
        if (formType == 0)
        {
            btnChange.SetActive(false);
            btnOffgo.SetActive(false);
        }
        else if (formType == 1)
        {
            labbtnChange.text = "更换";
            btnChange.SetActive(true);
            btnOffgo.SetActive(true);
        }
        else
        {
            labbtnChange.text = "装备";
            btnChange.SetActive(true);
            btnOffgo.SetActive(false);
        }
        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.OpenWindow, (int)GuideAcitveWindow.BaowuEquip);
    }

    //public override void closeDilaogEvent()
    //{
    //    onCloseCall?.Invoke();
    //}

    void setUI()
    {
        data_equipBean dbdata = data_equipDef.dicdatas[equipdata.equipID][0];
        GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
        texframeobjs.name = "icon";
        TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
        temppaintnode.create1(icongo, "equip");
        temppaintnode.setdepth(402);
        temppaintnode.playAction(dbdata.icon);


        labName.text = dbdata.name + " 等级：" + equipdata.level;
        labDes2.text = dbdata.skill_desc;//特殊属性

        string des1 = "[594f60]" + dbdata.prop1_desc + "[-]";
        string des2 = "[594f60]" + dbdata.prop2_desc + "[-]";
        string des3 = "[594f60]" + dbdata.prop3_desc + "[-]";
        labPwor.text = dbdata.fighting_capacity;
        superMarkgo.SetActive(true);
        if (formtype > 0 && equipdata.id > 0)
        {
            des1 = "[594f60]" + GameGlobal.gamedata.GetEffectDesByID(equipdata.effectID1) + ":[-] [ff1531]+" + equipdata.effectVal1 + "[-]";
            des2 = "[594f60]" + GameGlobal.gamedata.GetEffectDesByID(equipdata.effectID2) + ":[-] [ff1531]+" + equipdata.effectVal2 + "[-]";
            des3 = "[594f60]" + GameGlobal.gamedata.GetEffectDesByID(equipdata.effectID3) + ":[-] [ff1531]+" + equipdata.effectVal3 + "[-]";
            superMarkgo.SetActive((equipdata.effectVal1 + equipdata.effectVal2 + equipdata.effectVal3) / 2 < 27002);
            labPwor.text = equipdata.fightingCapacity.ToString();
            string des = data_equip_skillDef.dicdatas[equipdata.skillID].desc;
            des = replaceDesVpos(des, 0, equipdata.skillEffect1);
            des = replaceDesVpos(des, 1, equipdata.skillEffect2);
            des = replaceDesVpos(des, 2, equipdata.skillEffect3);
            labDes2.text = des;
        }
        labDes.text = "1." + des1 + "\n2." + des2 + "\n3." + des3;

    }

    string replaceDesVpos(string des, int pos, int vl)
    {
        string head = "{" + pos + "_";
        string tp = des;
        if (tp.Contains(head + "1}"))//直接显示值
        {
            tp = des.Replace(head + "1}", vl.ToString());
        }
        if (tp.Contains(head + "2}"))//万分比值（需转万分比再转百分比显示）
        {
            tp = des.Replace(head + "2}", (vl / 100).ToString());
        }
        if (tp.Contains(head + "3}"))//毫秒时间值（需转为秒显示）
        {
            tp = des.Replace(head + "3}", (vl / 1000).ToString());
        }
        return tp;
    }
}