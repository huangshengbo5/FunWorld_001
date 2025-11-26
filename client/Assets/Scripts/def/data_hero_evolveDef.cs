using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_hero_evolveBean{
		public int id;
		public int hero_id;
		public int evolve_times;
		public int soul_grade;
		public int soul_level;
		public int cost_type;
		public int cost_sub_type;
		public int cost_val;
		public int fighting_capacity;
		public int attack_ratio;
		public int defend_ratio;

	}
public class data_hero_evolveDef {
	public static int[] types;
	 public static List<data_hero_evolveBean> datas = new List<data_hero_evolveBean>();
	 public static Dictionary<int, List<data_hero_evolveBean>> dicdatas = new Dictionary<int, List<data_hero_evolveBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_hero_evolve.bin";
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
				data_hero_evolveBean row=new data_hero_evolveBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.hero_id = din.readInt();
				row.evolve_times = din.readInt();
				row.soul_grade = din.readInt();
				row.soul_level = din.readInt();
				row.cost_type = din.readInt();
				row.cost_sub_type = din.readInt();
				row.cost_val = din.readInt();
				row.fighting_capacity = din.readInt();
				row.attack_ratio = din.readInt();
				row.defend_ratio = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.hero_id))
				{
					 List<data_hero_evolveBean> tempdatas = dicdatas[row.hero_id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_hero_evolveBean> tempdatas = new List<data_hero_evolveBean>();
					tempdatas.Add(row);
					dicdatas.Add(row.hero_id, tempdatas);
				}
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
