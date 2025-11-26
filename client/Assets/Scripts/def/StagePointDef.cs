using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StagePointBean
{
    public int id;
    public int x;
    public int y;
    public List<int> aiidlist = new List<int>();


    public bool lockflag = true;

}
public class StagePointDef 
{
    public static int[] types;
    public static List<StagePointBean> datas = new List<StagePointBean>();
    public static void load()
    {
        string path = CTGlobal.defRoot + "stagepos.bin";
        JavaReader jr = DefTools.getSdCardResourcedef(path);
        load(jr);
    }
    public static void load(JavaReader din)
    {
        datas.Clear();
        try
        {
            int len = din.readInt();
          //  Debug.LogError(len);
            types = new int[len];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = din.readByte();
            }
            int dataLen = din.readInt();
            for (int i = 0; i < dataLen; i++)
            {
                StagePointBean row = new StagePointBean();
                din.resetReadTimes();
                row.id = din.readInt();
                row.x = din.readInt();
                row.y = din.readInt();
                string name = din.readUTF();
              //  Debug.LogError(row.x + " " + name);
                string[] splitstr = name.Split(',');
                for(int j = 0; j < splitstr.Length; j++)
                {
                    row.aiidlist.Add(int.Parse(splitstr[j]));
                }
                DefTools.skipNewValue(din, types);
                datas.Add(row);
            }
            din.close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }

    }
}
