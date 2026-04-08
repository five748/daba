using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEngine.Scripting;

[Preserve]
public class BaseMonoBehaviour
{
    public bool enabled;
    public GameObject gameObject
    {
        get
        {
            return transform.gameObject;
        }
    }
    public void Destroy(Object obj)
    {
        GameObject.Destroy(obj);
    }
    public void DestroyImmediate(Object obj)
    {
        GameObject.DestroyImmediate(obj);
    }
    public GameObject Instantiate(GameObject original, Transform parent)
    {
        return GameObject.Instantiate(original, parent);
    }
    public Transform transform;
    public string param;
    public System.Action<System.Action> ConcentAgainEvent;
    public System.Action InitFinish;
    public System.Action<string> _CallBack;
    public System.Action OpenCallBack;
    public void CloseBack(string str)
    {
        if (_CallBack != null)
        {
            _CallBack(str);
        }
    }
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return MonoTool.Instance.StartCor(routine);
    }
    public void StopCoroutine(Coroutine co)
    {
        MonoTool.Instance.StopCoroutine(co);
    }
    public IEnumerator EndFrame(System.Action finish)
    {
        yield return new WaitForEndOfFrame();
        finish();
    }
    public void FuncUsedOnlyOnce(System.Action finish)
    {

    }
    public void BaseInitIL(Transform tran, string _param, System.Action _openCallback)
    {
        transform = tran;
        param = _param;
        OpenCallBack = _openCallback;
    }
    public virtual void BaseInit()
    {
    }
    public virtual void Update()
    {
    }

    public virtual void BaseOpenUIName(string TipName)
    {


    }
    public void Wait(float time, System.Action callback)
    {
        MonoTool.Instance.Wait(time, callback);
    }
    public void WaitEndFrame(System.Action callback)
    {
        MonoTool.Instance.WaitEndFrame(callback);
    }
    public virtual void Destory()
    {

    }
}
