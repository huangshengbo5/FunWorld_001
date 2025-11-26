using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_buildingBean{
		public int id;
		public int type;
		public string name;
		public string pic;
		public int condition_type;
		public int condition_val;

	}
public class data_buildingDef {
	public static int[] types;
	 public static List<data_buildingBean> datas = new List<data_buildingBean>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_building.bin";
		JavaReader jr = DefTools.getSdCardResourcedef(path);
		load(jr);
	}
	public static void load(JavaReader din)
		{
		datas.Clear();
		try{
			int len=din.readInt();
			types=new int[len];
			for (int i = 0; i < types.Length; i++) {
				types[i]=din.readByte();
			}
			int dataLen=din.readInt();
			for (int i = 0; i < dataLen; i++) {
				data_buildingBean row=new data_buildingBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.type = din.readInt();
				row.name = din.readUTF();
				row.pic = din.readUTF();
				row.condition_type = din.readInt();
				row.condition_val = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}

    public static data_buildingBean getdatabybuildid(int buildingID)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].id == buildingID)
            {
                return datas[i];
            }
        }
        return null;
    }
}
