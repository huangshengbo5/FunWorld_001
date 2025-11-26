using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_equip_upgradeBean{
		public int id;
		//public int equip_id;
		//public int level;
		public int cost_type;
		public int cost_sub_type;
		public int cost_val;
		//public int fighting_capacity;
		public string rewards;
		public int multiplier;

	public Reward upcost;
    public Reward getlvcost()
    {
        if (upcost == null)
        {
            upcost = new Reward(cost_type, cost_sub_type, cost_val);
        }
        return upcost;
    }

}
public class data_equip_upgradeDef {
	public static int[] types;
	 public static List<data_equip_upgradeBean> datas = new List<data_equip_upgradeBean>();
    public static Dictionary<int, List<data_equip_upgradeBean>> dicdatas = new Dictionary<int, List<data_equip_upgradeBean>>();
    public static void load(){
		string path=CTGlobal.defRoot+"data_equip_upgrade.bin";
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
				data_equip_upgradeBean row=new data_equip_upgradeBean();
				din.resetReadTimes();
				row.id = din.readInt();
				//row.equip_id = din.readInt();
				//row.level = din.readInt();
				row.cost_type = din.readInt();
				row.cost_sub_type = din.readInt();
				row.cost_val = din.readInt();
				//row.fighting_capacity = din.readInt();
				row.rewards = din.readUTF();
				row.multiplier = din.readInt();
				DefTools.skipNewValue(din, types);
				datas.Add(row);

                if (dicdatas.ContainsKey(row.id))
                {
                    List<data_equip_upgradeBean> tempdatas = dicdatas[row.id];
                    tempdatas.Add(row);
                }
                else
                {
                    List<data_equip_upgradeBean> tempdatas = new List<data_equip_upgradeBean>();
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
