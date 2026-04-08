using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ClickClearData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventTriggerListener.Get(transform).onClick = (btn) =>
        {
            Debug.LogError("清理数据");
#if DY
            StarkSDKSpace.StarkSDK.API.PlayerPrefs.DeleteAll();
#endif
        };      
    }
}
