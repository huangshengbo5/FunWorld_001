using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableItem : MonoBehaviour
{
    public GameObject unselectnode, selectnode;
    public UILabel unselectlabel, selectlabel;
    public GameObject unselecticon, selecticon;

    public void setselect(bool v)
    {
        selectnode.SetActive(v);
        unselectnode.SetActive(!v);
    }
    int index;
    public void settitle(string v, int i)
    {
        index = i;
        unselectlabel.text = v;
        selectlabel.text = v;

    }
}
