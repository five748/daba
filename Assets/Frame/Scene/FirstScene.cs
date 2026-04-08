using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MonoTool.Instance.Wait(0.5f, () => {
            SceneManager.LoadScene("GameBegin");
        });
    }
}
