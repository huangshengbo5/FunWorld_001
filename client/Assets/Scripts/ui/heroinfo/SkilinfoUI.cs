
using System;
using UnityEngine;

public class SkilinfoUI : DialogMonoBehaviour
{
    public static SkilinfoUI instance;
    public SkillItemData skildata;
    public UISprite skillsp;
    public UILabel skillname;
    public UILabel skillUpcondition;
    public UILabel skillDesCurr;
    public UILabel skillDesNext;
    public GameObject btnClose;
    private void Awake()
    {
        instance = this;
        UIEventListener.Get(btnClose).onClick = closeDialog;
        SoundEventer.add_But_ClickSound(btnClose);
    }

    private void Start()
    {
        setUI();
    }

    private void setUI()
    {
        skillsp.spriteName = skildata.bean.skill_picture;
        skillname.text = skildata.bean.skill_name + " " + "等级" + skildata.skilllevel;
        skillUpcondition.text = skildata.nextneedlevel >0 ? "主角培养至" + skildata.nextneedlevel + "级升级":"已达最大等级";
        var skillleveldatas = data_skill_levelDef.dicdatas[skildata.skillid];
        data_skill_levelBean curdata = null;
        data_skill_levelBean nextdata = null;
        foreach (var item in skillleveldatas)
        {
            if (item.level == skildata.skilllevel)
            {
                curdata = item;
            }
            if (item.level == skildata.skilllevel+1)
            {
                nextdata = item;
            }
        }
            skillDesCurr.text = curdata.describe;
        if (nextdata != null)
        {
            skillDesNext.text = nextdata.describe;
        }else
        {
            skillDesNext.gameObject.SetActive(false);
        }

    }
}