using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardDlg : DialogMonoBehaviour
{
    public static AwardDlg instance;

    public TweenPosition _tweenPos;//动画组件
    public RewardItem _tempItem;//
    public UIGrid _grid;

    List<Reward> rblist;//奖励数据
    public bool _isPlay = false;
    public float _time = 0;
    public bool _isStart = false;

    public List<RewardItem> _awardList = new List<RewardItem>();

    public void SetData(List<Reward> award)
    {
        rblist = award;
        _time = 0;
        if (_isStart)
        {
            _ShowMessageAward();
        }
    }

    private void Awake()
    {
        instance = this;
        _tempItem.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _ShowMessageAward();
        _isStart = true;
    }

    private void _ShowMessageAward()
    {
        for (int i = 0; i < rblist.Count; i++)
        {
            data_resourcesBean resoursbean = data_resourcesDef.dicdatas[rblist[i].mainType][0];
            if (i < _awardList.Count)
            {
                _awardList[i].setReward(rblist[i]);
                _awardList[i].countLabel.text = resoursbean.resources_name + "x" + rblist[i].val;
            }
            else
            {
                if (_tempItem != null)
                {
                    var go = GameObject.Instantiate(_tempItem.gameObject) as GameObject;
                    if (go != null)
                    {
                        go.transform.parent = _grid.transform;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localScale = Vector3.one;
                        go.SetActive(true);
                        RewardItem item = go.GetComponent<RewardItem>();
                        if (item != null)
                        {
                            item.setReward(rblist[i]);
                            item.countLabel.text = resoursbean.resources_name + "x" + rblist[i].val;
                            _awardList.Add(item);
                        }
                    }
                }
            }
        }

        //播放动画
        _tweenPos.ResetToBeginning();
        _tweenPos.PlayForward();
        _isPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlay)
        {
            _time += Time.deltaTime;
            if (_time > 1.3f)
            {
                _isPlay = false;
                closeDialog(null);
            }
        }
    }
}
