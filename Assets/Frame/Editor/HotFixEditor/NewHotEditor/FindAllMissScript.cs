using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class FindAllMissScript : EditorWindow
{ 
	[MenuItem("程序工具/CheckMissingScript")]
    static void RemoveMissing()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            GameObject go = Selection.objects[i] as GameObject;
            if (go)
            {
				var components = go.GetComponentsInChildren<Component>(true);
				SerializedObject so = new SerializedObject(go);
				var soProperties = so.FindProperty("m_Component");
				int r = 0;
				for (int j = 0; j < components.Length; j++)
				{
					if (components[j] == null)
					{
						//soProperties.DeleteArrayElementAtIndex(j - r);
						if(j >= 1)
							Debug.LogError(components[j - 1].name+ ":missing脚本");
						r++;
					}
				}
			}
        }
    }
}
