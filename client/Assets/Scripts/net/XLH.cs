using LitJson;
using System;
using System.Reflection;
using UnityEngine;

public class XLH
{
    public T readToObj<T>(string jsonstr)
    {

        T t = default(T);
try
{
	        t= JsonMapper.ToObject<T>(jsonstr);
}
catch (System.Exception ex)
{
            Debug.Log("Exception Err:" + ex.Message);
        }

        return t;
    }

    public static object readToObject(string jsonstr,string classname)
    {
        Type t = Type.GetType(classname);

        XLH xlh = new XLH();

        MethodInfo mi = xlh.GetType().GetMethod("readToObj").MakeGenericMethod(t);
        object[] l_args = new object[1] { jsonstr };
        object testContent = mi.Invoke(xlh, l_args);
        return testContent;

    }
    public static Response readToResponse(string jsonstr, Request req)
    {
        JsonData jd = JsonMapper.ToObject(jsonstr);
        string data = "{}";
        if (jd["data"]!=null)
        {
            data = jd["data"].ToJson();
        }
        Response response = (Response)readToObject(data, req.responseName);
        response.code=(int)jd["code"];
        response.msg = (string)jd["msg"];
        response.sysTime = (int)jd["sysTime"];
        response.msgId = req.msgId;
        GameGlobal.setServerTime(response.sysTime);
        return response;
    }

    private T toObj<T>(string jsonstr, Type clazz)
    {
        T t = default(T);
        try
        {
            t = (T)read(jsonstr, clazz);
        }
        catch (Exception e)
        {
            //if (enableLog) log.error("字节数组转化为对象出现异常", e);
            //e.printStackTrace();
            //Debug.LogError(e.StackTrace);
        }
        return t;
    }
    public object read(string jsonstr, Type type)
    {
        object obj = new object();
        if (type == null)
        {
            return null;
        }
        else
        {
            obj = Activator.CreateInstance(type, null);
        }

        string name = "";

        try
        {
            FieldInfo[] fields = getClassFields(type);

            JsonData jd = JsonMapper.ToObject(jsonstr);

            /*
            int startAvailable = dis.available2();
            //  Debug.LogError("----" + startAvailable);
            while (startAvailable - maxLen < dis.available2() && dis.available2() >= 0)
            {
                name = dis.readUTF();
                if (isp)
                {
                    Debug.Log("name:" + name);
                }
                // Debug.LogError("----" + (startAvailable - maxLen)+" "+maxLen+" "+ startAvailable+" "+ dis.available2());

                FieldInfo field = getField(fields, name);

                object property = null;
                Type propertyClass = null;

                if (field == null)
                {
                    //field = null表示新的类结构中，该变量已经被删除或者改名
                    // Debug.Log("field为 null:"+  name);
                }
                else
                {
                    try
                    {
                        property = field.GetValue(obj);
                        if (property != null)
                        {
                            propertyClass = property.GetType();
                        }
                        else
                        {
                            propertyClass = field.FieldType;
                        }
                    }
                    catch (Exception e)
                    {
                       // Debug.Log("field:" + field);
                      //  Debug.Log("propertyClass:" + propertyClass);
                       // Debug.Log("property:" + property);
                       // Debug.LogError("序列化read出现异常。" + e.ToString());
                        throw e;
                    }
                }
                object obj1 = readProperty(dis, propertyClass, property, fromComponent);
                if (field != null && obj1 != null)
                {
                    bool isp = field.FieldType.IsAssignableFrom(obj1.GetType());
                    if (propertyClass == obj1.GetType() || isp)
                    {
                        field.SetValue(obj, obj1);
                    }
                    else
                    {
                        //Debug.Log("type:" + field);
                    }
                }
            }
              */
        }
        catch (Exception e)
        {
           // isp = false;
            Debug.LogError(e.Message + "   " + name + "\n" + e.StackTrace);
        }
      
        return obj;
    }
    public static FieldInfo[] getClassFields(Type type)
    {
        FieldInfo[] fields = type.GetFields();

        return fields;
    }
    public static FieldInfo getField(FieldInfo[] fields, string name)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].Name.Equals(name))
            {
                return fields[i];
            }
        }
        return null;
    }

    public static string getJson(Request request)
    {
        string str = JsonMapper.ToJson(request);
        return str;
    }
}
