using System;
using System.Collections;
using UnityEngine;

///用于协程。  用来在协程中等待值变化
public class YieldResult<T> {

    public T Value = default(T);
    public bool IsDone = false;

    public void Done(T value){
        Value = value;
        IsDone = true;
    }
    
    public void Done(){
        IsDone = true;
    }
    
    public IEnumerator Wait(Action FrameCall){
        while (!IsDone){
            FrameCall?.Invoke();
            yield return new WaitForSeconds(0.1f);
        }
    }
}