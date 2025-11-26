using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_dialogueBean
{
		public int id;
		public int dialogue_type;
		public int dialogue_group_id;
		public string dialogue_name;//Ãû³Æ
		public string dialogue_model;//ÐÎÏó
		public string face_action;//¶¯×÷
		public int model_position;//Î»ÖÃ
		public int flip;//·´×ª
		public string dialogue_content;//¶Ô»°ÄÚÈÝ
	}
public class data_dialogueDef
{
	public static int[] types;
	public static List<data_dialogueBean> datas = new List<data_dialogueBean>();

	public static Dictionary<int,List<data_dialogueBean>> datasDic = new Dictionary<int, List<data_dialogueBean>>();

	public static void load(){
		string path=CTGlobal.defRoot+ "data_dialogue.bin";
		JavaReader jr = DefTools.getSdCardResourcedef(path);

		load(jr);
	}
	public static void load(JavaReader din)
		{
		datas.Clear();
		datasDic.Clear();
		try
		{
			int len=din.readInt();
			types=new int[len];
			for (int i = 0; i < types.Length; i++) {
				types[i]=din.readByte();
			}
			int dataLen=din.readInt();
			for (int i = 0; i < dataLen; i++) {
				data_dialogueBean row =new data_dialogueBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.dialogue_type = din.readInt();
				row.dialogue_group_id = din.readInt();
				row.dialogue_name = din.readUTF();
				row.dialogue_model = din.readUTF();
				row.face_action = din.readUTF();
				row.model_position = din.readInt();
				row.flip = din.readInt();
				row.dialogue_content = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (datasDic.ContainsKey(row.dialogue_group_id))
				{
					datasDic[row.dialogue_group_id].Add(row);
				}
				else
				{
					List<data_dialogueBean> list = new List<data_dialogueBean>();
					list.Add(row);
					datasDic.Add(row.dialogue_group_id, list);
					//Debug.Log("添加剧情对话============="+ row.dialogue_group_id);
				}
				
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}

