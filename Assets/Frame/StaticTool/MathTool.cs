using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

public static class MathTool
{
    public static int RoundToInt(this float f, float middlef)
    {
        string[] strs = f.ToString().Split('.');
        if (strs.Length == 1)
        {
            return int.Parse(strs[0]);
        }
        if (float.Parse("0." + strs[1]) > middlef)
        {
            return int.Parse(strs[0]) + 1;
        }
        else
        {
            return int.Parse(strs[0]);
        }
    }
    public static int Fact(this int n)
    {
        if (n <= 1) //判断参数是否小于等于一
        {
            return 1;
        }
        else
        {
            return n * (n - 1).Fact();//自己调用自己并递减！
        }
    }
    public static int SumAdd(this int n)
    {
        int index = 0;
        n = n + 1;
        for (int i = 0; i < n; i++)
        {
            index += i;
        }
        return index;
    }

    public static float Scaling(this float n)
    {
        return 1f - (1624 - (750.0f / Screen.height) * Screen.width) * ((1 - n) / 290);
    }
    public static float GetScreenScale(int modify)
    {
        return (Screen.width * 750.0f / Screen.height - 1334f) / 290.0f * modify;
    }
    public static List<int> GetRandomArray(int Number, int minNum, int maxNum)
    {
        int j;
        List<int> b = new List<int>();
        System.Random r = new System.Random();
        for (j = 0; j < Number; j++)
        {
            int i = r.Next(minNum, maxNum + 1);
            if (b.Contains(i))  //是否包含这个数
            {
                j = j - 1;
            }
            else
            {
                b.Add(i);
            }
        }
        return b;

    }
    public static List<string> GetCutDesc(this int cur, int max, int min, System.Func<int, string> callback = null)
    {
        int num;
        if ((max - min) % cur == 0)
        {
            num = (max - min) / cur;
        }
        else
        {
            num = ((max - min) / cur) + 1;
        }
        List<string> reDes = new List<string>();
        var last = min;
        for (int i = 0; i < num; i++)
        {
            var index = i + 1;
            var maxnum = last + cur - 1;
            if (maxnum > max)
            {
                //break;
                maxnum = max;
            }
            string ondesc = last + "～" + maxnum;
            last = maxnum + 1;
            if (callback != null)
            {
                ondesc = string.Format(callback(index), ondesc);
            }
            reDes.Add(ondesc);
        }
        return reDes;
    }

    public static float angle_360(this Vector3 from_, Vector3 to_)
    {
        //两点的x、y值  
        float x = from_.x - to_.x;
        float y = from_.y - to_.y;

        //斜边长度  
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f));

        //求出弧度  
        float cos = x / hypotenuse;
        float radian = Mathf.Acos(cos);

        //用弧度算出角度  
        float angle = 180 / (Mathf.PI / radian);


        if (y < 0)
        {
            angle = -angle;
        }
        else if ((y == 0) && (x < 0))
        {
            angle = 180;
        }
        return angle;
    }
    public static int dgGetEFF(this List<int> students, int left, int right, int key)
    {
        if (left > right)
        {
            return -1;
        }
        else
        {
            int avg = (left + right) / 2;
            if (key > students[avg])
            {
                return dgGetEFF(students, left + 1, right, key);
            }
            else if (key < students[avg])
            {
                return dgGetEFF(students, left, right - 1, key);
            }
            else
            {
                return avg;
            }
        }
    }

    public static long binarySearch(this List<long> arr, long value)
    {
        if (arr.Count == 1)
        {
            return arr[0];
        }
        if (value <= arr[0])
        {
            return arr[0];
        }
        else if (value >= arr[arr.Count - 1])
        {
            int index = arr.Count - 1;
            long distant = arr[index] - value;
            while (distant == arr[arr.Count - 1] - value || index == 0)
            {
                index--;
                distant = arr[index] - value;
            }
            index++;
            return arr[index];
        }
        int start = 0;
        int end = arr.Count - 1;
        while (true)
        {
            int mid = (start + end) / 2;
            long midValue = arr[mid];
            long distance = midValue - value;
            if (distance == 0)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i] - value == 0)
                    {
                        return arr[i];
                    }
                }
            }
            else
            {
                if (distance > 0)
                {
                    end = mid - 1;
                }
                else
                {
                    start = mid + 1;
                }
            }
            if (start > end)
            {
                long startDistance = Math.Abs(arr[start] - value);
                long endDistance = Math.Abs(arr[end] - value);
                if (startDistance == endDistance)
                {
                    return arr[end];
                }
                else
                {
                    return startDistance < endDistance ? arr[start] : arr[end];
                }
            }
        }
    }
}





















































