using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePoint : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(gameObject).onClick = onclick;
        UIEventListener.Get(gameObject).onPress = onpress;
        UIEventListener.Get(gameObject).onDrag = ondrag;
        SoundEventer.add_But_ClickSound(gameObject);
    }

    private void onclick(GameObject go)
    {
        Debug.LogError(index);
        if (this == worldLayer.nowpoint)
        {
            if (!GameGlobal.gamedata.tongguan)
            {
                WorldLayer.ppnowindex = index;
                // UIManager.showStageInfo(worldLayer.selectdbbean);
                worldLayer.pp.walkto(index, () =>
                {
                    UIManager.showStageInfo(worldLayer.selectdbbean, 0);
                });
            }
            else
            {
                WorldLayer.ppnowindex = index;
                // UIManager.showStageInfo(worldLayer.selectdbbean);
                worldLayer.pp.walkto(index, () =>
                {
                    
                });
            }

        }
        else
        {
            WorldLayer.ppnowindex = index;
            // UIManager.showStageInfo(worldLayer.selectdbbean);
            worldLayer.pp.walkto(index, () =>
            {
                if (monster != null&&monster.status==0)
                {
                    UIManager.showStageInfo(worldLayer.getstagedatabyposid(index),1,monster);

                    //UIManager.showTip("打吗?", TipPoolDat.TIP_TYPE_TWO, -1, Lan.but_confirm, Lan.but_cancel, (int code) =>
                    //{
                    //    if (code == Tip.TIP_CONFIRM)
                    //    {
                    //        HttpManager.instance.sendMonsterFight(monster.towerID, (code1) =>
                    //        {

                    //            if (code1 == Callback.SUCCESS)
                    //            {
                    //                GameGlobal.enterFightScene();
                    //            }

                    //        });
                    //    }
                    //});
                }
            });
        }
    }

    private void ondrag(GameObject go, Vector2 delta)
    {
        worldLayer.OnDrag(delta);
    }

    private void onpress(GameObject go, bool state)
    {
        worldLayer.OnPress(state);
    }

    // Update is called once per frame
    void Update()
    {
        if (spaintnode != null)
        {
            spaintnode.logic(0.03f);
        }
    }
    public StagePointBean dbbean;
    public int index;
    public void setData(StagePointBean stagePointBean,int idx)
    {
        dbbean = stagePointBean;
        index = idx;
        gameObject.name = idx.ToString();
        gameObject.transform.localPosition = new Vector3(dbbean.x, dbbean.y);

        if (!GameGlobal.gamedata.tongguan)
            if (data_campaignDef.datas[GameGlobal.gamedata.stageindex].spot == dbbean.id)
        {
            //gameObject.GetComponent<UISprite>().color = Color.red;
            gameObject.GetComponent<UISprite>().spriteName="stagehong";

            GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
            APaintNodeSpine xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
            xgpaintnode.create1(gameObject, "xiaYiGuan", "xiaYiGuan");
            xgpaintnode.playActionAuto(0, true);
            // xgpaintnode.playAction2(actionname, true);
            xgpaintnode.setdepth(gameObject.GetComponent<UISprite>().depth + 1);
            xgpaintnode.transform.localScale = Vector3.one * 0.3f;
            xgpaintnode.transform.localPosition = new Vector3(0, 100, 0);

        }
    }
    public APaintNodeSpine spaintnode;
    public Monster monster;
    public void setMonster(Monster m)
    {
        monster = m;

        if (spaintnode != null)
        {
            Destroy(spaintnode.gameObject);
            spaintnode = null;
        }

        if (monster.status != 0)
        {
            return;
        }
        
        {
            string jsonname=data_npcDef.getdatabynpcid(data_towerDef.getdatabyid(monster.towerID).npc_id).resource;
           // jsonname = "luoya";
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
            texframeobjs.name = "tiao";
            APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
            temppaintnode.create1(gameObject, jsonname, jsonname);
            temppaintnode.transform.localPosition = Vector3.zero;
            temppaintnode.transform.localScale = Vector3.one * 0.15f;
            temppaintnode.setdepth(21);
            temppaintnode.playAction2Auto("idle", true);
            spaintnode = temppaintnode;
        }
    }
    WorldLayer worldLayer;
    public void setworld(WorldLayer worldLayer)
    {
        this.worldLayer = worldLayer;
    }

}
