using System;
using System.Collections.Generic;
using UnityEngine;
public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new();

    public static void Register<T>(T service)
    {
        var type = typeof(T);

        if (services.ContainsKey(type))
            Debug.LogWarning($"ServiceLocator: {type.Name} is already registered. Rewriting.");

        services[type] = service;
    }
    public static T Get<T>()
    {
        if (services.TryGetValue(typeof(T), out var service))
            return (T)service;

        Debug.LogError($"ServiceLocator: {typeof(T).Name} is not registered.");
        return default;
    }
    public static bool Has<T>() => services.ContainsKey(typeof(T));
    public static void Clear() => services.Clear();
}

