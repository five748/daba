
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;
using System.Threading;
using System.IO;

public class UploadTool
{
    private enum OperateType
    {
        Upload,
        Download,
        Delete,
    }
    ////[MenuItem("Upload工具/Upload")]
    //static void UploadT()
    //{
    //    operateType = OperateType.Upload;
    //    Debug.Log("Upload工具");
    //    GetMyWindow();
    //}
    ////[MenuItem("Upload工具/Download")]
    //static void DownloadT()
    //{
    //    operateType = OperateType.Download;
    //    Debug.Log("Download工具");
    //    GetMyWindow();
    //}
    ////[MenuItem("Upload工具/Delete")]
    //static void DeleteT()
    //{
    //    operateType = OperateType.Delete;
    //    Debug.Log("Delete工具");
    //    GetMyWindow();
    //}
    ////[MenuItem("Upload工具/Test")]
    //static void Test()
    //{
    //    Config.ServerChannel = 1;
    //    GameTools.VersionId = 1;
    //    GameVersionSpace.HotMgr.Instance.LoadSerVersion(()=> {
    //        Debug.Log("success");
    //    });
    //}

    private void OnGUI()
    {
        
    }


    public static void Upload(int resVersionId, string abPath, string channel)
    {
        Debug.LogError("Upload:" + abPath);
        operateType = OperateType.Upload;
        BreakPointUpload_Aliyun(abPath, channel + "/" + resVersionId + "/asset");
    }
    private void Download(int resVersionId, string abPath, string channel)
    {
        //BreakPointDownload_Aliyun(abPath, channel.id + "/" + ver + "/asset");
    }

    private static string endpoint = "oss-cn-beijing.aliyuncs.com";
    private static string accessKeyId = "LTAI5tE8KMiN6f5Y6HbxBxLL";
    private static string accessKeySecret = "tTcjlmXE9AjhYYQFbU1XC1B0xJlBe0";
    private static string bucketName = "yuewangame";
    private static string checkpointDir = Application.dataPath + "/EditorData";
    private struct StrTwo
    {
        public string path;
        public string address;
    }
    private static Queue<StrTwo> filesQueue;
    private static int nowFilesNum,allFilesNum;
    private static readonly object ThreadLock_Aliyun = new object();
    private static readonly object ThreadLockForEnd_Aliyun = new object();
    static Stopwatch overallStopwatch;
    private static int endThreadNum;
    private static int ThreadNum = 20;
    public static void BreakPointUpload_Aliyun(string path, string address)
    {
        filesQueue = new Queue<StrTwo>();
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        GetAllPath(filesQueue, path, address);
        stopwatch.Stop();
        Debug.Log("遍历文件夹共耗时："+stopwatch.Elapsed.TotalSeconds);
        nowFilesNum = 0;
        allFilesNum = filesQueue.Count;
        overallStopwatch = new Stopwatch();
        overallStopwatch.Start();
        endThreadNum = 0;
        for (int i = 0; i < ThreadNum; i++)
        {
            new Thread(new ThreadStart(Aliyun_ThreadOne)).Start();
        }
    }
    private void BreakPointDownload_Aliyun(string path, string address)
    {
        new Thread(new ThreadStart(()=> {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            GetAllOnlinePath(address,(keys)=> {
                stopwatch.Stop();
                Debug.Log("遍历文件夹共耗时：" + stopwatch.Elapsed.TotalSeconds);
                filesQueue = new Queue<StrTwo>();
                foreach(var item in keys)
                {
                    filesQueue.Enqueue(new StrTwo()
                    {
                        path = path + item.Substring(address.Length),
                        address = item,
                    });
                }
                nowFilesNum = 0;
                allFilesNum = filesQueue.Count;
                overallStopwatch = new Stopwatch();
                overallStopwatch.Start();
                endThreadNum = 0;
                for (int i = 0; i < ThreadNum; i++)
                {
                    new Thread(new ThreadStart(Aliyun_ThreadOne)).Start();
                }
            });
        })).Start();
    }
    private void DeleteFiles_Aliyun(string address)
    {
        new Thread(new ThreadStart(()=> {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            GetAllOnlinePath(address,(keys)=> {
                stopwatch.Stop();
                Debug.Log("遍历文件夹共耗时：" + stopwatch.Elapsed.TotalSeconds);
                filesQueue = new Queue<StrTwo>();
                foreach (var item in keys)
                {
                    filesQueue.Enqueue(new StrTwo()
                    {
                        address = item,
                    });
                }
                nowFilesNum = 0;
                allFilesNum = filesQueue.Count;
                overallStopwatch = new Stopwatch();
                overallStopwatch.Start();
                endThreadNum = 0;
                for (int i = 0; i < ThreadNum * 3; i++)
                {
                    new Thread(new ThreadStart(Aliyun_ThreadOne)).Start();
                }
            });
        })).Start();
    }
    private static void GetAllPath(Queue<StrTwo> que, string path, string address)
    {
        //Debug.LogError(path);
        DirectoryInfo folder = new DirectoryInfo(path);
        var items = folder.GetFileSystemInfos();
        foreach (var item in items)
        {
            if(item is DirectoryInfo)
            {
                GetAllPath(que, path + "/" + item.Name, address + "/" + item.Name);
            }
            else
            {
                if (item.Name.Contains(".manifest")) {
                    continue;
                }
                que.Enqueue(new StrTwo()
                {
                    path = path + "/" + item.Name,
                    address = address + "/" + item.Name,
                });
            }
        }
    }
    private void GetAllOnlinePath(string address, System.Action<List<string>> callback)
    {
        //var keys = new List<string>();
        //Debug.Log("开始遍历线上所有文件：" + address);
        //try
        //{
        //    ObjectListing result = null;
        //    string nextMarker = string.Empty;
        //    do
        //    {
        //        var listObjectsRequest = new ListObjectsRequest(bucketName)
        //        {
        //            Marker = nextMarker,
        //            MaxKeys = 100,
        //            Prefix = address,
        //        };
        //        result = client.ListObjects(listObjectsRequest);
        //        foreach (var summary in result.ObjectSummaries)
        //        {
        //            Debug.Log("遍历到文件：" + summary.Key);
        //            keys.Add(summary.Key);
        //        }
        //        nextMarker = result.NextMarker;
        //    } while (result.IsTruncated);
        //    Debug.Log("List objects of bucket:" + bucketName + " succeeded ");
        //}
        //catch (OssException ex)
        //{
        //    Debug.LogError(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
        //        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId));
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogError("Failed with error info: " + ex.Message);
        //}
        //Debug.Log("共遍历到" + keys.Count + "个文件");
        //callback(keys);
    }
    private static Dictionary<StrTwo, int> errorTimes=new Dictionary<StrTwo, int>();
    static OperateType operateType;
    private static void Aliyun_ThreadOne()
    {
        string operateStr="??";
        switch (operateType)
        {
            case OperateType.Upload:
                operateStr = "上传";
                break;
            case OperateType.Download:
                operateStr = "下载";
                break;
            case OperateType.Delete:
                operateStr = "删除";
                break;
        }
        if (filesQueue == null)
            return;
        while (filesQueue.Count > 0)
        {
            StrTwo item;
            lock (ThreadLock_Aliyun)
            {
                if (filesQueue.Count == 0)
                    break;
                item = filesQueue.Dequeue();
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool flag = false;
            switch (operateType)
            {
                case OperateType.Upload:
                    flag = Aliyun_UploadOne(item.path, item.address);
                    break;
                case OperateType.Download:
                    flag = Aliyun_DownloadOne(item.path, item.address);
                    break;
                case OperateType.Delete:
                    flag = Aliyun_DeleteOne(item.address);
                    break;
            }
            if (!flag)
            {
                stopwatch.Stop();
                if (!errorTimes.ContainsKey(item))
                    errorTimes.Add(item,0);
                errorTimes[item]++;
                if (errorTimes[item] > 0)
                {
                    Debug.LogError(item.path + " $#$#$出现10次以上的错误，现已停止对该文件的$#$#$".Replace("$#$#$", operateStr));
                    continue;
                }
                else
                {
                    Debug.Log(item.path + " $#$#$出现错误，稍后重新尝试$#$#$该文件，该次$#$#$消耗时间：".Replace("$#$#$", operateStr) + stopwatch.Elapsed.TotalSeconds);
                }
                filesQueue.Enqueue(item);
            }
            else
            {
                stopwatch.Stop();
                lock (ThreadLockForEnd_Aliyun)
                {
                    nowFilesNum++;
                    Debug.Log("当前进度：" + nowFilesNum + "/" + allFilesNum);
                    Debug.Log(item.path + " 该文件耗时:" + stopwatch.Elapsed.TotalSeconds);
                }
            }
        }
        Debug.Log(Thread.CurrentThread.ManagedThreadId.ToString()+ "线程完成$#$#$".Replace("$#$#$", operateStr));
        lock (ThreadLockForEnd_Aliyun)
        {
            endThreadNum++;
            if (endThreadNum >= ThreadNum)
            {
                overallStopwatch.Stop();
                Debug.Log("$#$#$完毕，共耗时:".Replace("$#$#$", operateStr) + overallStopwatch.Elapsed.TotalSeconds);
            }
        }
    }
    private static bool Aliyun_UploadOne(string path, string address)
    {
        //try
        //{
        //    UploadObjectRequest request = new UploadObjectRequest(bucketName, address, path)
        //    {
        //        // 指定上传的分片大小。
        //        PartSize = 8 * 1024 * 1024,
        //        // 指定并发线程数。
        //        ParallelThreadCount = 3,
        //        // checkpointDir保存断点续传的中间状态，用于失败后继续上传。如果checkpointDir为null，断点续传功能不会生效，每次失败后都会重新上传。
        //        CheckpointDir = checkpointDir,
        //    };
        //    // 断点续传上传。
        //    client.ResumableUploadObject(request);
        //    //Debug.Log("Resumable upload object:" + address + " succeeded");
        //}
        //catch (OssException ex)
        //{
        //    Debug.LogError(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
        //        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId));
        //    return false;
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogError("Failed with error info: " + ex.Message);
        //    return false;
        //}
        return true;
    }
    private static bool Aliyun_DownloadOne(string path, string address)
    {
        //var dir = path.Substring(0,path.LastIndexOf('/'));
        //if (!System.IO.Directory.Exists(dir))
        //{
        //    Directory.CreateDirectory(dir);
        //}
        //try
        //{
        //    // 通过DownloadObjectRequest设置多个参数。
        //    DownloadObjectRequest request = new DownloadObjectRequest(bucketName, address, path)
        //    {
        //        // 指定下载的分片大小。
        //        PartSize = 8 * 1024 * 1024,
        //        // 指定并发线程数。
        //        ParallelThreadCount = 3,
        //        // checkpointDir用于保存断点续传进度信息。如果某一分片下载失败，再次下载时会根据文件中记录的点继续下载。如果checkpointDir为null，断点续传功能不会生效，每次失败后都会重新下载。
        //        CheckpointDir = checkpointDir,
        //    };
        //    // 断点续传下载。
        //    client.ResumableDownloadObject(request);
        //    //Debug.Log("Resumable download object:" + address + " succeeded");
        //}
        //catch (OssException ex)
        //{
        //    Debug.LogError(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
        //        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId));
        //    return false;
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogError("Failed with error info: " + ex.Message);
        //    return false;
        //}
        return true;
    }
    private static bool Aliyun_DeleteOne(string address)
    {
        //try
        //{
        //    client.DeleteObject(bucketName, address);
        //    Debug.Log("Delete " + address + " succeeded");
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogError("Delete object failed. " + ex.Message);
        //    return false;
        //}
        return true;
    }
}

