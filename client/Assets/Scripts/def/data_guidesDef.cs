using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class data_guidesBean
{
		public int id;
		public int guidetype;//Òýµ¼ÀàÐÍ
		public int conditiontype;//´¥·¢Ìõ¼þ
		public int conparam;//´¥·¢²ÎÊý
		public int activetype;//³öÏÖÀàÐÍ
		public int activeparam;//³öÏÖ²ÎÊý
		public string path;//Òýµ¼Â·¾¶
		public int complatetype;//Íê³ÉÀàÐÍ
		public int comparam;//Íê³É²ÎÊý
		public string content;//ÄÚÈÝ
		public int next;//ÏÂÒ»¸öÒýµ¼
		public int group;//Òýµ¼×éid
		public int over;//ÊÇ·ñ´ú±íÍê³É
}
public class data_guidesDef
{
	public static int[] types;
	public static List<data_guidesBean> datas = new List<data_guidesBean>();

	public static Dictionary<int, data_guidesBean> guideDic = new Dictionary<int, data_guidesBean>();

	public static Dictionary<int, List<data_guidesBean>> guideList = new Dictionary<int, List<data_guidesBean>>();
	public static void load(){
		string path=CTGlobal.defRoot+ "data_guides.bin";
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
				data_guidesBean row =new data_guidesBean();
				din.resetReadTimes();
				row.id = din.readInt();
				row.guidetype = din.readInt();
				row.conditiontype = din.readInt();
				row.conparam = din.readInt();
				row.activetype = din.readInt();
				row.activeparam = din.readInt();
				row.path = din.readUTF();
				row.complatetype = din.readInt();
				row.comparam = din.readInt();
				row.content = din.readUTF();
				row.next = din.readInt();
                row.group = din.readInt();
                row.over = din.readInt();
				if (!guideList.ContainsKey(row.group))
				{
					List<data_guidesBean> list = new List<data_guidesBean>();
					list.Add(row);
					guideList.Add(row.group, list);
				}
				else
				{
					guideList[row.group].Add(row);
				}
                if (!guideDic.ContainsKey(row.id))
				{
					guideDic.Add(row.id,row);
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

