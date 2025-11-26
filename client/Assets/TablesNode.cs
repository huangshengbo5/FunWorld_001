using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablesNode : MonoBehaviour
{
    public UIGrid grid;
    public TableItem tab0;
    public List<TableItem> tablist = new List<TableItem>();
    public Action<int> onTableActive;
    public Action<int> onTableUnActive;
    void Awake()
    {
        tab0.gameObject.SetActive(false);
    }
    Func<int, bool> checkTable;
    public void InitTables(string[] tableNames, Action<int> onTableActiveCall, Action<int> onTableUnActiveCall,Func<int,bool> _checkTable =null)
    {
        for (int i = 0; i < tableNames.Length; i++)
        {
            GameObject tabobj = NGUITools.AddChild(grid.gameObject, tab0.gameObject);
            tabobj.gameObject.SetActive(true);
            TableItem stt = tabobj.GetComponent<TableItem>();
            stt.settitle(tableNames[i], i);
            tablist.Add(stt);
            UIEventListener.Get(tablist[i].gameObject).onClick = onEvent_button;
            SoundEventer.add_But_ClickSound(tablist[i].gameObject);
        }
        if (_checkTable != null)
        {
            checkTable = _checkTable;
        }
        if (onTableActiveCall != null)
        {
            onTableActive = onTableActiveCall;
        }
        if (onTableUnActiveCall != null)
        {
            onTableUnActive = onTableUnActiveCall;
        }

        settype(0);
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
        if (checkTable!= null && !checkTable(v))
        {
            return;
        }
        if (type != v)
        {
            if (type != -1)
            {
                tablist[type].setselect(false);
                onTableUnActive(type);
            }
            type = v;
            tablist[type].setselect(true);
            onTableActive(type);
        }
    }
}
