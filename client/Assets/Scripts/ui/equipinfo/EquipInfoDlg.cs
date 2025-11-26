using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInfoDlg : DialogMonoBehaviour
{
    public static EquipInfoDlg instance;
    public GameObject close;
    public EquipTypeNode typenode;
    public GameObject[] stanode = new GameObject[3];
    void Awake()
    {
        instance = this;
        setShowAnim(true);
        setClickOutZoneClose(false);
        UIEventListener.Get(close).onClick = closeDialog;
        SoundEventer.add_But_ClickSound(close);
    }
  
    int sta;
    public void settype(int type)
    {
        sta = type;
        GameObject tempstanode = null;
        for (int i = 0; i < stanode.Length; i++)
        {
            if (i == type)
            {
                stanode[i].SetActive(true);
                tempstanode = stanode[i];
            }
            else
            {
                stanode[i].SetActive(false);
            }
        }

        if (sta == 0)
        {
            tempstanode.GetComponent<EquipUpNode>().initlist();
        }else if (sta == 1)
        {
            tempstanode.GetComponent<EquipToPartNode>().initlist();
        }
        else if (sta == 2)
        {
            tempstanode.GetComponent<EquipStorageNode>().initlist();
        }
    }
}
