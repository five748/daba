using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Reflection;
using ExtendForCreateScript;
public enum TagState
{
    UIGameObject,
    UITransform,
    UISprite,
    UILabel,
    UISlider,
    UIShowOne,
    UIMenuGrid,
    UIScrollGrid,
    UITransformChilds,
    UITexture,
}
public class CreateScrite
{
    static string Top = "//=====================================由代码自动生成请勿修改=======================================";
    static string AttributeTag = "//-------------------------------属性标记请勿删除------------------------------------";
    static string InitAttributeTag = "//-----------------------------初始化属性标记请勿删除--------------------------------";
    static string AddListenTag = "//--------------------------添加button点击事件标记请勿删除---------------------------";
    static string ClickFunTag = "//------------------------------点击方法标记请勿删除---------------------------------";
    static string ViewFunTag = "//-------------------------------表现方法标记请勿删除---------------------------------";
    static string TargetName;

    static GameObject Target;
    static Dictionary<string, List<Transform>> MyTagTrans;
    static List<Transform> TextNeedTranslated;
    static List<Transform> ImageNeedTranslated;
    public static string UIScriptPath(string targetName)
    {
        return Application.dataPath + "/" + "Script/Hot//UI/";
    }
    //===============================================主函数==================================================
    //自动生成UI功能代码
    //[MenuItem("程序工具/代码/自动生成所有UI功能代码")]
    static void CreateAllUIScript()
    {
        if (!EditorUtility.DisplayDialog("", "是否修改所有UI功能代码?", "ok", "no"))
            return;
        GetFileNamesAndShort("", Application.dataPath + "/Res/Prefab/UIPrefab/", (fullpath, dirpath) =>
        {
            if (fullpath.EndsWith(".prefab"))
            {
                Target = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Res/Prefab/UIPrefab/" + dirpath);
                Debug.Log(Target.name);
                if (Target.name.Substring(0, 2) == "UI")
                    CreateByTarget(Target);
            }
        });
    }
    public static string GetPlatformPath(string path)
    {
        return path.ToLower().Replace(@"\", @"/");
    }
    public static void GetFileNamesAndShort(string begin, string targetPath, System.Action<string, string> finish)
    {
        targetPath = GetPlatformPath(targetPath);
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
    private static void CreateByTarget(GameObject go)
    {
        //-------------------------------------

        InitList();
        Target = go;
        TargetName = go.name;
        if (TargetName.Substring(0, 2) != "UI")
            return;
        Target.tag = "UIRoot";
        SetListByTag(Target.transform);
        CreateAuto();
        CreateUIClass();
        AssetDatabase.Refresh();
    }
    //自动生成UI功能代码
    [MenuItem("程序工具/自动生成UI功能代码")]
    static void CreateUIScript()
    {
        if (!SetTarget())
            return;
        CreateByTarget(Target);
    }
    public static void AddScriptByString()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length != 1)
        {
            return;
        }
        string name = targets[0].name;
        if (name.Length <= 2)
            return;
        if (name.Substring(0, 2) != "UI")
            return;
        //Debug.Log(name);
        Type aa = Type.GetType(name);

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                Type tt = asm.GetType(name);
                if (tt != null)
                {
                    if (!targets[0].GetComponent(tt))
                        targets[0].AddComponent(tt);
                    break;
                }
            }
            finally { }
        }
    }
    public static Type GetType(string TypeName)
    {
        var type = Type.GetType(TypeName);

        // If it worked, then we‘re done here
        if (type != null)
            return type;

        // Get the name of the assembly (Assumption is that we are using
        // fully-qualified type names)
        var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));

        // Attempt to load the indicated Assembly
        var assembly = Assembly.LoadWithPartialName(assemblyName);
        if (assembly == null)
            return null;

        // Ask that assembly to return the proper Type
        return assembly.GetType(TypeName);
    }
    static void InitList()
    {
        MyTagTrans = new Dictionary<string, List<Transform>>();
        TextNeedTranslated = new List<Transform>();
        ImageNeedTranslated = new List<Transform>();
        string[] names = Enum.GetNames(typeof(TagState));
        for (int i = 0; i < names.Length; i++)
        {
            MyTagTrans.Add(names[i], new List<Transform>());
        }
    }
    static void CreateAuto()
    {
        ClassBody data = new ClassBody();
        // using
        data.use = GetAutoUsing();
        data.name = GetAutoClassName();
        data.attributes = GetAutoAllAttribute();
        // Init
        data.funcs.Add(
            GetFunction("public void Init(Transform myTran, " + TargetName + " model)",
            AddTab() + AddTab() + "UITool.Instance.SetAnchor(myTran);"
             + AddLineAndTab() + AddTab() + "InitGameObject(myTran);"
            + AddLineAndTab() + AddTab() + "AddListen(model);"
            + AddLineAndTab() + AddTab() + "SetLanguage(myTran);"
            )
            );
        // InitGameObject
        data.funcs.Add(
            GetFunction(
                "private void InitGameObject(Transform myTran)", GetAllFindChildBody()
                )
            );
        // AddListen
        data.funcs.Add(
            GetFunction("void AddListen(" + TargetName + " model)", GetAddListenBody())
            );
        // SetLanguage
        data.funcs.Add(
            GetFunction("public void SetLanguage(Transform myTran)", GetSetLanguageBody())
            );


        GetVMAllAttribute(data.attributes, data.funcs);
        data.funcs.Add(GetViewFuncs());
        data.CreateClass(UIScriptPath(TargetName) + TargetName + "/");
    }
    public static string GetSetLanguageBody()
    {
        string res = "//组件:Text 部分".preTab().preTab().line();
        res = "".tab().tab().add(res.add("\t\tText get;").line()) + "".tab().tab().add("Transform trf;").line();
        string itemName = "get";
        string trfName = "trf";

        foreach (var item in TextNeedTranslated)
        {
            var ntf = GetFindChildBody(new List<Transform>() { item }, TagState.UITransform).Split('=')[1];
            ntf = ntf.pre("".add(trfName).add("=")).add("\n").tab().tab();

            ntf += "if(".add(trfName).add("!=null){\n").tab().tab().tab()
                .add(itemName).add("=").add(trfName).add(".GetComponent<Text>();\n");

            res = res.tab().tab().add(ntf);

            res += ""
                .tab().tab().tab()
                .add(itemName).add($".text = {itemName}.text.ChangePrefabStr();").line().tab().tab().add("}\n");
        }

        //itemName = "image";
        //res = res.add($"\t\tImage {itemName};").preTab().preTab().line();
        //foreach (var item in ImageNeedTranslated)
        //{
        //
        //    var ntf = GetFindChildBody(new List<Transform>() { item }, TagState.UIGameObject).Split('=')[1];
        //    ntf = ntf.Substring(0, ntf.Length - 1)
        //        .add(".GetComponent<Image>();")
        //        .pre("".add(itemName).add("=")
        //        );
        //    res = res.tab().tab().add(ntf);
        //
        //    res = res.line();
        //    res += ""
        //        .tab()
        //        .tab()
        //        .add(itemName).add($" = {itemName}.ChangeImageMultilan();").line();
        //}

        return res;
    }
    static void CreateUIClass()
    {
        string cache = "";
        //Component script = Target.GetComponent(Type.GetType(GetClassNameC()));
        string path = UIScriptPath(TargetName) + TargetName + "/" + TargetName + ".cs";
        if (File.Exists(path))
        {
            cache = ReadStringByFile(path);
            string clickFuncOld = GetCache(cache, ClickFunTag);
            string clickFuncNew = GetClickFuncs(clickFuncOld);
            cache = cache.Replace("public void BaseInit()", "public override void BaseInit()");
            cache = cache.Replace(clickFuncOld, clickFuncNew);
            WriteByUTF8(cache, TargetName);
            return;
        }
        ClassBody data = new ClassBody();
        data.use = GetUsingUI();
        data.name = TargetName;
        data.div = "BaseMonoBehaviour";
        data.attributes.Add("private " + GetAutoClassName() + " Auto = new " + GetAutoClassName() + "();");
        data.funcs.Add("public override void BaseInit(){" + AddTab() + AddLineAndTab() + AddTab()
           + "Auto.Init(transform, this);" + AddTab() + AddLineAndTab() + AddTab()
           + "Init(param);" + AddTab() + AddLineAndTab()
           + "}");
        data.funcs.Add(ClickFunTag);
        data.funcs.Add(GetClickFuncs(cache) + ClickFunTag);
        data.funcs.Add("private void Init(string param){" + AddLine() + AddTab() + AddTab() + "UIManager.FadeOut();"
             + AddLine() + AddLine() + AddTab() + "}");
        data.CreateClass(UIScriptPath(TargetName) + TargetName + "/");
    }
    //读取txt
    public static string ReadStringByFile(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.Default);
        string line;
        StringBuilder sb = new StringBuilder();
        while ((line = sr.ReadLine()) != null)
        {
            sb.AppendLine(line);
        }
        //sr.Flush();
        sr.Close();
        return sb.ToString();
    }
    static bool SetTarget()
    {
        Target = GetChooseGameObject();
        if (!Target)
            return false;
        TargetName = Target.name;
        if (TargetName.Substring(0, 2) != "UI")
            return false;
        Target.tag = "UIRoot";
        return true;
    }
    static GameObject GetChooseGameObject()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length == 0)
        {
            Debug.Log("请选择要生成代码的物体");
            return null;
        }
        if (targets.Length > 1)
        {
            Debug.Log("只能选中一个物体");
            return null;
        }
        return targets[0];
    }
    static string ChangeName(string name)
    {
        string startName = name.Substring(0, 2);
        if (startName.Equals("UI"))
            return FirstUp(name);
        return "UI" + FirstUp(name);
    }
    static void SetListByTag(Transform go)
    {
        for (int i = 0; i < go.childCount; i++)
        {
            Transform target = go.GetChild(i);
            AddListByTag(target.tag, target);
            TryAddTextNeedTranslate(target);
            TryAddImageNeedTranslate(target);
            SetListByTag(target);
        }
    }
    static void AddListByTag(string tag, Transform myTran)
    {
        if (!MyTagTrans.ContainsKey(tag))
            MyTagTrans.Add(tag, new List<Transform>());
        MyTagTrans[tag].Add(myTran);
    }
    /// <summary>
    /// 添加需要多语言的物体
    /// </summary>
    /// <param name="Tran"></param>
    static void TryAddTextNeedTranslate(Transform Tran)
    {
        var text = Tran.GetComponent<UnityEngine.UI.Text>();
        if (text == null)
        {
            return;
        }
        if (text.text.ContainChinese())
        {
            TextNeedTranslated.Add(Tran);
        }
    }
    static void TryAddImageNeedTranslate(Transform Tran)
    {
        var image = Tran.GetComponent<UnityEngine.UI.Image>();
        if (image != null && image.sprite != null && image.sprite.name.StartsWith("Art"))
        {
            ImageNeedTranslated.Add(Tran);
        }
    }
    static string GetTargePath(Transform myTran)
    {
        if (myTran.name.Equals(TargetName))
            return "";
        return GetTargePath(myTran.parent) + "/" + myTran.name;
    }

    //============================================ ClassComponent ==================================================
    static string GetAutoUsing()
    {
        List<string> usingStrings = new List<string>() {
            "UnityEngine",
            "System.Collections",
            "System.Collections.Generic",
            "UnityEngine.UI"
        };
        return GetUsing(usingStrings);
    }
    static string GetUsingUI()
    {
        List<string> usingStrings = new List<string>() {
            "UnityEngine",
            "System.Collections",
            "System.Collections.Generic",
            "UnityEngine.UI"
        };
        return GetUsing(usingStrings);
    }
    static string GetUsing(List<string> usingStrings)
    {
        string myString = "";
        for (int i = 0; i < usingStrings.Count; i++)
        {
            string info = "using " + usingStrings[i] + ";";
            myString += info + "\n";
        }
        return myString;
    }
    static string GetAutoClassName()
    {
        return (TargetName + "Auto").Trim();
    }
    static string GetAttribute(string attribute, string name)
    {
        return attribute + FirstUp(name) + ";";
    }
    static string GetFunction(string func, string body)
    {
        return func + AddBody(body, AddTab());
    }
    static string GetClickFuncs(string cache)
    {
        string last = "";
        if (cache != "")
        {
            int i = cache.IndexOf(ClickFunTag);
            last = cache.Substring(i);
            cache = cache.Substring(0, i);
        }
        if (MyTagTrans.ContainsKey("UIClick"))
        {
            foreach (var info in MyTagTrans["UIClick"])
            {
                if (cache.IndexOf("Click" + FirstUp(info.name) + "(") == -1)
                {
                    //Debug.Log(info.name);
                    cache += GetFunction("public void Click" + FirstUp(info.name)
                        + "(GameObject button)", AddTab() + AddTab() + "Debug.Log(\"click\" + button.name);") + AddLineAndTab();
                }
            }
        }
        if (MyTagTrans.ContainsKey("UIMenuGrid"))
        {
            foreach (var info in MyTagTrans["UIMenuGrid"])
            {
                if (cache.IndexOf("Click" + FirstUp(info.name) + "Menus") == -1)
                {
                    cache += GetFunction("public void Click" + FirstUp(info.name) + "Menus"
                        + "(GameObject button, System.Action callback)", AddTab() + AddTab() + "Debug.Log(\"click\" + button.name);"
                        + AddLineAndTab() + AddTab() + "callback();") + AddLineAndTab();
                }
            }
        }
        if (MyTagTrans.ContainsKey("UIScrollGrid"))
        {
            foreach (var info in MyTagTrans["UIScrollGrid"])
            {
                if (cache.IndexOf("Click" + FirstUp(info.name) + "Items") == -1)
                {
                    cache += GetFunction("public void Click" + FirstUp(info.name) + "Items"
                        + "(GameObject button)", AddTab() + AddTab() + "Debug.Log(\"click\" + button.name);") + AddLineAndTab();
                }
            }
        }
        return cache + last;
    }
    static string GetViewFuncs()
    {
        string cache = "";
        foreach (var dic in MyTagTrans)
        {
            switch (dic.Key)
            {
                case "UIGameObjectActive":
                    cache += SetActiveViewFun(dic.Value);
                    continue;
                case "UITranMovePos":
                    cache += SetMovePosViewFun(dic.Value);
                    continue;
                case "UITranMoveAroundOnce":
                    cache += SetMoveAroundOnceViewFun(dic.Value);
                    continue;
                case "UITranChangeScan":
                    cache += SetChangeScanViewFun(dic.Value);
                    continue;
                case "UITranChangeScanAroundOnce":
                    cache += SetChangeScanAroundOnceViewFun(dic.Value);
                    continue;
                case "UILabel":
                    cache += SetUILabelViewFun(dic.Value);
                    continue;
                case "UISprite":
                    cache += SetUISpriteViewFun(dic.Value);
                    continue;
                case "UISlider":
                    cache += SetUISliderViewFun(dic.Value);
                    continue;
                case "UITexture":
                    cache += SetUITextureViewFun(dic.Value);
                    continue;
                case "UIShowOne":
                    cache += SetUIShowOneViewFun(dic.Value);
                    continue;
                case "UIScrollGrid":
                    cache += SetUIScrollGridViewFun(dic.Value);
                    continue;
            }
        }
        return RemoveAddLineAndTable(cache);
    }
    static string SetActiveViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("private void " + GetFuncName(myTran) + "(bool active)",
                    AddTab() + AddTab() + FirstUp(myTran.name) + ".SetActive(active);") + AddLineAndTab();
        }
        return str;
    }
    static string SetMovePosViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("public void " + GetFuncName(myTran) + "(Vector3 targetPos, float speed, GameTools.Finish finish)",
                    AddTab() + AddTab() + "AssetTool.Instance.MoveToward(" + FirstUp(myTran.name) + ", targetPos, speed, finish);") + AddLineAndTab();
        }
        return str;
    }
    static string SetMoveAroundOnceViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str = GetFunction("public void " + GetFuncName(myTran) + "(Vector3 targetPos, float speed, GameTools.Finish finish)",
                AddTab() + AddTab() + "AssetTool.Instance.MoveAroundOnce(" + FirstUp(myTran.name) + ", targetPos, speed, finish);") + AddLineAndTab();
        }
        return str;
    }
    static string SetChangeScanViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("public void " + GetFuncName(myTran) + "(float scan, float speed, GameTools.Finish finish)",
                AddTab() + AddTab() + "AssetTool.Instance.ChangeScan(" + FirstUp(myTran.name) + ", scan, speed, finish);") + AddLineAndTab();
        }
        return str;
    }
    static string SetChangeScanAroundOnceViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("public void " + GetFuncName(myTran) + "(float scan, float speed, GameTools.Finish finish)",
                AddTab() + AddTab() + "AssetTool.Instance.ChangeScanAroundOnce(" + FirstUp(myTran.name) + ", scan, speed, finish);") + AddLineAndTab();
        }
        return str;
    }
    static string SetUILabelViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("private void " + GetFuncName(myTran) + "(string text)",
                AddTab() + AddTab() + FirstUp(myTran.name) + ".text = text;") + AddLineAndTab();
        }
        return str;
    }
    static string SetUISpriteViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("private void " + GetFuncName(myTran) + "(string spriteName)",
                AddTab() + AddTab() + FirstUp(myTran.name) + ".SetImage(spriteName);") + AddLineAndTab();
        }
        return str;
    }
    static string SetUITextureViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("private void " + GetFuncName(myTran) + "(string texturePath)",
                AddTab() + AddTab() + "GameTools.SetUITexture(" + FirstUp(myTran.name) + ", texturePath);") + AddLineAndTab();
        }
        return str;
    }
    static string SetUIShowOneViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("public void " + "Set" + FirstUp(myTran.name) + "SO(int id, List<string> itemData)",
                AddTab() + AddTab() + FirstUp(myTran.name) + ".ShowItem(id, itemData);") + AddLineAndTab();
        }
        return str;
    }
    static string SetUIScrollGridViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("public void " + "Set" + FirstUp(myTran.name) + "SG(Dictionary<string, List<string>> itemDatas, string filterCurrentType = \"\", string sortField = \"\", int sortItemDataIndex = -1)",
                AddTab() + AddTab() + FirstUp(myTran.name) + ".ShowItems(itemDatas, filterCurrentType, sortField, sortItemDataIndex);") + AddLineAndTab();
        }
        return str;
    }
    static string SetUISliderViewFun(List<Transform> myTrans)
    {
        string str = "";
        foreach (var myTran in myTrans)
        {
            str += GetFunction("private void " + GetFuncName(myTran) + "(float value)",
                AddTab() + AddTab() + FirstUp(myTran.name) + ".fillAmount = value;") + AddLineAndTab();
        }
        return str;
    }
    static string GetFuncName(Transform myTran)
    {
        return "Set" + myTran.tag + FirstUp(myTran.name);
    }
    //=======================================ClassBody=======================================
    /*
     * 属性定义区
     */
    static List<string> GetAutoAllAttribute()
    {
        List<string> dataAttributes = new List<string>();
        foreach (var dic in MyTagTrans)
        {
            if (dic.Key == "Untagged")
                continue;
            switch (dic.Key)
            {
                case "UISprite":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public Image ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UILabel":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public Text ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UITexture":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public Image ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UISlider":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public Image ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UIClick":
                case "UIButtonClick":
                case "UIGameObjectActive":
                case "UIGameObject":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public GameObject ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UITranMovePos":
                case "UITranMoveAroundOnce":
                case "UITranChangeScan":
                case "UITranChangeScanAroundOnce":
                case "UIScroll":
                case "UITransform":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public Transform ", FirstUp(myTran.name)));

                    }
                    continue;
                case "UIMenuGrid":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public MenuGrid ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UIShowOne":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public ShowOne ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UIScrollGrid":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public ScrollGrid ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UITransformChilds":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public Transform[] ", FirstUp(myTran.name) + "Childs"));
                    }
                    continue;

            }
            foreach (var myTran in dic.Value)
            {

                dataAttributes.Add(GetAttribute("public Object ", FirstUp(myTran.name)));
            }
        }

        return dataAttributes;
    }
    static void GetUICAttribute(List<string> dataAttributes)
    {
        if (MyTagTrans.ContainsKey("UIClick"))
        {
            foreach (var myTran in MyTagTrans["UIClick"])
            {
                dataAttributes.Add(GetAttribute("private GameObject ", FirstUp(myTran.name)));
            }
        }
        if (MyTagTrans.ContainsKey("UIMenuGrid"))
        {
            foreach (var myTran in MyTagTrans["UIMenuGrid"])
            {
                dataAttributes.Add(GetAttribute("private MenuGrid ", FirstUp(myTran.name)));
                if (!myTran.GetComponent<MenuGrid>())
                    myTran.gameObject.AddComponent<MenuGrid>();
            }
        }
        if (MyTagTrans.ContainsKey("UIScrollGrid"))
        {
            //foreach (var myTran in MyTagTrans["UIScrollGrid"])
            //{
            //    dataAttributes.Add(GetAttribute("private ScrollGrid ", FirstUp(myTran.name)));
            //    if (!myTran.GetComponent<ScrollGrid>())
            //        myTran.gameObject.AddComponent<ScrollGrid>();

            //}
        }
    }
    static void GetViewAllAttribute(List<string> dataAttributes)
    {
        foreach (var dic in MyTagTrans)
        {
            if (dic.Key == "Untagged")
                continue;
            switch (dic.Key)
            {
                case "UIClick":
                case "UIGameObjectActive":
                case "UIGameObject":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public GameObject ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UITranMovePos":
                case "UITranMoveAroundOnce":
                case "UITranChangeScan":
                case "UITranChangeScanAroundOnce":
                case "UIScroll":
                case "UITransform":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public Transform ", FirstUp(myTran.name)));

                    }
                    continue;
                case "UIScrollGrid":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public ScrollGrid ", FirstUp(myTran.name)));
                    }
                    continue;
                case "UITransformChilds":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("public Transform[] ", FirstUp(myTran.name) + "Childs"));
                    }
                    continue;
            }
            foreach (var myTran in dic.Value)
            {
                dataAttributes.Add(GetAttribute("public Object ", FirstUp(myTran.name)));
            }
        }
    }
    static string GetAndSetString(string attributeName, string funcName)
    {
        return AddTab() + AddTab() + "get { return " + attributeName + "; }" + AddLineAndTab() + AddTab() +
            "set {" + AddLineAndTab() + AddTab() + AddTab() + "if(" + attributeName + " == value)" + AddLineAndTab() + AddTab() + AddTab() + AddTab() +
            "return;" + AddLineAndTab() + AddTab() + AddTab() + attributeName + " = value;" + AddLineAndTab() + AddTab() + AddTab() +
            funcName + "(" + attributeName + ");" + AddLineAndTab() + AddTab() + "}";
    }
    static string GetAndSetMoveString(string attributeName, string funcName, string speed, string delegateString)
    {
        return AddTab() + AddTab() + "get { return " + attributeName + "; }" + AddLineAndTab() + AddTab() +
            "set {" + AddLineAndTab() + AddTab() + AddTab() + "if(" + attributeName + " == value)" + AddLineAndTab() + AddTab() + AddTab() + AddTab() +
            "return;" + AddLineAndTab() + AddTab() + AddTab() + attributeName + " = value;" + AddLineAndTab() + AddTab() + AddTab() +
            funcName + "(" + attributeName + "," + speed + "," + delegateString + ");" + AddLineAndTab() + AddTab() + "}";
    }
    static void GetVMAllAttribute(List<string> dataAttributes, List<string> dataFuncs)
    {
        foreach (var dic in MyTagTrans)
        {
            switch (dic.Key)
            {
                case "UIGameObjectActive":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("private bool _active", FirstUp(myTran.name)));
                        dataFuncs.Add(GetFunction("public bool Active" + FirstUp(myTran.name), GetAndSetString("_active" + FirstUp(myTran.name),
                           GetFuncName(myTran))));
                    }
                    continue;
                case "UILabel":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("private string _string", FirstUp(myTran.name)));
                        dataFuncs.Add(GetFunction("public string String" + FirstUp(myTran.name), GetAndSetString("_string" + FirstUp(myTran.name),
                            GetFuncName(myTran))));
                    }
                    continue;
                case "UISprite":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("private string _sprite", FirstUp(myTran.name)));
                        dataFuncs.Add(GetFunction("public string Sprite" + FirstUp(myTran.name), GetAndSetString("_sprite" + FirstUp(myTran.name),
                            GetFuncName(myTran))));
                    }
                    continue;
                case "UITexture":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("private string _texture", FirstUp(myTran.name)));
                        dataFuncs.Add(GetFunction("public string Texture" + FirstUp(myTran.name), GetAndSetString("_texture" + FirstUp(myTran.name),
                            GetFuncName(myTran))));
                    }
                    continue;
                case "UISlider":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("private float _slider", FirstUp(myTran.name)));
                        dataFuncs.Add(GetFunction("public float Slider" + FirstUp(myTran.name), GetAndSetString("_slider" + FirstUp(myTran.name),
                            GetFuncName(myTran))));
                    }
                    continue;
                case "UITranMovePos":
                case "UITranMoveAroundOnce":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("private Vector3 _pos", FirstUp(myTran.name)));
                        dataAttributes.Add(GetAttribute("private float Speed", FirstUp(myTran.name)));
                        dataAttributes.Add(GetAttribute("private GameTools.Finish Delegate", FirstUp(myTran.name)));
                        dataFuncs.Add(GetFunction("public Vector3 Pos" + FirstUp(myTran.name), GetAndSetMoveString("_pos" + FirstUp(myTran.name),
                            GetFuncName(myTran), "Speed" + FirstUp(myTran.name), "Delegate" + FirstUp(myTran.name))));
                    }
                    continue;
                case "UITranChangeScan":
                case "UITranChangeScanAroundOnce":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("private float _scan", FirstUp(myTran.name)));
                        dataAttributes.Add(GetAttribute("private float Speed", FirstUp(myTran.name)));
                        dataAttributes.Add(GetAttribute("private GameTools.Finish Delegate", FirstUp(myTran.name)));
                        dataFuncs.Add(GetFunction("public float Scan" + FirstUp(myTran.name), GetAndSetMoveString("_scan" + FirstUp(myTran.name),
                            GetFuncName(myTran), "Speed" + FirstUp(myTran.name), "Delegate" + FirstUp(myTran.name))));
                    }
                    continue;
                case "UIScroll":
                    foreach (var myTran in dic.Value)
                    {
                        dataAttributes.Add(GetAttribute("private bool _", myTran.name));
                        //dataFuncs.Add(GetFunction("public void "));
                    }
                    continue;
            }
            //foreach (var myTran in dic.Value)
            //{
            //    dataAttributes.Add(GetAttribute("private bool _", myTran.name));
            //    //dataFuncs.Add(GetFunction("public void "));
            //}
        }
    }
    /*
     * Awake
     */
    static string GetAwake(string body)
    {
        return GetFunction("void Awake()", body);
    }
    /*
     * Start
     */
    static string GetStart(string body)
    {
        return GetFunction("void Start()", body);
    }
    /*
     * OnEnable
     */
    static string GetOnEnable(string body)
    {
        return GetFunction("void OnEnable()", body);
    }
    /*
     * InitGameObject
     */
    static string GetInitGameObject(string body)
    {
        return GetFunction("void InitGameObject(Transform myTran)", body);
    }
    /*
     * AddListen
     */
    static string GetAddListen(string body)
    {
        return GetFunction("void AddListen()", body);
    }
    /*
     * InitGameObject Body
     */
    static string GetAllFindChildBody()
    {
        string myString = "";
        foreach (var dic in MyTagTrans)
        {
            if (dic.Value.Count == 0)
                continue;
            switch (dic.Key)
            {
                case "UIClick":
                case "UIButtonClick":
                case "UIGameObjectActive":
                case "UIScrollItem":
                case "UIGameObject":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UIGameObject);
                    continue;
                case "UIMenuGrid":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UIMenuGrid);
                    continue;
                case "UIShowOne":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UIShowOne);
                    continue;
                case "UIScrollGrid":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UIScrollGrid);
                    continue;
                case "UITranMovePos":
                case "UITranMoveAroundOnce":
                case "UITranChangeScan":
                case "UITranChangeScanAroundOnce":
                case "UITransform":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UITransform);
                    continue;
                case "UILabel":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UILabel);
                    continue;
                case "UISprite":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UISprite);
                    continue;
                case "UISlider":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UISlider);
                    continue;
                case "UITexture":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UITexture);
                    continue;
                case "UITransformChilds":
                    myString += AddLine() + GetFindChildBody(dic.Value, TagState.UITransformChilds);
                    continue;
            }


        }
        if (myString != "")
        {
            //Debug.Log(myString.Substring(1));
            return myString.Substring(1);
        }
        return "";
    }

    static string GetFindChildBody(List<Transform> myTrans, TagState endState)
    {
        string myString = "";
        foreach (var myTran in myTrans)
        {
            string path = GetTargePath(myTran);
            myString += AddTab() + AddTab() + GetFindChild(myTran.name, path, endState);
        }
        return RemoveEmptyLine(myString);
    }
    static string GetFindChild(string name, string path, TagState endState)
    {
        path = path.Substring(1);
        switch (endState)
        {
            case TagState.UIGameObject:
                return FirstUp(name) + " = myTran.Find(\"" + path + "\").gameObject;\n";
            case TagState.UITransform:
                return FirstUp(name) + " = myTran.Find(\"" + path + "\");\n";
            case TagState.UIMenuGrid:
                return FirstUp(name) + " = myTran.Find(\"" + path + "\").GetComponent<MenuGrid>();\n";
            case TagState.UIShowOne:
                return FirstUp(name) + " = myTran.Find(\"" + path + "\").GetComponent<ShowOne>();\n";
            case TagState.UIScrollGrid:
                return FirstUp(name) + " = myTran.Find(\"" + path + "\").GetComponent<ScrollGrid>();\n";
            case TagState.UISprite:
                return FirstUp(name) + " = myTran.Find(\"" + path + "\").GetComponent<Image>();\n";
            case TagState.UILabel:
                return FirstUp(name) + " = myTran.Find(\"" + path + "\").GetComponent<Text>();\n";
            case TagState.UISlider:
                return FirstUp(name) + " = myTran.Find(\"" + path + "\").GetComponent<Image>();\n";
            case TagState.UITexture:
                return FirstUp(name) + " = myTran.Find(\"" + path + "\").GetComponent<Image>();\n";
            case TagState.UITransformChilds:
                return FirstUp(name) + "Childs" + " = GameTools.GetChilds(myTran.Find(\"" + path + "\"));\n";
        }
        return "";
    }
    /*
     * AddListen Body
     */
    static string GetAddListenBody()
    {
        string myString = "";
        if (MyTagTrans.ContainsKey("UIClick"))
        {
            foreach (var myTran in MyTagTrans["UIClick"])
            {
                myString += AddTab() + AddTab() + "EventTriggerListener.Get(" + FirstUp(myTran.name) + ").onClick = model.Click" + FirstUp(myTran.name) + ";\n";
            }
        }
        if (MyTagTrans.ContainsKey("UIButtonClick"))
        {
            foreach (var myTran in MyTagTrans["UIButtonClick"])
            {
                myString += AddTab() + AddTab() + "ButtonTrigger.Get(" + FirstUp(myTran.name) + ").AddButtonClick(model.Click" + FirstUp(myTran.name) + ");\n";
            }
        }
        if (MyTagTrans.ContainsKey("UIMenuGrid"))
        {
            foreach (var myTran in MyTagTrans["UIMenuGrid"])
            {
                myString += AddTab() + AddTab() + FirstUp(myTran.name) + ".ClickEvent = model.Click" + FirstUp(myTran.name) + "Menus;\n";
            }
        }

        if (MyTagTrans.ContainsKey("UIScrollGrid"))
        {
            foreach (var myTran in MyTagTrans["UIScrollGrid"])
            {
                myString += AddTab() + AddTab() + FirstUp(myTran.name) + ".ClickItemEvent = model.Click" + FirstUp(myTran.name) + "Items;\n";
            }
        }
        return RemoveEmptyLine(myString);
    }
    //============================================ 符号 =============================================
    static string AddLine()
    {
        return "\n";
    }
    static string AddTab()
    {
        return "    ";
    }
    static string AddLineAndTab()
    {
        return AddLine() + AddTab();
    }
    static string RemoveAddLineAndTable(string str)
    {
        if (str == "")
            return str;
        return str.Substring(0, str.Length - (AddTab().Length + 1));
    }
    static string AddBody(string body, string tab0 = "", string tab1 = "", string tab2 = "", string tab3 = "")
    {
        string info = tab0 + tab1 + tab2 + tab3;
        return "{" + AddLine() + body + AddLine() + info + "}";
    }
    static string RemoveEmptyLine(string myString, int index = 1)
    {
        if (string.IsNullOrEmpty(myString))
            return myString;
        return myString.Substring(0, myString.Length - index);
    }
    //--------------------------------------------------------end----------------------------------------------------
    //获取从begin开始的字符串不包括begin
    public static string GetCache(string myString, string begin)
    {
        if (myString.IndexOf(begin) == -1)
            return myString;
        int i = myString.IndexOf(begin) + begin.Length;
        return myString.Substring(i + 3);
    }
    public static string FirstUp(string name)
    {
        return name.Substring(0, 1).ToUpper() + name.Substring(1);
    }
    //写入文本并且变为UTF8格式
    public static void WriteByUTF8(string myString, string name)
    {
        string filePath = UIScriptPath(TargetName) + TargetName + "/";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        StreamWriter stream = new StreamWriter(filePath + name + ".cs", false, Encoding.UTF8);
        UTF8Encoding utf8 = new UTF8Encoding(); // Create a UTF-8 encoding.
        byte[] bytes = utf8.GetBytes(myString.ToCharArray());
        string EnUserid = utf8.GetString(bytes);
        stream.WriteLine(EnUserid);
        stream.Flush();
        stream.Close();
    }
    public static string AddBodyOfFunc(string funcname, string fromcode, string add)
    {
        fromcode = fromcode.Replace(funcname, "⊕");
        var pices = fromcode.Split('⊕');
        if (pices.Length < 2)
        {
            Debug.LogError($"找不到函数 {funcname}");
            return fromcode;
        }
        int ed = pices[1].IndexOf("}") + 1;

        string body = pices[1].Substring(0, ed - 1) + add + "}";
        string pre = pices[0];
        string after = pices[1].Substring(ed + 1);
        string result = pre + funcname + body + after;
        return result;
    }
}