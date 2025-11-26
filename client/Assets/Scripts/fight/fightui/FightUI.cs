using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUI : MonoBehaviour
{
    public UILabel idxlabel;
    public FightTou[] touarray = new FightTou[2];
    public GameObject tipnode, tipbg,tipkuang,jumpbut;
    public UILabel tiplabel0, tiplabel1,timelabel;
    public UILabel[] wanjialabel = new UILabel[2];
    public UISprite heiping;
    void Awake()
    {
        UICamera mapcamera = transform.parent.parent.GetComponentInChildren<UICamera>();
        UIViewGroup.setmapcamera(mapcamera);
        tipnode.SetActive(false);

        timelabel.gameObject.SetActive(false);

        UIEventListener.Get(jumpbut).onClick = onclickjump;
    }

    private void onclickjump(GameObject go)
    {
        fc.sta = -2;
        fc.showresult();
    }

    internal void settip(BuffIcon buffIcon, int buffid)
    {
        tipnode.SetActive(true);
        tiplabel0.text = data_buff_comparisonDef.dicdatas[buffid][0].buff_name;
        tiplabel1.text = data_buff_comparisonDef.dicdatas[buffid][0].buff_des;

        tipkuang.transform.localPosition = gameObject.transform.InverseTransformPoint(buffIcon.transform.position) + new Vector3(buffIcon.transform.localScale.x*300, -100, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(tipbg).onClick = onclick;
    }

    private void onclick(GameObject go)
    {
        tipnode.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        idxlabel.text = fc.fbactindex+"/"+FightControl.fb.actionlist.Count;

       // time -= (long)(Time.deltaTime * 1000);

        if (time < 0)
        {
            time = 0;
        }

        timelabel.text = CTTools.mmToTimeMMSS(time);

    }

    FightControl fc;
    public void setFC(FightControl fightControl)
    {
        fc = fightControl;
    }
    public long time;
    public void init(List<FightObject> fightlist)
    {
        time = 2 * 60*1000L;

        for (int i = 0; i < touarray.Length; i++)
        {
            touarray[i].setFO(fightlist[i]);
        }

        if (FightControl.fbtype == 5 && GameGlobal.fromTianqiFight)
        {

            if (GameGlobal.gamedata.tianqiReportIndex == 0)
            {
                wanjialabel[0].gameObject.SetActive(true);
                wanjialabel[1].gameObject.SetActive(true);
                wanjialabel[0].text = GameGlobal.gamedata.tianqiReport[1].attacker.name;
                wanjialabel[1].text = GameGlobal.gamedata.tianqiReport[2].attacker.name;
            }
            else if (GameGlobal.gamedata.tianqiReportIndex == 1)
            {
                wanjialabel[0].gameObject.SetActive(true);
                wanjialabel[1].gameObject.SetActive(false);
                wanjialabel[0].text = GameGlobal.gamedata.tianqiReport[2].attacker.name;
            }
            else
            {
                wanjialabel[0].gameObject.SetActive(false);
                wanjialabel[1].gameObject.SetActive(false);
            }
        }
        else
        {
            wanjialabel[0].gameObject.SetActive(false);
            wanjialabel[1].gameObject.SetActive(false);
        }
    }

    internal void refreshbufficon()
    {
        for (int i = 0; i < touarray.Length; i++)
        {
            touarray[i].refreshbufficon();
        }
    }

    public void showheiping(Action act)
    {
        heiping.gameObject.SetActive(true);
        TweenAlpha tc = heiping.GetComponent<TweenAlpha>();


        tc.from = 0;
        tc.to = 1;
        tc.duration = 1f;
        tc.delay = 0.01f;
        tc.ResetToBeginning();
        tc.PlayForward();


        tc.onFinished.Clear();
        EventDelegate.Add(tc.onFinished, delegate ()
        {
            act();
        });
    }
}
