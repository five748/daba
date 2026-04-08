using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using ExtendForCreateScript;
using UnityEngine.Windows;

public class AltasCreate
{
    private static List<string> textureFullName = new List<string>();
    public static SpriteAtlas Create(string atlasName, string abName)
    {
        string path = "Assets/Res/Image/Altas/";
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        path = "Assets/Res/Image/Altas/{0}.spriteatlas".format(atlasName);
        if (!File.Exists(path))
        {
            SpriteAtlas spriteAtlas = new SpriteAtlas();
            SetAltas(spriteAtlas, path, abName);
            return spriteAtlas;
        }
        return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
    }
    public static void AddOneImage(string abName, string altasName, UnityEngine.Sprite sprite)
    {
        var spriteAtlas = Create(altasName, abName);
        List<Object> packables = new List<Object>(spriteAtlas.GetPackables());
        if (!packables.Contains(sprite))
        {
            // 添加到图集中
            //Debug.LogError(sprite.name);
            spriteAtlas.Add(new [] { sprite });
        }
    }
    public static void SetAltas(SpriteAtlas altas, string path, string abName) {
        SpriteAtlasTextureSettings textureSettings = new SpriteAtlasTextureSettings()
        {
            readable = false,
            generateMipMaps = false,
            sRGB = true,
            filterMode = FilterMode.Bilinear,
        };
        altas.SetTextureSettings(textureSettings);

        altas.SetPackingSettings(new SpriteAtlasPackingSettings() {
            enableRotation = false,
            enableTightPacking = false,
            padding = 2,
            enableAlphaDilation = true,
        });

        TextureImporterPlatformSettings webglSetting = altas.GetPlatformSettings("WebGL");
        webglSetting.overridden = true;
        webglSetting.maxTextureSize = 2048;
        webglSetting.textureCompression = TextureImporterCompression.Compressed;
        webglSetting.format = TextureImporterFormat.ASTC_8x8;
        webglSetting.allowsAlphaSplitting = true;
        altas.SetPlatformSettings(webglSetting);

        TextureImporterPlatformSettings androdiSetting = altas.GetPlatformSettings("Android");
        androdiSetting.overridden = true;
        androdiSetting.maxTextureSize = 2048;
        androdiSetting.textureCompression = TextureImporterCompression.Compressed;
        androdiSetting.format = TextureImporterFormat.ASTC_8x8;
        androdiSetting.allowsAlphaSplitting = true;
        altas.SetPlatformSettings(androdiSetting);
        AssetDatabase.CreateAsset(altas, path);
        var assetImport = AssetImporter.GetAtPath(path);
        assetImport.assetBundleName = abName;
        //assetImport.SaveAndReimport();
    }
}
