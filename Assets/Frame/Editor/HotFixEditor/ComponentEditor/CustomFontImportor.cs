using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

public class CustomFontImportor : EditorWindow
{
    [MenuItem("Tools/创建字体(Fnt)")]
    static void DoIt()
    {
        GetWindow<CustomFontImportor>("创建字体");
    }
    private string fontName;
    private string fontPath;
    private Texture2D tex;
    private string fntFilePath;

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("字体名称：");
        fontName = EditorGUILayout.TextField(fontName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("字体图片：");
        tex = (Texture2D)EditorGUILayout.ObjectField(tex, typeof(Texture2D), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(string.IsNullOrEmpty(fontPath) ? "选择路径" : fontPath))
        {
            fontPath = EditorUtility.OpenFolderPanel("字体路径", Application.dataPath, "");
            if (string.IsNullOrEmpty(fontPath))
            {
                Debug.Log("取消选择路径");
            }
            else
            {
                fontPath = fontPath.Replace(Application.dataPath, "") + "/";
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(string.IsNullOrEmpty(fntFilePath) ? "选择fnt文件" : fntFilePath))
        {
            fntFilePath = EditorUtility.OpenFilePanelWithFilters("选择fnt文件", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), new string[] { "", "fnt" });
            if (string.IsNullOrEmpty(fntFilePath))
            {
                Debug.Log("取消选择路径");
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("创建"))
        {
            Create();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    private void Create()
    {
        if (string.IsNullOrEmpty(fntFilePath))
        {
            Debug.LogError("fnt为空");
            return;
        }
        if (tex == null)
        {
            Debug.LogError("字体图片为空");
            return;
        }

        string fontSettingPath = fontPath + fontName + ".fontsettings";
        string matPath = fontPath + fontName + ".mat";
        if (File.Exists(Application.dataPath + fontSettingPath))
        {
            Debug.LogErrorFormat("已存在同名字体文件:{0}", fontSettingPath);
            return;
        }
        if (File.Exists(Application.dataPath + matPath))
        {
            Debug.LogErrorFormat("已存在同名字体材质:{0}", matPath);
            return;
        }
        var list = new List<CharacterInfo>();
        XmlDocument xmlDoc = new XmlDocument();
        var content = File.ReadAllText(fntFilePath, System.Text.Encoding.UTF8);
        xmlDoc.LoadXml(content);
        var nodelist = xmlDoc.SelectNodes("font/chars/char");
        foreach (XmlElement item in nodelist)
        {
            CharacterInfo info = new CharacterInfo();
            var id = int.Parse(item.GetAttribute("id"));
            var x = float.Parse(item.GetAttribute("x"));
            var y = float.Parse(item.GetAttribute("y"));
            var width = float.Parse(item.GetAttribute("width"));
            var height = float.Parse(item.GetAttribute("height"));

            info.index = id;
            //纹理映射，上下翻转
            info.uvBottomLeft = new Vector2(x / tex.width, 1 - (y + height) / tex.height);
            info.uvBottomRight = new Vector2((x + width) / tex.width, 1 - (y + height) / tex.height);
            info.uvTopLeft = new Vector2(x / tex.width, 1 - y / tex.height);
            info.uvTopRight = new Vector2((x + width) / tex.width, 1 - y / tex.height);

            info.minX = 0;
            info.maxX = (int)width;
            info.minY = -(int)height / 2;
            info.maxY = (int)height / 2;
            info.advance = (int)width;

            list.Add(info);
        }

        Material mat = new Material(Shader.Find("GUI/Text Shader"));
        mat.SetTexture("_MainTex", tex);
        Font m_myFont = new Font();
        m_myFont.material = mat;
        AssetDatabase.CreateAsset(mat, "Assets" + matPath);
        AssetDatabase.CreateAsset(m_myFont, "Assets" + fontSettingPath);
        m_myFont.characterInfo = list.ToArray();
        EditorUtility.SetDirty(m_myFont);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("创建成功！");
    }
    //
    [MenuItem("Assets/CreateBMFont")]
    static void CreateFont() {
        Object obj = Selection.activeObject;
        string fntPath = AssetDatabase.GetAssetPath(obj);
        if(fntPath.IndexOf(".fnt") == -1)
        {
            // 不是字体文件
            return;
        }

        string customFontPath = fntPath.Replace(".fnt", ".fontsettings");
        if(!File.Exists(customFontPath))
        {
            return;
        }

        Debug.Log(fntPath);
        StreamReader reader = new StreamReader(new FileStream(fntPath, FileMode.Open));

        List<CharacterInfo> charList = new List<CharacterInfo>();

        Regex reg = new Regex(@"char id=(?<id>\d+)\s+x=(?<x>\d+)\s+y=(?<y>\d+)\s+width=(?<width>\d+)\s+height=(?<height>\d+)\s+xoffset=(?<xoffset>\d+)\s+yoffset=(?<yoffset>\d+)\s+xadvance=(?<xadvance>\d+)\s+");
        string line = reader.ReadLine();
        int lineHeight = 0;
        int texWidth = 1;
        int texHeight = 1;

        while(line != null)
        {
            if(line.IndexOf("char id=") != -1)
            {
                Match match = reg.Match(line);
                if(match != Match.Empty)
                {
                    var id = System.Convert.ToInt32(match.Groups["id"].Value);
                    var x = System.Convert.ToInt32(match.Groups["x"].Value);
                    var y = System.Convert.ToInt32(match.Groups["y"].Value);
                    var width = System.Convert.ToInt32(match.Groups["width"].Value);
                    var height = System.Convert.ToInt32(match.Groups["height"].Value);
                    var xoffset = System.Convert.ToInt32(match.Groups["xoffset"].Value);
                    var yoffset = System.Convert.ToInt32(match.Groups["yoffset"].Value);
                    var xadvance = System.Convert.ToInt32(match.Groups["xadvance"].Value);
                  
                    CharacterInfo info = new CharacterInfo();
                    info.index = id;
                    float uvx = 1f * x / texWidth;
                    float uvy = 1 - (1f * y / texHeight);
                    float uvw = 1f * width / texWidth;
                    float uvh = -1f * height / texHeight;

                    info.uvBottomLeft = new Vector2(uvx, uvy);
                    info.uvBottomRight = new Vector2(uvx + uvw, uvy);
                    info.uvTopLeft = new Vector2(uvx, uvy + uvh);
                    info.uvTopRight = new Vector2(uvx + uvw, uvy + uvh);

                    info.minX = xoffset;
                    info.minY = -yoffset;   // 这样调出来的效果是ok的，原理未知
                    info.glyphWidth = width;
                    info.glyphHeight = -height; // 同上，不知道为什么要用负的，可能跟unity纹理uv有关
                    info.advance = xadvance;
                    charList.Add(info);
                }
            }
            else if(line.IndexOf("scaleW=") != -1)
            {

                Regex reg2 = new Regex(@"common lineHeight=(?<lineHeight>\d+)\s+.*scaleW=(?<scaleW>\d+)\s+scaleH=(?<scaleH>\d+)");
                Match match = reg2.Match(line);
                if(match != Match.Empty)
                {
                    lineHeight = System.Convert.ToInt32(match.Groups["lineHeight"].Value);
                    texWidth = System.Convert.ToInt32(match.Groups["scaleW"].Value);
                    texHeight = System.Convert.ToInt32(match.Groups["scaleH"].Value);
                }
            }
            line = reader.ReadLine();
        }

        Font customFont = AssetDatabase.LoadAssetAtPath<Font>(customFontPath);
        customFont.characterInfo = charList.ToArray();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log(customFont);
    }
}
