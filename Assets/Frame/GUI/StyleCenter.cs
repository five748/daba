using System.Collections.Generic;
using UnityEngine;
public class StyleCenter
{
    public static void ShowBox(Rect rect, string boxName, GUIStyle style)
    {
        GUI.Box(rect, boxName, style);
    }
    public static void ShowBoxLayout(string boxName, GUIStyle style, params GUILayoutOption[] options)
    {
        GUILayout.Box(boxName, style, options);
    }
    public static void ShowBoxLayout(string boxName, GUIStyle style)
    {
        GUILayout.Box(boxName, style);
    }
    public static void ShowMenuButton(Rect rect, string buttonName, List<string> strs, Event guiEvent, System.Action<string> callback)
    {
        if (GUI.Button(rect, buttonName))
        {
            if (guiEvent.button == 1)
            {
                //右键点击
                ShowMenu(strs, callback);
            }
        }
    }
    public static void ShowMenuButton(Rect rect, string buttonName, List<string> strs, Event guiEvent, GUIStyle style, System.Action<string> callback)
    {
        if (GUI.Button(rect, buttonName, style))
        {
            if (guiEvent.button == 1)
            {
                //右键点击
                ShowMenu(strs, callback);
            }
        }
    }
    public static void ShowMenuButtonLayout(string buttonName, List<string> strs, Event guiEvent, GUIStyle style, string key, System.Action<string, string> callback)
    {

        if (GUILayout.Button(buttonName, style))
        {
            if (guiEvent.button == 1)
            {
                //右键点击
                ShowMenu(strs, key, callback);
            }
        }
    }
    public static void ShowMenuButtonLayout(string buttonName, List<string> strs, Event guiEvent, GUIStyle style, string key, System.Action<string> callback)
    {

        if (GUILayout.Button(buttonName, style))
        {
            if (guiEvent.button == 1)
            {
                //右键点击
                ShowMenu(strs, callback);
            }
        }
    }
    public static void ShowMenuButtonLayout(string buttonName, List<string> strs, System.Action<string> callback, GUILayoutOption style)
    {

        if (GUILayout.Button(buttonName, style))
        {
            //右键点击
            ShowMenu(strs, (str) => {
                buttonName = str;
                callback(str);
            });
        }
    }
    public static void ShowMenuButtonLayout(string buttonName, List<string> strs, System.Action<string> callback)
    {

        if (GUILayout.Button(buttonName))
        {
            //右键点击
            ShowMenu(strs, (str) => {
                buttonName = str;
                callback(str);
            });
        }
    }
    public static void ShowMenuButtonLayout(string fieldName, ref string[] fieldLst, List<string> strs, System.Action<string, int> callback)
    {
        GUILayout.Label(fieldName);
        if (fieldLst == null)
        {
            return;
        }
#if UNITY_EDITOR
        var len = UnityEditor.EditorGUILayout.IntField(fieldLst.Length);
        if (len != fieldLst.Length)
        {
            string[] newData = new string[len];
            for (int i = 0; i < len; i++)
            {
                if (i < fieldLst.Length)
                    newData[i] = fieldLst[i];
            }
            fieldLst = newData;
        }
        for (int i = 0; i < len; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Button(i.ToString(), GUILayout.Width(100));
            if (GUILayout.Button(fieldLst[i]))
            {
                //右键点击
                ShowMenu(strs, i.ToString(), (str, index) => {
                    callback(str, int.Parse(index));
                });
            }
            GUILayout.EndHorizontal();
        }
#endif
    }
    public static void ShowMenuButtonLayoutDic(string fieldName, ref string[] keys, ref string[] values, List<string> strs, System.Action<string, int> callback)
    {
#if UNITY_EDITOR
        GUILayout.Label(fieldName);
        var len = UnityEditor.EditorGUILayout.IntField(keys.Length);
        if (len != keys.Length)
        {
            string[] newkeyData = new string[len];
            for (int i = 0; i < len; i++)
            {
                if (i < keys.Length)
                    newkeyData[i] = keys[i];
            }
            keys = newkeyData;
            string[] newValueData = new string[len];
            for (int i = 0; i < len; i++)
            {
                if (i < values.Length)
                    newValueData[i] = values[i];
            }
            values = newValueData;
        }
        for (int i = 0; i < len; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(keys[i], GUILayout.Width(100)))
            {
                //右键点击
                ShowMenu(strs, i.ToString(), (str, index) => {
                    callback(str, int.Parse(index));
                });
            }
            values[i] = UnityEditor.EditorGUILayout.TextField(values[i]);
            GUILayout.EndHorizontal();
        }
#endif

    }
    public static void ShowMenuButtonLayout(string buttonName, List<string> strs, Event guiEvent, GUIStyle style, System.Action<string> callback)
    {
        if (GUILayout.Button(buttonName, style))
        {
            if (guiEvent.button == 1)
            {
                //右键点击
                ShowMenu(strs, callback);
            }
        }
    }
    public static void ShowMenuButtonLayout(string buttonName, List<string> strs, Event guiEvent, GUIStyle style, System.Action<string> callback, params GUILayoutOption[] options)
    {
        if (GUILayout.Button(buttonName, style, options))
        {
            if (guiEvent.button == 1)
            {
                //右键点击
                ShowMenu(strs, callback);
            }
        }
    }
    public static void ShowMenuButtonLayout(string buttonName, List<string> strs, Event guiEvent, GUIStyle style, string index, System.Action<string, string> callback, params GUILayoutOption[] options)
    {
        if (GUILayout.Button(buttonName, style, options))
        {
            if (guiEvent.button == 1)
            {
                //右键点击
                ShowMenu(strs, index, callback);
            }
        }
    }
    public static void ShowMenu(List<string> strs, System.Action<string> callback)
    {
#if UNITY_EDITOR
        UnityEditor.GenericMenu menu = new UnityEditor.GenericMenu();
        for (int i = 0; i < strs.Count; i++)
        {
            menu.AddItem(new GUIContent(strs[i]), false, (str) =>
            {
                //Debug.Log("新建");
                callback((string)str);
            }, strs[i]);
        }
        menu.ShowAsContext();
        Event.current.Use();
#endif
    }
    //public static void ShowMenuButtonLayout(Rect rect, string buttonName, Dictionary<string, int> datas, GUIStyle style, System.Action<string> callback) {
    //    if(GUI.Button(rect, buttonName, style))
    //    {
    //        ShowMenu(datas, callback);
    //    }
    //}
    //public static void ShowMenu(Dictionary<string, int> datas, System.Action<string> callback) {
    //    GenericMenu menu = new GenericMenu();
    //    foreach(var data in datas)
    //    {
    //        menu.AddItem(new GUIContent(data.Key +　":" + data.Value), false, (str) =>
    //        {
    //            callback((string)str);
    //        }, data.Key.ToString());
    //    }
    //    menu.ShowAsContext();
    //    Event.current.Use();
    //}
    public static void ShowMenu(List<string> strs, string key, System.Action<string, string> callback)
    {

        if (strs == null)
        {
            return;
        }
#if UNITY_EDITOR
        UnityEditor.GenericMenu menu = new UnityEditor.GenericMenu();
        for (int i = 0; i < strs.Count; i++)
        {
            menu.AddItem(new GUIContent(strs[i]), false, (str) =>
            {
                //Debug.Log("新建");
                string[] strs1 = ((string)str).Split('⋚');
                callback(strs1[0], strs1[1]);
            }, strs[i] + '⋚' + key);
        }
        menu.ShowAsContext();
        Event.current.Use();
#endif
       

    }
    public static void ShowMenuButton(object obj, Rect rect, string buttonName, List<string> strs, Event guiEvent, System.Action<string, object> callback)
    {
        if (GUI.Button(rect, buttonName))
        {
            if (guiEvent.button == 1)
            {
                //右键点击
                ShowMenu(obj, strs, callback);
            }
        }
    }
    private static void ShowMenu(object obj, List<string> strs, System.Action<string, object> callback)
    {
#if UNITY_EDITOR
        UnityEditor.GenericMenu menu = new UnityEditor.GenericMenu();
        for (int i = 0; i < strs.Count; i++)
        {
            menu.AddItem(new GUIContent(strs[i]), false, (str) =>
            {
                callback((string)str, obj);
            }, strs[i]);
        }
        menu.ShowAsContext();
        Event.current.Use();
#endif
    }

    public static string ShowTextField(Rect rect, string value, GUISkin myskin)
    {
#if UNITY_EDITOR
        return GUI.TextField(rect, value, myskin.textField);
#endif
        return "";
    }
    public static string ShowTextFieldLayout(string value, GUISkin myskin)
    {
#if UNITY_EDITOR
        return GUILayout.TextField(value, myskin.box);
#endif
        return "";
    }
    public static string ShowTextFieldLayout(string value, GUISkin myskin, params GUILayoutOption[] options)
    {
#if UNITY_EDITOR
        return GUILayout.TextField(value, myskin.box, options);
#endif
        return "";
    }
    public static void ShowText(Rect rect, string value, GUISkin skin)
    {
#if UNITY_EDITOR
        UnityEditor.EditorGUI.LabelField(rect, value, skin.label);
#endif
    }
    public static float ShowFloatTextField(float value, GUISkin myskin)
    {
#if UNITY_EDITOR
        return UnityEditor.EditorGUILayout.FloatField(value, myskin.textArea);
#endif
        return 0;
    }
    public static Vector3 ShowVector3Field(string vecName, Vector3 vec, GUISkin myskin)
    {
#if UNITY_EDITOR
        Rect rect = UnityEditor.EditorGUILayout.GetControlRect();
        rect = new Rect(rect.x, rect.y + 5, rect.width, rect.height);
        return UnityEditor.EditorGUI.Vector3Field(rect, vecName, vec);
#endif
        return Vector3.zero;
    }
    public static Vector2 ShowVector2Field(string vecName, Vector3 vec, GUISkin myskin)
    {
#if UNITY_EDITOR
        Rect rect = UnityEditor.EditorGUILayout.GetControlRect();
        rect = new Rect(rect.x, rect.y + 5, rect.width, rect.height);
        return UnityEditor.EditorGUI.Vector2Field(rect, vecName, vec);
#endif
        return Vector3.zero;
    }
    public static int ShowIntTextField(int value, GUISkin myskin)
    {
#if UNITY_EDITOR
        return UnityEditor.EditorGUILayout.IntField(value, myskin.textArea);
#endif
        return 0;
    }
    public static int ShowIntTextField(Rect rect, int value, GUISkin myskin)
    {
#if UNITY_EDITOR
        return UnityEditor.EditorGUI.IntField(rect, value, myskin.textArea);
#endif
        return 0;
    }
    public static Vector2 ShowVectorTextField(string posName, Vector2 value)
    {
#if UNITY_EDITOR
        return UnityEditor.EditorGUILayout.Vector2Field(posName, value);
#endif
        return Vector2.zero;
    }
    public static void Drag(string msg, Rect rect)
    {
#if UNITY_EDITOR
        if (Event.current != null)
        {
            switch (Event.current.rawType)
            {
                case EventType.MouseDrag:
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        //Debug.LogError("Start dragging");
                        UnityEditor.DragAndDrop.SetGenericData("key", msg);
                        UnityEditor.DragAndDrop.StartDrag("111");
                    }
                    break;
            }
        }
#endif
    }
    public static void DragMenu(string msg, Rect rect)
    {
#if UNITY_EDITOR
        if (Event.current != null)
        {
            switch (Event.current.rawType)
            {
                case EventType.MouseDown:
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        //Debug.Log("Start dragging");
                        UnityEditor.DragAndDrop.SetGenericData("key", msg);
                        UnityEditor.DragAndDrop.StartDrag("选项");
                    }
                    break;
            }
        }
#endif
    }
    public static void DragStory(string msg, Rect rect)
    {
#if UNITY_EDITOR
        if (Event.current != null)
        {
            if (Event.current.button == 1)
            {
                return;
            }
            switch (Event.current.rawType)
            {
                case EventType.MouseDown:
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        //Debug.Log("Start dragging");
                        UnityEditor.DragAndDrop.SetGenericData("key", msg);
                        UnityEditor.DragAndDrop.StartDrag("111");
                    }
                    break;
            }
        }
#endif
    }
    public static int DropStory(Rect rect)
    {
#if UNITY_EDITOR
        switch (Event.current.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!rect.Contains(Event.current.mousePosition))
                {
                    return -1;
                }

                UnityEditor.DragAndDrop.visualMode = UnityEditor.DragAndDropVisualMode.Copy;
                int id = -1;
                if (Event.current.type == EventType.DragPerform)
                {
                    UnityEditor.DragAndDrop.AcceptDrag();
                    id = int.Parse(UnityEditor.DragAndDrop.GetGenericData("key").ToString());
                }
                Event.current.Use();
                return id;
            default:
                return -1;
        }
#endif
        return -1;
    }
}
