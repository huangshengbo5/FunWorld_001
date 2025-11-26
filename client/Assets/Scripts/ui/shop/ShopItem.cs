using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShopItem_Data : GridItem_Data
{
    public data_storeBean dbbean;
    public Technical bean;
    public int index;
    public bool selected = false;
    public callbackObj callObj;
    public ShopItem item;

}
public class ShopItem : GridItem
{
    public UISprite iconsp;
    public UILabel nameTxt;
    public UILabel desTxt;
    public GameObject levelUpbtngo;
    public UILabel levelUpcostTxt;
    public GameObject levelUpcostgo;
    UISprite levelUpcostsp;
    UISprite levelUpbtngosp;
    BoxCollider levelUpbtngocox;
    public GameObject levelMaxgo;

    public GameObject zuannode;
    public UILabel butlabel;

    // Start is called before the first frame update
    private void Awake()
    {
        levelUpcostsp = levelUpcostgo.GetComponent<UISprite>();
        levelUpbtngosp = levelUpcostgo.GetComponent<UISprite>();
        levelUpbtngocox = levelUpbtngo.GetComponent<BoxCollider>();
        UIEventListener.Get(levelUpbtngo).onClick = onclicklevelup;
        SoundEventer.add_But_ClickSound(levelUpbtngo);
    }

    ShopItem_Data data;
  //  List<data_science_levelBean> upgarddatas;
    public override void initItem(GridItem_Data data0)
    {
        data = (ShopItem_Data)data0;
        data.item = this;

        // dbbean = data_scienceDef.getdatabyTechid(data.bean.techID);
        //  upgarddatas = data_science_levelDef.dicdatas[data.dbbean.science_id];
        // Debug.LogError(data.bean.id);
        if (data.dbbean.type == 1)
        {
            iconsp.spriteName = "icon_zuan";
           
        }
        else if (data.dbbean.type == 2)
        {
            iconsp.spriteName = "icon_jinbi";

        }
        refreshUI();
    }
  //  data_science_levelBean nextlevelbean, nowlevelbean;
    // data_scienceBean dbbean;
    private void onclicklevelup(GameObject go)
    {
        if (data.dbbean.type == 1)
        {
           
        }

        Reward transGet = Reward.decode(data.dbbean.rewards);
        HttpManager.instance.sendbuyitem(data.dbbean, transGet,(int code) =>
        {
            refreshUI();
            //UIManager.showToast("升级成功");
        });
    }

    bool iscanup = true;
    void refreshUI()
    {

        nameTxt.text = data.dbbean.name;
        desTxt.text = data.dbbean.info;

        string spname = "icon_zhanli";
        if (data.dbbean.type == 2)
        {
            spname = "icon_zuan";
            levelUpcostsp.spriteName = spname;
            levelUpcostTxt.text = GameGlobal.gamedata.GetNumStr(data.dbbean.price);
            // long havenum = GameGlobal.gamedata.getCurrendy(nextlevelbean.cost_type);

            levelUpbtngo.SetActive(true);
            levelUpcostgo.SetActive(true);
            levelMaxgo.SetActive(false);
        }
        else
        {
            levelUpbtngo.SetActive(true);
            levelUpcostgo.SetActive(true);
            levelMaxgo.SetActive(false);

            zuannode.SetActive(false);
            butlabel.text = data.dbbean.price + "元";
        }


        //iscanup = havenum >= nextlevelbean.cost_val;
        //if (iscanup)
        //{
        //    levelUpbtngocox.enabled = true;
        //    levelUpbtngosp.spriteName = "btm_dalan";
        //}
        //else
        //{
        //    levelUpbtngocox.enabled = false;
        //    levelUpbtngosp.spriteName = "btn_hui";
        //}
    }
}

