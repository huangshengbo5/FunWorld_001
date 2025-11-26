using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_buff_comparisonBean{
		public int id;
		public string buff_name;
		public string buff_pic;
		public string buff_des;
		public string buff_special;

	}
public class data_buff_comparisonDef {
	public static int[] types;
	 public static List<data_buff_comparisonBean> datas = new List<data_buff_comparisonBean>();
	 public static Dictionary<int, List<data_buff_comparisonBean>> dicdatas = new Dictionary<int, List<data_buff_comparisonBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_buff_comparison.bin";
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
				data_buff_comparisonBean row=new data_buff_comparisonBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.buff_name = din.readUTF();
				row.buff_pic = din.readUTF();
				row.buff_des = din.readUTF();
				row.buff_special = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.id))
				{
					 List<data_buff_comparisonBean> tempdatas = dicdatas[row.id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_buff_comparisonBean> tempdatas = new List<data_buff_comparisonBean>();
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
