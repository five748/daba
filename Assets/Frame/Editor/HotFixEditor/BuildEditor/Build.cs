using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using DateTime = System.DateTime;
using System.Text;
using System.Text.RegularExpressions;
using GameVersionSpace;

public class Build : EditorWindow
{
    [MenuItem("程序工具/热跟打包/显示项目中中文名")]
    static public void ClearBundleFileNameZH()
    {
        AssetPath.GetAllFileName(AssetPath.AssetDataPath, (path) =>
        {
            if (path.Contains(@"effect\need"))
            {
                if (path.isHaveChinese())
                {
                    Debug.Log(path);
                }
                return;
            }
            if (path.isHaveChinese())
            {
                Debug.LogError(path);
            }
        });
    }

    [MenuItem("程序工具/热跟打包/清理AssetBundle名字")]
    static public void ClearBundleFileName()
    {
        AssetImporter assetImporter;
        AssetPath.GetAllFileName(AssetPath.AssetDataPath, (path) =>
        {
            if (!path.EndsWith(".cs"))
            {
                path = AssetPath.CutLowerPath(AssetPath.ModifToCanLoadPath(path), "Assets/");
                assetImporter = AssetImporter.GetAtPath(AssetPath.PrefabTargePath + path);

                if (assetImporter)
                {
                    SetAssetBundleName(assetImporter, string.Empty);
                }
            }
        });
        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.Refresh();
    }
    static public void ClearBundleResourcesName()
    {
        AssetImporter assetImporter;
        AssetPath.GetAllFileName(AssetPath.AssetDataPath  + "/Resources/", (path) =>
        {
            if (!path.EndsWith(".cs"))
            {
                path = AssetPath.CutLowerPath(AssetPath.ModifToCanLoadPath(path), "Assets/");
                assetImporter = AssetImporter.GetAtPath(AssetPath.PrefabTargePath + path);

                if (assetImporter)
                {
                    SetAssetBundleName(assetImporter, string.Empty);
                }
            }
        });
        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.Refresh();
    }
    [MenuItem("程序工具/热跟打包/清理Table8和Skill8名字")]
    static public void ClearBundleTable8AndSkill8()
    {
        AssetImporter assetImporter;
        AssetPath.GetAllFileName(Application.dataPath + "/Res/table8", (path) =>
        {
            if (!path.EndsWith(".cs"))
            {
                path = AssetPath.CutLowerPath(AssetPath.ModifToCanLoadPath(path), "Assets/");
                assetImporter = AssetImporter.GetAtPath(AssetPath.PrefabTargePath + path);

                if (assetImporter)
                {
                    SetAssetBundleName(assetImporter, string.Empty);
                }
            }
        });
        AssetPath.GetAllFileName(Application.dataPath + "/Res/Skill8", (path) =>
        {
            if (!path.EndsWith(".cs"))
            {
                path = AssetPath.CutLowerPath(AssetPath.ModifToCanLoadPath(path), "Assets/");
                assetImporter = AssetImporter.GetAtPath(AssetPath.PrefabTargePath + path);

                if (assetImporter)
                {
                    SetAssetBundleName(assetImporter, string.Empty);
                }
            }
        });
        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.Refresh();
        SendDing("Clear bundle name finish");
    }

    //[MenuItem("程序工具/热跟打包/设置图集名称")]
    static public void ChangeSpriteAltas()
    {
        if (!EditorUtility.DisplayDialog("", "是否设置图集名称?", "ok", "no"))
            return;
        AssetImporter assetImporter;
        AssetPath.GetAllFileName(AssetPath.AssetDataPath + "/Res/Image/", (path) =>
        {
            if (path.EndsWith(".png"))
            {
                //Debug.Log(path);
                path = AssetPath.CutLowerPath(AssetPath.ModifToCanLoadPath(path), "Assets/");
                assetImporter = AssetImporter.GetAtPath(AssetPath.PrefabTargePath + path);
                if (assetImporter)
                {
                    TextureImporter textureImporter = (TextureImporter)assetImporter;
                    textureImporter.spritePackingTag = "";
                    textureImporter.SaveAndReimport();
                }
            }
        });
        AssetDatabase.Refresh();
    }

    //[MenuItem("策划工具/清理本地缓存")]
    static void ClearCacheData1()
    {
        if (Directory.Exists(Application.persistentDataPath))
            Directory.Delete(Application.persistentDataPath, true);
        Directory.CreateDirectory(Application.persistentDataPath);
        AssetDatabase.Refresh();

    }
    [MenuItem("程序工具/热跟打包/ShowAllChinesePath")]
    static void ShowAllChinesePath()
    {
        if (!EditorUtility.DisplayDialog("", "是否显示所有中文路径?", "ok", "no"))
            return;
        AssetPath.CheckAllChinesePath(Application.dataPath);
    }
    static Dictionary<string, int> Commom = new Dictionary<string, int>();
    private static void BuildInit()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
    }

    //[MenuItem("程序工具/热跟打包/SetAssetBundleName")]
    static void SetAssetBundleNameChinese()
    {
        SetAssetBundleName(true);
    }
    public static void SetAssetBundleName(bool isWarn)
    {
        if (isWarn)
        {
            if (!EditorUtility.DisplayDialog("", "是否设置所有AssetBundle名字?", "ok", "no"))
                return;

        }
        commonItemStrs.Clear();
        BuildInit();
        //获取所有需要打包公共资源的文件路径
        List<string> paths = GetAllNeedBuildPath();
        //设置公用资源
        SetCommonName(paths);
        SetBundleName(paths);
        SetDirSameName();
        SetSpineSameName();
        ClearBundleResourcesName();
        AssetDatabase.Refresh();
    }
    static void SetDirSameName()
    {
        List<string> dirPath = new List<string>();
        (Application.dataPath + "/Res/Image/Frame/").GetAllFileName((dir) => {
            dirPath.Add("/Res/Image/Frame/" + dir.Name);
        });
        //dirPath.Add("/Res/Image/UICommon/");
        for (int i = 0; i < dirPath.Count; i++)
        {
            AssetPath.GetAllFileName(Application.dataPath + dirPath[i], (path) =>
            {
                if (path.IndexOf(".meta") != -1)
                {
                    return;
                }
                path = AssetPath.GetPlatformPath(path);
                path = AssetPath.CutLowerPath(path, dirPath[i]);
                AssetImporter assetImporter = AssetImporter.GetAtPath("Assets" + dirPath[i] + path);
                SetAssetBundleName(assetImporter, dirPath[i].Substring(1));
            });
        }
    }
    public static void SetSpineSameName() {
        List<string> dirPath = new List<string>();
        (Application.dataPath + "/Res/Spine/").GetAllFileName((dir) => {
            dirPath.Add("/Res/Spine/" + dir.Name);
        });
        //dirPath.Add("/Res/Image/UICommon/");
        for (int i = 0; i < dirPath.Count; i++)
        {
            AssetPath.GetAllFileName(Application.dataPath + dirPath[i], (path) =>
            {
                if (path.IndexOf(".meta") != -1)
                {
                    return;
                }
                path = AssetPath.GetPlatformPath(path);
                path = AssetPath.CutLowerPath(path, dirPath[i]);
                AssetImporter assetImporter = AssetImporter.GetAtPath("Assets" + dirPath[i] + path);
                //Debug.LogError(dirPath[i].Substring(1));
                SetAssetBundleName(assetImporter, dirPath[i].Substring(1));
            });
        }
    }
    public static void SetDirName() { 
        
    }
    static void ClearBunleName()
    {
        string dataPath = Application.dataPath;

        List<string> directory_list = new List<string>()
        {
            @"\Res\Spine\UIAnimator\dengru"
        };

        foreach (string dir in directory_list)
        {
            BundleNameHandler.SetDirectoryAssetBundleName(dataPath + dir, true);
        }

        List<string> file_list = new List<string>()
        {
            @"\Res\Image\UIBigBg\NoHaveAlpha\LoginBg.png",
            @"\Res\ImageKR\UIBigBg\NoHaveAlpha\LoginBg.png",
            @"\Res\Image\ArtImage\chinese\logo.png",
            @"\Res\ImageKR\ArtImage\chinese\logo.png"
        };

        foreach (string file in file_list)
        {
            BundleNameHandler.SetFileBundleName(dataPath + file, true);
        }
    }
    //[MenuItem("程序工具/热跟打包/BuildByUnSetName")]
    static void BuildByUnSetName()
    {
        //BuildByUnSetName(true);
    }
    public static void BuildByUnSetName(int resVersionId, bool isWarn, bool isAndorid = true)
    {
        if (isWarn)
        {
            if (!EditorUtility.DisplayDialog("", "是否使用设置好的名字打包所有AssetBundle?", "ok", "no"))
                return;
        }
        BuildInit();
        BuildAssetBundle(resVersionId, isAndorid);
    }

    //[MenuItem("程序工具/热跟打包/SetNameAndBuild")]
    static void SetNameAndBuildChinese()
    {
        if (!EditorUtility.DisplayDialog("", "是否重新设置所有AssetBundle名字和打包?", "ok", "no"))
            return;
        SetAssetBundleName(false);
        //BuildByUnSetName(false);
    }
    private static List<string> GetAllNeedBuildPath()
    {
        List<string> paths = new List<string>();
        GetAllAudio(paths);
        GetAllPrefab(paths);
        GetUITextrue(paths);
        GetAllData(paths);
        GetAllBytes(paths);
        GetSpine(paths);
        return paths;
    }
    private static void GetAllAnimation(List<string> fileData)
    {
        AssetPath.GetAllFilePath(HotConfig.UIAltasPath, fileData, ".prefab");
    }
    private static void GetAllAltas(List<string> fileData)
    {
        //AssetPath.GetAllFilePath(HotConfig.UIAltasPath, fileData, ".prefab");
    }
    private static void GetAllAudio(List<string> fileData)
    {
        AssetPath.GetAllFilePath(HotConfig.AudioPath, fileData);
    }
    private static void GetAllPrefab(List<string> fileData)
    {
        AssetPath.GetAllFilePath(HotConfig.PrefabPath, fileData, ".prefab");
        AssetPath.GetAllFilePath(HotConfig.EffectNewPrefabPath, fileData, ".prefab");
    }
    private static void GetUITextrue(List<string> fileData)
    {
        AssetPath.GetAllFilePath(HotConfig.UITexturePath, fileData);
        //AssetPath.GetAllFilePath(HotConfig.UIFChinesePath, fileData);
        //if(Config.IsHaveEnglish)
        //    AssetPath.GetAllFilePath(HotConfig.UIEnglishPath, fileData);
    }
    private static void GetAllData(List<string> fileData)
    {
        AssetPath.GetAllFilePath(HotConfig.TabelPath, fileData, ".txt");
    }
    private static void GetAllBytes(List<string> fileData)
    {
        AssetPath.GetAllFilePath(HotConfig.SceneBin, fileData, ".bytes");
        AssetPath.GetAllFilePath(HotConfig.SkillBin, fileData, ".bytes");
        AssetPath.GetAllFilePath(HotConfig.StoryBin, fileData, ".bytes");
        AssetPath.GetAllFilePath(HotConfig.HotScrBin, fileData, ".bytes");
        AssetPath.GetAllFilePath(HotConfig.TabelPath, fileData, ".bytes");
    }
    private static void GetSpine(List<string> fileData)
    {
        AssetPath.GetAllFilePath(HotConfig.SpinePrefab, fileData);
    }
    static void ReName()
    {
        AssetPath.ReFileName(Application.dataPath + "/test");
    }
    //打成assetbundle
    //[MenuItem("程序工具/热跟打包/测试bunlde(直接打)")]
    static void BuildAssetBundle(int resVersionId, bool isAndroid = true)
    {
        BuildAssetBundleOptions bulidOptions = BuildAssetBundleOptions.None;
        BuildTarget bulidTarget = BuildTarget.StandaloneWindows;
        string bundle_path = AssetPath.ExportBundlePath(isAndroid);
#if DY || WX
        bulidOptions = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
        bulidTarget = BuildTarget.WebGL;
        BuildPipeline.BuildAssetBundles(bundle_path, BuildAssetBundleOptions.AppendHashToAssetBundleName | BuildAssetBundleOptions.ChunkBasedCompression
            | UnityEditor.BuildAssetBundleOptions.DisableWriteTypeTree | BuildAssetBundleOptions.None, BuildTarget.WebGL);
#else

        if (isAndroid)
        {
            bulidOptions = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
            bulidTarget = BuildTarget.Android;
        }
        else
        {
            Debug.LogError("Ios打包");
            bulidOptions = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
            bulidTarget = BuildTarget.iOS;
        }
        BuildPipeline.BuildAssetBundles(bundle_path, bulidOptions, bulidTarget);
#endif

        WriteVersion(resVersionId, isAndroid); //写入版本配置文件
        AssetDatabase.Refresh();
    }
    static long GetCurTime()
    {
        System.TimeSpan st = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        return (System.Convert.ToInt64(st.TotalSeconds));
    }
    public class VersionTime {
        public string md5;
        public System.DateTime time;
        public string path;
        public long size;
        public int sortId;
    }
    [MenuItem("程序工具/热跟打包/WriteVersion")]
    static void WriteVersion(int resVersionId = 24, bool isAndroid = true)
    {
        string bundle_path = AssetPath.ExportBundlePath(isAndroid);
        string[] files = Directory.GetFiles(bundle_path, "*", SearchOption.AllDirectories);
#if WX || DY
        ClearSameBundle(files, bundle_path);
#endif
        Dictionary<string, VersionTime> pathAndMD5 = new Dictionary<string, VersionTime>();
        pathAndMD5.Add(GameVersionSpace.VersionConst.idKey, new VersionTime() { md5 = resVersionId.ToString()});
        pathAndMD5.Add(GameVersionSpace.VersionConst.timeKey, new VersionTime() { md5 = GetCurTime().ToString() });
        files = Directory.GetFiles(bundle_path, "*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            if (file.EndsWith(".manifest", System.StringComparison.Ordinal)) continue;
            if (file.EndsWith(".meta", System.StringComparison.Ordinal)) continue;
            var key = GetBundlePath(file.Replace(bundle_path, ""));
#if WX || DY
            if (key == "nearing")
            {
                continue;
            }
#endif
            FileInfo fileInfo = new FileInfo(file);
            var versionData = new VersionTime();
            versionData.md5 = AssetPath.GetMD5(file);
            versionData.time = fileInfo.CreationTime;
            versionData.size = fileInfo.Length;
            versionData.path = file;
            if (pathAndMD5.ContainsKey(key))
            {
                var oldData = pathAndMD5[key];
                if (versionData.time > oldData.time)
                {
                    pathAndMD5[key] = versionData;
                    File.Delete(oldData.path);
                }
                else {
                    fileInfo.Delete();
                }
            }
            else {
                pathAndMD5.Add(key, versionData);
            }
        }
        StringBuilder mySB = new StringBuilder();
        foreach (var item in pathAndMD5)
        {
            mySB.Append(string.Format("{0},{1},{2}\n", item.Key, item.Value.md5, item.Value.size));
        }
        //PlayerSettings.bundleVersion, PlayerSettings.Android.bundleVersionCode
        mySB.Append(DateTime.Now.ToString("yyyy年MM月dd日--HH:mm:ss"));

        string version_path = bundle_path + $"version{resVersionId}.txt";
        FileStream stream = new FileStream(version_path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(mySB.ToString());
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
        SortVersionText(resVersionId);
        Debug.Log("版本文件生成完成:" + version_path);

    }
    static Dictionary<string, VersionTime> nameAndHashNames;
    static Dictionary<string, VersionTime> hashnameAndHashNames;
    static void LoadDep() {
        string bundle_path = AssetPath.ExportBundlePath(true);
        var ab = AssetBundle.LoadFromFile(bundle_path + "nearing");
        var depAb = ab.LoadAsset("AssetBundleManifest");
        AssetPath.AssetDep = depAb as AssetBundleManifest;
        ab.Unload(false);
    }
    //[MenuItem("程序工具/热跟打包/SortVersion")]
    static void SortVersionText(int versionId) {
        //var versionId = 24;
        nameAndHashNames = new Dictionary<string, VersionTime>();
        hashnameAndHashNames = new Dictionary<string, VersionTime>();
        string bundle_path = AssetPath.ExportBundlePath(true) + $"version{versionId}.txt";
        bundle_path.ReadStringByLine((line) => {
            var strs = line.Split(',');
            if (strs.Length == 1) {
                return;
            }
            var path = strs[0];
            var newPath = CutABPath(path);
            var data = new VersionTime();
            data.path = path;
            data.md5 = strs[1];
            data.size = long.Parse(strs[2]);
            nameAndHashNames.Add(newPath, data);
            hashnameAndHashNames.Add(path, data);
        });
        LoadDep();
        ChangeByTable();
        StringBuilder mySB = new StringBuilder();
        hashnameAndHashNames = hashnameAndHashNames.SortByBeginIndex((a, b) => {
            return a.Value.sortId.CompareTo(b.Value.sortId);
        }, 3);
        foreach (var item in hashnameAndHashNames)
        {
            mySB.Append(string.Format("{0},{1},{2},{3}\n", item.Key, item.Value.md5, item.Value.size, item.Value.sortId));
        }
        //PlayerSettings.bundleVersion, PlayerSettings.Android.bundleVersionCode
        mySB.Append(DateTime.Now.ToString("yyyy年MM月dd日--HH:mm:ss"));

        string version_path = bundle_path;
        FileStream stream = new FileStream(version_path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(mySB.ToString());
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
    }
    static string CutABPath(string path)
    {
        if (!path.Contains("_"))
        {
            return path;
        }
        var strs = path.Split('_');
        var str = "";
        for (int i = 0; i < strs.Length - 1; i++)
        {
            str += strs[i] + "_";
        }
        return str.CutLast();
    }
    static void SetPrefabDepPathSort(string path, int index) {
        var newPath = FrameConfig.UIPrefabPath + path;
        newPath = AssetPath.GetPlatformPath(newPath);
        if (!nameAndHashNames.ContainsKey(newPath)) {
            return;
        }
        var newName = nameAndHashNames[newPath].path;
        if (index == -1) {
            hashnameAndHashNames.Remove(newName);
            return;
        }
        string[] depPaths = AssetPath.AssetDep.GetAllDependencies(newName);
        hashnameAndHashNames[newName].sortId = index;
        foreach (var item in depPaths)
        {
            hashnameAndHashNames[item].sortId = index;
        }
    }
    static void SetChangeImagePathSort(string path, int index)
    {
        path = ImageImport.ChangeImageName(path, true);
        var newPath = FrameConfig.ImagePath + "ChangeImage/" + path;
        newPath = AssetPath.GetPlatformPath(newPath);
        if (!nameAndHashNames.ContainsKey(newPath))
        {
            return;
        }
        var newName = nameAndHashNames[newPath].path;
        hashnameAndHashNames[newName].sortId = index;
    }
    static void SetFramePathSort(string path, int index)
    {
        var newPath = FrameConfig.ImagePath + "Frame/" + path;
        newPath = AssetPath.GetPlatformPath(newPath);
        if (!nameAndHashNames.ContainsKey(newPath))
        {
            return;
        }
        var newName = nameAndHashNames[newPath].path;
        hashnameAndHashNames[newName].sortId = index;
    }
    static void ChangeByTable() {
        foreach (var item in TableCache.Instance.prefabUITable)
        {
            SetPrefabDepPathSort(item.Value.name, item.Value.index);
        }
        foreach (var item in TableCache.Instance.changeImgaTable)
        {
            SetChangeImagePathSort(item.Value.name, item.Value.index);
        }
        foreach (var item in TableCache.Instance.changeframeTable)
        {
            SetFramePathSort(item.Value.name, item.Value.index);
        }
    }

    public static void ClearSameBundle(string[] files, string null_path) {
        List<string> delName = new List<string>();
        Dictionary<string, VersionTime> lst = new Dictionary<string, VersionTime>();
        foreach (string file in files)
        {
            if (file.Contains("nearing/nearing_"))
            {
                File.Delete(file);
                delName.Add(file);
            }
        }
        foreach (string file in files)
        {
            if (file.Contains("version.txt")) {
                continue;
            }
            if (delName.Contains(file)) {
                continue;
            }
           
            if (file.EndsWith(".manifest", System.StringComparison.Ordinal)) continue;
            if (file.EndsWith(".meta", System.StringComparison.Ordinal)) continue;
            FileInfo fileInfo = new FileInfo(file);
            var key = GetBundlePath(file.Replace(null_path, "")).GetSpitUpLast('_');
            var versionData = new VersionTime();
            versionData.md5 = AssetPath.GetMD5(file);
            versionData.time = fileInfo.CreationTime;
            versionData.path = file;
            if (lst.ContainsKey(key))
            {
                var oldData = lst[key];
                if (versionData.time > oldData.time)
                {
                    lst[key] = versionData;
                    File.Delete(oldData.path);
                    //Debug.LogError(oldData.path);
                }
                else
                {
                    fileInfo.Delete();
                    //Debug.LogError(file);
                }
            }
            else {
                lst.Add(key, versionData);
            }
            if (file.Contains("nearing/nearing"))
            {
                SetNearingName(file);
            }
        }
        
    }
    private static void SetNearingName(string path) {
        //Debug.LogError(path);
        if (path.Contains(".manifest"))
        {
            File.Move(path, path.Replace(".manifest", "") + "_" + AssetPath.GetMD5(path) + ".manifest");
        }
        else {
            File.Copy(path, path + "_" + AssetPath.GetMD5(path), true);
        }
     
    }
    public static string GetBundlePath(string str)
    {
        return str.ToLower().Replace("\\", "/");
    }

    static void GetVersion(string fullPath, Dictionary<string, string> versions)
    {
        AssetPath.GetAllFileName(fullPath, delegate (string fullName)
        {
            if (fullName.EndsWith(".meta") || fullName.EndsWith(".manifest"))
                return;
            string name = AssetPath.CutLowerPath(fullName, @"\StreamingAssets\asset\");
            name = AssetPath.CutLowerPath(name, "/StreamingAssets/asset/");
            versions.Add(name, AssetPath.GetMD5(fullName));
        });
    }

    //设置公用资源
    static void SetCommonName(List<string> paths)
    {
        Commom = new Dictionary<string, int>();
        Dictionary<string, int> depNum = new Dictionary<string, int>(); //依赖计数
        AssetImporter assetImporter = null;
        foreach (string path in paths)
        {
            if (path.IndexOf(".meta") != -1)
                continue;
            string[] depPaths = AssetDatabase.GetDependencies(AssetPath.PrefabTargePath + path);
            var deps2 = EditorUtility.CollectDependencies(GetObjectsByPaths(depPaths));
            SetDepNum(deps2, depNum, path);
        }
        //int index = 0;
        foreach (var dic in depNum)
        {
            //Debug.Log(dic.Key);
            if (dic.Value <= 1)
                continue;
            if (dic.Key == "")
                continue;
            if (dic.Key.IndexOf(".meta") != -1)
                continue;
            if (dic.Key.IndexOf(".cs") != -1)
                continue;
            if (dic.Key.IndexOf("itemimage") != -1)
                continue;
            if (dic.Key.IndexOf("UIImage") != -1)
                continue;
            if (dic.Key.IndexOf("Spine") != -1)
                continue;
            SetCommom(assetImporter, dic.Key, dic.Value);
        }
    }
    static void SetDepNum(Object[] deps, Dictionary<string, int> depNum, string path)
    {
        for (int i = 0; i < deps.Length; i++)
        {
            string str = AssetDatabase.GetAssetPath(deps[i]);

            Object go = deps[i];
            if (depNum.ContainsKey(str))
                depNum[str]++;
            else
                depNum.Add(str, 1);
            if (path.IndexOf("ItemImage") != -1)//非依赖
            {
                continue;
            }
            if (str.IndexOf("ItemImage") != -1)//依赖
            {
                str = CutEndAsset(str).Substring(7);
                str = AssetPath.GetPlatformPath(str).ToLower();
                if (!commonItemStrs.Contains(str))
                    commonItemStrs.Add(str);
            }
        }
    }
    static void SetCommom(AssetImporter assetImporter, string path, int num)
    {
        if (path.IndexOf("spine") != -1)
            return;
        assetImporter = AssetImporter.GetAtPath(path);
        if (assetImporter == null)
            return;
        if (path.IndexOf(".meta") == -1 && path.IndexOf(".cs") == -1)
        {
            if (path.IndexOf("Spine") == -1)
            {
                string newname = AssetPath.ReplaceStreamFileName("common/" + CutEndAsset(path).ToLower().Replace("assets", "")).Replace("//", "/");
                if (path.Contains("/all/"))
                {
                    return;
                }
                if (path.Contains("/altas/"))
                {
                    return;
                }
                SetAssetBundleName(assetImporter, newname);
            }

        }

        Commom.Add(path, num);
    }
    static void SetBundleName(List<string> paths)
    {

        for (int i = 0; i < paths.Count; i++)
        {
            if (paths[i].IndexOf(".meta") == -1 && paths[i].IndexOf(".cs") == -1)
                SetAssetBundleName(AssetPath.GetPlatformPath(paths[i]));
        }
    }
    static string abName = "";
    static void SetAssetBundleName(string path)
    {
        if (path.Replace(@"\", "/").Contains("/UIImage/") || path.Replace(@"\", "/").Contains("/BigBg/"))
        {
            return;
        }
        AssetImporter assetImporter = AssetImporter.GetAtPath(AssetPath.PrefabTargePath + path);
        if (assetImporter)
        {
            if (path.IndexOf(".meta") == -1 && path.IndexOf(".cs") == -1)
            {
                string newname = AssetPath.ReplaceStreamFileName(CutEndAsset(path));
                SetAssetBundleName(assetImporter, newname);
            }
        }
        string[] depPaths = AssetDatabase.GetDependencies(AssetPath.GetPlatformPath(AssetPath.PrefabTargePath + path));
        //var deps2 = EditorUtility.CollectDependencies(GetObjectsByPaths(depPaths));
        if (path.Contains("UIHeroList") || path.Contains("uiherolist"))
        {
            Debug.LogError(path);
        }
        for (int i = 0; i < depPaths.Length; i++)
        {
            string str = depPaths[i];
            if (!Commom.ContainsKey(str))
            {
                if (str.IndexOf(".meta") == -1 && str.IndexOf(".cs") == -1)
                {
                    assetImporter = AssetImporter.GetAtPath(str);
                    if (assetImporter)
                    {
                        string newname = AssetPath.ReplaceStreamFileName(CutEndAsset(str));
                        SetAssetBundleName(assetImporter, newname);
                    }
                }
            }
        }
    }
    public static string CutEndAsset(string str)
    {
        string[] strs = str.Split('.');
        str = "";
        for (int i = 0; i < strs.Length - 1; i++)
        {
            str += "." + strs[i];
        }
        if (str == "")
            return str;
        return str.Substring(1);
    }
    private static List<string> commonItemStrs = new List<string>();
    static Object[] GetObjectsByPaths(string[] paths)
    {
        Object[] targets = new Object[paths.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = AssetDatabase.LoadMainAssetAtPath(paths[i]);
        }
        return targets;
    }

    static void SetAssetBundleName(AssetImporter assetImporter, string name)
    {
        if (name.Contains("unityengine.ui"))
        {
            return;
        }
        if (name.Contains("res/image/all/"))
        {
            return;
        }
        if (name.Contains("res/image/altas/"))
        {
            return;
        }
        name = name.Replace("assets/", "");
        if (assetImporter.assetBundleName == name) return;
        try
        {
            assetImporter.assetBundleName = name;
        }
        catch
        {
            Debug.LogError(name);
        }
    }

    static string token = "07b56bd562d6b17a7e31c9ee29a7246e5b1fd354b3e1a19112eba19fdd302590";
    public static void SendDing(string content)
    {
        //DingDingHelper.SendPost(token, content, "");
    }
    #region 在一些文件的类名前边加个[Preserve]标签，以免被裁剪

    [MenuItem("Assets/程序工具/添加[Preserve]标签", false, 1)]
    static void AddPreserveLabel()
    {
        string selectedFolderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!string.IsNullOrEmpty(selectedFolderPath) && Directory.Exists(selectedFolderPath))
        {
            string[] filePaths = Directory.GetFiles(selectedFolderPath, "*.cs", SearchOption.AllDirectories);
            foreach (string filePath in filePaths)
            {
                if (Path.GetFileName(filePath).StartsWith("MA_"))
                {
                    string fileContent = File.ReadAllText(filePath);
                    string modifiedContent = AddNamespaceAndPreserveAttribute(fileContent);
                    File.WriteAllText(filePath, modifiedContent);
                }
            }

            Debug.Log("Finished processing files in folder: " + selectedFolderPath);
        }
        else
        {
            Debug.LogError("Please select a valid folder.");
        }
    }

    private static string AddNamespaceAndPreserveAttribute(string fileContent)
    {
        // 添加命名空间
        string namespaceLine = "using UnityEngine.Scripting;";
        if (!fileContent.Contains(namespaceLine))
        {
            fileContent = namespaceLine + "\n" + fileContent;
        }

        // 添加 [preserve] 标签
        string preserveAttributeLine = "[Preserve]";
        if (!fileContent.Contains(preserveAttributeLine))
        {
            // 使用正则表达式匹配所有类定义，并在每个类定义前添加 [preserve] 标签
            var pattern = @"(\b(public|private|protected|internal)\b\s+)?\bclass\b\s+\w+\s*(\s*:\s*\w+\s*)?\{";
            MatchCollection matches = Regex.Matches(fileContent, pattern);
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                // 获取匹配到的类定义的起始位置，并在该位置之前一行添加 [preserve] 标签
                int index = match.Index + (preserveAttributeLine.Length + 1) * i;
                fileContent = fileContent.Insert(index, preserveAttributeLine + "\n");

            }
        }

        return fileContent;
    }

    #endregion
}