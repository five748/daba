using UnityEngine;
using UnityEngine.UI;

public class NewText : Text
{
    public bool isGrey = false;
    private bool isSetOldColor;
    public Color _oldColor;
    public Color oldColor
    {

        get
        {
            return _oldColor;
        }
        //set
        //{
        //    Debug.LogError(value);
        //    _oldColor = value;
        //}
    }
    public Color Color
    {
        get
        {
            if (!isSetOldColor)
            {
                _oldColor = color;
                isSetOldColor = true;
            }
            return color;
        }
        set
        {
            if (!isSetOldColor)
            {
                _oldColor = color;
                isSetOldColor = true;
            }
            color = value;
        }
    }
    public string menuName = "样式";
    //protected new void Awake()
    //{
    //    _oldColor = color;
    //}
    //protected new void Reset()
    //{
    //    _oldColor = color;
    //}
    public int key = -1;
    public bool isInArray = false;
    public int keyIndex = -1;
    public void SetText(string str)
    {
        text = str;
    }
    public void SetText(string str, int index)
    {
        UnityEngine.Debug.Log(str + ":" + index);
        if (keyIndex != index)
        {
            return;
        }
        text = str;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        //if(key == -1)
        //    return;
        //if(isInArray)
        //{
        //    return;
        //}
        //else
        //{
        //    AllValueData.Instance.StringEvents.Remove(key);
        //}
    }
}