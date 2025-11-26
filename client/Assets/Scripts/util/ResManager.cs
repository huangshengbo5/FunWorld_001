using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;


public class ResManager : MonoBehaviour
{
 public  static string wwwsrcpath1 =
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        "file:///" + Application.streamingAssetsPath + "/";
#elif UNITY_ANDROID
        Application.streamingAssetsPath + "/";
#elif UNITY_IOS
        "file://"+Application.streamingAssetsPath + "/";
#endif



    public static string wwwsrcpath = wwwsrcpath1;

    static string platformpath =
#if UNITY_EDITOR||UNITY_STANDALONE_WIN 
        "win/"; 
#elif UNITY_ANDROID
        "android/";
#elif UNITY_IOS
    "ios/"; 
#endif

    static string srcpath =
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        Application.streamingAssetsPath + "/";
#elif UNITY_ANDROID
       Application.dataPath + "!assets/";
#elif UNITY_IOS
        "file://"+Application.streamingAssetsPath + "/";
#endif
    static string despath = Application.persistentDataPath + "/";
    public static IEnumerator savetosdcard(string filename)
    {

        WWW w = new WWW(wwwsrcpath1 + filename);
        yield return w;
        NGUITools.Save(filename, w.bytes);
    }

    static bool usedxtflag = false;
    public static Texture2D getTex(string filename)
    {
        Texture2D gobj = null;
        if (filename.EndsWith(".jpg"))
        {
            gobj = getTexJPG(filename);
        }
        else
        {
            gobj = getTex11111(filename);
        }
        return gobj;
    }

    private static WWW getwww(string wwwsrcpath, string filename)
    {
        WWW w = null;

        try
        {
	        w= new WWW(wwwsrcpath.Replace("\\","/") + filename);
	
	        while (!w.isDone)
	        {
	
	        }
        }
        catch (System.Exception ex)
        {
            w = null;
            Debug.Log("System.Exception");
        }
      //  Debug.Log(filename);
        if (w==null||(w!=null&&w.bytes.Length==0))
        {
            w = new WWW(wwwsrcpath1 + filename);

            while (!w.isDone)
            {

            }
        }

        return w;
    }

    public static Texture2D getTex999(string filename)
    {
        Texture2D gobj = null;
        // gobj = Instantiate(Resources.Load("Prefabs/umod")) as GameObject;
      //  Debug.Log("tex filename=" + filename);
        AssetBundle bundle = null;




        if (bundle == null)
        {
            if (File.Exists(despath + filename))
            {
                bundle = AssetBundle.LoadFromFile(despath + filename);
            }
            else if (File.Exists(srcpath + platformpath + filename))
            {

                bundle = AssetBundle.LoadFromFile(srcpath + platformpath + filename);
            }
        }

        if (bundle != null)
        {
            gobj = bundle.LoadAsset(filename) as Texture2D;
        }
        else
        {
            gobj = (Texture2D)Resources.Load(filename);
            //Debug.LogError(filename);
            //TextAsset asset = Resources.Load<TextAsset>(filename);
            //Texture2D dTexture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
            //dTexture.LoadImage(asset.bytes, true);
            //dTexture.wrapMode = TextureWrapMode.Clamp;
            //dTexture.filterMode = FilterMode.Trilinear;
            //gobj = dTexture;

        }

        if (gobj == null)
        {
            if (!filename.EndsWith(".png"))
            {
                filename = filename + ".png";
            }

            WWW w = getwww(wwwsrcpath, filename);

            Texture2D dTexture;
            if (usedxtflag)
            {
                dTexture = new Texture2D(5, 5, TextureFormat.DXT5, false);
                dTexture.LoadImage(w.bytes);
            }
            else
            {





            //     dTexture = w.textureNonReadable;



                dTexture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
                dTexture.LoadImage(w.bytes, true);


                dTexture.wrapMode = TextureWrapMode.Clamp;
                dTexture.filterMode = FilterMode.Trilinear;

                /*

                Color[] temppix = dTexture.GetPixels(1, 1, dTexture.width-2,dTexture.height-2);
                Color[] temppix2 = new Color[(dTexture.width - 2)*(dTexture.height - 2)];
                //Debug.Log("pixxx999&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&" + (CTTools.currentTimeMillis() - btime));
                //btime = CTTools.currentTimeMillis();

                for (int i = 0; i < temppix.Length; i++)
                {
                     temppix2[i] = new Color(temppix[i].r, temppix[i].g, temppix[i].b, 0);
                }

                //Debug.Log("pixxx111&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&" + (CTTools.currentTimeMillis() - btime));
                //btime = CTTools.currentTimeMillis();

                //Debug.Log(tex.name);
                //Debug.Log(temppix0.Length);

                int dstX=1,dstY=1;

                dTexture.SetPixels(dstX - 1, dstY, dTexture.width - 2, dTexture.height - 2, temppix2);
                dTexture.SetPixels(dstX + 1, dstY, dTexture.width - 2, dTexture.height - 2, temppix2);
                dTexture.SetPixels(dstX, dstY - 1, dTexture.width - 2, dTexture.height - 2, temppix2);
                dTexture.SetPixels(dstX, dstY + 1, dTexture.width - 2, dTexture.height - 2, temppix2);

                dTexture.SetPixels(dstX, dstY, dTexture.width - 2, dTexture.height - 2, temppix);


                //for (int i = 0; i < dTexture.width; i++)
                //{
                //    for (int j = 0; j < dTexture.height; j++)
                //    {
                //        int x = i;
                //        int y = j;
                //        Color ctemp = dTexture.GetPixel(x, y);

                //        Vector2[] wzzhou = new Vector2[4];
                //        wzzhou[0] = new Vector2(x + 1, y);
                //        wzzhou[1] = new Vector2(x - 1, y);
                //        wzzhou[2] = new Vector2(x, y - 1);
                //        wzzhou[3] = new Vector2(x, y + 1);

                //        if (ctemp.a > 0)
                //        {
                //            for (int k = 0; k < 4; k++)
                //            {
                //                Color czhou = dTexture.GetPixel((int)wzzhou[k].x, (int)wzzhou[k].y);
                //                if (czhou.a == 0)
                //                {
                //                    dTexture.SetPixel((int)wzzhou[k].x, (int)wzzhou[k].y, new Color(ctemp.r, ctemp.g, ctemp.b, 0));
                //                }
                //            }
                //        }
                //    }
                //}


                dTexture.Apply(false, true);*/
            }



           

            gobj = dTexture;
          
            w = null;

        }

        gobj.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
        // bundle.Unload(false);
        // bundle = null;
        return gobj;
    }
    static Color[,] xyarray;
    static int[,] pxy = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
   public static Texture2D getTex11(string filename)
    {
        if (!filename.EndsWith(".png"))
        {
            filename = filename + ".png";
        }

        byte[] bytes = NGUITools.Load(filename);
        if (bytes != null)
        {
            Texture2D tex = new Texture2D(4, 4,TextureFormat.DXT5, false);
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
            tex.LoadImage(bytes);
            bytes = null;
           // GC.Collect();
          
            return tex;
        }
        else
        {

            WWW w = getwww(wwwsrcpath, filename);
            Texture2D tex;
#if UNITY_IOS
                tex = new Texture2D(4, 4, TextureFormat.ARGB32, false);
                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
                tex.LoadImage(w.bytes,true);

#elif UNITY_ANDROID
            tex = new Texture2D(4, 4, TextureFormat.ETC2_RGBA1, false);
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
            tex.LoadImage(w.bytes, true);




        //    tex = w.textureNonReadable;
#else

            if (usedxtflag)
            {
                TextureFormat format = TextureFormat.DXT5;
                //  format= TextureFormat.PVRTC_RGBA2; 
                tex = new Texture2D(4, 4, format, false);
                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
                tex.LoadImage(w.bytes,true);
            }
            else
            {

             //   tex = w.textureNonReadable;
             //   tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");

                //tex = w.texture;
                //FixTransparency(tex);

                tex = new Texture2D(4, 4, TextureFormat.ARGB32, false);
                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
                tex.LoadImage(w.bytes, true);
            }

#endif

            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Trilinear;


            w = null;


            return tex;
        }
    }


    public static Texture2D getTex11ios(string filename)
    {
        if (!filename.EndsWith(".png"))
        {
            filename = filename + ".png";
        }

        byte[] bytes = NGUITools.Load(filename);
        if (bytes != null)
        {
            Texture2D tex = new Texture2D(5, 5, TextureFormat.DXT5, false);
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
            tex.LoadImage(bytes);
            bytes = null;
            // GC.Collect();
            return tex;
        }
        else
        {

            WWW w = getwww(wwwsrcpath, filename);
            Texture2D tex;

            tex = new Texture2D(4, 4, TextureFormat.PVRTC_RGBA4, false);
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
            tex.LoadImage(w.bytes, true);

            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Trilinear;

            w = null;


            return tex;
        }
    }

    public static Texture2D getTex12(string filename)
    {
        if (!filename.EndsWith(".png"))
        {
            filename = filename + ".png";
        }

        byte[] bytes = NGUITools.Load(filename);
        if (bytes != null)
        {
            Texture2D tex = new Texture2D(5, 5, TextureFormat.DXT5, false);
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
            tex.LoadImage(bytes);
            bytes = null;
          //  GC.Collect();
            return tex;
        }
        else
        {

            WWW w = getwww(wwwsrcpath, filename);
            Texture2D tex;
            if (usedxtflag)
            {
                tex = new Texture2D(5, 5, TextureFormat.DXT5, false);
                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
                tex.LoadImage(w.bytes);
            }
            else
            {

                tex = w.texture;
                tex.wrapMode = TextureWrapMode.Clamp;
                tex.filterMode = FilterMode.Trilinear;
                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");

                //tex = w.texture;
                //FixTransparency(tex);
            }

            w = null;

            tex = ResManager.ScaleTexture(tex, tex.width / 2, tex.height / 2);
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
            return tex;
        }
    }
    public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);

        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }

        result.Apply();
        DestroyImmediate(source);
        return result;
    }


    public static void FixTransparency(Texture2D texture)
    {
        Color32[] pixels = texture.GetPixels32();
        int w = texture.width;
        int h = texture.height;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                int idx = y * w + x;
                Color32 pixel = pixels[idx];
                if (pixel.a == 0)
                {
                    bool done = false;
                    if (!done && x > 0) done = TryAdjacent(ref pixel, pixels[idx - 1]);        // Left   pixel
                    if (!done && x < w - 1) done = TryAdjacent(ref pixel, pixels[idx + 1]);        // Right  pixel
                    if (!done && y > 0) done = TryAdjacent(ref pixel, pixels[idx - w]);        // Top    pixel
                    if (!done && y < h - 1) done = TryAdjacent(ref pixel, pixels[idx + w]);        // Bottom pixel
                    pixels[idx] = pixel;
                }
            }
        }

        texture.SetPixels32(pixels);
        texture.Apply();
    }

    //=========================================================================
    private static bool TryAdjacent(ref Color32 pixel, Color32 adjacent)
    {
        if (adjacent.a == 0) return false;

        pixel.r = adjacent.r;
        pixel.g = adjacent.g;
        pixel.b = adjacent.b;
        return true;
    }


    public static Texture2D getTex11111(string filename)
    {
        if (!filename.EndsWith(".png"))
        {
            filename = filename + ".png";
        }

        byte[] bytes = NGUITools.Load(filename);
        if (bytes != null)
        {
            Texture2D tex = new Texture2D(5, 5, TextureFormat.DXT5, false);
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
            tex.LoadImage(bytes);
     
            bytes = null;
          //  GC.Collect();
            return tex;
        }
        else
        {

            WWW w = getwww(wwwsrcpath, filename);

            Texture2D tex;
            if (usedxtflag)
            {
                tex = new Texture2D(5, 5, TextureFormat.DXT5, false);
                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
                tex.LoadImage(w.bytes);
            }
            else
            {

                tex = w.texture;

                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
            }

            w = null;
             tex.filterMode = FilterMode.Point;


            return tex;
        }
    }

    public static Texture2D getTexJPG(string filename)
    {
        if (!filename.EndsWith(".jpg"))
        {
            filename = filename + ".jpg";
        }

        byte[] bytes = NGUITools.Load(filename);
        if (bytes != null)
        {
            Texture2D tex = new Texture2D(5, 5, TextureFormat.ARGB32, false);
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".jpg", "");
            tex.LoadImage(bytes);
            bytes = null;
           // GC.Collect();
            return tex;
        }
        else
        {

            WWW w = getwww(wwwsrcpath, filename);

            Texture2D tex;

#if UNITY_IOS

                tex = new Texture2D(4, 4, TextureFormat.RGB24, false);
                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".jpg", "");
                tex.LoadImage(w.bytes,true);
#elif UNITY_ANDROID
            //    tex = w.textureNonReadable;
            tex = new Texture2D(5, 5, TextureFormat.ETC2_RGB, false);
             tex.LoadImage(w.bytes,true);           
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".jpg", "");

#else
            if (usedxtflag || true)
            {
                TextureFormat format = TextureFormat.DXT1;
                //  format = TextureFormat.PVRTC_RGB4;
                tex = new Texture2D(5, 5, format, false);
                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".jpg", "");
                tex.LoadImage(w.bytes,true);
            }
            else
            {
                tex = w.textureNonReadable;
                tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".jpg", "");
            }

#endif





            tex.wrapMode = TextureWrapMode.Clamp;
               tex.filterMode = FilterMode.Trilinear;
           // tex.filterMode = FilterMode.Point;


            //  tex.anisoLevel = 16;

            //for (int i = 0; i < tex.width; i++)
            //{
            //    for (int j = 0; j < tex.height; j++)
            //    {
            //        int x = i;
            //        int y = j;
            //        Color ctemp = tex.GetPixel(x, y);

            //        Vector2[] wzzhou = new Vector2[4];
            //        wzzhou[0] = new Vector2(x + 1, y);
            //        wzzhou[1] = new Vector2(x - 1, y);
            //        wzzhou[2] = new Vector2(x, y - 1);
            //        wzzhou[3] = new Vector2(x, y + 1);

            //        if (ctemp.a > 0)
            //        {
            //            for (int k = 0; k < 4; k++)
            //            {
            //                Color czhou = tex.GetPixel((int)wzzhou[k].x, (int)wzzhou[k].y);
            //                if (czhou.a == 0)
            //                {
            //                    tex.SetPixel((int)wzzhou[k].x, (int)wzzhou[k].y, new Color(ctemp.r, ctemp.g, ctemp.b, 0));
            //                }
            //            }
            //        }
            //    }
            //}

            //xyarray = new Color[tex.width, tex.height];


            //for (int i = 0; i < tex.width; i++)
            //{
            //    for (int j = 0; j < tex.height; j++)
            //    {
            //        xyarray[i,j]= tex.GetPixel(i, j);

            //    }
            //}

            //for(int k = 0; k < 4; k++)
            //{
            //    for (int i = 0; i < tex.width; i++)
            //    {
            //        for (int j = 0; j < tex.height; j++)
            //        {
            //            tex.SetPixel(i+pxy[k,0],j + pxy[k, 1], xyarray[i,j]);

            //        }
            //    }               
            //}



            //tex.Apply();

            //          tex.alphaIsTransparency = true;

            //Texture2D tex = new Texture2D(5, 5,TextureFormat.RGBA4444,false);
            //tex.LoadImage(w.bytes);

            // Debug.Log("==============================" + tex.alphaIsTransparency);
            //Texture2D tex = new Texture2D(5, 5, TextureFormat.ARGB32, false, true);
            //tex.filterMode = FilterMode.Point;
            //tex.Compress(true);

            //tex.LoadImage(w.bytes);

            //Debug.Log(tex);

            //tex.anisoLevel = 16;
            //tex.alphaIsTransparency = true;

            //w.LoadImageIntoTexture(tex);

            w = null;

            return tex;
        }

        //return (Texture2D)Resources.Load("ani/" + filename); 
    }

    public static Texture2D getTex22(string filename)
    {
        byte[] bytes = NGUITools.Load(filename + ".png");
        if (bytes != null)
        {
            Texture2D tex = new Texture2D(5, 5, TextureFormat.ARGB32, false);
            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");
            tex.LoadImage(bytes);
            bytes = null;
           // GC.Collect();
            return tex;
        }
        else
        {

            WWW w = getwww(wwwsrcpath, filename);

            Texture2D tex = w.textureNonReadable;

            tex.name = filename.Substring(filename.LastIndexOf("/") + 1).Replace(".png", "");


            //tex.wrapMode = TextureWrapMode.Clamp;
            //tex.filterMode = FilterMode.Bilinear;
            //tex.anisoLevel = 16;
            //tex.alphaIsTransparency = true;


            // Debug.Log("==============================" + tex.alphaIsTransparency);
            //             Texture2D tex = new Texture2D(5, 5, TextureFormat.ARGB32, false);
            //             tex.LoadImage(w.bytes);
            Debug.Log(tex);

            //tex.wrapMode = TextureWrapMode.Clamp;
            //tex.filterMode = FilterMode.Bilinear;
            //tex.anisoLevel = 16;
            //tex.alphaIsTransparency = true;

            //w.LoadImageIntoTexture(tex);


           // string path = AssetDatabase.GetAssetPath(tex);

           // TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

           //// textureImporter.maxTextureSize = size;

           // AssetDatabase.ImportAsset(path);

            w = null;

            return tex;
        }

        //return (Texture2D)Resources.Load("ani/" + filename); 
    }
    public static byte[] getBytes(string filename)
    {
        byte[] bytes = NGUITools.Load(filename);
        if (bytes != null)
        {
            return bytes;
        }
        else
        {
           // Debug.Log("zheili==" + wwwsrcpath + filename);

            WWW w = getwww(wwwsrcpath, filename);
            bytes = w.bytes;
            
            w = null;
            return bytes;
        }


        //{
        //    TextAsset asset = Resources.Load<TextAsset>("ani/"+filename);
        //    return  asset.bytes;
        //}
    }
    public static AudioClip getAC(string filename)
    {
        WWW w = getwww(wwwsrcpath, filename);
        AudioClip bytes = w.GetAudioClip();

        w = null;
        return bytes;
    }
    public static string getText(string filename)
    {
        Debug.Log("zheili==" + wwwsrcpath + filename);

        WWW w = getwww(wwwsrcpath, filename);

        Debug.Log("zheili==" + w.text.Length);
        return w.text;
    }

    public static string getLuaText(string filename)
    {
        if (luaoneflag)
        {
            return luapool[filename];
        }
        else
        {

            WWW w = getwww(wwwsrcpath, filename);

            return w.text;
        }


    }

    private static Dictionary<string, UnityEngine.Object> listTexture2d = new Dictionary<string, UnityEngine.Object>();

    public static GameObject getGameObject(string bname, string name,bool usebuffer=false)
    {

        if (listTexture2d.ContainsKey(name))
        {
            return Instantiate(listTexture2d[name]) as GameObject;
        }


        GameObject gobj = null;
        // gobj = Instantiate(Resources.Load("Prefabs/" + name)) as GameObject;

        AssetBundle bundle = null;

        if (bundleDictionary.ContainsKey(bname))
        {
            bundle = bundleDictionary[bname];
        }


        if (bundle == null)
        {
            //Debug.Log(despath + bname);
           // Debug.Log(File.Exists(srcpath + platformpath + bname) + " ===|" + srcpath + platformpath + bname);
            if (File.Exists(despath + bname))
            {
                bundle = AssetBundle.LoadFromFile(despath + bname);
                bundleDictionary.Add(bname, bundle);
            }
            else if (File.Exists(srcpath + platformpath + bname))
            {

                bundle = AssetBundle.LoadFromFile(srcpath + platformpath + bname);

                bundleDictionary.Add(bname, bundle);


            }
        }

        if (bundle != null)
        {
            UnityEngine.Object one = bundle.LoadAsset(name);
            if (one != null)
            {
                gobj = Instantiate(one) as GameObject;
            }else
            {
                gobj = Instantiate(Resources.Load("Prefabs/" + name)) as GameObject;
            }         
        }
        else
        {
        

            if (usebuffer)
            {
                UnityEngine.Object obj = Resources.Load("Prefabs/" + name);
                listTexture2d.Add(name, obj);
                gobj = Instantiate(obj) as GameObject;
            }
            else
            {
                UnityEngine.Object obj = Resources.Load("Prefabs/" + name);
                gobj = Instantiate(obj) as GameObject;
                obj = null;
            }


        }

        return gobj;
    }
    public static GameObject getGameObjectNoInit(string bname, string name)
    {

        if (listTexture2d.ContainsKey(name))
        {
            return (listTexture2d[name]) as GameObject;
        }


        GameObject gobj = null;
        // gobj = Instantiate(Resources.Load("Prefabs/" + name)) as GameObject;

        AssetBundle bundle = null;

        if (bundleDictionary.ContainsKey(bname))
        {
            bundle = bundleDictionary[bname];
        }


        if (bundle == null)
        {
            //Debug.Log(despath + bname);
            // Debug.Log(File.Exists(srcpath + platformpath + bname) + " ===|" + srcpath + platformpath + bname);
            if (File.Exists(despath + bname))
            {
                bundle = AssetBundle.LoadFromFile(despath + bname);
                bundleDictionary.Add(bname, bundle);
            }
            else if (File.Exists(srcpath + platformpath + bname))
            {

                bundle = AssetBundle.LoadFromFile(srcpath + platformpath + bname);

                bundleDictionary.Add(bname, bundle);


            }
        }

        if (bundle != null)
        {
            UnityEngine.Object one = bundle.LoadAsset(name);
            if (one != null)
            {
                gobj = (one) as GameObject;
            }
            else
            {
                gobj = (Resources.Load("Prefabs/" + name)) as GameObject;
            }
        }
        else
        {
            UnityEngine.Object obj = Resources.Load("Prefabs/" + name);

            listTexture2d.Add(name, obj);
            gobj = (obj) as GameObject;

        }

        return gobj;
    }
    public static Dictionary<string,AssetBundle> bundleDictionary = new Dictionary<string, AssetBundle>();
    public static AssetBundle allbundle=null;
    //public static IEnumerator LoadBundle(string name)
    //{
    //    bundle = new WWW(srcpath + name);        
    //    yield return bundle;
    //}

    public static void LoadBundle(string name)
    {
        allbundle = AssetBundle.LoadFromFile(wwwsrcpath + name);
    }

    //public static System.Reflection.Assembly assembly;
    //public static Component GetCompotent(GameObject gobj, string compotentName)
    //{

    //     //只有android进行dll热更处理  
    //    if (Application.platform == RuntimePlatform.Android|| Application.platform == RuntimePlatform.WindowsEditor) {  
    //        if (assembly != null) {
    //            Component com = gobj.GetComponent(compotentName);
    //            if (com != null)
    //            {
    //                Debug.Log("Destroy");
    //                Destroy(com);
    //            }
    //            Type type = assembly.GetType(compotentName);  
    //            if (gobj != null) {  
    //                Debug.Log ("---------->add momo suc  "+ compotentName+"  "+type.GetMethod("gettempstring"));
    //                com = gobj.AddComponent(type);

    //                System.Reflection.MethodInfo methodInfo = type.GetMethod("gettempstring");
    //                object[] obj = new object[] {};
    //                methodInfo.Invoke(com, obj); //反射调用DllManager  
    //            }  
    //            return com;  
    //        }  
    //    } else {  
    //        Component com = gobj.GetComponent(compotentName);   
    //        if(com == null){  
    //            Type type = Type.GetType(compotentName);
    //            gobj.AddComponent (type);  
    //        }  
    //        return com;  
    //    }  
    //    return null;  
    //}
    //public static void loadDllScript(string filename)
    //{
    //    AssetBundle bundle = null;
    //    if (bundle == null)
    //    {

    //        if (File.Exists(despath + filename))
    //        {
    //            bundle = AssetBundle.LoadFromFile(despath + filename);
    //        }
    //        else
    //        {
    //            Debug.Log(srcpath + platformpath + filename);
    //            bundle = AssetBundle.LoadFromFile(srcpath + platformpath + filename);
    //        }
    //    }
    //    TextAsset asset = bundle.LoadAsset("dllmen", typeof(TextAsset)) as TextAsset;
    //    assembly = System.Reflection.Assembly.Load(asset.bytes);


    //    Debug.Log(assembly);

    //    Type script2 = assembly.GetType("dlltest");
    //    object obj2 = Activator.CreateInstance(script2);
    //    System.Reflection.MethodInfo mInfo2 = script2.GetMethod("gettempstring");
    //    object result2 = mInfo2.Invoke(obj2, null);

    //}

    public static string[] assetfiles = {


    };

    public static APaintNodeSpine getspine(GameObject die, string atlasname, string jsonname)
    {
        GameObject texframeobjs = ResManager.getGameObject("allpre", "vapaintnodespine");
        texframeobjs.name = "yan";
        APaintNodeSpine temppaintnode = texframeobjs.GetComponent<APaintNodeSpine>();
        temppaintnode.create1nopatch(die, atlasname, jsonname);
        return temppaintnode;
    }

    public static void ScaleTextureFile(string dstPath, byte[] datas)
    {


#if UNITY_IOS
        if(SystemInfo.systemMemorySize>1500){
        return;
        }

#else
        return;
#endif

        if (!dstPath.EndsWith(".png"))
        {
            return;
        }

        if (dstPath.IndexOf("live2d/clothes")==-1
            && dstPath.IndexOf("live2d/model") == -1
            && dstPath.IndexOf("Expro") == -1
            )
        {
            return;
        }




        Texture2D tex = new Texture2D(5, 5, TextureFormat.ARGB32, false);
        tex.LoadImage(datas);

        if(dstPath.IndexOf("Expro") != -1)
        {
            tex = ScaleTexture(tex, tex.width / 2, tex.height / 2);
        }
        else
        if (tex.width == 2048&& tex.height == 2048)
        {
            tex = ScaleTexture(tex, 1024, 1024);
        }



        byte[] tempdatas = tex.EncodeToPNG();

        FileHandler file = new FileHandler(dstPath.Replace(".png","_s.png"));
        file.create();
        file.write(tempdatas);

        DestroyImmediate(tex);

        tempdatas = null;
        tex = null;

    }
    public static void CheckTextureFile(string dstPath, FileHandler fileh)
    {

#if UNITY_IOS
        if(SystemInfo.systemMemorySize>1500){
        return;
        }
#else
        return;
#endif

        if (!dstPath.EndsWith(".png"))
        {
            return;
        }

        if (dstPath.IndexOf("live2d/clothes") == -1 && dstPath.IndexOf("live2d/model") == -1)
        {
            return;
        }

        FileHandler file0 = new FileHandler(dstPath.Replace(".png", "_s.png"));
        if (file0.exist())
        {
            return ;
        }



        Texture2D tex = new Texture2D(5, 5, TextureFormat.ARGB32, false);
        tex.LoadImage(fileh.getData());

        if (tex.width == 2048)
        {
            tex = ScaleTexture(tex, 1024, 1024);
        }

        byte[] tempdatas = tex.EncodeToPNG();

        FileHandler file = new FileHandler(dstPath.Replace(".png", "_s.png"));
        file.create();
        file.write(tempdatas);

        DestroyImmediate(tex);

        tempdatas = null;
        tex = null;

    }
    
    public static void copyall()
    {

        if (true)
        {
            return;
        }

        for(int i = 0; i < assetfiles.Length; i++)
        {
            copy(assetfiles[i],"Win");
        }
        for (int i = 0; i < assetfiles.Length; i++)
        {
            copy(assetfiles[i],"iOS");
        }
        for (int i = 0; i < assetfiles.Length; i++)
        {
            copy(assetfiles[i], "Android");
        }
    }
    public static void copy(string fileName, string platformname)
    {
        string src = getStreamingPath_for_www() + fileName;
        string des = Application.persistentDataPath + "/"+platformname+"/" + fileName;
        //  Debug.Log("des:" + des);
        //  Debug.Log("src:" + src);
        if (File.Exists(des))
        {
            //   Debug.Log("exitreturn");
            return;
        }
        WWW www = new WWW(src);
        //yield return www;
        while (!www.isDone)
        {

        }


        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("www.error:" + www.error);
        }
        else
        {
            //des = Application.persistentDataPath + "/" + fileName;  
            if (File.Exists(des))
            {
                Debug.Log("exitdelete");
                File.Delete(des);
            }
            string dir = Path.GetDirectoryName(des);
            Directory.CreateDirectory(dir);

            FileStream fsDes = File.Create(des);
            fsDes.Write(www.bytes, 0, www.bytes.Length);
            fsDes.Flush();
            fsDes.Close();
        }
        www.Dispose();
    }
    /// <summary>  
    /// 将streaming path 下的文件copy到对应用  
    /// 为什么不直接用io函数拷贝，原因在于streaming目录不支持，  
    /// 不管理是用getStreamingPath_for_www，还是Application.streamingAssetsPath，  
    /// io方法都会说文件不存在  
    /// </summary>  
    /// <param name="fileName"></param>  
    public static void copy(string fileName)
    {
        string src = getStreamingPath_for_www() + fileName;
        string des = Application.persistentDataPath + "/" + fileName;
      //  Debug.Log("des:" + des);
      //  Debug.Log("src:" + src);
        if (File.Exists(des))
        {
         //   Debug.Log("exitreturn");
            return;
        }
        WWW www = new WWW(src);
        //yield return www;
        while (!www.isDone)
        {

        }


        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("www.error:" + www.error);
        }
        else
        {
            //des = Application.persistentDataPath + "/" + fileName;  
            if (File.Exists(des))
            {
                Debug.Log("exitdelete");
                File.Delete(des);
            }
            string dir = Path.GetDirectoryName(des);
            Directory.CreateDirectory(dir);

            FileStream fsDes = File.Create(des);
            fsDes.Write(www.bytes, 0, www.bytes.Length);
            fsDes.Flush();
            fsDes.Close();
        }
        www.Dispose();
    }
    static string getStreamingPath_for_www()
    {
        string pre = "file://";
#if UNITY_EDITOR
        pre = "file://";
#elif UNITY_ANDROID
        pre = "";  
#elif UNITY_IPHONE
        pre = "file://";  
#endif
        string path = pre + Application.streamingAssetsPath + "/";
        return path;
    }

   static string getPersistentPath_for_www()
    {
        string pre = "file://";
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        pre = "file:///";
#elif UNITY_ANDROID
        pre = "file://";  
#elif UNITY_IPHONE
        pre = "file://";  
#endif
        string path = pre + Application.persistentDataPath + "/";
        return path;
    }
    public static bool luaoneflag = false;
    private static Dictionary<string, string> luapool = new Dictionary<string, string>();
    internal static bool halftexflag;

    public static void readalllua()
    {
        if (!luaoneflag)
        {
            return;
        }

        if (luapool.Count > 0)
        {
            return;
        }
        byte[] bytes = ResManager.getBytes("luares/mylua");
        ByteArray din = new ByteArray(bytes);
        int num = din.readInt();
        for(int i = 0; i < num; i++)
        {
            string name = din.readUTF();
            int bytelenght = din.readInt();
            byte[] byteArray = din.readByteArray(bytelenght);
            string neirong = System.Text.Encoding.UTF8.GetString(byteArray);
            //   Debug.Log(name);
            //   Debug.Log(neirong);
            luapool.Add(name, neirong);
        }
    }



    public static Array CreateArrayInstance(Type itemType, int itemCount)
    {
        return Array.CreateInstance(itemType, itemCount);
    }

    public static IList CreateListInstance(Type itemType)
    {
        Type genericListType = typeof(List<>);
        Type concreteListType = genericListType.MakeGenericType(itemType);
        return (IList)Activator.CreateInstance(concreteListType);
    }

    public static IDictionary CreateDictionaryInstance(Type keyType, Type valueType)
    {
        Type genericListType = typeof(Dictionary<,>);
        Type concreteListType = genericListType.MakeGenericType(keyType, valueType);
        return (IDictionary)Activator.CreateInstance(concreteListType);
    }
}


