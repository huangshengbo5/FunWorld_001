using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEventer 
{
    public static void add_But_ClickSound(GameObject obj)
    {
        UIEventListener.Get(obj).onPress += onEvent_but_click;
    }

    private static void onEvent_but_click(GameObject go, bool state)
    {
        if (state)
        {
            SoundPlayer.getInstance().PlayW("Clik",false);
        }
    }
}
