using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_crystal_upgradeBean{
		public int id;
		public int crystal_id;
		public int level;
		public int effect_id;
		public int effect_val;
		public int cost_type;
		public int cost_sub_type;
		public int cost_val;

	}
public class data_crystal_upgradeDef {
	public static int[] types;
	 public static List<data_crystal_upgradeBean> datas = new List<data_crystal_upgradeBean>();
	 public static Dictionary<int, List<data_crystal_upgradeBean>> dicdatas = new Dictionary<int, List<data_crystal_upgradeBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_crystal_upgrade.bin";
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
				data_crystal_upgradeBean row=new data_crystal_upgradeBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.crystal_id = din.readInt();
				row.level = din.readInt();
				row.effect_id = din.readInt();
				row.effect_val = din.readInt();
				row.cost_type = din.readInt();
				row.cost_sub_type = din.readInt();
				row.cost_val = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.crystal_id))
				{
					 List<data_crystal_upgradeBean> tempdatas = dicdatas[row.crystal_id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_crystal_upgradeBean> tempdatas = new List<data_crystal_upgradeBean>();
					tempdatas.Add(row);
					dicdatas.Add(row.crystal_id, tempdatas);
				}
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
