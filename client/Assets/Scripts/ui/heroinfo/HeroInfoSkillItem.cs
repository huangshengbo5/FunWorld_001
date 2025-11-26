using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItemData
{
    public int skillIndex;
    public int skillid;
    public int skilllevel;
    public int nextneedlevel;
    public data_skillBean bean;
}

public class HeroInfoSkillItem : MonoBehaviour
{
    public UISprite paintnode;
    public SkillItemData skilldata;
    private void Awake()
    {
        UIEventListener.Get(gameObject).onClick = onEvent_button;
        SoundEventer.add_But_ClickSound(gameObject);
    }
    private void onEvent_button(GameObject go)
    {
        UIManager.showSkillInfo(skilldata);
    }

    public void initItem(SkillItemData _skilldata)
    {
        skilldata = _skilldata;
        setUI();
    }

    public void setUI()
    {
        if (paintnode != null)
        {
            paintnode.spriteName = skilldata.bean.skill_picture;
        }
    }

}
