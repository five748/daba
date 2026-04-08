using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(NewImage))]
public class NewImageEditor:UnityEditor.UI.ImageEditor
{
    private SerializedProperty tempProperty;
    private int key = 0;

    void OnEnable() {
        base.OnEnable();
        tempProperty = serializedObject.FindProperty("key");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        var data = (NewImage)target;
        GUILayout.Space(10);
        data.isInArray = EditorGUILayout.Toggle("isInArray", data.isInArray);
        data.isAutoSize = EditorGUILayout.Toggle("isAutoSize", data.isAutoSize);
        key = DragAreaGetObject.GetOjbect(tempProperty.intValue);
        if(key != -1)
        {
            tempProperty.intValue = key;
        }
        if(key == 0)
        {
            tempProperty.intValue = -1;
            key = -1;
        }
        GUILayout.Space(10);
        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }
}