using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_alchemyBean{
		public int id;
		public int attr_min;
		public int attr_max;
		public int cd_times;
		public int cost_type;
		public int cost_sub_type;
		public int cost_val;
		public string rewards;

	}
public class data_alchemyDef {
	public static int[] types;
	 public static List<data_alchemyBean> datas = new List<data_alchemyBean>();
	 public static Dictionary<int, List<data_alchemyBean>> dicdatas = new Dictionary<int, List<data_alchemyBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_alchemy.bin";
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
				data_alchemyBean row=new data_alchemyBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.attr_min = din.readInt();
				row.attr_max = din.readInt();
				row.cd_times = din.readInt();
				row.cost_type = din.readInt();
				row.cost_sub_type = din.readInt();
				row.cost_val = din.readInt();
				row.rewards = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.attr_min))
				{
					 List<data_alchemyBean> tempdatas = dicdatas[row.attr_min];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_alchemyBean> tempdatas = new List<data_alchemyBean>();
					tempdatas.Add(row);
					dicdatas.Add(row.attr_min, tempdatas);
				}
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
