using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_exploreBean{
		public int id;
		public string rewards;
		public string races;
		public int duration;

	}
public class data_exploreDef {
	public static int[] types;
	 public static List<data_exploreBean> datas = new List<data_exploreBean>();
	 public static Dictionary<int, List<data_exploreBean>> dicdatas = new Dictionary<int, List<data_exploreBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_explore.bin";
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
				data_exploreBean row=new data_exploreBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.rewards = din.readUTF();
				row.races = din.readUTF();
				row.duration = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.id))
				{
					 List<data_exploreBean> tempdatas = dicdatas[row.id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_exploreBean> tempdatas = new List<data_exploreBean>();
					tempdatas.Add(row);
					dicdatas.Add(row.id, tempdatas);
				}
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
