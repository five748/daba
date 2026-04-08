using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtendForCreateScript
{
    public static class ExtendCreateScript
    {
        public static string pre(this string origin,string add)
        {
            return add + origin;
        }
        public static string add(this string origin,string add)
        {
            return origin + add;
        }
        public static string tab(this string origin)
        {
            return origin + "\t";
        }
        public static string line(this string origin)
        {
            return origin + "\n";
        }
        public static string preTab(this string origin)
        {
            return "\t" + origin;
        }
        public static string preLine(this string origin)
        {
            return "\n" + origin;
        }
        public static string brace(this string origin)
        {
            return "{\n" + origin + "\n}";
        }
        public static string format(this string origin,params object[] args)
        {
            return string.Format(origin, args);
        }
        public static string anno(this string origin, string annotate)
        {
            return origin.pre(annotate.pre("//").line());
        }


        //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
        static int chfrom = Convert.ToInt32("4e00", 16);
        static int chend = Convert.ToInt32("9fff", 16);
        public static bool ContainChinese(this string origin)
        {
            if (origin.IndexOfChinese() < 0)
            {
                return false;
            }
            return true;
        }
        public static int IndexOfChinese(this string origin)
        {
            if (origin == "")
            {
                return -1;
            }
            int code = 0;
            for (int i = 0; i < origin.Length; i++)
            {
                code = Char.ConvertToUtf32(origin, i);
                if (code >= chfrom && code <= chend)
                {
                    return i;

                }
            }
            return -1;
        }

    }
}
