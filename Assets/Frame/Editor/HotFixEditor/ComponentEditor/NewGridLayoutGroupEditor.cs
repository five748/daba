using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
[CanEditMultipleObjects]
[CustomEditor(typeof(NewGridLayoutGroup))]
public class NewGridLayoutGroupEditor: GridLayoutGroupEditor
{
    private SerializedProperty tempProperty;
    private int key = 0;
    private List<string> MenuStyles;
    private string menuName = "样式";
    private NewGridLayoutGroup data;
    void OnEnable()
    {
        base.OnEnable();
        tempProperty = serializedObject.FindProperty("key");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        GUILayout.Space(10);
        data = (NewGridLayoutGroup)target;
        data.isInArray = EditorGUILayout.Toggle("isInArray", data.isInArray);
        key = DragAreaGetObject.GetOjbect(tempProperty.intValue);
        if (key != -1)
        {
            tempProperty.intValue = key;
        }
        if (key == 0)
        {
            tempProperty.intValue = -1;
            key = -1;
        }
        GUILayout.Space(10);
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        StyleCenter.ShowMenuButtonLayout(menuName, MenuStyles, (str) =>
        {
            menuName = str;
        });
        serializedObject.ApplyModifiedProperties();
    }
}