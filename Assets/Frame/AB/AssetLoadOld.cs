using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections;
using UnityEngine.Networking;
using GameVersionSpace;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class AssetLoadOld:Single<AssetLoadOld>
{
    public void LoadStreamAssetText(string path, System.Action<string> callback) {
       MonoTool.Instance.StartCoroutine(DownUnityStreamAssetRequest(path, callback));
    }
    IEnumerator DownUnityStreamAssetRequest(string path, System.Action<string> callback) {
        using(UnityWebRequest uwr = UnityWebRequest.Get(path))
        {
            yield return uwr.SendWebRequest();
            if(uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError("path" + path);
            }
            if(callback != null)
            {
                callback(uwr.downloadHandler.text);
            }
        }
    }
    public void LoadTriggerPrefab(string path, System.Action<GameObject> finish) {
#if UNITY_EDITOR
        finish(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path));
#endif
    }
    public void LoadBasePrefab(string path, System.Action<GameObject> finish) {
        LoadPrefabAsset(path, path, ".prefab", finish);
    }
    public void LoadPrefab(string path, System.Action<GameObject> finish) {
        LoadPrefabAsset(FrameConfig.PrefabPath + path, path, ".prefab", finish);
    }
    public void LoadPrefabResources(string path, System.Action<GameObject> finish)
    {
        finish(Resources.Load<GameObject>(path));
    }
    public void LoadFont(string path, System.Action<Font> finish)
    {
        LoadAsset("Res/font/" + path, ".TTF", (go => {
            finish(go as Font);
        }));
    }
    public GameObject LoadPrefabQuick(string path, Transform father = null) {
        GameObject target = null;
        LoadPrefabAsset(FrameConfig.PrefabPath + path, path, ".prefab", (go) => {
            if(father != null)
            {
                target = GameObject.Instantiate(go, father) as GameObject;
            }
            else {
                target = GameObject.Instantiate(go) as GameObject;
            }
        });
        return target;
    }
    public void LoadUIPrefab(string path, System.Action<GameObject> finish) {
        LoadPrefabAsset(FrameConfig.UIPrefabPath + path, path, ".prefab", finish);
    }
    public static void LoadTextEditor(string path, System.Action<string> finish) {
#if UNITY_EDITOR
        var go = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/" + FrameConfig.TablePath + path + ".txt");
        finish(System.Text.Encoding.UTF8.GetString((go as TextAsset).bytes));
#endif
    }
    public void LoadText(string path, System.Action<string> finish, bool isLoadEditor = false) {
        if (isLoadEditor) {
            LoadTextEditor(path, finish);
            return;
        }
        LoadTextAsset(FrameConfig.TablePath + path, ".txt", (go) =>
        {
            if (go == null) {
                Debug.Log(FrameConfig.TablePath + path);
                finish("");
                return;
            }
            finish(System.Text.Encoding.UTF8.GetString((go as TextAsset).bytes));
        });
    }
    public void LoadBytes(string path, System.Action<byte[]> finish)
    {
        LoadTextAsset(path, ".bytes", (go) =>
        {
            if (go != null)
            {
                finish((go as TextAsset).bytes);
            }
            else {
                Debug.LogError("找不到:" + path);
            }
        });
    }
    public void LoadTableText(string path, System.Action<string> finish, bool isUseEditor = false) {
        LoadTabInRes(path, finish);
    }
    private void LoadTabInRes(string path, System.Action<string> finish) {
       var go = Resources.Load("table/" + path);
       finish(System.Text.Encoding.UTF8.GetString((go as TextAsset).bytes));
    }
    public void LoadTableTextSplit(string path, System.Action<string[][]> finish)
    {
        LoadAsset(FrameConfig.TablePath + path, ".txt", (go) =>
        {
            if (go == null)
            {
                finish(null);
                return;
            }
            var str = System.Text.Encoding.UTF8.GetString((go as TextAsset).bytes);
            str = str.Replace("⋚\r\n", "⋚");
            string[] lines = str.Split('⋚');
            string[][] datas = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                datas[i] = lines[i].Split('↕');
            }
            finish(datas);
        });
    }
    public void LoadImage(Transform tran, string path, System.Action<Sprite> finish, bool isCommom = false) {
        //tran.SetActive(false);
        LoadImageAsset(HotMgr.Instance.ModifyLanguage(FrameConfig.ImagePath + "ChangeImage/" + path), ".png", (sprite) => {
            finish(sprite);
            //tran.SetActive(true);
            //GameProcess.Instance.AddLoadOverTran(tran);
        });
    }
    public void LoadImageFromOrigin(string path, System.Action<Sprite> finish)
    {
        LoadImageAsset(FrameConfig.ImagePath + path, ".png", finish);
    }
    public void LoadRawImage(Transform tran, string path, System.Action<Texture> finish, bool isCommom = false) {
        //tran.SetActive(false);
        LoadRawImageAsset(FrameConfig.ImagePath + "ChangeImage/" + path, ".jpg", (texture) => {
            finish(texture);
            //GameProcess.Instance.AddLoadOverTran(tran);
        });
    }
    public void LoadTextureImage(Transform tran, string path, System.Action<Texture> finish, bool isCommom = false) {
        tran.SetActive(false);
        LoadRawImageAsset(path, ".png", (texture) =>{
            finish(texture);
            //GameProcess.Instance.AddLoadOverTran(tran);
        });
    }
    public Coroutine LoadDirSprite(Transform tran, string dir, System.Action<Sprite[]> callback) {
        //tran.SetActive(false);
        return LoadDirSpriteAsset(FrameConfig.ImagePath + dir, (sprites) => {
            callback(sprites);
            //GameProcess.Instance.AddLoadOverTran(tran);
        });
    }
    public void LoadAudio(string path, System.Action<AudioClip> callback) {
        LoadAudioAsset(FrameConfig.MusicPath + "sound/" + path, ".mp3", (clip) =>
        {
            callback(clip);
        });
    }
    public void LoadBaseAudio(string path, System.Action<AudioClip> callback)
    {
        LoadAudioAsset(FrameConfig.MusicPath + path, ".mp3", (clip) =>
        {
            callback(clip);
        });
    }
    public void LoadBgAudio(string path, System.Action<AudioClip> callback) {
        LoadAudioAsset(FrameConfig.MusicPath + "bg/"  + path, ".mp3", (clip) =>
        {
            callback(clip);
        }, true);
    }
    public void LoadMinMusic(string path, System.Action<AudioClip> callback) {
        LoadAudioAsset(FrameConfig.MusicPath+ "hero/" + path, ".mp3", (clip) =>
        {
            if(clip == null)
                Debug.Log(FrameConfig.MusicPath + "hero/" + path + ":" + clip);
            callback(clip);
        }, true);
    }
    public void LoadMinOnMusic(string path, System.Action<AudioClip> callback)
    {
        LoadAudioAsset(FrameConfig.MusicPath + "onhero/" + path, ".mp3", (clip) =>
        {
            if (clip == null)
                Debug.Log(FrameConfig.MusicPath + "onhero/" + path + ":" + clip);
            callback(clip);
        }, true);
    }
    public void LoadMinLiHuiOnMusic(string path, System.Action<AudioClip> callback)
    {
        LoadAudioAsset(FrameConfig.MusicPath + "lihuihero/" + path, ".mp3", (clip) =>
        {
            if (clip == null)
                Debug.Log(FrameConfig.MusicPath + "lihuihero/" + path + ":" + clip);
            callback(clip);
        }, true);
    }
    public void LoadMinGetHeroOnMusic(string path, System.Action<AudioClip> callback)
    {
        LoadAudioAsset(FrameConfig.MusicPath + "gethero/" + path, ".mp3", (clip) =>
        {
            if (clip == null)
                Debug.Log(FrameConfig.MusicPath + "gethero/" + path + ":" + clip);
            callback(clip);
        }, true);
    }
    private Coroutine LoadDirSpriteAsset(string path, System.Action<Sprite[]> finish) {
        if(ProssData.Instance.IsOpenHot)
        {
            LoadDirSpritesInAsset(path, finish);
        }
#if UNITY_EDITOR
        if (finish != null)
        {
            finish(LoadSprites(path));
        }
#else
        LoadDirSpritesInAsset(path, finish);
#endif
        return null;
    }
    public void LoadPrefabAsset(string path, string prefabName, string end, System.Action<GameObject> finish) {

        if(ProssData.Instance.IsOpenHot)
        {
            LoadInAssetPrefab(path, prefabName, finish);
            return;
        }
#if UNITY_EDITOR
        if (finish != null)
        {
            var go = LoadInResPrefab("Assets/" + path + end);
            if (go == null) {
               Debug.Log("Assets/" + path + end);
            }
            finish(go);
        }
#else
        LoadInAssetPrefab(path, prefabName, finish);
#endif
    }
    private void LoadAudioAsset(string path, string end, System.Action<AudioClip> finish, bool isAnysc = false) {

        if(ProssData.Instance.IsOpenHot)
        {
            LoadInAssetAudio(path.Replace(" ", ""), finish, isAnysc);
            return;
        }
#if UNITY_EDITOR
        if (finish != null)
        {
            //Debug.LogError(("Assets/" + path + end));
            finish((AudioClip)(LoadInResAudioClip("Assets/" + path + end)));
        }
#else
        LoadInAssetAudio(path, finish, isAnysc);
#endif
    }
    public void LoadImageAsset(string path, string end, System.Action<Sprite> finish) {

        if(ProssData.Instance.IsOpenHot)
        {
            LoadInAssetSprite(path, finish);
            return;
        }
#if UNITY_EDITOR
        if (finish != null)
        {
            finish((Sprite)(LoadInResImage("Assets/" + path + end)));
        }
#else
        LoadInAssetSprite(path, finish);
#endif
    }
    private void LoadRawImageAsset(string path, string end, System.Action<Texture> finish) {

        if(ProssData.Instance.IsOpenHot)
        {
            LoadInAssetRawSprite(path, finish);
            return;
        }
#if UNITY_EDITOR
        if (finish != null)
        {
            finish((Texture)(LoadInResRawImage("Assets/" + path + end)));
        }
#else
        LoadInAssetRawSprite(path, finish);
#endif
    }
    //加载资源
    public void LoadAsset(string path, string end, System.Action<UnityEngine.Object> finish) {
        if(ProssData.Instance.IsOpenHot)
        {
            LoadInAsset(path, finish);
            return;
        }
#if UNITY_EDITOR
        if (finish != null)
        {
            finish(LoadInRes("Assets/" + path + end));
        }
#else
        LoadInAsset(path, finish);
#endif
    }
    public void LoadAssetResources(string path, System.Action<UnityEngine.Object> finish) {
        finish(Resources.Load(path));
    }
    public void LoadAssetEditor(string path, string end, System.Action<UnityEngine.Object> finish)
    {
#if UNITY_EDITOR
        if (finish != null)
        {
            finish(LoadInRes("Assets/" + path + end));
        }
#endif
    }
    //加载资源
    public void LoadTextAsset(string path, string end, System.Action<UnityEngine.Object> finish) {
        if(ProssData.Instance.IsOpenHot)
        {
            LoadTextInAsset(path, finish);
            return;
        }

#if UNITY_EDITOR
        if (finish != null)
        {
            finish((UnityEngine.Object)(LoadInRes("Assets/" + path + end)));
        }
#else
        LoadTextInAsset(path, finish);
#endif
    }
#if UNITY_EDITOR
    private UnityEngine.Object LoadInRes(string path)
    {
        //Debug.LogError(path);
        var go = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        if(go == null) {
            Debug.Log("找不到文件:" + path);
        }
        return go;
    }
#endif
#if UNITY_EDITOR
    private UnityEngine.Object LoadInResImage(string path)
    {
        //print(path);
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }
    private UnityEngine.Object LoadInResRawImage(string path)
    {
        //print(path);
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture>(path);
    }
    private UnityEngine.Object LoadInResAudioClip(string path)
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    }
    private GameObject LoadInResPrefab(string path)
    {
        //print(path);
        //Debug.LogError(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path));
        //print("over");
        return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
    }
#endif
#if UNITY_EDITOR
    private Sprite[] LoadSprites(string path)
    {
        List<Sprite> sprites = new List<Sprite>();
        AssetPath.GetAllShortFileName(Application.dataPath + "/" + path, (pathname) =>
        {
            sprites.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/" + path + "/" + pathname));
        });
        return sprites.ToArray();
    }
#endif
    private void LoadInAsset(string path, System.Action<UnityEngine.Object> finish) {
        path = AssetPath.GetPlatformPath(path);
        DownLoadAsset(path, (go) =>
        {
            if(go == null)
            {
                Debug.Log(path);
            }
            if(finish != null)
                finish(go);
            //go = null;
        });
    }
    private void LoadTextInAsset(string path, System.Action<UnityEngine.Object> finish) {
        path = AssetPath.GetPlatformPath(path);
        DownLoadTextAsset(path, (go) =>
        {
            if(go == null)
            {
                Debug.Log(path);
            }
            if(finish != null)
                finish(go);
            //go = null;
        });
    }
    private void LoadInAssetSprite(string path, System.Action<Sprite> finish) {
        path = AssetPath.GetPlatformPath(path);
        DownLoadSpriteAsset(path, (go) =>
        {
            if(finish != null)
            {
                finish(go);
            }
        });
    }
    private void LoadInAssetRawSprite(string path, System.Action<Texture> finish) {
        path = AssetPath.GetPlatformPath(path);
        DownLoadRawAsset(path, (go) =>
        {
            if(finish != null)
            {
                finish(go);
            }
        });
    }
    private void LoadInAssetAudio(string path, System.Action<AudioClip> finish, bool isAnysc = false) {
        path = AssetPath.GetPlatformPath(path);
        DownLoadAudioAsset(path, (go) =>
        {
            if(finish != null)
                finish(go);
            //go = null;
        }, isAnysc);
    }
    private void LoadInAssetPrefab(string path, string prefabName, System.Action<GameObject> finish) {
        path = AssetPath.GetPlatformPath(path);
        LoadDepAsset(path, prefabName, () => {
            DownLoadPrefabAsset(prefabName, path, (go) =>
            {
                if (finish != null)
                    finish(go);
                //go = null;
            });
        });
    }
    private void LoadDirSpritesInAsset(string path, System.Action<Sprite[]> callback) {
        path = AssetPath.GetPlatformPath(path);
        DownLoadSpriteDirAsset(path, (go) =>
        {
            if(callback != null)
                callback(go);
            //go = null;
        });
    }
    //-----------------------------------------------------------------------------------------------
    //
    //  调用区
    //
    //-----------------------------------------------------------------------------------------------
    public async void DownLoadAsset(string name, System.Action<UnityEngine.Object> finish)
    {
        var obj = await HotMgr.Instance.LoadAB(name, (ab, abName) => {
            return ab.LoadAsset(abName);
        });
        finish(obj as UnityEngine.Object);
    }
    public async void DownLoadTextAsset(string name, System.Action<TextAsset> finish)
    {
        var obj = await HotMgr.Instance.LoadAB(name, (ab, abName) => {
            return ab.LoadAsset(abName);
        });
        finish(obj as TextAsset);
    }
    public async void DownLoadSpriteAsset(string name, System.Action<Sprite> finish)
    {
        var obj = await HotMgr.Instance.LoadAB(name, (ab, abName) => {
            //var all = ab.LoadAllAssets();
            return ab.LoadAsset<Sprite>(abName);
        });
        finish(obj as Sprite);
    }
    public async void DownLoadRawAsset(string name, System.Action<Texture> finish)
    {
        var obj = await HotMgr.Instance.LoadAB(name, (ab, abName) => {
            return ab.LoadAsset<Texture>(abName);
        });
        finish(obj as Texture);
    }
    public async void DownLoadAudioAsset(string name, System.Action<AudioClip> finish, bool isAnysc)
    {
        var obj = await HotMgr.Instance.LoadAB(name, (ab, abName) => {
#if (DY || WX) && UNITY_EDITOR
            return null;
#endif
            return ab.LoadAsset<AudioClip>(abName);
        });
        finish(obj as AudioClip);
    }
    public async void DownLoadPrefabAsset(string prefabName, string name, System.Action<GameObject> finish)
    {
        HotMgr.Instance.AddPrefabAbNames(prefabName, name);
        var obj = await HotMgr.Instance.LoadAB(name, (ab, abName) => {
            return ab.LoadAsset<GameObject>(abName);
        });
        finish(obj as GameObject);
    }
    public async void DownLoadSpriteDirAsset(string dirname, System.Action<Sprite[]> finish, bool isAnync = false)
    {
        var obj = await HotMgr.Instance.LoadAB(dirname, (ab, abName) => {
            return ab.LoadAllAssets<Sprite>();
        });
        finish(obj as Sprite[]);
    }
    public async void LoadDepAsset(string name, string prefabName, System.Action finish)
    {
        var newName = HotMgr.Instance.AbDatas[name].basePath;
        string[] paths = AssetPath.AssetDep.GetAllDependencies(newName);
        List<string> needDownLoadFiles = new List<string>();
        string fileName = "";
        List<UniTask<object>> tasks =new List<UniTask<object>>();
        foreach (string info in paths)
        {
            fileName = AssetPath.GetPlatformPath(info.ToLower().Trim());
            needDownLoadFiles.Add(fileName);
            HotMgr.Instance.AddPrefabAbNames(prefabName, fileName);
            tasks.Add(HotMgr.Instance.LoadAB(fileName, (ab, abName) => {
                return null;
            }));
        }
        await UniTask.WhenAll(tasks);
        finish();
    }
}
