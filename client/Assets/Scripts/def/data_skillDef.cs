using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_skillBean{
		public int id;
		public int skill_id;
		public int skill_type;
		public string skill_name;
		public string skill_picture;
		public string action;
		public string skillname_picture;
    public string special_name;


    }
public class data_skillDef {
	public static int[] types;
	 public static List<data_skillBean> datas = new List<data_skillBean>();
	 public static Dictionary<int, data_skillBean> dicdatas = new Dictionary<int, data_skillBean>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_skill.bin";
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
				data_skillBean row=new data_skillBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.skill_id = din.readInt();
				row.skill_type = din.readInt();
				row.skill_name = din.readUTF();
				row.skill_picture = din.readUTF();
				row.action = din.readUTF();
				row.skillname_picture = din.readUTF();
                row.special_name = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
                dicdatas.Add(row.skill_id, row);
            }
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}
