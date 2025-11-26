using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//基础Loop数据
public class BasePoolDat
{
    public DialogMonoBehaviour dialogMono;

    public void setDialogMonoBehaviour(DialogMonoBehaviour mono)
    {
        this.dialogMono = mono;
    }
}

//Dialog池
public class DialogPool {

    

    private static List<BasePoolDat> dialogLoop = new List<BasePoolDat>();

    //增加dialog数据
    public static void addDialog(BasePoolDat dialog)
    {
        dialogLoop.Add(dialog);
    
       /* if(dialogLoop.Count == 1)
        {*/


            showRemainDialog();
        /*}*/
    }

    //移除显示的dialog
    public static void removeDialog(BasePoolDat dialog)
    {
        dialogLoop.Remove(dialog);
        UIViewGroup.removeNotification(dialog.dialogMono);
        showRemainDialog();
    }
    
    //显示dialog
    private static void showRemainDialog()
    {
        for (int i = 0; i < dialogLoop.Count; i++)
        {
            if (dialogLoop[i].dialogMono != null && dialogLoop[i].dialogMono.gameObject != null && dialogLoop[i].dialogMono.gameObject.activeSelf)
            {
                return;
            }
        }

        if (dialogLoop.Count > 0)
        {
            BasePoolDat dat = dialogLoop[0];

            if(dat is TipPoolDat)
            {
                UIManager.showTip((TipPoolDat)dat);
            }
            //else if(dat is RewardDialogLoopDat)
            //{
            //    UIManager.showReward((RewardDialogLoopDat)dat);
            //}
            //else if(dat is MenuPoolDat)
            //    {
            //        UIHelp.showMenuUpgrade((MenuPoolDat)dat);
            //    }else if(dat is LevelUpPoolDat)
            //    {
            //        UIHelp.showLevelUp((LevelUpPoolDat)dat);
            //    }else if(dat is ThingUnlockDialogLoopDat)
            //    {
            //        UIHelp.showUnlockThing((ThingUnlockDialogLoopDat)dat);
            //    }
        }
    }

    //清理所有的Dialog
    public static void clearAll()
    {
        //for(int i = 0; i < dialogLoop.Count; i++)
        //{
        //    //如果显示中。 则移除
        //    if(dialogLoop[i].dialogMono != null && dialogLoop[i].dialogMono.gameObject != null && dialogLoop[i].dialogMono.gameObject.activeSelf)
        //    {
        //        UIHelp.removeNotification(dialogLoop[i].dialogMono);
        //    }
        //}
        dialogLoop.Clear();
    }

    public static void clearPool()
    {
        dialogLoop.Clear();
    }

}
