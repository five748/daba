using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameTime : SingleMono2<GameTime>
{
    private static int _tzong = -1;
    public static int Tzong
    {//时区
        get
        {
            if (_tzong == -1)
            {
                string str = DateTime.Now.ToString("%z");
                if (str.IsInt())
                {
                    _tzong = int.Parse(str);
                }
            }
            return _tzong;
        }
    }
    private static bool isHaveGet1970 = false;
    private static DateTime _time1970;
    private static DateTime Time1970
    {
        get
        {
            if (!isHaveGet1970)
            {
                isHaveGet1970 = true;
                _time1970 = new DateTime(1970, 1, 1);
            }
            return _time1970;
        }
    }

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    public static string ConvertDateTimeToStamp(System.DateTime time, int length = 13)
    {
        int ts = ConvertDateTimeToIntSpc(time);
        return ts.ToString().Substring(0, length);
    }
    /// <summary>  
    /// 将c# DateTime时间格式转换为Unix时间戳格式  
    /// </summary>  
    /// <param name="time">时间</param>  
    /// <returns>long</returns>  
    public static ulong ConvertDateTimeToLong(System.DateTime time)
    {
        ulong t = (ulong)(time.Ticks - Time1970.Ticks) / 10000;   //除10000调整为13位      
        return t;
    }
    public static int ConvertDateTimeToInt(System.DateTime time)
    {
        long t = (time.Ticks - Time1970.Ticks) / 10000000;
        //print(t);
        return (int)(t);
    }
    public static int ConvertDateTimeToIntSpc(System.DateTime time)
    {
        long t = (time.Ticks - Time1970.Ticks) / 10000000;
        return (int)(t);
    }
    
    public static string ConvertDateTimeToStringSpc(System.DateTime time)
    {
        long t = (time.Ticks - Time1970.Ticks) / 10000000;
        return t.ToString();
    }

    /// <summary>        
    /// 时间戳转为C#格式时间        
    /// </summary>        
    /// <param name=”timeStamp”></param>        
    /// <returns></returns>        
    public static DateTime ConvertStringToDateTime(string timeStamp)
    {
        long lTime = long.Parse(timeStamp);
        TimeSpan toNow = new TimeSpan(lTime);
        return Time1970.Add(toNow);
    }
    /// <summary>
    /// 时间戳转为C#格式时间10位 零时区
    /// </summary>
    /// <param name="timeStamp">Unix时间戳格式</param>
    /// <returns>C#格式时间</returns>
    public static DateTime GetDateTimeFrom1970Ticks(long curSeconds)
    {
        return Time1970.AddSeconds(curSeconds);
    }

    public static bool IsExpire(long curSeconds)
    {
        return TimeTool.SerNowTime.CompareTo(GetDateTimeFrom1970Ticks(curSeconds)) > 0;
    }

    public static string GetStampString(long timeStamp, bool isHaveYear, bool isHaveHour, bool isHourHaveNewLine = false)
    {
        
        var str = "";
        DateTime dataTime = GetDateTimeFrom1970Ticks(timeStamp).AddHours(Tzong);
        if (isHaveYear) {
            str += dataTime.Year + ".";
        }
        str += dataTime.Month + ".";
        str += dataTime.Day;
        if (isHaveHour) {
            str += ".";
            if (isHourHaveNewLine)
            {
                str += System.Environment.NewLine;
            }
            else {
                str += " ";
            }
            str += GetTemTime(dataTime.Hour) + ":" + GetTemTime(dataTime.Minute);
        }
        return str;
    }
    public static string GetStampStringHour(long timeStamp)
    {
        DateTime dataTime = GetDateTimeFrom1970Ticks(timeStamp).AddHours(Tzong);
        return GetTemTime(dataTime.Hour) + ":" + GetTemTime(dataTime.Minute);
    }
    public static string GetStampStringCutDHM(long timeStamp, bool isHaveDay = true, bool isHaveSecond = false)
    {
        DateTime dt = GetDateTimeFrom1970Ticks(timeStamp).AddHours(Tzong);
        TimeSpan time = dt - TimeTool.SerNowTime;
        if (time.TotalSeconds <= 0)
        {
            return "0时0分0秒".ChangeTextStr();
        }
        string str = "";
        if (isHaveDay)
        {
            if (time.Days > 0)
                str += time.Days + "天".ChangeTextStr();
        }
        if (time.Hours < 10)
            str += "0";
        str += time.Hours + "时".ChangeTextStr();
        if (time.Minutes < 10)
            str += "0";
        str += time.Minutes + "分".ChangeTextStr();
        if (isHaveSecond)
        {
            str += time.Seconds + "秒".ChangeTextStr();
        }
        return str;
    }
    public static string GetStampStringCutOnLine(long timeStamp)
    {
        if (timeStamp == 0)
        {
            return "不在线".ChangeTextStr();
        }
        DateTime dt = GetDateTimeFrom1970Ticks(timeStamp).AddHours(Tzong);
        TimeSpan time = TimeTool.SerNowTime - dt;
        if (time.TotalSeconds <= 60)
        {
            return "在线".ChangeTextStr();
        }
        if (time.TotalSeconds < 3600)
        {
            return time.Minutes + "分前".ChangeTextStr();
        }

        if (time.TotalSeconds < 86400)
        {
            return time.Hours + "小时前".ChangeTextStr();
        }
        return time.Days + "天前".ChangeTextStr();
    }
    /// <summary>        
    /// 时间戳转为历史时间字符串        
    /// </summary>        
    /// <param name=”timeStamp”></param>        
    /// <returns></returns>   
    private static string GetTemTime(int num)
    {
        if (num < 10)
            return "0" + num;
        return num.ToString();
    }
    public static int GetWeekCutTime()
    {
        var day = 6 - ((int)TimeTool.SerNowTime.DayOfWeek + 6) % 7;
        var hour = TimeTool.SerNowTime.Minute + TimeTool.SerNowTime.Second > 0 ? 23 - TimeTool.SerNowTime.Hour : 24 - TimeTool.SerNowTime.Hour;
        var min = TimeTool.SerNowTime.Second > 0 ? 59 - TimeTool.SerNowTime.Minute : 60 - TimeTool.SerNowTime.Minute;
        var msecond = 60 - TimeTool.SerNowTime.Second;
        var time = TimeTool.SerNowUtcTimeInt + day * 24 * 60 * 60 + hour * 60 * 60 + min * 60 + msecond;
        //time += TableCache.Instance.systemTable[1].DayResetHour * 60 * 60;
        return time;
    }

    //获取下个月的开始时间
    public static int GetMonthCutTime()
    {
        var now = DateTime.Now;
        var year = now.Year + now.Month / 12;
        var month = now.Month % 12 + 1;
        var to = new DateTime(year, month, 1, 0, 0, 0);
        var time = TimeTool.SerNowUtcTimeInt + (int)(to-now).TotalSeconds;
        return time;
    }

    public static long OneDayLong = 24 * 60 * 60;

    //获取传入时间的逻辑日开始时间
    public static long GetDayBeginTime(long dt)
    {
        return 0;
        //var refreshTime = AllValueData.Instance.rootData.All.Global.RefreshTime;
        //long k;
        //if (dt > refreshTime)
        //    k = (dt - refreshTime) / OneDayLong;
        //else
        //    k = (dt - refreshTime - OneDayLong + 1) / OneDayLong;
        //return refreshTime + k * OneDayLong;
    }
}
