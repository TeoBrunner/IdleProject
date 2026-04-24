using System;
using System.Collections.Concurrent;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;

    private static MainThreadDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject dispatcherObject = new GameObject("MainThreadDispatcher");
                instance = dispatcherObject.AddComponent<MainThreadDispatcher>();
                DontDestroyOnLoad(dispatcherObject);
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private readonly ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        while (actions.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }

    public static void Enqueue(Action action)
    {
        if (action == null) return;
        var inst = Instance; 
        inst.actions.Enqueue(action);
    }
}