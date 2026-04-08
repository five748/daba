using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HotConfig {
    public static string artFlag = "↕";
    public static string ArtWritePath
    {
        get
        {
            return Application.dataPath + "/Res/table/artab.txt".ModifyTablePath();
        }
    }
    public static string ArtReadPath = "artab";
    
    public static string ArtABFlag = "artaart";
    //public static string FChinese = "fchinese";
    public static string UIAnimation = "Res/animation/";
    public static string UIAltasPath = "Res/atlas/";
    public static string UITexturePath = "Res/Image/ChangeImage/";
    //public static string UIFChinesePath = "Res/FChinese/ChangeImage/";
    //public static string UIEnglishPath = "Res/English/ChangeImage/";
    public static string PrefabPath = "Res/Prefab/";
    public static string EffectPrefabPath = "Res/effectold/";
    public static string EffectNewPrefabPath = "Res/effect/";
    public static string AudioPath = "Res/music/";
    public static string TabelPath = "Res/table/";
    public static string TabelPath8 = "Res/table8/";
    public static string SpinePrefab = "Res/Spine/";
    public static string SceneBin = "Res/AutoMesh";
    public static string SkillBin = "Res/Skill";
    public static string SkillBin8 = "Res/Skill8";
    public static string StoryBin = "Res/StoryAndAI";
    public static string HotScrBin = "Res/hotscript";
    public static string TableBytes = "Res/tableByte/";
    public static readonly string UpURL = "http://192.168.31.158:8092/admin/common/uploadPic";              //资源服务器上传地址
    public static string DownVersionURL
    { //版本文件地址
        get
        {
            return "";
        }
    }
    public static string DownURL
    { //资源服务器下载地址
        get
        {
            return "https://www.resmagicgame.com/Xjfc/Webgl";
        }
    }
}
