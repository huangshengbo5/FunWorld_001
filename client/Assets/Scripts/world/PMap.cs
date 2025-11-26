using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMap : MonoBehaviour
{
    public static int mappixwidth = 6210;
    public static int mappixheight = 5376;
    public static int mappixbeginx = 0;
    public static int mappixbeginy = 0;

    public static float mapscale = 2.0f;

    public List<UITexture> bgs = new List<UITexture>();

    WorldLayer worldLayer;
    public void setworld(WorldLayer worldLayer)
    {
        this.worldLayer = worldLayer;
    }

    void Start()
    {
        initbg();

        gameObject.transform.localScale = new Vector3(mapscale, mapscale, mapscale);
    }

    private void initbg()
    {
        int tempy = 0;
        for(int i = 0; i < 12; i++)
        {
            {
                GameObject texframeobjs = ResManager.getGameObject("allpre", "umodtex");
                texframeobjs.name = "bg"+i;
                UITexture temppaintnode = texframeobjs.GetComponent<UITexture>();
                temppaintnode.pivot= UIWidget.Pivot.TopLeft;
                temppaintnode.mainTexture=ResManager.getTex999("map/map"+(i+1) );
                temppaintnode.MakePixelPerfect();
                temppaintnode.transform.parent = transform;
                temppaintnode.transform.localPosition = new Vector3(i % 4 * 2048, mappixheight-tempy);
                temppaintnode.transform.localScale = Vector3.one;

                if (i % 4 == 3)
                {
                    tempy += temppaintnode.height;
                }


                bgs.Add(temppaintnode);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
