using UnityEngine;

public abstract class LocalizedComponent : MonoBehaviour
{
    private LocalizationProvider localizationProvider;
    protected LocalizationProvider Localization
    {
        get
        {
            if (localizationProvider == null)
                localizationProvider = ServiceLocator.Get<LocalizationProvider>();
            return localizationProvider;
        }
    }

    protected virtual void OnEnable()
    {
        if (Localization != null)
            Localization.LocalizationUpdated += RefreshLocalization;
    }

    protected virtual void OnDisable()
    {
        if (Localization != null)
            Localization.LocalizationUpdated -= RefreshLocalization;
    }

    protected abstract void RefreshLocalization();
}
