using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFollow : MonoBehaviour
{
    public float cuurrZ = 0f;
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, -(transform.parent.eulerAngles.z - cuurrZ));
    }

}