//﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;
//using System.Linq;
//using System;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Text.RegularExpressions;
//using LitJson;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

//public static class JsonTool {
//    public static void RenameKey(this JsonData json, string oldkey, string newKey) {
//        if(newKey == "") {
//            return;
//        }
//        if(json.Keys.Count == 0) {
//            return;
//        }
//        //Debug.Log(json.Keys);
//        if(!json.Keys.Contains(oldkey))
//        {
//            Debug.Log("json no have this key:" + oldkey);
//            return;
//        }
//        var cache = json[oldkey];
//        json[newKey] = cache;
//        json.RemoveKey(oldkey);
//    }
//    public static void RemoveKey(this JsonData json, string key) {
//        if(!json.Keys.Contains(key)) {
//            Debug.Log("json no have this key:" + key);
//            return;
//        }
//        JsonData newData = new JsonData();
//        foreach(var item in json.Keys)
//        {
//            if(item != key)
//                newData[item] = json[item];
//        }
//        json.Clear();
//        foreach(var item in newData.Keys)
//        {
//            json[item] = newData[item];
//        }
//    }
//}
