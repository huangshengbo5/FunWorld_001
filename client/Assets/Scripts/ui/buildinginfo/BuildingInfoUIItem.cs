using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoUIItem_Data : GridItem_Data
{
    public Building bean;
    public int index;
    public bool selected = false;
    public callbackObj callObj;
    public BuildingInfoUIItem item;

}

public class BuildingInfoUIItem : GridItem
{
    public UITexture iconTexture;
    public UILabel nameTxt;
    public UILabel desTxt;
    public GameObject levelUpbtngo;
    public RewardItem costItem;
    public GameObject levelMaxgo;
    UISprite levelUpbtnsp;
    BoxCollider levelUpbtncox;
    // Start is called before the first frame update
    private void Awake()
    {
        UIEventListener.Get(levelUpbtngo).onClick = onclicklevelup;
        SoundEventer.add_But_ClickSound(levelUpbtngo);
        levelUpbtnsp = levelUpbtngo.GetComponent<UISprite>();
        levelUpbtncox = levelUpbtngo.GetComponent<BoxCollider>();
    }

    BuildingInfoUIItem_Data data;
    List<data_building_upgradeBean> upgarddatas;
    public override void initItem(GridItem_Data data0)
    {
        data = (BuildingInfoUIItem_Data)data0;
        data.item = this;
        gameObject.name = data.bean.buildingID.ToString();

        dbbean = data_buildingDef.getdatabybuildid(data.bean.buildingID);
        upgarddatas = data_building_upgradeDef.dicdatas[data.bean.buildingID];
        Debug.LogError(data.bean.buildingID);
        {
            var img = ResManager.getTex999("actiongroup/" + "jianzhutou" + "/mod" + 0 + "_" + (dbbean.id - Statics.BUILDID));
            iconTexture.mainTexture = img;
            refreshUI();
        }
    }
    data_building_upgradeBean nextlevelbean, nowlevelbean;
    data_buildingBean dbbean;
    private void onclicklevelup(GameObject go)
    {
        if (data.bean.level >= Statics.BUILDLEVELMAX)
        {
            UIManager.showToast("等级已满");
            return;
        }
        if (isCanUp)
        {
            HttpManager.instance.sendBuildingLevelUp(data.bean, nextlevelbean.getlvcost(), (int code) =>
            {
                refreshUI();
                //UIManager.showToast("升级成功");
            });
        }
        GuideManager.Instance.GameGuideEventCheck(GuideActiveType.LevelUp, 0);
    }
    void refreshUI()
    {
        BuildingInfoUIItem[] items = transform.parent.GetComponentsInChildren<BuildingInfoUIItem>();
        for (int i = 0; i < items.Length; i++)
        {
            items[i].refreshUI2();
        }
    }
    bool isCanUp = true;
    void refreshUI2()
    {
        nowlevelbean = upgarddatas[data.bean.level - 1];
        if (upgarddatas.Count > data.bean.level)
        {
            nextlevelbean = upgarddatas[data.bean.level];
        }
        nameTxt.text = dbbean.name + "等级:" + data.bean.level;
        desTxt.text = nowlevelbean.desc;

        if (nextlevelbean == null)
        {
            levelMaxgo.SetActive(true);
            costItem.gameObject.SetActive(false);
            levelUpbtngo.SetActive(false);
        }
        else
        {
            var cost = nextlevelbean.getlvcost();
            costItem.setReward(cost);
            costItem.gameObject.SetActive(true);
            levelUpbtngo.SetActive(true);
            levelMaxgo.SetActive(false);
            long havenum = GameGlobal.gamedata.getCurrendy(nextlevelbean.cost_type);
            isCanUp = havenum >= nextlevelbean.cost_val;
            if (isCanUp)
            {
                levelUpbtncox.enabled = true;
                levelUpbtnsp.spriteName = "btm_dalan";
            }
            else
            {
                levelUpbtncox.enabled = false;
                levelUpbtnsp.spriteName = "btn_hui";
            }

        }
    }
}
