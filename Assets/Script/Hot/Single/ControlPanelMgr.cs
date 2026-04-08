

using System;
using System.Collections.Generic;
using System.Linq;
using TreeData;
using UnityEngine;
using Random = UnityEngine.Random;

public class ControlPanelMgr : Singleton<ControlPanelMgr>
{
    public DataControlPanel Data { get; }

    private ControlPanelMgr()
    {
        Data = DataControlPanel.ReadFromFile();
    }

    public void SetGuideShow(bool value)
    {
        Data.ShowGuide = value;
        UIGuide.ShowGuide = value;
        UIManager.Root.Find("UIGuide").SetActive(value);
        Data.SaveToFile();
        var str = value ? "开启" : "关闭";
        Debug.Log($"引导 {str}");
    }
}