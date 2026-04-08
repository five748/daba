using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTool : MonoBehaviour
{
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    void Update()
    {
        transform.Rotate(new Vector3(x, y, z), Space.Self);
    }

}