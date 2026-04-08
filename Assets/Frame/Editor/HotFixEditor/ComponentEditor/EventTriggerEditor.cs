using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(EventTriggerListener))]
public class EventTriggerEditor:Editor
{
    private SerializedProperty property3D;
    private SerializedProperty tempProperty;
    private SerializedProperty prefabPathPy;
    private SerializedProperty onCLickPy;
    private SerializedProperty keyIndexsPy;

    private int key = 0;
    private string prefabPath;
    void OnEnable() {
        property3D = serializedObject.FindProperty("is3D");
        tempProperty = serializedObject.FindProperty("key");
        prefabPathPy = serializedObject.FindProperty("GotoPrefabPath");
        onCLickPy = serializedObject.FindProperty("onClick");
        keyIndexsPy = serializedObject.FindProperty("keyIndexs");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        var data = (EventTriggerListener)target;
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(property3D, true);
        EditorGUILayout.PropertyField(keyIndexsPy, true);
        data.isInArray = EditorGUILayout.Toggle("isInArray", data.isInArray);
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
        prefabPathPy.stringValue = DragAreaGetObject.GetGameObject(prefabPathPy.stringValue);
        //Debug.LogError(prefabPathPy.stringValue);

        GUILayout.Space(10);
        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }
}