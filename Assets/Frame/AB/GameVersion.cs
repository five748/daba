﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GameVersionSpace
{
    public static class VersionConst
    {
        public const string idKey = "version";
        public const string timeKey = "versionTime";

        public static string outPath = AssetPath.AssetBundlePath + "version.txt";
    }

    public class FileVersion
    {
        public string path;
        public bool isIn = true;
        public string md5;
        public string newMd5;
        public bool needDonwUpdate = false;
        public bool needDel = false;
        public long length;

        public override string ToString()
        {
            return string.Format("{0},{1}\n", path, md5);
        }
    }

    public class VersionClass
    {
        public int id = 0; //1023
        public long timestamp = 0;  //创建时间戳

        public int newId = 0;
        public long newTimestamp = 0;
        public string note = "";
        public string new_note = "";

        public Dictionary<string, FileVersion> dict;

        public VersionClass()
        {
          
        }
        public VersionClass(string str)
        {
            StringToDict(str);

            int.TryParse(dict[VersionConst.idKey].md5, out id);
            //dict.Remove(VersionConst.idKey);

            if(dict.ContainsKey(VersionConst.timeKey))
            {
                long.TryParse(dict[VersionConst.timeKey].md5, out timestamp);
                dict.Remove(VersionConst.timeKey);
            }
        }

        void StringToDict(string str)
        {
            str.Replace("\r\n", "\n");
            dict = new Dictionary<string, FileVersion>();
            //Config
            string[] items = str.Split('\n');
            string[] versionInfo = items[0].Split(',');
            for (int i = 0; i < items.Length; i++)
            {
                string[] info = items[i].Split(',');
                if (info != null && info.Length >= 2)
                {
                    FileVersion fv = new FileVersion();

                    fv.path = info[0];
                    fv.md5 = info[1];

                    if(info.Length == 3)
                        long.TryParse(info[2], out fv.length);

                    dict.Add(info[0], fv);
                }
            }

            note = items[items.Length - 1];
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.Append(string.Format("{0},{1}\n", "version", id));
            str.Append(string.Format("{0},{1}\n", "versionTime", timestamp));

            foreach (FileVersion f in dict.Values)
            {
                str.Append(f.ToString());
            }

            str.Append(note);
            return str.ToString();
        }
    }

    public class GameVersion : Single<GameVersion>
    {
        string outPath;
        public VersionClass localVersion;
        private string inStr;
        public System.Action CallBack;
        public void InitVersion(System.Action callback)
        {
            CallBack = callback;
            outPath = VersionConst.outPath;
            if(ProssData.Instance.IsOpenHot) {
                LoadLoaclVersion();
            }
            else
            {
                ProssData.Instance.VersionId = int.MaxValue;
                callback();
            }
        }

        void LoadLoaclVersion() {
            string url = AssetPath.GetStreamingAssetsPath() + "/asset/version.txt";
            AssetLoadOld.Instance.LoadStreamAssetText(url, CompareVersion);
        }

        void CompareVersion(string str)
        {
            localVersion = new VersionClass(str);
            if (File.Exists(outPath))
            {
                string outStr = File.ReadAllText(outPath);
                VersionClass outVersion = new VersionClass(outStr);

                if(localVersion.timestamp > outVersion.timestamp)
                {
                    ProssData.Instance.VersionId = localVersion.id;
                    File.Delete(outPath);
                }
                else if (localVersion.timestamp == outVersion.timestamp)
                {
                    ProssData.Instance.VersionId = localVersion.id;
                }
                else if (localVersion.timestamp < outVersion.timestamp)
                {
                    Dictionary<string, FileVersion> inDict = localVersion.dict;
                    Dictionary<string, FileVersion> outDict = outVersion.dict;

                    foreach(KeyValuePair<string, FileVersion> element in outDict)
                    {
                        if (element.Key == VersionConst.idKey || element.Key == VersionConst.timeKey)
                        {
                            continue;
                        }

                        if (inDict.ContainsKey(element.Key))
                        {
                            if (element.Value.md5 != inDict[element.Key].md5)
                            {
                                element.Value.isIn = false;
                            }
                        }
                        else
                        {
                            element.Value.isIn = false;
                        }
                    }

                    localVersion = outVersion;
                    ProssData.Instance.VersionId = localVersion.id;
                }
                else
                {
                    ProssData.Instance.VersionId = localVersion.id;
                    File.Delete(outPath);
                }
            }
            else
            {
                ProssData.Instance.VersionId = localVersion.id;
            }
            CallBack();
        }
        
        void SaveVersionTxt()
        {
            // StringBuilder str = new StringBuilder();

            // localVersion.id = localVersion.newId;
            // localVersion.timestamp = localVersion.newTimestamp;

            // foreach (FileVersion f in localVersion.dict.Values)
            // {
            //     str.Append(f.ToString());
            // }

            File.WriteAllText(outPath, localVersion.ToString());
        }
    }
}








