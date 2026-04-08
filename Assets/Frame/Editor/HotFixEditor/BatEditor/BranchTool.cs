
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text;
using System.Threading;
public class CMDTool
{
    public string WorkingDirectory { get; set; }

    public CMDTool(string workingDirectory)
    {
        //Debug.LogError(workingDirectory);
        WorkingDirectory = workingDirectory;
    }
    public string Run(string executablePath, string arguments = "")
    {
        ProcessStartInfo info = null;
        if (arguments != "")
        {
            info = new ProcessStartInfo(executablePath, arguments);
        }
        else
        {
            info = new ProcessStartInfo(executablePath);
        }

        info.CreateNoWindow = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        info.WorkingDirectory = WorkingDirectory;
        info.ErrorDialog = true;
        var process = new Process
        {
            StartInfo = info,
        };
        process.Start();

        string log = process.StandardOutput.ReadToEnd();

        process.Dispose();
        process.Close();
        return log;
    }
    public void RunAnsic(string executablePath, string arguments, System.Action callback)
    {
        using (Process process = new Process())
        {
            StringBuilder processOutputBuilder = new StringBuilder();

            process.StartInfo = new ProcessStartInfo(executablePath, arguments);
            if (WorkingDirectory != null)
            {
                process.StartInfo.WorkingDirectory = WorkingDirectory;
            }

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.EnableRaisingEvents = true;

            AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
            AutoResetEvent errorWaitHandle = new AutoResetEvent(false);
            process.OutputDataReceived += (sender, eventArgs) =>
            {
                //if (eventArgs.Data != null)
                //{
                //    processOutputBuilder.AppendLine(eventArgs.Data);
                //}
                //else
                //{
                //    outputWaitHandle.Set();
                //}
            };

            process.ErrorDataReceived += (sender, eventArgs) =>
            {
                //if (eventArgs.Data != null)
                //{
                //    processOutputBuilder.AppendLine(eventArgs.Data);
                //}
                //else
                //{
                //    errorWaitHandle.Set();
                //}
            };
            process.Start();
            process.BeginOutputReadLine();
            //process.BeginErrorReadLine();
            //outputWaitHandle.WaitOne();
            //errorWaitHandle.WaitOne();
            //process.CancelOutputRead();
            //process.CancelErrorRead();
            callback();
        }
    }
}
public class BranchTool : EditorWindow
{
    [MenuItem("Git工具/切换分支")]
    static void ChangeBanch()
    {
        //AssetPath.GetAllFileNames(Application.dataPath.Replace("Assets", "") + ".git" + "");
        cmdTool.Run("git", "fetch --prune");
        string str = cmdTool.Run("git", "branch -r");
        OnBranch = GetOnBanch();
        Debug.Log(OnBranch);
        if (str != "")
        {
            Debug.LogError(str);
            BranchStrs = str.Trim().Split('\n');
        }
        EditorWindow.GetWindow<BranchTool>();
    }
    private static string OnBranch;
    public static string GetOnBanch()
    {
        string localstr = cmdTool.Run("git", "branch");
        var strs = localstr.Trim().Split('\n');
        foreach (var item in strs)
        {
            if (item.IndexOf("*") != -1)
            {
                return item.Replace("*", "").Trim();
            }
        }
        return "";
    }
    public static bool IsPlanTestBanch()
    {
        return GetOnBanch() == "PlannerTest";
    }
    private static string[] BranchStrs;
    private static CMDTool _cmdTool;
    public static CMDTool cmdTool
    {
        get
        {
            if (_cmdTool == null)
            {
                _cmdTool = new CMDTool(Application.dataPath.Replace("Assets", ""));
            }
            return _cmdTool;
        }
    }
    private void OnGUI()
    {
        if (BranchStrs != null)
        {

            foreach (var str in BranchStrs)
            {
                string newstr = str.Replace("origin/", "");
                if (newstr.IndexOf("HEAD") == -1)
                {
                    if (newstr.Trim() != OnBranch)
                        CreateBranchWindow(newstr.Trim());
                }
            }
        }
    }
    void CreateBranchWindow(string str)
    {
        if (GUILayout.Button(str))
        {
            if (!EditorUtility.DisplayDialog("", "切换分支会丢弃本地修改, 是否继续?", "ok", "no"))
                return;
            cmdTool.Run("git", "checkout .");
            cmdTool.Run("git", "clean -df");
            cmdTool.Run("git", "checkout " + str);
            Close();
            AssetDatabase.Refresh();
        }
    }



    //[MenuItem("美术工具/Git上传并拉取仓库数据")]
    static void UpdateProgramAssetArt()
    {
        int i = Application.dataPath.Length - 6;
        LoadAssetToUnity(Application.dataPath.Substring(0, i) + "artupdate.bat", GetOnBranch());
    }

    [MenuItem("Git工具/Git上传并拉取仓库数据")]
    static void UpdateProgramAsset()
    {
        if (CheckoutIsHaveErr())
        {
            return;
        }
        PermissionAdd();
        int i = Application.dataPath.Length - 6;
        LoadAssetToUnity(Application.dataPath.Substring(0, i) + "gitupdate.bat", GetOnBranch());
    }
    private static bool CheckoutIsHaveErr()
    {
        var log = cmdTool.Run("git", "diff --check");
        if (!string.IsNullOrEmpty(log) && log.Contains("conflict marker"))
        {
            EditorUtility.DisplayDialog("", "项目冲突,请找程序解决!", "ok");
            return true;
        }
        return false;
    }
    private static void PermissionAdd()
    {
        string useName = cmdTool.Run("git", "config user.name").Trim();
        string idStr = "";
        //foreach (var item in TableCache.Instance.clientPermissionTable)
        //{
        //    if (item.Value.name.Trim() == useName)
        //    {
        //        idStr = item.Value.pathIds;
        //    }
        //}
        if (idStr == "")
        {
            cmdTool.Run("git", "add .");
            return;
        }
        //var ids = idStr.SplitToInt('_');
        string str = "";
        //foreach (var id in ids)
        //{
        //    str += TableCache.Instance.clientPathTable[id].path.Replace("#", " ") + " ";
        //}
        Debug.LogError(useName);
        Debug.LogError(str.Replace(" ", "\n"));
        var log = cmdTool.Run("git", "add " + str.CutLast());
        cmdTool.Run("git", "checkout .");
        cmdTool.Run("git", "clean -df");
    }

    [MenuItem("Git工具/使用服务器版本(放弃本地修改)")]
    static void RemoveLocalAndUseServerArt()
    {
        if (CheckoutIsHaveErr())
        {
            return;
        }
        if (!EditorUtility.DisplayDialog("", "是否使用服务器版本(丢弃本地修改)?", "ok", "no"))
            return;
        int i = Application.dataPath.Length - 6;
        LoadAssetToUnity(Application.dataPath.Substring(0, i) + "useserver.bat", GetOnBranch());
    }
    [MenuItem("程序工具/Frame/使用服务器版本(放弃本地修改)")]
    static void RemoveLocalAndUseServerArtFrame()
    {
        if (CheckoutIsHaveErr())
        {
            return;
        }
        if (!EditorUtility.DisplayDialog("", "是否使用服务器版本(丢弃本地修改)?", "ok", "no"))
            return;
        LoadAssetToUnity(Application.dataPath + "/baseframewx/" + "useserver.bat", GetOnBranch());
    }
    [MenuItem("程序工具/Frame/上传并拉取仓库数据")]
    static void FrameUpdateProgramAsset()
    {
        if (CheckoutIsHaveErr())
        {
            return;
        }
        LoadAssetToUnity(Application.dataPath + "/baseframewx/" + "gitupdate.bat", GetOnBranch());
    }
    public static void LoadAssetToUnity(string path, string Arguments, System.Action callback = null, bool isOpenShell = true)
    {
        UnityEngine.Debug.Log(path);
        Process process = new Process();

        process.StartInfo.FileName = path;
        process.StartInfo.UseShellExecute = isOpenShell;
        process.StartInfo.WorkingDirectory = path.GetSpitUpLast('/');
        process.StartInfo.Arguments = Arguments;
        //这里相当于传参数 
        //process.StartInfo.Arguments = "hello world";
        process.Start();
        //执行结束 
        process.WaitForExit();
        AssetDatabase.Refresh();
        if (callback != null)
        {
            callback();
            AssetDatabase.Refresh();
        }
    }
    public static void CMDSHANDLoadAssetToUnity(string path, System.Action callback = null, bool isOpenShell = true)
    {
        UnityEngine.Debug.Log(path);
        Process process = new Process();

        process.StartInfo.FileName = "/bin/sh";
        process.StartInfo.UseShellExecute = isOpenShell;
        process.StartInfo.WorkingDirectory = path.GetSpitUpLast('/');
        process.StartInfo.CreateNoWindow = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.Arguments = path;
        //这里相当于传参数 
        //process.StartInfo.Arguments = "hello world";
        process.Start();
        //执行结束 
        process.WaitForExit();
        string stdOutput = process.StandardOutput.ReadToEnd();
        if (stdOutput.Length > 0)
            UnityEngine.Debug.LogError(stdOutput);
        string errOutput = process.StandardError.ReadToEnd();
        if (errOutput.Length > 0)
            UnityEngine.Debug.LogError(errOutput);
        AssetDatabase.Refresh();
        if (callback != null)
        {
            callback();
            AssetDatabase.Refresh();
        }
    }
    public static string GetOnBranch()
    {
        var cmd = new CMDTool(Application.dataPath.Replace("Assets", ""));
        string str = cmd.Run("git", "branch");
        Debug.Log(str);
        if (str != "")
        {
            var strs = str.Trim().Split('\n');
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i].IndexOf("*") != -1)
                {
                    return strs[i].Replace("*", "").Trim();
                }
            }
        }
        return "";
    }

}
