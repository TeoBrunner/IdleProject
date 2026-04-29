using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AltarFlameHUD : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text levelText;

    private const string LEVEL_KEY = "level_short";

    private LocalizationProvider localizationProvider;

    private bool isInitialized;

    private int lastLevel;
    private float lastCurrentFlame;
    private int lastRequiredFlame;

    private void Start()
    {
        gameObject.SetActive(false);

        EventBus.Subscribe<AltarFlameChangedEvent>(OnFlameChanged);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<AltarFlameChangedEvent>(OnFlameChanged);
        UnsubscribeFromLocalization();
    }

    private void OnFlameChanged(AltarFlameChangedEvent e)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            gameObject.SetActive(true);
            SubscribeToLocalization();
        }

        lastLevel = e.Level;
        lastCurrentFlame = e.CurrentFlame;
        lastRequiredFlame = e.RequiredFlame;

        UpdateAll();
    }

    private void SubscribeToLocalization()
    {
        if (localizationProvider == null)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        if (localizationProvider != null)
            localizationProvider.LocalizationUpdated += UpdateText;
    }

    private void UnsubscribeFromLocalization()
    {
        if (localizationProvider != null)
            localizationProvider.LocalizationUpdated -= UpdateText;
    }

    private void UpdateAll()
    {
        UpdateText();
        UpdateProgressBar();
    }

    private void UpdateText()
    {
        if (localizationProvider == null)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        if (localizationProvider == null || levelText == null) return;

        levelText.text = $"{localizationProvider.GetString(LEVEL_KEY)} {lastLevel}";
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.maxValue = lastRequiredFlame;
            progressBar.value = Mathf.Min(lastCurrentFlame, lastRequiredFlame);
        }
    }
}