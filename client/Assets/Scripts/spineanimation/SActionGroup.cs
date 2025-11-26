using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;

public class SActionGroup {

    //public static Texture2D img = null;
    //public static AttachmentLoader attachmentLoader;
    private static List<string> beanKeys = new List<string>();
    private static List<SpineStreamAtlas> beanBuffers = new List<SpineStreamAtlas>();

    public static List<string> agKeys = new List<string>();
    public static List<SActionGroup> agBuffers = new List<SActionGroup>();


    public List<Attachment> cailist = new List<Attachment>();

    public List<Attachment> daolist = new List<Attachment>();

    public SpineStreamAtlas loaderbean;

    public SkeletonDataAsset skeletonDataAsset;

    TextAsset testasset;
    public SActionGroup(string attachname,string modelname)
    {




        loaderbean = getLoadBeanFromBuffer(attachname);

        string jsonbyte = ResManager.getText("Export/" + modelname + ".json");
        testasset = new TextAsset(jsonbyte);

        // SkeletonData loadedSkeletonData = SkeletonDataAsset.ReadSkeletonData(jsonbyte, loaderbean.attachmentLoader, 1.0f);
        skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(testasset, loaderbean.atlasAsset, true, 1.0f);
        jsonbyte = null;




    }
    public SkeletonDataAsset skeletonDataAssetInstance;
    public SkeletonDataAsset getSkeletonDataAssetInstance()
    {
        if(skeletonDataAssetInstance==null)
        skeletonDataAssetInstance = SkeletonDataAsset.CreateRuntimeInstance(testasset, loaderbean.getatlasAsset(), true, 1.0f);
        return skeletonDataAssetInstance;
    }
    public SkeletonDataAsset getSkeletonDataAsset()
    {
        skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(testasset, loaderbean.getatlasAsset(), true, 1.0f);
        return skeletonDataAsset;
    }
    public SkeletonDataAsset getSkeletonDataAsset(Material mat)
    {
        skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(testasset, loaderbean.getatlasAsset(mat), true, 1.0f);
        return skeletonDataAsset;
    }

    public Attachment getdaobyname(string name)
    {
        for(int i = 0; i < daolist.Count; i++)
        {
            if (daolist[i].Name.IndexOf(name)!=-1)
            {
                return daolist[i];
            }
        }
        return null;
    }

    public static SActionGroup getAGFromBuffer(string attachname, string modelname)
    {
        int targetIndex = -1;
        for (int i = 0; i < agKeys.Count; i++)
        {
            if ((agKeys[i]).Equals(attachname+","+modelname))
            {
                targetIndex = i;
                break;
            }
        }
        if (targetIndex != -1)
        {
            return agBuffers[targetIndex];
        }
        else
        {
            SActionGroup ag = new SActionGroup(attachname,modelname);

            agKeys.Add(attachname + "," + modelname);
            agBuffers.Add(ag);

            return ag;
        }
    }

    public static void delAGFromBuffer(string attachname, string modelname)
    {
        int targetIndex = -1;
        for (int i = 0; i < agKeys.Count; i++)
        {
            if ((agKeys[i]).Equals(attachname + "," + modelname))
            {
                targetIndex = i;
                break;
            }
        }
        if (targetIndex != -1)
        {
            SActionGroup ag = agBuffers[targetIndex];
            delLoadBeanFromBuffer(attachname);
            ag.skeletonDataAsset = null;
            agBuffers.RemoveAt(targetIndex);
            agKeys.RemoveAt(targetIndex);
        }

    }
    public static void delAGFromBuffer(string attachname)
    {

        for (int i = 0; i < agKeys.Count; i++)
        {
            if ((agKeys[i]).StartsWith(attachname))
            {
                SActionGroup ag = agBuffers[i];
                delLoadBeanFromBuffer(attachname);
                ag.skeletonDataAsset = null;
                agBuffers.RemoveAt(i);
                agKeys.RemoveAt(i);

                i--;
            }
        }


    }

    public static SpineStreamAtlas getLoadBeanFromBuffer(string attachname)
    {
        int targetIndex = -1;
        for (int i = 0; i < beanKeys.Count; i++)
        {
            if ((beanKeys[i]).Equals(attachname))
            {
                targetIndex = i;
                break;
            }
        }
        if (targetIndex != -1)
        {
          //  textureSums[targetIndex] = textureSums[targetIndex] + 1;
          //  loadedIndex = targetIndex;
            return beanBuffers[targetIndex];
        }
        else
        {
            SpineStreamAtlas bean = new SpineStreamAtlas(attachname);

            beanKeys.Add(attachname);
            beanBuffers.Add(bean);
          //  textureSums.Add(1);
          //  loadedIndex = -1;
            return bean;
        }
    }

    public static void delLoadBeanFromBuffer(string attachname)
    {
        int targetIndex = -1;
        for (int i = 0; i < beanKeys.Count; i++)
        {
            if ((beanKeys[i]).Equals(attachname))
            {
                targetIndex = i;
                break;
            }
        }
        if (targetIndex != -1)
        {
            beanBuffers[targetIndex].clear();
            beanBuffers.RemoveAt(targetIndex);
            beanKeys.RemoveAt(targetIndex);
        }

    }


}
