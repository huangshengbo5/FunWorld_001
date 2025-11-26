using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//using WindowsForms;
//using System.Windows.Forms;
#endif

public class PopInput : MonoBehaviour
{
    public static PopInput Instance;

    void Awake()
    {
        Instance = this;
    }


    private bool isStatusShow = false;


    private TouchScreenKeyboard keyboard;

    private int maxLength = 10;

    private callbackObj callObj;
    public void showInput(string str, int maxLength, callbackObj callObj)
    {
        this.maxLength = maxLength;
        this.callObj = callObj;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //ProgramForm.showInput((object inputObj) =>
        //{
        //    Loom.RunAsync(() =>
        //    {
        //        Loom.QueueOnMainThread(() =>
        //        {
        //            callObj(inputObj);
        //        });
        //    });

        //}, str, maxLength);
#else
        isStatusShow = true;
        keyboard = TouchScreenKeyboard.Open(str, TouchScreenKeyboardType.Default, false, false, false, false);
#endif

    }



    void Update()
    {
        if (keyboard == null) return;

        if (keyboard.active)
        {
            //还没有完成
            if (keyboard.status!= TouchScreenKeyboard.Status.Done)
            {
                if (keyboard.text.Length > maxLength)
                {
                    keyboard.text = keyboard.text.Substring(0, maxLength);
                }
            }
        }

        if (isStatusShow)
        {
            //已经完成 或者 没激活状态
            if (keyboard.status == TouchScreenKeyboard.Status.Done || !keyboard.active)
            {
                if (callObj != null && keyboard.status != TouchScreenKeyboard.Status.Canceled)
                {
                    callObj(keyboard.text);
                }

                isStatusShow = false;
                keyboard = null;
            }
        }


    }
}
