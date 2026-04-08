using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class AltasTool {
    [MenuItem("程序工具/设置图集")]
    public static void SetAltas() {
        //SetImageImporter(Application.dataPath + "/SceneRes/texture/5.png", spine_platform_settings_android, spine_platform_settings_ios);
        //return;
        SetAllAltas();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    [MenuItem("程序工具/设置图片")]
    public static void SetOtherTexture() {
        SetSpineImage();
        SetResImgeOther();
        SetSceneImage();
        SetResourseImage();
        SetEffectImage();
    }
    [MenuItem("程序工具/设置图集和图片")]
    public static void SetAltasAndImage()
    {
        //SetImageImporter(Application.dataPath + "/SceneRes/texture/5.png", spine_platform_settings_android, spine_platform_settings_ios);
        //return;
        SetAllAltas();
        SetSpineImage();
        SetResImgeOther();
        SetSceneImage();
        SetEffectImage();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public static void SetAllAltas() {
        Dictionary<string, List<string>> depNums = GetAllImageDepPrefab();
        var artPaths = GetArtAllImage();
        //SetArtChangeImage();
        if (Directory.Exists(_atlasPath)) {
            Directory.Delete(_atlasPath, true);
            Directory.CreateDirectory(_atlasPath);
            AssetDatabase.Refresh();
        }
        foreach (var item in depNums)
        {
            if(!item.Key.Contains("Res/Image/All"))
            {
                continue;
            }
            SetImage(item.Key, item.Value, artPaths);
        }
    }
    public static void SetImage(string path, List<string> prefabNames, Dictionary<string, string> artPaths) {
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter == null) {
            return;
        }
        //Debug.LogError(path);
        textureImporter.mipmapEnabled = false;
        string artModify = "";
        if (artPaths.ContainsKey(path))
        {
            artModify = HotConfig.ArtABFlag; 
        }
        string packTag = "";
        string abName = "";
        if (prefabNames.Count == 1)
        {
            packTag = artModify + prefabNames[0].Replace(".prefab", "").ToLower();
            abName = ("Altas/" + artModify + prefabNames[0].Replace(".prefab", "")).ToLower();
        }
        else
        {
            packTag = artModify + ("common").ToLower();
            abName = ("Altas/" + artModify + "common").ToLower();
        }
        textureImporter.spritePackingTag = "";
        textureImporter.assetBundleName = abName;
        AltasCreate.AddOneImage(abName, packTag,  AssetDatabase.LoadAssetAtPath<Sprite>(path));
        if (artModify != "") {
            textureImporter.assetBundleVariant = "c";
            SetArtImage(artPaths[path], packTag, abName, "fchinese");
            //if(Config.IsHaveEnglish)
            //    SetArtImage(artPaths[path].Replace("FChinese", "Englis"), packTag, abName, "english");
        }
        TextureImporterPlatformSettings settingAndroid = textureImporter.GetPlatformTextureSettings("Android");
        settingAndroid.maxTextureSize = 1024;
        settingAndroid.overridden = true;
        settingAndroid.format = TextureImporterFormat.ASTC_8x8;  //设置格式
        settingAndroid.allowsAlphaSplitting = true;
        settingAndroid.compressionQuality = 100;
        textureImporter.SetPlatformTextureSettings(settingAndroid);

        TextureImporterPlatformSettings settingIOS = textureImporter.GetPlatformTextureSettings("iPhone");
        settingIOS.maxTextureSize = 1024;
        settingIOS.overridden = true;
        settingIOS.format = TextureImporterFormat.ASTC_6x6;  //设置格式    
        textureImporter.SetPlatformTextureSettings(settingIOS);

        TextureImporterPlatformSettings settingWebGL = textureImporter.GetPlatformTextureSettings("WebGL");
        settingWebGL.maxTextureSize = 1024;
        settingWebGL.overridden = true;
        settingWebGL.format = TextureImporterFormat.ASTC_8x8;  //设置格式
        settingWebGL.allowsAlphaSplitting = true;
        settingWebGL.compressionQuality = 100;
        textureImporter.SetPlatformTextureSettings(settingWebGL);
        
        textureImporter.SaveAndReimport();
    }
    private static string _atlasPath {
        get { 
            return Application.dataPath + "/Res/Image/Altas/";
        }
    } 
    private static string _texturePath = "Assets/Texture";
    private static void SetArtImage(string path, string spritePackingTag, string abName, string variant) {
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter == null)
        {
            Debug.LogError("找不到:" + path);
            return;
        }
        textureImporter.spritePackingTag = spritePackingTag;
        textureImporter.assetBundleName = abName;
        textureImporter.assetBundleVariant = variant;

        TextureImporterPlatformSettings settingAndroid = textureImporter.GetPlatformTextureSettings("Android");
        settingAndroid.maxTextureSize = 1024;
        settingAndroid.overridden = true;
        settingAndroid.format = TextureImporterFormat.ETC_RGB4;  //设置格式
        settingAndroid.allowsAlphaSplitting = true;
        settingAndroid.compressionQuality = 100;
        textureImporter.SetPlatformTextureSettings(settingAndroid);

        TextureImporterPlatformSettings settingIOS = textureImporter.GetPlatformTextureSettings("iPhone");
        settingIOS.maxTextureSize = 1024;
        settingIOS.overridden = true;
        settingIOS.format = TextureImporterFormat.ASTC_4x4;  //设置格式    
        textureImporter.SetPlatformTextureSettings(settingIOS);
        textureImporter.SaveAndReimport();
    }
    private static Dictionary<string, string> GetArtAllImage() {
        Dictionary<string, string> strs = new Dictionary<string, string>();
        return strs;
        string gameFChinesePath = Application.dataPath + "/Res/FChinese/UIImage/";
        gameFChinesePath.GetAllFileName(null, (file) => {
            if (!file.Name.Contains(".png"))
            {
                return;
            }
            strs.Add("Assets/Res/Image/All/" + file.Name, "Assets/Res/FChinese/UIImage/" + file.Name);
        });
        return strs;
    }
    private static void SetArtChangeImage() {
        string gameFChinesePath = Application.dataPath + "/Res/FChinese/ChangeImage/";
        Dictionary<string, string> strs = new Dictionary<string, string>();
        gameFChinesePath.GetAllFileName(null, (file) => {
            if (!file.Name.Contains(".png"))
            {
                return;
            }
            var fileName = AssetPath.GetPlatformPath(file.FullName).GetSpitLastDirAndFile('/').Replace(".png", "");
            var key = "Res/Image/ChangeImage/" + fileName;
            strs.Add(key, "Res/FChinese/ChangeImage/" + fileName);
        });
        HotConfig.ArtWritePath.WriteByDicStr(strs);
    }
    //设置公用资源
    static Dictionary<string, List<string>> GetAllImageDepPrefab() {
        Dictionary<string, List<string>> depNums = new Dictionary<string, List<string>>();
        (Application.dataPath + "/" + FrameConfig.PrefabPath).GetAllFileName(null, (file) => {
            if(!file.Name.Contains(".prefab")) {
                return;
            }
            string[] depPaths = AssetDatabase.GetDependencies("Assets/" + FrameConfig.PrefabPath + "UIPrefab/" + file.Name);
            foreach(var depPath in depPaths)
            {
                if(!depNums.ContainsKey(depPath))
                {
                    depNums.Add(depPath, new List<string>());
                }
                depNums[depPath].Add(file.Name);
            }
        });
        return depNums;
    }
    public static void SetSpineImage() {
        //UnityEngine.Debug.LogError(AssetPath.AssetDataPath + "/Res/Spine/");
        AssetPath.GetAllFileName(AssetPath.AssetDataPath + "/Res/Spine/", (path) =>
        {
            SetImageImporter(path, spine_platform_settings_android, spine_platform_settings_ios, platform_settings_webgl);
        });
    }
    public static void SetResImgeOther() {
        AssetPath.GetAllFileName(Application.dataPath + "/" + FrameConfig.ImagePath, (path) =>
        {
            if(path.Contains("/All/") || path.Contains("/all/") || path.Contains("\\All\\") || path.Contains("\\all\\"))
            {
                return;
            }
            SetImageImporter(path, platform_settings_android, platform_settings_ios, platform_settings_webgl);
        });
    }
    public static void SetSceneImage() {
        return;
        AssetPath.GetAllFileName(Application.dataPath + "/SceneRes/", (path) =>
        {
            SetImageImporter(path, scene_platform_settings_android, scene_platform_settings_ios, platform_settings_webgl);
        });
    }
    public static void SetResourseImage()
    {
        AssetPath.GetAllFileName(Application.dataPath + "/Resources/Frame/", (path) =>
        {
            SetImageImporter(path, scene_platform_settings_android, scene_platform_settings_ios, platform_settings_webgl);
        });
    }
    public static void SetEffectImage() {
        AssetPath.GetAllFileName(Application.dataPath + "/Res/effect/", (path) =>
        {
            SetImageImporter(path, spine_platform_settings_android, spine_platform_settings_ios, platform_settings_webgl);
        });
    }
    static TextureImporterPlatformSettings platform_settings_android = new TextureImporterPlatformSettings() {
        overridden = true,
        //name = "iPhone",
        name = "Android",
        format = TextureImporterFormat.ASTC_6x6,
    };
    static TextureImporterPlatformSettings platform_settings_webgl = new TextureImporterPlatformSettings()
    {
        overridden = true,
        //name = "iPhone",
        maxTextureSize = 1024,
        name = "WebGL",
        format = TextureImporterFormat.ASTC_8x8,
    };
    static TextureImporterPlatformSettings platform_settings_ios = new TextureImporterPlatformSettings()
    {
        overridden = true,
        name = "iPhone",
        format = TextureImporterFormat.ASTC_6x6,
    };
    static TextureImporterPlatformSettings spine_platform_settings_android = new TextureImporterPlatformSettings() {
        overridden = true,
        //name = "iPhone",
        name = "Android",
        format = TextureImporterFormat.ASTC_6x6
    };
    static TextureImporterPlatformSettings spine_platform_settings_ios = new TextureImporterPlatformSettings() {
        overridden = true,
        name = "iPhone",
        format = TextureImporterFormat.ASTC_6x6,
    };
    static TextureImporterPlatformSettings scene_platform_settings_android = new TextureImporterPlatformSettings()
    {
        overridden = true,
        //name = "iPhone",
        name = "Android",
        format = TextureImporterFormat.ASTC_6x6,
    };
    static TextureImporterPlatformSettings scene_platform_settings_ios = new TextureImporterPlatformSettings()
    {
        overridden = true,
        name = "iPhone",
        format = TextureImporterFormat.ASTC_6x6,
    };

    static void SetImageImporter(string path, TextureImporterPlatformSettings settingAndroid, TextureImporterPlatformSettings settingIOS, TextureImporterPlatformSettings settingWebGL) {
        if(!(path.EndsWith(".png") || !path.EndsWith(".jpg"))) return;
        path = path.GetUnityAssetPathLow();
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if(textureImporter == null)
        {
            //Debug.LogError(path);
            return;
        }
        //if (textureImporter.mipmapEnabled == false && textureImporter.GetAutomaticFormat("Android") == settingAndroid.format)
        //{
        //    return;
        //}
        //Debug.LogError(path + ":" + textureImporter.GetAutomaticFormat("Android") + ":" + settingAndroid.format);
        textureImporter.mipmapEnabled = false;
        textureImporter.SetPlatformTextureSettings(settingAndroid);
        textureImporter.SetPlatformTextureSettings(settingIOS);
        textureImporter.SetPlatformTextureSettings(settingWebGL);
        textureImporter.SaveAndReimport();
    }
    private static void ChangePrefabImagePath(List<string> files, string sourcePath, string targetPath, bool isCopyMeta = true) {
        sourcePath = sourcePath.GetUnityAssetPath();
        targetPath = targetPath.GetUnityAssetPath();
        if(files.Count > 0)
        {
            for(int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Replace(Application.dataPath, "Assets");
            }
        }
        for(int i = 0; i < files.Count; i++)
        {
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(files[i]);
            if(go == null) continue;
            // Debug.LogError(files[i]);
            UnityEngine.UI.Image[] imgs = go.GetComponentsInChildren<UnityEngine.UI.Image>(true);
            if(imgs == null)
            {
                continue;
            }
            for(int j = 0; j < imgs.Length; j++)
            {
                Sprite d = imgs[j].sprite;
                if(d == null) continue;
                string oldPath = AssetDatabase.GetAssetPath(d);
                string newPath = targetPath + d.name + ".png";
                if(!oldPath.Contains("/UIImage/"))
                {
                    continue;
                }
                if(oldPath.Contains("UIMain"))
                {
                    Debug.Log(oldPath + ":" + newPath);
                }
                if(isCopyMeta)
                {

                    bool isSucc = CopyImageMetaSet(oldPath.GetFullPathByUnityPath(), newPath.GetFullPathByUnityPath());
                    if(!isSucc)
                    {
                        continue;
                    }
                }
                Sprite spr = AssetDatabase.LoadAssetAtPath<Sprite>(newPath);
                imgs[j].sprite = spr;
            }
            EditorUtility.SetDirty(go);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private static bool CopyImageMetaSet(string a, string b) {
        a = a.GetUnityAssetPath();
        b = b.GetUnityAssetPath();
        TextureImporter textureImporterA = (TextureImporter)AssetImporter.GetAtPath(a);
        TextureImporter textureImporterB = (TextureImporter)AssetImporter.GetAtPath(b);

        if(textureImporterA && textureImporterB)
        {
            textureImporterB.SetPlatformTextureSettings(textureImporterA.GetPlatformTextureSettings("iPhone"));
            textureImporterB.SetPlatformTextureSettings(textureImporterA.GetPlatformTextureSettings("Android"));
            EditorUtility.SetDirty(textureImporterB);
            return true;
        }
        if(!textureImporterA)
        {
            Debug.LogError("找不到文件:" + a);
            return false;
        }
        if(!textureImporterB)
        {
            Debug.LogError("找不到文件:" + b);
            return false;
        }
        return true;
    }
}
