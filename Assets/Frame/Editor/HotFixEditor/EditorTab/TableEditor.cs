using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Data;
using ExcelDataReader;
using System.Linq;
using System;
using System.Text;
using System.Web;
using UnityEngine.Networking.PlayerConnection;
using Table;

public class TableEditor
{
    public static List<string> TypeTemStrs = new List<string> { "string", "int", "byte", "bool", "long", "float" };
    private static List<string> tableNames;
    private static List<string> frameTabNames;
    [MenuItem("策划工具/导入配置表")]
    public static void ImportAllTabelToUnity()
    {
        ImportTabelToUnityBase(false, (path) =>
        {
            return false;
        }, true);
    }
    public delegate bool IsNoImportTable(string path);
    public static void ImportTabelToUnityBase(bool isQuick, IsNoImportTable NoImport, bool importArt = false)
    {
        tableNames = new List<string>();
        frameTabNames = new List<string>()
        {
            "basecolor","bgm", "channel", "color_style", "frame", "sound", "sub_channel"
        };
        ArtData = new Dictionary<string, string>();
        ArtTableData = new Dictionary<string, string>();
        EnmuData = new Dictionary<string, Dictionary<int, TableMenuData>>();
        TableFieldAndTypes = new List<TableFieldAndType>();
        TableLineDatas = new List<TableLineData>();

        //var framePath = Application.dataPath + "/BaseFrame/ConfigCache/Table/";
        //framePath.GetAllFileName(null, (file) =>
        //{
        //    Debug.Log(file.Name);
        //    if (file.Name.Contains("模板表"))
        //    {
        //        // Debug.LogError(file.FullName);
        //        return;
        //    }
        //    if (file.Name.Contains("~$"))
        //    {
        //        // Debug.LogError(file.FullName);
        //        return;
        //    }
        //    if (NoImport(file.FullName))
        //    {
        //        Debug.LogError(file.FullName);
        //        return;
        //    }
        //    //if(file.Name.Contains("err_错误码"))
        //    {
        //        LoadData(file.FullName, GameSign.RowFlag.Trim(), GameSign.ColumnsFlag, true, true);
        //    }
        //});
        LoadData(FrameConfig.DataTablePath, GameSign.RowFlag.Trim(), GameSign.ColumnsFlag, false, false);
        string newPath = FrameConfig.PlanTablePath + "new";
        if (Directory.Exists(FrameConfig.PlanTablePath))
        {
            if (Directory.Exists(newPath))
            {
                FileUtil.DeleteFileOrDirectory(newPath);
            }
            FileUtil.CopyFileOrDirectory(FrameConfig.PlanTablePath, newPath);
            newPath.GetAllFileName(null, (file) =>
            {
                if (file.Name.Contains("模板表"))
                {
                    //Debug.LogError(file.FullName);
                    return;
                }
                if (file.Name.Contains("~$"))
                {
                    Debug.LogError(file.FullName);
                    return;
                }
                if (NoImport(file.FullName))
                {
                    Debug.LogError(file.FullName);
                    return;
                }
                //if(file.Name.Contains("err_错误码"))
                {
                    LoadData(file.FullName, GameSign.RowFlag.Trim(), GameSign.ColumnsFlag, false, false);
                }
            });
        }
        CreateMenu();
        CreateScripts();
        CreateEnmuScript(tableNames);
        CreateTableCacheScript(tableNames);
        CreateTableTexts();
        FileUtil.DeleteFileOrDirectory(newPath);
        AssetDatabase.Refresh();
        SDKConfigSet.SetGame();
    }
    public static void LoadOneTab(string tabName)
    {
        tableNames = new List<string>();
        ArtData = new Dictionary<string, string>();
        EnmuData = new Dictionary<string, Dictionary<int, TableMenuData>>();
        TableFieldAndTypes = new List<TableFieldAndType>();
        TableLineDatas = new List<TableLineData>();
        string newPath = FrameConfig.PlanTablePath + "new";
        CopyFolder(FrameConfig.PlanTablePath, newPath);
        LoadData(newPath + "/" + tabName, GameSign.RowFlag.Trim(), GameSign.ColumnsFlag, false, false);
        CreateTableTexts();
        FileUtil.DeleteFileOrDirectory(newPath);
        AssetDatabase.Refresh();
    }
    public static void CopyFolder(string sourceFolder, string destFolder)
    {
        try
        {
            //如果目标路径不存在,则创建目标路径
            if (!System.IO.Directory.Exists(destFolder))
            {
                System.IO.Directory.CreateDirectory(destFolder);
            }
            //得到原文件根目录下的所有文件
            string[] files = System.IO.Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(destFolder, name);
                System.IO.File.Copy(file, dest, true);//复制文件
            }
            //得到原文件根目录下的所有文件夹
            string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);//构建目标路径,递归复制文件
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private static void CreateColorTool()
    {
        string str = @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ColorTool{
    public static string ChangeColor(this string str, int index) {
        return ""<color=#x>str</color>"".Replace(""x"", TableCache.Instance.basecolorTable[index].name).Replace(""str"", str);
    }
function
}";
        string strfun = "";
        //foreach (var item in TableCache.Instance.basecolorTable)
        //{
        //    strfun += "     public static string ChangeXXX(this string str){return str.ChangeColor(Index);}\n".Replace("XXX", item.Value.EnumKey).Replace("Index", item.Key.ToString());
        //}
        strfun = strfun.CutLast();
        string path = Application.dataPath + "/Script/Frame/Main/Auto/Other/ColorTool.cs";
        path.WriteByUTF8(str.Replace("function", strfun));
    }
    private static void CreateScripts()
    {
        int index = -1;
        foreach (var item in TableFieldAndTypes)
        {
            index++;
            Dictionary<string, int> addDatas = new Dictionary<string, int>();
            int fieldIndex = -1;
            foreach (var fieldName in item.fieldNames)
            {
                fieldIndex++;
                if (item.tabName == "explore_scene_npc_type")
                {
                    continue;
                }
                if (EnmuData.ContainsKey(fieldName))
                {
                    addDatas.Add(fieldName + "Name", fieldIndex);
                }
            }
            foreach (var data in addDatas)
            {
                item.fieldNames.Add(data.Key);
                item.fieldTypes.Add("string");
                for (int i = 0; i < TableLineDatas[index].linelst.Count; i++)
                {
                    var keyvalue = TableLineDatas[index].linelst[i].Split(GameSign.RowFlagChar)[data.Value];
                    if (item.tabName == "color")
                    {
                        TableLineDatas[index].linelst[i] += GameSign.RowFlag + EnmuData[data.Key.CutLast(4)][int.Parse(keyvalue)].value.Substring(2);
                    }
                    else
                    {
                        try
                        {
                            TableLineDatas[index].linelst[i] += GameSign.RowFlag + EnmuData[data.Key.CutLast(4)][int.Parse(keyvalue)].name;
                        }
                        catch
                        {
                            TableLineDatas[index].linelst[i] += GameSign.RowFlag + "";
                        }
                    }
                }
            }
            CreateScript(item.tabFatherName,item.tabName, item.fieldNames, item.fieldTypes);
        }
    }
    private static void CreateTableTexts(bool needBattleNpc = true)
    {
        var newBattleNpcs = new Dictionary<string, string>();
        foreach (var item in TableLineDatas)
        {
            CreateTableText(item.linelst, item.tabName, newBattleNpcs);
        }
    }
    private static void CreateChangeImageDir()
    {
        string basePath = FrameConfig.ArtPath + "Image/ChangeImage_变化/";
        string demoImagePath = FrameConfig.ArtPath + "other/demo/demo.png";
        foreach (var data in ArtData)
        {
            string path = basePath + data.Key;
            if (!Directory.Exists(path))
            {
                Debug.LogError(path);
                Directory.CreateDirectory(path);
                string[] strs = data.Value.Substring(1).Split('|');
                for (int i = 0; i < strs.Length; i++)
                {
                    if (data.Key.Contains("skilleffect_技能表"))
                    {
                        //1000 2000 3400 4000
                        var str = strs[i].Replace(".png", "").Split('#')[1].CutLast(2);
                        if (!string.IsNullOrEmpty(str))
                        {
                            int id = int.Parse(strs[i].Replace(".png", "").Split('#')[1].CutLast(2));
                            if (id >= 100000 && id <= 300000)
                            {
                                File.Copy(demoImagePath, path + "/" + strs[i].CutLast(6) + ".png", true);
                            }
                        }
                    }
                    else
                    {
                        File.Copy(demoImagePath, path + "/" + strs[i], true);
                    }
                }
            }
        }
    }
    private static void CreateMenu()
    {
        foreach (var data in EnmuData)
        {
            ClassBody body = new ClassBody();
            body.name = data.Key;
            foreach (var item in data.Value)
            {
                string modify = "";
                if (data.Key == "color")
                {
                    modify = "xx";
                }
                if (string.IsNullOrEmpty(item.Value.name) || item.Value.name.Length <= 1)
                {
                    continue;
                }
                if (item.Value.name.Substring(1).IsInt())
                {
                    continue;
                }

                body.attributes.Add(modify + item.Value.key.Replace(" ", "") + " = " + item.Key + "," + "//" + item.Value.name);
            }
            body.CreateEnmu(FrameConfig.tableMenuScritPath + "/Enmu/", "//=======代码自动生成请勿修改=======", "tableMenu");
            //body.CreateEnmu(GameSign.hotTableScritPath + "/Enmu/", "//=======代码自动生成请勿修改=======", "tableMenu1");
        }
    }
    public static Dictionary<string, string> ArtData;
    public static Dictionary<string, string> ArtTableData;
    private static Dictionary<string, Dictionary<int, TableMenuData>> EnmuData;
    private static List<TableFieldAndType> TableFieldAndTypes;
    private static List<TableLineData> TableLineDatas;
    private class TableMenuData
    {
        public string name;
        public string key;
        public string value;
    }
    private class TableFieldAndType
    {
        public string tabName;
        public List<string> fieldNames = new List<string>();
        public List<string> fieldTypes = new List<string>();
        public string tabFatherName;
    }
    private class TableLineData
    {
        public string tabName;
        public List<string> linelst = new List<string>();
    }
    private static Dictionary<string, TableFieldAndType> ChooseTaskTabFields = new Dictionary<string, TableFieldAndType>();
    public static void LoadData(string path, string rowFlag, string columnFlag, bool isFrame, bool isCache)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("找不到:" + path);
            return;
        }
        using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
            DataSet result = excelDataReader.AsDataSet();
            //DataSet result = excelDataReader.AsDataSet();
            for (int k = 0; k < result.Tables.Count; k++)
            {
                var table = result.Tables[k];
                if (table.TableName.Contains("Sheet"))
                {
                    continue;
                }
                if (!table.TableName.Contains("_") && !isCache)
                {
                    continue;
                }
                //if(table.TableName.Contains("starInfo"))
                //    Debug.LogError(table.TableName);
                // 获取表格有多少列 
                int columns = table.Columns.Count;
                // 获取表格有多少行 
                int rows = table.Rows.Count;
                int begin = 4;
                bool isNeedCreate = false;
                int idIndex = -1;
                int nameIndex = -1;
                int enmuIndex = -1;
                int enmuValueIndex = -1;
                // int mscIndex = -1;
                //int activePrivilege = -1;
                //int modifyIndex = -1;
                string tableArtName = table.TableName.Replace("配置表", "").Replace("_", "") + "_" + table.Rows[1][1].ToString().Trim();
                tableArtName = tableArtName.Replace("配置表", "");

                bool isHaveEnmu = false;
                //Debug.LogError(table.TableName);

                var tabName = table.TableName.Replace("E_", "").Replace("TD_", "");
                if (table.TableName.Split('_')[0] == "E")
                {
                    isHaveEnmu = true;
                    //Debug.LogError(table.TableName);
                    if (EnmuData.ContainsKey(tabName))
                    {
                        EnmuData[tabName] = new Dictionary<int, TableMenuData>();
                    }
                    else {
                        EnmuData.Add(tabName, new Dictionary<int, TableMenuData>());
                    }
                  
                }

                // 根据行列依次打印表格中的每个数据
                List<string> CSTypes = new List<string>();
                List<string> fieldNames = new List<string>();
                List<string> fieldTypes = new List<string>();
                for (int j = 0; j < columns; j++)
                {
                    CSTypes.Add(table.Rows[2][j].ToString().Trim());
                }
                for (int j = 0; j < columns; j++)
                {
                    //if (CSTypes[j] == "S")
                    //{
                    //    continue;
                    //}
                    string fieldName = table.Rows[1][j].ToString().Trim();
                    fieldNames.Add(fieldName);
                    //Debug.LogError("fieldName:" + fieldName);
                    if (fieldName.ToLower() == "id")
                    {
                        idIndex = j;
                    }
                    if (fieldName == "enumDes")
                    {
                        nameIndex = j;
                    }
                    if (fieldName == "enumKey")
                    {
                        enmuIndex = j;
                    }
                    if (fieldName == "enum_value")
                    {
                        enmuValueIndex = j;
                    }
                }
                for (int j = 0; j < columns; j++)
                {
                    string str = table.Rows[2][j].ToString().Trim();
                    if (!TypeTemStrs.Contains(str))
                    {
                        str = ChangeTypeName(str);
                    }
                    fieldTypes.Add(str);
                }
                TableFieldAndType tableTypeOne = new TableFieldAndType();
                tableTypeOne.tabName = tabName;
                tableTypeOne.tabFatherName = path;
                if (isFrame)
                {
                    frameTabNames.Add(tabName);
                }
                tableTypeOne.fieldNames = fieldNames;
                tableTypeOne.fieldTypes = fieldTypes;
                TableFieldAndTypes.Add(tableTypeOne);
                List<string> lst = new List<string>();
                //第一行为表头，不读取
                List<string> HaveKeys = new List<string>();
                for (int i = begin; i < rows; i++)
                {
                    if (string.IsNullOrEmpty(table.Rows[i][0].ToString()))
                    {
                        break;
                    }
                    string str = "";
                    if (isNeedCreate)
                    {
                        ArtData[tableArtName] += "|";
                    }
                    string idStr = "";
                    string nameStr = "";
                    // try
                    // {
                        var tableKey = table.Rows[i][idIndex].ToString().Trim().CutNoUse();
                        if (HaveKeys.Contains(tableKey))
                        {
                            Debug.LogError(table.TableName + "有同名Id:" + tableKey);
                            continue;
                        }
                        HaveKeys.Add(tableKey);
                    // }
                    // catch (Exception err)
                    // {
                        // Debug.LogError(err.Message);
                        // Debug.LogError(table.TableName);
                        // return;
                    // }

                    for (int j = 0; j < columns; j++)
                    {
                        if (CSTypes[j] == "S")
                        {
                            continue;
                        }
                        var valueStr = table.Rows[i][j].ToString().Trim().CutNoUse();

                        if (isNeedCreate)
                        {
                            if (j == idIndex)
                            {
                                idStr = valueStr;
                            }
                        }
                        str += valueStr + rowFlag;
                    }
                    if (isNeedCreate)
                    {
                        ArtData[tableArtName] += nameStr + "#" + idStr + ".png";
                    }
                    try
                    {
                        if (isHaveEnmu)
                        {
                            var idstr = table.Rows[i][idIndex].ToString().Trim().CutNoUse();
                            var namestr = table.Rows[i][nameIndex].ToString().Trim().CutNoUse();
                            var enmuName = table.Rows[i][enmuIndex].ToString().Trim().CutNoUse();
                            string enmuValue = "";
                            if (enmuValueIndex != -1)
                            {
                                enmuValue = table.Rows[i][enmuValueIndex].ToString().Trim().CutNoUse();
                            }
                            TableMenuData data = new TableMenuData();
                            data.name = namestr;
                            data.key = enmuName;
                            data.value = enmuValue;
                            try
                            {
                                EnmuData[tabName].Add(int.Parse(idstr), data);
                            }
                            catch
                            {
                                Debug.LogError(table.TableName);
                            }
                        }
                    }
                    catch
                    {
                        Debug.LogError("配置表错误:" + tabName);
                    }
                    str = str.CutLast();
                    lst.Add(str);
                }
                TableLineData lineData = new TableLineData();
                lineData.tabName = tabName;
                lineData.linelst = lst;
                TableLineDatas.Add(lineData);
                tableNames.Add(tabName);
            }
        }
    }
    private static string ChangeTypeName(string str)
    {
        if (str.Contains("array|"))
        {
            return str.Split('|')[1] + "[]";
        }
        if (str.Contains("array2|"))
        {
            return str.Split('|')[1] + "[][]";
        }
        if (str.Contains("dic|"))
        {
            //Debug.LogError(str);
            var strs = str.Split('|')[1].Split(',');
            return "System.Collections.Generic.Dictionary<xx,yy>".Replace("xx", strs[0]).Replace("yy", strs[1]);
        }
        if (str == "item") {
            return "System.Collections.Generic.List<TreeData.item>";
        }
        return "string";
    }
    public static void CreateScript(string fathername,string name, List<string> fieldNames, List<string> fieldTypes)
    {
        //if (name == "universe") {
        //    Debug.LogError("universe" + fathername);
        //}
        if (fieldNames.Count != fieldTypes.Count)
        {
            Debug.LogError(name + ":" + "有字段类型未填写!");
            return;
        }
        ClassBody body = new ClassBody();
        body.name = name;
        body.div = "ITable";
        List<string> bodys = new List<string>();
        bodys.Add("string[] strs = str.Split(\'" + GameSign.RowFlag + "\');");
        for (int i = 0; i < fieldNames.Count; i++)
        {
            if (string.IsNullOrEmpty(fieldNames[i]))
            {
                continue;
            }
            body.attributes.Add("public " + fieldTypes[i] + " " + fieldNames[i] + ";");
            bodys.Add(GetBody(fieldNames[i], fieldTypes[i], i));
        }
        body.isSerializable = true;
        bodys.Add("return int.Parse(strs[0]);");
        body.funcs.Add(body.GetFunction("public int Init(string str)", bodys));
        if (frameTabNames.Contains(name))
        {
            body.CreateClass(FrameConfig.tableFrameScritPath, "//=======代码自动生成请勿修改=======", "Table");
        }
        else
        {
            body.CreateClass(FrameConfig.tableScritPath, "//=======代码自动生成请勿修改=======", "Table");
        }
        //body.CreateClass(GameSign.hotTablePath, "//=======代码自动生成请勿修改=======", "Table1");
    }
    private static string GetBody(string fieldName, string fieldType, int index)
    {
        string str = "if(!string.IsNullOrEmpty(strs[" + index + "])){";
        string end = "}";
        if (fieldType == "int")
        {
            return str + fieldName + " = int.Parse(strs[" + index + "]);" + end;
        }
        if (fieldType == "float")
        {
            return str + fieldName + " = float.Parse(strs[" + index + "]);" + end;
        }
        if (fieldType == "long")
        {
            return str + fieldName + " = long.Parse(strs[" + index + "]);" + end;
        }
        if (fieldType == "bool")
        {
            return str + fieldName + " = strs[" + index + "] == 1;" + end;
        }
        if (fieldType == "lst")
        {
            return str + fieldName + " = strs[" + index + "] == 1;" + end;
        }
        if (fieldType.Contains("[][]"))
        {
            var sType = fieldType.Split('[')[0];
            return str + fieldName + " = strs[" + index + "].TwoStringToArray<XXX>();".Replace("XXX", sType) + end;
        }
        if (fieldType.Contains("[]"))
        {
            var sType = fieldType.Split('[')[0].FirstToUpper();
            return str + fieldName + " = strs[" + index + "].SplitToXXXArray(',');".Replace("XXX", sType) + end;
        }
       
        if (fieldType.Contains("Dictionary<"))
        {
            var strs = fieldType.Split('<')[1].Split(',');
            var sType1 = strs[0].FirstToUpper();
            var sType2 = strs[1].FirstToUpper().Replace(">", "");
            return str + fieldName + " = strs[" + index + "].ToDicXXXYYY();".Replace("XXX", sType1).Replace("YYY", sType2) + end;
        }
        if (fieldType.Contains("List<TreeData.item>")) {
            return str + fieldName + " = strs[" + index + "].ToItems();" + end;
        }

        return str + fieldName + " = strs[" + index + "];" + end;
    }
    public static void CreateTableText(List<string> lst, string tableName, Dictionary<string, string> battleNpc)
    {
        string str = "";
        foreach (var item in lst)
        {
            str += item + GameSign.ColumnsFlag;
        }
        (FrameConfig.UnityTabPath + tableName + ".txt").WriteByUTF8(str.CutLast(GameSign.ColumnsFlag.Length));
    }
    private static string TaskTabAddSameNum(string str, int index, int sameNum)
    {
        var strs = str.Split(GameSign.RowFlagChar);
        strs[index] = sameNum.ToString();
        var newstr = "";
        foreach (var item in strs)
        {
            newstr += item + GameSign.RowFlag;
        }
        return newstr.CutLast();
    }
    public static void CreateEnmuScript(List<string> tableNames)
    {
        ClassBody body = new ClassBody();
        body.name = "TableName";
        for (int i = 0; i < tableNames.Count; i++)
        {
            var newName = tableNames[i] + ",";
            if (!body.attributes.Contains(newName))
            {
                body.attributes.Add(newName);
            }
        }
        body.isSerializable = true;
        body.CreateEnmu(FrameConfig.tableScritPath, "//=======代码自动生成请勿修改=======");
        body.name = "TableName1";
        //body.CreateEnmu(GameSign.hotTableScritPath, "//=======代码自动生成请勿修改=======");
    }
    public static void CreateTableCacheScript(List<string> tableNames)
    {
        ClassBody body = new ClassBody();
        body.name = "TableCache";
        body.div = "Single<TableCache>";
        body.use = "using Table;\nusing System.Collections.Generic;\n";
        List<string> strs = new List<string>();
        List<string> strsWrite = new List<string>();
        List<string> overTableName = new List<string>();
        for (int i = 0; i < tableNames.Count; i++)
        {
            if (overTableName.Contains(tableNames[i]))
            {
                continue;
            }
            overTableName.Add(tableNames[i]);
            string fieldName = tableNames[i].Trim();
            string fieldNameData = fieldName + "Table";
            string _fieldNameData = "_" + fieldNameData;
            if (tableNames[i] == "side" || tableNames[i] == "pub" || tableNames[i] == "unionbuild" || tableNames[i] == "unionmanage")
            {
                tableNames[i] = "main";
            }
            if (tableNames[i] == "side_talk" || tableNames[i] == "pub_talk" || tableNames[i] == "unionbuild_talk" || tableNames[i] == "unionmanage_talk")
            {
                tableNames[i] = "main_talk";
            }
            body.attributes.Add("private Dictionary<int," + tableNames[i].Trim() + "> " + _fieldNameData + ";");
            List<string> bodys = new List<string>();
            bodys.Add("if(" + _fieldNameData + " == null ||" + _fieldNameData + ".Count == 0)");
            bodys.Add("    " + _fieldNameData + " = TableRead.Instance.ReadTable<" + tableNames[i].Trim() + ">(\"" + tableNames[i] + "\");");
            strs.Add("    " + _fieldNameData + " = TableRead.Instance.ReadTable<" + tableNames[i].Trim() + ">(\"" + fieldName + "\");");
            //GameSign.Instance.SaveTableByte(TableCache.Instance.battle_arrayTable, TableName.battle_array);
            strsWrite.Add("    " + "TableRead.Instance.SaveTableByte(TableCache.Instance." + fieldNameData + ", TableName." + fieldName + ");");
            bodys.Add("return " + _fieldNameData + ";");
            string getStr = body.AddArributeBody(bodys, "      ");
            if (fieldNameData == "explore_pirateTable" || fieldNameData == "battle_arrayTable")
            {
                body.funcs.Add(body.GetArributeFunction("private Dictionary<int," + tableNames[i].Trim() + "> " + fieldNameData, getStr));
            }
            else
            {
                body.funcs.Add(body.GetArributeFunction("public Dictionary<int," + tableNames[i].Trim() + "> " + fieldNameData, getStr));
            }
        }
        body.funcs.Add(body.GetFunction("public void ReadAllTab()", strs));
        //body.funcs.Add(body.GetFunction("public void WriteAllTabToBytes()", strsWrite));
        body.CreateClass(FrameConfig.TableCache, "//=======代码自动生成请勿修改=======");
    }
}
