using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text;
using System.Reflection;
using UnityEngine.UI;
using ExtendForCreateScript;
public class ChineseData
{
    public string inScript; // Content 要多语言的中文
    public string modify;
    public string time; //用来排序 天为单位
    public string traditionalType;
    public List<string> linkTo;

    public ChineseData()
    {
        linkTo = new List<string>();
    }
}
public class OutScriptMsg
{
    private Dictionary<string, string> scriptNames;
    private class ChineseData
    {
        public string inScript; // Content 要多语言的中文
        public string modify;
        public string time; //用来排序 天为单位
        public string FChinese;//繁体
        public string English;//英文

        public List<string> uiNames;
    }

    [MenuItem("程序工具/多语言工具/提取代码中中文")]
    public static void GetChineseMsg()
    {
        var lst = new List<string>();
        AssetPath.GetAllFileName(Application.dataPath + "/Script", (path) =>
        {
            lst.Add(path);
        });
        AssetPath.GetAllFileName(Application.dataPath + "/ScriptHotFix", (path) =>
        {
            lst.Add(path);
        });
        TableEditor.LoadOneTab("gamemsg_游戏中提示.xlsx");
        //var tab = TableMgr.Instance.ReadTable<Table.gamemsg>(TableName.gamemsg);
        //Dictionary<string, ChineseData> datas = new Dictionary<string, ChineseData>();
        //foreach (var item in tab)
        //{
        //    var one = new ChineseData();
        //    one.inScript = item.Value.inscript;
        //    one.uiNames = item.Value.linkuiname.SplitToString('†');
        //    one.time = item.Value.time;
        //    one.modify = item.Value.modify;
        //    one.FChinese = item.Value.FChinese;
            
        //    //GameLog.Log($"({one.inScript}) + ({one.uiNames}) + ({one.time}) + ({one.modify})");
        //}


        //var csNames = GetArtChineseNamme();
        //foreach (var path in lst)
        //{
        //    string csname = path.GetFileNameByPatHhaveEnd();
        //    if (csname == "OutScriptMsg.cs") {
        //        continue;
        //    }
        //    if (csname == "CreateScript.cs")
        //    {
        //        continue;
        //    }
        //    if (csname == "ExcelModuleContext.cs")
        //    {
        //        continue;
        //    }
        //    if (csname.Contains("Auto.cs"))
        //    {
        //        continue;
        //    }
        //    csname = csname.Split('.')[0];
        //    if (csNames.ContainsKey(csname)) {
        //        csname = csNames[csname];
        //    }
        //    path.ReadStringByLine((line) => {
                
        //        var str = CheckLine(line);
        //        if (str == "") 
        //        {
        //            return;
        //        }
        //        if (!datas.ContainsKey(str)) {
        //            var data = new ChineseData();
        //            data.inScript = str;
        //            data.uiNames = new List<string>();
        //            datas.Add(str, data);
        //        }
        //        var cdata = datas[str];
        //        if (!cdata.uiNames.Contains(csname)) {
        //            cdata.uiNames.Add(csname);
        //            cdata.time = System.DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "");
        //        }
        //    });
        //}
        //ExcelTool.Write(Config.GameMsgPath, "gamemsg", (workSheet) =>
        //{
        //    workSheet.DeleteRow(11, tab.Count);
        //    workSheet.InsertRow(11, tab.Count);
        //    int index = 10;
        //    foreach (var item in datas)
        //    {
        //        index++;
        //        workSheet.Cells[index, 1].Value = index - 10;
        //        workSheet.Cells[index, 2].Value = item.Value.inScript;
        //        workSheet.Cells[index, 3].Value = item.Value.time;
        //        workSheet.Cells[index, 4].Value = item.Value.modify;
        //        workSheet.Cells[index, 5].Value = item.Value.uiNames.LstToStrBySpilt("†");
        //    }
        //});
    }
   
    private static Dictionary<string, string> GetArtChineseNamme() {
        return null;
        //var path = Config.ArtPath + "Image/UIImage_私有/";
        //Dictionary<string, string> datas = new Dictionary<string, string>();
        //path.GetDirOnlyThis((dir) => {
        //    var key = dir.Name.Split('_')[0];
        //    if (datas.ContainsKey(key))
        //    {
        //        Debug.LogError("美术界面名字重复:" + key);
        //    }
        //    else {
        //        datas.Add(key, dir.Name);
        //    }
        //});
        //return datas;
    }
    //private static List<string> SplitLine(string line) {
    //    if (line.Contains("?")) {
        
    //    }
    //}
    private static string CheckLine(string line)
    {
        if (line.Contains("Msg.Instance.Show") || line.Contains(".ChangeTextStr"))
        {
            var strs = line.Split('"');
            if (strs.Length <= 2)
            {
                return "";
            }
            return strs[1];
        }
        return "";
    }
    public static List<GameObject> GetAllUIPrefab() {
        var lst = new List<GameObject>();
        GetFileNamesAndShort("", Application.dataPath + "/Res/Prefab/UIPrefab/", (fullpath, dirpath) =>
        {
            if (fullpath.EndsWith(".prefab"))
            {
                var target = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Res/Prefab/UIPrefab/" + dirpath);
                lst.Add(target);
            }
        });
        return lst;
    }
    public static void LoopOneTran(Transform tran) {
        for (int i = 0; i < tran.childCount; i++)
        {
            Transform target = tran.GetChild(i);
            SetText(target);
            SetImage(target);
        }
    }
    public static void SetText(Transform myTran)
    {
        if (myTran.GetComponent<Text>())
        {
            
        }
    }
    static void SetImage(Transform myTran)
    {
        var image = myTran.GetComponent<Image>();
        if (image != null && image.sprite != null)
        {
          
        }
    }
    public static void GetFileNamesAndShort(string begin, string targetPath, System.Action<string, string> finish)
    {
        //获取路径下的所有文件
        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(targetPath);
        //获取所有子路径
        foreach (System.IO.DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.FullName.IndexOf(".svn") != -1)
                continue;
            if (info.FullName.IndexOf(".meta") != -1)
                continue;
            GetFileNamesAndShort(begin + info.Name + "/", info.FullName, finish);
        }
        //获取所有子文件
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (finish != null)
            {
                if (info.FullName.IndexOf(".meta") != -1)
                    continue;
                finish(info.FullName, begin + info.Name);
            }
        }
    }


}
