using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUILoadRes : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var go = Resources.Load<GameObject>("UILoadRes");
        var loadRes = GameObject.Instantiate(go);
        loadRes.name = "UILoadRes";
        loadRes.transform.SetParentOverride(this.transform);
        //Debug.LogError("");
    }
}
