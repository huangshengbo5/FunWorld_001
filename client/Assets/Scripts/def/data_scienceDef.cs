using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class data_scienceBean{
		public int id;
		public int science_id;
		public string science_name;
		public string science_picture;

	}
public class data_scienceDef {
	public static int[] types;
	 public static List<data_scienceBean> datas = new List<data_scienceBean>();
	 public static Dictionary<int, List<data_scienceBean>> dicdatas = new Dictionary<int, List<data_scienceBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+"data_science.bin";
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
				data_scienceBean row=new data_scienceBean();
				din.resetReadTimes();
				//row.id = din.readInt();
				row.science_id = din.readInt();
				row.science_name = din.readUTF();
				row.science_picture = din.readUTF();
				DefTools.skipNewValue(din, types);
				datas.Add(row);
				if (dicdatas.ContainsKey(row.science_id))
				{
					 List<data_scienceBean> tempdatas = dicdatas[row.science_id];
					tempdatas.Add(row);
				 }
				 else
				 {
					 List<data_scienceBean> tempdatas = new List<data_scienceBean>();
					tempdatas.Add(row);
					dicdatas.Add(row.science_id, tempdatas);
				}
			}
			din.close();
		} catch (Exception e) {
			Debug.LogError(e.StackTrace);
		}

	}

    public static data_scienceBean getdatabyTechid(int techid)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].science_id == techid)
            {
                return datas[i];
            }
        }
        return null;
    }
}
