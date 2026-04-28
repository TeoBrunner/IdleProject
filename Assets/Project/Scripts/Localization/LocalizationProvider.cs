using Localization;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEditor.Hardware.DevDeviceList;

public class LocalizationProvider : MonoBehaviour
{
    public static LocalizationProvider Instance { get; private set; }

    private const string LOCALIZATION_FOLDER = "Localization";

    private Dictionary<string, LocalizationValue> localization = new();
    private string currentLanguage = "EN";

    private FileSystemWatcher watcher;
    private FileSystemEventHandler onChangedHandler;

    public event Action LocalizationUpdated;

    public string CurrentLanguage => currentLanguage;

    void Awake()
    {
        ServiceLocator.Register<LocalizationProvider>(this);

        LoadAllLocalizations();
        SetupWatcher();
    }
    private void SetupWatcher()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, LOCALIZATION_FOLDER);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        watcher = new FileSystemWatcher(folderPath, "*.xlsx")
        {
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
        };

        onChangedHandler = (_, args) =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Debug.Log($"Localization file changed: {args.Name}, reloading...");
                LoadAllLocalizations();
                LocalizationUpdated?.Invoke();
            });
        };
    }

    private void OnEnable()
    {
        if (watcher != null)
        {
            watcher.Changed += onChangedHandler;
            watcher.Created += onChangedHandler;
            watcher.Deleted += onChangedHandler;
            watcher.EnableRaisingEvents = true;
        }
    }

    private void OnDisable()
    {
        if (watcher != null)
        {
            watcher.Changed -= onChangedHandler;
            watcher.Created -= onChangedHandler;
            watcher.Deleted -= onChangedHandler;
            watcher.EnableRaisingEvents = false;
        }
    }

    public LocalizationValue Get(string key)
    {
        if (localization.TryGetValue(key, out var val))
            return val;
        Debug.LogWarning($"Localization key '{key}' not found");
        return null;
    }

    public string GetString(string key)
    {
        return Get(key)?.Get(currentLanguage) ?? key;
    }

    public void SetLanguage(string language)
    {
        if (currentLanguage == language) return;
        currentLanguage = language;
        Debug.Log($"Language switched to {language}");
        LocalizationUpdated?.Invoke();
    }

    private void OnLocalizationFileChanged(object sender, FileSystemEventArgs e)
    {
        MainThreadDispatcher.Enqueue(() =>
        {
            Debug.Log($"Localization file changed: {e.Name}, reloading...");
            LoadAllLocalizations();
            LocalizationUpdated?.Invoke();
        });
    }

    private void LoadAllLocalizations()
    {
        localization.Clear();

        string folderPath = Path.Combine(Application.streamingAssetsPath, LOCALIZATION_FOLDER);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning($"Localization folder not found: {folderPath}");
            return;
        }

        string[] excelFiles = Directory.GetFiles(folderPath, "*.xlsx", SearchOption.AllDirectories);
        foreach (string filePath in excelFiles)
        {
            LoadFromExcel(filePath);
        }
    }

    private void LoadFromExcel(string filePath)
    {
        List<string> sheetNames;
        try
        {
            sheetNames = MiniExcel.GetSheetNames(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read sheets from {filePath}: {e.Message}");
            return;
        }

        foreach (string sheetName in sheetNames)
        {
            var rows = MiniExcel.Query(filePath, sheetName: sheetName, useHeaderRow: true)
                .Cast<IDictionary<string, object>>()
                .ToArray();

            if (rows.Length == 0) continue;

            var headers = rows[0].Keys.ToArray();
            string[] languages = headers.Skip(1).ToArray();

            foreach (var row in rows)
            {
                if (!row.TryGetValue("key", out var keyObj) || keyObj == null)
                    continue;

                string key = keyObj.ToString();

                var values = new Dictionary<string, string>();
                foreach (string lang in languages)
                {
                    if (row.TryGetValue(lang, out var val) && val != null)
                        values[lang] = val.ToString();
                }

                localization[key] = new LocalizationValue(values);
            }
        }
    }

    private void OnDestroy()
    {
        if (watcher != null)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }
    }
}