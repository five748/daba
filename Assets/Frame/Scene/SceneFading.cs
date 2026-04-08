using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFading :MonoBehaviour
{
    public static SceneFading Instance;
    void Awake()
    {
        Instance = this;
    }
    public bool isBlack = false;
    public void AddFadeInt(System.Action callback)
    {
        if (isBlack) {
            callback();
            return;
        }
        if (transform == null) {
            return;
        }
        isBlack = true;
        transform.ChangeAlpha(true, 0.02f, null, callback);
    }
    public void BlackReset(){
        isBlack = false;
    }
    public void AddFadeOut(System.Action callback = null, float speed = 0.02f, bool resetB = false)
    {
        if (resetB) isBlack = true;
        if (!isBlack) {
            if(callback != null)
                callback();
            return;
        }
        isBlack = false;
        transform.ChangeAlpha(false, speed, null, () => {
            if (callback != null)
                callback();
        });
    }
}
