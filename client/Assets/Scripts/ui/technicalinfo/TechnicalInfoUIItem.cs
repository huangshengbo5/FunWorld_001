using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnicalInfoUIItem_Data : GridItem_Data
{
    public data_scienceBean dbbean;
    public Technical bean;
    public int index;
    public bool selected = false;
    public callbackObj callObj;
    public TechnicalInfoUIItem item;

}

public class TechnicalInfoUIItem : GridItem
{
    public UISprite iconsp;
    public UILabel nameTxt;
    public UILabel desTxt;
    public GameObject levelUpbtngo;
    public UILabel levelUpcostTxt;
    public GameObject levelUpcostgo;
   // UISprite levelUpcostsp;
   // UISprite levelUpbtngosp;
    BoxCollider levelUpbtngocox;
    public GameObject levelMaxgo;

    // Start is called before the first frame update
    private void Awake()
    {
       // levelUpcostsp = levelUpcostgo.GetComponent<UISprite>();
       // levelUpbtngosp = levelUpcostgo.GetComponent<UISprite>();
        levelUpbtngocox = levelUpbtngo.GetComponent<BoxCollider>();
        UIEventListener.Get(levelUpbtngo).onClick = onclicklevelup;
        SoundEventer.add_But_ClickSound(levelUpbtngo);
    }

    TechnicalInfoUIItem_Data data;
    List<data_science_levelBean> upgarddatas;
    public override void initItem(GridItem_Data data0)
    {
        data = (TechnicalInfoUIItem_Data)data0;
        data.item = this;

       // dbbean = data_scienceDef.getdatabyTechid(data.bean.techID);
        upgarddatas = data_science_levelDef.dicdatas[data.dbbean.science_id];
       // Debug.LogError(data.bean.id);
        {
            iconsp.spriteName = data.dbbean.science_picture;
            refreshUI();
        }
    }
    data_science_levelBean nextlevelbean, nowlevelbean;
   // data_scienceBean dbbean;
    private void onclicklevelup(GameObject go)
    {
        if (data.bean.level >= Statics.TechnicalMax)
        {
            UIManager.showToast("等级已满");
            return;
        }

        if (iscanup)
        {
            HttpManager.instance.sendTechnicalLevelUp(data.bean, nextlevelbean.getlvcost(), (int code) =>
            {
                refreshUI();
                //UIManager.showToast("升级成功");
            });
        }
    }
    public void refreshUI()
    {
        TechnicalInfoUIItem[] items= transform.parent.GetComponentsInChildren<TechnicalInfoUIItem>();
        for(int i = 0; i < items.Length; i++)
        {
            items[i].refreshUI2();
        }
    }
    bool iscanup = true;
   public void refreshUI2()
    {
        nowlevelbean = upgarddatas[data.bean.level];
        nextlevelbean = upgarddatas[data.bean.level + 1];
        nameTxt.text = data.dbbean.science_name + "等级:" + data.bean.level;
        desTxt.text = nowlevelbean.science_describe;

        if (nextlevelbean == null)
        {
            levelUpbtngo.SetActive(false);
            levelUpcostgo.SetActive(false);
            levelMaxgo.SetActive(true);
        }
        else
        {
            string spname = "icon_jinbi";
            if (nextlevelbean.cost_type == 2)
            {
                spname = "icon_zuan";
            }
            levelUpcostgo.GetComponent<UISprite>().spriteName = spname;
            long havenum = GameGlobal.gamedata.getCurrendy(nextlevelbean.cost_type);
         //   Debug.LogError("xian" + nextlevelbean.cost_val);
            levelUpcostTxt.text = GameGlobal.gamedata.GetNumStr(nextlevelbean.cost_val) + "/ " + GameGlobal.gamedata.GetNumStr(havenum);
            levelUpbtngo.SetActive(true);
            levelUpcostgo.SetActive(true);
            levelMaxgo.SetActive(false);
            iscanup = havenum >= nextlevelbean.cost_val;
            if (iscanup)
            {
                levelUpbtngocox.enabled = true;
                levelUpbtngo.GetComponent<UISprite>().spriteName = "btm_dalan";
            }
            else
            {
                levelUpbtngocox.enabled = false;
                levelUpbtngo.GetComponent<UISprite>().spriteName = "btn_hui";
            }
        }
    }
}
