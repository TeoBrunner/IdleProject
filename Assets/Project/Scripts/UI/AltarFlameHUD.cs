using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AltarFlameHUD : LocalizedComponent
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text levelText;

    private bool isInitialized;

    private int lastLevel;
    private float lastCurrentFlame;
    private int lastRequiredFlame;

    private const string LEVEL_KEY = "level_short";

    private string levelLabel;

    private void Start()
    {
        gameObject.SetActive(false);
        EventBus.Subscribe<AltarFlameChangedEvent>(OnFlameChanged);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<AltarFlameChangedEvent>(OnFlameChanged);
        UnsubscribeFromLocalization();
        EventBus.Unsubscribe<AltarFlameChangedEvent>(OnFlameChanged);
    }

    protected override void RefreshLocalization()
    {
        levelLabel = Localization.GetString(LEVEL_KEY);
        UpdateText();
    }

    private void OnFlameChanged(AltarFlameChangedEvent e)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            gameObject.SetActive(true);
            RefreshLocalization();
            SubscribeToLocalization();
        }

        lastLevel = e.Level;
        lastCurrentFlame = e.CurrentFlame;
        lastRequiredFlame = e.RequiredFlame;

        UpdateAll();
    }

    private void SubscribeToLocalization()
    {
        if (Localization != null)
            Localization.LocalizationUpdated += RefreshLocalization;
    }

    private void UnsubscribeFromLocalization()
    {
        if (Localization != null)
            Localization.LocalizationUpdated -= RefreshLocalization;
    }

    private void UpdateAll()
    {
        UpdateText();
        UpdateProgressBar();
    }

    private void UpdateText()
    {
        if (string.IsNullOrEmpty(levelLabel) || levelText == null) return;
        levelText.text = $"{levelLabel} {lastLevel}";
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