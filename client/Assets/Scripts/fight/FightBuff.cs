using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightBuff : MonoBehaviour
{
    public GameObject agroot, agroot1;
    public UILabel xxlabel;
    public FightControl fc;
    public void create(int v, FightControl fightControl, GameObject target)
    {
        fc = fightControl;
        gameObject.transform.SetParent(target.transform);
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.transform.localPosition = Vector3.zero;
    }

    public FightObject roleto;
    public int sta;
    public void iinit(int k, long sk, FightObject rto)
    {
        buffid = k;
        time = sk;
        roleto = rto;
        totaltime = time;


        gameObject.name = "buff" + buffid;
        {
            GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vlabel");
            texframeobjs111.transform.parent = agroot.transform;
            texframeobjs111.transform.localScale = Vector3.one;
            xxlabel = texframeobjs111.GetComponent<UILabel>();
            xxlabel.depth = 100;
            xxlabel.text = k + " " + sk;
            texframeobjs111.transform.localPosition = new Vector3(0, 350 + rto.getbuffNum(-1) * 50, 0);
            xxlabel.gameObject.SetActive(false);
        }

        if (data_buff_comparisonDef.dicdatas.ContainsKey(buffid)&&data_buff_comparisonDef.dicdatas[buffid][0].buff_special != "0")
        {
            string actionname = data_buff_comparisonDef.dicdatas[buffid][0].buff_special;
            GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
            APaintNodeSpine xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
            xgpaintnode.create1(agroot, actionname, actionname);
            xgpaintnode.playActionAuto(0, true);
            // xgpaintnode.playAction2(actionname, true);
            xgpaintnode.setdepth(rto.spaintnode.dp + 1);
            xgpaintnode.transform.localPosition = new Vector3(0, data_fight_positionDef.dicdatas[rto.agname][0].effects_vertigo_position, 0);
            xgpaintnode.gameObject.transform.localScale = Vector3.one;
        }
    }
   
    public int buffid;
    public long time,totaltime;

    public void settime(long btime)
    {
        time = btime;
        xxlabel.text = buffid + " " + btime;
    }
}
