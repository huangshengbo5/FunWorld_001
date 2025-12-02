using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager : MonoBehaviour
{
    public static long gettokentime;
    public static string token = "";
    public static int tokenouttime;
    public static int playerid;

    public static HttpManager instance;
    void Awake()
    {
        instance = this;
        //initSendThread();
        //initReviceThread();
    }



    // Start is called before the first frame update
    //void Start()
    //{

    //HttpManager.instance.sendLogin((code0) =>
    //{

    //    if (UISettingPanel.creatroleflag == 0)
    //    {
    //        HttpManager.instance.sendCreate();
    //    }

    //});

    //   sendCreate();

    // TestHttpSend();
    // WWWForm form = new WWWForm();
    // StartCoroutine(Login("http://39.97.188.232:12580/api/v1/perth/login"));

    //StringBuilder sb = new StringBuilder();
    //JsonWriter writer = new JsonWriter(sb);

    //writer.WriteObjectStart();

    //writer.WritePropertyName("device_id");
    //writer.Write("6755345555663322");

    //writer.WritePropertyName("audience");
    //writer.Write("sha");

    //writer.WritePropertyName("os");
    //writer.Write("Android");

    //writer.WritePropertyName("device_model");
    //writer.Write("mi");

    //writer.WriteObjectEnd();
    //Debug.Log(sb.ToString());

    //StartCoroutine(CallPost("http://39.97.188.232:12580/api/v1/perth/login",sb.ToString()));


    //StringBuilder sb = new StringBuilder();
    //JsonWriter writer = new JsonWriter(sb);

    //writer.WriteObjectStart();

    //writer.WritePropertyName("Name");
    //writer.Write("yusong");
    //writer.WritePropertyName("Age");
    //writer.Write(26);
    //writer.WritePropertyName("Girl");
    //writer.WriteArrayStart();
    //writer.WriteObjectStart();
    //writer.WritePropertyName("name");
    //writer.Write("ruoruo");
    //writer.WritePropertyName("age");
    //writer.Write(24);
    //writer.WriteObjectEnd();
    //writer.WriteObjectStart();
    //writer.WritePropertyName("name");
    //writer.Write("momo");
    //writer.WritePropertyName("age");
    //writer.Write(26);
    //writer.WriteObjectEnd();
    //writer.WriteArrayEnd();
    //writer.WriteObjectEnd();
    //Debug.Log(sb.ToString());

    // JsonData jd = JsonMapper.ToObject(sb.ToString());
    //Debug.Log("name = " + (string)jd["Name"]);
    //Debug.Log("Age = " + (int)jd["Age"]);
    //JsonData jdItems = jd["Girl"];
    //for (int i = 0; i < jdItems.Count; i++)
    //{
    //    Debug.Log("Girl name = " + jdItems[i]["name"]);
    //    Debug.Log("Girl age = " + (int)jdItems[i]["age"]);
    //}

    // inittestThread();


    //string url = "http://39.97.188.232:12580/api/v1/perth/player/create";

    //byte[] body = Encoding.UTF8.GetBytes("{\"name\": \"zhangsan5\",\"avatar\": 1}");

    //// Debug.Log(bigurl + request.url);

    //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
    //WebResponse wenReq = null;
    //try
    //{
    //    myRequest.Method = "POST";
    //    myRequest.ContentLength = body.Length;
    //    myRequest.ContentType = "application/json";
    //    myRequest.MaximumAutomaticRedirections = 1;
    //    myRequest.AllowAutoRedirect = true;

    //    token = "eyJhbGciOiJSUzI1NiJ9.eyJzdWIiOiIyMDg4MDM5NzkwMDg2NzU4NDAiLCJhdWQiOiJwZXJ0aCIsImlzcyI6Imh0dHBzOi8vY24uaWxpdmVkYXRhLmNvbS8iLCJpYXQiOjE2MDAzNjE4OTYsImV4cCI6MTYwMDQ0ODI5Nn0.h1E523aa4-5Ky47jdomrfgydJxU7ucmPOkww8v9IzxT_TmJjvy5CllZmhOtHR4c0h0ySyR2-UFL8v65smkQCPA";

    //    // if (request.type != 0)
    //    {
    //        // unityWeb.SetRequestHeader("Authorization", "Bearer" + token);
    //        myRequest.Headers.Add("Authorization", "Bearer " + token);
    //    }

    //    //发送请求
    //    Stream stream = myRequest.GetRequestStream();
    //    stream.Write(body, 0, body.Length);
    //    stream.Close();

    //    //获取接口返回值
    //    //通过Web访问对象获取响应内容
    //    HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
    //    //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
    //    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
    //    //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
    //    string returnXml = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
    //    reader.Close();
    //    myResponse.Close();

    //    Debug.Log(returnXml);
    //}
    //catch (WebException ex) // 这样我们就能捕获到异常，并且获取服务器端的输出
    //{
    //    wenReq = (HttpWebResponse)ex.Response;
    //    using (StreamReader sr = new StreamReader(wenReq.GetResponseStream(), Encoding.UTF8))
    //    {
    //        Debug.Log( "Exception Err:" + ex.Message + ";Err JSON:" + sr.ReadToEnd());
    //    }
    //    myRequest.Abort();
    //}
    //catch (Exception ex)
    //{
    //    //bgTask g = new bgTask();
    //    Debug.Log("Exception Err:" + ex.Message);
    //}


    //}

    public Thread testThread;

    private void inittestThread()
    {
        testThread = new Thread(runtestThread);
        testThread.Start();
    }

    private void runtestThread()
    {
        while (true)
        {
            Thread.Sleep(10);
            Debug.Log("thread");
        }

    }

    /*
    private void sendBaseMessageback(MyRequest request)
    {
       StartCoroutine(CallPost(request));
    }


   


    private IEnumerator CallPost(MyRequest request)
    {
        byte[] body = Encoding.UTF8.GetBytes(request.canshu);
        UnityWebRequest unityWeb = new UnityWebRequest(bigurl + request.url, "POST");
        unityWeb.uploadHandler = new UploadHandlerRaw(body);
        unityWeb.SetRequestHeader("Content-Type", "application/json");

        unityWeb.SetRequestHeader("Authorization", "Bearer"+token);

        unityWeb.downloadHandler = new DownloadHandlerBuffer();

        yield return unityWeb.SendWebRequest();
        //异常处理，很多博文用了error!=null这是错误的，请看下文其他属性部分
        if (unityWeb.isHttpError || unityWeb.isNetworkError)

            Debug.Log(unityWeb.error);

        else
        {
            Debug.Log(unityWeb.downloadHandler.text);

            MyResponse response = new MyResponse(request.msgId, unityWeb.downloadHandler.text,request.type);
            addRevice(response);
        }
    }


    IEnumerator CallPost(string login_url, string jsonDataPost)
    {

        //byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(jsonDataPost);


        //UnityWebRequest webRequest = UnityWebRequest.Post(login_url, jsonDataPost);

        //webRequest.SetRequestHeader("Content-Type", "application/json");


        byte[] body = Encoding.UTF8.GetBytes(jsonDataPost);
        UnityWebRequest unityWeb = new UnityWebRequest(login_url, "POST");
        unityWeb.uploadHandler = new UploadHandlerRaw(body);
        unityWeb.SetRequestHeader("Content-Type", "application/json");
        unityWeb.downloadHandler = new DownloadHandlerBuffer();

        yield return unityWeb.SendWebRequest();
        //异常处理，很多博文用了error!=null这是错误的，请看下文其他属性部分
        if (unityWeb.isHttpError || unityWeb.isNetworkError)

            Debug.Log(unityWeb.error);

        else
        {
            Debug.Log(unityWeb.downloadHandler.text);

            JsonData jd = JsonMapper.ToObject(unityWeb.downloadHandler.text);
            JsonData jdItems = jd["data"];
            Debug.Log("name = " + (string)jdItems["token"]);
            Debug.Log("Age = " + (string)jdItems["playerId"]);
        }

        //Dictionary<string, string> headers = new Dictionary<string, string>();
        //headers["Content-Type"] = "application/json";
        ////将文本转为byte数组  
        //byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(jsonDataPost);
        //Debug.Log("JSON数据：" + bs);

        //WWW www = new WWW(login_url, bs, headers);

        ////等待服务器的响应  
        //yield return www;

        ////如果出现错误  
        //if (www.error != null)
        //{
        //    //获取服务器的错误信息  
        //    Debug.Log(www.error);
        //    yield return null;
        //}
        //else
        //{
        //    //TokenDataInfo tokenDataInfo = JsonMapper.ToObject<TokenDataInfo>(www.text);

        //    //authorization = tokenDataInfo.token_type + " " + tokenDataInfo.access_token;
        //    //Debug.Log(authorization);
        //}

    }
    */

    private static System.Random rd = new System.Random();
    public static int getRandom(int range)
    {
        return rd.Next(range);
    }
    public static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static long currentTimeMillis()
    {
        return (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
    }

    public void sendHeroLevelUp(Hero hero, Reward currencyitem, callbackInt callInt)
    {
        HeroUpgradeRequest request = new HeroUpgradeRequest(hero.id);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            HeroUpgradeResponse response = (HeroUpgradeResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                hero.level = response.level;
                GameGlobal.gamedata.subCurrendy(currencyitem.mainType, currencyitem.val);
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void sendHeroevolveUp(Hero hero, Reward currencyitem, callbackInt callInt)
    {
        HeroEvolveRequest request = new HeroEvolveRequest(hero.id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            HeroEvolveResponse response = (HeroEvolveResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                hero.evolveTimes = response.evolveTimes;
                GameGlobal.gamedata.subCurrendy(currencyitem.mainType, currencyitem.val);
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }
    internal void sendbuyitem(data_storeBean bean, Reward transGet, callbackInt callInt)
    {
        BuyRequest request = new BuyRequest(bean.id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            BuyResponse response = (BuyResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {

                if (response.error)
                {
                    string str = "";
                    if (GameGlobal.gamedata.userinfo.type == 1)
                    {
                        str = "亲爱的用户您好:\n\n"
                        + "根据相关部门对于未成年用户监管要求，该帐号不能在游戏内充值\n\n"
                        + "根据《关于进一步严格管理切实防止未成年人沉迷网络游戏的通知》要求，禁止未成年用户在周五、周六、周日和法定节假日的20: 00 - 21:00外时间登录。网络游戏企业不得为未满8周岁用户提供游戏付费服务。";

                    }
                    else if (GameGlobal.gamedata.userinfo.type == 2)
                    {
                        str = "亲爱的用户您好:\n\n"
    + "抱歉，您的当前帐号不能维续充值~\n\n"
    + "根据《关于进一步严格管理切实防止未成年人沉迷网络游戏的通知》要求，禁止未成年用户在周五、周六、周日和法定节假日的20: 00 - 21:00外时间登录。8周岁以上未满16周岁的用户，单次充值金额不得超过50元人民币，每月充值金额累计不得超过200元人民币。";


                    }
                    else if (GameGlobal.gamedata.userinfo.type == 3)
                    {
                        str = "亲爱的用户您好:\n\n"
    + "抱歉，您的当前帐号不能继续充值 -\n\n"
    + "根据《关于进一步严格管理切实防止未成年人沉迷网络游戏的通知》要求，禁止未成年用户在周五、周六、周日和法定节假日的20: 00 - 21:00外时间登录。16周岁以上未满18周岁的用户，单次充值金额不得超过100元人民币，每月充值金额累计不得超过400元人民币。";


                    }
                    UIManager.showTip(str, null, null, null,null,"充值限制");
                }
                else
                {
                    if (bean.type == 2)
                    {
                        GameGlobal.gamedata.subCurrendy(bean.type, bean.price);
                    }
                    List<Reward> list = new List<Reward>();
                    for (int i = 0; i < response.rewards.Length; i++)
                    {
                        var reward = response.rewards[i];
                        list.Add(reward);
                        GameGlobal.gamedata.addCurrendy(reward.mainType, reward.val);
                    }
                    UIManager.showAwardDlg(list);
                }
               
            }
            else
            {
                //  UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);

            }

        }));


    }


    public void sendBuildingLevelUp(Building build, Reward currencyitem, callbackInt callInt)
    {
        BuildingUpgradeRequest request = new BuildingUpgradeRequest(build.id);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            BuildingUpgradeResponse response = (BuildingUpgradeResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                build.level = response.level;
                GameGlobal.gamedata.subCurrendy(currencyitem.mainType, currencyitem.val);
                sendUserInfo(null);
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void getTeasurehuntInfo(int getType, string adId, callbackInt callInt)
    {
        TeasurehuntInfoRequest request = new TeasurehuntInfoRequest(getType, adId);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            TeasurehuntInfoResponse response = (TeasurehuntInfoResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.userinfo.extend.onlineReward.rewardIDs = response.rewardIDs;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void getTeasurehuntReward(int getType, string adId, callbackInt callInt)
    {
        TeasurehuntRewardGetRequest request = new TeasurehuntRewardGetRequest(getType, adId);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            TeasurehuntRewardGetResponse response = (TeasurehuntRewardGetResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                for (int i = 0; i < response.Rewards.Count; i++)
                {
                    var reward = response.Rewards[i];
                    GameGlobal.gamedata.addCurrendy(reward.mainType, reward.val);
                }
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}
        }));
    }

    public void OffHeroEquip(Equip equipdata, callbackInt callInt)
    {
        OffHeroEauipRequest request = new OffHeroEauipRequest(equipdata.id);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            OffHeroEauipResponse response = (OffHeroEauipResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                GetEquipList(1, 100, null);
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}
        }));
    }

    public void ChangeHeroEquip(Equip equipdata, int selectPos, callbackInt callInt)
    {
        EquipChangeRequest request = new EquipChangeRequest(equipdata.id, selectPos);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            EquipChangeResponse response = (EquipChangeResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                GetEquipList(1, 100, null);
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}
        }));
    }

    public void sendEquipLevelUp(Equip equip, Reward currencyitem, callbackInt callInt)
    {
        EquipUpgradeRequest request = new EquipUpgradeRequest(equip.equipID, equip.id);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            EquipUpgradeResponse response = (EquipUpgradeResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                equip.level++;

                GameGlobal.gamedata.subCurrendy(currencyitem.mainType, currencyitem.val);
                GameGlobal.gamedata.UpdataUserPowerDataAct?.Invoke();
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void sendEquipToPart(List<EquipInfoItem_Data> selectdatalist, callbackInt callInt)
    {
        List<int> partIdslist = new List<int>();
        for (int i = 0; i < selectdatalist.Count; i++)
        {
            partIdslist.Add(selectdatalist[i].bean.id);
        }
        EquipDecomposeRequest request = new EquipDecomposeRequest(partIdslist);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            EquipDecomposeResponse response = (EquipDecomposeResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                List<Reward> rewards = new List<Reward>(); 
                for (int i = 0; i < selectdatalist.Count; i++)
                {
                    var bean = selectdatalist[i].bean;
                    data_equip_upgradeBean nowlevelbean = data_equip_upgradeDef.dicdatas[bean.level][0];

                    Reward partrb = Reward.decode(nowlevelbean.rewards);
                    rewards.Add(partrb);
                    GameGlobal.gamedata.addCurrendy(partrb.mainType, partrb.val);

                    GameGlobal.gamedata.equiplist.Remove(bean);
                }
                    UIManager.showAwardDlg(rewards);
                GameGlobal.gamedata.UpdataUserPowerDataAct?.Invoke();
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
                //UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void sendPartnerShang(Hero hero, callbackInt callInt)
    {
        // GameGlobal.gamedata.selectpartner = hero;
        //callInt?.Invoke(Callback.SUCCESS);
        HeroJoinRequest request = new HeroJoinRequest(hero.id);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            HeroJoinResponse response = (HeroJoinResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.selectpartner = hero;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }
    public void sendPartnerXia(Hero hero, callbackInt callInt)
    {
        //GameGlobal.gamedata.selectpartner = null;
        //callInt?.Invoke(Callback.SUCCESS);
        HeroUnloadRequest request = new HeroUnloadRequest();

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            HeroUnloadResponse response = (HeroUnloadResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.selectpartner = null;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void sendTechnicalLevelUp(Technical tech, Reward currencyitem, callbackInt callInt)
    {
        TechUpgradeRequest request = new TechUpgradeRequest(tech.id);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            TechUpgradeResponse response = (TechUpgradeResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                tech.level = response.level;
             //   Debug.LogError("kou"+currencyitem.val);
                GameGlobal.gamedata.subCurrendy(currencyitem.mainType, currencyitem.val);
                sendUserInfo(null);
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void sendLogin(string name,string password, callbackInt callInt, bool isClearData = false)
    {
        Debug.Log("sendLogin");

        UIManager.showNetLoad("等待中...", 0, 5000, true);

        LoginRequest request = new LoginRequest(name, password, isClearData);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            LoginResponse response = (LoginResponse)backResponse;
            UIManager.hideNetLoad();
            if (response.code == Response.SUCCESS)
            {
                token = response.token;
                playerid = response.uid;
                //UIManager.showToast(response.uid+ " " + response.token);
                isLoginedGet = true;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else if (response.code == 2000)
            //{
            //    string str = "提示信息：\n"

            //    + "未成年人用户可在周五、周六、周日和法定节假日每日晚20时至21时登录游戏，其他时间无法登录游戏。\n"


            //    UIManager.showTip(str, null, null, null, null, "提示信息");
            //}

        }));
    }
    public void sendLogin1(string name, string password,string idcard, callbackInt callInt)
    {
        Debug.Log("sendLogin1");

        UIManager.showNetLoad("等待中...", 0, 5000, true);

        Login1Request request = new Login1Request(name, password, idcard);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            Login1Response response = (Login1Response)backResponse;
            UIManager.hideNetLoad();
            if (response.code == Response.SUCCESS)
            {
                Debug.Log(response.name);
                callInt?.Invoke(Callback.SUCCESS);
            }
        }));
    }
    public void GMIncreatGold(int value)
    {
        GMIncreatGoldRequest request = new GMIncreatGoldRequest(value);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            var response = backResponse;
            MyClient.getInstance().AdjustServerTime(response.sysTime);
            if (response.code == Response.SUCCESS)
            {
                sendUserInfo(null);
            }
        }));
    }

    bool isLoginedGet = false;
    public void sendUserInfo(callbackInt callInt)
    {
        UserInfoRequest request = new UserInfoRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            UserInfoResponse response = (UserInfoResponse)backResponse;
            MyClient.getInstance().AdjustServerTime(response.sysTime);
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.userinfo = response.user;
                GameGlobal.gamedata.userinfo.extend = response.extend;
                GameGlobal.gamedata.stageindex = response.user.extend.campaign.lastCampainID;
                Debug.Log(GameGlobal.gamedata.stageindex + " sendUserInfo " + data_campaignDef.datas.Count);
                if (GameGlobal.gamedata.stageindex>= data_campaignDef.datas.Count)
                {
                    GameGlobal.gamedata.stageindex = data_campaignDef.datas.Count - 1;
                    GameGlobal.gamedata.tongguan = true;
                }
                else
                {
                  // GameGlobal.gamedata.tongguan = false;
                }
            
                GameGlobal.gamedata.guideStep = response.user.guideStep;

                //UIManager.showToast(response.user.level + " " + response.user.name);
                if (response.user.goldIncrement > 0)
                {
                    GameGlobal.gamedata.SetActAutoAddGlod(true);
                }
                else
                {
                    GameGlobal.gamedata.SetActAutoAddGlod(false);
                }
                callInt?.Invoke(Callback.SUCCESS);
                if (isLoginedGet)
                {
                    isLoginedGet = false;
                    //获取玩家相关信息
                    GetHeroList(null);

                    GetBuildingList(null);
                    int techBuildingOpenLevel = data_buildingDef.getdatabybuildid(1006).condition_val;
                    if (techBuildingOpenLevel <= GameGlobal.gamedata.stageindex)//研究院开启
                    {
                        GetTechList(null);
                    }

                    int campBuildingOpenLevel = data_buildingDef.getdatabybuildid(1012).condition_val;
                    if (campBuildingOpenLevel <= GameGlobal.gamedata.stageindex) //探险家营地开启
                    {
                        GetEquipList(1, 100, null);
                    }
                    int stoneOpenLevel = 2;
                    if (stoneOpenLevel <= GameGlobal.gamedata.stageindex)//水晶强化开启
                    {
                        getHeroStoneInfo(null);
                    }
                }
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}
        }));
    }

    public void changeHeadIcon(int headId, callbackInt callInt)
    {
        ChangeHeadRequest request = new ChangeHeadRequest(headId);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            ChangeHeadResponse response = (ChangeHeadResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.userinfo.avatar = headId;
                callInt?.Invoke(Callback.SUCCESS);
            }
        }));
    }

    public void changeName(string name, callbackInt callInt)
    {
        ChangeNameRequest request = new ChangeNameRequest(name);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            ChangeNameResponse response = (ChangeNameResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.userinfo.name = name;
                callInt?.Invoke(Callback.SUCCESS);
            }
        }));
    }


    public void getHeroStoneInfo(callbackInt callInt)
    {
        HeroStoneInfoRequest request = new HeroStoneInfoRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            HeroStoneInfoResponse response = (HeroStoneInfoResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                if (response.Crystals != null)
                {
                    GameGlobal.gamedata.herostonelist.Clear();

                    for (int i = 0; i < response.Crystals.Length; i++)
                    {
                        HeroStoneData item = response.Crystals[i];
                        GameGlobal.gamedata.herostonelist.Add(item);
                    }
                    GameGlobal.gamedata.herostonelist.Sort((a, b) => { return a.CrystalID.CompareTo(b.CrystalID); });
                    callInt?.Invoke(Callback.SUCCESS);
                }
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}
        }));
    }

    public void sendHeroStoneLevelUp(HeroStoneData stone, Reward currencyitem, callbackInt callInt)
    {
        //stone.level += 1;
        //callInt?.Invoke(Callback.SUCCESS);
        //return;
        HeroStoneLevelUpRequest request = new HeroStoneLevelUpRequest(stone.id);

        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            HeroStoneLevelUpResponse response = (HeroStoneLevelUpResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                stone.level = response.level;
                GameGlobal.gamedata.subCurrendy(currencyitem.mainType, currencyitem.val);
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}
        }));
    }

    public void GetHeroList(callbackInt callInt)
    {
        HeroListRequest request = new HeroListRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            HeroListResponse response = (HeroListResponse)backResponse;

            if (response.code == Response.SUCCESS && response.heros != null)
            {
                //GameGlobal.gamedata.partnerlist.Clear();
                for (int i = 0; i < response.heros.Count; i++)
                {
                    Hero item = response.heros[i];
                    item.dbbean = data_heroDef.getdatabyheroid(item.heroID);
                    if (item.type == 1)
                    {
                        GameGlobal.gamedata.mainhero = item;
                    }
                    else
                    {
                        if (item.id == GameGlobal.gamedata.userinfo.subHeroID)
                        {
                            GameGlobal.gamedata.selectpartner = item;
                        }
                        for (int j = 0; j < GameGlobal.gamedata.partnerlist.Count; j++)
                        {
                            if (GameGlobal.gamedata.partnerlist[j].dbbean.id == item.dbbean.id)
                            {
                                GameGlobal.gamedata.partnerlist[j] = item;
                            }
                        }
                        //GameGlobal.gamedata.partnerlist.Add(item);
                    }
                }
                GameGlobal.gamedata.partnerlist.Sort((a, b) => { return a.dbbean.campaign_id.CompareTo(b.dbbean.campaign_id); });
                callInt?.Invoke(Callback.SUCCESS);
                GameGlobal.gamedata.UpdataUserPowerDataAct?.Invoke();
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void GetEquipList(int page, int perPage, callbackInt callInt)
    {
        EquipListRequest request = new EquipListRequest(page, perPage);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            EquipListResponse response = (EquipListResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.equiplist.Clear();
                for (int i = 0; i < response.equips.Count; i++)
                {
                    Equip item = response.equips[i];
                    GameGlobal.gamedata.equiplist.Add(item);
                }
                callInt?.Invoke(Callback.SUCCESS);
                GameGlobal.gamedata.UpdataUserPowerDataAct?.Invoke();
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void GetBuildingList(callbackInt callInt)
    {
        BuildingListRequest request = new BuildingListRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            BuildingListResponse response = (BuildingListResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.buildlist.Clear();
                if (response.buildings != null)
                {
                    for (int i = 0; i < response.buildings.Count; i++)
                    {
                        Building item = response.buildings[i];
                        GameGlobal.gamedata.buildlist.Add(item);
                    }
                }
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void GetTechList(callbackInt callInt)
    {
        TechListRequest request = new TechListRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            TechListResponse response = (TechListResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.technicallist.Clear();
                if (response.techs != null)
                {
                    for (int i = 0; i < response.techs.Count; i++)
                    {
                        Technical item = response.techs[i];
                        GameGlobal.gamedata.technicallist.Add(item);
                    }
                }
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void GetADStartData(callbackInt callInt)
    {
        GetStartADCodeRequest request = new GetStartADCodeRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            GetStartADCodeResponse response = (GetStartADCodeResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.curADServerId = response.adsID;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void sendFight(int id, callbackInt callInt)
    {

        CampaignAttackRequest request = new CampaignAttackRequest(id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            CampaignAttackResponse response = (CampaignAttackResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                response.report.win = response.win;
                response.report.rewards = response.rewards;
                FightControl.fb = response.report;
                FightControl.fbtype = 0;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }

    public void GetFightRwards(int type, string adid, callbackInt callInt)
    {

        CampaignGetRewardsRequest request = new CampaignGetRewardsRequest(type, adid);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            CampaignGetRewardsResponse response = (CampaignGetRewardsResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                //response.report.win = response.win;
                //response.report.rewards = response.rewards;
                //FightControl.fb = response.report;
                //FightControl.fbtype = 0;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }

    public void GetWorldBossRwards(int id, int type, string adid, callbackInt callInt)
    {
        WorldBoosGetRewardsRequest request = new WorldBoosGetRewardsRequest(id, type, adid);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            WorldBoosGetRewardsResponse response = (WorldBoosGetRewardsResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }

    public void sendMonsterFight(Monster m, callbackInt callInt)
    {

        MonsterAttackRequest request = new MonsterAttackRequest(m.index);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            MonsterAttackResponse response = (MonsterAttackResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                response.report.win = response.win;
                response.report.rewards = response.rewards;
                FightControl.fb = response.report;
                FightControl.fbtype = 1;
                FightControl.monster = m;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }
    public void sendMonsterReceive(int id, callbackInt callInt)
    {
        MonsterReceiveRequest request = new MonsterReceiveRequest(id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            MonsterReceiveResponse response = (MonsterReceiveResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}
        }));
    }
    public void sendMonsterShow(callbackInt callInt)
    {

        MonsterShowRequest request = new MonsterShowRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            MonsterShowResponse response = (MonsterShowResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                for (int i = 0; i < response.monsters.Count; i++)
                {
                    response.monsters[i].index = i;
                }
                WorldLayer.instance.monsters = response.monsters;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }
    public void sendConfig(callbackInt callInt)
    {
        SystemConfigRequest request = new SystemConfigRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            SystemConfigResponse response = (SystemConfigResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                //地图怪物入侵时间配置
                GameGlobal.gamedata.towerfromtolist.Clear();
                GameGlobal.gamedata.towerfromtolist.Add(response.towerSettings.additionalProp1);
                GameGlobal.gamedata.towerfromtolist.Add(response.towerSettings.additionalProp2);
                GameGlobal.gamedata.towerfromtolist.Add(response.towerSettings.additionalProp3);
                GameGlobal.gamedata.towerfromtolist.Add(response.towerSettings.additionalProp4);
                GameGlobal.gamedata.towerfromtolist.Add(response.towerSettings.additionalProp5);
                GameGlobal.gamedata.towerfromtolist.Add(response.towerSettings.additionalProp6);
                GameGlobal.gamedata.towerfromtolist.Add(response.towerSettings.additionalProp7);
                GameGlobal.gamedata.towerfromtolist.Add(response.towerSettings.additionalProp8);
                GameGlobal.gamedata.towerConfigGetTime = response.sysTime;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }

    //public void sendCreate()
    //{
    //    MyNetCallback nc = new MyNetCallback((MyResponse response) =>
    //    {

    //        if (response.code == 0)
    //        {
    //            UISettingPanel.creatroleflag = 1;
    //            UIController.save();
    //        }
    //        else
    //        {

    //        }
    //    });

    //    MyRequest request = new MyRequest(1, nc.callbackId);
    //    sendMessage(request, nc);
    //}
    //public void sendName()
    //{
    //    MyNetCallback nc = new MyNetCallback((MyResponse response) =>
    //    {

    //        if (response.code == 0)
    //        {

    //        }
    //        else
    //        {

    //        }
    //    });

    //    MyRequest request = new MyRequest(2, nc.callbackId);
    //    sendMessage(request, nc);
    //}
    //public void sendAvatar()
    //{
    //    MyNetCallback nc = new MyNetCallback((MyResponse response) =>
    //    {

    //        if (response.code == 0)
    //        {

    //        }
    //        else
    //        {

    //        }
    //    });

    //    MyRequest request = new MyRequest(3, nc.callbackId);
    //    sendMessage(request, nc);
    //}
    //public void sendScore()
    //{
    //    MyNetCallback nc = new MyNetCallback((MyResponse response) =>
    //    {

    //        if (response.code == 0)
    //        {

    //        }
    //        else
    //        {

    //        }
    //    });

    //    MyRequest request = new MyRequest(4, nc.callbackId);
    //    sendMessage(request, nc);
    //}
    //public void sendRank(webcallback callObject)
    //{
    //    //{ "message":"Success.","data":{ "leaderboardItems":[],"rank":null,"score":null},"return_code":0}
    //    Debug.Log("sendRank");
    //    MyNetCallback nc = new MyNetCallback((MyResponse response) =>
    //    {

    //        if (response.code == 0)
    //        {
    //            Debug.Log(response.content);



    //            JsonData jd = JsonMapper.ToObject(response.content);
    //            JsonData jdItems = jd["data"];


    //            JsonData items = jdItems["leaderboardItems"];

    //         //   int mRankId = (int)jdItems["rank"];
    //         //   int mRankScore = (int)jdItems["score"];

    //            GameController.Instance.sectionManager.robotmsgList.Clear();
    //         //   Debug.LogError(items.Count);
    //            for (int i = 0; i < items.Count; i++)
    //            {
    //                robotmsg bean = new robotmsg();
    //                bean.RankId = (int)items[i]["rank"];
    //                bean.RankScore = Convert.ToInt32(((double)items[i]["score"]));
    //                bean.RankName = (string)items[i]["name"];
    //                bean.PortraitId = (int)items[i]["avatar"];
    //                if (bean.PortraitId >= 20)
    //                {
    //                    bean.PortraitId = bean.PortraitId%20;
    //                }
    //                string id= (string)items[i]["id"];

    //                if (id != playerid)
    //                {
    //                    GameController.Instance.sectionManager.robotmsgList.Add(bean);
    //                }
    //                else
    //                {
    //                  //  Debug.LogError(bean.RankId);
    //                }

    //            }

    //            if (callObject != null)
    //                callObject(0);
    //        }
    //        else
    //        {
    //            EffLayer.instance.msgbox("网络异常");
    //        }
    //    });

    //    MyRequest request = new MyRequest(5, nc.callbackId);
    //    sendMessage(request, nc);

    //}
    //public void sendConfig(webcallback callObject)
    //{
    //    //{ "message":"Success.","data":{ "leaderboardItems":[],"rank":null,"score":null},"return_code":0}
    //    Debug.Log("sendConfig");
    //    MyNetCallback nc = new MyNetCallback((MyResponse response) =>
    //    {

    //        if (response.code == 0)
    //        {
    //            Debug.Log(response.content);

    //            JsonData jd = JsonMapper.ToObject(response.content);
    //            JsonData jdItems = jd["data"];
    //            bool token = (bool)jdItems["isAppStoreReview"];

    //            if (token)
    //            {
    //                GameGlobal.ping = 1;
    //            }
    //            else
    //            {
    //                GameGlobal.ping = 0;

    //                UIController.save();
    //            }

    //            if (callObject != null)
    //                callObject(0);
    //        }
    //        else
    //        {
    //          //  EffLayer.instance.msgbox("network error");
    //            if (callObject != null)
    //                callObject(404);
    //        }
    //    });

    //    MyRequest request = new MyRequest(6, nc.callbackId);
    //    sendMessage2(request, nc);

    //}



    public void ArenaSignup(callbackInt callInt)
    {
        ArenaSignupRequest request = new ArenaSignupRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            ArenaSignupResponse response = (ArenaSignupResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                if (response.arenaIsSign)
                {
                    UIManager.showToast("报名成功");
                    GameGlobal.gamedata.userinfo.extend.arenaissign = response.arenaIsSign;
                }
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void ArenaList(callbackInt callInt)
    {
        ArenaListRequest request = new ArenaListRequest(1, 200);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            ArenaListResponse response = (ArenaListResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {

                GameGlobal.gamedata.pageInfo = response.pageInfo;
                GameGlobal.gamedata.rankers.Clear();
                GameGlobal.gamedata.records.Clear();
                GameGlobal.gamedata.self = response.self;
                for (int i = 0; i < response.rankers.Count; i++)
                {
                    GameGlobal.gamedata.rankers.Add(response.rankers[i]);
                }
                if (response.records != null)
                {
                    for (int i = 0; i < response.records.Count; i++)
                    {
                        GameGlobal.gamedata.records.Add(response.records[i]);
                    }
                }
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void ArenaAttack(int id, callbackInt callInt)
    {
        ArenaAttackRequest request = new ArenaAttackRequest(id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            ArenaAttackResponse response = (ArenaAttackResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                response.report.win = response.win;
                response.report.rewards = response.rewards;
                FightControl.fb = response.report;
                FightControl.fbtype = 4;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void MailList(callbackInt callInt)
    {
        MailListRequest request = new MailListRequest(1, 100);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            MailListResponse response = (MailListResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.mailPageInfo = response.pageInfo;
                GameGlobal.gamedata.mails.Clear();
                for (int i = 0; i < response.mails.Count; i++)
                {
                    GameGlobal.gamedata.mails.Add(response.mails[i]);
                }
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void MailRead(int id, callbackInt callInt)
    {
        MailReadRequest request = new MailReadRequest(id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            MailReadResponse response = (MailReadResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                for (int i = 0; i < GameGlobal.gamedata.mails.Count; i++)
                {
                    GameGlobal.gamedata.mails[i].status = 1;//状态已读
                }
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void MailReceive(int id, callbackInt callInt)
    {
        MailReceiveRequest request = new MailReceiveRequest(id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            MailReceiveResponse response = (MailReceiveResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                for (int i = 0; i < GameGlobal.gamedata.mails.Count; i++)
                {
                    if (GameGlobal.gamedata.mails[i].id == id)
                    {
                        GameGlobal.gamedata.mails[i].hasReceived = 1;
                    }
                }
                List<Reward> rewards = new List<Reward>();
                for (int i = 0; i < response.rewards.Length; i++)
                {
                    rewards.Add(response.rewards[i]);
                }
                UIManager.showAwardDlg(rewards);
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void MailReceiveAll(callbackInt callInt)
    {
        MailReceiveAllRequest request = new MailReceiveAllRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            MailReceiveAllResponse response = (MailReceiveAllResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {

                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }
    public void MailRemoveAll(callbackInt callInt)
    {
        MailRemoveAllRequest request = new MailRemoveAllRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            MailRemoveAllResponse response = (MailRemoveAllResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {

                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void TianqiShow(callbackInt callInt)
    {
        TianqiShowRequest request = new TianqiShowRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            TianqiShowResponse response = (TianqiShowResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.apocalypseID = response.apocalypseID;
                GameGlobal.gamedata.remainNum = response.remainNum;
                GameGlobal.gamedata.status = response.status;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void TianqiAttack(int id, int type, string ADIdstr, callbackInt callInt)
    {
        TianqiAttackRequest request = new TianqiAttackRequest(id, type, ADIdstr);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            TianqiAttackResponse response = (TianqiAttackResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.players.Clear();
                for (int i = 0; i < response.players.Length; i++)
                {
                    GameGlobal.gamedata.players.Add(response.players[i]);
                }
                if (response.report.Length > 0)
                {
                    FightControl.fb = response.report[0];
                    FightControl.fbtype = 5;
                }
                GameGlobal.gamedata.tianqiReport.Clear();
                for (int i = 0; i < response.report.Length; i++)
                {
                    GameGlobal.gamedata.tianqiReport.Add(response.report[i]);
                }
                GameGlobal.gamedata.tianqiWin = response.win;
                GameGlobal.gamedata.tianqiRewards.Clear();
                for (int i = 0; i < response.rewards.Length; i++)
                {
                    GameGlobal.gamedata.tianqiRewards.Add(response.rewards[i]);
                }
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void TianqiAward(int id, callbackInt callInt)
    {
        TianqiAwardRequest request = new TianqiAwardRequest(id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            TianqiAwardResponse response = (TianqiAwardResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.tianqiRewards.Clear();
                for (int i = 0; i < response.rewards.Length; i++)
                {
                    GameGlobal.gamedata.tianqiRewards.Add(response.rewards[i]);
                }
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void GetHistoryInfo(callbackInt callInt)
    {
        HistoryInfoRequest request = new HistoryInfoRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            HistoryInfoResponse response = (HistoryInfoResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.historyInfoData = response;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void GetHistoryRewards(int id, callbackInt callInt)
    {
        HistoryRewardsRequest request = new HistoryRewardsRequest(id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            HistoryRewardsResponse response = (HistoryRewardsResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GetHistoryInfo(callInt);
                List<Reward> list = new List<Reward>();
                for (int i = 0; i < response.rewards.Length; i++)
                {
                    var reward = response.rewards[i];
                    list.Add(reward);
                    GameGlobal.gamedata.addCurrendy(reward.mainType, reward.val);
                }
                UIManager.showAwardDlg(list);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void EnergeTransfor(int type = 0, string adid = "", callbackInt callInt = null)
    {
        EnergeTransforRequest request = new EnergeTransforRequest(type, adid);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            EnergeTransforResponse response = (EnergeTransforResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                sendUserInfo(callInt);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void ClearEnergeTansCD(callbackInt callInt)
    {
        ClearEnergeTansCDRequest request = new ClearEnergeTansCDRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            ClearEnergeTansCDResponse response = (ClearEnergeTansCDResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                sendUserInfo(callInt);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void sendGuideBattle(callbackInt callInt)
    {
        GuideBattleRequest request = new GuideBattleRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            GuideBattleResponse response = (GuideBattleResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                response.report.win = response.win;
                //response.report.rewards = response.rewards;
                FightControl.fbtype = 6;
                FightControl.fb = response.report;
                FightControl.fb.type = 6;
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }

    public void sendGuideStep(int step, callbackInt callInt)
    {
        GuideStepRequest request = new GuideStepRequest(step);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            GuideStepResponse response = (GuideStepResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.guideStep = response.step;

                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }

    public void GetBuffGoldAdd(string aDIdstr, callbackInt callInt)
    {
        BuffGoldAddRequest request = new BuffGoldAddRequest(aDIdstr);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            BuffGoldAddResponse response = (BuffGoldAddResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.userinfo.gold = response.gold;
                GameGlobal.gamedata.userinfo.goldBuffEndIn = response.goldBuffEndIn;
               // Debug.LogError("time=" + response.goldBuffEndIn);
                GameGlobal.gamedata.userinfo.goldFlushIn = response.goldFlushIn;
                GameGlobal.gamedata.userinfo.goldIncrement = response.goldIncrement;
                callInt?.Invoke(Callback.SUCCESS);
                //sendUserInfo(callInt);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));

    }

    public void GetTradeGoods(int getType, string aDIdstr, callbackInt callInt)
    {
        GetTradeGoodsRequest request = new GetTradeGoodsRequest(getType, aDIdstr);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            GetTradeGoodsResponse response = (GetTradeGoodsResponse)backResponse;
            if (response.code == Response.SUCCESS)
            {
                List<Reward> rewards = new List<Reward>();
                for (int i = 0; i < response.rewards.Length; i++)
                {
                    var reward = response.rewards[i];
                    GameGlobal.gamedata.addCurrendy(reward.mainType, reward.val);
                    rewards.Add(reward);
                }
                UIManager.showAwardDlg(rewards);
                callInt?.Invoke(Callback.SUCCESS);
                //sendUserInfo(callInt);
            }
            //else
            //{
            //    UIManager.showToast(response.code + " " + response.msg + " \n" + response.GetType().Name);
            //}

        }));
    }

    public void GetTianxianInfo(Action<GetTianxianInfoResponse> p)
    {
        GetTianxianInfoRequest request = new GetTianxianInfoRequest();
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            GetTianxianInfoResponse response = (GetTianxianInfoResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                p?.Invoke(response);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }

    public void GetTianxianReward(int id, callbackInt callInt)
    {
        GetTianxianRewardsRequest request = new GetTianxianRewardsRequest(id);
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            GetTianxianRewardsResponse response = (GetTianxianRewardsResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                GameGlobal.gamedata.addCurrendy(response.rewards[0].mainType, response.rewards[0].val);
                UIManager.showAwardDlg( new List<Reward>() { response.rewards[0] });
                callInt?.Invoke(Callback.SUCCESS);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }

    public void SetTianxianHeros(int curId, List<int> selectedHeroIds, Action<SetTianxianHerosResponse> p)
    {
        SetTianxianHerosRequest request = new SetTianxianHerosRequest(curId, selectedHeroIds.ToArray());
        MyClient.getInstance().sendMessage(request, NetCallback.create((Response backResponse) =>
        {
            SetTianxianHerosResponse response = (SetTianxianHerosResponse)backResponse;

            if (response.code == Response.SUCCESS)
            {
                if (response.rewards != null&& response.rewards.Length >0 )
                {
                    GameGlobal.gamedata.addCurrendy(response.rewards[0].mainType, response.rewards[0].val);
                    UIManager.showAwardDlg(new List<Reward>() { response.rewards[0] });
                }
                p?.Invoke(response);
            }
            //else
            //{
            //    UIManager.showToast(response.msg + " " + response.GetType().Name);
            //}

        }));
    }

    private void OnDestroy()
    {
        Debug.Log("destory");
        //threadover = true;
    }
}