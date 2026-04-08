using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class TagAndLayoutAutoAdd
{
    [MenuItem("程序工具/添加tag和layout")]
    public static void SetTagAndLayout() {
        List<string> tags = new List<string>()
        {
            "UIGameObject",
            "UITransform",
            "UISprite",
            "UILabel",
            "UIMenuGrid",
            "UITexture",
        };
        List<string> layouts = new List<string>()
        {
            "Tip0",
            "Tip1",
            "Tip2",
            "Tip3",
            "Tip4",
            "Tip5",
            "guide",
            "msg"
        };
        foreach (var tag in tags)
        {
            Debug.LogError("tag:" + tag);
            UnityEditorInternal.InternalEditorUtility.AddTag(tag);
        }
        //增加一个你的的layer，比如：wall
        foreach (var layout in layouts)
        {
            Debug.LogError("layout:" + layout);
            AddOrCreateMyNewLayer(layout);
        }
    }
    /// <summary>
    /// 添加或者创建新的层标记
    /// </summary>
    /// <param name="layer">标记层名</param>
    /// <param name="obj">需要添加的对象</param>
    public static void AddOrCreateMyNewLayer(string layer)
    {
        if (!isHasLayer(layer))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Tagmanager.asset"));
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "layers")
                {
                    for (int i = 8; i < it.arraySize; i++)
                    {
                        SerializedProperty sp = it.GetArrayElementAtIndex(i);
                        if (string.IsNullOrEmpty(sp.stringValue))
                        {
                            sp.stringValue = layer;
                            tagManager.ApplyModifiedProperties();
                            Debug.Log("success:" + layer);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        //obj.layer = LayerMask.NameToLayer(layer);
    }

    /// <summary>
    /// 是否有层标记
    /// </summary>
    /// <param name="layer">标记层名</param>
    /// <returns>判断结果</returns>
    public static bool isHasLayer(string layer)
    {
        string[] layers = InternalEditorUtility.layers;
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].Contains(layer))
                return true;
        }
        return false;
    }
}
