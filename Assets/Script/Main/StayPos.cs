using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayPos : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 oldPos;
    private bool isInited;
    void Start()
    {
        oldPos = transform.position;
        isInited = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInited)
        {
            transform.position = oldPos;
        }
    }
}
