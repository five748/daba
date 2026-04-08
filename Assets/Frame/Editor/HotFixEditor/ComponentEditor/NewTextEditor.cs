using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(NewText))]
public class NewTextEditor : UnityEditor.UI.TextEditor
{
    private SerializedProperty tempProperty;
    private SerializedProperty tempPropertyStyle;
    private int key = 0;
    private List<string> MenuStyles;
    private NewText data;
    void OnEnable()
    {
        base.OnEnable();
        tempProperty = serializedObject.FindProperty("key");
        tempPropertyStyle = serializedObject.FindProperty("menuName");
        ReadTab();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ReadTab();
        serializedObject.Update();
        GUILayout.Space(10);
        data = (NewText)target;
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
        //StyleCenter.ShowMenuButtonLayout(data.menuName, MenuStyles, (str) =>
        //{
        //    ComponentMgr.Instance.SetStyle(data.transform, str);
        //    SetStyle(data.gameObject.GetComponent<Text>(), str);
        //    data.menuName = str;
        //});
        serializedObject.ApplyModifiedProperties();
    }
    private void ReadTab()
    {
        return;
        if (MenuStyles != null)
            return;
        var tab = FrameTableCache.Instance.color_styleTable;
        MenuStyles = new List<string>();
        foreach (var item in tab)
        {
            MenuStyles.Add(item.Value.name);
        }
    }
    private void SetStyle(Text text, string str)
    {
        Table.color_style config = null;
        foreach (var item in TableRead.Instance.ReadTable<Table.color_style>("color_style"))
        {
            if (str == item.Value.name)
            {
                config = item.Value;
                break;
            }
        }
        text.color = Color.white;
        var colorArr = config.Code.Split('_');
        Color oneColor;
        ColorUtility.TryParseHtmlString(colorArr[0], out oneColor);
        if (colorArr.Length == 1)//修改字体颜色
            text.color = oneColor;
        var gradient = data.gameObject.GetComponent<GradientAuto>();//修改描边
        if (gradient != null)
            DestroyImmediate(gradient);
        var outLine = data.gameObject.GetComponent<Outline>();//修改描边
        if (outLine != null)
            DestroyImmediate(outLine);
        var shadows = data.gameObject.GetComponents<Shadow>();//修改投影
        Shadow shadow = null;
        foreach (var item in shadows)
        {
            if (item.GetType().Name == "Shadow")
            {
                shadow = item;
                break;
            }
        }
        if (shadow != null)
            DestroyImmediate(shadow);
        if (colorArr.Length == 2)   //修改渐变
        {
            Color twoColor;
            ColorUtility.TryParseHtmlString(colorArr[1], out twoColor);
            if (gradient == null)
                gradient = data.gameObject.AddComponent<GradientAuto>();

            gradient.OneColor = oneColor;
            gradient.TwoColor = twoColor;
            gradient.m_Whole = (GradientAuto.Whole)config.GradientWhole;
            gradient.m_GradientType = (GradientAuto.GradientType)config.GradientType;
        }
        if (!string.IsNullOrEmpty(config.outline))
        {
            if (outLine == null)
                outLine = data.gameObject.AddComponent<Outline>();

            Color newColor;
            if (ColorUtility.TryParseHtmlString(config.outline, out newColor))
            {
                outLine.effectColor = newColor;
            }
            if (!string.IsNullOrEmpty(config.outlineRang))
            {
                var strs = config.outlineRang.Split('_');
                outLine.effectDistance = new Vector2(float.Parse(strs[0]), float.Parse(strs[1]));
            }
            else
            {
                outLine.effectDistance = new Vector2(0.5f, -0.5f);
            }

        }
        if (!string.IsNullOrEmpty(config.shadow))
        {
            if (shadow == null)
                shadow = data.gameObject.AddComponent<Shadow>();

            Color newColor;
            if (ColorUtility.TryParseHtmlString(config.shadow, out newColor))
            {
                shadow.effectColor = new Color(newColor.r, newColor.g, newColor.b, 0.4f);
            }
            if (!string.IsNullOrEmpty(config.shadowRang))
            {
                var strs = config.shadowRang.Split('_');
                shadow.effectDistance = new Vector2(float.Parse(strs[0]), float.Parse(strs[1]));
            }
        }
    }
}