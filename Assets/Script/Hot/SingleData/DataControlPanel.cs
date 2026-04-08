

using System;
using System.Collections.Generic;
using TreeData;

[Serializable]
public class DataControlPanel:ReadAndSaveTool<DataControlPanel>
{
    private static string _savePath = "control_panel";

    public bool ShowGuide = true;

    public static DataControlPanel ReadFromFile()
    {
        return ReadByPhone(_savePath, () =>
            { 
                var data = new DataControlPanel();
                return data;
            }
        );
    }
    public void SaveToFile()
    {
        WriteInPhone(this, _savePath);
    }
}
