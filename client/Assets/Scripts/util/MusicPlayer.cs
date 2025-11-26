using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;

[RequireComponent(typeof(AudioListener))]
public class MusicPlayer : MonoBehaviour{

    public string musicPath = "Audio/";

    public static float volume = 1.0f;
    public static bool isPlayering = true;

    private AudioMixer audioMixer;

    private AudioSource audioSource;
	private AudioClip ac;

    private List<AudioClip> clipList = new List<AudioClip>();



    private static MusicPlayer instance = null;
    public static MusicPlayer getInstance()
	{
        return instance;
	}

    void Awake()
    {
        instance = this;
        ac = null;
        clipList.Clear();
        audioSource = this.gameObject.AddComponent<AudioSource>();

        //加入混音
        addAudioMixer();
    }

    public void Play(string fileName, bool loop)
    {
        Play(fileName, loop, null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName">文件+路径名</param>
    /// <param name="loop">是否循环播放</param>
    /// <param name="usePool">是否缓存起来   TODO 需要测试内容是否可自动回收未加到List中的AudioClip</param>
    public void Play(string fileName, bool loop,/* bool usePool, */callbackInt callFinish = null)
    {
        if (isPlayering == false) return;
        

        if(ac != null && !ac.name.Equals(fileName))
        {
            ac.UnloadAudioData();
            ac = null;
        }


        if(ac == null)
        {
            ac = Resources.Load(musicPath+fileName) as AudioClip;

            if (ac == null)
            {
                Debug.LogError("加载不到该bgm====" + musicPath + fileName);
            }
        }

        //如果是同一个音乐 就不往下执行了
        if(audioSource.clip == ac && audioSource.isPlaying)
        {
            return;
        }

        if (ac == null)
        {
            Debug.LogError("始终没有找到该bgm，  不播放");
            return;
        }

        audioSource.clip = ac;
        audioSource.loop = loop;
        audioSource.Play();

        this.callFinish = callFinish;

        Debug.Log("Music Player play " + musicPath + fileName   + "    "+ audioSource.clip.name);
    }

    public void PlayW(string fileName, bool loop,callbackInt callFinish = null)
    {
        if (isPlayering == false) return;


        if (ac != null && !ac.name.Equals(fileName))
        {
            ac.UnloadAudioData();
            ac = null;
        }


        if (ac == null)
        {
            ac = ResManager.getAC(musicPath + fileName + ".ogg");
            ac.name = fileName;
            if (ac == null)
            {
                Debug.LogError("加载不到该bgm====" + musicPath + fileName);
            }
        }

        //如果是同一个音乐 就不往下执行了
        if (audioSource.clip == ac && audioSource.isPlaying)
        {
            return;
        }

        if (ac == null)
        {
            Debug.LogError("始终没有找到该bgm，  不播放");
            return;
        }

        audioSource.clip = ac;
        audioSource.loop = loop;
        audioSource.pitch = 1;
        audioSource.volume = volume;
        audioSource.Play();

       

        this.callFinish = callFinish;

        Debug.Log("Music Player play " + musicPath + fileName + "    " + audioSource.clip.name);
    }

    public void setjiasu(int sudu)
    {
         audioSource.pitch = sudu;
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.UnPause();
    }
	
	public void Stop()
	{
		audioSource.Stop();
	}


    //全局声音大小
    public void setVolume(float volume)
    {
       audioSource.volume = volume;
       MusicPlayer.volume = volume;
    }

    public float getVolume()
    {
        return audioSource.volume;
    }

    public void clear()
    {
        for(int i = 0; i < clipList.Count; i++)
        {
            AudioClip clip = clipList[i];
            clip.UnloadAudioData();
        }
        clipList.Clear();
    }




    private callbackInt callFinish;
    
    void Update()
    {
        if(callFinish != null)
        {
            if(!audioSource.isPlaying)
            {
                callFinish(0);
                callFinish = null;
            }
        }
    }



    //-------混音部分-------
    private void addAudioMixer()
    {
        //加入混音
        audioMixer = Resources.Load("Audio/Bgm_Mixer") as AudioMixer;

        if (audioMixer != null)
        {
            string groupStr = "Master";
            AudioMixerGroup[] groups = audioMixer.FindMatchingGroups(groupStr);
            if (groups.Length > 0)
            {
                audioSource.outputAudioMixerGroup = groups[0];
            }
        }
    }

    public void setMixerValue(string flag,float value)
    {
        if(audioMixer != null)
        {
            audioMixer.SetFloat(flag, value);
        }
    }
    //------混音---



    public static void setPlayering(bool flag)
    {
        isPlayering = flag;
        if (!flag && getInstance() != null)
        {
            getInstance().Stop();
        }
    }
}