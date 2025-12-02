using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;


public class Calendar
{
    public int status { get; set; }
    public List<CalendarData> data { get; set; }
}
public class CalendarData
{
    //public Holiday holiday { get; set; }
    public List<Holiday> holiday { get; set; }
}
public class Holiday
{
    public string desc { get; set; }
    public string festival { get; set; }
    public List<HolidayList> list { get; set; }
    public string name { get; set; }
    public string rest { get; set; }
}
public class HolidayList
{
    public string date { get; set; }
    /// <summary>
    /// 1休息2上班
    /// </summary>
    public int status { get; set; }
    public string remark
    {
        get
        {
            return status == 1 ? "休假" : "上班";
        }
    }
}
public class Calendar2
{
    public int status { get; set; }
    public List<CalendarData2> data { get; set; }
}
public class CalendarData2
{
    public Holiday holiday { get; set; }
}

public class CoverDlg : MonoBehaviour
{
    public static CoverDlg instance;
    public static bool logoflag = false;
    public GameObject logonode;
    public TweenPosition ts;
    public UILabel msglabel;
    public UIInput input,input2;
    public GameObject but,but16,zhucebut;
    public UIToggle togl_IsClear;
    public NewStoryPaly storyPlay;
    public GameObject rootgo;
    void Awake()
    {
        instance = this;
        UICamera mapcamera = transform.parent.GetComponentInChildren<UICamera>();
        UIViewGroup.setmapcamera(mapcamera);
        if (logoflag)
        {
            logonode.SetActive(false);
        }
        else
        {
            logonode.SetActive(true);

            EventDelegate.Add(ts.onFinished, delegate ()
            {
                logonode.SetActive(false);
                logoflag = true;
            });
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        input.value = "CCC211";
        string uname = PlayerPrefs.GetString("login_input0", "");
        string upass = PlayerPrefs.GetString("login_input1", "");
        input.value = uname;
        input2.value = upass;
        UIEventListener.Get(but).onClick = onClick;
        UIEventListener.Get(but16).onClick = onClick16;
        UIEventListener.Get(zhucebut).onClick = onClickzc;
        SoundEventer.add_But_ClickSound(but);
        storyPlay.ActOnStoryPalyEnd += OnPlayStoryEnd;
      //  MusicPlayer.getInstance().PlayW("Map", true);
    }

    private void onClickzc(GameObject go)
    {
        UIManager.showRTip();
    }

    public bool CheckNeedToWork(DateTime date)
    {
        var result = CheckIsHoliday(date);
        bool flag = false;
        if (result == "休假")
        {
            flag = false;
        }
        else if (result == "上班")
        {
            flag = true;
        }
        else if (result == "")
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    flag = false;
                    break;
                case DayOfWeek.Monday:
                    flag = true;
                    break;
                case DayOfWeek.Tuesday:
                    flag = true;
                    break;
                case DayOfWeek.Wednesday:
                    flag = true;
                    break;
                case DayOfWeek.Thursday:
                    flag = true;
                    break;
                case DayOfWeek.Friday:
                    flag = false;
                    break;
                case DayOfWeek.Saturday:
                    flag = false;
                    break;
                default:
                    break;
            }
        }
        return flag;
    }

    private static string CheckIsHoliday(DateTime time)
    {
        var timeStr = time.ToString("yyyy-MM-dd");
        string date = string.Concat(time.Year, "-", time.Month);
        WebClient client = new WebClient();
        client.Encoding = Encoding.Default;
        var url = $"https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?query={date}&resource_id=6018&ie=utf8&oe=gbk";
        Debug.LogError(url);
        var jsondata = client.DownloadString(url);
        Debug.LogError(jsondata);
        try
        {
            var model = JsonConvert.DeserializeObject<Calendar>(jsondata);
            foreach (var item in model.data)
            {
                foreach (var holiday in item.holiday)
                {
                    foreach (var day in holiday.list)
                    {
                        DateTime.TryParse(day.date.ToString(), out DateTime dt);
                        if (DateTime.Compare(dt, time) == 0)
                        {
                            return day.remark;
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            var model = JsonConvert.DeserializeObject<Calendar2>(jsondata);
            foreach (var item in model.data)
            {
                if (item.holiday != null && item.holiday.list != null)
                {
                    foreach (var day in item.holiday.list)
                    {
                        Console.WriteLine($"{day.date}:{day.remark}");
                        DateTime.TryParse(day.date.ToString(), out DateTime dt);
                        if (DateTime.Compare(dt, time) == 0)
                        {
                            return day.remark;
                        }
                    }
                }
            }
        }
        return "";
    }



    private void onClick16(GameObject go)
    {
        string str = "提示信息：\n"
+ "1）本游戏是一款移动端游戏，适用于年满 16 周岁及以上的用户，建议未成年人在家长监护下使用游戏产品。\n"
+ "2）本游戏以幻想大陆为背景，有结识同伴、战胜对手、锄强扶弱的内容，这部分内容不会与现实生活相混淆。游戏玩法基于卡牌对战的游戏玩法，融合不断提升自我的强化功能，鼓励玩家收集道具并通过活动道具功能，击败强大的对手。游戏中没有基于文字或语音的陌生人社交系统。\n"
+ "3）游戏中有用户实名认证系统，未实名账号不能登录。认证为未成年人的用户将接受以下管理：\n"
+ "游戏中部分玩法和道具需要付费购买。未满8周岁的用户不能付费；8周岁以上未满16周岁的未成年人用户，单次充值金额不得超过50元人民币，每月充值金额累计不得超过200元人民币；16周岁以上的未成年人用户，单次充值金额不得超过100元人民币，每月充值金额累计不得超过400元人民币。\n"
+ "未成年人用户可在周五、周六、周日和法定节假日每日晚20时至21时登录游戏，其他时间无法登录游戏。\n"
+ "4）本游戏将卡牌游戏与幻想大陆世界相融合，人物角色设计采用卡通造型，通过易于上手而又富有挑战的关卡设计，培养玩家观察、挑战、运用策略的能力，适合想要在轻松的游戏娱乐过程中挑战关卡和成长策略的玩家。";

        UIManager.showTip(str, null, null, null, null, "提示信息");
    }

    private void loginover()
    {
      //  long now = GameGlobal.getCurrTimeMM();
      //  Date nowd = new Date(now*1000, true);
      //  Debug.LogError(nowd.getHours());
      //  Debug.LogError(IsHolidayByDate(nowd.toYMD2()));

        //string str = "";
        //if (GameGlobal.gamedata.userinfo.type == 1|| GameGlobal.gamedata.userinfo.type == 2|| GameGlobal.gamedata.userinfo.type == 3)
        //{
        //    str = "亲爱的用户您好:\n\n"
        //    + "根据《关于进一步严格管理切实防止未成年人沉迷网络游戏的通知》要求，禁止未成年用户在周五、周六、周日和法定节假日的20: 00 - 21:00外时间登录。";

        //    UIManager.showTip(str, null, null, null, null, "登录");
        //}
      






        var userId = input.value;
        //  GameGlobal.gamedata.guideStep = 0;
        
        //todo 暂时屏蔽引导逻辑
        // if (GameGlobal.gamedata.guideStep < 8)
        // {
        //     rootgo.SetActive(false);
        //     storyPlay.PlayStroy();
        // }
        // else
        // {
        //     if (GameGlobal.gamedata.guideStep >= 9 && GameGlobal.gamedata.guideStep < 16)
        //     {
        //         GameGlobal.gamedata.isWaitWorld = true;
        //     }
        //     else
        //     {
        //         GameGlobal.gamedata.isWaitWorld = false;
        //     }
        //     GameGlobal.enterMenuScene();
        // }

        GameGlobal.enterMenuScene();
        
        //var localuserId = PlayerPrefs.GetString("storyMark", "");
        //if (localuserId.Equals("storyMark" + userId))
        //{
        //    GameGlobal.enterMenuScene();
        //}
        //else
        //{
        //    rootgo.SetActive(false);
        //    PlayStory();
        //}
        msglabel.text = "getuser";

        HttpManager.instance.sendConfig(null);
    }

    public void onClick(GameObject go)
    {

        //if (true)
        //{
        //    UIManager.showTip("ddddddd", null, null, null);
        //    return;
        //}
        HttpManager.instance.sendLogin(input.value, input2.value,(code0) =>
        {

            if (code0 == Callback.SUCCESS)
            {
                msglabel.text = "getuser";
                PlayerPrefs.SetString("login_input0", input.value);
                PlayerPrefs.SetString("login_input1", input2.value);

                GameGlobal.inituser();

                HttpManager.instance.sendUserInfo((code1) =>
                {

                    if (code1 == Callback.SUCCESS)
                    {
                        loginover();

                    }

                });
            }

        }, togl_IsClear.value);
    }



    private void OnDestroy()
    {
        storyPlay.ActOnStoryPalyEnd -= OnPlayStoryEnd;
    }

    public void PlayStory()
    {
        if (GameGlobal.gamedata.guideStep < 8)
        {
            storyPlay.PlayStroy();
        }
        else
        {
            GameGlobal.enterMenuScene();
        }
    }
    
    public void OnPlayStoryEnd()
    {
        var userId = input.value;
        PlayerPrefs.SetString("storyMark", "storyMark" + userId);
        Debug.Log("播放完片头动画了=============="+ GameGlobal.gamedata.guideStep);
        //todo  GuideBattle 协议有问题
        // HttpManager.instance.sendGuideBattle((code1) =>
        // {
        //     if (code1 == Callback.SUCCESS)
        //     {
        //         GameGlobal.enterFightScene();
        //     }
        //
        // });
        
        GameGlobal.enterFightScene();
        //FightControl.fb = response.report;
        //FightControl.fbtype = 0;
        //GameGlobal.enterFightScene();
        //播放完之后，直接进特殊战斗
    }
}