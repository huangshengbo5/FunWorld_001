using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_helpDialogueBean
{
		public int id;
		public int campaign_id_lower;
		public int campaign_id_upper;
		public string dialogue_choice_group;//Ãû³Æ

		public List<dia_weight> list;
	}

public class dia_weight
{
	public int dialogueId;
	public int weight;
}
public class help_dialogueDef
{
	public static int[] types;
	public static List<data_helpDialogueBean> datas = new List<data_helpDialogueBean>();

	public static void load(){
		string path=CTGlobal.defRoot+ "help_dialogue.bin";
		JavaReader jr = DefTools.getSdCardResourcedef(path);

		load(jr);
	}
	public static void load(JavaReader din)
		{
		datas.Clear();
		try
		{
			int len=din.readInt();
			types=new int[len];
			for (int i = 0; i < types.Length; i++) {
				types[i]=din.readByte();
			}
			int dataLen=din.readInt();
			for (int i = 0; i < dataLen; i++) {
				data_helpDialogueBean row =new data_helpDialogueBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.campaign_id_lower = din.readInt();
				row.campaign_id_upper = din.readInt();
				row.dialogue_choice_group = din.readUTF();
				row.list = new List<dia_weight>();
			 	var arry = row.dialogue_choice_group.Split(';');
				for (int n = 0; n < arry.Length; n++)
				{
                    if (arry[n].Length == 0)
                    {
                        continue;
                    }
                  //  Debug.Log(row.dialogue_choice_group+"添加权重=========" + arry[n] +"对话"+ n+"权重"+ arry.Length);
                    var weights = arry[n].Split('_');
					int dialogueId = int.Parse(weights[0]);
					int weight = int.Parse(weights[1]);
					dia_weight one = new dia_weight();
					one.dialogueId = dialogueId;
					one.weight = weight;
					row.list.Add(one);

				}

				DefTools.skipNewValue(din, types);
				datas.Add(row);
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
}

