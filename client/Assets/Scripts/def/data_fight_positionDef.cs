using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_fight_positionBean{
		public int id;
		public string fight_model;
		public string effects_hittarget;
		public int effects_hittarget_position;
		public int effects_vertigo_position;
		public int effects_luminescence_position;
		public int effects_purify_position;
		public int effects_accelerate_position;

	}
public class data_fight_positionDef {
	public static int[] types;
	 public static List<data_fight_positionBean> datas = new List<data_fight_positionBean>();
	 public static Dictionary<string, List<data_fight_positionBean>> dicdatas = new Dictionary<string, List<data_fight_positionBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_fight_position.bin";
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
				data_fight_positionBean row=new data_fight_positionBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.fight_model = din.readUTF();
				row.effects_hittarget = din.readUTF();
				row.effects_hittarget_position = din.readInt();
				row.effects_vertigo_position = din.readInt();
				row.effects_luminescence_position = din.readInt();
				row.effects_purify_position = din.readInt();
				row.effects_accelerate_position = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.fight_model))
				{
					 List<data_fight_positionBean> tempdatas = dicdatas[row.fight_model];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_fight_positionBean> tempdatas = new List<data_fight_positionBean>();
					tempdatas.Add(row);
					dicdatas.Add(row.fight_model, tempdatas);
				}
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
