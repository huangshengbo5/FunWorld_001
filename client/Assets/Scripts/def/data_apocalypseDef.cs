using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_apocalypseBean
{
		public int id;
		public int fighting_capacity;
		public int npc_id;
		public string rewards;
		public int limited;
	}
public class data_apocalypseDef
{
	public static int[] types;
	public static List<data_apocalypseBean> datas = new List<data_apocalypseBean>();

	public static Dictionary<int,data_apocalypseBean> datasDic = new Dictionary<int,data_apocalypseBean>();

	public static void load(){
		string path=CTGlobal.defRoot+ "data_apocalypse.bin";
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
				data_apocalypseBean row =new data_apocalypseBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.fighting_capacity = din.readInt();
				row.npc_id = din.readInt();
				row.rewards = din.readUTF();
				row.limited = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				datasDic.Add(row.id,row);
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}

