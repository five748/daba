using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditAssetLoad{

    public static Object Load(string path) {
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Object>("Asset/res" + path);
    }
}
