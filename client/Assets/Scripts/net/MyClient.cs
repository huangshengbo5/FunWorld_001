using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class MyClient
{
    private static List<NetCallback> callbacks = new List<NetCallback>();
    private static List<Response> reviceResponse = new List<Response>();
    private static List<Request> sendRequest0 = new List<Request>();

    private static MyClient instance;
    public static MyClient getInstance()
    {
        if (instance == null)
        {
            instance = new MyClient();
        }

        return instance;
    }

    public MyClient()
    {
        initSendThread();
        initReviceThread();
    }


    private void sendMessage2(Request request, NetCallback nc)
    {
        Debug.Log("sendMessage2" + request.msgId);
        addRequestMain(request);
        addCallback(nc, request.msgId);
    }
    public void sendMessage(Request request, NetCallback nc)
    {
        //if (playerid==null||currentTimeMillis() > gettokentime + tokenouttime * 1000)
        //{
        //    sendLogin((int code) =>
        //    {
        //        if (code == 0)
        //        {
        //            sendMessage2(request, nc);
        //        }
        //        else
        //        {
        //            EffLayer.instance.msgbox("网络异常");
        //        }

        //    });
        //    return;
        //}

        sendMessage2(request, nc);
    }




    private static readonly object sendLockObject = new object();
    private static void addRequestMain(Request request)
    {

        lock (sendLockObject)
        {
            try
            {
                sendRequest0.Add(request);
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
        }
    }
    private static readonly object callBackLockObject = new object();
    private static void addCallback(NetCallback callback, int mid)
    {
        lock (callBackLockObject)
        {
            callback.callbackId = mid;
            callbacks.Add(callback);
        }

    }

    public static bool threadover = false;
    public static Thread sendThread;

    private static void initSendThread()
    {
        sendThread = new Thread(runSendThread);
        sendThread.Start();
    }
    private static void runSendThread()
    {
        while (!threadover)
        {
            try
            {
                lock (sendLockObject)
                {
                    if (sendRequest0.Count > 0)
                    {
                        Request request = sendRequest0[0];
                        sendBaseMessage(request);
                        sendRequest0.RemoveAt(0);

                        //释放该协议
                        if (request != null)
                        {
                            request = null;
                        }
                    }
                }

                Thread.Sleep(10);///报错
                //    Debug.Log("send");
            }
            catch (Exception e)
            {
                Debug.LogError("client runSendThread=" + e.StackTrace);
            }
        }
    }
    public static Thread reviceThread;
    private static void initReviceThread()
    {
        reviceThread = new Thread(runReviceThread);
        reviceThread.Start();
    }

    public long serverTime;
    public int serverOffTime = 0;
    public void AdjustServerTime(int msgservertime)
    {
        serverTime = msgservertime + serverOffTime;
    }

    private static void runReviceThread()
    {
        while (!threadover)
        {
            try
            {
                lock (reviceLockObject)
                {
                    if (reviceResponse.Count > 0)
                    {
                        Response response = reviceResponse[0];
                        reviceResponse.RemoveAt(0);

                        notifyCallback(response);


                        //释放该协议
                        if (response != null)
                        {
                            response = null;
                        }

                    }
                }

                Thread.Sleep(10);//报错
                //  Debug.Log("Revice");
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace + "\n" + e.Message);
            }
        }

    }

    private static void notifyCallback(Response response)
    {
        for (int i = 0; i < callbacks.Count; i++)
        {
            NetCallback callback = callbacks[i];
            if (callback.callbackId == response.msgId)
            {

                Loom.RunAsync(() =>
                {
                    Loom.QueueOnMainThread(() =>
                    {

                        string tip = "reponse>>> " + response.ToString() + "  " + response.msgId + "==============================" + response.code;
                        Debug.Log(tip);

                        lock (callBackLockObject)
                        {
                            if (response.code == Response.SUCCESS)
                            {
                                callback.callback(response);
                            }
                            else
                            {
                                //if(response is BuyResponse)
                                //{
                                //    callback.callback(response);
                                //}

                                if (response.code == 1033 || response.code == 1003)
                                {
                                    UIManager.showToast("您已经断开连接，请重新登录!");
                                    return;
                                }
                                else if (response.code == 2000)
                                {
                                    string str = "\n"

                                    + "未成年人用户可在周五、周六、周日和法定节假日每日晚20时至21时登录游戏，其他时间无法登录游戏。\n";
                                    UIManager.showTip(str, null, null,  (int code) =>
                                    {
                                        GameGlobal.enterCoverScene();

                                    }, null, "提示信息");

                                  

                                    return;
                                }
                                Debug.LogError(response.code + " " + response.msg + " \n" + response.GetType().Name);
                                UIManager.showToast( response.msg );
                            }

                            callbacks.Remove(callback);
                            if (callback != null)
                            {
                                callback = null;
                            }
                        }

                    });
                });

            }
        }
    }

    // public static string bigurl = "http://182.92.5.29:8080";
    
    public static string bigurl = "http://127.0.0.1:8080";


    private static readonly object reviceLockObject = new object();
    private static void addRevice(Response response)
    {
        lock (reviceLockObject)
        {
            try
            {
                reviceResponse.Add(response);
            }
            catch (Exception e)
            {
                Debug.LogError("client revice=" + response.msgId + "  " + e.StackTrace);
            }

        }
    }
    public static void sendBaseMessage(Request request)
    {

        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(bigurl + request.serviceName);
        WebResponse wenReq = null;
        try
        {
            string bodyjson = XLH.getJson(request);
            byte[] body = Encoding.UTF8.GetBytes(bodyjson);

            Debug.Log(bigurl + request.serviceName);



            myRequest.Method = "POST";
            myRequest.ContentLength = body.Length;
            myRequest.ContentType = "application/json";
            myRequest.MaximumAutomaticRedirections = 1;
            myRequest.AllowAutoRedirect = true;

            if (HttpManager.token != "")
            {
                myRequest.Headers.Add("GameUid", HttpManager.playerid + "");
                myRequest.Headers.Add("GameToken", HttpManager.token);
                myRequest.Headers.Add("SeqId", request.msgId + "");
            }

            Debug.Log("sendBaseMessage:" + bodyjson);

            //发送请求
            Stream stream = myRequest.GetRequestStream();
            stream.Write(body, 0, body.Length);
            stream.Close();

            Debug.Log("sendBaseMessage");

            //获取接口返回值
            //通过Web访问对象获取响应内容
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
            string returnXml = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
            reader.Close();
            myResponse.Close();

            Debug.Log(returnXml);

            // Response response = new Response(request.msgId, returnXml, request.type);
            Response response = XLH.readToResponse(returnXml, request);
            addRevice(response);
        }
        catch (WebException ex) // 这样我们就能捕获到异常，并且获取服务器端的输出
        {
            //wenReq = (HttpWebResponse)ex.Response;
            //using (StreamReader sr = new StreamReader(wenReq.GetResponseStream(), Encoding.UTF8))
            //{
            //    Debug.Log("Exception Err:" + ex.Message + ";Err JSON:" + sr.ReadToEnd());
            //}

            Response response = new Response(404, request.msgId);
            addRevice(response);

            Debug.Log("Exception Err:" + ex.Message);
            myRequest.Abort();
        }
        catch (Exception ex)
        {
            //bgTask g = new bgTask();
            Debug.Log("Exception Err:" + ex.Message);
            // myRequest.Abort();
        }

    }

}
