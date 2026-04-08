using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NewImage:Image{
    protected override void Start() {
        base.Start();
    }
    public bool isGrey = false;
    private bool isSetOldColor;
    public Color _oldColor;
    public Color oldColor
    {

        get
        {
            return _oldColor;
        }
        set
        {
            _oldColor = value;
        }
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
    public int key = -1;
    public bool isAutoSize = false;
    public bool isInArray = false;
    public int keyIndex = -1;
    public void SetImage(string path) {
        AssetLoadOld.Instance.LoadImage(transform, path, (go) => {
            if(go != null)
            {
                sprite = go as Sprite;
                if(isAutoSize)
                {
                    SetNativeSize();
                }
            }
        });
    }
    protected override void OnDestroy() {
        base.OnDestroy();
    }
}
