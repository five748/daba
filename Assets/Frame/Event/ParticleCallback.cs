using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCallback : MonoBehaviour
{
    public System.Action particleCallback;

    public ParticleSystem ps;
    public void Start()
    {
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    void OnParticleSystemStopped()
    {
        Debug.LogError("触发了");
        if (particleCallback != null)
        {
            particleCallback();
        }
    }
    public void OnDestroy()
    {
        particleCallback = null;
    }
}
