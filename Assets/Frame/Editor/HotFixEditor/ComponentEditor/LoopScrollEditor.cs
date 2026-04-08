using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(LoopScroll))]
public class LoopScrollEditor: Editor
{
    private SerializedProperty tempProperty;
    private SerializedProperty beginDirPy;
    private SerializedProperty beginPosPy;
    private SerializedProperty spacePy;
    private SerializedProperty sortPy;
    private SerializedProperty GroupShowPy;
    private SerializedProperty IsHaveGroupMenuPy;
    private SerializedProperty ChildShowsPy;
    private SerializedProperty IsHaveStartPy;
    private SerializedProperty isHaveLoopPy;
    private int key = -1;

    void OnEnable() { 
        tempProperty = serializedObject.FindProperty("key");
        key = tempProperty.intValue;
        beginDirPy = serializedObject.FindProperty("beginDir");
        beginPosPy = serializedObject.FindProperty("BeginPos");
        spacePy = serializedObject.FindProperty("space");
        sortPy = serializedObject.FindProperty("Sorts");
        GroupShowPy = serializedObject.FindProperty("GroupShow");
        //IsHaveGroupMenuPy = serializedObject.FindProperty("IsAutoGroupMenu");
        ChildShowsPy = serializedObject.FindProperty("ChildShows");
        IsHaveStartPy = serializedObject.FindProperty("isHaveStart");
        isHaveLoopPy = serializedObject.FindProperty("isHaveLoop");
        if(key != -1)
        {
            if(key != tempProperty.intValue || fieldNames.Count == 0)
            {
                //SetFieldName();
            }
        }
    }
    private List<string> fieldNames = new List<string>();
    public override void OnInspectorGUI() {
        serializedObject.Update();
        GUILayout.Space(10);
        var loop = (LoopScroll)target;
        EditorGUILayout.PropertyField(IsHaveStartPy);
        EditorGUILayout.PropertyField(isHaveLoopPy);
        EditorGUILayout.PropertyField(beginDirPy);
        EditorGUILayout.PropertyField(beginPosPy);
        EditorGUILayout.PropertyField(spacePy);
        EditorGUILayout.PropertyField(GroupShowPy, true);
        //EditorGUILayout.PropertyField(IsHaveGroupMenuPy, true);
        GUILayout.BeginHorizontal();
        GUILayout.Label("GroupStr", GUILayout.Width(100));
        StyleCenter.ShowMenuButtonLayout(loop.GroupStr, fieldNames, (str) => {
            loop.GroupStr = str;
        });
        GUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(ChildShowsPy, true);
        GUILayout.BeginVertical();
        StyleCenter.ShowMenuButtonLayout("Sort", ref loop.Sorts, fieldNames, (str, index) => {
            loop.Sorts[index] = str;
        });
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        StyleCenter.ShowMenuButtonLayout("Filts", ref loop.Filts, fieldNames, (str, index) => {
            loop.Filts[index] = str;
        });
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        if(loop.SpcKeys == null) {
            loop.SpcKeys = new string[0];
        }
        if(loop.SpcValues == null)
        {
            loop.SpcValues = new string[0];
        }
        StyleCenter.ShowMenuButtonLayoutDic("SpcDatas", ref loop.SpcKeys, ref loop.SpcValues, fieldNames, (str, index) => {
            loop.SpcKeys[index] = str;
        });
        GUILayout.EndVertical();

        key = DragAreaGetObject.GetOjbectScroll(tempProperty.intValue, loop.Create);
        if(key == 0)
        {
            tempProperty.intValue = -1;
            key = -1;
        }
        else
        {
            if(key != -1)
            {
                if(key != tempProperty.intValue || fieldNames.Count == 0)
                {
                    //SetFieldName();
                }
                tempProperty.intValue = key;
            }
        }
        GUILayout.Space(10);
        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }
}