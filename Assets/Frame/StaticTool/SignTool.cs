using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SignTool
{
    public static string ModifyToEngishFlag(this string str) {
        return str.Replace("£º", ":").Replace("£¬", ",").Replace("£¨", "(").Replace("£©", ")");
    }
}
