using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_science_levelBean{
		public int id;
		public int science_id;
		public int science_level;
		public string science_describe;
		public int target_type;
		public int target_value;
		public int effect_id;
		public int effect_value;
		public int cost_type;
		public int cost_sub_type;
		public int cost_val;

	public Reward upcost;
	public Reward getlvcost()
	{
		if (upcost == null)
		{
			upcost = new Reward(cost_type, cost_sub_type, cost_val);
		}
		return upcost;
	}

}
public class data_science_levelDef {
	public static int[] types;
	 public static List<data_science_levelBean> datas = new List<data_science_levelBean>();
	 public static Dictionary<int, List<data_science_levelBean>> dicdatas = new Dictionary<int, List<data_science_levelBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_science_level.bin";
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
				data_science_levelBean row=new data_science_levelBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.science_id = din.readInt();
				row.science_level = din.readInt();
				row.science_describe = din.readUTF();
              //  Debug.Log(row.science_describe);
				row.target_type = din.readInt();
				row.target_value = din.readInt();
				row.effect_id = din.readInt();
				row.effect_value = din.readInt();
				row.cost_type = din.readInt();
				row.cost_sub_type = din.readInt();
				row.cost_val = din.readInt();
              //  Debug.LogError(i + " " + row.cost_val);
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.science_id))
				{
					 List<data_science_levelBean> tempdatas = dicdatas[row.science_id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_science_levelBean> tempdatas = new List<data_science_levelBean>();
					tempdatas.Add(row);
					dicdatas.Add(row.science_id, tempdatas);
				}
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
