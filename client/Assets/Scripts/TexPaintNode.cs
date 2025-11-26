using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexPaintNode : MonoBehaviour
{
    public string path;
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void create1(GameObject die, string v)
    {
        path = v;
        gameObject.transform.SetParent(die.gameObject.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }
    public int dp;
    public void setdepth(int v) {
        dp = v;
        gameObject.GetComponent<UITexture>().depth = v;

   }
    Texture2D img=null;
    public void playAction(int v,int v2)
    {
        string v2str = v2.ToString();
        playAction(v, v2str);

    }
    public void playAction(int v, string v2)
    {
        img = ResManager.getTex999("actiongroup/" + path + "/mod" + v + "_" + v2);
        gameObject.GetComponent<UITexture>().mainTexture = img;
        gameObject.GetComponent<UITexture>().MakePixelPerfect();
        adjustLocalScale();
    }

    public void playAction(string resName)
    {
        img = ResManager.getTex999("actiongroup/" + path + "/" + resName);
        gameObject.GetComponent<UITexture>().mainTexture = img;
        gameObject.GetComponent<UITexture>().MakePixelPerfect();
        adjustLocalScale();
    }

    private int rectW = -1;
    private int rectH = -1;
    public void setShowRectLimit(int size)
    {
        this.rectW = size;
        this.rectH = size;
    }

    public void setShowRectLimit(int sizex, int sizey)
    {
        this.rectW = sizex;
        this.rectH = sizey;
    }
    private void adjustLocalScale()
    {
        if (rectW == -1 && rectH == -1)
        {
            return;
        }
        float rectwidth = img.width;
        float rectheight = img.height;
        float scalex = rectW / rectwidth;
        //if (scalex > 1) scalex = 1;

        float scaley = rectH / rectheight;
        //if (scaley > 1) scaley = 1;

        float scale = Mathf.Min(scalex, scaley);

        transform.localScale = new Vector3(scale, scale, scale);
    }
    public void SetNormal()
    {
        Shader shader = Shader.Find("Unlit/Transparent Colored");
        gameObject.GetComponent<UITexture>().shader = shader;
    }
    public void sethui()
    {
        Shader shader = Shader.Find("Hidden/Unlit/Transparent Colored hui 2");
        gameObject.GetComponent<UITexture>().shader = shader;
    }

    void OnDestroy()
    {
        if (img != null)
        {
            //Destroy(img);
            img = null;
        }
        Resources.UnloadUnusedAssets();
    }
}
