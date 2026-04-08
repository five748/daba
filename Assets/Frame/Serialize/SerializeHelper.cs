using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization;

class MyBinder<T> : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Type typeToDeserialize = null;
        string assemVer1 = "HotFix, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        string assemVer2 = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        string assemVer3 = "Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        typeToDeserialize = Type.GetType(typeName);
        if (typeToDeserialize == null)
        {
            if (typeName.Contains("Version="))
            {
                typeToDeserialize = Type.GetType(typeName.Replace("Assembly-CSharp", "HotFix"));
                if (typeToDeserialize == null)
                {
                    typeToDeserialize = Type.GetType(typeName.Replace("Assembly-CSharp", "Assembly-CSharp"));
                }
                if (typeToDeserialize == null)
                {
                    typeToDeserialize = Type.GetType(typeName.Replace("Assembly-CSharp", "Main"));
                }
                if (typeToDeserialize == null)
                {
                    typeToDeserialize = Type.GetType(typeName.Replace("Main", "HotFix"));
                }
                if (typeToDeserialize == null)
                {
                    typeToDeserialize = Type.GetType(typeName.Replace("HotFix", "Main"));
                }
            }
            else
            {
                typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemVer1));
                if (typeToDeserialize == null)
                {
                    typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemVer2));
                }
                if (typeToDeserialize == null)
                {
                    typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemVer3));
                }
            }
        }
        if (typeToDeserialize == null)
        {
            UnityEngine.Debug.LogError(typeName);
        }
        return typeToDeserialize;
    }
}

public class SerializeHelper
{
    //序列化 把实例对象写入流中
    public static MemoryStream FormatInstanceToMemory(object instance)
    {
        //创建一个流
        MemoryStream ms = new MemoryStream();
        //创建格式化器
        BinaryFormatter bf = new BinaryFormatter();
        //序列化为二进制流
        bf.Serialize(ms, instance);
        return ms;
    }
    //反序列化, 从流中读出实例对象
    public static T MemoryToInstance<T>(Stream st)
    {
        //创建格式化器
        BinaryFormatter bf = new BinaryFormatter();
        //bf.Binder = new MyBinder1<T>();
        //UnityEngine.Debug.LogError(bf.AssemblyFormat);
        //把二进制流反序列化为指定的对象
        return (T)bf.Deserialize(st);
    }
    public static object MemoryToInstanceToObject(Stream st)
    {
        //创建格式化器
        BinaryFormatter bf = new BinaryFormatter();
        //bf.Binder = new MyBinder<T>();
        //把二进制流反序列化为指定的对象
        return bf.Deserialize(st);
    }
    public static T MemoryToInstanceTab<T>(Stream st)
    {
        //创建格式化器
        BinaryFormatter bf = new BinaryFormatter();
        //bf.Binder = new MyBinder<T>();
        //UnityEngine.Debug.LogError(bf.AssemblyFormat);
        //把二进制流反序列化为指定的对象
        return (T)bf.Deserialize(st);
    }
    public static object MemoryToInstanceObject(Stream st)
    {
        //创建格式化器
        BinaryFormatter bf = new BinaryFormatter();
        //bf.Binder = new MyBinder<>();
        //UnityEngine.Debug.LogError(bf.AssemblyFormat);
        //把二进制流反序列化为指定的对象
        return bf.Deserialize(st);
    }


    static System.Reflection.Assembly LoadAssembly(object sender, ResolveEventArgs e)
    {
        UnityEngine.Debug.LogError(e);
        return System.Reflection.Assembly.LoadFrom("Assembly-CSharp");
    }
    public static T FileToInstance<T>(string path)
    {
        T ob = default(T);
        StreamHelper.ReadFileStream(path, fs => {
            ob = MemoryToInstance<T>(fs);
        });
        return ob;
    }
    public static void InstaceToFile<T>(T go, string path)
    {
        //UnityEngine.Debug.Log(path);
        MemoryStream ms = FormatInstanceToMemory(go);
        if (!IsSerializeIsRight<T>(ms))
        {
            return;
        }
        StreamHelper.WriteMemoryStream(path, ms);
    }
    public static T XMLFileToInstance<T>(string _name) where T : new()
    {
        _name = _name.Replace(".bin", ".XML").Replace(".bytes", ".XML");
        XmlSerializer xs = new XmlSerializer(typeof(T));
        Stream stream = new FileStream(_name.Replace(".bin", ".XML"), FileMode.Create, FileAccess.Write, FileShare.Read);
        T t = new T();
        xs.Serialize(stream, t);
        stream.Close();
        return t;
    }
    public static void XMLInstaceToFile<T>(T go, string _name)
    {
        _name = _name.Replace(".bin", ".XML").Replace(".bytes", ".XML");
        XmlSerializer xs = new XmlSerializer(typeof(T));
        Stream stream = new FileStream(_name, FileMode.Create, FileAccess.Write, FileShare.Read);
        xs.Serialize(stream, go);
        stream.Close();
        stream.Dispose();
    }
    public static void JsonInstaceToFile<T>(T go, string _name)
    {
        //UnityEngine.Debug.LogError(typeof(T));
        //_name = _name.Replace(".bin", ".txt");
        //var str = LitJson.JsonMapper.ToJson(go);
        //_name.WriteByUTF8(str);
    }

    public static string CachePath
    {
        get
        {
            return Application.persistentDataPath + "/Test/cache.bin";
        }
    }
    private static bool IsSerializeIsRight<T>(MemoryStream st)
    {
        try
        {
            StreamHelper.WriteMemoryStream(CachePath, st);
            FileToInstance<T>(CachePath);
            return true;
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log("系列化失败:" + e);
            return false;
        }
    }
    public static byte[] ToBytes(object ob)
    {
        var aa = FormatInstanceToMemory(ob);
        return aa.ToArray();
    }
    public static T ToInstance<T>(byte[] bytes)
    {
        MemoryStream ms = new MemoryStream(bytes);
        return MemoryToInstance<T>(ms);
    }
#if UNITY_EDITOR
    public void Test()
    {
        Type t = Type.GetType("LocalDataNode");
        var node = t.Assembly.CreateInstance("LocalDataNode");
    }
#endif
}
