using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ClassToolData
{
    public bool isNorType = false;
    public string newTypeName = "";
    public string valueStr;
    public string oldTypeName;
    public bool isListOrMap = false;
    public string mapOneType = "";
    public string LongName = "";
    public string Name = "";
    public string Des = "";
    public string ClassTypeName;
    public string fieldTyppeName;
    public string GetNewTypeName()
    {
        if (oldTypeName == "lst")
        {
            return "[]" + this.newTypeName + "one";
        }
        else if (oldTypeName == "map")
        {
            return "map[int32]" + this.newTypeName + "one";
        }

        return this.newTypeName;
    }
}
