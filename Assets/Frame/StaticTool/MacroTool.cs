using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class MacroTool
{
#if UNITY_EDITOR
    /// <summary>
    /// 添加某一宏定义
    /// </summary>
    /// <param name="strMacroName">宏名称</param>
    public static void AddMacro(string strMacroName, BuildTargetGroup currentTarget) {
        if (currentTarget == BuildTargetGroup.Unknown) return;
        var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
        var defines = definesString.Split(';').ToList();

        int index = 0;
        while (index < defines.Count)
        {
            if (defines.Contains(strMacroName))
            {
                return;
            }
            index++;
        }

        if (defines.Contains(strMacroName) == false)
        {
            definesString = string.Empty;
            for (int i = 0; i < defines.Count; i++)
            {
                definesString += defines[i];
                definesString += ";";
            }
            Debug.Log(definesString);
            if (definesString.EndsWith(";", StringComparison.InvariantCulture) == false)
            {
                definesString += ";";
            }

            definesString += strMacroName;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
        }
        AssetDatabase.Refresh();
    }
    
    /// <summary>
    /// 批量添加宏定义
    /// </summary>
    /// <param name="strMacroNames">宏定义列表</param>
    public static void AddMacro(List<string> strMacroNames, BuildTargetGroup currentTarget) 
    {
        if (currentTarget == BuildTargetGroup.Unknown) return;
        var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
        var defines = definesString.Split(';').ToList();

        List<string> addList = new List<string>();
        foreach (var value in strMacroNames)
        {
            if (defines.Contains(value))
            {
                continue;
            }
            addList.Add(value);
        }


        definesString = string.Empty;
        for (int i = 0; i < defines.Count; i++)
        {
            definesString += defines[i];
            definesString += ";";
        }
        Debug.Log(definesString);
        if (definesString.EndsWith(";", StringComparison.InvariantCulture) == false)
        {
            definesString += ";";
        }

        int count = addList.Count;
        for (int i = 0; i < count; i++)
        {
            definesString += addList[i];
            if (i != count - 1)
            {
                definesString += ";";
            }
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
        AssetDatabase.Refresh();
    }
    
    /// <summary>
    /// 移除某一宏定义
    /// </summary>
    /// <param name="strMacroName">宏名称</param>
    public static void RemoveMacro(string strMacroName, BuildTargetGroup currentTarget) {
        if (currentTarget == BuildTargetGroup.Unknown) return;
        var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
        var defines = definesString.Split(';').ToList();

        int index = 0;
        bool hasFind = false;
        while (index < defines.Count)
        {
            if (defines[index] == strMacroName)
            {
                defines.RemoveAt(index);
                hasFind = true;
                break;
            }
            index++;
        }

        if (!hasFind)
        {
            Debug.Log($"原本不存在{strMacroName}, 不需要移除");
            return;
        }
        
        definesString = string.Empty;
        for (int i = 0; i < defines.Count; i++)
        {
            definesString += defines[i];
            definesString += ";";
        }
        Debug.Log(definesString);
        if (definesString.EndsWith(";", StringComparison.InvariantCulture) == false)
        {
            definesString += ";";
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
        
        AssetDatabase.Refresh();
    }
    
    /// <summary>
    /// 批量移除宏定义
    /// </summary>
    /// <param name="strMacroNames">宏定义列表</param>
    public static void RemoveMacros(List<string> strMacroNames, BuildTargetGroup currentTarget) {
        if (currentTarget == BuildTargetGroup.Unknown) return;
        var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
        var defines = definesString.Split(';').ToList();

        foreach (var value in strMacroNames)
        {
            int index = 0;
            while (index < defines.Count)
            {
                if (defines[index] == value)
                {
                    defines.RemoveAt(index);
                    break;
                }
                index++;
            }
        }
        
        definesString = string.Empty;
        for (int i = 0; i < defines.Count; i++)
        {
            definesString += defines[i];
            definesString += ";";
        }
        Debug.Log(definesString);
        if (definesString.EndsWith(";", StringComparison.InvariantCulture) == false)
        {
            definesString += ";";
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
        
        AssetDatabase.Refresh();
    }
    
    [MenuItem("程序工具/项目宏定义/飞船/添加项目宏定义")]
    public static void SpaceshipAddMacros() {
        AddMacro(new List<string>() { "SHIP_AIR" }, BuildTargetGroup.Android);
        AddMacro(new List<string>() { "SHIP_AIR" }, BuildTargetGroup.iOS);
        AddMacro(new List<string>() { "SHIP_AIR" }, BuildTargetGroup.WebGL);
        AddMacro(new List<string>() { "SHIP_AIR" }, BuildTargetGroup.Standalone);
    }
    
    [MenuItem("程序工具/项目宏定义/大坝/打开控制面板")]
    public static void OpenControlPanel() {
        AddMacro("CONTROL_PANEL", BuildTargetGroup.Android);
        AddMacro("CONTROL_PANEL", BuildTargetGroup.iOS);
        AddMacro("CONTROL_PANEL", BuildTargetGroup.WebGL);
        AddMacro("CONTROL_PANEL", BuildTargetGroup.Standalone);
    }
    
    [MenuItem("程序工具/项目宏定义/大坝/关闭控制面板")]
    public static void CloseControlPanel() {
        RemoveMacro("CONTROL_PANEL", BuildTargetGroup.Android);
        RemoveMacro("CONTROL_PANEL", BuildTargetGroup.iOS);
        RemoveMacro("CONTROL_PANEL", BuildTargetGroup.WebGL);
        RemoveMacro("CONTROL_PANEL", BuildTargetGroup.Standalone);
    }
#endif
}