using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;
using System.IO;

public class SpineStreamAtlas {
    public Texture2D img = null;
    public SpineAtlasAsset atlasAsset;
    public string name;
    TextAsset testasset;
    //    private string attachname;

    public SpineStreamAtlas(string attachname)
    {
        this.name = attachname;
        string atlasstring = ResManager.getText("Export/"+name+".atlas");
        img = ResManager.getTex11("Export/" + name + ".png");
        Shader shader = Shader.Find("Spine/Skeleton");
        Material mat = new Material(shader);
        mat.mainTexture = img;
        testasset = new TextAsset(atlasstring);

        mat.name = "tttt0";

        atlasAsset = SpineAtlasAsset.CreateRuntimeInstance(testasset, new Material[] { mat }, true);

    }

    public SpineAtlasAsset getatlasAsset()
    {
        Shader shader = Shader.Find("Spine/Skeleton");
        Material mat = new Material(shader);
        mat.mainTexture = img;
        return SpineAtlasAsset.CreateRuntimeInstance(testasset, new Material[] { mat }, true);
    }
    public SpineAtlasAsset getatlasAsset(Material mat)
    {
        mat.mainTexture = img;
        return SpineAtlasAsset.CreateRuntimeInstance(testasset, new Material[] { mat }, true);
    }
    public Material getguangmat()
    {
        Shader shader = Shader.Find("Spine/OutLine/Skeleton");
        Material mat = new Material(shader);
        mat.mainTexture = img;
        return mat;
    }

    internal void clear()
    {
        GameObject.Destroy(img);
        atlasAsset = null;
    }
}
