using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.Runtime.InteropServices;

public static class RegexTool
{
    private static List<string> spcstr = new List<string>() {
        "‡","㉿","☋","☍","☌","◪","◔","@","↕","↔","⊱","⋛","⋌","⋚","⊰","¬","￢","▔","†","◮","◁","☝","☞","◑","♂","☀","¤"
    };
    public static string ChangeNumColor(this string str)
    {
        return str;
        //if (str.Contains("<color") || string.IsNullOrEmpty(str))
        //{
        //    return str;
        //}
        //List<string> redstrs = new List<string>();
        //List<string> yellowstrs = new List<string>();
        //Regex re = new Regex(@"[0-9]+%");
        //Regex re1 = new Regex(@"[0-9]+");
        //Regex re2 = new Regex(@"[0-9]+[.][0-9]+");
        //Regex re3 = new Regex(@"[[].*?[]]");
        //MatchCollection mc = re.Matches(str);
        //MatchCollection mc1 = re1.Matches(str);
        //MatchCollection mc2 = re2.Matches(str);
        //MatchCollection mc3 = re3.Matches(str);
        //List<string> strSpc = new List<string>();

        //foreach (Match item in mc)
        //{
        //    redstrs.Add(item.Value);
        //    strSpc.Add(item.Value.CutLast());
        //}
        //foreach (Match item in mc2)
        //{
        //    redstrs.Add(item.Value);
        //}
        //foreach (Match item in mc3)
        //{
        //    yellowstrs.Add(item.Value);
        //}
        //foreach (Match item in mc1)
        //{
        //    if (!strSpc.Contains(item.Value))
        //    {
        //        redstrs.Add(item.Value);
        //    }
        //}
        //redstrs.Sort((b, a) =>
        //{
        //    return a.Length.CompareTo(b.Length);
        //});
        //int index = -1;
        //foreach (var item in redstrs)
        //{
        //    index++;
        //    try
        //    {
        //        str = str.Replace(item, spcstr[index]);
        //    }
        //    catch
        //    {
        //        Debug.LogError(index);
        //        Debug.LogError(str);
        //        Debug.LogError(str.Length);
        //    }
        //}
        //foreach (var item in yellowstrs)
        //{
        //    index++;
        //    try
        //    {
        //        str = str.Replace(item, spcstr[index]);
        //    }
        //    catch
        //    {
        //        Debug.LogError(index);
        //        Debug.LogError(str);
        //        Debug.LogError(str.Length);
        //    }
        //}
        //index = -1;
        //foreach (var item in redstrs)
        //{
        //    index++;
        //    str = str.Replace(spcstr[index], item.ChangeRed());//ChangeColor("FF8C00")
        //}
        //foreach (var item in yellowstrs)
        //{
        //    index++;
        //    str = str.Replace(spcstr[index], item.ChangeOrange());//ChangeColor("FF8C00")
        //}
        //return str;
    }

    private static Regex regutf8 = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
    public static string ToUTF8(this string str)
    {
        return regutf8.Replace(str, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
    }
}
