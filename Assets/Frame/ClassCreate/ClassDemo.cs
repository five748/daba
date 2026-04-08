using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ClassDemo
{
    public static string GoStr
    {
        get
        {
            return @"package iproto

type ClientData struct {
	Code string
	Key  string
	Data interface{}
}



const (
keyvalue
)
var (
	IProtoMap = map[string]string{
valuekey
	}
)";
        }
    }
    public static string GetSetFun(string fieldName, string typeName, string EventType, string keyStr, string defauleValue, string msc, bool parentIsLst = false)
    {
        string str = @"private typeName _fieldName = default;
        public typeName fieldName{//msc
            get{
                return _fieldName;
            }
            set {
                if(_fieldName == value)
                    return;
                _fieldName = value;
            }
	    }";

        if (parentIsLst)
        {
            str = str.Replace("EventTypeEvents", "ScrEventTypeEvents").Replace("Invoke(value)", "Invoke(value, _Key)").Replace("Invoke(value.ToString())", "Invoke(value.ToString(), _Key)");
        }
        return str.Replace("fieldName", fieldName).Replace("EventType", EventType).Replace("typeName", typeName).Replace("keyStr", keyStr).Replace("default", defauleValue).Replace("msc", msc);
    }
    public static string GetSetFunStr(string fieldName, string typeName, string EventType, string keyStr, string defauleValue, string msc, bool parentIsLst = false)
    {
        string str = @"private typeName _fieldName = default;
        public typeName fieldName{//msc
            get{
                return _fieldName;
            }
            set {
                if(_fieldName == value)
                    return;
                _fieldName = value;
            }
	    }";
        if (parentIsLst)
        {
            str = str.Replace("EventTypeEvents", "ScrEventTypeEvents").Replace("Invoke(value)", "Invoke(value, _Key)").Replace("Invoke(value.ToString())", "Invoke(value.ToString(), _Key)").Replace("StringEvents", "ScrStringEvents");
        }
        return str.Replace("fieldName", fieldName).Replace("EventType", EventType).Replace("typeName", typeName).Replace("keyStr", keyStr).Replace("default", defauleValue).Replace("msc", msc);
    }
    public static string GetSwitchFun(string inSwitch, Dictionary<string, string> caseDatas, bool isHaveReturn = false, string defaultStr = "null")
    {
        string swithchStr = "";
        string oneStr = "";
        if (isHaveReturn)
        {
            swithchStr =
               @"        switch(inSwitch) {
        bodyStr
        }
        return null;".Replace("null", defaultStr);
            oneStr =
                  @"    case keyStr:
                return valueKey;";
        }
        else
        {
            swithchStr =
                @"        switch(inSwitch) {
            bodyStr
        }";
            oneStr =
                 @"case keyStr:
            valueKey;
            break;";
        }
        string bodyStr = "";
        foreach (var item in caseDatas)
        {
            bodyStr += oneStr.Replace("keyStr", item.Key).Replace("valueKey", item.Value) + "\n        ";
        }
        bodyStr = bodyStr.CutLast("\n        ".Length);
        return swithchStr.Replace("inSwitch", inSwitch).Replace("bodyStr", bodyStr);
    }

    public static string GetProtoFun(string funName, Dictionary<string, ClassToolData> inDatas, Dictionary<string, ClassToolData> outDatas)
    {
        string str =
  @"public static void funName(iiiSystem.Action<ooo> callback) {
        var req = new {
            xxx
        };
        ServerMonoSocket.Instance.CallServer(serverName, JsonMapper.ToJson(req),(isSucc, jsonStr) =>
        {
            callback(coo);
        });
    }";
        str = str.Replace("funName", funName.Replace("/", "_")).Replace("serverName", "\"" + funName + "\"");
        string iii = "";
        foreach (var item in inDatas)
        {
            iii += item.Value.newTypeName + " _" + item.Value.Name + ", ";
        }
        str = str.Replace("iii", iii);

        string xxx = "protoName = \"" + funName + "\",\n            ";
        foreach (var item in inDatas)
        {
            xxx += item.Value.Name + " = _" + item.Value.Name + ",\n            ";
        }
        str = str.Replace("xxx", xxx.Substring(0, xxx.Length - 13));

        string ooo = "bool, ";
        foreach (var item in outDatas)
        {
            if (item.Value.ClassTypeName == "MapTree")
            {
                ooo += "Dictionary<yyy,xxxone>".Replace("yyy", item.Value.fieldTyppeName).Replace("xxx", item.Value.Name) + ", ";
            }
            else if (item.Value.ClassTypeName == "LstTree")
            {
                ooo += "List<xxxone>".Replace("xxx", item.Value.Name) + ", ";
            }
            else if (item.Value.isNorType) {
                ooo += item.Value.newTypeName + ", ";
            }
            else
            {
                ooo += item.Value.Name + ", ";
            }
          
        }
        if (string.IsNullOrEmpty(ooo))
        {
            str = str.Replace("<ooo>", "");
        }
        else {
            str = str.Replace("ooo", ooo.Substring(0, ooo.Length - 2));
        }
      

        string coo = "isSucc, ";
        foreach (var item in outDatas)
        {
            coo += "AllValueData.Instance." + item.Value.valueStr;
            coo += ", ";
        }
        if (string.IsNullOrEmpty(coo))
        {
            str = str.Replace("coo", "");
        }
        else {
            str = str.Replace("coo", coo.Substring(0, coo.Length - 2));
        }
    
        return str;
    }
    public static string CreateProteCenterSer(string des, string funName, Dictionary<string, ClassToolData> inDatas, Dictionary<string, ClassToolData> outDatas) {
        string str = @"message funName ( iii ){ des
    ooo
}";
        str = str.Replace("des", des);
        str = str.Replace("funName", funName.Replace("/", "_")).Replace("serverName", "\"" + funName + "\"");
        string iii = "";
        foreach (var item in inDatas)
        {
            iii += item.Value.newTypeName  + " " + item.Value.Name + ", ";
        }
        str = str.Replace("iii", iii.CutLast(2));

        string ooo = "";
        foreach (var item in outDatas)
        {
            ooo += item.Value.Name + " = " + item.Key + ",\n    ";
        }
        str = str.Replace("ooo", ooo.CutLast(6));
        //Debug.LogError(str);
        return str;
    }
    public static string ProtoScriptStr
    {
        get
        {
            return @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using TreeData;
public class ProtoCenter{
    xxx
}";
        }
    }
}
