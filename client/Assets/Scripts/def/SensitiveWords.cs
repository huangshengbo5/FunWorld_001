using UnityEngine;
using System;
using System.Collections.Generic;


public class SensitiveWordsBean
{
    public string Word;
    public int Lv;
}


public class SensitiveWords
{

    public static int[] types;
    public static List<SensitiveWordsBean> datas = new List<SensitiveWordsBean>();

    public static void load()
    {

        JavaReader jr = DefTools.getSdCardResourcedef(CTGlobal.defRoot + "SensitiveWords.bin");
        load(jr);
    }

    public static List<SensitiveWordsBean> load(JavaReader din)
    {
        datas.Clear();
#if TW
        return SensitiveWords.datas;
#endif



        try
        {
            int len = din.readInt();
            types = new int[len];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = din.readByte();
            }
            int dataLen = din.readInt();
            datas.Clear();
            for (int i = 0; i < dataLen; i++)
            {
                SensitiveWordsBean row = new SensitiveWordsBean();
                din.resetReadTimes();



                row.Word = din.readUTF();
                row.Lv = din.readInt();
              // Debug.LogError(row.Word);

                DefTools.skipNewValue(din, types);
                datas.Add(row);
            }
        //    Debug.LogError(dataLen);
            din.close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
        return SensitiveWords.datas;
    }

    // 是否包含敏感词
    public static bool containIllegalWord(string content)
    {
        if (content == null) return false;

        content = content.Replace("*", "").Replace("&", "").Replace("-", "").Replace("+", "").Replace("=", "").Replace(" ", "").Replace("　", "")
        .Replace("#", "").Replace("~", "").Replace(",", "").Replace("%", "").Replace("，", "").Replace("。", "")
        .Replace("（", "").Replace("）", "").Replace("(", "").Replace(")", "").Replace(":", "").Replace("：", "")
        .Replace("O", "0").Replace("o", "0").Replace("⑩", "0").Replace("①", "1").Replace("②", "2").Replace("③", "3").Replace("④", "4")
        .Replace("⑤", "5").Replace("⑥", "6").Replace("⑦", "7").Replace("⑧", "8").Replace("⑨", "9")
        .Replace("㈩", "0").Replace("㈠", "1").Replace("㈡", "2").Replace("㈢", "3").Replace("㈣", "4")
        .Replace("㈤", "5").Replace("㈥", "6").Replace("㈦", "7").Replace("㈧", "8").Replace("㈨", "9")
        .Replace("⑽", "0").Replace("⑴", "1").Replace("⑵", "2").Replace("⑶", "3").Replace("⑷", "4")
        .Replace("⑸", "5").Replace("⑹", "6").Replace("⑺", "7").Replace("⑻", "8").Replace("⑼", "9")
        .Replace("⒑", "0").Replace("⒈", "1").Replace("⒉", "2").Replace("⒊", "3").Replace("⒋", "4")
        .Replace("⒌", "5").Replace("⒍", "6").Replace("⒎", "7").Replace("⒏", "8").Replace("⒐", "9")
        .Replace("０", "0").Replace("１", "1").Replace("２", "2").Replace("３", "3").Replace("４", "4")
        .Replace("５", "5").Replace("６", "6").Replace("７", "7").Replace("８", "8").Replace("９", "9")
        //.Replace("零", "0").Replace("一", "1").Replace("二", "2").Replace("三", "3").Replace("四", "4")
        //.Replace("五", "5").Replace("六", "6").Replace("七", "7").Replace("八", "8").Replace("九", "9")
        //.Replace("零", "0").Replace("壹", "1").Replace("贰", "2").Replace("叁", "3").Replace("肆", "4")
        //.Replace("伍", "5").Replace("陆", "6").Replace("柒", "7").Replace("捌", "8").Replace("玖", "9")
       // .Replace("零", "0")
        .Replace("⒈", "1").Replace("⒉", "2").Replace("⒊", "3").Replace("⒋", "4")
        .Replace("⒌", "5").Replace("⒍", "6").Replace("⒎", "7").Replace("⒏", "8").Replace("⒐", "9");

        /*if (content.Contains("#") || content.Contains("%") || content.Contains("@") || content.Contains("*"))
        {//@#*
            return false;
        }*/
        for (int i = 0; i < datas.Count; i++)
        {
            if (content.Contains(datas[i].Word))
            {
                return true;
            }
        }
        return false;
    }

    public static bool containIllegalWord2(string content)
    {
        if (content == null) return false;

        content = content.Replace(" ", "").Replace("&", "");

        /*if (content.Contains("#") || content.Contains("%") || content.Contains("@") || content.Contains("*"))
        {//@#*
            return false;
        }*/
        for (int i = 0; i < datas.Count; i++)
        {
            if (content.Contains(datas[i].Word))
            {
                return true;
            }
        }
        return false;
    }

    // 是否包含敏感词
    public static bool containIllegalWord(string[] content)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            for (int j = 0; j < content.Length; j++)
            {
                if (content[j].Contains(datas[i].Word))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static string replaceIllegalWord(String content)
    {
        //		long start=System.currentTimeMillis();

      //  content = content.Replace(" ", "").Replace("&", "");

        for (int i = 0; i < datas.Count; i++)
        {
            if (content.Contains(datas[i].Word))
            {
                content = content.Replace(datas[i].Word, "***");
            }
        }
        //		System.out.println("check spend "+(System.currentTimeMillis()-start));
        return content;
    }


    public static string replaceIllegalWord2Level(String content)
    {
        //		long start=System.currentTimeMillis();

#if TW
        return content;
#endif

        if (!containIllegalWord(content))
        {
            return content;
        }

        content = content.Replace(" ", "").Replace("&", "");

        for (int i = 0; i < datas.Count; i++)
        {
            if (content.Contains(datas[i].Word))
            {
                content = content.Replace(datas[i].Word, "***");
            }
        }
        //		System.out.println("check spend "+(System.currentTimeMillis()-start));
        return content;
    }


}
