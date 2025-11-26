using UnityEngine;
using System.Collections;
using System;

public class Date {

    public DateTime dataTime;

    public Date()
    {
        dataTime = DateTime.Now;
    }


    //使用服务器时区   还是  使用本地时间
    public Date(long mm, bool useServerZone)
    {
        //if(useServerZone)
        //{
        //    DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0);

        //    int serverZone =GameGlobal.user.gameData.userData.timeZone - CTGlobal.timeZone;
        //    dataTime = Jan1st1970.AddMilliseconds(mm).ToLocalTime();
        //    dataTime = dataTime.AddHours(serverZone);
        //}
        //else
        {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dataTime = Jan1st1970.AddMilliseconds(mm).ToLocalTime();
        }
    }


    



    public void setYear(int value)
    {
        int cha = value - dataTime.Year;
        dataTime = dataTime.AddYears(cha);
    }

    public int getYear()
    {
        return dataTime.Year;
    }

    public void setMonth(int value)
    {
        int cha = value - dataTime.Month;
        dataTime = dataTime.AddMonths(cha);
    }
    public int getMonth()
    {
        return dataTime.Month;
    }

    public void setDate(int value)
    {
        int cha = value - dataTime.Day;
        dataTime = dataTime.AddDays(cha);
    }
    public int getDate()
    {
        return dataTime.Day;
    }

    public int getDay()
    {
        return dataTime.Day;
    }

    public void setHours(int value)
    {
        int cha = value - dataTime.Hour;
        dataTime = dataTime.AddHours(cha);
    }
    public int getHours()
    {
        return dataTime.Hour;
    }

    public void setMinutes(int value)
    {
        int cha = value - dataTime.Minute;
        dataTime = dataTime.AddMinutes(cha);
    }
    public int getMinutes()
    {
        return dataTime.Minute;
    }
    
    public int getSecond()
    {
        return dataTime.Second;
    }

    public bool before(Date data)
    {
        if(DateTime.Compare(dataTime, data.dataTime) < 0)
        {
            return true;
        }
        return false;
    }

    public bool after(Date data)
    {
        if (DateTime.Compare(dataTime, data.dataTime) > 0)
        {
            return true;
        }
        return false;
    }


    
    public void setSeconds(int v)
    {
        int cha = v - dataTime.Second;
        dataTime = dataTime.AddSeconds(cha);
    }


    

    public static long getTotalMilliseconds(DateTime time)
    {
        DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        DateTime universalData = time.ToUniversalTime();
        TimeSpan chaData = (universalData - Jan1st1970);
        return Convert.ToInt64(chaData.TotalMilliseconds);
    }
    

    public string toString()
    {
        return " " +getYear() + "年" + getMonth() + "月" + getDay() + "日 " + get00Format(getHours()) + ":" + get00Format(getMinutes()) + ":" + get00Format(getSecond());
    }
    public string toString2()
    {
        return "" + getYear() + "/" + getMonth() + "/" + getDay() + " " + get00Format(getHours()) + ":" + get00Format(getMinutes()) + ":" + get00Format(getSecond());
    }
    public string toString3()
    {
        return "" + getMonth() + "/" + getDay() + " " + get00Format(getHours()) + ":" + get00Format(getMinutes());
    }
    public string toString4()
    {
        return "" + getYear() + "/" + getMonth() + "/" + getDay() + " " + get00Format(getHours()) + ":" + get00Format(getMinutes());
    }
    public string toString5()
    {
        return "" + getYear() + "." + getMonth() + "." + getDay() + " " + get00Format(getHours()) + ":" + get00Format(getMinutes());
    }
    public string toJustTimeData()
    {

        return " " + getYear() + "-" + getMonth() + "-" + getDay() + " " + get00Format(getHours()) + ":" + get00Format(getMinutes()) + ":" + get00Format(getSecond());
    }

    public string toYearDay()
    {

        return getMonth() + "月" + getDay() + "日" + get00Format(getHours()) + ":" + get00Format(getMinutes()) + ":" + get00Format(getSecond());
    }
    public string toYearMonthDay()
    {

        return getYear() + "年" + getMonth() + "月" + getDay() + "日" + get00Format(getHours()) + ":" + get00Format(getMinutes()) + ":" + get00Format(getSecond());
    }

    public string toMonthDay()
    {

        return get00Format(getMonth()) + "月" + get00Format(getDay()) + "日";
    }
    public string toMonthDay2()
    {

        return get00Format(getMonth()) + "." + get00Format(getDay()) + "";
    }

    public string toYMD()
    {

        return getYear() + "-" + getMonth() + "-" + getDay();
    }
    public string toYMD2()
    {

        return getYear() + "." + get00Format(getMonth()) + "." + get00Format(getDay());
    }

    public string toLogString()
    {

        return get00Format(getHours()) + ":" + get00Format(getMinutes()) + ":" + get00Format(getSecond());
    }


    private string get00Format(int nd)
    {
        if(nd < 10)
        {
            return "0" + nd;
        }else
        {
            return nd + "";
        }
    }

    public string toJustYearToDay()
    {

        return " " + getYear() + "." + getMonth() + "." + getDay();
    }



    public string toLocaleString()
    {
        return dataTime.ToLongDateString();
    }

    public string toLongTimeString()
    {
        return dataTime.ToLongTimeString();
    }

    public TimeSpan getTimeZoneOffset()
    {
        TimeSpan time = dataTime.ToLocalTime() - dataTime.ToUniversalTime();
        return time;
    }


    //解析字符串成当前毫秒数
    public static long parseDate(string timeYMDHM, bool useServerZoneTime = true)
    {
        string str_year = "0";
        string str_month = "0";
        string str_day = "0";
        string str_hour = "0";
        string str_min = "0";

        try
        {
            str_year = timeYMDHM.Substring(0, 4);
            str_month = timeYMDHM.Substring(4, 2);
            str_day = timeYMDHM.Substring(6, 2);
            str_hour = timeYMDHM.Substring(8, 2);
            str_min = timeYMDHM.Substring(10, 2);
        }
        catch(Exception e)
        {
            Debug.LogError(e.StackTrace);
        }

        int year = Convert.ToInt32(str_year);
        int month = Convert.ToInt32(str_month);
        int day = Convert.ToInt32(str_day);
        int hour = Convert.ToInt32(str_hour);
        int minute = Convert.ToInt32(str_min);

        DateTime startTime = new DateTime(year, month, day, hour, minute, 0);

        if(useServerZoneTime)
        {
           // int serverZone = NetSender.timeZone - NetSender.getInstance().user.gameData.userData.timeZone;
           // startTime = startTime.AddHours(serverZone);
        }

        long time = Date.getTotalMilliseconds(startTime);

        return time;
    }

    //这个月最后一天的index（最大30）
    public static int getLastDay(int year, int month)
    {
        int days = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(year, month);
        return days;
    }


}
