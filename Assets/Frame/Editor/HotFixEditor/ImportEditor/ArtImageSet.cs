using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArtImageSet
{
    //[MenuItem("美术工具/命名图片")]
    static void ImportImage()
    {
       
    }
    private static void ModifyImageNameToEnglish() { 
        
    }
    private void GetAllImage(string path) {
        path.GetAllFileName(null, (file) => {
            if (file.Name.Contains(".png") || file.Name.Contains(".jpg")) {
                return;
            }
        });
    }
}
