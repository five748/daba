
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System;
public static class StringTool
{
    #region 简转繁体工具

    /// <summary>
    /// 中文字符工具类
    /// </summary>
    public static class ChineseStringUtility
    {
        private const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        private const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        private const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

        /// <summary>
        /// 讲字符转换为繁体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToTraditional(string source)
        {
            String target = new String(' ', source.Length);
            int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, source, source.Length, target, source.Length);
            return target;
        }
    }
    /// <summary>
    /// 转为繁体中文
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static string ToTraditional(this string origin)
    {
        return ChineseStringUtility.ToTraditional(origin);
    }

    #endregion

    public static string Trim(string myString)
    {
        return myString.Trim();
    }
    public static string CutLast(this string str, int cutEndLen = 1)
    {
        if (str.Length < cutEndLen)
        {
            return str;
        }
        return str.Substring(0, str.Length - cutEndLen);
    }
    public static string CutDir(this string str, int num = 1)
    {
        var strs = str.Split('/');
        string newStr = "";
        if (strs.Length < num)
        {
            Debug.LogError("裁剪长度大于最大长度");
            return str;
        }
        for (int i = 0; i < strs.Length - num; i++)
        {
            newStr += strs[i] + "/";
        }
        return newStr;
    }
    public static string CutBeginLast(this string str)
    {
        if (str == "")
        {
            return str;
        }
        return str.Substring(1, str.Length - 2);
    }
    public static string ChangeSpaceFlag(this string str)
    {
        return str.Replace(" ", "♣");
    }
    public static string ChangeLineN(this string str)
    {
        return str.Replace("/n", "\n").Replace("\n", "\n\n");
    }
    public static string ChangeColor(this string str, string color)
    {
        return "<color=#" + color + ">" + str + "</color>";
    }
    public static string ChangeSize(this string str, int size)
    {
        return "<size=" + size + ">" + str + "</size>";
    }
    public static string ChangeSize(this int str, int size)
    {
        return "<size=" + size + ">" + str + "</size>";
    }
    public static string ChangeGreyColor(this string str)
    {
        return "<color=#898989>" + str + "</color>";
    }
    public static string ChangeGreenColor1(this string str)
    {
        return "<color=#009159>" + str + "</color>";
    }
    public static string ChangeGrey(this string str)
    {
        return "<color=#7E7E7E>" + str + "</color>";
    }
    public static string ChangeGreenColor2(this string str)
    {
        return "<color=#28ffac>" + str + "</color>";
    }
    public static string ChangeGreenColor(this string str)
    {
        return "<color=#ade2a7>" + str + "</color>";
    }
    public static string ChangeGreen1Color(this string str)
    {
        return "<color=#00FF01FF>" + str + "</color>";
    }
    public static string ChangeQingColor(this string str)
    {
        return "<color=#9094A7FF>" + str + "</color>";
    }

    public static string ChangeWhiteColor(this string str)
    {
        return "<color=#FFFFFFFF>" + str + "</color>";
    }
    public static string ChangeRedColor(this string str)
    {
        return "<color=#ff7679>" + str + "</color>";
    }
    public static string ChangeYellowColor(this string str)
    {
        return "<color=#fff68d>" + str + "</color>";
    }
    public static string ChangeBlueColor1(this string str)
    {
        return "<color=#373f63>" + str + "</color>";

    }
    public static string ChangeBrownColor(this string str)
    {
        return "<color=#433313>" + str + "</color>";
    }
    public static string ChangePupleColor(this string str)
    {
        return "<color=#d776ff>" + str + "</color>";
    }
    public static string ChangeFadeColor(this string str)
    {
        return "<color=#00000000>" + str + "</color>";
    }
    public static string ChangeBlueColor(this string str)
    {
        return "<color=#7689ff>" + str + "</color>";
    }
    public static string ChangeOrangeColor(this string str)
    {
        return "<color=#ffb376>" + str + "</color>";
    }
    public static string ChangeColorByIndex(this string str, int index)
    {
        if (index == 0)
        {
            return str.ChangeOne();
        }
        if (index == 1)
        {
            return str.ChangeTwo();
        }
        if (index == 2)
        {
            return str.ChangeThree();
        }
        return str;
    }
    public static string ChangeOne(this string str)
    {
        return "<color=#FFFD7BFF>" + str + "</color>";
    }
    public static string ChangeTwo(this string str)
    {
        return "<color=#85CDFFFF>" + str + "</color>";
    }
    public static string ChangeThree(this string str)
    {
        return "<color=#81FF97FF>" + str + "</color>";
    }
    private static string[] typecolor = new string[] {
        "e5e5e5",
        "6cd973",
        "7C8DFFFF",
        "E37AFFFF",
        "FFC162FF",
        "FF5151FF",
    };
    public static string ChangeColorByType(this string str, string itemId, int type)
    {
        return "<color=#" + "" + typecolor[type - 1] + ">" + str + "</color>";
    }
    public static string ChangeColorByQuality(this string str, uint quality)
    {
        //Debug.LogError(quality);
        return "<color=#" + "" + typecolor[quality - 1] + ">" + str + "</color>";
    }

    //名字使用这个颜色
    public static string ChangeYellowName(this string str)
    {
        return "<color=#ef8803>" + str + "</color>";
    }
    //招募那边使用的
    public static string ChangeYellowColor02(this string str)
    {
        return "<color=#ef8803>" + str + "</color>";
    }
    public static string ChangeBuleColor(this string str)
    {
        return "<color=#9FC8FFFF>" + str + "</color>";
    }
    public static string RemoveRichText(this string str)
    {
        while (str.Contains("</color>") || str.Contains("</Color>"))
        {
            var startIndex = str.IndexOf("<");
            var endIndex = str.IndexOf(">");
            str = str.Remove(startIndex, endIndex - startIndex + 1);
        }
        return str;
    }
    public static ulong TouLong(this uint num)
    {
        return (ulong)num;
    }
    public static ulong TouLong(this int num)
    {
        return (ulong)num;
    }
    public static string ChangeNum(this ulong num)
    {
        if (num >= 1000000000)
        {
            return num / 100000000 + "亿";
        }
        if (num >= 100000)
        {
            return num / 10000 + "万";
        }
        return num.ToString();
    }
    public static string ChangeNum(this long num)
    {
        if (num >= 1000000000)
        {
            return num / 100000000 + "亿";
        }
        if (num >= 100000)
        {
            return num / 10000 + "万";
        }
        return num.ToString();
    }
    public static string ChangeNumW(this long num)
    {
        if (num >= 100000)
        {
            return num / 10000 + "万";
        }
        return num.ToString();
    }
    public static string ChangeNumWArt(this long num)
    {
        if (num >= 100000)
        {
            return num / 10000 + "w";
        }
        return num.ToString();
    }
    public static string ChangeNumTenWArt(this long num)
    {
        if (num >= 1000000)
        {
            return num / 10000 + "w";
        }
        return num.ToString();
    }
    public static string ChangeNumTenWArtCut(this long num)
    {
        if (num <= -1000000)
        {
            return num / 10000 + "w";
        }
        return num.ToString();
    }
    public static string ChangeNum(this int num)
    {
        // Debug.Log("真实数目:" + num);
        if (num >= 1000000000)
        {
            return num / 100000000 + "亿";
        }
        if (num >= 100000)
        {
            return num / 10000 + "万";
        }
        return num.ToString();
    }
    public static string ChangeNum(this string num)
    {
        return long.Parse(num).ChangeNum();
    }
    //百分比
    public static string ToPTC(this double num)
    {
        if (num == 1)
        {
            return "100%";
        }
        if (num == 0)
        {
            return "0";
        }
        if (num > 0.9999f)
        {
            return "99.99%";
        }
        if (num < 0.0001f)
        {
            return "0.01%";
        }
        return (num * 100).ToString("f2") + "%";
    }
    //获取文件的前一个文件夹路径
    public static string GetSpitByIndex(this string str, int index, char splitStr)
    {
        string[] strs = str.Split(splitStr);
        string info = "";
        for (int i = index; i < strs.Length; i++)
        {
            info += strs[i] + splitStr;

        }
        return info;
    }
    public static string GetSpitLast(this string str, char splitStr)
    {
        string[] strs = str.Split(splitStr);
        return strs[strs.Length - 1];
    }
    //获取文件的前一个文件夹路径
    public static string GetSpitLastDir(this string str, char splitStr)
    {
        string[] strs = str.Split(splitStr);
        return strs[strs.Length - 2];
    }
    public static string GetSpitLastDirAndFile(this string str, char splitStr)
    {
        string[] strs = str.Split(splitStr);
        return strs[strs.Length - 2] + splitStr + strs[strs.Length - 1];
    }
    public static string GetFileNameByPath(this string str)
    {
        str = str.Replace(@"\", @"/"); ;
        string[] strs = str.Split('/');
        return strs[strs.Length - 1].Split('.')[0];
    }
    public static string GetFileNameByPatHhaveEnd(this string str)
    {
        str = str.Replace(@"\", @"/"); ;
        string[] strs = str.Split('/');
        return strs[strs.Length - 1];
    }
    //获取文件文件夹路径
    public static string GetSpitUpLast(this string str, char splitStr)
    {
        return str.GetSpit(splitStr, 1);
    }
    //获取文件文件夹路径
    public static string GetSpit(this string str, char splitStr, int index)
    {
        string[] strs = str.Split(splitStr);
        str = "";
        for (int i = 0; i < strs.Length - index; i++)
        {
            str += strs[i] + splitStr;
        }
        return str.CutLast();
    }
    //获取文件的前一个文件夹路径
    public static int GetSpitLen(this string str, char splitStr)
    {
        return str.Split(splitStr).Length + str.Length;
    }
    public static string AddKuoHao(this string str)
    {
        return "【" + str + "】";
    }

    public static string Join<T>(this string mark, T[] arr, Func<T, string> func)
    {
        string[] awardss = new string[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            awardss[i] = func(arr[i]);
        }
        return string.Join(mark, awardss);
    }

    public static string AddFileUpDir(this string path)
    {
        string[] strs = path.Split('/');
        path = "";
        for (int i = 0; i < strs.Length; i++)
        {
            path += strs[i] + "/";
            if (i == strs.Length - 2)
            {
                path += "UICommon_" + strs[i] + "_";
            }
        }
        return path.CutLast(1);
    }
    //获取从begin开始的字符串不包括begin
    public static string CutPathByLast(this string myString, int index = 1)
    {
        string[] strs = myString.Split('/');
        string str = "";
        for (int i = 0; i < strs.Length - 1; i++)
        {
            str += strs[i] + "/";
        }
        return str.CutLast();
    }
    //获取从begin开始的字符串包括begin
    public static string CutPathHaveBegin(this string myString, string begin)
    {
        int i = myString.IndexOf(begin);
        return myString.Substring(i);
    }
    public static string GetChinese(this string str)
    {
        for (int i = 0; i < str.Length; i++)
        {

            if (str[i] >= 0x4E00 && str[i] <= 0x9FA5)
            {
                return str.Substring(i);
            }
        }
        return "";
    }
    public static string CutChinese(this string str)
    {
        for (int i = 0; i < str.Length; i++)
        {

            if (str[i] >= 0x4E00 && str[i] <= 0x9FA5)
            {
                return str.Substring(0, i);
            }
        }
        return "";
    }
    public static bool isHaveChinese(this string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (isChinese(str[i]))
            {
                return true;
            }
        }
        return false;
    }
    public static bool isChinese(this char str)
    {
        return str >= 0x4E00 && str <= 0x9FA5;
    }
    public static bool isNum(this char str)
    {

        return str.isNum();
    }
    public static void DelDirectory(this string path)
    {
        if (System.IO.Directory.Exists(path))
            System.IO.Directory.Delete(path, true);
        System.IO.Directory.CreateDirectory(path);
    }
    public static string RemoveNum(this string str)
    {
        string str2 = string.Empty;
        char[] arr = str.ToCharArray();
        for (int i = 0; i < arr.Length; i++)
        {
            if (!(arr[i] >= '0' && arr[i] <= '9'))
            {
                str2 += arr[i];
            }
        }
        return str2;
    }
    public static string GetNum(this string str)
    {
        string str2 = string.Empty;
        char[] arr = str.ToCharArray();
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] >= '0' && arr[i] <= '9')
            {
                str2 += arr[i];
            }
        }
        return str2;
    }
    //删除中文
    public static string RemoveChinese(this string fileName)
    {
        fileName = fileName.Replace(@"\", "/");
        string[] names = fileName.Split('/');
        //UnityEngine.Debug.Log(fileName);
        fileName = "/";
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].IndexOf("_") == -1 || names[i].IndexOf("-") == -1 || names[i].IndexOf("#") == -1)
            {
                fileName += names[i] + "/";
                continue;
            }
            if (names[i].EndsWith(".png"))
            {
                if (names[i].isHaveChinese())
                {
                    string end = "";
                    if (names[i].EndsWith(".png"))
                    {
                        end = ".png";
                    }

                    fileName += names[i].GetSpitUpLast('_') + end + "/";
                }
                else
                {
                    fileName += names[i] + "/";
                }
            }
            else
            {
                fileName += names[i].GetSpitUpLast('_') + "/";
            }
        }
        return StringTool.CutLast(fileName, 1);
    }
    public static string RemoveLastChinese(this string fileName)
    {
        fileName = fileName.Replace(@"\", "/");
        string[] names = fileName.Split('/');
        //UnityEngine.Debug.Log(fileName);
        fileName = "";
        for (int i = 0; i < names.Length; i++)
        {
            if (i < names.Length - 1)
            {
                fileName += names[i] + "/";
                continue;
            }
            if (names[i].IndexOf("_") == -1)
            {
                fileName += names[i] + "/";
                continue;
            }
            if (names[i].EndsWith(".png"))
            {
                if (names[i].isHaveChinese())
                {
                    string end = "";
                    if (names[i].EndsWith(".png"))
                    {
                        end = ".png";
                    }

                    fileName += names[i].GetSpitUpLast('_') + end + "/";
                }
                else
                {
                    fileName += names[i] + "/";
                }
            }
            else
            {
                fileName += names[i].GetSpitUpLast('_') + "/";
            }
        }
        return StringTool.CutLast(fileName, 1);
    }
    //md5字符串
    public static string MD5(this string msg)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }
    public static string ChangeToMinterPath(this string minId)
    {
        return uint.Parse(minId).ChangeToMinterPath();
    }
    public static string ChangeToMinterPath(this uint minId)
    {
        if (minId > 9999)
        {
            if (minId < 11000)
            {
                return "minster/" + minId;
            }
            else if (minId < 12000)
            {
                return "beauty/" + minId;
            }
            else if (minId < 13000)
            {
                return "npc/" + minId;
            }
        }
        return "king/" + minId;
    }
    public static string ChangeToMinterPath(this int minId)
    {
        return ((uint)minId).ChangeToMinterPath();
    }
    public static string RemoveColor(this string str)
    {
        int index = -1;
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        int len = str.Length;

        string onstr = "";
        string oldstr = str;
        int colorIndex = 0;
        while (true)
        {
            index++;
            if (index >= len)
            {
                break;
            }
            if (len - index > 6)
            {
                onstr = str.Substring(index);

                if (onstr.Substring(0, 6) == "<color")
                {
                    colorIndex = onstr.IndexOf(">") + 1;
                    index += colorIndex;
                    //Debug.Log(onstr.Substring(0, colorIndex));
                    oldstr = oldstr.Replace(onstr.Substring(0, colorIndex), "");
                }
            }
            if (len - index > 6)
            {
                onstr = str.Substring(index);
                if (onstr.Substring(0, 6) == "</colo")
                {
                    colorIndex = onstr.IndexOf(">") + 1;
                    index += colorIndex;
                    //Debug.Log(onstr.Substring(0, colorIndex));
                    oldstr = oldstr.Replace(onstr.Substring(0, colorIndex), "");
                }
            }
        }
        return oldstr;
    }
    public static string InsertLine(this string str, int online)
    {
        int len = str.Length;
        //string ss = CodeStrMgr.GetCodeStr("StringTool", 4);
        int index = -1;
        string onstr = "";
        int colorIndex;
        int sumLen = online;
        while (true)
        {
            index++;
            if (len <= sumLen)
            {
                break;
            }
            if (len - index > 6)
            {
                onstr = str.Substring(index);

                if (onstr.Substring(0, 6) == "<color")
                {
                    colorIndex = onstr.IndexOf(">");
                    index += colorIndex;
                    sumLen += colorIndex + 1;
                }
            }
            if (len - index > 6)
            {
                onstr = str.Substring(index);
                if (onstr.Substring(0, 6) == "</colo")
                {
                    colorIndex = onstr.IndexOf(">");
                    index += colorIndex;
                    sumLen += colorIndex + 1;
                }
            }
            if (index == sumLen - 1)
            {
                if (index + 1 + 8 < len)
                {
                    if (str.Substring(index + 1, 8) == "</color>")
                    {
                        index += 8;
                        sumLen += 8;
                    }
                }
                if (str[sumLen - 1] == '【')
                {
                    sumLen--;
                }
                else if (str[sumLen - 2] == '【')
                {
                    sumLen -= 2;
                }
                str = str.Insert(sumLen, "\n");
                len++;
                sumLen++;
                sumLen += online;
            }
        }
        return str;
    }
    public static void CopyFile(this string a, string b)
    {
        try
        {
            File.Copy(a, b);
        }
        catch (System.Exception e)
        {
            Debug.Log(a + ":" + b);
            //Debug.LogError(CodeStrMgr.GetCodeStr("StringTool", 3) + e);
        }
    }

    public static bool IsHan(this string content)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(content, @"[\uac00-\ud7ff]");
    }
    public static bool IsInt(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }
        return Regex.IsMatch(value, @"^[+-]?\d*$");
    }
    public static bool IsNum(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }
        return Regex.IsMatch(value, @"^[1-9]\d*$");
    }
    public static bool IsFloat(this string value)
    {
        float a = 0;
        return float.TryParse(value, out a);
    }
    //1.不间断空格\u00A0,主要用在office中,让一个单词在结尾处不会换行显示,快捷键ctrl+shift+space ;
    //2.半角空格(英文符号)\u0020,代码中常用的;
    //3.全角空格(中文符号)\u3000,中文文章中使用;
    public static string blank_space_1 = "\u3000";
    public static string blank_space_2 = "\u0020";
    public static string blank_space_3 = "\u00A0";
    //2019-07-03 14:53:58变成不会换行的空格
    public static string ChangeSpace(this string value)
    {
        return value.Replace(blank_space_1, blank_space_3).Replace(blank_space_2, blank_space_3);
    }
    public static string GetUnityAssetPathByLower(this string path)
    {
        return ("Assets/" + AssetPath.CutLowerPath(AssetPath.ModifToCanLoadPath(path), "Assets")).Replace(@"//", "/");
    }
    public static string GetUnityAssetPath(this string path)
    {
        return ("Assets/" + AssetPath.CutPath(AssetPath.ModifToCanLoadPath(path), "Assets")).Replace(@"//", "/");
    }
    public static string GetUnityAssetPathLow(this string path)
    {
        path = path.ToLower();
        return ("assets/" + AssetPath.CutPath(AssetPath.ModifToCanLoadPath(path), "assets")).Replace(@"//", "/");
    }
    public static string GetFullPathByUnityPath(this string path)
    {
        return AssetPath.ModifToCanLoadPath(path).Replace("Assets", Application.dataPath);
    }

    public static List<string> GetNoHaveFilePathsIn(this string pathB, string pathA)
    {
        string[] filesA = pathA.GetAllFileNameNoToLower(".prefab");
        string[] filesB = pathB.GetAllFileNameNoToLower(".prefab");
        string[] cacheA = new string[filesA.Length];
        int index = -1;
        foreach (var item in filesA)
        {
            index++;
            cacheA[index] = item.Replace(pathA, pathB);
        }
        var bNoHaves = cacheA.GetNoHavesIn(filesB);
        return bNoHaves;
    }
    public static string[] GetAllFileNameNoToLower(this string path, string searchPattern)
    {
        List<string> paths = new List<string>();
        Debug.Log(path);
        AssetPath.GetAllFileNameNoToLower(path, (fullPath) =>
        {
            if (fullPath.IndexOf(searchPattern) != -1)
            {
                paths.Add(fullPath.Replace(@"\", "/"));
            }
        });
        return paths.ToArray();
    }
    public static void SaveFile(this string fileName, byte[] bytes)
    {
        Debug.Log(fileName);
        AssetPath.CreateDir(fileName);
        FileStream stream = new FileStream(fileName, FileMode.Create);
        BufferedStream buffStream = new BufferedStream(stream);
        buffStream.Write(bytes, 0, bytes.Length);
        buffStream.Flush();
        stream.Flush();
        buffStream.Close();
        stream.Close();
    }
    public static int ModifyPetId(this int petId, int fashionId = 0)
    {
        if (fashionId == 0)
        {
            return petId;
        }
        else
        {
            return fashionId;
        }
    }
    public static int GetIslandSeaId(this int isLandId)
    {
        return (isLandId - 6001) / 5 + 1010;
    }
    public static string ChangeTextStr(this string str)
    {
        if (str == null)
            return "";

        //if (TableMgr.Instance.gameMscs != null)
        //{
        //    if (TableMgr.Instance.gameMscs.ContainsKey(str))
        //    {
        //        return TableMgr.Instance.gameMscs[str];
        //    }
        //}
        return str;
    } 
    public static string ChangeConfigStr(this string str)
    {
        if (str == null)
            return "";
        return str;
    }
    public static string ChangePrefabStr(this string str)
    {
        if (str == null)
            return "";
        //if (TableMgr.Instance.prefabMscs != null)
        //{
        //    if (TableMgr.Instance.prefabMscs.ContainsKey(str))
        //    {
        //        return TableMgr.Instance.prefabMscs[str];
        //    }
        //}
        return str;
    }
    public static string ChangeTextStr1(this string str, object arg)
    {
        //str = str.ChangeTextStr();
        return string.Format(str, arg);
    }
    public static string ChangeTextStr2(this string str, object arg0, object arg1)
    {
        //str = str.ChangeTextStr();
        return string.Format(str, arg0, arg1);
    }
    public static string ChangeTextStr3(this string str, object arg0, object arg1, object arg2)
    {
        //str = str.ChangeTextStr();
        return string.Format(str, arg0, arg1, arg2);
    }
    public static float ModifyRota(this float rota)
    {
        if (rota > 300)
        {
            return rota - 360;
        }
        return rota;
    }
    public static Vector2 ModifyRotaVec(this Vector2 rota)
    {
        if (rota.y > 300)
        {
            return new Vector2(rota.x, rota.y - 360);
        }
        return rota;
    }
    public static string ModifyToPercent(this float num)
    {
        return (num * 100).ToString().Split('.')[0] + "%";
    }
    public static string ModifyToPercentKeepNum(this float num, int few)
    {
        if (num > 1)
        {
            num = 1;
        }
        return Math.Round(num * 100, few) + "%";
    }

    public static string IntToOneChar(this int num)
    {
        string ans = "";
        if (num < 10)
            ans += num;
        else if (num < 36)
            ans += (char)('a' + num - 10);
        else if (num < 62)
            ans += (char)('A' + num - 36);
        return ans;
    }
    public static int OneCharToInt(this string num)
    {
        int ans = 0;
        char c = num[0];
        if ('0' <= c && c <= '9')
            ans = c - '0';
        else if ('a' <= c && c <= 'z')
            ans = c - 'a';
        else if ('A' <= c && c <= 'Z')
            ans = c - 'A';
        return ans;
    }
    public static string ModifyTablePath(this string str)
    {
        return str.Replace("table", "table");
    }
    public static string ModifySkillPath(this string str)
    {
        return str.Replace("Skill", "Skill");
    }
    public static int GetRealSectionId(this int sectionId)
    {
        return (sectionId - 1) % 7 + 1;
    }
    public static string FirstToUpper(this string str) {
        if (string.IsNullOrEmpty(str)) {
            return str;
        }
        if (str.Length == 1) {
            return str.ToUpper();
        }
        return str[0].ToString().ToUpper() + str.Substring(1);
    }
    public static string ToRankStr(this int num) {
        if (num == 0 || num > 100) {
            return "100+";
        }
        return num.ToString();
    }
    
    public static List<int> StringToIntList(this string input)
    {
        // 使用逗号分割字符串
        string[] stringArray = input.Split(',');

        // 创建 List<int> 并将字符串数组转换为整数列表
        List<int> intList = new List<int>();
        foreach (string str in stringArray)
        {
            if (int.TryParse(str, out int num))
            {
                intList.Add(num);
            }
            else
            {
                Debug.Log($"Invalid integer: {str}");
            }
        }

        return intList;
    }
}