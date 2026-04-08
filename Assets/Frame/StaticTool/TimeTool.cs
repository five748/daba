
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public enum TimeUnitEnum
{
    Default,
    Sec,
    Min,
    Hour,
    Day,
    Month,
    Year
}
public static class TimeTool
{
    public static void SetDiffToSer(int serTime)
    {
        DiffToSer = serTime - GameTime.ConvertDateTimeToIntSpc(DateTime.UtcNow);
        //Debug.LogError("DiffToSer:" + DiffToSer);
    }
    public static int DiffToSer;
    public static int DaySceconds = 86400;
    public static DateTime SerNowTime
    {
        get
        {
            return DateTime.Now.AddSeconds(DiffToSer);
        }
    }
    //秒
    public static DateTime SerUtcTime
    {
        get
        {
            return DateTime.UtcNow.AddSeconds(DiffToSer);
        }
    }
    public static int SerNowUtcTimeInt//零时区
    {
        get
        {
            return GameTime.ConvertDateTimeToIntSpc(SerUtcTime);
        }
    }
    public static int GetUtcTimeInt
    {
        get
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt32(ts.TotalSeconds);
        }
    }
    public static int SerNowTimeInt//当前时间戳
    {
        get
        {
            return GameTime.ConvertDateTimeToIntSpc(SerNowTime);
        }
    }

    public static bool isInTime(this DateTime targetTime, uint begin, uint end)
    {
        int time = GameTime.ConvertDateTimeToIntSpc(targetTime);
        return begin <= time && time < end;
    }
    public static bool isEnd(this long end)
    {
        int time = GameTime.ConvertDateTimeToIntSpc(SerUtcTime);
        return time >= end;
    }
    public static bool isInTime(this DateTime targetTime, uint begin)
    {

        int time = GameTime.ConvertDateTimeToIntSpc(targetTime);
        return time > begin;
    }
    public static bool isInAceBtTime(this string targetTime, uint begin)
    {
        return uint.Parse(targetTime) > begin;
    }
    public static string SetTimeRang(long begin, long end)
    {
        return GameTime.GetStampString(begin, true, false) + "-" + GameTime.GetStampString(end, true, false);
    }
    public static string CutTimeSecond(long second, TimeUnitEnum enums = TimeUnitEnum.Default)
    {
        string str = "";
        if (enums == TimeUnitEnum.Default)
        {
            str = string.Format("{0:D2}", second % 60);
            second /= 60;
            str = string.Format("{0:D2}:{1}", second % 60, str);
            second /= 60;
            str = string.Format("{0:D2}:{1}", second % 24, str);
            second /= 24;
            if (second > 0)
                str = "{0}天{1}{2}".ChangeTextStr3(second, "", str);
            return str;
        }

        if (enums == TimeUnitEnum.Hour)
        {
            if (second > 0)
            {
                if (second / 60 / 60 / 24 < 1)
                {
                    str = "{0}小时".ChangeTextStr1(second / 60 / 60 % 24);
                }
                else
                {
                    str = "{0}天{1}小时".ChangeTextStr2(second / 60 / 60 / 24, second / 60 / 60 % 24);
                }
            }
            return str;
        }

        return "";
    }
    public static string CutTimeNowToEnd(System.DateTime endTime, TimeUnitEnum enums = TimeUnitEnum.Default)
    {
        TimeSpan time = endTime - SerNowTime;
        if (time.TotalSeconds <= 0)
        {
            return "0";
        }
        string str = "";
        if (enums == TimeUnitEnum.Default)
        {
            if (time.Days > 0)
                str += time.Days + "天".ChangeTextStr();
            if (time.Hours < 10 && time.Hours > 0)
                str += "0";
            if (time.Hours > 0)
                str += time.Hours + ":";
            if (time.Minutes < 10)
                str += "0";
            str += time.Minutes + ":";
            if (time.Seconds < 10)
                str += "0";
            str += time.Seconds + "";
            return str;
        }

        str = GetBeforeZero(time.Seconds) + time.Seconds.ToString();
        if (enums.CompareTo(TimeUnitEnum.Min) >= 0)
        {
            str = GetBeforeZero(time.Minutes) + time.Minutes + ":" + str;
        }
        if (enums.CompareTo(TimeUnitEnum.Hour) >= 0)
        {
            str = GetBeforeZero(time.Hours) + time.Hours + ":" + str;
        }
        return str;
    }
    static string GetBeforeZero(int number)
    {
        return number < 10 ? "0" : "";
    }

    public static string CutTimeNowToBegin(System.DateTime beginTime, System.DateTime endTime)
    {
        TimeSpan timeend = endTime - SerNowTime;
        if (timeend.TotalSeconds <= 0)
        {
            return "0";
        }
        TimeSpan time = SerNowTime - beginTime;
        string str = "";
        if (time.Days > 0)
            str += time.Days + "天".ChangeTextStr();
        if (time.Hours < 10)
            str += "0";
        str += time.Hours + ":";
        if (time.Minutes < 10)
            str += "0";
        str += time.Minutes + ":";
        if (time.Seconds < 10)
            str += "0";
        str += time.Seconds + "";
        return str;
    }
    public static string CutTimeNowToEnd(System.DateTime endTime, out uint leftTime)
    {
        TimeSpan time = endTime - SerNowTime;
        leftTime = (uint)(time.TotalSeconds * 10000);
        if (time.TotalSeconds <= 0)
        {
            return "0";
        }
        string str = "";
        if (time.Days > 0)
            str += time.Days + "天".ChangeTextStr();
        if (time.Hours < 10)
            str += "0";
        str += time.Hours + ":";
        if (time.Minutes < 10)
            str += "0";
        str += time.Minutes + ":";
        if (time.Seconds < 10)
            str += "0";
        str += time.Seconds + "";
        return str;
    }
    public static int DisEndTime(this int begin, int end)
    {
        System.DateTime beginTime = GameTime.GetDateTimeFrom1970Ticks(begin);
        System.DateTime endTime = GameTime.GetDateTimeFrom1970Ticks(end);
        TimeSpan time = endTime - beginTime;
        return (int)time.TotalSeconds;
    }
    public static int DayEndTime(this int begin, int end)
    {
        System.DateTime beginTime = GameTime.GetDateTimeFrom1970Ticks(begin);
        System.DateTime endTime = GameTime.GetDateTimeFrom1970Ticks(end);
        TimeSpan time = endTime - beginTime;
        return (int)time.TotalDays;
    }
    public static Coroutine CutTime(this GameObject go, int endtime, System.Action<string> refreshTime, System.Action end = null, Func<System.DateTime, string> timeToStringFunc = null)
    {
        string str = "";
        System.DateTime endTime = GameTime.GetDateTimeFrom1970Ticks(endtime);
        return go.CutTime(endTime, refreshTime, end, timeToStringFunc);
    }
    public static Coroutine CutTime(this GameObject go, int endtime, float intervalTime, System.Action<string> refreshTime, System.Action end = null, Func<System.DateTime, string> timeToStringFunc = null)
    {
        string str = "";
        System.DateTime endTime = GameTime.GetDateTimeFrom1970Ticks(endtime);
        return go.CutTime(endTime, intervalTime, refreshTime, end, timeToStringFunc);
    }
    public static Coroutine CutTime(this GameObject go, System.DateTime endTime, System.Action<string> refreshTime, System.Action end = null, Func<System.DateTime, string> timeToStringFunc = null)
    {
        string str = "";
        endTime = endTime.AddHours(GameTime.Tzong);
        return MonoTool.Instance.StartCor(0.5f,
            () =>
            {
                if (!go)
                {
                    return false;
                }
                if (timeToStringFunc == null)
                {
                    str = CutTimeNowToEnd(endTime);
                }
                else
                {
                    str = timeToStringFunc(endTime);
                }
                if (refreshTime != null)
                {
                    refreshTime(str);
                }
                return str != "0";
            }, () =>
            {
                if (!go)
                    return;
                if (end != null)
                {
                    end();
                }
            });
    }
    public static Coroutine CutTime(this GameObject go, System.DateTime endTime, float intervalTime, System.Action<string> refreshTime, System.Action end = null, Func<System.DateTime, string> timeToStringFunc = null)
    {
        string str = "";
        endTime = endTime.AddHours(GameTime.Tzong);
        return MonoTool.Instance.StartCor(intervalTime,
            () =>
            {
                if (!go)
                {
                    return false;
                }
                if (timeToStringFunc == null)
                {
                    str = CutTimeNowToEnd(endTime);
                }
                else
                {
                    str = timeToStringFunc(endTime);
                }
                if (refreshTime != null)
                {
                    refreshTime(str);
                }
                return str != "0";
            }, () =>
            {
                if (!go)
                    return;
                if (end != null)
                {
                    end();
                }
            });
    }
    public static Coroutine CutTimeRuturnLiveTime(this GameObject go, int endtime, System.Action<double> LiveTime, System.Action end)
    {
        System.DateTime endTime = GameTime.GetDateTimeFrom1970Ticks(endtime);
        return go.CutTimeRuturnLiveTime(endTime, LiveTime, end);
    }
    public static Coroutine CutTimeRuturnLiveTime(this GameObject go, System.DateTime endTime, System.Action<double> LiveTime, System.Action end)
    {
        string str = "";
        endTime = endTime.AddHours(GameTime.Tzong);
        double liveSecond = 0;
        return MonoTool.Instance.StartCor(
            () =>
            {
                if (!go)
                {
                    return false;
                }
                liveSecond = (endTime - SerNowTime).TotalSeconds;
                LiveTime(liveSecond);
                return liveSecond >= 0;
            }, () =>
            {
                if (!go)
                    return;
                if (end != null)
                {
                    end();
                }
            });
    }
    public static Coroutine AddTime(this GameObject go, int begintime, int endtime, System.Action<string, int> refreshTime, System.Action end = null)
    {
        string str = "";
        System.DateTime beginTime = GameTime.GetDateTimeFrom1970Ticks(begintime);
        System.DateTime endTime = GameTime.GetDateTimeFrom1970Ticks(endtime);
        return go.AddTime(beginTime, endTime, refreshTime, end);
    }
    public static Coroutine AddTime(this GameObject go, System.DateTime beginTime, System.DateTime endTime, System.Action<string, int> refreshTime, System.Action end = null)
    {
        string str = "";
        beginTime = beginTime.AddHours(GameTime.Tzong);
        var newEndTime = endTime.AddHours(GameTime.Tzong);
        bool isOver = false;
        return MonoTool.Instance.StartCor(1f,
            () =>
            {
                if (!go)
                {
                    return false;
                }
                str = CutTimeNowToBegin(beginTime, newEndTime);
                TimeSpan timeend = SerNowTime - beginTime;
                isOver = str == "0";
                if (!isOver)
                {
                    refreshTime(str, (int)timeend.TotalSeconds);
                }
                return !isOver;
            }, () =>
            {
                if (!go)
                    return;
                if (end != null)
                {
                    end();
                }
            });
    }
    public static void Stop(this Coroutine co)
    {
        if (co != null)
        {
            //Debug.LogError("StopCo");
            MonoTool.Instance.StopCoroutine(co);
            co = null;
        }
    }
    public static string CutTimeNowToEndCutFirst(System.DateTime endTime)
    {
        TimeSpan time = endTime - SerNowTime;
        if (time.TotalSeconds <= 0)
        {
            return "0";
        }
        string str = "";
        if (time.Days > 0)
            str += time.Days + "天".ChangeTextStr();
        if (time.Hours > 0)
        {
            if (time.Hours < 10)
                str += "0";
            str += time.Hours + ":";
        }
        if (time.Minutes < 10)
            str += "0";
        str += time.Minutes + ":";
        if (time.Seconds < 10)
            str += "0";
        str += time.Seconds + "";
        return str;
    }
    public static string ToActivityOverTime(this long targetTime)
    {
        return GameTime.GetStampStringCutDHM(targetTime);
    }





    //当前时间秒
    public static long GetCurTimeStampSecond()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }

    //当前时间戳
    public static long GetCurTimeStampMilliseconds()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }
    //获取时间字符串,传入秒
    public static string GetHourMinSecTime(long time)
    {
        int hour = (int)time / 3600;
        int minute = (int)time % 3600 / 60;
        int second = (int)time % 3600 % 60;
        return $"{hour:D2}:{minute:D2}:{second:D2}";
    }
    //获取时间字符串,传入秒
    public static string GetHourMinTime(long time)
    {
        int hour = (int)time / 3600;
        int minute = (int)time % 3600 / 60;
        return $"{hour:D2}:{minute:D2}";
    }
    //获取时间字符串,传入秒
    public static string GetMinSecTime(long time)
    {
        if (time < 0)
        {
            return "00:00";
        }
        int minute = (int)time / 60;
        int second = (int)time % 3600 % 60;
        return $"{minute:D2}:{second:D2}";
    }

    //获取时间字符串,传入秒
    public static string GetMinSecTime2(long time)
    {
        if (time < 0)
        {
            return "00:00";
        }
        int minute = (int)time / 60;
        int second = (int)time % 3600 % 60;
        return $"{minute:D2}分{second:D2}秒";
    }
    //获取今日0点时间秒数
    public static long GetTodayZeroTime()
    {
        DateTime now = SerNowTime;
        DateTime t = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 0);
        return new DateTimeOffset(t).ToUnixTimeSeconds();
    }
    public static int GetTodayZeroTimeInt()
    {
        DateTime now = SerUtcTime;
        DateTime t = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 0);
        return GameTime.ConvertDateTimeToInt(t.AddHours(-GameTime.Tzong));
    }
    public static int GetTodayTimeInt(int hour)
    {
        DateTime now = SerUtcTime;
        DateTime t = new DateTime(now.Year, now.Month, now.Day, hour, 0, 0, 0);
        return GameTime.ConvertDateTimeToInt(t.AddHours(-GameTime.Tzong));
    }
    public static int GetTodayZeroTime(int hour)
    {
        DateTime now = SerNowTime;
        DateTime t = new DateTime(now.Year, now.Month, now.Day, hour, 59, 59, 0);
        return (int)(new DateTimeOffset(t).ToUnixTimeSeconds());
    }
    public static int GetWeekLeavyDay()
    {
        return 6 - (int)SerNowTime.DayOfWeek;
    }

    //获取今日整点时间秒数
    public static int GetTodayHourTime(int hour)
    {
        DateTime now = DateTime.UtcNow;
        DateTime t = new DateTime(now.Year, now.Month, now.Day, hour - 1, 59, 59, 0);
        return (int)(new DateTimeOffset(t).ToUnixTimeSeconds());
    }


    //根据时间搓获取改时间搓几天后的零点时间
    public static int GetDayEndTime(int startSec, int day)
    {
        var todayzero = GetTodayZeroTime();
        var now = GetCurTimeStampSecond();
        var day1 = startSec / 3600 / 24;//时间戳天数
        var day2 = now / 3600 / 24;//当前天数
        return (int)(todayzero + (day - (day2 - day1)) * 24 * 60 * 60);
    }



    /// <summary>
    /// 秒数转分钟
    /// </summary>
    /// <param name="seconds">秒</param>
    /// <returns></returns>
    public static string FormatTime(int duration)
    {
        int h = (duration / 60 / 60 % 24);

        string sh = h < 10 ? "0" + h : "" + h;

        int m = (duration / 60 % 60);
        string sm = m < 10 ? "0" + m : "" + m;

        int s = (duration % 60);
        string ss = s < 10 ? "0" + s : "" + s;

        string str = string.Format("{0}:{1}", sm, ss);

        return str;
    }

    /// <summary>
    /// 当前时间秒数
    /// </summary>
    /// <value></value>
    public static int CurTimeSeconds
    {
        get
        {
            return (int)new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }

    public static bool IsSameDay(int time_a, int time_b)
    {
        int day_a = time_a / 86400;
        int day_b = time_b / 86400;
        return day_a - day_b == 0;
    }
}
