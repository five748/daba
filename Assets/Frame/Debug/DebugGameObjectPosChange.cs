using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameObjectPosChange : MonoBehaviour
{
    public Vector3 oldPos;
    public Vector3 oldRota;
    public void Awake()
    {
        oldPos = transform.position;
        oldRota = transform.eulerAngles;
    }
    private void Update()
    {
        //if (oldPos != transform.position) {
        //    oldPos = transform.position;
        //    Debug.LogError(oldPos);
        //}
        //if (oldRota != transform.eulerAngles)
        //{
        //    oldRota = transform.eulerAngles;
        //    Debug.LogError(oldRota);
        //}
        if (transform.localRotation.z != 0) {
            Debug.LogError("lotaZ:" + transform.localRotation.z);
        }
    }

}
