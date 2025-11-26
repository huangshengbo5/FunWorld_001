using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightLoseDlg : DialogMonoBehaviour
{
    public GameObject texgen;
    // Start is called before the first frame update
    void Start()
    {
      //  setClickOutZoneClose(false);
        setCloseCallback((int code) =>
        {
            GameGlobal.enterMenuScene(true);
        });
        SoundPlayer.getInstance().PlayW("Gameover", false);

        NGUITools.DestroyChildren(texgen.transform);

        GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
        APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
        temppaintnode.create1(texgen, "shengLi-ShiBai", "shengLi-ShiBai");
        temppaintnode.transform.localPosition = Vector3.zero;
        //temppaintnode.transform.localScale = Vector3.one * 0.15f;
        temppaintnode.setdepth(21);
        temppaintnode.playAction2Auto("shiBai1", false, () => { temppaintnode.playAction2Auto("shiBai2", true); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        SoundPlayer.getInstance().Stop();
    }
}
