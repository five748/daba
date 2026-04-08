using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class ResourcesAffair
{
    public int Id;
    public string Desc;
    public int type;
    public long Createdtime;
    public string Ip;
    public int State;
    public string Info;
}
public class ResourceTransactionWindow : EditorWindow
{
    public static EditorWindow window;
    /// <summary>
    /// [MenuItem("事务管理/资源事务管理工具")]  //添加菜单选项
    /// </summary>
    public static void ShowWindow()
    {
        if (window != null)
            return;
        window = EditorWindow.GetWindow(typeof(ResourceTransactionWindow), true, "资源事务管理工具", true);
        window.Show();
    }
    void OnGUI()
    {
        try
        {

            ShowWindow();
            ShowEmpty();
            ShowList();
        }
        catch { }
    }
    static string type = "";
    public int selected_SceneNpcIndex;
    private void ShowEmpty()
    {
        if (GUILayout.Button("刷新".ToString())) // 创建这些按钮，并且当点击按钮时触发条件  
        {
            //刷新
        }
        EditorGUILayout.BeginHorizontal();  //开始水平布局

        EditorGUILayout.LabelField("清空这个类型");
        var arr = new[] { "资源", "其他" };
        selected_SceneNpcIndex = EditorGUILayout.Popup(selected_SceneNpcIndex, arr.ToArray());
        // Name = EditorGUILayout.TextField("Name", Name);
        if (GUILayout.Button("清空".ToString())) // 创建这些按钮，并且当点击按钮时触发条件  
        {
            //清空

        }
        EditorGUILayout.EndHorizontal();  //
        EditorGUILayout.BeginHorizontal();  //开始水平布局
        EditorGUILayout.LabelField("ID", GUILayout.MaxWidth(50));
        EditorGUILayout.LabelField("描述", GUILayout.MaxWidth(100));
        EditorGUILayout.LabelField("类型", GUILayout.MaxWidth(50));
        EditorGUILayout.LabelField("创建时间", GUILayout.MaxWidth(50));
        EditorGUILayout.LabelField("状态", GUILayout.MaxWidth(50));
        EditorGUILayout.LabelField("ip", GUILayout.MaxWidth(50));
        EditorGUILayout.LabelField("其他", GUILayout.MaxWidth(50));
        EditorGUILayout.EndHorizontal();  //结束水平布局
    }

    public static void ShowList()
    {
        //Debug.LogError(TransfromHelp.Instance.rocords.Count);
        //for (var i = 0; i < TransfromHelp.Instance.rocords.Count; i++)
        //{
        //    EditorGUILayout.BeginHorizontal();  //开始水平布局
        //    EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(50));
        //    EditorGUILayout.LabelField(TransfromHelp.Instance.rocords[i], GUILayout.MaxWidth(100));
        //    EditorGUILayout.LabelField("类型", GUILayout.MaxWidth(50));
        //    EditorGUILayout.LabelField("202010201", GUILayout.MaxWidth(50));
        //    if (i == 0)
        //    {
        //        EditorGUILayout.LabelField("状态", GUILayout.MaxWidth(50));
        //    }
        //    else
        //    {
        //        if (GUILayout.Button("修复", GUILayout.MaxWidth(50))) // 创建这些按钮，并且当点击按钮时触发条件  
        //        {

        //        }
        //    }
        //    EditorGUILayout.EndHorizontal();  //结束水平布局
        //}
    }


}
