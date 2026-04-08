using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;
public class ImageImport : MonoBehaviour
{
    [MenuItem("美术工具/提取图片")]
    static void ImportImage()
    {
        CopyAniDir();
    }
    public static void ImportNewImage(GameObject parent)
    {
        BranchTool.LoadAssetToUnity(FrameConfig.ArtPath + "gitupdate.bat", "", () =>
        {
            fileList = new List<FileSystemInfo>();
            CopyAniDir();
            UnityEngine.Debug.Log("加载最新的图片");
            fileList = fileList.OrderByDescending(r => r.LastWriteTime).ToList();
            if (fileList.Count > 0)
            {
                fileList = fileList.GetRange(0, fileList.Count > 15 ? 15 : fileList.Count);
            }
            for (int i = 0; i < fileList.Count; i++)
            {
                var item = fileList[i];
                var name = ChangeImageName(item.Name, true);
                UIEditorTool.Create("All", name, item.Name.Split('#')[0], parent, i);
            }
            fileList = null;
        });
    }
    private static List<FileSystemInfo> fileList = null;
    //记录最新文件
    private static void PushFile(FileSystemInfo f)
    {
        if (fileList != null)
        {
            fileList.Add(f);
        }
    }
    public static void CopyAniDir()
    {
        string gamePath = Application.dataPath + "/Res/Image/";
        string framePath = Application.dataPath + "/Res/Image/Frame/";
        if (!ProssData.Instance.FrameImageUseRes){
            framePath = Application.dataPath + "/Resources/Frame/";
        }
        string artPath = FrameConfig.ArtPath + "/Auto/";
        string artPath1 = FrameConfig.ArtPath + "/Image/";
        string artPath2 = FrameConfig.ArtPath + "/Image/Frame/";
        CopyAniDirectory(artPath.Replace("/", "\\"), gamePath.Replace("/", "\\"));
        CopyAniDirectory(artPath1.Replace("/", "\\"), gamePath.Replace("/", "\\"));
        CopyAniDirectory(artPath2.Replace("/", "\\"), framePath, false, false);
        CopyFChinese();
        //if (Config.IsHaveEnglish)
        //    CopyEnglish();
        AssetDatabase.Refresh();
        if (!ProssData.Instance.FrameImageUseRes)
        {
            ChangeSprite("/Resources/Frame/");
        }
        ChangeSprite("/Res/Image/");
        ChangeSprite("/Res/FChinese/");
        //if (Config.IsHaveEnglish)
        //    ChangeSprite("/Res/English/");
    }
    //繁体
    private static void CopyFChinese() {
        string artFChinesePath = FrameConfig.ArtPath + "/FChinese_繁体中文/";
        if (!Directory.Exists(artFChinesePath)) {
            return;
        }
        string gameFChinesePath = Application.dataPath + "/Res/FChinese/";
        CopyAniDirectory(artFChinesePath.Replace("/", "\\"), gameFChinesePath.Replace("/", "\\"), true);
    }
    //英文
    private static void CopyEnglish()
    {
        string artFChinesePath = FrameConfig.ArtPath + "/English_英文/";
        string gameFChinesePath = Application.dataPath + "/Res/English/";
        CopyAniDirectory(artFChinesePath.Replace("/", "\\"), gameFChinesePath.Replace("/", "\\"), true);
    }
    public static void CopyAniDirectory(string srcPath, string destPath, bool isLoadUIImage = false, bool isChangeName = true)
    {
        if (!Directory.Exists(srcPath)) {
            Debug.Log("找不到:"+ srcPath);
            return;
        }
        DirectoryInfo dir = new DirectoryInfo(srcPath);
        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
        foreach (FileSystemInfo i in fileinfo)
        {
            string outName = destPath + "\\" + ChangeImageName(i.Name, isChangeName);
            if (!isLoadUIImage) {
                if (outName.Contains("效果图") || outName.Contains("UIImage"))
                {
                    continue;
                }
            }
            if (i is DirectoryInfo)     //判断是否文件夹
            {
                if (!Directory.Exists(outName))
                {
                    Directory.CreateDirectory(outName);   //目标目录下不存在此文件夹即创建子文件夹
                }
                CopyAniDirectory(i.FullName, outName, isLoadUIImage);    //递归调用复制子文件夹
            }
            else
            {
                if (i.Name == "demo.png")
                {
                    continue;
                }
                if (i.Name.Contains(".png") || i.Name.Contains(".jpg"))
                {
                    PushFile(i);
                    try
                    {
                        File.Copy(i.FullName, outName, true);     //不是文件夹即复制文件，true表示可以覆盖同名文件
                    }
                    catch (Exception e) {
                        UnityEngine.Debug.LogError(e);
                    }
                }
            }
        }
    }


    public static string ChangeImageName(string fileName, bool isChangeName)
    {
        if (!isChangeName) {
            return fileName;
        }
        bool isPng = fileName.Contains(".png");
        if (fileName.Contains("."))  //包含点的为图片，否则为文件夹
        {
            if (fileName.Contains(".png") || fileName.Contains(".jpg"))
            {
                string[] strs = null;
                if(fileName.Contains("#"))
                {
                    strs = fileName.Split('#');
                }
                else {
                    if(fileName.Contains("_"))
                    {
                        strs = fileName.Split('_');
                    }
                    if(fileName.Contains("-"))
                    {
                        strs = fileName.Split('-');
                    }
                }
                if(strs == null) {
                    //UnityEngine.Debug.Log(fileName);
                    return fileName;
                }
                foreach(var str in strs)
                {
                    if(!str.isHaveChinese()) {
                        if(str.Contains('.'))
                            return str;
                        else {
                            if(isPng)
                            {
                                return str + ".png";
                            }
                            else {
                                return str + ".jpg";
                            }
                        }
                    }
                }
                UnityEngine.Debug.Log("图片名字错误:" + fileName);
                return fileName;
            }
            else
            {
                return fileName;
            }
        }
        else
        {
            return fileName.Split('_')[0];
        }
    }
    static void ChangeSprite(string opath)
    {
        string targetPath = Application.dataPath + opath;
        if (!Directory.Exists(targetPath))
        {
            return;
        }
        targetPath.GetAllFileName(null, (path) =>
        {
            if (!path.FullName.EndsWith(".png"))
            {
                return;
            }
            var pathName = path.FullName.Replace(@"\", @"/").Replace(targetPath, "");
            AssetImporter assetImporter = AssetImporter.GetAtPath("Assets" + opath + pathName);
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            if (textureImporter == null)
            {
                UnityEngine.Debug.LogError(pathName);
            }
            if (textureImporter.textureType != TextureImporterType.Sprite)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(assetImporter));
            }
        });
    }
    //比较美术图片文件夹获取需要删除的图片
    //[MenuItem("美术工具/删除无用图片")]
    public static void DelUnUseImage()
    {
        string artAllPath = FrameConfig.ArtPath + "Auto/";
        string artPath = FrameConfig.ArtPath + "Image/";
        string gamePath = Application.dataPath + "/Res/Image/";
        List<string> outPath = new List<string>();
        List<string> apaths = new List<string>();
        List<string> cpahts = new List<string>();
        artAllPath.GetAllFileName(null, file => {
            
            outPath.Add(file.FullName.Replace("\\", "/").Replace(artAllPath, ""));
        });
        artPath.GetAllFileName(null, file => {
            //UnityEngine.Debug.LogError(file.FullName.Replace("\\", "/")+ ":" + artPath);
            outPath.Add(file.FullName.Replace("\\", "/").Replace(artPath, ""));
        });
        gamePath.GetAllFileName(null, file => {
            if(file.FullName.Replace("\\", "/").Contains("Image/Scene/") || file.FullName.Contains("image/scene/")) {
                return;
            }
            cpahts.Add(file.FullName.Replace("\\", "/").Replace(gamePath, ""));
        });
        foreach (var item in outPath)
        {
            var strs = item.Split('/');
            string path = "";
            foreach(var str in strs)
            {
                if(str.Contains('.'))
                {
                    path += ChangeImageName(str, true);
                }
                else {
                    path += str.Split('_')[0];
                }
                path += "/";
            }
            path = path.CutLast();
            //UnityEngine.Debug.LogError(path);
            apaths.Add(path);
        }
        foreach (var item in cpahts)
        {
            if (!apaths.Contains(item))
            {
                if(item.IndexOf(".git") == -1) {
                    //UnityEngine.Debug.LogError(gamePath + item);
                    File.Delete(gamePath + item);
                }
            }
        }
        AssetDatabase.Refresh();
    }
}
