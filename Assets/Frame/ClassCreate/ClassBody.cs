﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.IO;
using System.Collections.Generic;
using System.Text;
public class ClassBody  {
 
    public string topTag;
    public string use;
    public string name;
    public string div;  //继承
    public List<string> attributes = new List<string>();
    public List<string> funcs = new List<string>();
    public bool isSerializable = false;
    public void CreateClass(string path, string addtoend = "", string namespace1 = "") {
        CreateBase(0, path, addtoend, namespace1);
    }
    public void CreateEnmu(string path, string addtoend = "", string namespace1 = "") {
        CreateBase(1, path, addtoend, namespace1);
    }
    public void CreateBase(int type, string path, string addtoend = "", string namespace1 = "") {
        string myString = "";
        if(!string.IsNullOrEmpty(topTag))
            myString += topTag + TurnLine();
        myString += use;
        if(type == 0)
        {
            myString += GetClassInfo(name, GetDeriverName(div));
        }
        if(type == 1)
        {
            myString += CreateEnmu(name);
        }
        string body = GetAttributeString();
        body += GetFunString();
        body += addtoend;
        myString += AddBody(new List<string>(){ body});
        WriteToFile(path, myString, namespace1, isSerializable);
    }
    private string GetClassInfo(string className, string derName = "")
    {
        return "public class " + className + derName;
    }
    public string CreateEnmu(string className) {
        return "public enum " + className;
    }
    private string GetDeriverName(string div)
    {  //获取继承
        if (string.IsNullOrEmpty(div))
            return "";
        return ":" + div;
    }
    private string GetAttributeString() {
        string myString = "";
        foreach (var str in attributes) {
            myString += AddTab() + str + TurnLine();
        }
        return myString;
    }
    private string GetFunString()
    {
        string myString = "";
        foreach (var str in funcs)
        {
            myString += AddTab() + str + TurnLine();
        }
        return RemoveEmptyLine(myString);
    }
    public string  GetFunction(string func, string body) {
        return func + AddBody(body, AddTab());
    }
    public string GetFunction(string func, List<string> bodys) {
        return func + AddBody(bodys, AddTab());
    }
    public string GetArributeFunction(string func, string getStr = "", string setStr = "") {
        string str = func;
        if(getStr != "") {
            getStr = "    get" + getStr + TurnLineAndAddTab();
        }
        if(setStr != "") {
            setStr = "      set" + setStr + TurnLineAndAddTab();
        }
        str += "{" + TurnLineAndAddTab() + getStr + setStr + "}";
        return str;
    }
   
    //============================================ 符号 =============================================
    private string TurnLine()
    {
        return "\n";
    }
    public string AddTab()
    {
        return "    ";
    }
    private string TurnLineAndAddTab()
    {
        return TurnLine() + AddTab();
    }
    private string AddBody(string body, string tab0 = "", string tab1 = "", string tab2 = "", string tab3 = "")
    {
        string info = tab0 + tab1 + tab2 + tab3;
        return "{" + TurnLine() + body + TurnLine() + info + "}";
    }
    public string AddBody(List<string> bodys, string tab0 = "", string tab1 = "", string tab2 = "", string tab3 = "") {
        string info = tab0 + tab1 + tab2 + tab3;
        string str = "";
        foreach(var body in bodys)
        {
            str += info + body + TurnLine() + info;
        }
        return "{" + TurnLine() + info + str + "}";
    }
    public string AddArributeBody(List<string> bodys, string tab0 = "", string tab1 = "", string tab2 = "", string tab3 = "") {
        string info = tab0 + tab1 + tab2 + tab3;
        string str = "";
        foreach(var body in bodys)
        {
            str += info + body + TurnLine() + info;
        }
        return "{" + TurnLine() + info + str + "  }";
    }
    public string GetStr(string str) {
        return "get{xxx}".Replace("xxx", str);
    }
    static string RemoveEmptyLine(string myString)
    {
        if (string.IsNullOrEmpty(myString))
            return "";
        return myString.Substring(0, myString.Length - 1);
    }
    //把类写入文件中
    private void WriteToFile(string path, string str, string namespce1 = "", bool _isSerializable = false) {
        //Debug.Log(path);
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        StreamWriter stream = new StreamWriter(path + name + ".cs", false, Encoding.UTF8);
        UTF8Encoding utf8 = new UTF8Encoding(); // Create a UTF-8 encoding.
        byte[] bytes = utf8.GetBytes(str.ToCharArray());
        string EnUserid = utf8.GetString(bytes);
        stream.WriteLine(EnUserid);
        stream.Flush();
        stream.Close();
        if (namespce1 != "") {
            (path + name + ".cs").AddNameSpace(namespce1);
        }
        if (_isSerializable)
        {
            (path + name + ".cs").AddSerializable(namespce1 != "");
        }
    }
}











































