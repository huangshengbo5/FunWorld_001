using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTypeNode : MonoBehaviour
{
    public UIGrid grid;
    public EquipTypeItem tab0;
    public List<EquipTypeItem> tablist = new List<EquipTypeItem>();
    public static string[] tabstr = { "宝物强化", "宝物分解", "宝物图鉴" };
    void Awake()
    {
        tab0.gameObject.SetActive(false);
        for (int i = 0; i < tabstr.Length; i++)
        {
            GameObject tabobj = NGUITools.AddChild(grid.gameObject, tab0.gameObject);
            tabobj.gameObject.SetActive(true);
            EquipTypeItem stt = tabobj.GetComponent<EquipTypeItem>();
            stt.settitle(tabstr[i], i);
            tablist.Add(stt);
            UIEventListener.Get(tablist[i].gameObject).onClick = onEvent_button;
            SoundEventer.add_But_ClickSound(tablist[i].gameObject);
        }
        settype(2);
    }
    private void onEvent_button(GameObject go)
    {
        for (int i = 0; i < tablist.Count; i++)
        {

            if (go == tablist[i].gameObject)
            {
                    settype(i);
            }
        }
    }
    public int type = -1;
    private void settype(int v)
    {
        if (type == v)
        {
            return;
        }
        type = v;

        for (int i = 0; i < tablist.Count; i++)
        {

            if (type == i)
            {
                tablist[i].setselect(true);
            }
            else
            {
                tablist[i].setselect(false);
            }
        }

        EquipInfoDlg.instance.settype(type);
        //   StorageDlg.instance.initlist(type);


    }
}
