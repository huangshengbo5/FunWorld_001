using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_stageBean{
		public int id;
		public string name;
		public int start_campaign_id;
		public int end_campaign_id;
		public string description;

	}
public class data_stageDef {
	public static int[] types;
	 public static List<data_stageBean> datas = new List<data_stageBean>();
	 public static Dictionary<int, List<data_stageBean>> dicdatas = new Dictionary<int, List<data_stageBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_stage.bin";
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
				data_stageBean row=new data_stageBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.name = din.readUTF();
				row.start_campaign_id = din.readInt();
				row.end_campaign_id = din.readInt();
				row.description = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.id))
				{
					 List<data_stageBean> tempdatas = dicdatas[row.id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_stageBean> tempdatas = new List<data_stageBean>();
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
