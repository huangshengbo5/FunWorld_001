using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGlobal : MonoBehaviour
{
    public static GameGlobal instance;
    public static GameObject loadgen = null;

    public LoadingNode loadingnode;


    void Awake()
    {
        // Debug.Log(PlatformInterface.getInstance().getDeviceInfo());

        instance = this;

        loadAll();



        initall();

        initloadnode();

        loadInfo();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = CTGlobal.FPS;


        CTGlobal.realHeight = CTGlobal.scrHeight;
        CTGlobal.realWidth = Screen.width * CTGlobal.scrHeight / Screen.height;
        //  if (Mathf.Abs(Screen.height * 1.0f / Screen.width - 0.75f) < 0.01f)
        if (Screen.width * 1.0f / Screen.height < CTGlobal.scrWidth * 1.0f / CTGlobal.scrHeight)
        {
            CTGlobal.ispad = true;
            CTGlobal.realWidth = CTGlobal.scrWidth;
            CTGlobal.realHeight = Screen.height * CTGlobal.scrWidth / Screen.width;
        }


        TimeSpan date = new Date().getTimeZoneOffset();
        int timeOffset = (int)date.TotalSeconds;
        CTGlobal.timeZone = timeOffset / 3600;// 除以小时毫秒数
                                              //   Debug.LogError("本地时区=" + timeOffset + "    " + CTGlobal.timeZone);

    }
    private void initall()
    {
        GameObject preobj = ResManager.getGameObject("allpre", "ui/otherroot");
        preobj.name = "globaluiroot";
        preobj.transform.localPosition = new Vector3(-preobj.transform.localPosition.x * 2, preobj.transform.localPosition.y * 2, preobj.transform.localPosition.z);
        preobj.GetComponentInChildren<Camera>().depth = 20;
        //   preobj.SetActive(false);
        loadgen = preobj;
        DontDestroyOnLoad(loadgen);

        DontDestroyOnLoad(gameObject);
        gameObject.AddComponent<PopInput>();
        gameObject.AddComponent<HttpManager>();
      //  gameObject.AddComponent<SDKManager>();
        gameObject.AddComponent<Loom>();
        gameObject.AddComponent<MusicPlayer>();
        gameObject.AddComponent<SoundPlayer>();
        gameObject.AddComponent<VoicePlayer>();
    }

    //public static void setGameData(GameData gamedata)
    //{
    //    user.setData(gamedata);
    //}

    public void initloadnode()
    {
        if (loadingnode == null)
        {
            GameObject preobj = ResManager.getGameObject("allpre", "loadingnode");
            preobj.name = "globaluiroot";
            preobj.transform.parent = loadgen.transform;
            preobj.transform.localScale = Vector3.one;
            preobj.transform.localPosition = Vector3.zero;
            preobj.SetActive(false);
            loadingnode = preobj.GetComponent<LoadingNode>();
        }


    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    OnPointerDown();
        //}
        //if (Input.GetMouseButton(0))
        //{
        //    OnDrag();
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    OnPointerUp();
        //}

        if (Input.GetKeyDown(KeyCode.F))
        {
            string filepath = UTools.openFileDialog();
            Debug.Log(filepath);
        }

        if (timer < 1)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SecondTimerCall?.Invoke();
            timer = 0;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {

            StartCoroutine(UTools.CutScreenCapture(0, 0, CTGlobal.realWidth, CTGlobal.scrHeight));
        }
    }
    float timer = 0;
    public Action SecondTimerCall;

    //void LateUpdate()
    //{
    //   //updatetp();
    //}

    public static void updatetp()
    {
        //if (tp != null)
        //{
        //    tp.apply();
        //}
    }
    /*
    private EffClickScreen drawShow;
    private Vector2 pressPoint = new Vector2();
    public void OnPointerDown()
    {


        Vector3 position = Input.mousePosition;
        position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
        if (lastcam != null)
        {
           // Debug.Log(lastcam.name + " " + UICamera.currentCamera.name);
        }
     //   Debug.Log(UICamera.currentCamera.name);
        position = UICamera.currentCamera.transform.InverseTransformPoint(position);

        if (drawShow == null)
        {
            drawShow = UIManager.showClickScreenEff();

        }
        drawShow.showDown();

        pressPoint = new Vector2(position.x, position.y);

        lastcam = UICamera.currentCamera;
    }

    public void OnPointerUp()
    {
        if (drawShow != null)
        {
            drawShow.End();
        }
    }
    public Camera lastcam = null;
    public void OnDrag()
    {
        Vector3 position = Input.mousePosition;
        position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
        position = UICamera.currentCamera.transform.InverseTransformPoint(position);
        if (drawShow == null && Vector2.Distance(pressPoint, new Vector2(position.x, position.y)) > 0.03f)
        {
            drawShow = UIManager.showClickScreenEff();
        }


        drawShow.showDrag();

        if (lastcam != UICamera.currentCamera)
        {
            drawShow.End();
            lastcam = UICamera.currentCamera;
        }

        // Debug.Log(position+" "+ UICamera.currentCamera);
        //lastcam = UICamera.currentCamera;
    }
    */

    //public static UserBean user;
    //public static void inituser()
    //{
    //    GameData gamedata = new GameData();
    //    user = new UserBean();
    //    user.setData(gamedata);
    //}

    public static bool loadover = false;
    public static void loadAll()
    {
        if (loadover)
        {
            return;
        }
        CTGlobal.initRoot();



        // SActionGroup.getAGFromBuffer("ui_effect", "ui_effect");

        loadDef(false);
        LocalStorage.readInfo();



        inituser();

        loadover = true;
    }
    public static GameData gamedata;
    public static void inituser()
    {
        gamedata = new GameData();
    }
    public static void loadDef(bool v)
    {
        Debug.Log("loaddef");

        //if (true)
        //{
        //    // return;
        //}
        data_resourcesDef.load();
        data_buildingDef.load();
        data_building_upgradeDef.load();
        data_scienceDef.load();
        data_science_levelDef.load();
        data_equip_upgradeDef.load();
        data_equipDef.load();
        data_online_rewardDef.load();
        data_hero_evolveDef.load();
        data_hero_upgradeDef.load();
        data_heroDef.load();
        data_npcDef.load();
        data_equip_skillDef.load();

        data_fight_positionDef.load();
        data_buff_comparisonDef.load();

        data_skillDef.load();
        data_skill_levelDef.load();

        data_storeDef.load();

        data_towerDef.load();

        data_campaignDef.load();
        data_stageDef.load();
        data_arena_rewardDef.load();
        data_mailDef.load();
        data_apocalypseDef.load();

        StagePointDef.load();

        data_dialogueDef.load();
        help_dialogueDef.load();

        data_crystal_upgradeDef.load();
        data_alchemyDef.load();//炼丹
        data_annalsDef.load();
        data_guidesDef.load();

        data_avatarDef.load();
        data_exploreDef.load();

        SensitiveWords.load();

    }

    public static void enterFightScene(bool isNeedLoad = true)
    {
        UIViewGroup.removeAllDlg();
        if (isNeedLoad)
        {
            instance.loadingnode.setstart(() =>
            {

            });

            instance.loadingnode.setend(() =>
            {
                string loadName = "Scenes/fight";
                SceneManager.LoadScene(loadName);
            });
        }
        else
        {
            string loadName = "Scenes/fight";
            SceneManager.LoadScene(loadName);
        }
    }
    public static bool fromfightflag = false;
    public static bool fromArenaFight = false;
    public static bool fromTianqiFight = false;
    public static bool fromMonsterFight = false;


    public static void enterMenuScene(bool fromfight = false)
    {
        fromfightflag = fromfight;
        UIViewGroup.removeAllDlg();
        instance.loadingnode.setstart(() =>
        {
            string loadName = "Scenes/menu";
            SceneManager.LoadScene(loadName);

        }, 10);

        instance.loadingnode.setend(() =>
        {

        });
    }

    public static void enterCoverScene(bool isNeedLoad = true)
    {
        if (SceneManager.GetActiveScene().name == "cover")
        {
            return;
        }
        MusicPlayer.getInstance().Stop();
        UIViewGroup.removeAllDlg();
        if (isNeedLoad)
        {
            instance.loadingnode.setstart(() =>
            {

            });

            instance.loadingnode.setend(() =>
            {
                string loadName = "Scenes/cover";
                SceneManager.LoadScene(loadName);
            });
        }
        else
        {
            string loadName = "Scenes/cover";
            SceneManager.LoadScene(loadName);
        }
    }

    public static long serverTimeAdjust;
    //取得系统时间，此方法需要增加一步与服务器同步的计算
    public static long getCurrTimeMM()
    {
        long time = UTools.currentTimeMillis()/1000 + serverTimeAdjust;
        return time;
    }

    public static void setServerTime(long serverTime)
    {
     //   Debug.LogError(serverTime + "    " + UTools.currentTimeMillis()/1000);
        serverTimeAdjust = serverTime - UTools.currentTimeMillis()/1000;
    }

    void OnDestroy()
    {
        // MyClient.stopThread();
        // ClientHelper.getInstance().closeAll();
        saveInfo();
    }

    public static bool Bgm_Switch = true;
    public static bool Snd_Switch = true;
    public static bool fireworksOn = true;
    public static void saveInfo()
    {
        PlayerPrefs.SetInt("Bgm_Switch", Bgm_Switch ? 1 : 0);
        PlayerPrefs.SetInt("Snd_Switch", Snd_Switch ? 1 : 0);
        PlayerPrefs.SetInt("fireworksOn", fireworksOn ? 1 : 0);
    }
    public static void loadInfo()
    {
        Bgm_Switch= (PlayerPrefs.GetInt("Bgm_Switch", 1) == 1);
        Snd_Switch = (PlayerPrefs.GetInt("Snd_Switch", 1) == 1);
        fireworksOn = (PlayerPrefs.GetInt("fireworksOn", 1) == 1);

        MusicPlayer.setPlayering(Bgm_Switch);
        SoundPlayer.setPlayering(Snd_Switch);
    }

    /*
    public string nextSceneName;
    private AsyncOperation async = null;
    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            //if (async.progress < 0.9f)
            //    progressValue = async.progress;
            //else
            //    progressValue = 1.0f;

            //slider.value = progressValue;
            //progress.text = (int)(slider.value * 100) + " %";

            //if (progressValue >= 0.9)
            //{
            //    progress.text = "按任意键继续";
            //    if (Input.anyKeyDown)
            //    {
            //        async.allowSceneActivation = true;
            //    }
            //}

            Debug.Log(async.progress);

            yield return null;
        }

    }
    */
}
