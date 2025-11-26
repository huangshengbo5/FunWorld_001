
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;


public class EmoijData
{
    public int pos;
    public string emoji;

    public EmoijData(int p, string s)
    {
        this.pos = p;
        this.emoji = s;
    }
}

public class UIEmojiLabel : UILabel
{



    //是否开启货币表情
    public bool currency_Enable = false;//货币表情
    public bool emoji_Enable = true;    //emoji表情的开启


    private char emSpace = '\u2001';

    public NGUIAtlas altas;


    public string emojiText = null;
    private bool isRefreshEmoji = false;

    public void setText(string str, bool emojiEnable, bool currencyEnable)
    {
        if (emojiText == str) return;

        str = UTools.removeNguiSymbolsEffect(str, true);

        this.emoji_Enable = emojiEnable;
        this.currency_Enable = currencyEnable;


        emojiText = str;
        isRefreshEmoji = false;


        if (emoji_Enable || currency_Enable)
        {
            UIEmojiWrapper.Instance.Init(altas);
            checkEmojiInfo(str);
        }
        else
        {
            this.text = str;
        }

    }


    //自动转码

    public override void OnFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
    {
        base.OnFill(verts, uvs, cols);

        refreshEmoji();
    }

    private void refreshEmoji()
    {
        if (!isRefreshEmoji)
        {
            StartCoroutine(parseEmoji());
            isRefreshEmoji = true;
        }
    }


    private List<EmoijData> emojiReplacements = new List<EmoijData>();
    private void checkEmojiInfo(string content)
    {
        emojiReplacements.Clear();

        int emojiIndex = 0;

        string inputString = NGUIText.StripSymbols(content);


        //先筛选出有几个表情
        int i = 0;

        List<bool> emjioIndex = new List<bool>();
        while (i < content.Length)
        {
            string singleChar = content.Substring(i, 1);

            if (singleChar.Equals("@") && i + 2 < content.Length)
            {
                string key = content.Substring(i, 3);

                if (UIEmojiWrapper.Instance.HasEmoji(key, emoji_Enable, currency_Enable))
                {
                    emjioIndex.Add(true);
                }
                else
                {
                    emjioIndex.Add(false);
                }
            }
            i++;
        }


        int spaceCount = 0;

        i = 0;
        int atIndex = 0;
        while (i < inputString.Length)
        {
            string singleChar = inputString.Substring(i, 1);

            if (singleChar.Equals(" ")) spaceCount++;//空格不会增加定点

            if (singleChar.Equals("@") && i + 2 < inputString.Length)
            {
                string key = inputString.Substring(i, 3);

                bool isEmoji = emjioIndex.Count > atIndex && emjioIndex[atIndex] ? true : false;

                if (isEmoji && UIEmojiWrapper.Instance.HasEmoji(key, emoji_Enable, currency_Enable))
                {
                    emojiIndex++;
                    emojiReplacements.Add(new EmoijData(emojiIndex - 1 - spaceCount, key));
                    i += 3;
                }
                else
                {
                    emojiIndex++;
                    i++;
                }

                atIndex++;
            }
            else
            {
                emojiIndex++;
                i++;
            }
        }

        i = 0;
        StringBuilder sb = new StringBuilder();
        while (i < content.Length)
        {
            string singleChar = content.Substring(i, 1);

            if (singleChar.Equals("@") && i + 2 < content.Length)
            {
                string key = content.Substring(i, 3);

                if (UIEmojiWrapper.Instance.HasEmoji(key, emoji_Enable, currency_Enable))
                {
                    sb.Append(emSpace);
                    i += 3;
                }
                else
                {
                    sb.Append(content[i]);
                    i++;
                }
            }
            else
            {
                sb.Append(content[i]);
                i++;
            }
        }

        this.text = sb.ToString();
    }

    public IEnumerator parseEmoji()
    {
        yield return new WaitForEndOfFrame();
        //先回收
        clearEmoji();
        for (int j = 0; j < emojiReplacements.Count; j++)
        {
            addEmoji(emojiReplacements[j]);
        }
    }



    private GameObject obj_Emoji;
    public void addEmoji(EmoijData tuple)
    {
        if (obj_Emoji == null)
        {
            obj_Emoji = new GameObject();
            obj_Emoji = UTools.setPresent(gameObject, obj_Emoji);
            obj_Emoji.name = "Emoji";
        }

        if (tuple != null)
        {
            int emojiIndex = tuple.pos;
            string emojiName = tuple.emoji;

            GameObject sprite_emoji = ResManager.getGameObjectNoInit("allpre", "ui/exui/Label_Emoji");

            if (sprite_emoji != null)
            {
                int fontIndex = emojiIndex * 4;

                if (fontIndex >= this.geometry.verts.Count) return;

                GameObject newObj = NGUITools.AddChild(obj_Emoji, sprite_emoji.gameObject);
                UISprite spt = newObj.GetComponent<UISprite>();

                string emoji = UIEmojiWrapper.Instance.GetEmoji(emojiName);
                if (!string.IsNullOrEmpty(emoji))
                {
                    spt.name = emoji;
                    spt.spriteName = emoji;
                }
                spt.hideIfOffScreen = false;
                spt.depth = this.depth;

                spt.MakePixelPerfect();


                float goalScaleX = (float)(this.fontSize + 2) / spt.width;
                float goalScaleY = (float)(this.fontSize + 2) / spt.height;

                float goalScale = Mathf.Min(goalScaleX, goalScaleY);

                spt.width = (int)(spt.width * goalScale);
                spt.height = (int)(spt.height * goalScale);

                spt.transform.parent = this.obj_Emoji.transform;
                spt.transform.localScale = Vector3.one;

                spt.transform.localPosition = new Vector3(this.geometry.verts[fontIndex].x + fontSize / 2 + 1, this.geometry.verts[fontIndex].y + spt.height / 2 - 7 * ((float)fontSize / 65));
            }


        }

    }

    private void clearEmoji()
    {
        if (obj_Emoji != null)
        {
            MonoBehaviour.DestroyImmediate(obj_Emoji, true);
        }
    }




    void OnDestroy()
    {
        clearEmoji();
    }







}
