using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameObjectAcitve : MonoBehaviour
{
    public void OnEnable()
    {
        Debug.LogError(transform.name + ":true");
    }
    public void OnDisable()
    {
        Debug.LogError(transform.name + ":false");
    }
    public void OnDestroy()
    {
        Debug.LogError(transform.name + ":Destroy");
    }

}
