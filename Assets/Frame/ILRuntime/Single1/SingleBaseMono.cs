using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMono2<T>: MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    public static T Instance
    {
        get
        {
            if(!ProssData.Instance.SingleGo) {
                ProssData.Instance.SingleGo = GameObject.Find("Single");
            }
            if(!ProssData.Instance.SingleGo) {
                ProssData.Instance.SingleGo = new GameObject("Single");
            }
            if(_instance == null)
            {
                _instance = ProssData.Instance.SingleGo.transform.GetOrAddComponent<T>();
            }
         
            return _instance;
        }
    }
    public void Clear() {
        GameObject.DestroyImmediate(_instance);
    }
}