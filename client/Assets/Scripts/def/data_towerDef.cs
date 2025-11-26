using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
	public class data_towerBean{
   // campaign_id_lower campaign_id_upper   seq

        public int id;
		public int npc_id;
		public int campaign_id_lower;
    public int campaign_id_upper;
    public int seq;
    public int weight;
		public string rewards1;
		public int rewards1_probability;
		public string rewards2;
		public int rewards2_probability;
		public string rewards3;
		public int ewards3_probability;
		public string rewards4;
		public int rewards4_probability;

    public string getrewardstr()
    {
        StringBuilder sb = new StringBuilder();
        if (rewards1 != "0")
        {
            sb.Append(rewards1);
        }
        if (rewards2 != "0")
        {
            sb.Append("|");
            sb.Append(rewards2);
        }
        if (rewards3 != "0")
        {
            sb.Append("|");
            sb.Append(rewards3);
        }
        //if (rewards4 != "0")
        //{
        //    sb.Append("|");
        //    sb.Append(rewards4);
        //}
        return sb.ToString();
    }
}
public class data_towerDef {
	public static int[] types;
	 public static List<data_towerBean> datas = new List<data_towerBean>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_tower.bin";
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
				data_towerBean row=new data_towerBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.npc_id = din.readInt();
				row.campaign_id_lower = din.readInt();
                row.campaign_id_upper = din.readInt();
                row.seq = din.readInt();
                row.weight = din.readInt();
				row.rewards1 = din.readUTF();
				row.rewards1_probability = din.readInt();
				row.rewards2 = din.readUTF();
				row.rewards2_probability = din.readInt();
				row.rewards3 = din.readUTF();
				row.ewards3_probability = din.readInt();
				row.rewards4 = din.readUTF();
				row.rewards4_probability = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}
    public static data_towerBean getdatabyid(int id)
    {
        for(int i = 0; i < datas.Count; i++)
        {
            if (datas[i].id == id)
            {
                return datas[i];
            }
        }
        return null;
    }
}
