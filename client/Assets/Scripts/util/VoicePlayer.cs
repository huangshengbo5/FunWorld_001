using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;

[RequireComponent(typeof(AudioListener))]
public class VoicePlayer : MonoBehaviour
{
    public string soundPath = "Audio/";

    public int soundMaxLength = 5;

    public static float volume = 1.0f;
    public static bool isPlayering = true;

    private List<AudioSource> sourceList = new List<AudioSource>();
    
    private List<AudioClip> clipList = new List<AudioClip>();

    private AudioMixer audioMixer;


    private static VoicePlayer instance = null;
    public static VoicePlayer getInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        clipList.Clear();


        for(int i = 0; i < soundMaxLength; i++)
        {
            AudioSource audioMgr = this.gameObject.AddComponent<AudioSource>();
            sourceList.Add(audioMgr);

         //   addAudioMixer(audioMgr);
        }
        
    }

    public void Play(string fileName)
    {
        Play(fileName, false, true);
    }

    public void Play(string fileName, bool loop)
    {
        Play(fileName, loop, true);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName">文件+路径名</param>
    /// <param name="loop">是否循环播放</param>
    /// <param name="usePool">是否缓存起来   TODO 需要测试内容是否可自动回收未加到List中的AudioClip</param>
    public void Play(string fileName, bool loop, bool usePool)
    {
        if (isPlayering == false) return;

        AudioClip ac = null;

        for (int i = 0; i < clipList.Count; i++)
        {
            AudioClip clip = clipList[i];

            if (clip.name.Equals(fileName))
            {
                ac = clip;
            }
        }

        if (ac == null)
        {
            ac = Resources.Load(soundPath + fileName) as AudioClip;

            if (ac == null)
            {
                Debug.Log("加载不到该音效====" + soundPath + fileName);
            }


            if (ac != null && usePool)
            {
                clipList.Add(ac);
            }
        }

        if (ac == null)
        {
            Debug.LogError("始终没有找到该音效，  不播放");
            return;
        }

        AudioSource audio = null;
        for (int i = 0; i < sourceList.Count; i++)
        {
            AudioSource temp = sourceList[i];
            if (temp.clip == null)
            {
                audio = temp;
                break;
            }
            else if (temp.isPlaying == false)
            {
                audio = temp;
                break;
            }
        }
        if (audio != null)
        {
            audio.clip = ac;
            audio.loop = loop;
            audio.volume = volume;
            audio.Play();

            if (usePool == false)
            {
                Debug.Log("ac.length=" + ac.length + "   " + ac.samples+" "+ CTTools.currentTimeMillis());
             //   StartCoroutine(DelayedCallback(ac.length, ac));

                acdata acd = new acdata();
                acd.ac = ac;
                acd.begintime = CTTools.currentTimeMillis();
                acdatalist.Add(acd);
            }
        }else
        {
            Debug.Log("音效太多 忽略了此音效=" + fileName);
        }
    }

    public void PlayW(string fileName, bool loop, bool usePool)
    {
        if (isPlayering == false) return;

        AudioClip ac = null;

        for (int i = 0; i < clipList.Count; i++)
        {
            AudioClip clip = clipList[i];

            if (clip.name.Equals(fileName))
            {
                ac = clip;
            }
        }

        if (ac == null)
        {
            ac = ResManager.getAC(soundPath + fileName + ".ogg");
            ac.name = fileName;

            if (ac == null)
            {
                Debug.Log("加载不到该音效====" +fileName);
            }


            if (ac != null && usePool)
            {
                clipList.Add(ac);
            }
        }

        if (ac == null)
        {
            Debug.LogError("始终没有找到该音效，  不播放");
            return;
        }

        AudioSource audio = null;
        for (int i = 0; i < sourceList.Count; i++)
        {
            AudioSource temp = sourceList[i];
            if (temp.clip == null)
            {
                audio = temp;
                break;
            }
            else if (temp.isPlaying == false)
            {
                audio = temp;
                break;
            }
        }
        if (audio != null)
        {
            audio.clip = ac;
            audio.loop = loop;
            audio.volume = volume;
            audio.Play();

            if (usePool == false)
            {
                Debug.Log("ac.length=" + ac.length + "   " + ac.samples + " " + CTTools.currentTimeMillis());
                //   StartCoroutine(DelayedCallback(ac.length, ac));

                acdata acd = new acdata();
                acd.ac = ac;
                acd.begintime = CTTools.currentTimeMillis();
                acdatalist.Add(acd);
            }
        }
        else
        {
            Debug.Log("音效太多 忽略了此音效=" + fileName);
        }
    }

    void Update()
    {
        for(int i = 0; i < acdatalist.Count; i++)
        {
            if(CTTools.currentTimeMillis()-acdatalist[i].begintime> acdatalist[i].ac.length * 1010)
            {
                Debug.Log("xiaohui=" + acdatalist[i].ac.length + " " + CTTools.currentTimeMillis());
                acdatalist[i].ac.UnloadAudioData();
                acdatalist.RemoveAt(i);
                break;
            }
        }
    }

    public List<acdata> acdatalist = new List<acdata>();
    public class acdata
    {
        public AudioClip ac;
        public long begintime = 0;
    }

    //释放内存
    private IEnumerator DelayedCallback(float time, AudioClip ac)
    {
        yield return 1;
        Debug.Log("xiaohui0=" + time);
        yield return new WaitForSeconds(time*2);

        ac.UnloadAudioData();
        Debug.Log("xiaohui="+ ac.length+" "+ CTTools.currentTimeMillis());
    }





    public void Pause()
    {
       for(int i = 0; i < sourceList.Count; i++)
        {
            AudioSource audio = sourceList[i];
            if(audio.isPlaying)
            {
                audio.Pause();
            }
        }
    }

    public void Resume()
    {
        for (int i = 0; i < sourceList.Count; i++)
        {
            AudioSource audio = sourceList[i];
            audio.UnPause();
        }
    }


    //此名字 不包含路径名
    public void Stop(string name)
    {
        if (name == null) return;
        for (int i = 0; i < sourceList.Count; i++)
        {
            AudioSource audio = sourceList[i];

            if (audio == null) continue;
            if (audio.clip == null) continue;

            if(audio.clip.name != null && audio.clip.name.Equals(name) && audio.isPlaying)
            {
                audio.Stop();
            }
        }

        Debug.Log("Stop background music");
    }
    public void Stop()
    {
        for (int i = 0; i < sourceList.Count; i++)
        {
            AudioSource audio = sourceList[i];
            audio.Stop();
        }
        
        Debug.Log("Stop background music");
    }


    //全局声音大小
    public void setVolume(float volume)
    {
        VoicePlayer.volume = volume;
        for (int i = 0; i < sourceList.Count; i++)
        {
            AudioSource audio = sourceList[i];
            audio.volume = volume;
        }
    }
        

    public float getVolume()
    {
        return VoicePlayer.volume;
    }

    public void clear()
    {
        for (int i = 0; i < clipList.Count; i++)
        {
            AudioClip clip = clipList[i];
            clip.UnloadAudioData();
        }
        clipList.Clear();
        acdatalist.Clear();
    }




    //-------混音部分-------
    private void addAudioMixer(AudioSource audioSource)
    {
        //加入混音
        audioMixer = Resources.Load("Audio/Snd_Mixer") as AudioMixer;

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

    public void setMixerValue(string flag, float value)
    {
        if (audioMixer != null)
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
