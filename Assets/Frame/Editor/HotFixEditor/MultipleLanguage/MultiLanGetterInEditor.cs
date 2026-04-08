using System.Collections.Generic;
using UnityEngine;
using ExtendForCreateScript;
using UnityEditor;
using UnityEngine.UI;
namespace MultipleLanguage
{
    public enum Language
    {
        Chinese,
        English,
    }
    public class MultiLanGetterInEditor
    {
        public static List<string> DontAnalysis = new List<string>()
        {
            "msginprefab_预制体中提示.xlsx",
            "gamemsg_游戏中提示.xlsx",
            "msgsinconfig_配置中提示.xlsx",
        };

        [MenuItem("程序工具/多语言工具/提取预制体中中文")]
        public static void GetChineseFromPrefab()
        {
            Dictionary<string, bool> PathsRepeat = new Dictionary<string, bool>();

            Dictionary<string, ChineseData> UITextToPaths = new Dictionary<string, ChineseData>();
            List<string> pathsOfPrefabs = new List<string>();
            AssetPath.GetAllFileName(Application.dataPath + "/Res/Prefab/UIPrefab", (path) =>
            {
                pathsOfPrefabs.Add(path);
            });

            foreach (string path in pathsOfPrefabs)
            {
                var loadpath = "Assets" + path.Replace("assets", "#").Split('#')[1].Replace(@"\", "/");
                var load = AssetDatabase.LoadAssetAtPath<GameObject>(loadpath);
                if (load != null)
                {
                    Text[] texts = load.GetComponentsInChildren<Text>();
                    foreach (var text in texts)
                    {

                        if (text.text.ContainChinese())
                        {
                            ChineseData tmp = new ChineseData();
                            if (!UITextToPaths.TryGetValue(text.text, out tmp))
                            {
                                UITextToPaths[text.text] = new ChineseData();
                                UITextToPaths[text.text].time = System.DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "");
                                UITextToPaths[text.text].inScript = text.text;
                                UITextToPaths[text.text].traditionalType = text.text.ToTraditional();
                            }
                            var fathername = GetRootFather(text.transform);
                            if (fathername != "")
                            {
                                UITextToPaths[text.text].linkTo.Add(fathername);
                            }
                            else
                            {
                                Debug.LogError($"找不到 {text.transform} 的根父物体");
                            }
                        }
                    }

                }
            }

            //string sheetName = "msgsinprefab";

            //ExcelTool.Write(Config.GameMsgPath, sheetName, (st) =>
            //{
            //    int line = 11;
            //    foreach (var key in UITextToPaths.Keys)
            //    {
            //        var vitem = UITextToPaths[key];
            //        st.Cells[line, 1].Value = line - 10;
            //        st.Cells[line, 2].Value = vitem.inScript;
            //        st.Cells[line, 3].Value = vitem.time;
            //        st.Cells[line, 4].Value = vitem.modify;
            //        st.Cells[line, 5].Value = vitem.linkTo.LstToStrBySpilt("†");
            //        st.Cells[line, 6].Value = vitem.traditionalType;
            //        Debug.LogWarning($"Write {vitem.inScript}, {vitem.modify}, {vitem.time}, {st.Cells[line, 5].Value}");
            //        line += 1;
            //    }
            //}
            //);
            Debug.Log("提取结束");
        }

        private static string GetAbsolutePathToFather(Transform sub)
        {
            if (sub.tag == "UIRoot")
            {
                return sub.name;
            }
            string res = sub.name;

            while (sub.parent != null)
            {
                sub = sub.parent;
                res = res.pre(sub.name + "/");
            }
            return res;
        }
        private static string GetRootFather(Transform sub)
        {
            if (sub.tag == "UIRoot")
            {
                return sub.name;
            }

            while (sub.parent != null)
            {
                sub = sub.parent;
            }
            return sub.name;
        }

        private static string now()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "");
        }
    }
}
