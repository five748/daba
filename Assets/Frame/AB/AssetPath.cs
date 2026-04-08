using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
public class AssetPath
{
    public static bool IsLoadAsync = false;                                                     //同步加载还是异步加载
    public static string Commom = "common";
    public static AssetBundleManifest AssetDep;                                                 //资源的依赖关系
    public static string DepPath = "nearing";                                                     //依赖关系地址
    public static readonly string EndPath = ".assetbundle";
    private static readonly string VersionName = "version.txt";                       //版本相对路径文件名
                                                                                      //public static readonly string UpURL = "http://180.150.177.134/download/god/";              //资源服务器上传地址
                                                                                      //public static readonly string DownURL = "http://60.205.224.134/assetbundles";       //资源服务器下载地址

    public static string AssetBundlePath
    { //assetbundle资源包保存路径
        get
        {
            return  Application.persistentDataPath + "/asset/";
        }
    }
    public static string CommonItemStrPath = Application.dataPath + "/Resources/commonStr.txt";
    public static string VersionPath = VersionName;
    public static string VersionBundlePath = VersionName;
    public static string PrefabTargePath = "Assets/";
    public static string AssetDataPath = Application.dataPath;
    public static string VersionBundleURL = HotConfig.DownURL + "/" + VersionName;
    public static string BuildFileEndPath = "Resources";                                    //需要打包的资源路径                         
    public static string ExportPath = Application.dataPath.Replace("Assets", "Export/");    //导出路径
    public static string ExportBundlePath(bool isAndorid) {
        if (isAndorid)
        {
            return FrameConfig.ClintFatherPath + "assetbundle/andorid/nearing/";
        }
        else
        {
            return FrameConfig.ClintFatherPath + "assetbundle/ios/nearing/";
        }
    }
    public static string ExportApkPath = ExportPath + "Apk/"; 
    public static string ExportXcodePath = ExportPath + "Xcode/";  

    public static string DepBundlePath = DepPath;                                           //依赖关系路径
    public static string DepBundleURL = HotConfig.DownURL + DepPath;                        //依赖关系URL
    public static string CommmonAltas = "Altas/common";

    public static string PrefabPath = "resources/prefabs/";
    public static string PrefabBundlePath = ModifyFullPath(AssetBundlePath + PrefabPath);
    public static string TexturePath = "resources/Image/";
    public static string TextureBundlePath = ModifyFullPath(AssetBundlePath + TexturePath);
    public static string TextPath = "resources/Data/";
    public static string NewTextPath = "resources/LocalData/";
    public static string TextBundlePath = ModifyFullPath(AssetBundlePath + TextPath);

    //获取从begin开始的字符串包括begin
    public static string GetPathHaveBegin(string myString, string begin)
    {
        int i = myString.IndexOf(begin);
        return myString.Substring(i);
    }
    //获取从begin开始的字符串不包括begin
    public static string CutLowerPath(string myString, string begin, int index = 0)
    {
        begin = begin.ToLower();
        if (myString.IndexOf(begin) == -1)
            return myString;
        int i = myString.IndexOf(begin) + begin.Length + index;
        return myString.Substring(i);
    }
    public static string CutPath(string myString, string begin, int index = 0)
    {
        begin = begin;
        if (myString.IndexOf(begin) == -1)
            return myString;
        int i = myString.IndexOf(begin) + begin.Length + index;
        return myString.Substring(i);
    }
    //从第index个字符开始获取
    public static string GetStringAtIndex(string myString, int index)
    {
        return myString.Substring(index);
    }
    public static string GetFileName(string fullPath)
    {
        string myString = CutLowerPath(fullPath, "Assets", 1);
        return ChangPathEnd(myString, EndPath);
    }
    public static string ChangPathEnd(string path, string end = ".assetbundle")
    {
        if (path.Contains("."))
            return path.Split('.')[0];
        return path;
    }
    public static string RemovePathEnd(string path)
    {
        if (path.Contains("."))
            return path.Split('.')[0];
        return path;
    }
    //根据文件名获取本地资源总路径
    public static string GetFullAssetBundlePath(string path)
    {
        return AssetBundlePath + ModifToCanLoadPath(path);
    }
    //根据文件名获取本地资源www总路径
    public static string GetWWWAssetBundlePath(string path)
    {
        return ModifyFullPath(AssetBundlePath + GetPlatformPath(path));
    }
    //LoadFromFile加载路径
    public static string GetLoadFilePath(string path)
    {
        return Application.streamingAssetsPath + "/asset/" + GetPlatformPath(path);
    }
    //根据文件名获取服务器资源地址
    public static string GetDownLoadURL(string path)
    {
        if (ProssData.Instance.VersionId == 0)
        {
            Debug.LogError(path);
        }
        return HotConfig.DownURL + "/asset/" + path.Replace(@"\", @"/");
        //return HotConfig.DownURL + "/" + ProssData.Instance.ServerChannel + "/"  + ProssData.Instance.VersionId + "/asset/" + path.Replace(@"\", @"/");
    }
    public static string ModifToCanLoadPath(string path)
    {
        return (StringTool.Trim(path)).Replace(@"\", @"/");
    }
    //获取需要打包的文件夹
    public static List<string> GetNeedBuildPath()
    {
        List<string> assetPaths = new List<string>();
        //获取路径下的所有文件
        DirectoryInfo Dir = new DirectoryInfo(Application.dataPath);
        //获取所有子路径
        foreach (DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.Name.EndsWith("Resources") || info.Name.EndsWith("versionBundle"))
                assetPaths.Add(info.FullName);
        }
        return assetPaths;
    }
    //获取该路径下的所有文件路径名
    public static void GetAllPaths(string targetPath, List<string> paths)
    {
        //获取路径下的所有文件
        DirectoryInfo Dir = new DirectoryInfo(targetPath);
        //获取所有子路径
        foreach (DirectoryInfo info in Dir.GetDirectories())
        {
            GetAllPaths(info.FullName, paths);
        }
        //获取所有子文件
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (info.FullName.EndsWith(EndPath) || info.FullName.EndsWith("assetBundle"))
            {
                paths.Add(ModifyFullPath(info.FullName));
            }
        }
    }
    //根据平台修改path
    public static string ModifyFullPath(string path)
    {
        if (ProssData.Instance.platform == Platform.Ios)
        {
            return "file://" + path;
        }
        return path;
    }
    //根据平台获取persistentDataPath
    public static string GetPersistentDataPath()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return ModifyFullPath( Application.persistentDataPath);
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            return ModifyFullPath( Application.persistentDataPath);
        }
        return  Application.persistentDataPath;
    }
    //根据平台获取streamingAssetsPath
    public static string GetStreamingAssetsPath()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return Application.streamingAssetsPath;
        }
        return "file://" + Application.streamingAssetsPath;
    }
    public static string GetStreamingRealPath()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return Application.dataPath + "/Raw"; ;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            return "jar:file://" + Application.dataPath + "!/assets/";
        }
        return Application.dataPath + "/StreamingAssets";
    }
    public static void GetShorDir(string targetPath, System.Action<string> finish)
    {
        targetPath = GetPlatformPath(targetPath);
        DirectoryInfo Dir = new DirectoryInfo(targetPath);
        //获取所有子路径
        foreach (DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.FullName.IndexOf(".git") != -1)
                continue;
            finish(info.Name);
        }
    }
    public static void GetAllFileName(string targetPath, System.Action<string> finish)
    {
        targetPath = GetPlatformPath(targetPath);
        if (!Directory.Exists(targetPath)) {
            return;
        }
        //Debug.Log(targetPath);
        //获取路径下的所有文件
        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(targetPath);
        //获取所有子文件
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (finish != null)
            {
                if (info.FullName.IndexOf(".meta") != -1)
                    continue;
                finish(info.FullName);
            }
        }
        //获取所有子路径
        foreach (System.IO.DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.FullName.IndexOf(".svn") != -1)
                continue;
            if (info.FullName.IndexOf(".meta") != -1)
                continue;
            GetAllFileName(info.FullName, finish);
        }
    }
    public static void GetAllFileNameNoToLower(string targetPath, System.Action<string> finish)
    {
        targetPath = targetPath.Replace(@"\", @"/");
        //Debug.Log(targetPath);
        //获取路径下的所有文件
        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(targetPath);
        //获取所有子路径
        foreach (System.IO.DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.FullName.IndexOf(".svn") != -1)
                continue;
            if (info.FullName.IndexOf(".meta") != -1)
                continue;
            GetAllFileNameNoToLower(info.FullName, finish);
        }
        //获取所有子文件
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (finish != null)
            {
                if (info.FullName.IndexOf(".meta") != -1)
                    continue;
                finish(info.FullName);
            }
        }
    }
    public static void GetAllFileNames(string targetPath, System.Action<string, string> finish)
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
            GetAllFileNames(info.FullName, finish);
        }
        //获取所有子文件
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (finish != null)
            {
                if (info.FullName.IndexOf(".meta") != -1)
                    continue;
                finish(info.FullName, info.Name);
            }
        }
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

    public static void GetAllFileAndDriName(string targetPath, System.Func<string, string> finish)
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
            GetAllFileAndDriName(finish(info.FullName), finish);
        }
        //获取所有子文件
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (finish != null)
            {
                if (info.FullName.IndexOf(".meta") != -1)
                    continue;
                info.Refresh();
                finish(info.FullName);
            }
        }
    }
    public static void GetAllShortFileName(string targetPath, System.Action<string> finish)
    {
        targetPath = GetPlatformPath(targetPath);
        //获取路径下的所有文件
        DirectoryInfo Dir = new DirectoryInfo(targetPath);
        //获取所有子路径
        foreach (DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.FullName.IndexOf(".svn") != -1)
                continue;
            GetAllShortFileName(info.FullName, finish);
        }
        //获取所有子文件
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (info.Name.EndsWith(".meta"))
                continue;
            if (finish != null)
                finish(info.Name);
        }
    }
    public static void GetAllShortDirName(string targetPath, System.Action<string> finish)
    {
        targetPath = GetPlatformPath(targetPath);
        //获取路径下的所有文件
        DirectoryInfo Dir = new DirectoryInfo(targetPath);
        //获取所有子路径
        foreach (DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.FullName.IndexOf(".svn") != -1)
                continue;
            finish(info.Name);
            GetAllShortDirName(info.FullName, finish);
        }
    }
    public static void CheckAllChinesePath(string path)
    {
        GetAllFileName(path, (fullPath) =>
         {
             if (StrHasChinese(fullPath))
             {
                 Debug.Log(fullPath);
             }
         });
    }
    private static bool StrHasChinese(string str)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
    }

    //删除文件夹下的文件
    public static void DelFile(string targePath, string end)
    {
        GetAllFileName(targePath, delegate (string fullPath)
        {
            if (fullPath.IndexOf("asset.manifest") != -1)
                return;
            if (fullPath.EndsWith(end))
                File.Delete(fullPath);
        });
    }
    public static string GetFullLocalPath(string path)
    {
        return ModifyFullPath(GetPlatformPath(path));
    }
    public static void GetFilePathsAndMD5(string targetPath, Dictionary<string, string> versions, string end = "")
    {
        GetAllFileName(targetPath, delegate (string fullName)
        {
            if (end != "")
            {
                if (fullName.IndexOf(end) != -1)
                    return;
            }
            if (fullName.EndsWith(".meta"))
                return;
            string name = CutLowerPath(fullName, "Assets", 1);
            versions.Add(name.Replace(@"\", "/"), GetMD5(fullName));
        });
    }
    public static void GetAllFilePath(string targetPath, List<string> versions, string end = "", string cut = "")
    {
        targetPath = GetPlatformPath(Application.dataPath + "/" + targetPath);
        if (!Directory.Exists(targetPath)) {
            Debug.LogError("找不到:"+ targetPath);
            return;
        }
        GetAllFilePathByFullTar(targetPath, versions, end, cut);
    }
    private static void GetAllFilePathByFullTar(string targetPath, List<string> versions, string end = "", string cut = "")
    {
        GetAllFileName(targetPath, delegate (string fullName)
        {
            if (end != "")
            {
                if (!fullName.EndsWith(end))
                    return;
            }
            if (cut != "")
            {
                if (fullName.IndexOf(cut) != -1)
                    return;
            }
            if (fullName.EndsWith(".meta"))
                return;
            if (fullName.EndsWith(".meta"))
                return;
            string name = CutLowerPath(fullName, "Assets", 1);
            //Debug.Log(name);
            versions.Add(name.Replace(@"\", "/"));
        });
    }

    //获取文件的MD5
    public static string GetMD5(string fileName)
    {
        FileStream file = new FileStream(fileName, FileMode.Open);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(file);
        file.Close();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        return sb.ToString();
    }
    //获取文件的MD5
    public static string GetMD5(byte[] bytes)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(bytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        return sb.ToString();
    }
    //新建文本文件
    public static void NewText(string fullpath)
    {
        StreamWriter sw = File.CreateText(fullpath);
        sw.Close();
    }
    //新建文件
    public static void NewFile(string fileName)
    {
        FileStream stream = File.Create(fileName);
        stream.Flush();
        stream.Close();
    }
  
    public static void WriteText(string path, string str)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        //开始写入
        sw.Write(str);
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
    }
    //写入文本并且变为UTF8格式
    public static void WriteByUTF8(string path, string myString)
    {
        CreateDir(path);
        StreamWriter stream = new StreamWriter(path, false, Encoding.UTF8);
        UTF8Encoding utf8 = new UTF8Encoding(); // Create a UTF-8 encoding.
        byte[] bytes = utf8.GetBytes(myString.ToCharArray());
        string EnUserid = utf8.GetString(bytes);
        stream.WriteLine(EnUserid);
        stream.Flush();
        stream.Close();
    }
    public static string ReadByUTF8(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("找不到文件!");
            return "";
        }
        byte[] buf = ReadText(path);
        UTF8Encoding utf8 = new UTF8Encoding(); // Create a UTF-8 encoding.
        string EnUserid = utf8.GetString(buf);
        //Debug.Log(EnUserid);
        return EnUserid;
    }
    public static byte[] ReadByUTF8Byte(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("找不到文件!");
            return null;
        }
        byte[] buf = ReadText(path);
        UTF8Encoding utf8 = new UTF8Encoding(); // Create a UTF-8 encoding.
        return buf;
    }
    //获取文本
    public static string GetAssetText(AssetBundle ab)
    {
        if (ab == null)
            return "";
        return System.Text.Encoding.UTF8.GetString((ab.LoadAllAssets()[0] as TextAsset).bytes);
    }
    //读取txt
    public static byte[] ReadText(string path)
    {
        //FileStream fs = File.OpenRead(path);
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] data = new byte[fs.Length];
        fs.Read(data, 0, data.Length);
        fs.Flush();
        fs.Close();
        return data;
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
    public static void ReadStringByLine(string fullpath, System.Action<string> StrEvent)
    {
        StreamReader sr = new StreamReader(fullpath, Encoding.Default);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            StrEvent(line);
        }
        //sr.Flush();
        sr.Close();
    }

    //获得图片
    public static Sprite GetAssetSprite(WWW www)
    {
        if (string.IsNullOrEmpty(www.error))
        {
            Sprite[] sprites = www.assetBundle.LoadAllAssets<Sprite>();
            return sprites[0];
        }
        return null;
    }
    //获得预制
    public static GameObject GetAssetPrefab(WWW www)
    {
        //GameObject prefab = new GameObject();
        if (string.IsNullOrEmpty(www.error))
        {
            string[] names = www.assetBundle.GetAllAssetNames();
            //先加载依赖资源
            string[] depPaths = AssetDep.GetAllDependencies(names[0]);
            //prefabs = www.assetBundle.LoadAllAssets<GameObject>();

        }
        return null;
    }
    public void LoadDepAsset(string path, System.Action finish)
    {
        string[] paths = AssetDep.GetAllDependencies(path);
        foreach (string info in paths)
        {

        }
    }
    public static void AddDepToVersion(Dictionary<string, string> fileData)
    {
        fileData.Add(DepPath, "");
    }
    public static string GetPlatformPath(string path)
    {
        //     if(GameTools.OnGamePlatform == GamePlatform.IOS)
        //return path.Replace(@"/", @"\");
        //     if (GameTools.OnGamePlatform == GamePlatform.Android)
        //         return path.Replace(@"\", @"/");
        return path.ToLower().Replace(@"\", @"/");
    }
    public static string GetAndroidPath(string path)
    {
        return path.Replace(@"\", @"/");
    }
    public static string TrimAll(string path)
    {
        return path.Replace(" ", "");
    }
    //修改文件名
    public static string ReplaceStreamFileName(string fileName)
    {
        fileName = GetPlatformPath(fileName);
        return AssetPath.TrimAll(fileName);
    }
    public static string ReplacePersistentFileName(string fileName)
    {
        return fileName.Replace("##", "/");
    }
    //修改文件夹下文件的名字
    public static void ReFileName(string filePath)
    {
        Debug.Log("beginReName");
        //Debug.Log(filePath);
        //获取路径下的所有文件
        DirectoryInfo Dir = new DirectoryInfo(filePath);
        string fileName = "";
        //获取所有子文件
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (info.Name.EndsWith(".meta"))
            {
                File.Delete(info.FullName);
                continue;
            }
            if (info.FullName.IndexOf(@"\") != -1)
            {
                //fileName = GetPlatformPath(info.FullName);
                // Debug.Log(fileName);
                CreateDir(fileName);
                //Debug.Log(fileName);
                //if(fileName.EndsWith(".Dsstone"))
                File.Move(info.FullName, fileName);
            }
        }
    }

    //根据文件路径创建文件夹(假设路径是/a/b/c.prefab, 建立文件夹/a/b/)
    public static void CreateDir(string path)
    {
        string info = GetDirectoryByFileName(path);
        if (!Directory.Exists(info))
        {
            Debug.Log("createFile:" + path);
            Directory.CreateDirectory(info);
        }
    }
    //获取文件的前一个文件夹路径
    public static string GetDirectoryByFileName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    //保存byte[]到文件中
    public static void SaveBuffToFile(string path, byte[] buff)
    {
        if (buff == null)
        {
            Debug.Log(path);
            return;
        }
        CreateDir(path);
        Stream stream = new FileStream(path, FileMode.Create,
        FileAccess.Write, FileShare.None);
        stream.Write(buff, 0, buff.Length);
        stream.Flush();
        stream.Dispose();
    }
    //读取文件到byte[]
    public static byte[] ReadFileToBuff(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("文件不存在, 请检查路径! path::" + path);
            return null;
        }
        Stream stream = new FileStream(path, FileMode.Open,
        FileAccess.Read, FileShare.None);
        byte[] buff = new byte[stream.Length];
        stream.Read(buff, 0, buff.Length);
        stream.Flush();
        stream.Dispose();
        return buff;
    }
    //保存对象都文件中
    public static void SaveClassToFile<T>(string path, T t)
    {
        CreateDir(path);
        Stream stream = new FileStream(path, FileMode.Create,
        FileAccess.Read, FileShare.None);
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, t);
        stream.Flush();
        stream.Dispose();
    }
    //从文件中新建对象
    public static T CreateClassFromFile<T>(string path)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path, FileMode.Open,
        FileAccess.Read, FileShare.Read);
        return (T)formatter.Deserialize(stream);
    }
    //修改文件名
    public static void NewFileName(string oldName, string newName)
    {
        if (File.Exists(newName))
            File.Delete(newName);
        else
            CreateDir(newName);
        File.Move(oldName, newName);
    }
    public static string FirstUp(string name)
    {
        return name.Substring(0, 1).ToUpper() + name.Substring(1);
    }
    public static string OpenCSV(string filePath)
    {
        FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
        string str = "";
        string strLine = "";
        while ((strLine = sr.ReadLine()) != null)
        {
            str += strLine + "\n";
        }
        fs.Dispose();
        fs.Close();
        sr.Dispose();
        sr.Close();
        return str;
    }
    public static void CopyDirectory(string sourcePath, string destinationPath, string filt = "", bool overwrite = true)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        if (!Directory.Exists(destinationPath))
        {
            Directory.CreateDirectory(destinationPath);
        }
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = destinationPath + "/" + fsi.Name;
            //UnityEngine.Debug.Log(destName);
            if (fsi is System.IO.FileInfo)          //如果是文件，复制文件
            {
                if (filt != "" && fsi.Name.Contains(filt)) {
                    continue;
                }
                if (!fsi.FullName.EndsWith(".meta"))
                {
                    if (!overwrite)
                    {
                        if (File.Exists(fsi.FullName)) {
                            File.Copy(fsi.FullName, destName, true);
                        }
                    }
                    else {
                        File.Copy(fsi.FullName, destName, true);
                    }
                }
            }
            else  //如果是文件夹，新建文件夹，递归
            {
                Directory.CreateDirectory(destName);
                CopyDirectory(fsi.FullName, destName, filt, overwrite);
            }
        }
    }
    public static bool IsNumeric(string value)
    {
        return Regex.IsMatch(value, @" ^[+-]?\d*[.]?\d*$");
    }
    public static bool IsInt(string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*$");

    }
    public static bool IsChinese(string value)
    {
        return Regex.IsMatch(value, @"[\u4e00-\u9fbb]");
    }
}

