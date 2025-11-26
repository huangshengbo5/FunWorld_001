using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_avatarBean{
		public int id;
		public string name;

	}
public class data_avatarDef {
	public static int[] types;
	 public static List<data_avatarBean> datas = new List<data_avatarBean>();
	 public static Dictionary<int, List<data_avatarBean>> dicdatas = new Dictionary<int, List<data_avatarBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_avatar.bin";
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
				data_avatarBean row=new data_avatarBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.name = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.id))
				{
					 List<data_avatarBean> tempdatas = dicdatas[row.id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_avatarBean> tempdatas = new List<data_avatarBean>();
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
