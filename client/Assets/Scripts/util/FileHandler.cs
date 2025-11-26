using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class FileHandler
{


    public FileInfo file;


    public FileHandler(string path)
    {
        file = new FileInfo(path);
    }

    public bool exist()
    {
        return file.Exists;
    }

    public void create()
    {
        if (file.Exists) return;
        if (!Directory.Exists(file.DirectoryName))
        {
            Directory.CreateDirectory(file.DirectoryName);
        }
    }

    public void delect()
    {
        if (file.Exists == false) return;
        file.Delete();
    }

    public void writeBytesUseTime(byte[] data, bool isUseServerTime = false)
    {
        write(data);

        //if (isUseServerTime)
        //{
        //    Date date = new Date(UserBean.getCurrTimeMM(), false);

        //    try
        //    {
        //        //android 7.0 会报 IOException  参数无效 Invalid parameter
        //        file.LastWriteTime = date.dataTime;
        //    }
        //    catch (Exception e)
        //    {
        //        //UIHelp.showToast(e.Message);
        //    }
        //}
    }

    //
    public void write(byte[] data, bool isAppend = false)
    {
        FileStream fs = null;
        try
        {
            if (isAppend)
            {
                fs = file.Open(FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = file.Open(FileMode.Create, FileAccess.Write);
            }

            fs.Write(data, 0, data.Length);
            fs.Flush();


            file.Refresh();
        }
        catch (Exception e)
        {
            Debug.LogError(file.FullName + "\n" + e.StackTrace);
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
                fs.Dispose();
            }
        }
    }

    public static void writeByte(string path, byte[] data, bool isAppend = false)
    {
        //创建文件
        FileHandler file = new FileHandler(path);
        file.create();

        FileStream fs = null;
        try
        {
            if (isAppend)
            {
                fs = file.file.Open(FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = file.file.Open(FileMode.Create, FileAccess.Write);
            }

            fs.Write(data, 0, data.Length);
            fs.Flush();

            file.file.Refresh();
        }
        catch (Exception e)
        {
            Debug.LogError(file.file.FullName + "\n"/*+e.StackTrace */+ e.Message);
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
                fs.Dispose();
            }
        }
    }

    public byte[] getData()
    {
        byte[] data = new byte[0];
        FileStream fs = null;
        try
        {
            fs = file.OpenRead();
            data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
        }
        catch (Exception e)
        {
            Debug.Log("读取失败=" + file.FullName + "\n" + e.StackTrace);
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
                fs.Dispose();
            }
        }

        return data;
    }


    public static void copyByStreamingAssets(string srcPath, string dstPath)
    {

        //JavaReader jr = PlatformTools.getStreamingAssetsData(srcPath);

        //if (jr != null)
        //{
        //    FileHandler file = new FileHandler(dstPath);
        //    file.create();
        //    file.write(jr.toByteArrayBR());

        //    jr.close();
        //}

    }

    public static void copyByFile(string srcPath, string dstPath)
    {

        //JavaReader jr = PlatformTools.getFileDataInputStream(srcPath);

        //if (jr != null)
        //{
        //    FileHandler.writeByte(dstPath, jr.toByteArrayBR());

        //}

    }



}
