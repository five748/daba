using UnityEngine;
using System.Collections;
using UnityEditor;

public class DragAreaGetObject:Editor
{
    public static int GetOjbect(int meg) {
        Event aEvent;
        aEvent = Event.current;

        GUI.contentColor = Color.white;
        int temp = -1;
        var dragArea = GUILayoutUtility.GetRect(100f, 20f, GUILayout.ExpandWidth(true));
        var rect0 = new Rect(dragArea.x, dragArea.y, 80, dragArea.height);
        var rect1 = new Rect(dragArea.x + 80, dragArea.y, dragArea.width - 130, dragArea.height);
        var rect2 = new Rect(dragArea.x + dragArea.width - 50, dragArea.y, 50, dragArea.height);

        GUI.Box(rect0, "key", GUI.skin.button);
        GUI.Box(rect1, meg.ToString(), GUI.skin.button);
        if(GUI.Button(rect2, "重置")) {
            return 0;
        }
        switch(aEvent.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if(!dragArea.Contains(aEvent.mousePosition))
                {
                    break;
                }

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if(aEvent.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    temp = int.Parse(DragAndDrop.GetGenericData("key").ToString());
                }
                Event.current.Use();
                break;
            default:
                break;
        }
        return temp;
    }
    public static string GetGameObject(string prefabName) {
        Event aEvent;
        aEvent = Event.current;
        GUI.contentColor = Color.white;
        string temp = prefabName;
        var dragArea = GUILayoutUtility.GetRect(100f, 20f, GUILayout.ExpandWidth(true));
        var rect0 = new Rect(dragArea.x, dragArea.y, 100, dragArea.height);
        var rect1 = new Rect(dragArea.x + 100, dragArea.y, dragArea.width - 150, dragArea.height);
        var rect2 = new Rect(dragArea.x + dragArea.width - 50, dragArea.y, 50, dragArea.height);

        GUI.Box(rect0, "GotoPrefabPath", GUI.skin.button);
        GUI.Box(rect1, prefabName, GUI.skin.button);
        if(GUI.Button(rect2, "重置"))
        {
            return "";
        }
        switch(aEvent.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if(!dragArea.Contains(aEvent.mousePosition))
                {
                    break;
                }

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if(aEvent.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    if(DragAndDrop.paths.Length == 1)
                        temp = DragAndDrop.paths[0].GetSpitLast('/').Replace(".prefab", "");
                    //Debug.LogError(temp);
                }
                Event.current.Use();
                break;
            default:
                break;
        }
        return temp;
    }
    public static int GetOjbectScroll(int meg, System.Action clickCreate) {
        Event aEvent;
        aEvent = Event.current;

        GUI.contentColor = Color.white;
        int temp = -1;
        var dragArea = GUILayoutUtility.GetRect(100f, 20f, GUILayout.ExpandWidth(true));
        var rect0 = new Rect(dragArea.x, dragArea.y, 50, dragArea.height);
        var rect1 = new Rect(dragArea.x + 50, dragArea.y, dragArea.width - 150, dragArea.height);
        var rect2 = new Rect(dragArea.x + dragArea.width - 100, dragArea.y, 50, dragArea.height);
        var rect3 = new Rect(dragArea.x + dragArea.width - 50, dragArea.y, 50, dragArea.height);

        GUI.Box(rect0, "key", GUI.skin.button);
        GUI.Box(rect1, meg.ToString(), GUI.skin.button);
        if(GUI.Button(rect2, "重置"))
        {
            return 0;
        }
        if(GUI.Button(rect3, "生成"))
        {
            clickCreate();
        }
        switch(aEvent.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if(!dragArea.Contains(aEvent.mousePosition))
                {
                    break;
                }

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if(aEvent.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    temp = int.Parse(DragAndDrop.GetGenericData("key").ToString());
                }
                Event.current.Use();
                break;
            default:
                break;
        }
        return temp;
    }
}