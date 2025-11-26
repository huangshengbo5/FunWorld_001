using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_online_rewardBean{
		public int id;
		public string rewards;
		public int probability;

	}
public class data_online_rewardDef {
	public static int[] types;
	 public static List<data_online_rewardBean> datas = new List<data_online_rewardBean>();
	 public static Dictionary<int, List<data_online_rewardBean>> dicdatas = new Dictionary<int, List<data_online_rewardBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_online_reward.bin";
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
				data_online_rewardBean row=new data_online_rewardBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.rewards = din.readUTF();
				row.probability = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.id))
				{
					 List<data_online_rewardBean> tempdatas = dicdatas[row.id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_online_rewardBean> tempdatas = new List<data_online_rewardBean>();
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
