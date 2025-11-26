using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_equip_skillBean{
		public int id;
		public int trigger_type;
		public int trigger_condition;
		public int condition_val;
		public int probability;
		public int target;
		public int effect_id;
		public int trigger_limited;
		public int effect1_lower;
		public int effect1_upper;
		public int effect2_lower;
		public int effect2_upper;
		public int effect3_lower;
		public int effect3_upper;
		public string desc;
		public int treasurename_picture;
		public string special_name;

	}
public class data_equip_skillDef {
	public static int[] types;
	 public static List<data_equip_skillBean> datas = new List<data_equip_skillBean>();
	 public static Dictionary<int, data_equip_skillBean> dicdatas = new Dictionary<int, data_equip_skillBean>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_equip_skill.bin";
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
				data_equip_skillBean row=new data_equip_skillBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.trigger_type = din.readInt();
				row.trigger_condition = din.readInt();
				row.condition_val = din.readInt();
				row.probability = din.readInt();
				row.target = din.readInt();
				row.effect_id = din.readInt();
				row.trigger_limited = din.readInt();
				row.effect1_lower = din.readInt();
				row.effect1_upper = din.readInt();
				row.effect2_lower = din.readInt();
				row.effect2_upper = din.readInt();
				row.effect3_lower = din.readInt();
				row.effect3_upper = din.readInt();
				row.desc = din.readUTF();
				row.treasurename_picture = din.readInt();
				row.special_name = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
                dicdatas.Add(row.id, row);
            }
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
