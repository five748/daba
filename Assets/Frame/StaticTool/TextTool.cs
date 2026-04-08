using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using LitJson;
public class FlagData
{
    public int BgOrder;
    public int VeinsOrder;
    public int DesignOrder;
    public string BgHSV;
    public string VeinsHSV;
    public string DesignHSV;
}
public static class TextTool
{
    private static string[] NumIndex = {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
        "k", "l", "m", "n", "o", "p", "q", "r", "s", "t",
        "u", "v", "w", "x", "y", "z", "!", "#", "$", "%",
        "&", "P", "(", ")", "*", "+", ",", "-", ".", ":",
        ";", "<", "=", ">", "?", "@", "A", "B", "C", "D",
        "E", "F", "G", "H", "I", "K", "L", "M", "N", "O",
    };
    private static Dictionary<string, int> NumCount = new Dictionary<string, int>();

    public static string GzipToString(UnityWebRequest res)
    {
        if (res.GetResponseHeader("Content-Encoding") != null && res.GetResponseHeader("Content-Encoding").ToLower().Equals("gzip"))
        {
            Stream ff = new GZipStream(new MemoryStream(res.downloadHandler.data), CompressionMode.Decompress);
            using (StreamReader reader = new StreamReader(ff, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        return res.downloadHandler.text;
    }

    //写入文本并且变为UTF8格式
    public static void WriteByUTF8(this string path, string myString)
    {
        path.CreateDirAndFile();
        UTF8Encoding m_utf8 = new UTF8Encoding(false);
        StreamWriter stream = new StreamWriter(path, false, m_utf8);
        UTF8Encoding utf8 = new UTF8Encoding(); // Create a UTF-8 encoding.
        byte[] bytes = utf8.GetBytes(myString.ToCharArray());
        string EnUserid = utf8.GetString(bytes);
        stream.Write(EnUserid);
        stream.Flush();
        stream.Close();
    }
    public static void WriteByUTF8ByTablePath(this string path, string myString)
    {
        path = Application.dataPath + "/" + FrameConfig.TablePath + path + ".txt";
        path.WriteByUTF8(myString);
    }
    public static string ReadByUTF8(this string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("路径不存在:" + path);
            return "";
        }
        byte[] buf = ReadText(path);
        UTF8Encoding utf8 = new UTF8Encoding();
        string EnUserid = utf8.GetString(buf).Trim();
        return EnUserid;
    }
    //读取txt
    public static byte[] ReadText(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] data = new byte[fs.Length];
        fs.Read(data, 0, data.Length);
        fs.Flush();
        fs.Close();
        return data;
    }
    public static void ReadStringByLine(this string fullpath, System.Action<string> StrEvent)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        StreamReader sr = new StreamReader(fullpath, utf8);
        string line;
        var all = sr.ReadToEnd();
        sr.Close();
        sr = new StreamReader(fullpath, utf8);
        while ((line = sr.ReadLine()) != null)
        {
            StrEvent(line);
        }
        //sr.Flush();
        sr.Close();
    }
    public static void AddNameSpace(this string path, string name)
    {
        path = path.Replace(@"\", "/");
        //Debug.Log(path);
        string str = "";
        bool isInserted = false;
        string className = path.GetSpitLast('/').Split('.')[0];
        bool isHaveOldNameSpace = false;
        string space = "    ";
        ReadStringByLine(path, line =>
        {
            if (line.IndexOf("namespace") != -1)
            {
                str += "namespace " + name + "{\n";
                isInserted = true;
                isHaveOldNameSpace = true;
                space = "";
                return;
            }
            if (isInserted)
            {
                str += space + line + "\n";
            }
            else
            {
                if (line.IndexOf("public class " + className) != -1)
                {
                    isInserted = true;
                    str += "namespace " + name + "{\n    ";
                }
                if (line.IndexOf("public enum " + className) != -1)
                {
                    isInserted = true;
                    str += "namespace " + name + "{\n    ";
                }
                if (line.IndexOf("public struct " + className) != -1)
                {
                    isInserted = true;
                    str += "namespace " + name + "{\n    ";
                }
                str += line + "\n";
            }
        });
        if (!isHaveOldNameSpace)
            str += "}";
        WriteByUTF8(path, str);
    }
    public static void AddSerializable(this string path, bool isHaveNameSpace)
    {
        path = path.Replace(@"\", "/");
        string str = "";
        bool isInserted = false;
        string className = path.GetSpitLast('/').Split('.')[0];
        string space = "    ";
        bool isEndClassName = false;
        ReadStringByLine(path, line =>
        {
            if (!isEndClassName)
            {
                if (line.Contains("Serializable]"))
                {
                    return;
                }
            }
            if (isInserted)
            {
                str += space + line + "\n";
            }
            else
            {
                if (line.IndexOf("public class " + className) != -1 ||
                    line.IndexOf("public enum " + className) != -1||
                    line.IndexOf("public struct " + className) != -1
                    )
                {
                    isInserted = true;
                    if (isHaveNameSpace)
                    {
                        str += "   [System.Serializable]\n";
                    }
                    else {
                        str += "   [System.Serializable]";
                    }
                }
                str += line + "\n";
            }
        });
        WriteByUTF8(path, str);
    }
    public static void GetData(this string str, char[] param)
    {
        for (int i = 0; i < param.Length; i++)
        {
            str.Split(param);
        }
    }
    public static List<int> ToListInt(this string str, char c)
    {
        string[] strs = str.Split(c);
        List<int> lst = new List<int>();
        for (int i = 0; i < strs.Length; i++)
        {
            lst.Add(int.Parse(strs[i]));
        }
        return lst;
    }
    public static List<List<int>> ToListListInt(this string str, char cBig, char c2)
    {
        string[] strs = str.Split(cBig);
        List<List<int>> lst = new List<List<int>>();
        for (int i = 0; i < strs.Length; i++)
        {
            lst.Add(ToListInt(strs[i], c2));
        }
        return lst;
    }
    public static Dictionary<int, int> ToMapInt(this string str, char cBig, char c2)
    {
        string[] strs = str.Split(cBig);
        Dictionary<int, int> dic = new Dictionary<int, int>();
        for (int i = 0; i < strs.Length; i++)
        {
            var lst = ToListInt(strs[i], c2);
            dic.Add(lst[0], lst[1]);
        }
        return dic;
    }
    public static List<float> ToListFloat(this string str, char c)
    {
        string[] strs = str.Split(c);
        List<float> lst = new List<float>();
        for (int i = 0; i < strs.Length; i++)
        {
            lst.Add(float.Parse(strs[i]));
        }
        return lst;
    }
    public static Vector2 StringToVector2(this string str)
    {
        str = str.Replace("(", " ").Replace(")", " ");
        string[] s = str.Split(',');
        return new Vector2(float.Parse(s[0]), float.Parse(s[1]));
    }
    public static Vector3 StringToVector3(this string str)
    {
        str = str.Replace("(", " ").Replace(")", " ");
        string[] s = str.Split(',');
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }
    public static Vector3 StringToVector3(this string str, char splitStr)
    {
        str = str.Replace("(", " ").Replace(")", " ");
        string[] s = str.Split(splitStr);
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }
    public static Vector3 StringToVector3Base(this string str)
    {
        string[] s = str.Split(':');
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }
    public static List<long> ToListLong(this string str, char c)
    {
        string[] strs = str.Split(c);
        List<long> lst = new List<long>();
        for (int i = 0; i < strs.Length; i++)
        {
            lst.Add(long.Parse(strs[i]));
        }
        return lst;
    }
    public static string CutFirstByChar(this string str, char c)
    {
        if (!str.Contains(c))
        {
            return str;
        }
        string[] strs = str.Split(c);
        str = "";
        for (int i = 1; i < strs.Length; i++)
        {
            str += strs[i] + c;
        }
        return str.CutLast();
    }
    public static string SetFirstToUpper(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return "";
        }
        return str.Substring(0, 1).ToUpper() + str.Substring(1);
    }
    public static string SetGoType(this string str)
    {
        switch (str)
        {
            case "byte":
                return "int";
            case "int":
                return "int";
            case "long":
                return "int64";
            case "float":
                return "float64";
            case "string":
                return "string";
        }
        return str;
    }
    public static bool IsBaseType(this string str)
    {
        switch (str)
        {
            case "byte":
                return true;
            case "int":
                return true;
            case "long":
                return true;
            case "float":
                return true;
            case "string":
                return true;
        }
        return false;
    }
    public static string SetProtoBufType(this string str)
    {
        switch (str)
        {
            case "byte":
                return "int32";
            case "int":
                return "int32";
            case "long":
                return "int64";
            case "float":
                return "double";
            case "string":
                return "string";
        }
        return str;
    }
    public static void ReplaceClassFun(this string path, string funName, string newBody)
    {
        path.ReplaceClassFunToOther(path, funName, newBody);
    }
    public static void ReplaceClassFunToOther(this string path, string writePath, string funName, string newBody)
    {
        writePath.WriteByUTF8(path.GetClassStrByRepaceFun(funName, newBody));
    }


    public static string GetClassStrByRepaceFun(this string path, string funName, string newBody)
    {
        string str = "";
        bool isTarget = false;
        int index = -1;
        path.ReadStringByLine(line =>
        {
            if (isTarget)
            {
                if (line.Contains("{"))
                {
                    index++;
                }
                if (line.Contains("}"))
                {
                    index--;
                    if (index == -1)
                    {
                        isTarget = false;
                    }
                }
            }
            if (isTarget)
            {
                return;
            }
            str += line + "\n";
            if (line.Contains(" " + funName + "(") && !line.Contains(";"))
            {
                if (line.Contains("{"))
                {
                    index++;
                }
                else
                {
                    str += "    {" + "\n";
                }
                isTarget = true;
                str += newBody + "\n";
            }
        });
        return str;
    }    
    public static List<Vector2> SplitToPos(this string str)
    {
        List<Vector2> lst = new List<Vector2>();
        if (string.IsNullOrEmpty(str))
        {
            return lst;
        }
        string[] strs = str.Split('|');
        for (int i = 0; i < strs.Length; i++)
        {
            var strss = strs[i].Split('_');
            lst.Add(new Vector2(float.Parse(strss[0]), float.Parse(strss[1])));
        }
        return lst;
    }
    public static string CutNoUse(this string str)
    {
        return str.Replace("|♩-1", "");
    }



    public static string GetProcessAwardByScore(this string process, long score)
    {
        var processArr = process.Split('^');
        for (int i = 0; i < processArr.Length; i++)
        {
            var itemArr = processArr[i].Split('&');
            if (score == long.Parse(itemArr[0]))
            {
                return itemArr[1];
            }
        }
        return string.Empty;

    }

    public static string GetProcessAwardByIndex(this string process, int index)
    {
        var processArr = process.Split('^');

        var str = index >= processArr.Length - 1 ? processArr[processArr.Length - 1] : processArr[index];

        return str.Split('&')[1];
    }


    public static string GetUseStr(this string str, int Times)
    {
        string resource = "";
        Times++;
        foreach (var item in str.Split('^'))
        {
            var _item = item.Split('&');
            var use = _item[0].Split('_');
            if (Times >= int.Parse(use[0]) && Times <= int.Parse(use[1]))
            {
                resource = _item[1];
                break;
            }
        }
        return resource;
    }
    public static float GetRate(List<int> rates, int nowRate)
    {
        float jindu = 0;
        var endRate = rates[rates.Count - 1];
        for (int i = 0; i < rates.Count; i++)
        {
            if (nowRate < rates[i])
            {
                int frontRate = rates[i - 1];
                int behindRate = rates[i];
                float jindu1 = (float)(i - 1) / (rates.Count - 1);
                float jindu2 = (float)i / (rates.Count - 1);
                float jindu3 = (float)(nowRate - frontRate) / (behindRate - frontRate);
                float jindu4 = (jindu2 - jindu1) * jindu3;
                jindu = jindu1 + jindu4;
                break;
            }
            if (nowRate == rates[i])
            {
                jindu = (float)i / (rates.Count - 1);
                break;
            }
        }
        return jindu;
    }
    public static bool IsResourceProp(this int id)
    {
        if (id > 300000 && id < 310000)
            return true;

        if (id > 320000 && id < 330000)
            return true;

        return false;
    }


    public static int getSex(this int iconId)
    {
        if (0 < iconId && iconId < 4)
        {
            return 1;
        }
        return 0;
    }
    public static bool HaveRole(this int spineId)
    {
        if (spineId >= 130000 && spineId < 140000)
        {
            return true;
        }
        if (spineId >= 1300000 && spineId < 1400000)
        {
            return true;
        }
        return false;
    }
    public static string GetRoleIconBg(this int iconBgId)
    {
        return "iconbg/" + iconBgId;
    }


    public static string GetUnionIcon(this int iconId)
    {
        if (iconId == 0)
        {
            iconId = 1;
        }
        return "unionflag/" + iconId;
    }

    public static DateTime ToDateTime(this long time)
    {
        return GameTime.GetDateTimeFrom1970Ticks(time);
    }
    private static float minSpeedX = 5f;
    private static float minSpeedY = 5f;
    private static float MaxSpeedX = 10f;
    private static float MaxSpeedY = 10f;
    public static int MoidfyShipSpeed(this int shipSpeed)
    {
        return (int)((MaxSpeedX - minSpeedX) / 200.0f * shipSpeed + minSpeedX);
    }

    public static Vector2 ToVector2(this string str)
    {
        var sts = str.Split('_');
        return new Vector2(float.Parse(sts[0]), float.Parse(sts[1]));
    }
    public static void InitItems<T>(this Transform awardgrid, Dictionary<int, T> dic, System.Action<Transform, T, int> action, Vector2 pos = new Vector2())
    {
        var keys = dic.GetKeys();
        var loop = awardgrid.GetComponent<LoopScroll>();
        loop.space = new Vector2(24.8f, 5.9f);
        loop.InitItems(dic.Count, (tran, index, isOnlyGetSize) =>
        {
            if (!isOnlyGetSize)
            {
                tran.name = index.ToString();
                action(tran, dic[keys[index]], index);
            }
            return loop.ItemDefaultSize;
        });
    }

    public static void InitItems<T>(this Transform awardgrid, List<T> dayAwardList, System.Action<Transform, T, int> action)
    {
        var loop = awardgrid.GetComponent<LoopScroll>();
        loop.space = new Vector2(24.8f, 5.9f);
        loop.InitItems(dayAwardList.Count, (tran, index, isOnlyGetSize) =>
        {
            if (!isOnlyGetSize)
            {
                tran.name = index.ToString();
                action(tran, dayAwardList[index], index);
            }
            return loop.ItemDefaultSize;
        });
    }
    public static void InitItems<T>(this Transform awardgrid, Dictionary<int, T> dic, System.Action<Transform, T, int> action)
    {
        var keys = dic.GetKeys();
        var loop = awardgrid.GetComponent<LoopScroll>();
        loop.space = new Vector2(24.8f, 5.9f);
        loop.InitItems(dic.Count, (tran, index, isOnlyGetSize) =>
        {
            if (!isOnlyGetSize)
            {
                tran.name = index.ToString();
                action(tran, dic[keys[index]], index);
            }
            return loop.ItemDefaultSize;
        });
    }
    public static bool IntGetOne(this int n, int index)
    {
        return (n & (1 << index)) > 0;
    }
    public static int StringGetOne(this string str, int index)
    {
        if (NumCount.Count == 0)
        {
            InitNumCount();
        }
        if (str.Length <= index)
            return 0;
        var c = str[index].ToString();
        if (NumCount.ContainsKey(c))
        {
            return NumCount[c];
        }
        return 0;
    }
    private static void InitNumCount()
    {
        for (int i = 0; i < NumIndex.Length; i++)
        {
            if (!NumCount.ContainsKey(NumIndex[i]))
                NumCount.Add(NumIndex[i], i);
        }
    }
    public static string StringSetOne(this string str, int index, int num)
    {
        char[] chars = str.ToCharArray();
        ArrayList list = new ArrayList();
        for (int i = 0; i < chars.Length; i++)
        {
            list.Add(chars[i]);
        }
        if (str.Length <= index)
        {
            for (int i = str.Length; i < index + 1; i++)
            {
                list.Add('0');
            }
        }

        char c = '0';
        if (num < NumIndex.Length)
        {
            c = NumIndex[num][0];
        }
        list.RemoveAt(index);
        list.Insert(index, c.ToString());
        var end = new char[list.Count];
        for (int i = 0; i < end.Length; i++)
        {
            end[i] = Convert.ToChar(list[i]);
        }
        // (char[])list.ToArray(typeof(char));
        return new string(end);
    }
    public static int GetStoryIdInt(this string str)
    {
        var strInt = str.GetStoryId();
        if (!strInt.IsInt())
        {
            return 0;
        }
        return int.Parse(strInt);
    }
    public static string GetStoryId(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return "0";
        }
        string[] strs = str.Split('-');
        if (int.Parse(strs[1]) < 10)
        {
            strs[1] = "0" + strs[1];
        }
        if (int.Parse(strs[2]) < 10)
        {
            strs[2] = "0" + strs[2];
        }
        return strs[0] + strs[1] + strs[2];
    }
    public static string GetStrStoryId(this int id)
    {
        string str = id.ToString();
        int len = str.Length;
        var one = int.Parse(str.Substring(0, len - 4));
        var two = int.Parse(str.Substring(len - 4, 2));
        var three = int.Parse(str.Substring(len - 2));
        return one + "-" + two + "-" + three;
    }
    public static string MD5Setting(string str)
    {
        StringBuilder sb = new StringBuilder();
        using (MD5 md5 = MD5.Create())
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] md5Bytes = md5.ComputeHash(bytes);
            for (int i = 0; i < md5Bytes.Length; i++)
            {
                sb.Append(md5Bytes[i].ToString("X2"));
            }
        }
        return sb.ToString();
    }

    public static long[] GetProcessState(this string process, long lv, int lastLevel)
    {
        var processArr = process.Split('^');
        long bengin = 0;
        long end = 0;
        var before = (long)0;
        var state = 0;
        long limit = 0;
        for (int i = 0; i < processArr.Length; i++)
        {
            var itemArr = processArr[i].Split('&');
            var lvLimit = long.Parse(itemArr[0]);
            end = lvLimit - before;

            if (i == lastLevel)
            {
                bengin = lv >= lvLimit ? end : lv - before;
                state = lv >= lvLimit ? 1 : 0;
                limit = lvLimit;
                return new long[] { state, bengin, end, limit };
            }
            else
            {
                if (i == processArr.Length - 1)
                {
                    return new long[] { 2, end, end, limit };
                }
            }

            before = lvLimit;
        }
        return new long[] { state, bengin, end, limit };
    }
    public static long[] GetEquipOrInsProcessState(this string process, long score, int lastLevel)
    {
        var processArr = process.Split('^');
        long bengin = 0;
        long end = 0;
        var before = (long)0;
        var state = 0;
        for (int i = 0; i < processArr.Length; i++)
        {
            var itemArr = processArr[i].Split('&');
            var itemScore = long.Parse(itemArr[0]);
            end = itemScore - before;

            if (i == lastLevel)
            {
                bengin = score >= itemScore ? end : score - before;
                state = score >= itemScore ? 1 : 0;
                return new long[] { state, bengin, end };
            }
            else
            {
                if (i == processArr.Length - 1)
                {
                    return new long[] { 2, end, end };
                }
            }

            before = itemScore;
        }

        return new long[] { state, bengin, end };
    }
    public static string ToMStr(this long size)
    {
        return (size / 1048576.0f).ToString("F1") + "M";
    }
    public static int ToInt(this string str)
    {
        if (str.IsInt())
        {
            return int.Parse(str);
        }
        return -1;
    }
    public static string NumberToChinese(this int number)
    {

        string[] UNITS = { "", "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百", "千" };
        string[] NUMS = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        if (number == 0)
        {
            return NUMS[0];
        }
        string results = "";
        for (int i = number.ToString().Length - 1; i >= 0; i--)
        {
            int r = (int)(number / (Math.Pow(10, i)));
            results += NUMS[r % 10] + UNITS[i];
        }
        results = results.Replace("零十", "零")
                         .Replace("零百", "零")
                         .Replace("零千", "零")
                         .Replace("亿万", "亿");
        results = Regex.Replace(results, "零([万, 亿])", "$1");
        results = Regex.Replace(results, "零+", "零");

        if (results.StartsWith("一十"))
        {
            results = results.Substring(1);
        }
    cutzero:
        if (results.EndsWith("零"))
        {
            results = results.Substring(0, results.Length - 1);
            if (results.EndsWith("零"))
            {
                goto cutzero;
            }
        }
        return results;
    }
    public static string HourTurnToDays(this int hour)
    {
        int day = hour / 24;
        int hourStr = hour % 24;
        if (day == 0)
        {
            return hourStr + "小时".ChangeTextStr();
        }
        else if (hourStr == 0)
        {
            return day + "天".ChangeTextStr();
        }
        else
            return day + "天".ChangeTextStr() + hourStr + "小时".ChangeTextStr();

    }
}