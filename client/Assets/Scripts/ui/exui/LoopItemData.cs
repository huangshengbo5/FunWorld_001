/*
 * 描术：
 * 
 * 作者：AnYuanLzh
 * 公司：lexun
 * 时间：2014-xx-xx
 */
using UnityEngine;
using System.Collections;

/// <summary>
/// 与item对关联的数据类，具体的item的数据类一定继承它
/// </summary>
public class LoopItemData
{
    // ***

    public object userData;

    public int id = -1;
    public int type = -1;

    public void setId(int id)
    {
        this.id = id;
    }

    public void setType(int type)
    {
        this.type = type;
    }

}
