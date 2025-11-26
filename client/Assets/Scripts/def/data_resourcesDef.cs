using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_resourcesBean{
		public int id;
		public string resources_name;
		public string resources_describe;
		public string resources_picture;

	}
public class data_resourcesDef {
	public static int[] types;
	 public static List<data_resourcesBean> datas = new List<data_resourcesBean>();
	 public static Dictionary<int, List<data_resourcesBean>> dicdatas = new Dictionary<int, List<data_resourcesBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_resources.bin";
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
				data_resourcesBean row=new data_resourcesBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.resources_name = din.readUTF();
				row.resources_describe = din.readUTF();
				row.resources_picture = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.id))
				{
					 List<data_resourcesBean> tempdatas = dicdatas[row.id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_resourcesBean> tempdatas = new List<data_resourcesBean>();
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
