using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, object> subscribers = new();

    public static void Subscribe<T>(Action<T> handler)
    {
        Type type = typeof(T);
        if (!subscribers.TryGetValue(type, out var list))
        {
            list = new List<Action<T>>();
            subscribers[type] = list;
        }
        (list as List<Action<T>>)?.Add(handler);
    }

    public static void Unsubscribe<T>(Action<T> handler)
    {
        Type type = typeof(T);
        if (subscribers.TryGetValue(type, out var list) && list is List<Action<T>> typedList)
        {
            typedList.Remove(handler);
            if (typedList.Count == 0)
                subscribers.Remove(type);
        }
    }

    public static void Publish<T>(T eventData)
    {
        Type type = typeof(T);
        if (subscribers.TryGetValue(type, out var list) && list is List<Action<T>> typedList)
        {
            var handlers = typedList.ToArray();
            foreach (var handler in handlers)
                handler?.Invoke(eventData);
        }
    }
    public static void Clear()
    {
        subscribers.Clear();
    }
}