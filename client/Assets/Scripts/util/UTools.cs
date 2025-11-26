using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//using System.Windows.Forms;
#endif

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public string filter = null;
    public string customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public string file = null;
    public int maxFile = 0;
    public string fileTitle = null;
    public int maxFileTitle = 0;
    public string initialDir = null;
    public string title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public string defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public string templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}




public class UTools 
{



    public static IEnumerator CutScreenCapture(int scrX, int scrY, int width, int height)
    {
        //等待所有的摄像机跟GUI渲染完成
        yield return new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        //如果 recalculateMipMaps　设置为真，这个贴图的mipmaps就会更新 如果 recalculateMipMaps设置为假，你需要调用Apply重新计算它们
        tex.ReadPixels(new Rect(scrX, scrY, width, height), 0, 0, true);

        byte[] imagebytes = tex.EncodeToPNG();//转化为png图

        tex.Compress(false);//对屏幕缓存进行压缩

        string imgPath = "screen" + currentTimeMillis() + ".png";


        System.IO.File.WriteAllBytes(imgPath, imagebytes);//存储png图

        Debug.Log("cutover");
    }
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn)

    {
        return GetOpenFileName(ofn);
    }
    public static string openFileDialog()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        /*return ProgramForm.showFileDialog();*/
        /*
        try
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string[] names = fileDialog.FileNames;
                if (names.Length > 0)
                {
                    return names[0];
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        catch (Exception e)
        {
            Debug.Log("打开错误：" + e.Message);
            return "";
        }
        */


        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = "All Files\0*.*\0\0";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = UnityEngine.Application.dataPath;//默认路径

        ofn.title = "Open Project";

        ofn.defExt = "*";//显示文件的类型
                           //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        if (GetOpenFileName(ofn))
        {
            return ofn.file;
        }

        return "";

#else
        return "";
#endif
    }
/*
    public static void setIcon(GameObject die,int type,int id,int dp)
    {
        UISprite icongen = die.GetComponent<UISprite>();
        if (type == Statics.TYPE_CRYSTALS_PAY)
        {
            icongen.spriteName = "main_icon_s6";
        }
        else if (type == Statics.TYPE_CRYSTALS_FREE)
        {
            icongen.spriteName = "main_icon_s5";
        }
        else if (type == Statics.TYPE_NUT)
        {
            icongen.spriteName = "main_icon_s2";
        }
        else if (type == Statics.TYPE_ENERGY)
        {
            icongen.spriteName = "main_icon_s0";
        }
        else if (type == Statics.TYPE_BREAD)
        {
            icongen.spriteName = "main_icon_s1";
        }
        else
        {

            PaintNode icon = getIconByType(icongen.gameObject, type, icongen.depth);
            icon.setShowRectLimit(50);

            if (type >= Statics.TYPE_EQUIP_WEAPON && type <= Statics.TYPE_EQUIP_BACKWEAR)
            {
                icon.setFrameSpr(type - 81, EquipmentDef.datas[id].MonitorID);
            }
            else if (type == Statics.TYPE_STAFF_PIECE)
            {
                icon.setFrameSpr(0, Staff_ChipDef.datas[id].MonitorID);
            }
            else if (type == Statics.TYPE_STAFF_WAKEMAT)
            {
                icon.setFrameSpr(0, Staff_AwakeDef.datas[id].MonitorID);
            }
            else if (type == Statics.TYPE_PACKAGE)
            {
                icon.setFrameSpr(0, P_PackDef.datas[id].MonitorID);
            }
            else if (type == Statics.TYPE_STAFF)
            {
                icon.setFrameSpr(0, StaffDef.datas[id].id);
            }
            else
            {
                icon.setFrameSpr(0, P_ArticleDef.datas[id].MonitorID);
            }

            icongen.enabled = false;
        }
    }
    public static PaintNode getIcon(GameObject die,string filename,int dp, int rectsize = -1)
    {
        PaintNode icon = null;
        GameObject texframeobj33 = ResManager.getGameObject("allpre", "vpaintnode");
        texframeobj33.name = "iconpaintnode0";
        icon = texframeobj33.GetComponent<PaintNode>();
        icon.create(die, filename+".bin", 0, 0, 0);
        icon.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        icon.setdepth(dp);
        icon.paintforce();
        if (rectsize != -1)
        {
            icon.setShowRectLimit(rectsize);
        }
        return icon;
    }
    public static PaintNode getIconByRewardBean(GameObject die, RewardBean bean, int dp,int rectsize=-1)
    {
        string filename = "Icon_Article";
        if (bean.type == Statics.TYPE_PACKAGE)
        {
            filename = "P_Pack";
        }
        else if (bean.type >= Statics.TYPE_EQUIP_WEAPON && bean.type <= Statics.TYPE_EQUIP_BACKWEAR)
        {
            filename = "Equipment";
        }
        else if (bean.type == Statics.TYPE_STAFF_PIECE)
        {
            filename = "Staff_Chip";
        }
        else if (bean.type == Statics.TYPE_STAFF_WAKEMAT)
        {
            filename = "Icon_Awake";
        }
        else if (bean.type == Statics.TYPE_STAFF)
        {
            filename = "Icon_Staff";
        }
        else if (bean.type == Statics.TYPE_PLAYER_HEAD)
        {
            filename = "Icon_Head";
        }
        else if (bean.type == Statics.TYPE_MEDAL)
        {
            filename = "Icon_Medal";
        }
        else if (bean.type == Statics.TYPE_GENERAL)
        {
            filename = "Icon_Building";
        }
        else if (bean.type == Statics.TYPE_DECO)
        {
            filename = "Icon_Building";
        }
        PaintNode icon = null;
        GameObject texframeobj33 = ResManager.getGameObject("allpre", "vpaintnode");
        texframeobj33.name = "iconpaintnode0";
        icon = texframeobj33.GetComponent<PaintNode>();
        icon.create(die, filename + ".bin", 0, 0, 0);
        icon.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        icon.setdepth(dp);
        icon.paintforce();

        if (rectsize != -1)
        {
            icon.setShowRectLimit(rectsize);
        }

        if (bean.type >= Statics.TYPE_EQUIP_WEAPON && bean.type <= Statics.TYPE_EQUIP_BACKWEAR)
        {
            icon.setFrameSpr(bean.type - Statics.TYPE_EQUIP_WEAPON, EquipmentDef.datas[bean.id].MonitorID);
        }
        else if (bean.type == Statics.TYPE_STAFF_PIECE)
        {
            icon.setFrameSpr(0, Staff_ChipDef.datas[bean.id].MonitorID);
        }
        else if (bean.type == Statics.TYPE_STAFF_WAKEMAT)
        {
            icon.setFrameSpr(0, Staff_AwakeDef.datas[bean.id].MonitorID);
        }
        else if (bean.type == Statics.TYPE_PACKAGE)
        {
            icon.setFrameSpr(0, P_PackDef.datas[bean.id].MonitorID);
        }
        else if (bean.type == Statics.TYPE_STAFF)
        {
            icon.setFrameSpr(0, StaffDef.datas[bean.id].id);
        }
        else if (bean.type == Statics.TYPE_PLAYER_HEAD)
        {
            icon.setFrameSpr(0, Player_HeadDef.datas[bean.id].Resource);
        }
        else if (bean.type == Statics.TYPE_MEDAL)
        {
            icon.setFrameSpr(0, Player_MedalDef.datas[bean.id].Resource);
        }
        else if (bean.type == Statics.TYPE_GENERAL)
        {
            icon.setFrameSpr(0, bean.id);
        }
        else if (bean.type == Statics.TYPE_DECO)
        {
            icon.setFrameSpr(1, bean.id);
        }
        else
        {
            icon.setFrameSpr(0, P_ArticleDef.datas[bean.id].MonitorID);
        }
        return icon;
    }
    public static PaintNode getIconByType(GameObject die, int type, int dp, int rectsize = -1)
    {
        string filename = "Icon_Article";
        if (type == Statics.TYPE_PACKAGE)
        {
            filename = "P_Pack";
        }
        else if (type >= Statics.TYPE_EQUIP_WEAPON && type <= Statics.TYPE_EQUIP_BACKWEAR)
        {
            filename = "Equipment";
        }
        else if (type == Statics.TYPE_STAFF_PIECE)
        {
            filename = "Staff_Chip";
        }
        else if (type == Statics.TYPE_STAFF_WAKEMAT)
        {
            filename = "Icon_Awake";
        }
        else if (type == Statics.TYPE_STAFF)
        {
            filename = "Icon_Staff";
        }
        else if (type == Statics.TYPE_PLAYER_HEAD)
        {
            filename = "Icon_Head";
        }else if (type == Statics.TYPE_MEDAL)
        {
            filename = "Icon_Medal";
        }
        PaintNode icon = null;
        GameObject texframeobj33 = ResManager.getGameObject("allpre", "vpaintnode");
        texframeobj33.name = "iconpaintnode0";
        icon = texframeobj33.GetComponent<PaintNode>();
        icon.create(die, filename + ".bin", 0, 0, 0);
        icon.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        icon.setdepth(dp);
        icon.paintforce();

        if (rectsize != -1)
        {
            icon.setShowRectLimit(rectsize);
        }

        return icon;
    }
    */
    public static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static long currentTimeMillis()
    {
        return (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
    }

    public static GameObject setPresent(GameObject parent, GameObject go)
    {
#if UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }

    public static void setActive(GameObject obj, bool flag)
    {
        if (obj == null) return;
        if (flag)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
            }
        }
    }



    public static string removeNguiSymbolsEffect(string str, bool isAllowEnter)
    {
        str = str.Replace("[b]", "").Replace("[/b]", "");
        str = str.Replace("[i]", "").Replace("[/i]", "");
        str = str.Replace("[u]", "").Replace("[/u]", "");
        str = str.Replace("[s]", "").Replace("[/s]", "");
        str = str.Replace("[c]", "").Replace("[/c]", "");
        /*str = str.Replace("[sub]", "").Replace("[/sub]", "");
        str = str.Replace("[sup]", "").Replace("[/sup]", "");*/
        if (!isAllowEnter)
            str = str.Replace("\n", "").Replace("\r", "");
        return str;
    }
    public static void setLabelEffect(UILabel label, int typeID, int newSize)
    {
        //setLabelEffect(label, typeID, label.overflowMethod, newSize);
    }

    public static string toShortCount(int count)
    {
        string str = "";

        //百万级别
        if (count >= 1000000)
        {
            str = (int)(count / 1000000) + "m";
        }
        else if (count >= 10000)
        {
            str = (int)(count / 1000) + "k";
        }
        else
        {
            str = count + "";
        }
        return str;
    }



    public static void clearChildImmediate(GameObject itemParent, bool isClearInActiy)
    {
        //清理child
        List<Transform> lst = new List<Transform>();
        foreach (Transform child in itemParent.transform)
        {
            if (child.gameObject.activeSelf || isClearInActiy)
            {

                lst.Add(child);
            }
        }
        for (int i = 0; i < lst.Count; i++)
        {
            MonoBehaviour.DestroyImmediate(lst[i].gameObject);
        }
    }
    public static void clearChild(GameObject itemParent)
    {
        //清理child
        List<Transform> lst = new List<Transform>();
        foreach (Transform child in itemParent.transform)
        {
            //             if (child.gameObject.activeSelf)
            //             {
            lst.Add(child);
            //            }
        }
        for (int i = 0; i < lst.Count; i++)
        {
            MonoBehaviour.Destroy(lst[i].gameObject);
        }
    }
    public static void DestroyMonoRef(ref MonoBehaviour mob)
    {
        if (mob != null)
        {
            MonoBehaviour.Destroy(mob);
            mob = null;
        }
    }
    public static void DestroyMonoRef(ref Texture2D mob)
    {
        if (mob != null)
        {
            MonoBehaviour.Destroy(mob);
            mob = null;
        }
    }

    public static void DestroyMonoRef(ref UILabel mob)
    {
        if (mob != null)
        {
            MonoBehaviour.Destroy(mob);
            mob = null;
        }
    }
    public static void DestroyMonoRef(ref List<UILabel> mob)
    {
        if (mob != null)
        {
            for (int i = 0; i < mob.Count; i++)
            {
                MonoBehaviour.Destroy(mob[i]);
                mob[i] = null;
            }
            mob = null;
        }
    }

    public static void DestroyMonoRef(ref UILabel[] mob)
    {
        if (mob != null)
        {
            for (int i = 0; i < mob.Length; i++)
            {
                MonoBehaviour.Destroy(mob[i]);
                mob[i] = null;
            }
            mob = null;
        }
    }
    public static void DestroyMonoRef(ref UISprite mob)
    {
        if (mob != null)
        {
            MonoBehaviour.Destroy(mob);
            mob = null;
        }
    }
    public static void DestroyMonoRef(ref List<UISprite> mob)
    {
        if (mob != null)
        {
            for (int i = 0; i < mob.Count; i++)
            {
                MonoBehaviour.Destroy(mob[i]);
                mob[i] = null;
            }
            mob = null;
        }
    }

    public static void DestroyMonoRef(ref UISprite[] mob)
    {
        if (mob != null)
        {
            for (int i = 0; i < mob.Length; i++)
            {
                MonoBehaviour.Destroy(mob[i]);
                mob[i] = null;
            }
            mob = null;
        }
    }
    public static void DestroyMonoRef(ref UITexture mob)
    {
        if (mob != null)
        {
            MonoBehaviour.Destroy(mob);
            mob = null;
        }
    }

    public static void DestroyMonoRef(ref GameObject mob)
    {
        if (mob != null)
        {
            MonoBehaviour.Destroy(mob);
            mob = null;
        }
    }

    internal static GameObject AddChild(GameObject parent, GameObject go)
    {
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }


}
