using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ConfigProvider : MonoBehaviour
{
    private const string CONFIGS_FOLDER = "Configs";
    private const string CONFIGS_NAMESPACE = "Configs";
    private const string CONFIGS_POSTFIX = "Config";

    private FileSystemWatcher watcher;
    private FileSystemEventHandler onChangedHandler;
    private readonly Dictionary<Type, Array> configs = new();

    public event Action ConfigUpdated;

    private void Awake()
    {
        ServiceLocator.Register<ConfigProvider>(this);
        LoadAllConfigs();
        SetupWatcher();
    }

    private void SetupWatcher()
    {
        string configsPath = Path.Combine(Application.streamingAssetsPath, CONFIGS_FOLDER);
        if (!Directory.Exists(configsPath))
            Directory.CreateDirectory(configsPath);

        watcher = new FileSystemWatcher(configsPath, "*.xlsx")
        {
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
        };

        onChangedHandler = (_, args) =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Debug.Log($"Config change detected: {args.Name}");
                LoadAllConfigs();
                ConfigUpdated?.Invoke();
            });
        };
    }

    private void OnEnable()
    {
        if (watcher != null)
        {
            watcher.Changed += onChangedHandler;
            watcher.EnableRaisingEvents = true;
        }
    }

    private void OnDisable()
    {
        if (watcher != null)
        {
            watcher.Changed -= onChangedHandler;
            watcher.EnableRaisingEvents = false;
        }
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

        string configsPath = Path.Combine(Application.streamingAssetsPath, CONFIGS_FOLDER);
        if (!Directory.Exists(configsPath))
        {
            Debug.LogWarning($"Configs folder not found: {configsPath}");
            return;
        }

        string[] excelFiles = Directory.GetFiles(configsPath, "*.xlsx", SearchOption.AllDirectories);
        foreach (string filePath in excelFiles)
        {
            string tableName = Path.GetFileNameWithoutExtension(filePath);
            LoadTableConfigs(filePath, tableName);
        }
    }

    private void LoadTableConfigs(string filePath, string tableName)
    {
        tableName = tableName.Replace(" ", "");
        List<string> sheetNames;
        try
        {
            sheetNames = MiniExcel.GetSheetNames(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read sheets from {tableName}.xlsx: {e.Message}");
            return;
        }

        foreach (string sheetName in sheetNames)
        {
            string className = $"{CONFIGS_NAMESPACE}.{tableName}{sheetName}{CONFIGS_POSTFIX}";
            Type type = Type.GetType(className);

            if (type == null)
            {
                Debug.LogWarning($"No config class '{className}' for {tableName}.xlsx / {sheetName}");
                continue;
            }

            LoadSheetAsArray(filePath, sheetName, type);
        }
    }

    private void LoadSheetAsArray(string filePath, string sheetName, Type type)
    {
        var rows = MiniExcel.Query(filePath, sheetName: sheetName, useHeaderRow: true)
            .Cast<IDictionary<string, object>>()
            .ToArray();

        if (rows.Length == 0) return;

        var columns = rows[0].Keys.Select(k => k.ToLower().Replace("_", "")).ToHashSet();
        ConstructorInfo ctor = type.GetConstructors()
            .FirstOrDefault(c =>
            {
                var pNames = c.GetParameters().Select(p => p.Name.ToLower());
                return pNames.All(n => columns.Contains(n));
            });

        if (ctor == null)
        {
            Debug.LogError($"No matching constructor for {type.Name}. " +
                           $"Columns: [{string.Join(", ", columns)}]");
            return;
        }

        ParameterInfo[] parameters = ctor.GetParameters();

        Array array = Array.CreateInstance(type, rows.Length);

        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];
            object[] args = new object[parameters.Length];

            for (int j = 0; j < parameters.Length; j++)
            {
                string paramName = parameters[j].Name;
                var kv = row.FirstOrDefault(k =>
                    string.Equals(k.Key.Replace("_", ""), paramName, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(kv.Key))
                {
                    args[j] = parameters[j].DefaultValue;
                }
                else
                {
                    args[j] = ConvertValue(kv.Value, parameters[j].ParameterType);
                }
            }

            object instance = ctor.Invoke(args);
            array.SetValue(instance, i);
        }

        configs[type] = array;
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