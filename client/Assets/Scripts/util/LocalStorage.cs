using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalStorage 
{

    //沙盒路径
    public static string getPersistentPath()
    {
        Debug.Log(UnityEngine.Application.persistentDataPath);
        return UnityEngine.Application.persistentDataPath + "/";
    }

    private const string userFile = "userInfo";

    public static bool musicswitch = true;
    public static bool soundswitch = true;
    public static bool voiceswitch = true;

    public static float musicvolume = 1.0f;
    public static float soundvolume = 1.0f;
    public static float voicevolume = 1.0f;

    public static void saveInfo()
    {
        JavaReader jr = null;
        try
        {
            string path = getPersistentPath() + userFile;
            BinaryFile bf = new BinaryFile();
            BinaryWriter bw = bf.writeBinaryFile(path);
            if (bw == null)
            {
                Debug.Log("写入失败!");
                return;
            }
            jr = new JavaReader();
            jr.setBinaryWriter(bw);

            jr.writeBoolean(musicswitch);
            jr.writeBoolean(soundswitch);
            jr.writeBoolean(voiceswitch);
            jr.writeFloat(musicvolume);
            jr.writeFloat(soundvolume);
            jr.writeFloat(voicevolume);
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
        finally
        {
            if (jr != null)
            {
                jr.close();
            }
        }


    }


    public static void readInfo()
    {


        JavaReader jr = null;
        try
        {
            string path = getPersistentPath() + userFile;
            BinaryFile bf = new BinaryFile();
            BinaryReader br = bf.readBinaryFile(path);
            if (br == null)
            {
                Debug.Log("读取失败!");
            }
            else
            {
                jr = new JavaReader();
                jr.setBinaryReader(br);

                musicswitch = jr.readBoolean();
                soundswitch = jr.readBoolean();
                voiceswitch = jr.readBoolean();
                musicvolume = jr.readFloat();
                soundvolume = jr.readFloat();
                voicevolume = jr.readFloat();
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
        finally
        {
            if (jr != null)
            {
                jr.close();
            }
        }

        MusicPlayer.setPlayering(musicswitch);
        SoundPlayer.setPlayering(soundswitch);
        VoicePlayer.setPlayering(voiceswitch);
        MusicPlayer.volume = musicvolume;
        SoundPlayer.volume = soundvolume;
        VoicePlayer.volume = voicevolume;

    }

}
