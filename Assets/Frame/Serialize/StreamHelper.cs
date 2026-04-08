using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class StreamHelper  {
    public static void ReadFileStream(string path, System.Action<FileStream> callback) {
        string dir = Path.GetDirectoryName(path);
        if(!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
        //Debug.Log(path);
        using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            callback(fs);
        }
    }
    public static void WriteMemoryStream(string path, MemoryStream ms) {
        ReadFileStream(path, fs => {
            BinaryWriter w = new BinaryWriter(fs);
            w.Write(ms.ToArray());
        });
    }
}
