using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_skill_levelBean{
		public int id;
		public int skill_id;
		public int level;
		public string describe;
		public int trigger_type;
		public int seq;
		public bool must_hit;
		public bool can_skip;
		public int probability;
		public int target;
		public int effect_id;
		public int effect_probability;
		public int effect_val1;
		public int effect_val2;

	}
public class data_skill_levelDef {
	public static int[] types;
	 public static List<data_skill_levelBean> datas = new List<data_skill_levelBean>();
	 public static Dictionary<int, List<data_skill_levelBean>> dicdatas = new Dictionary<int, List<data_skill_levelBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_skill_level.bin";
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
				data_skill_levelBean row=new data_skill_levelBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.skill_id = din.readInt();
				row.level = din.readInt();
				row.describe = din.readUTF();
				row.trigger_type = din.readInt();
				row.seq = din.readInt();
				row.must_hit = din.readBoolean();
				row.can_skip = din.readBoolean();
				row.probability = din.readInt();
				row.target = din.readInt();
				row.effect_id = din.readInt();
				row.effect_probability = din.readInt();
				row.effect_val1 = din.readInt();
				row.effect_val2 = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.skill_id))
				{
					 List<data_skill_levelBean> tempdatas = dicdatas[row.skill_id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_skill_levelBean> tempdatas = new List<data_skill_levelBean>();
					tempdatas.Add(row);
					dicdatas.Add(row.skill_id, tempdatas);
				}
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
