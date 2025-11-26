/*
 * Emoji Wrapper @By Jumbo 2017/3/3
 * */

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class UIEmojiWrapper
{
    //使用的表情包
    private bool hasAtlas = false;
    private NGUIAtlas mAtlas;
    

    private Dictionary<string, string> emojiName = new Dictionary<string, string>();
    

    private static UIEmojiWrapper sInstance;
    public static UIEmojiWrapper Instance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new UIEmojiWrapper();
            }

            return sInstance;
        }
    }

    public void Init(NGUIAtlas atlas)
    {
        if (!hasAtlas)
        {
            if (atlas == null)
            {
                atlas = Resources.Load("Atlas/font/Emoticon_emojia", typeof(NGUIAtlas)) as NGUIAtlas;
            }

            if (atlas == null)
            {
                Debug.LogError("[UIEmojiWrapper Atlas is null]");
                return;
            }
            mAtlas = atlas;
            //预分配
            for (int i = 0, cnt = mAtlas.spriteList.Count; i < cnt; i++)
            {
                emojiName.Add("@" + mAtlas.spriteList[i].name, mAtlas.spriteList[i].name);
            }

            hasAtlas = true;
        }

    }


    public string GetEmoji(string key)
    {
        string em;

        if (emojiName.TryGetValue(key, out em))
        {
            return em;
        }

        return null;
    }
    public bool HasEmoji(string key, bool emojiEnable, bool currencyEnable)
    {
        //是否包含该表情
        bool isCont = emojiName.ContainsKey(key);

        //根本不存在这个表情
        if (isCont == false) return false;

        if (emojiEnable && currencyEnable) return true;

        bool isCurrency = isHaveCurrencyEmoji(key);


        if (isCurrency && currencyEnable)
        {
            return true;
        }else if(emojiEnable && !currencyEnable && !isCurrency)
        {
            return true;
        }
        return false;
    }


    public Dictionary<string, string> getEmojis()
    {
        return emojiName;
    }







    public static string ReplaceEmoji(string source)
    {

        //source = source.Replace(Lan.Emojistrs[0], "@zs");
        //source = source.Replace(Lan.Emojistrs[1], "@jb");
        //source = source.Replace(Lan.Emojistrs[2], "@hg");
        //source = source.Replace(Lan.Emojistrs[3], "@js");
        //source = source.Replace(Lan.Emojistrs[4], "@yh");
        //source = source.Replace(Lan.Emojistrs[5], "@xz");
        //source = source.Replace(Lan.Emojistrs[6], "@jz");

        return source;

    }

    public bool isHaveCurrencyEmoji(string key)
    {
        //if (key.Equals("@zs"))
        //{
        //    return true;
        //}else if (key.Equals("@fz"))
        //{
        //    return true;
        //}
        //else if (key.Equals("@jb"))
        //{
        //    return true;
        //}
        //else if (key.Equals("@hg"))
        //{
        //    return true;
        //}
        //else if (key.Equals("@js"))
        //{
        //    return true;
        //}
        //else if (key.Equals("@yh"))
        //{
        //    return true;
        //}
        //else if (key.Equals("@xz"))
        //{
        //    return true;
        //}
        //else if (key.Equals("@jz"))
        //{
        //    return true;
        //}
        return false;
    }
}
