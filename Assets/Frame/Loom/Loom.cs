using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

public class Loom : SingleMono2<Loom>
{
    public static int maxThreads = 8;
    static int numThreads;
    private int _count; 
    private List<Action> _actions = new List<Action>();
    public void Init() {
            
    }
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    public void QueueOnMainThread(Action action) {
        QueueOnMainThread(action, 0f);
    }
    public void QueueOnMainThread(Action action, float time) {
        if (time != 0) {
            lock (_delayed) {
               _delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
            }
        } else {
            lock (_actions) {
                _actions.Add(action);
            }
        }
    }
    public static Thread RunAsync(Action a) {
        while (numThreads >= maxThreads) {
            Thread.Sleep(1);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    }

    private static void RunAction(object action) {
        try
        {
            ((Action)action)();
        }
        catch
        {
        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }
    }
    List<Action> _currentActions = new List<Action>();

    // Update is called once per frame
    public void Update() {
        lock (_actions) {
            _currentActions.Clear();
            _currentActions.AddRange(_actions);
            _actions.Clear();
        }
        foreach (var a in _currentActions) {
            a();
        }
        lock (_delayed) {
            _currentDelayed.Clear();
            _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
            foreach (var item in _currentDelayed)
                _delayed.Remove(item);
        }
        foreach (var delayed in _currentDelayed) {
            delayed.action();
        }
    }
}