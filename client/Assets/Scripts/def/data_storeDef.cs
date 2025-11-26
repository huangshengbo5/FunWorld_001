using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_storeBean{
		public int id;
		public string name;
		public string info;
		public string icon;
		public int type;
		public int price;
		public string rewards;

	}
public class data_storeDef {
	public static int[] types;
	 public static List<data_storeBean> datas = new List<data_storeBean>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_store.bin";
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
				data_storeBean row=new data_storeBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.name = din.readUTF();
				row.info = din.readUTF();
				row.icon = din.readUTF();
				row.type = din.readInt();
				row.price = din.readInt();
				row.rewards = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
