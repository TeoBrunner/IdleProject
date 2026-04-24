using Configs;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ConfigProvider : MonoBehaviour
{
    private const string CONFIG_TABLE_NAME = "Config.xlsx";
    private const string CONFIGS_NAMESPACE = "Configs";

    private FileSystemWatcher watcher;
    private FileSystemEventHandler onChangedHandler;
    private readonly Dictionary<Type, Array> configs = new();

    public event Action ConfigUpdated;
    private void Awake()
    {
        ServiceLocator.Register<ConfigProvider>(this);
        LoadAllConfigs();
        watcher = new FileSystemWatcher(Application.streamingAssetsPath, CONFIG_TABLE_NAME);
        onChangedHandler = (obj, args) =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                LoadAllConfigs();
                ConfigUpdated?.Invoke();
            });
        };

    }
    private void OnEnable()
    {
        watcher.Changed += onChangedHandler;
        watcher.EnableRaisingEvents = true;
    }
    private void OnDisable()
    {
        watcher.Changed -= onChangedHandler;
        watcher.EnableRaisingEvents = false;
    }
    public T[] GetConfigs<T>() where T : class
    {
        if (configs.TryGetValue(typeof(T), out var configArray))
            return configArray as T[];
        Debug.LogError($"Config of type {typeof(T).Name} not found");
        return null;
    }
    private void LoadAllConfigs()
    {
        configs.Clear();

        string path = Path.Combine(Application.streamingAssetsPath, CONFIG_TABLE_NAME);
        List<string> sheetNames = MiniExcel.GetSheetNames(path);

        foreach (string sheetName in sheetNames)
        {
            string className = $"{CONFIGS_NAMESPACE}.{sheetName}Config";
            Type type = Type.GetType(className);

            if (type == null || !typeof(BaseConfig).IsAssignableFrom(type))
            {
                Debug.LogWarning($"No suitable config class found for sheet '{sheetName}' (expected {className})");
                continue;
            }

            var rows = MiniExcel.Query(path, sheetName: sheetName, useHeaderRow: true).Cast<IDictionary<string, object>>().ToArray();
            if (rows.Length == 0)
                continue;

            var columns = rows[0].Keys.Select(k => k.ToLower()).ToHashSet();
            ConstructorInfo ctor = type.GetConstructors()    
                .FirstOrDefault(c =>    
                {        
                    var pNames = c.GetParameters().Select(p => p.Name.ToLower());        
                    return pNames.All(n => columns.Contains(n));    
                });
            if (ctor == null)
            {
                Debug.LogError($"No matching constructor for {type.Name} with columns: {string.Join(", ", columns)}");
                continue;
            }

            ParameterInfo[] parameters = ctor.GetParameters();

            int maxLevel = rows.Max(r => Convert.ToInt32(r["Level"]));
            Array array = Array.CreateInstance(type, maxLevel);

            foreach (var row in rows)
            {
                int level = Convert.ToInt32(row["Level"]);
                object[] args = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    string paramName = parameters[i].Name;
                    var kv = row.FirstOrDefault(k => string.Equals(k.Key, paramName, StringComparison.OrdinalIgnoreCase));
                    if (string.IsNullOrEmpty(kv.Key))
                    {
                        args[i] = parameters[i].DefaultValue;
                        Debug.LogWarning($"Column '{paramName}' not found for {type.Name}");
                    }
                    else
                    {
                        args[i] = ConvertValue(kv.Value, parameters[i].ParameterType);
                    }
                }

                object instance = ctor.Invoke(args);
                array.SetValue(instance, level - 1);
            }

            configs[type] = array;

        }
    }
    private object ConvertValue(object value, Type targetType)
    {
        if (value == null) return null;
        Type actualType = value.GetType();
        if (targetType.IsAssignableFrom(actualType)) return value;

        if (targetType == typeof(float) && value is double d) return (float)d;
        if (targetType == typeof(int) && value is double d2) return (int)d2;
        if (targetType.IsEnum && value is string s) return Enum.Parse(targetType, s);
        return Convert.ChangeType(value, targetType);
    }
}
