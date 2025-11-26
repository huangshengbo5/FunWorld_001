using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_mailBean
{
		public int id;
		public int type;
		public string title;
		public string pic;
		public string content;
	}
public class data_mailDef
{
	public static int[] types;
	public static List<data_mailBean> datas = new List<data_mailBean>();

	public static Dictionary<int, data_mailBean> datasDic = new Dictionary<int, data_mailBean>();

	public static void load(){
		string path=CTGlobal.defRoot+ "data_mail.bin";
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
				data_mailBean row =new data_mailBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.type = din.readInt();
				row.title = din.readUTF();
				row.pic = din.readUTF();
				row.content = din.readUTF();
				DefTools.skipNewValue(din, types);
				datasDic.Add(row.id, row);
				datas.Add(row);
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}

