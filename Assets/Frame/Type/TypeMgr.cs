using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.ComponentModel;
using UnityEditor;

public class TypeMgr
{
    public static Type GetTypeByStr(string typeName)
    {
        Type type = null;
        Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
        int assemblyArrayLength = assemblyArray.Length;
        for (int i = 0; i < assemblyArrayLength; ++i)
        {
            type = assemblyArray[i].GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }

        for (int i = 0; (i < assemblyArrayLength); ++i)
        {
            Type[] typeArray = assemblyArray[i].GetTypes();
            int typeArrayLength = typeArray.Length;
            for (int j = 0; j < typeArrayLength; ++j)
            {
                if (typeArray[j].Name.Equals(typeName))
                {
                    return typeArray[j];
                }
            }
        }
        return type;
    }
    public static void GetTypeDes(string typeName, System.Action<string, string> callback)
    {
        Type type = GetTypeByStr(typeName);
        foreach (PropertyInfo property in type.GetProperties())
        {
            System.Object[] obs = property.GetCustomAttributes(false);
            foreach (DescriptionAttribute record in obs)
            {
                Debug.Log(record.Description);
            }
        }
    }
    public static List<string> GetTypeFieldNames(string typeName)
    {
        List<string> fileNames = new List<string>();
        Type type = Type.GetType(typeName);
        if (type == null)
        {
            return fileNames;
        }
        var datas = type.GetFields();//获取属性
        foreach (var p in datas)
        {
            fileNames.Add(p.Name);
        }
        return fileNames;
    }
    public static Dictionary<string, string> GetTypeFieldNamesAndTypes(string typeName)
    {
        Dictionary<string, string> fileAndTypeNames = new Dictionary<string, string>();
        Type type = Type.GetType(typeName);
        if (type == null)
        {
            return fileAndTypeNames;
        }
        var datas = type.GetFields();//获取属性
        foreach (var p in datas)
        {
            fileAndTypeNames.Add(p.Name, p.FieldType.Name);
        }
        return fileAndTypeNames;
    }
#if UNITY_EDITOR
    public static bool WindowIsOpen(string windowName)
    {
        System.Reflection.Assembly assembly = typeof(EditorWindow).Assembly;
        Type[] types = assembly.GetTypes();
        for (int i = 0; i < types.Length; i++)
        {
            Debug.Log(types[i].Name);
            if (types[i].Name == windowName)
            {
                Debug.LogError(types[i].Name);
                return true;
            }
        }
        return false;
    }
    static bool isEditorWindow(Type type)
    {
        int i = 0;
        Type temp = type;
        while (null != temp && i < 10000)
        {
            i++;
            if (temp.BaseType == typeof(EditorWindow))
            {
                return true;
            }
            temp = temp.BaseType;
        }
        return false;
    }
#endif
}
