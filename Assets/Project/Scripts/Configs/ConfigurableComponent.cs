using UnityEngine;

public abstract class ConfigurableComponent : MonoBehaviour
{
    private ConfigProvider configProvider;

    protected ConfigProvider Configs
    {
        get
        {
            if (configProvider == null)
                configProvider = ServiceLocator.Get<ConfigProvider>();
            return configProvider;
        }
    }

    protected virtual void OnEnable()
    {
        if (Configs != null)
            Configs.ConfigUpdated += OnConfigUpdated;
    }

    protected virtual void OnDisable()
    {
        if (Configs != null)
            Configs.ConfigUpdated -= OnConfigUpdated;
    }

    protected virtual void OnConfigUpdated()
    {
        LoadConfigs();
    }

    protected abstract void LoadConfigs();
}