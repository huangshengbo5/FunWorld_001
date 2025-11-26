using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTou : MonoBehaviour
{
    public UISprite neilitiao;
    public UILabel namelabel,lvlabel;
    public GameObject headgen;

    public UIGrid grid;
    public GameObject itemobj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fbean != null)
        {
            neilitiao.fillAmount = fbean.hp * 1.0f / fbean.maxhp;
        }
    }
    FightObjBean fbean;
    FightObject fightObject;
    public  void setFO(FightObject fobj)
    {
        fightObject = fobj;
        this.fbean = fobj.fbean;
        namelabel.text = fbean.name;
        lvlabel.text = "等级:" + fbean.level;
        GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
        texframeobjs.name = "icon";
        TexPaintNode temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
        temppaintnode.create1(headgen, "huobantou");
        temppaintnode.setdepth(29);
        temppaintnode.setShowRectLimit(270);
        if (data_npcDef.getdatabynpcid(fbean.heroID)!=null)
        {
            temppaintnode.playAction(data_npcDef.getdatabynpcid(fbean.heroID).avatar);
            temppaintnode.GetComponentInChildren<UITexture>().flip = UITexture.Flip.Horizontally;
        }
        else
        {
            temppaintnode.playAction(data_heroDef.getdatabyheroid(fbean.heroID).face);
            temppaintnode.playAction(data_heroDef.getdatabyheroid(GameGlobal.gamedata.mainhero.heroID).face);
            lvlabel.text = "等级:" + GameGlobal.gamedata.mainhero.level;
           // namelabel.text = GameGlobal.gamedata.mainhero.name;
        }
       

    }
    public void refreshbufficon()
    {
        NGUITools.DestroyChildren(grid.transform);
        for (int i = 0; i < fightObject.bufflist.Count; i++)
        {
            FightBuff fb = fightObject.bufflist[i];

            if (data_buff_comparisonDef.dicdatas.ContainsKey(fb.buffid))
            {
                GameObject obj = NGUITools.AddChild(grid.gameObject, itemobj);
                obj.transform.localScale = itemobj.transform.localScale;
                obj.GetComponent<BuffIcon>().setdata(fb);
                UTools.setActive(obj, true);
            }


            //if (fb.agvisible)
            //{
            //    if (tempiconlist.Count > 0)
            //    {
            //        PaintNode pn = tempiconlist[0];
            //        tempiconlist.RemoveAt(0);
            //        pn.transform.parent = bufficongrid.transform;
            //        UTools.setActive(pn.gameObject, true);
            //        pn.setFrameSpr(0, fb.buffbean.ID);
            //        iconlist.Add(pn);
            //    }
            //    else
            //    {
            //        PaintNode icon = UTools.getIcon(bufficongrid.gameObject, "Buff_IconMini", 70);
            //        icon.transform.localScale = new Vector3(1, -1, 1);
            //        icon.setFrameSpr(0, fb.buffbean.ID);
            //        iconlist.Add(icon);
            //    }
            //}
        }
    }
}
