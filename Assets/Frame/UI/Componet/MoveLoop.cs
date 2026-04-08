using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoop : MonoBehaviour
{
    // Use this for initialization
    public float speed = 0.2f;
    private bool moving = true;
    public float beginDir = 0f;
    public float endDir = 5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            if (transform.localPosition.y >= endDir)
            {
                moving = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.localPosition.y <= beginDir)
            {
                moving = true;
            }
        }
    }
}