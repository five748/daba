using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[System.Serializable]
public class ReadAndSaveToolSomeOne
{
    public static T Read<T>(string readPath, System.Func<T> newT) {
        TextAsset textAsset = null;
        AssetLoadOld.Instance.LoadAsset(readPath, ".bytes", (go) => {
            textAsset = go as TextAsset;
        });
        if(textAsset == null)
        {
            return newT();
        }
        MemoryStream ms = new MemoryStream(textAsset.bytes);
        return SerializeHelper.MemoryToInstance<T>(ms);
    }
    public static void Save<T>(T t,string savePath) {
        SerializeHelper.InstaceToFile(t, savePath);
    }
}
